using BLL;
using DTO;
using GUI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmDatPhong : Form
    {
        private DatPhongBLL bll = new DatPhongBLL();
        private readonly bool isAdmin;
        private decimal currentDonGia = 0m;
        private FrmMain mainForm;

        // Constructor nhận FrmMain và quyền
        public FrmDatPhong(FrmMain main, string quyen)
        {
            InitializeComponent();
            mainForm = main;
            isAdmin = string.Equals(quyen, "admin", StringComparison.OrdinalIgnoreCase);
            cboMaPhong.DropDownStyle = ComboBoxStyle.DropDown;
        }

        // Constructor mặc định cho designer
        public FrmDatPhong()
        {
            InitializeComponent();
            cboMaPhong.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void FrmDatPhong_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxData();
            AutoGenerateMaDatPhong();
        }

        private void LoadComboBoxData()
        {
            // Load khách hàng
            var dsKH = bll.LayDanhSachKhachHang();
            cboMaKH.DataSource = dsKH;
            cboMaKH.DisplayMember = "TenKH";
            cboMaKH.ValueMember = "MaKH";

            // Load nhân viên
            var dsNV = bll.LayDanhSachNhanVien();
            cboMaNV.DataSource = dsNV;
            cboMaNV.DisplayMember = "TenNV";
            cboMaNV.ValueMember = "MaNV";

            // Load phòng trống
            var dsPhong = bll.LayDanhSachPhongTrong();
            cboMaPhong.DataSource = dsPhong;
            cboMaPhong.DisplayMember = "TenPhong";
            cboMaPhong.ValueMember = "MaPhong";

            // Load đơn giá phòng, ngày mặc định
            if (!int.TryParse(cboMaPhong.SelectedValue?.ToString(), out int maPhong))
                return;

            currentDonGia = bll.LayDonGiaPhong(maPhong);
            txtDonGia.Text = currentDonGia.ToString("N0");
            dtpNgayDat.Value = DateTime.Now;
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);
        }

        private void LoadData()
        {
            var data = bll.LayDanhSachDatPhong();
            dgvDatPhong.DataSource = data;
        }

        private void AutoGenerateMaDatPhong()
        {
            int nextMa = bll.LayMaDatPhongTiepTheo();
            txtMaDatPhong.Text = nextMa.ToString();
        }

        private void cboMaPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaPhong.SelectedValue == null)
            {
                currentDonGia = 0m;
                txtDonGia.Text = "0";
                return;
            }

            if (!int.TryParse(cboMaPhong.SelectedValue.ToString(), out int maPhong))
                return;

            currentDonGia = bll.LayDonGiaPhong(maPhong);
            txtDonGia.Text = currentDonGia.ToString("N0");
            TinhTongTien();
        }

        private void dtpNgayDen_ValueChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void dtpNgayDi_ValueChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void TinhTongTien()
        {
            int soNgay = bll.TinhSoNgay(dtpNgayDen.Value, dtpNgayDi.Value);
            decimal tongTien = bll.TinhTongTien(currentDonGia, dtpNgayDen.Value, dtpNgayDi.Value);

            txtTongTien.Text = $"{tongTien:N0} VND";
            txtSoNgay.Text = soNgay.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMaKH.SelectedValue == null ||
                    cboMaNV.SelectedValue == null ||
                    cboMaPhong.SelectedValue == null)
                {
                    CustomMessageBox.ShowWarning("Vui lòng chọn đầy đủ khách hàng, nhân viên và phòng!");
                    return;
                }

                if (dtpNgayDi.Value < dtpNgayDen.Value)
                {
                    CustomMessageBox.ShowWarning("Ngày đi không thể nhỏ hơn ngày đến!");
                    dtpNgayDi.Focus();
                    return;
                }

                int maDP;
                if (!int.TryParse(txtMaDatPhong.Text.Trim(), out maDP) || maDP <= 0)
                {
                    maDP = bll.LayMaDatPhongTiepTheo();
                    txtMaDatPhong.Text = maDP.ToString();
                }

                var dto = new DatPhongDTO
                {
                    MaDatPhong = maDP,
                    MaKH = Convert.ToInt32(cboMaKH.SelectedValue),
                    MaNV = Convert.ToInt32(cboMaNV.SelectedValue),
                    MaPhong = Convert.ToInt32(cboMaPhong.SelectedValue),
                    NgayDat = dtpNgayDat.Value,
                    NgayDen = dtpNgayDen.Value,
                    NgayDi = dtpNgayDi.Value
                };

                string error;
                if (bll.ThemDatPhong(dto, out error))
                {
                    CustomMessageBox.Show("Đặt phòng và tạo hóa đơn thành công!");
                    LoadData();
                    LoadComboBoxData();
                    AutoGenerateMaDatPhong();
                }
                else
                {
                    CustomMessageBox.ShowError("Lỗi: " + error);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi đặt phòng: " + ex.Message);
            }
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaDatPhong.Clear();
            txtDonGia.Clear();
            cboMaKH.SelectedIndex = 0;
            cboMaNV.SelectedIndex = 0;
            cboMaPhong.SelectedIndex = 0;
            txtTongTien.Text = "0 VND";
            txtSoNgay.Text = "0";
            dtpNgayDat.Value = DateTime.Now;
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);
            LoadData();
            LoadComboBoxData();
            AutoGenerateMaDatPhong();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDatPhong.CurrentRow == null)
            {
                CustomMessageBox.Show("Vui lòng chọn phiếu đặt phòng cần trả!");
                return;
            }

            int maDP = Convert.ToInt32(dgvDatPhong.CurrentRow.Cells["MaDatPhong"].Value);

            if (CustomMessageBox.ShowConfirm(
                "Xác nhận trả phòng này? (Hóa đơn vẫn được giữ lại)",
                "Xác nhận") != DialogResult.Yes)
                return;

            try
            {
                string error;
                if (bll.XoaDatPhong(maDP, out error))
                {
                    CustomMessageBox.Show("Trả phòng thành công! Hóa đơn vẫn giữ nguyên mã đặt phòng.");
                    LoadData();
                    LoadComboBoxData();
                    AutoGenerateMaDatPhong();
                }
                else
                {
                    CustomMessageBox.ShowError("Lỗi: " + error);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError("Lỗi khi trả phòng: " + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                var result = bll.TimKiemDatPhong(keyword);

                if (result.Count == 0)
                {
                    CustomMessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo");
                    return;
                }

                dgvDatPhong.DataSource = result;
                CustomMessageBox.Show($"Tìm thấy {result.Count} kết quả!", "Thành công");
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi");
            }
        }

        private void btnLapHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDatPhong.Text))
            {
                CustomMessageBox.Show("Vui lòng chọn phiếu đặt phòng!");
                return;
            }

            var frmHD = new FrmHoaDon(mainForm, isAdmin);
            frmHD.Tag = txtMaDatPhong.Text.Trim();
            mainForm.NavigateTo(frmHD);
        }

        private void dgvDatPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var datPhong = dgvDatPhong.Rows[e.RowIndex].DataBoundItem as DatPhongDTO;
            if (datPhong == null) return;

            txtMaDatPhong.Text = datPhong.MaDatPhong.ToString();
            cboMaKH.SelectedValue = datPhong.MaKH;
            cboMaNV.SelectedValue = datPhong.MaNV;

            cboMaPhong.SelectedValue = datPhong.MaPhong;

            if (cboMaPhong.SelectedValue == null ||
                (int)cboMaPhong.SelectedValue != datPhong.MaPhong)
            {
                cboMaPhong.Text = datPhong.TenPhong;
            }

            dtpNgayDat.Value = datPhong.NgayDat;
            dtpNgayDen.Value = datPhong.NgayDen;
            dtpNgayDi.Value = datPhong.NgayDi;

            currentDonGia = bll.LayDonGiaPhong(datPhong.MaPhong);
            txtDonGia.Text = currentDonGia.ToString("N0");

            txtTongTien.Text = $"{datPhong.TongTien:N0} VND";
            txtSoNgay.Text = ((datPhong.NgayDi - datPhong.NgayDen).Days).ToString();
        }


        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm...")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.FromArgb(52, 73, 94);
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Tìm kiếm...";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }
    }
}