using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class FormOrdersView
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvOrders;
        private Button btnBack;
        private Button btnMarkDone;
        private TableLayoutPanel mainLayout;
        private FlowLayoutPanel buttonPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.Text = "Перегляд замовлень";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = ColorTranslator.FromHtml("#3E2723");

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(20)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));

            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = ColorTranslator.FromHtml("#FFF3E0"),
                ForeColor = Color.Black
            };

            // Цвета заголовков
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvOrders.EnableHeadersVisualStyles = false;

            buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0),
                AutoSize = true
            };

            btnBack = CreateButton("Назад");
            btnMarkDone = CreateButton("Готово");

            btnBack.Click += new System.EventHandler(this.btnBack_Click);
            btnMarkDone.Click += new System.EventHandler(this.btnMarkDone_Click);

            buttonPanel.Controls.Add(btnMarkDone);
            buttonPanel.Controls.Add(btnBack);

            mainLayout.Controls.Add(dgvOrders, 0, 0);
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        private Button CreateButton(string text)
        {
            return new Button
            {
                Text = text,
                BackColor = ColorTranslator.FromHtml("#795548"),
                ForeColor = ColorTranslator.FromHtml("#FFF3E0"),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Width = 150,
                Height = 40,
                Margin = new Padding(10),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }
    }
}