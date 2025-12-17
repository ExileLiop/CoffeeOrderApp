using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoffeeOrderApp.Forms
{
    public partial class FormMenuView : Form
    {
        private Form _mainForm;

        public FormMenuView(Form mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            dgvMenu.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.DefaultCellStyle.ForeColor = Color.Black;
            dgvMenu.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
            dgvMenu.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.EnableHeadersVisualStyles = false;

            LoadMenu();
        }

        private void LoadMenu()
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string query = @"SELECT id, name AS 'Назва', category AS 'Категорія', 
                                price AS 'Ціна (грн)', IF(is_available=1,'Є','Немає') AS 'Наявність' 
                                FROM menu_items";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvMenu.DataSource = dt;
                if (dgvMenu.Columns["id"] != null) dgvMenu.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження меню:\n" + ex.Message);
            }
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) dgvMenu.Rows[e.RowIndex].Selected = true;
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть товар для зміни наявності");
                return;
            }

            var row = dgvMenu.SelectedRows[0];
            var idValue = row.Cells["id"].Value;
            if (idValue == null) return;

            int id = Convert.ToInt32(idValue);
            string currentStatus = row.Cells["Наявність"].Value?.ToString() ?? "Немає";
            bool newStatus = currentStatus == "Немає";

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                string sql = "UPDATE menu_items SET is_available=@status WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Статус успішно змінено!");
                LoadMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при зміні статусу:\n" + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _mainForm.Show();
            this.Close();
        }

        private void FormMenuView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mainForm.Show();
        }
    }
}