using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class FrmLoaiPhong : Form
    {
        private LoaiPhongBLL bll = new LoaiPhongBLL();
        private int? selectedMaLoai = null;

        private Color btnThemColor = Color.FromArgb(124, 179, 66);
        private Color btnSuaColor = Color.FromArgb(74, 144, 226);
        private Color btnXoaColor = Color.FromArgb(239, 83, 80);
        private Color btnLamMoiColor = Color.FromArgb(255, 152, 0);

        public FrmLoaiPhong()
        {
            InitializeComponent();
        }

        private void FrmLoaiPhong_Load(object sender, EventArgs e)
        {
            ApplyUIStyles();
            LoadLoaiPhong();
            CenterButtons();
        }

        private void ApplyUIStyles()
        {
            // Rounded corners cho Panel
            UIHelper.SetRoundedPanel(panelInputs, 12);

            // Rounded corners và hover effect cho Buttons
            UIHelper.SetRoundedButton(btnThem, 8);
            UIHelper.SetRoundedButton(btnSua, 8);
            UIHelper.SetRoundedButton(btnXoa, 8);
            UIHelper.SetRoundedButton(btnLamMoi, 8);

            UIHelper.SetButtonHoverEffect(btnThem, btnThemColor);
            UIHelper.SetButtonHoverEffect(btnSua, btnSuaColor);
            UIHelper.SetButtonHoverEffect(btnXoa, btnXoaColor);
            UIHelper.SetButtonHoverEffect(btnLamMoi, btnLamMoiColor);

            // Rounded corners cho TextBoxes
            UIHelper.SetRoundedTextBox(txtMaLoaiPhong, 8, 35);
            UIHelper.SetRoundedTextBox(txtTenLoaiPhong, 8, 35);
            UIHelper.SetRoundedTextBox(txtMoTa, 8, 35);
            UIHelper.SetRoundedTextBox(txtGiaCoBan, 8, 35);
        }

        public void CenterButtons()
        {
            if (panelButtons.Controls.Count == 0) return;

            var buttons = panelButtons.Controls.OfType<Button>().ToList();
            if (buttons.Count == 0) return;

            int spacing = 20;
            int totalWidth = buttons.Sum(b => b.Width) + spacing * (buttons.Count - 1);
            int startX = (panelButtons.Width - totalWidth) / 2;

            int x = startX;
            foreach (var btn in buttons)
            {
                btn.Location = new Point(x, btn.Location.Y);
                x += btn.Width + spacing;
            }
        }

        // Phương thức tự động hiển thị mã loại phòng tiếp theo (chỉ để xem)
        private void AutoGenerateMaLoaiPhong()
        {
            try
            {
                int nextMa = bll.LayMaLoaiPhongTiepTheo();
                txtMaLoaiPhong.Text = nextMa.ToString();
                txtMaLoaiPhong.ReadOnly = true; // Không cho phép sửa mã
                txtMaLoaiPhong.BackColor = Color.FromArgb(240, 240, 240); // Màu xám nhạt
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi tạo mã loại phòng: " + ex.Message, "Lỗi");
            }
        }

        private void LoadLoaiPhong()
        {
            try
            {
                dgvLoaiPhong.DataSource = bll.LayDanhSachLoaiPhong();

                if (dgvLoaiPhong.Columns.Count > 0)
                {
                    if (dgvLoaiPhong.Columns.Contains("MaLoaiPhong"))
                        dgvLoaiPhong.Columns["MaLoaiPhong"].HeaderText = "Mã Loại";
                    if (dgvLoaiPhong.Columns.Contains("TenLoai"))
                        dgvLoaiPhong.Columns["TenLoai"].HeaderText = "Tên Loại Phòng";
                    if (dgvLoaiPhong.Columns.Contains("GiaPhong"))
                    {
                        dgvLoaiPhong.Columns["GiaPhong"].HeaderText = "Giá Phòng (VNĐ)";
                        dgvLoaiPhong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0";
                        dgvLoaiPhong.Columns["GiaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvLoaiPhong.Columns.Contains("MoTa"))
                        dgvLoaiPhong.Columns["MoTa"].HeaderText = "Mô Tả";
                }

                ClearInputs();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi");
            }
        }

        private void ClearInputs()
        {
            txtTenLoaiPhong.Text = "";
            txtGiaCoBan.Text = "";
            txtMoTa.Clear();
            selectedMaLoai = null;
            AutoGenerateMaLoaiPhong(); // Tự động tạo mã mới khi clear
            txtTenLoaiPhong.Focus();
        }

        private void dgvLoaiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var row = dgvLoaiPhong.Rows[e.RowIndex];
                    selectedMaLoai = Convert.ToInt32(row.Cells["MaLoaiPhong"].Value);
                    txtMaLoaiPhong.Text = row.Cells["MaLoaiPhong"].Value.ToString();
                    txtTenLoaiPhong.Text = row.Cells["TenLoai"].Value?.ToString();
                    txtGiaCoBan.Text = row.Cells["GiaPhong"].Value?.ToString();
                    txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                }
                catch (Exception ex)
                {
                    CustomMessageBox.ShowError("Lỗi khi chọn dòng: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập tên loại phòng.", "Thông báo");
                txtTenLoaiPhong.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaCoBan.Text, out decimal gia) || gia <= 0)
            {
                CustomMessageBox.ShowWarning("Giá phòng không hợp lệ. Vui lòng nhập số dương.", "Thông báo");
                txtGiaCoBan.Focus();
                return;
            }

            try
            {

                bll.ThemLoaiPhong(txtTenLoaiPhong.Text.Trim(), gia, txtMoTa.Text.Trim());
                CustomMessageBox.Show("Thêm loại phòng thành công!", "Thành công");
                LoadLoaiPhong();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi thêm: " + ex.Message, "Lỗi");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaLoai == null)
            {
                CustomMessageBox.ShowWarning("Vui lòng chọn loại phòng cần sửa từ bảng.", "Thông báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập tên loại phòng.", "Thông báo");
                txtTenLoaiPhong.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaCoBan.Text, out decimal gia) || gia <= 0)
            {
                CustomMessageBox.ShowWarning("Giá phòng không hợp lệ. Vui lòng nhập số dương.", "Thông báo");
                txtGiaCoBan.Focus();
                return;
            }

            try
            {
                bll.CapNhatLoaiPhong(selectedMaLoai.Value, txtTenLoaiPhong.Text.Trim(),
                    gia, txtMoTa.Text.Trim());
                CustomMessageBox.Show("Cập nhật loại phòng thành công!", "Thành công");
                LoadLoaiPhong();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi cập nhật: " + ex.Message, "Lỗi");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaLoai == null)
            {
                CustomMessageBox.ShowWarning("Vui lòng chọn loại phòng cần xóa từ bảng.", "Thông báo");
                return;
            }

            if (CustomMessageBox.ShowConfirm(
                    $"Bạn có chắc chắn muốn xóa loại phòng '{txtTenLoaiPhong.Text}'?\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa") != DialogResult.Yes)
            {
                return;
            }

            try
            {
                bll.XoaLoaiPhong(selectedMaLoai.Value);
                CustomMessageBox.Show("Xóa loại phòng thành công!", "Thành công");
                LoadLoaiPhong();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi xóa: " + ex.Message, "Lỗi");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadLoaiPhong();
        }
    }
}