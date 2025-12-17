using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoffeeOrderApp.Forms
{
    public partial class FormOrdersView : Form
    {
        private Form _mainForm;

        public FormOrdersView(Form mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            // Настройка цветов DataGridView
            dgvOrders.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvOrders.DefaultCellStyle.ForeColor = Color.Black;
            dgvOrders.RowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvOrders.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvOrders.EnableHeadersVisualStyles = false;

            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string query = @"
                    SELECT o.id AS 'ID Замовлення', o.order_date AS 'Дата', o.status AS 'Статус', 
                           GROUP_CONCAT(mi.name, ' x', oi.quantity) AS 'Замовлені товари', o.total AS 'Сума'
                    FROM orders o
                    LEFT JOIN order_items oi ON o.id = oi.order_id
                    LEFT JOIN menu_items mi ON oi.menu_item_id = mi.id
                    WHERE o.status = 'Готується'
                    GROUP BY o.id, o.order_date, o.status
                    ORDER BY o.order_date DESC;
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvOrders.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні замовлень:\n" + ex.Message);
            }
        }

        private void btnMarkDone_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть замовлення для завершення");
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["ID Замовлення"].Value);

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("UPDATE orders SET status='Завершено' WHERE id=@id", conn);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Замовлення завершено!");
                LoadOrders();
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

        private void FormOrdersView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mainForm.Show();
        }
    }
}