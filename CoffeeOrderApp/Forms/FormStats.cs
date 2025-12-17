using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    public partial class FormStats : Form
    {
        private ComboBox cbPeriod;
        private DateTimePicker dtStart;
        private DateTimePicker dtEnd;
        private Button btnExportExcel;
        private Button btnExportPDF;

        public FormStats()
        {
            InitializeComponent();
            InitCustomControls();
            LoadStats(DateTime.Today, DateTime.Today);
        }

        private void InitCustomControls()
        {
            cbPeriod = new ComboBox
            {
                Items = { "День", "Неделя", "Месяц", "Квартал", "Год", "Произвольный" },
                SelectedIndex = 0,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150
            };
            cbPeriod.SelectedIndexChanged += (s, e) =>
            {
                bool custom = cbPeriod.SelectedItem.ToString() == "Произвольный";
                dtStart.Enabled = dtEnd.Enabled = custom;
                ApplyFilter();
            };
            topPanel.Controls.Add(cbPeriod);

            dtStart = new DateTimePicker { Enabled = false, Width = 120 };
            dtEnd = new DateTimePicker { Enabled = false, Width = 120 };
            topPanel.Controls.Add(dtStart);
            topPanel.Controls.Add(dtEnd);

            dtStart.ValueChanged += (s, e) => ApplyFilter();
            dtEnd.ValueChanged += (s, e) => ApplyFilter();

            btnExportExcel = new Button
            {
                Text = "Експорт в Excel",
                Width = 150,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#4CAF50"),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnExportPDF = new Button
            {
                Text = "Експорт в PDF",
                Width = 150,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#4CAF50"),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnExportExcel.Click += BtnExportExcel_Click;
            btnExportPDF.Click += BtnExportPDF_Click;
            topPanel.Controls.Add(btnExportExcel);
            topPanel.Controls.Add(btnExportPDF);
        }

        private void ApplyFilter()
        {
            string period = cbPeriod.SelectedItem.ToString();
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today;

            switch (period)
            {
                case "День":
                    start = end = DateTime.Today;
                    break;
                case "Неделя":
                    start = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    end = start.AddDays(6);
                    break;
                case "Месяц":
                    start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                    break;
                case "Квартал":
                    int currentQuarter = (DateTime.Today.Month - 1) / 3 + 1;
                    start = new DateTime(DateTime.Today.Year, (currentQuarter - 1) * 3 + 1, 1);
                    end = start.AddMonths(3).AddDays(-1);
                    break;
                case "Год":
                    start = new DateTime(DateTime.Today.Year, 1, 1);
                    end = new DateTime(DateTime.Today.Year, 12, 31);
                    break;
                case "Произвольный":
                    start = dtStart.Value.Date;
                    end = dtEnd.Value.Date;
                    break;
            }

            LoadStats(start, end);
        }

        private void LoadStats(DateTime start, DateTime end)
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string query = @"
                    SELECT mi.name AS 'Товар',
                           SUM(oi.quantity) AS 'Продано',
                           mi.price AS 'Ціна',
                           SUM(oi.quantity * mi.price) AS 'Сума'
                    FROM orders o
                    JOIN order_items oi ON o.id = oi.order_id
                    JOIN menu_items mi ON oi.menu_item_id = mi.id
                    WHERE o.status = 'Завершено' 
                          AND DATE(o.order_date) BETWEEN @start AND @end
                    GROUP BY mi.name, mi.price;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                DataTable dt = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dt);
                dgvStats.DataSource = dt;

                dgvStats.BackgroundColor = ColorTranslator.FromHtml("#FFF3E0");
                dgvStats.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
                dgvStats.DefaultCellStyle.ForeColor = Color.Black;
                dgvStats.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
                dgvStats.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
                dgvStats.EnableHeadersVisualStyles = false;

                decimal total = 0;
                foreach (DataRow r in dt.Rows)
                    total += Convert.ToDecimal(r["Сума"]);

                lblTotal.Text = $"Сума: {total} грн ({start:dd.MM.yyyy} - {end:dd.MM.yyyy})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка:\n" + ex.Message);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvStats.Rows.Count == 0) return;

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = "Stats.xlsx"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using var wb = new XLWorkbook();
                wb.Worksheets.Add(dgvStats.DataSource as DataTable, "Статистика");
                wb.SaveAs(saveFile.FileName);
                MessageBox.Show("Експорт успішно виконано!");
            }
        }

        private void BtnExportPDF_Click(object sender, EventArgs e)
        {
            if (dgvStats.Rows.Count == 0) return;

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "PDF File|*.pdf",
                FileName = "Stats.pdf"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                var pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, new FileStream(saveFile.FileName, FileMode.Create));
                pdfDoc.Open();

                PdfPTable table = new PdfPTable(dgvStats.Columns.Count);

                foreach (DataGridViewColumn col in dgvStats.Columns)
                    table.AddCell(new iTextSharp.text.Phrase(
                        col.HeaderText,
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)
                    ));

                foreach (DataGridViewRow row in dgvStats.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                        table.AddCell(new iTextSharp.text.Phrase(
                            cell.Value?.ToString() ?? "",
                            new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)
                        ));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                MessageBox.Show("Експорт у PDF виконано!");
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => this.Close();
    }
}