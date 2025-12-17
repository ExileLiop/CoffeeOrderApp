using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class FormMenu
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel mainLayout;
        private Button btnViewMenu;
        private Button btnAddOrder;
        private Button btnViewOrders;
        private Button btnStats;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainLayout = new TableLayoutPanel();
            this.btnViewMenu = new Button();
            this.btnAddOrder = new Button();
            this.btnViewOrders = new Button();
            this.btnStats = new Button();

            this.SuspendLayout();

            // mainLayout
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.RowCount = 4;
            this.mainLayout.Dock = DockStyle.Fill;
            this.mainLayout.Padding = new Padding(20);
            this.mainLayout.BackColor = Color.FromArgb(245, 245, 220);

            // Добавляем кнопки
            this.mainLayout.Controls.Add(this.btnViewMenu, 0, 0);
            this.mainLayout.Controls.Add(this.btnAddOrder, 0, 1);
            this.mainLayout.Controls.Add(this.btnViewOrders, 0, 2);
            this.mainLayout.Controls.Add(this.btnStats, 0, 3);

            ConfigureButton(btnViewMenu, "Перегляд меню");
            ConfigureButton(btnAddOrder, "Створити замовлення");
            ConfigureButton(btnViewOrders, "Перегляд замовлень");
            ConfigureButton(btnStats, "Статистика");

            this.ClientSize = new Size(400, 400);
            this.Controls.Add(this.mainLayout);
            this.Text = "Головне меню";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void ConfigureButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Dock = DockStyle.Fill;
            btn.Margin = new Padding(10);
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn.BackColor = Color.FromArgb(200, 143, 82);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }
    }
}