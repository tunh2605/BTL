using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmMain : Form
    {
        public string TenDangNhap { get; set; }
        public string Quyen { get; set; }
        public int MaNhanVien { get; set; }

        private Timer clockTimer;
        private Button currentButton;
        private Form currentChildForm;

        public FrmMain()
        {
            InitializeComponent();
        }

        public FrmMain(string tenDangNhap, string quyen)
        {
            InitializeComponent();
            this.TenDangNhap = tenDangNhap;
            this.Quyen = quyen;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lblNhanVien.Text = $"👤 {TenDangNhap} ({Quyen})";

            // Phân quyền
            if (Quyen != null && Quyen.ToLower() == "nhanvien")
            {
                btnNhanVien.Enabled = false;
                btnNhanVien.ForeColor = Color.Gray;
                btnBaoCao.Enabled = false;
                btnBaoCao.ForeColor = Color.Gray;
            }

            // Khởi tạo timer cho đồng hồ
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
            UpdateClock();

            // Add hover effects
            AddHoverEffects();
        }

        private void UpdateClock()
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void AddHoverEffects()
        {
            // Main menu buttons
            AddMenuButtonHover(btnPhong);
            AddMenuButtonHover(btnKhachHang);
            AddMenuButtonHover(btnDatPhong);
            AddMenuButtonHover(btnHoaDon);
            AddMenuButtonHover(btnNhanVien);
            AddMenuButtonHover(btnBaoCao);

            // Utility buttons
            AddUtilityButtonHover(btnDoiMatKhau);
            AddUtilityButtonHover(btnDangXuat);

            // Exit button
            btnThoat.MouseEnter += (s, e) => btnThoat.BackColor = Color.FromArgb(192, 57, 43);
            btnThoat.MouseLeave += (s, e) => btnThoat.BackColor = Color.FromArgb(231, 76, 60);
        }

        private void AddMenuButtonHover(Button btn)
        {
            if (!btn.Enabled) return;

            btn.Paint += (s, e) => DrawButtonBorder(btn, e);

            btn.MouseEnter += (s, e) => {
                if (currentButton != btn)
                {
                    btn.BackColor = Color.FromArgb(236, 240, 241);
                }
                btn.Invalidate(); // Vẽ lại button để hiện border
            };

            btn.MouseLeave += (s, e) => {
                if (currentButton != btn)
                {
                    btn.BackColor = Color.Transparent;
                }
                btn.Invalidate(); // Vẽ lại button để ẩn border
            };
        }

        private void AddUtilityButtonHover(Button btn)
        {
            btn.Paint += (s, e) => DrawButtonBorder(btn, e);

            btn.MouseEnter += (s, e) => {
                btn.BackColor = Color.FromArgb(236, 240, 241);
                btn.Invalidate();
            };

            btn.MouseLeave += (s, e) => {
                btn.BackColor = Color.Transparent;
                btn.Invalidate();
            };
        }

        private void DrawButtonBorder(Button btn, PaintEventArgs e)
        {
            // Vẽ border left khi hover hoặc active
            if (btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position)) || btn == currentButton)
            {
                using (Pen pen = new Pen(Color.FromArgb(30, 30, 30), 6))
                {
                    e.Graphics.DrawLine(pen, 0, 0, 0, btn.Height);
                }
            }
        }

        public void ActivateButton(Button btn)
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.Transparent;
                currentButton.Invalidate();
            }

            currentButton = btn;
            btn.BackColor = Color.FromArgb(236, 240, 241);
            btn.Invalidate();
        }

        private void OpenChildForm(Form childForm)
        {
            // Đóng form con hiện tại nếu có
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                currentChildForm.Dispose();
            }

            currentChildForm = childForm;

            // Cấu hình form con
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Xóa controls cũ và thêm form mới
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        public void NavigateTo(Form nextForm)
        {
            OpenChildForm(nextForm);
        }

        private void btnPhong_Click(object sender, EventArgs e)
        {
            ActivateButton(btnPhong);
            FrmPhong frm = new FrmPhong(Quyen);
            OpenChildForm(frm);
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ActivateButton(btnKhachHang);
            FrmKhachHang f = new FrmKhachHang();
            OpenChildForm(f);
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            ActivateButton(btnDatPhong);
            FrmDatPhong frm = new FrmDatPhong(this, Quyen);
            OpenChildForm(frm);
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            ActivateButton(btnHoaDon);
            bool isAdmin = string.Equals(Quyen, "admin", StringComparison.OrdinalIgnoreCase);
            FrmHoaDon f = new FrmHoaDon(this, isAdmin);
            OpenChildForm(f);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(btnNhanVien);
            FrmNhanVien f = new FrmNhanVien();
            OpenChildForm(f);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ActivateButton(btnBaoCao);
            FrmBaoCaoDoanhThu f = new FrmBaoCaoDoanhThu();
            OpenChildForm(f);
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            FrmDoiMatKhau frm = new FrmDoiMatKhau(MaNhanVien);
            frm.ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if( CustomMessageBox.ShowConfirm(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận") == DialogResult.Yes){

                if (clockTimer != null)
                {
                    clockTimer.Stop();
                    clockTimer.Dispose();
                }

                if (currentChildForm != null)
                {
                    currentChildForm.Close();
                    currentChildForm.Dispose();
                }

                this.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.ShowConfirm(
                    "Bạn có chắc chắn muốn thoát chương trình?",
                    "Xác nhận") == DialogResult.Yes)
            {
                clockTimer?.Stop();
                clockTimer?.Dispose();

                Application.Exit();
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Dispose();
            }

            // Đóng form con nếu có
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                currentChildForm.Dispose();
            }

            base.OnFormClosing(e);
        }
        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ border bottom cho pnlTop
            using (Pen pen = new Pen(Color.FromArgb(189, 195, 199), 2))
            {
                e.Graphics.DrawLine(pen, 0, pnlTop.Height - 2, pnlTop.Width, pnlTop.Height - 2);
            }
        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ border right cho pnlSidebar
            using (Pen pen = new Pen(Color.FromArgb(189, 195, 199), 2))
            {
                e.Graphics.DrawLine(pen, pnlSidebar.Width - 2, 0, pnlSidebar.Width - 2, pnlSidebar.Height);
            }
        }
    }
}