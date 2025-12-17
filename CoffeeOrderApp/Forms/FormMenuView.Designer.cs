using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class FormMenuView
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvMenu;
        private Button btnBack;
        private Button btnChangeStatus;
        private TableLayoutPanel mainLayout;
        private FlowLayoutPanel buttonPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            dgvMenu = new DataGridView();
            btnBack = new Button();
            btnChangeStatus = new Button();
            mainLayout = new TableLayoutPanel();
            buttonPanel = new FlowLayoutPanel();

            SuspendLayout();

            // mainLayout
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainLayout.Padding = new Padding(20);
            mainLayout.BackColor = ColorTranslator.FromHtml("#3E2723");

            // dgvMenu
            dgvMenu.Dock = DockStyle.Fill;
            dgvMenu.ReadOnly = true;
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenu.BackgroundColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.CellClick += new DataGridViewCellEventHandler(this.dgvMenu_CellClick);

            // buttonPanel
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.AutoSize = true;
            buttonPanel.Padding = new Padding(0, 10, 0, 0);

            // btnBack
            btnBack.Text = "Назад";
            btnBack.Size = new Size(120, 40);
            btnBack.BackColor = ColorTranslator.FromHtml("#795548");
            btnBack.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // btnChangeStatus
            btnChangeStatus.Text = "Змінити наявність";
            btnChangeStatus.Size = new Size(180, 40);
            btnChangeStatus.BackColor = ColorTranslator.FromHtml("#795548");
            btnChangeStatus.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            btnChangeStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnChangeStatus.Click += new System.EventHandler(this.btnChangeStatus_Click);

            buttonPanel.Controls.Add(btnBack);
            buttonPanel.Controls.Add(btnChangeStatus);

            mainLayout.Controls.Add(dgvMenu, 0, 0);
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            // Form
            this.Text = "Перегляд меню";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(mainLayout);
            this.FormClosed += new FormClosedEventHandler(this.FormMenuView_FormClosed);

            ResumeLayout(false);
        }
    }
}