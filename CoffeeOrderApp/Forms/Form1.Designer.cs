using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblUsername;
        private Label lblPassword;
        private Button btnLogin;
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel panelInputs;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.Text = "Вхід в CoffeeOrderApp";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 300);
            this.MinimumSize = new Size(500, 300);
            this.BackColor = ColorTranslator.FromHtml("#3E2723");

            mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1 };

            panelInputs = new TableLayoutPanel
            {
                BackColor = ColorTranslator.FromHtml("#FFF3E0"),
                ColumnCount = 1,
                RowCount = 5,
                Dock = DockStyle.None,
                AutoSize = true,
                Padding = new Padding(20)
            };

            panelInputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelInputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelInputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelInputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelInputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblUsername = new Label
            {
                Text = "Логін:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#3E2723"),
                AutoSize = true
            };
            txtUsername = new TextBox { Font = new Font("Segoe UI", 12F), Width = 260 };

            lblPassword = new Label
            {
                Text = "Пароль:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#3E2723"),
                AutoSize = true
            };
            txtPassword = new TextBox { Font = new Font("Segoe UI", 12F), Width = 260, PasswordChar = '*' };

            btnLogin = new Button
            {
                Text = "Вхід",
                BackColor = ColorTranslator.FromHtml("#C68642"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Width = 140,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            panelInputs.Controls.Add(lblUsername);
            panelInputs.Controls.Add(txtUsername);
            panelInputs.Controls.Add(lblPassword);
            panelInputs.Controls.Add(txtPassword);
            panelInputs.Controls.Add(btnLogin);

            panelInputs.Location = new Point(
                (this.ClientSize.Width - panelInputs.PreferredSize.Width) / 2,
                (this.ClientSize.Height - panelInputs.PreferredSize.Height) / 2
            );
            panelInputs.Anchor = AnchorStyles.None;

            this.Controls.Add(panelInputs);
        }
    }
}