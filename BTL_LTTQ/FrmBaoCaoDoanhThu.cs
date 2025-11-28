using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using BLL;
using DTO;

namespace GUI
{
    public partial class FrmBaoCaoDoanhThu : Form
    {
        private BaoCaoDoanhThuBLL bll = new BaoCaoDoanhThuBLL();

        public FrmBaoCaoDoanhThu()
        {
            InitializeComponent();
        }

        private void FrmBaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            btnThongKe_Click(sender, e);
        }

        private void InitializeComboBoxes()
        {
            // Khởi tạo combo box tháng
            for (int i = 1; i <= 12; i++)
            {
                cboThang.Items.Add(i.ToString());
            }
            cboThang.SelectedIndex = DateTime.Now.Month - 1;

            // Khởi tạo combo box năm
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear; i++)
            {
                cboNam.Items.Add(i.ToString());
            }
            cboNam.SelectedItem = currentYear.ToString();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (cboThang.SelectedItem == null || cboNam.SelectedItem == null)
            {
                CustomMessageBox.ShowWarning("Vui lòng chọn đầy đủ Tháng và Năm.", "Thông báo");
                return;
            }

            int month = int.Parse(cboThang.SelectedItem.ToString());
            int year = int.Parse(cboNam.SelectedItem.ToString());

            try
            {
                // Lấy dữ liệu
                List<BaoCaoDoanhThuDTO> danhSach = bll.LayBaoCaoDoanhThu(month, year);

                // Hiển thị lên DataGridView
                dgvDoanhThu.DataSource = danhSach;

                // Tính và hiển thị tổng doanh thu
                decimal tongDoanhThu = bll.TinhTongDoanhThu(month, year);
                txtTongDoanhThu.Text = bll.FormatTien(tongDoanhThu);

                // Kiểm tra nếu kh có dữ liệu
                if (danhSach.Count == 0)
                {
                    CustomMessageBox.Show("Không có dữ liệu trong tháng này.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowWarning("Lỗi trong quá trình thống kê: " + ex.Message, "Lỗi");
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDoanhThu.Rows.Count == 0)
                {
                    CustomMessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                    return;
                }

                dgvDoanhThu.AllowUserToAddRows = false;

                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Chọn nơi lưu file Excel",
                    FileName = $"BaoCaoDoanhThu_{cboThang.SelectedItem}_{cboNam.SelectedItem}.xlsx"
                };

                if (saveFile.ShowDialog() != DialogResult.OK)
                    return;

                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
                exSheet.Name = "DoanhThu";

                // ===== TIÊU ĐỀ CHÍNH =====
                Excel.Range header = exSheet.Range["A1", "D1"];
                header.Merge();
                header.Value = "BÁO CÁO DOANH THU KHÁCH SẠN";
                header.Font.Bold = true;
                header.Font.Size = 18;
                header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                header.Interior.Color = Color.LightYellow;

                // ===== THÔNG TIN PHỤ =====
                exSheet.Cells[2, 1] = "Tháng:";
                exSheet.Cells[2, 2] = cboThang.SelectedItem?.ToString();
                exSheet.Cells[2, 3] = "Năm:";
                exSheet.Cells[2, 4] = cboNam.SelectedItem?.ToString();
                exSheet.Cells[3, 1] = "Ngày xuất:";
                exSheet.Cells[3, 2] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                // ===== TIÊU ĐỀ CỘT =====
                int colIndex = 0;
                for (int i = 0; i < dgvDoanhThu.Columns.Count; i++)
                {
                    if (dgvDoanhThu.Columns[i].Visible)
                    {
                        colIndex++;
                        exSheet.Cells[5, colIndex] = dgvDoanhThu.Columns[i].HeaderText;
                    }
                }

                Excel.Range head = exSheet.Range["A5", GetExcelColumnName(colIndex) + "5"];
                head.Font.Bold = true;
                head.Font.Color = Color.Black;
                head.Interior.Color = Color.LightSkyBlue;
                head.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                head.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ===== GHI DỮ LIỆU =====
                int rowIndex = 6;
                for (int i = 0; i < dgvDoanhThu.Rows.Count; i++)
                {
                    if (dgvDoanhThu.Rows[i].IsNewRow) continue;

                    colIndex = 0;
                    for (int j = 0; j < dgvDoanhThu.Columns.Count; j++)
                    {
                        if (dgvDoanhThu.Columns[j].Visible)
                        {
                            colIndex++;
                            object value = dgvDoanhThu.Rows[i].Cells[j].Value;
                            if (value != null)
                            {
                                if (value is DateTime)
                                    exSheet.Cells[rowIndex, colIndex] = ((DateTime)value).ToString("dd/MM/yyyy");
                                else
                                    exSheet.Cells[rowIndex, colIndex] = value.ToString();
                            }
                        }
                    }
                    rowIndex++;
                }

                // ===== DÒNG TỔNG DOANH THU =====
                int lastRow = rowIndex;
                exSheet.Cells[lastRow + 1, colIndex - 1] = "TỔNG DOANH THU:";
                exSheet.Cells[lastRow + 1, colIndex] = txtTongDoanhThu.Text;
                Excel.Range totalRange = exSheet.Range[GetExcelColumnName(colIndex - 1) + (lastRow + 1),
                    GetExcelColumnName(colIndex) + (lastRow + 1)];
                totalRange.Font.Bold = true;
                totalRange.Font.Size = 12;
                totalRange.Font.Color = Color.Blue;
                totalRange.Interior.Color = Color.LightYellow;

                // ===== ĐỊNH DẠNG TOÀN BẢNG =====
                Excel.Range fullRange = exSheet.Range["A5", GetExcelColumnName(colIndex) + lastRow];
                fullRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                fullRange.Columns.AutoFit();
                fullRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ===== LƯU FILE =====
                exBook.SaveAs(saveFile.FileName);
                exBook.Close();
                exApp.Quit();

                // Giải phóng COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(exSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(exBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);

                CustomMessageBox.Show("Xuất file Excel thành công!\nĐã lưu tại: " + saveFile.FileName, "Thông báo");

                // Hỏi mở file Excel
                if (CustomMessageBox.ShowConfirm("Bạn có muốn mở file Excel vừa xuất không?", "Mở file") == DialogResult.Yes)
                {
                    Process.Start(saveFile.FileName);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowWarning("Lỗi khi xuất Excel: " + ex.Message, "Lỗi");
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }
    }
}