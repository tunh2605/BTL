using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class UIHelper
{
    // Set bo tròn cho Button
    public static void SetRoundedButton(Button btn, int radius = 8)
    {
        if (btn == null) return;

        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Region?.Dispose();

        GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), radius);
        btn.Region = new Region(path);
        path.Dispose();
        btn.Resize += (s, e) =>
        {
            btn.Region?.Dispose();
            GraphicsPath newPath = GetRoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), radius);
            btn.Region = new Region(newPath);
            newPath.Dispose();
        };
    }

    // Set bo tròn cho TextBox
    public static void SetRoundedTextBox(TextBox txt, int radius = 8, int height = 35)
    {
        if (txt == null) return;

        txt.BorderStyle = BorderStyle.None;
        txt.Height = height;

        txt.Region?.Dispose();

        GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, txt.Width, txt.Height), radius);
        txt.Region = new Region(path);
        path.Dispose();
        txt.Resize += (s, e) =>
        {
            txt.Region?.Dispose();
            GraphicsPath newPath = GetRoundedRectanglePath(new Rectangle(0, 0, txt.Width, txt.Height), radius);
            txt.Region = new Region(newPath);
            newPath.Dispose();
        };
    }

    // set bo tròn cho Panel
    public static void SetRoundedPanel(Panel panel, int radius = 12)
    {
        if (panel == null) return;
        panel.Region?.Dispose();

        GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, panel.Width, panel.Height), radius);
        panel.Region = new Region(path);
        path.Dispose();
        panel.Resize += (s, e) =>
        {
            panel.Region?.Dispose();
            GraphicsPath newPath = GetRoundedRectanglePath(new Rectangle(0, 0, panel.Width, panel.Height), radius);
            panel.Region = new Region(newPath);
            newPath.Dispose();
        };
    }

    // Tạo GraphicsPath cho hình chữ nhật bo tròn
    private static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int diameter = radius * 2;
        if (diameter > rect.Width) diameter = rect.Width;
        if (diameter > rect.Height) diameter = rect.Height;

        Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

        // Top left arc
        path.AddArc(arc, 180, 90);

        // Top right arc
        arc.X = rect.Right - diameter;
        path.AddArc(arc, 270, 90);

        // Bottom right arc
        arc.Y = rect.Bottom - diameter;
        path.AddArc(arc, 0, 90);

        // Bottom left arc
        arc.X = rect.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }

    // hover cho Button
    public static void SetButtonHoverEffect(Button btn, Color normalColor, float lightFactor = 0.2f)
    {
        if (btn == null) return;

        Color hoverColor = ControlPaint.Light(normalColor, lightFactor);

        btn.MouseEnter += (s, e) =>
        {
            btn.BackColor = hoverColor;
            btn.Cursor = Cursors.Hand;
        };

        btn.MouseLeave += (s, e) =>
        {
            btn.BackColor = normalColor;
            btn.Cursor = Cursors.Default;
        };
    }

    // Thêm method để vẽ border cho control rounded
    public static void DrawRoundedBorder(Control control, PaintEventArgs e, int radius, Color borderColor, int borderWidth = 1)
    {
        if (control == null || e == null) return;

        using (GraphicsPath path = GetRoundedRectanglePath(
            new Rectangle(0, 0, control.Width - 1, control.Height - 1), radius))
        using (Pen pen = new Pen(borderColor, borderWidth))
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(pen, path);
        }
    }
}