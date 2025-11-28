using BLL;
using DTO;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmNhanVien : Form
    {
        private NhanVienBLL nvBLL = new NhanVienBLL();
        private int? selectedMaNV = null;

        // Lưu màu gốc của các button
        private Color btnThemColor = Color.FromArgb(124, 179, 66);
        private Color btnSuaColor = Color.FromArgb(74, 144, 226);
        private Color btnXoaColor = Color.FromArgb(239, 83, 80);
        private Color btnLamMoiColor = Color.FromArgb(255, 152, 0);
        private Color btnTimKiemColor = Color.FromArgb(156, 39, 176);

        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            // Apply rounded corners và hover effects
            ApplyUIStyles();

            // Load dữ liệu
            LoadData();
            cboGioiTinh.SelectedIndex = 0;

            panelButtons.Resize += PanelButtons_Resize;
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
            UIHelper.SetRoundedButton(btnTimKiem, 8);

            UIHelper.SetButtonHoverEffect(btnThem, btnThemColor);
            UIHelper.SetButtonHoverEffect(btnSua, btnSuaColor);
            UIHelper.SetButtonHoverEffect(btnXoa, btnXoaColor);
            UIHelper.SetButtonHoverEffect(btnLamMoi, btnLamMoiColor);
            UIHelper.SetButtonHoverEffect(btnTimKiem, btnTimKiemColor);

            // Rounded corners cho TextBoxes
            UIHelper.SetRoundedTextBox(txtTenNV, 8, 35);
            UIHelper.SetRoundedTextBox(txtDiaChi, 8, 35);
            UIHelper.SetRoundedTextBox(txtTimKiemNV, 8, 35);
        }

        private void LoadData()
        {
            try
            {
                dgvNhanVien.DataSource = nvBLL.LayDanhSachNhanVien();
                ClearInputs();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi");
            }
        }

        private void ClearInputs()
        {
            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtTimKiemNV.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            cboGioiTinh.SelectedIndex = 0;
            selectedMaNV = null;
            txtTenNV.Focus();
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var row = dgvNhanVien.Rows[e.RowIndex];
                    selectedMaNV = Convert.ToInt32(row.Cells["MaNV"].Value);
                    txtTenNV.Text = row.Cells["TenNV"].Value?.ToString();
                    cboGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();

                    if (row.Cells["NgaySinh"].Value != null && row.Cells["NgaySinh"].Value != DBNull.Value)
                    {
                        dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                    }

                    txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                }
                catch (Exception ex)
                {
                    CustomMessageBox.ShowError("Lỗi khi chọn dòng: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên nhân viên.", "Thông báo");
                txtTenNV.Focus();
                return;
            }

            if (cboGioiTinh.SelectedIndex == -1)
            {
                CustomMessageBox.Show("Vui lòng chọn giới tính.", "Thông báo");
                cboGioiTinh.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập địa chỉ.", "Thông báo");
                txtDiaChi.Focus();
                return;
            }

            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-tuoi)) tuoi--;
            if (tuoi < 18)
            {
                CustomMessageBox.ShowWarning("Nhân viên phải từ 18 tuổi trở lên.", "Thông báo");
                dtpNgaySinh.Focus();
                return;
            }

            try
            {
                var nv = new NhanVienDTO
                {
                    TenNV = txtTenNV.Text.Trim(),
                    GioiTinh = cboGioiTinh.Text,
                    NgaySinh = dtpNgaySinh.Value,
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (nvBLL.ThemNhanVien(nv))
                {
                    CustomMessageBox.Show("Thêm nhân viên thành công!", "Thành công");
                    LoadData();
                }
                else
                {
                    CustomMessageBox.ShowError("Thêm nhân viên thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi thêm: " + ex.Message, "Lỗi");
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == null)
            {
                CustomMessageBox.Show("Vui lòng chọn nhân viên cần sửa từ bảng.", "Thông báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên nhân viên.", "Thông báo");
                txtTenNV.Focus();
                return;
            }

            if (cboGioiTinh.SelectedIndex == -1)
            {
                CustomMessageBox.Show("Vui lòng chọn giới tính.", "Thông báo");
                cboGioiTinh.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo");
                txtDiaChi.Focus();
                return;
            }

            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-tuoi)) tuoi--;
            if (tuoi < 18)
            {
                CustomMessageBox.ShowWarning("Nhân viên phải từ 18 tuổi trở lên.", "Thông báo");
                dtpNgaySinh.Focus();
                return;
            }

            try
            {
                var nv = new NhanVienDTO
                {
                    MaNV = selectedMaNV.Value,
                    TenNV = txtTenNV.Text.Trim(),
                    GioiTinh = cboGioiTinh.Text,
                    NgaySinh = dtpNgaySinh.Value,
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (nvBLL.CapNhatNhanVien(nv))
                {
                    CustomMessageBox.Show("Cập nhật nhân viên thành công!", "Thành công");
                    LoadData();
                }
                else
                {
                    CustomMessageBox.ShowError("Cập nhật nhân viên thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi cập nhật: " + ex.Message, "Lỗi");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == null)
            {
                CustomMessageBox.Show("Vui lòng chọn nhân viên cần xóa từ bảng.", "Thông báo");
                return;
            }

            if (CustomMessageBox.ShowConfirm(
                    $"Bạn có chắc chắn muốn xóa nhân viên '{txtTenNV.Text}'?\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa") != DialogResult.Yes)
            {
                return;
            }



            try
            {
                if (nvBLL.XoaNhanVien(selectedMaNV.Value))
                {
                    CustomMessageBox.Show("Xóa nhân viên thành công!", "Thành công");
                    LoadData();
                }
                else
                {
                    CustomMessageBox.ShowError("Xóa nhân viên thất bại!");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi xóa: " + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiemNV.Text))
            {
                CustomMessageBox.ShowWarning("Vui lòng nhập từ khóa tìm kiếm.");
                txtTimKiemNV.Focus();
                return;
            }

            try
            {
                string keyword = txtTimKiemNV.Text.Trim();
                var result = nvBLL.TimKiemNhanVien(keyword);

                if (result == null || result.Count == 0)
                {
                    CustomMessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo");
                    LoadData();
                    return;
                }

                dgvNhanVien.DataSource = result;
                CustomMessageBox.Show($"Tìm thấy {result.Count} kết quả!", "Kết quả tìm kiếm");
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }


        private void PanelButtons_Resize(object sender, EventArgs e)
        {
            CenterButtonsInPanel(panelButtons);
        }

        private void CenterButtonsInPanel(Panel panel)
        {
            if (panel.Controls.Count == 0) return;

            int totalWidth = 0;
            int spacing = 10;
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is Button)
                    totalWidth += ctrl.Width + spacing;
            }
            totalWidth -= spacing;

            int startX = (panel.Width - totalWidth) / 2;
            int y = (panel.Height - panel.Controls[0].Height) / 2;

            int currentX = startX;
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.Location = new Point(currentX, y);
                    currentX += ctrl.Width + spacing;
                }
            }
        }

    }
}