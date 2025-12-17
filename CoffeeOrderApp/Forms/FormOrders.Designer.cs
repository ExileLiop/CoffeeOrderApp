using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class FormOrders
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvMenu;
        private DataGridView dgvCart;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnSave;
        private Button btnBack;
        private Label lblTotal;
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
            this.Text = "Створення замовлення";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = ColorTranslator.FromHtml("#3E2723");

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(20)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));

            dgvMenu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackgroundColor = ColorTranslator.FromHtml("#FFF3E0"),
                ForeColor = ColorTranslator.FromHtml("#3E2723"),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvCart = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackgroundColor = ColorTranslator.FromHtml("#FFF3E0"),
                ForeColor = ColorTranslator.FromHtml("#3E2723"),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(0, 50, 0, 0),
                Anchor = AnchorStyles.None
            };

            btnAdd = CreateButton("➕");
            btnRemove = CreateButton("Видалити ❌");
            btnSave = CreateButton("Зберегти");
            btnBack = CreateButton("Назад");

            lblTotal = new Label
            {
                Text = "Сума: 0 грн",
                ForeColor = ColorTranslator.FromHtml("#FFF3E0"),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnRemove);

            mainLayout.Controls.Add(dgvMenu, 0, 0);
            mainLayout.Controls.Add(buttonPanel, 1, 0);
            mainLayout.Controls.Add(dgvCart, 2, 0);

            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                AutoSize = true
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            bottomPanel.Controls.Add(btnBack, 0, 0);

            var rightPanel = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Right,
                AutoSize = true
            };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rightPanel.Controls.Add(btnSave, 0, 0);
            rightPanel.Controls.Add(lblTotal, 0, 1);

            bottomPanel.Controls.Add(rightPanel, 2, 0);

            mainLayout.Controls.Add(bottomPanel, 0, 1);
            mainLayout.SetColumnSpan(bottomPanel, 3);

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
                Width = 120,
                Height = 50,
                Margin = new Padding(10)
            };
        }
    }
}
