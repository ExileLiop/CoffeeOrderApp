using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class FormStats
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvStats;
        private Label lblTotal;
        private Button btnBack;
        private TableLayoutPanel mainLayout;
        private FlowLayoutPanel topPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.Text = "Статистика продажів";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = ColorTranslator.FromHtml("#3E2723");

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };
            mainLayout.Controls.Add(topPanel, 0, 0);

            dgvStats = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackgroundColor = ColorTranslator.FromHtml("#FFF3E0"),
                ForeColor = ColorTranslator.FromHtml("#3E2723"),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            mainLayout.Controls.Add(dgvStats, 0, 1);

            lblTotal = new Label
            {
                Text = "Сума: 0 грн",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFF3E0"),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };

            btnBack = new Button
            {
                Text = "Назад",
                BackColor = ColorTranslator.FromHtml("#795548"),
                ForeColor = ColorTranslator.FromHtml("#FFF3E0"),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Width = 120,
                Height = 40,
                Margin = new Padding(10)
            };
            btnBack.Click += new System.EventHandler(this.btnBack_Click);

            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            bottomPanel.Controls.Add(lblTotal, 0, 0);
            bottomPanel.Controls.Add(btnBack, 1, 0);

            mainLayout.Controls.Add(bottomPanel, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private Button CreateButton(string text)
        {
            return new Button
            {
                Text = text,
                BackColor = ColorTranslator.FromHtml("#FF9800"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Width = 150,
                Height = 40,
                Margin = new Padding(10)
            };
        }
    }
}
