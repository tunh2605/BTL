using BLL;
using DTO;
using GUI;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmHoaDon : Form
    {
        private HoaDonBLL bll = new HoaDonBLL();
        private bool isAdmin;
        private FrmMain mainForm;

        public FrmHoaDon(FrmMain main = null, bool admin = false)
        {
            InitializeComponent();
            mainForm = main;
            isAdmin = admin;
            btnSua.Enabled = isAdmin;
            btnXoa.Enabled = isAdmin;
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
            btnSua.Enabled = isAdmin;
            btnXoa.Enabled = isAdmin;

            // Nếu có Tag (ví dụ từ FrmDatPhong) thì điền vào txtMaDatPhong
            if (this.Tag is string s && !string.IsNullOrWhiteSpace(s))
                txtMaDatPhong.Text = s;
        }

        private void LoadData()
        {
            dgvHoaDon.DataSource = bll.LayDanhSachHoaDon();

            if (dgvHoaDon.Columns["MaHD"] != null) dgvHoaDon.Columns["MaHD"].HeaderText = "Mã hóa đơn";
            if (dgvHoaDon.Columns["MaDatPhong"] != null) dgvHoaDon.Columns["MaDatPhong"].HeaderText = "Mã đặt phòng";

            txtMaHD.Clear();
            txtMaDatPhong.Clear();
            txtTongTien.Clear();
            dtpNgayLap.Value = DateTime.Now;
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvHoaDon.Rows[e.RowIndex];

                txtMaHD.Text = row.Cells["MaHD"].Value?.ToString() ?? "";
                txtMaDatPhong.Text = row.Cells["MaDatPhong"].Value?.ToString() ?? "";

                if (row.Cells["NgayLap"].Value != null)
                    dtpNgayLap.Value = Convert.ToDateTime(row.Cells["NgayLap"].Value);

                txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "";
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                CustomMessageBox.Show("Chỉ admin mới được sửa hóa đơn!");
                return;
            }

            if (!int.TryParse(txtMaHD.Text, out int maHD))
            {
                CustomMessageBox.Show("Mã hóa đơn không hợp lệ.");
                return;
            }

            var hd = bll.LayHoaDonTheoMa(maHD);
            if (hd == null)
            {
                CustomMessageBox.Show("Không tìm thấy hóa đơn.");
                return;
            }

            // MaDatPhong có thể null
            if (!bll.KiemTraMaDatPhong(txtMaDatPhong.Text, out int? maDatPhong))
            {
                CustomMessageBox.Show("Mã đặt phòng phải là số nguyên.");
                return;
            }

            if (!bll.KiemTraTongTien(txtTongTien.Text, out decimal tongTien))
            {
                CustomMessageBox.Show("Tổng tiền không hợp lệ.");
                return;
            }

            if (bll.CapNhatHoaDon(maHD, maDatPhong, dtpNgayLap.Value, tongTien))
            {
                LoadData();
                CustomMessageBox.Show("Cập nhật hóa đơn thành công!");
            }
            else
            {
                CustomMessageBox.Show("Cập nhật thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                CustomMessageBox.Show("Chỉ admin mới được xóa hóa đơn!");
                return;
            }


            if (CustomMessageBox.ShowConfirm(
                    $"Bạn có chắc chắn muốn xóa hóa đơn '{txtMaHD.Text}'?\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa") != DialogResult.Yes)
            {
                return;
            }

            if (!int.TryParse(txtMaHD.Text, out int maHD))
            {
                CustomMessageBox.Show("Vui lòng chọn hóa đơn hợp lệ để xóa.");
                return;
            }

            var hd = bll.LayHoaDonTheoMa(maHD);
            if (hd == null)
            {
                CustomMessageBox.Show("Không tìm thấy hóa đơn.");
                return;
            }

            if (bll.XoaHoaDon(maHD))
            {
                LoadData();
                CustomMessageBox.Show("Xóa hóa đơn thành công!");
            }
            else
            {
                CustomMessageBox.Show("Xóa thất bại!");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            var result = bll.TimKiemHoaDon(keyword);

            if (result.Count == 0)
            {
                CustomMessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo");
                return;
            }

            dgvHoaDon.DataSource = result;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
