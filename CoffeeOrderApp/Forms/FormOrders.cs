using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoffeeOrderApp.Forms
{
    public partial class FormOrders : Form
    {
        private Form _mainForm;
        private DataTable cartTable = new DataTable();

        public FormOrders(Form mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            dgvMenu.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.DefaultCellStyle.ForeColor = Color.Black;
            dgvMenu.RowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMenu.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
            dgvMenu.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvMenu.EnableHeadersVisualStyles = false;

            dgvCart.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvCart.DefaultCellStyle.ForeColor = Color.Black;
            dgvCart.RowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvCart.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#795548");
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFF3E0");
            dgvCart.EnableHeadersVisualStyles = false;

            LoadMenu();
            InitCart();
            UpdateTotal();

            btnAdd.Click += btnAdd_Click;
            btnRemove.Click += btnRemove_Click;
            btnSave.Click += btnSave_Click;
            btnBack.Click += (s, e) => { _mainForm.Show(); this.Close(); };
        }

        private void LoadMenu()
        {
            using var conn = Database.GetConnection();
            conn.Open();
            string sql = "SELECT id, name AS 'Назва', price AS 'Ціна' FROM menu_items WHERE is_available = 1";
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvMenu.DataSource = dt;
            if (dgvMenu.Columns["id"] != null) dgvMenu.Columns["id"].Visible = false;
        }

        private void InitCart()
        {
            cartTable.Columns.Add("id", typeof(int));
            cartTable.Columns.Add("Назва", typeof(string));
            cartTable.Columns.Add("Ціна", typeof(decimal));
            cartTable.Columns.Add("Кількість", typeof(int));
            cartTable.Columns.Add("Сума", typeof(decimal));
            dgvCart.DataSource = cartTable;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0) return;
            var row = dgvMenu.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id"].Value);
            string name = row.Cells["Назва"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["Ціна"].Value);

            foreach (DataRow r in cartTable.Rows)
            {
                if ((int)r["id"] == id)
                {
                    int qty = (int)r["Кількість"] + 1;
                    r["Кількість"] = qty;
                    r["Сума"] = qty * price;
                    UpdateTotal();
                    return;
                }
            }

            cartTable.Rows.Add(id, name, price, 1, price);
            UpdateTotal();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0) return;
            dgvCart.Rows.RemoveAt(dgvCart.SelectedRows[0].Index);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow r in cartTable.Rows)
                total += Convert.ToDecimal(r["Сума"]);
            lblTotal.Text = $"Сума: {total} грн";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Кошик порожній");
                return;
            }

            using var conn = Database.GetConnection();
            conn.Open();
            var tx = conn.BeginTransaction();

            try
            {
                decimal total = 0;
                foreach (DataRow r in cartTable.Rows)
                    total += Convert.ToDecimal(r["Сума"]);

                var cmd = new MySqlCommand(
                    "INSERT INTO orders (status, total) VALUES ('Готується', @total)",
                    conn, tx);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.ExecuteNonQuery();
                int orderId = (int)cmd.LastInsertedId;

                foreach (DataRow r in cartTable.Rows)
                {
                    var itemCmd = new MySqlCommand(
                        "INSERT INTO order_items (order_id, menu_item_id, quantity, price) VALUES (@o, @m, @q, @p)",
                        conn, tx);
                    itemCmd.Parameters.AddWithValue("@o", orderId);
                    itemCmd.Parameters.AddWithValue("@m", r["id"]);
                    itemCmd.Parameters.AddWithValue("@q", r["Кількість"]);
                    itemCmd.Parameters.AddWithValue("@p", r["Ціна"]);
                    itemCmd.ExecuteNonQuery();
                }

                tx.Commit();

                var result = MessageBox.Show("Бажаєте роздрукувати чек?", "Чек", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    PrintReceipt(orderId, cartTable.Copy(), total);
                }

                cartTable.Clear();
                UpdateTotal();
                MessageBox.Show("Замовлення успішно збережено!");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Помилка:\n" + ex.Message);
            }
        }

        private void PrintReceipt(int orderId, DataTable cart, decimal total)
        {
            string receiptText = "";
            receiptText += "=== COFFEE ORDER ===\n";
            receiptText += $"Замовлення № {orderId}\n";
            receiptText += DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            receiptText += "--------------------\n";

            foreach (DataRow r in cart.Rows)
            {
                string line = $"{r["Назва"],-15} x{r["Кількість"],-3} {r["Сума"],6} грн";
                receiptText += line + "\n";
            }

            receiptText += "--------------------\n";
            receiptText += $"ВСЬОГО: {total} грн\n";
            receiptText += "Дякуємо за покупку ☕\n";

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, e) =>
            {
                e.Graphics.DrawString(receiptText, new Font("Consolas", 10), Brushes.Black, 20, 20);
            };

            if (printDoc.PrinterSettings.IsValid)
                printDoc.Print();
            else
                MessageBox.Show("Принтер не знайдено. Перевірте налаштування.");
        }
    }
}
