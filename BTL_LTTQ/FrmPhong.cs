using BLL;
using GUI;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmPhong : Form
    {
        private PhongBLL bll = new PhongBLL();
        private LoaiPhongBLL loaiPhongBLL = new LoaiPhongBLL();
        private int? selectedMaPhong = null;
        private string quyen;

        public FrmPhong(string quyen)
        {
            InitializeComponent();
            this.quyen = quyen ?? "nhanvien";
        }

        private void FrmPhong_Load(object sender, EventArgs e)
        {
            string role = (quyen ?? "").Trim().ToLower();

            btnThem.Enabled = (role == "admin");
            btnXoa.Enabled = (role == "admin");
            btnMoLoaiPhong.Enabled = (role == "admin");
            btnSua.Enabled = true;

            LoadLoaiPhongToCombo();
            LoadPhongToGrid();

            if (role == "nhanvien")
            {
                txtMaPhong.Enabled = false;
                txtTenPhong.Enabled = false;
                cboLoaiPhong.Enabled = false;
                cboTinhTrang.Enabled = true;
            }
            else if (role == "admin")
            {
                txtMaPhong.Enabled = true;
                txtTenPhong.Enabled = true;
                cboLoaiPhong.Enabled = true;
                cboTinhTrang.Enabled = true;
            }
        }

        private void LoadLoaiPhongToCombo()
        {
            var ds = loaiPhongBLL.LayDanhSachLoaiPhong();

            cboLoaiPhong.DataSource = ds;
            cboLoaiPhong.DisplayMember = "TenLoai";
            cboLoaiPhong.ValueMember = "MaLoaiPhong";
        }

        private void LoadPhongToGrid()
        {
            dgvPhong.DataSource = bll.LayDanhSachPhong();
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            if (cboLoaiPhong.Items.Count > 0) cboLoaiPhong.SelectedIndex = 0;
            cboTinhTrang.SelectedIndex = 0;
            selectedMaPhong = null;

            txtMaPhong.Enabled = true;
            txtMaPhong.Text = bll.LayMaPhongTiepTheo().ToString();
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvPhong.Rows[e.RowIndex];
                selectedMaPhong = Convert.ToInt32(row.Cells["MaPhong"].Value);
                txtMaPhong.Text = row.Cells["MaPhong"].Value.ToString();
                txtTenPhong.Text = row.Cells["TenPhong"].Value?.ToString();

                if (row.Cells["MaLoaiPhong"] != null && row.Cells["MaLoaiPhong"].Value != null)
                {
                    int maLoai = Convert.ToInt32(row.Cells["MaLoaiPhong"].Value);
                    cboLoaiPhong.SelectedValue = maLoai;
                }

                cboTinhTrang.Text = row.Cells["TrangThai"].Value?.ToString();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadLoaiPhongToCombo();
            LoadPhongToGrid();
        }

        private void btnMoLoaiPhong_Click(object sender, EventArgs e)
        {
            if (!quyen.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                CustomMessageBox.Show("Chỉ admin mới có quyền quản lý loại phòng.", "Không có quyền");
                return;
            }

            FrmLoaiPhong frm = new FrmLoaiPhong();

            if (this.ParentForm is FrmMain main)
            {
                main.NavigateTo(frm);
            }

            LoadLoaiPhongToCombo();
            LoadPhongToGrid();
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!quyen.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                CustomMessageBox.Show("Chỉ admin mới có thể thêm phòng.", "Không có quyền");
                return;
            }

            string ten = txtTenPhong.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập tên phòng.", "Thông báo");
                return;
            }

            if (cboLoaiPhong.SelectedValue == null)
            {
                CustomMessageBox.ShowWarning("Vui lòng chọn loại phòng.", "Thông báo");
                return;
            }

            int maLoai = Convert.ToInt32(cboLoaiPhong.SelectedValue);
            string trangThai = cboTinhTrang.Text;

            int maPhong;
            bool userTypedId = !string.IsNullOrWhiteSpace(txtMaPhong.Text);

            if (userTypedId)
            {
                if (!int.TryParse(txtMaPhong.Text, out maPhong))
                {
                    CustomMessageBox.ShowWarning("Vui lòng nhập Mã phòng là số nguyên!", "Thông báo");
                    txtMaPhong.Focus(); return;
                }
                if (bll.KiemTraMaPhongTonTai(maPhong))
                {
                    CustomMessageBox.ShowWarning("Mã phòng đã tồn tại, vui lòng nhập mã khác!", "Thông báo");
                    txtMaPhong.Focus(); return;
                }
            }
            else
            {
                maPhong = bll.LayMaPhongTiepTheo();
            }

            try
            {
                if (bll.ThemPhong(maPhong, ten, maLoai, trangThai))
                {
                    CustomMessageBox.Show("Thêm phòng thành công.", "Thông báo");
                    LoadPhongToGrid();
                }
                else
                {
                    CustomMessageBox.ShowError("Thêm phòng thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi thêm phòng: " + ex.Message, "Lỗi");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == null)
            {
                CustomMessageBox.Show("Vui lòng chọn phòng để sửa.", "Thông báo");
                return;
            }

            var p = bll.LayPhongTheoMa(selectedMaPhong.Value);
            if (p == null)
            {
                CustomMessageBox.ShowError("Không tìm thấy phòng.", "Lỗi");
                return;
            }

            string tenPhong = txtTenPhong.Text.Trim();
            int maLoaiPhong = Convert.ToInt32(cboLoaiPhong.SelectedValue);
            string trangThai = cboTinhTrang.Text;

            string ten = txtTenPhong.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập tên phòng.", "Thông báo");
                return;
            }

            if (bll.CapNhatPhong(selectedMaPhong.Value, tenPhong, maLoaiPhong, trangThai))
            {
                CustomMessageBox.Show("Cập nhật phòng thành công.", "Thông báo");
                LoadPhongToGrid();
            }
            else
            {
                CustomMessageBox.ShowError("Cập nhật thất bại.", "Lỗi");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!quyen.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                CustomMessageBox.ShowWarning("Chỉ admin mới có thể xóa phòng.", "Không có quyền");
                return;
            }

            if (selectedMaPhong == null)
            {
                CustomMessageBox.ShowWarning("Vui lòng chọn phòng để xóa.", "Thông báo");
                return;
            }

            if (CustomMessageBox.ShowConfirm("Bạn có chắc muốn xóa phòng này?", "Xác nhận") != DialogResult.Yes)
            {
                return;
            }

            var p = bll.LayPhongTheoMa(selectedMaPhong.Value);
            if (p == null)
            {
                CustomMessageBox.ShowError("Không tìm thấy phòng.", "Lỗi");
                return;
            }

            try
            {
                if (bll.XoaPhong(selectedMaPhong.Value))
                {
                    CustomMessageBox.Show("Xóa phòng thành công.", "Thông báo");
                    LoadPhongToGrid();
                }
                else
                {
                    CustomMessageBox.ShowError("Xóa thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi xóa: " + ex.Message, "Lỗi");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            if (bll.TimKiemPhong(keyword).Count == 0)
            {
                CustomMessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo");
                return;
            }
            else
            {
                CustomMessageBox.Show($"Đã tìm thấy {bll.TimKiemPhong(keyword).Count} kết quả phù hợp!", "Thông báo");
            }
            dgvPhong.DataSource = bll.TimKiemPhong(keyword);
            ClearInputs();
        }
    }
}