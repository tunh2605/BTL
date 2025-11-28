using System;
using System.Drawing;
using System.Windows.Forms;

public class CustomMessageBox : Form
{
    private Label lblMessage;
    private Button btnOk;
    private Button btnYes;
    private Button btnNo;

    private DialogResult result = DialogResult.None;

    public CustomMessageBox(string message, string title = "Thông báo", bool isConfirm = false)
    {
        this.Text = title;
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;
        this.Padding = new Padding(2);
        this.Size = new Size(380, 190);

        // Viền đen
        this.Paint += (s, e) =>
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 2, this.Height - 2);
            }
        };

        lblMessage = new Label()
        {
            Text = message,
            Font = new Font("Segoe UI", 11),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 100
        };

        this.Controls.Add(lblMessage);

        if (!isConfirm)
        {
            // ----- NÚT OK -----
            btnOk = new Button()
            {
                Text = "OK",
                Width = 90,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 1;
            btnOk.FlatAppearance.BorderColor = Color.Black;
            btnOk.Location = new Point((this.Width - btnOk.Width) / 2, 120);
            btnOk.Click += (s, e) => { this.result = DialogResult.OK; this.Close(); };
            this.Controls.Add(btnOk);
        }
        else
        {
            // ----- NÚT YES -----
            btnYes = new Button()
            {
                Text = "Yes",
                Width = 90,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnYes.FlatAppearance.BorderSize = 1;
            btnYes.FlatAppearance.BorderColor = Color.Black;
            btnYes.Location = new Point(75, 120);
            btnYes.Click += (s, e) => { this.result = DialogResult.Yes; this.Close(); };
            this.Controls.Add(btnYes);

            // ----- NÚT NO -----
            btnNo = new Button()
            {
                Text = "No",
                Width = 90,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNo.FlatAppearance.BorderSize = 1;
            btnNo.FlatAppearance.BorderColor = Color.Black;
            btnNo.Location = new Point(200, 120);
            btnNo.Click += (s, e) => { this.result = DialogResult.No; this.Close(); };
            this.Controls.Add(btnNo);
        }
    }

    // Thông báo bình thường
    public static void Show(string message, string title = "Thông báo")
    {
        CustomMessageBox msg = new CustomMessageBox(message, title, false);
        msg.ShowDialog();
    }

    // Xác nhận Yes/No → trả về DialogResult
    public static DialogResult ShowConfirm(string message, string title = "Xác nhận")
    {
        CustomMessageBox msg = new CustomMessageBox(message, title, true);
        msg.ShowDialog();
        return msg.result;
    }

    public static void ShowWarning(string message, string title = "Cảnh báo")
    {
        CustomMessageBox msg = new CustomMessageBox(message, title, false);
        msg.ShowDialog();
    }

    public static void ShowError(string message, string title = "Lỗi")
    {
        CustomMessageBox msg = new CustomMessageBox(message, title, false);
        msg.ShowDialog();
    }
}
