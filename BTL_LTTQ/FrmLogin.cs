using BLL;
using DTO;
using GUI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        private LoginBLL bll = new LoginBLL();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                picLogo.Image = Properties.Resources.homestay_logo;

                string logoPath = Path.Combine(Application.StartupPath, "logo.png");
                if (File.Exists(logoPath))
                {
                    picLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch
            {
                // Không có logo thì bỏ qua
            }

            ResizeControls();
            txtUser.Focus();
        }

        private void FrmLogin_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void ResizeControls()
        {
            if (this.Width < 800)
                pnlLeft.Width = (int)(this.Width * 0.35);
            else
                pnlLeft.Width = 300;

            int logoSize = Math.Min(pnlLeft.Width - 50, 320);

            int centerX = (pnlLeft.Width - logoSize) / 2;

            int centerY = (pnlLeft.Height - logoSize) / 2;

            picLogo.Location = new Point(centerX, centerY);
            picLogo.Size = new Size(logoSize, logoSize);
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUser.Text.Trim();
            string matKhau = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                CustomMessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            TaiKhoanDTO taiKhoan = bll.GetTaiKhoan(tenDangNhap, matKhau);

            if (taiKhoan != null)
            {
                CustomMessageBox.Show("Đăng nhập thành công!");
                FrmMain frmMain = new FrmMain(taiKhoan.TenDangNhap, taiKhoan.Quyen);
                frmMain.MaNhanVien = taiKhoan.MaNV;
                frmMain.Quyen = taiKhoan.Quyen;
                frmMain.TenDangNhap = taiKhoan.TenDangNhap;

                this.Hide();
                frmMain.ShowDialog();
                this.Show();

                txtPass.Clear();
                txtUser.Focus();
            }
            else
            {
                CustomMessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                txtPass.Clear();
                txtUser.Focus();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.ShowConfirm("Bạn có chắc chắn muốn thoát?", "Xác nhận") == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnLogin_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                btnExit_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
