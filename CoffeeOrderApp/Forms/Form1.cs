using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoffeeOrderApp.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text ?? "";
            string password = txtPassword.Text ?? "";

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string query = @"
                    SELECT u.id, u.username, r.name AS role
                    FROM users u
                    JOIN roles r ON u.role_id = r.id
                    WHERE u.username=@user AND u.password=@pass AND u.is_active=1;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string role = reader["role"].ToString() ?? "Unknown";
                    MessageBox.Show($"Вхід успішний! Роль: {role}");

                    FormMenu menuForm = new FormMenu(role.ToLower());
                    menuForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неправильний логін або пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при вході: " + ex.Message);
            }
        }
    }
}