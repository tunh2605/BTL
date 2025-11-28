
using BLL;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmDoiMatKhau : Form
    {
        private TaiKhoanBLL bll = new TaiKhoanBLL();
        private int maNhanVien;

        public FrmDoiMatKhau(int maNV)
        {
            InitializeComponent();
            this.maNhanVien = maNV;
        }

        private void FrmDoiMatKhau_Load(object sender, EventArgs e)
        {
            txtOldPass.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string matKhauCu = txtOldPass.Text.Trim();
            string matKhauMoi = txtNewPass.Text.Trim();
            string xacNhan = txtConfirm.Text.Trim();

            if (matKhauCu == "" || matKhauMoi == "" || xacNhan == "")
            {
                CustomMessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            if (matKhauMoi != xacNhan)
            {
                CustomMessageBox.ShowWarning("Mật khẩu mới và xác nhận không khớp!", "Lỗi");
                return;
            }

            if (bll.DoiMatKhau(maNhanVien, matKhauCu, matKhauMoi))
            {
                CustomMessageBox.Show("Đổi mật khẩu thành công!", "Thông báo");
                this.Close();
            }
            else
            {
                CustomMessageBox.ShowWarning("Mật khẩu cũ không đúng hoặc tài khoản không tồn tại!", "Lỗi");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}