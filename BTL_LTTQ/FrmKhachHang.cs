using BLL;
using DTO;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmKhachHang : Form
    {
        private KhachHangBLL bll = new KhachHangBLL();

        public FrmKhachHang()
        {
            InitializeComponent();
        }

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");
            cboGioiTinh.SelectedIndex = 0;
            LoadData();
            ClearFields();
        }

        private void LoadData()
        {
            dgvKhachHang.DataSource = bll.LayDanhSachKhachHang();
        }

        private void ClearFields()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtCMND.Clear();
            cboGioiTinh.SelectedIndex = 0;
            txtTimKiemMaKH.Clear();
            txtMaKH.Enabled = true;
            txtMaKH.Text = bll.LayMaKHTiepTheo().ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            int maKH;
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                maKH = bll.LayMaKHTiepTheo();
            }


            else if (!int.TryParse(txtMaKH.Text, out maKH))
            {
                CustomMessageBox.Show("Vui lòng nhập Mã KH là số nguyên!");
                txtMaKH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên khách hàng!");
                txtTenKH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập sdt khách hàng !");
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên khách hàng!");
                txtTenKH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCMND.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập CMND/CCCD của khách hàng!");
                txtCMND.Focus();
                return;
            }

            if (!txtCMND.Text.All(char.IsDigit))
            {
                CustomMessageBox.Show("CMND/CCCD chỉ được chứa chữ số!");
                txtCMND.Focus();
                return;
            }

            if (txtCMND.Text.Length != 9 && txtCMND.Text.Length != 12)
            {
                CustomMessageBox.Show("CMND phải gồm 9 số hoặc CCCD phải gồm 12 số!");
                txtCMND.Focus();
                return;
            }


            if (txtTenKH.Text.Any(char.IsDigit))
            {
                CustomMessageBox.Show("Tên khách hàng không được chứa số!");
                txtTenKH.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                string sdt = txtSDT.Text.Trim();

                if (sdt.Length != 11 || !sdt.All(char.IsDigit) || sdt[0] != '0')
                {
                    CustomMessageBox.Show("Số điện thoại phải gồm 11 chữ số, bắt đầu bằng số 0 và không chứa chữ cái!");
                    txtSDT.Focus();
                    return;
                }
            }



            if (bll.ThemKhachHang(maKH, txtTenKH.Text.Trim(), cboGioiTinh.Text, txtSDT.Text.Trim(), txtCMND.Text.Trim()))
            {
                CustomMessageBox.Show("Thêm khách hàng thành công!");
                LoadData();
                ClearFields();
            }
            else
            {
                CustomMessageBox.Show("Thêm thất bại! Kiểm tra mã KH hoặc dữ liệu.");
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text) || !int.TryParse(txtMaKH.Text, out int maKH))
            {
                CustomMessageBox.Show("Mã khách hàng không hợp lệ!");
                return;
            }


            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập sdt khách hàng !");
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên khách hàng!");
                txtTenKH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCMND.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập CMND/CCCD của khách hàng!");
                txtCMND.Focus();
                return;
            }

            if (!txtCMND.Text.All(char.IsDigit))
            {
                CustomMessageBox.Show("CMND/CCCD chỉ được chứa chữ số!");
                txtCMND.Focus();
                return;
            }

            if (txtCMND.Text.Length != 9 && txtCMND.Text.Length != 12)
            {
                CustomMessageBox.Show("CMND phải gồm 9 số hoặc CCCD phải gồm 12 số!");
                txtCMND.Focus();
                return;
            }


            if (txtTenKH.Text.Any(char.IsDigit))
            {
                CustomMessageBox.Show("Tên khách hàng không được chứa số!");
                txtTenKH.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                string sdt = txtSDT.Text.Trim();

                if (sdt.Length != 11 || !sdt.All(char.IsDigit) || sdt[0] != '0')
                {
                    CustomMessageBox.Show("Số điện thoại phải gồm 11 chữ số, bắt đầu bằng số 0 và không chứa chữ cái!");
                    txtSDT.Focus();
                    return;
                }
            }

            if (bll.CapNhatKhachHang(maKH, txtTenKH.Text.Trim(), cboGioiTinh.Text, txtSDT.Text.Trim(), txtCMND.Text.Trim()))
            {
                CustomMessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearFields();
            }
            else
            {
                CustomMessageBox.Show("Cập nhật thất bại!");
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text) || !int.TryParse(txtMaKH.Text, out int maKH))
            {
                CustomMessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            if (CustomMessageBox.ShowConfirm("Bạn có chắc muốn xóa?", "Xác nhận") == DialogResult.Yes)
            {
                if (bll.XoaKhachHang(maKH))
                {
                    CustomMessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearFields();
                }
                else
                {
                    CustomMessageBox.Show("Không thể xóa! Khách hàng đang được sử dụng.");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadData();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtTenKH.Text = row.Cells["TenKH"].Value.ToString();
                cboGioiTinh.Text = row.Cells["GioiTinh"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtCMND.Text = row.Cells["CMND"].Value.ToString();
                txtMaKH.Enabled = false;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiemMaKH.Text) || !int.TryParse(txtTimKiemMaKH.Text, out int maKH))
            {
                CustomMessageBox.Show("Vui lòng nhập mã hợp lệ!");
                return;
            }

            var result = bll.TimKiemKhachHangTheoMa(maKH);
            if (result.Count == 0)
            {
                CustomMessageBox.Show("Không tìm thấy khách hàng!");
                LoadData();
            }
            else
            {
                dgvKhachHang.DataSource = result;
            }
        }
    }
}