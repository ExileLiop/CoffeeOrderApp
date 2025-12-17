using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeOrderApp.Forms
{
    public partial class FormMenu : Form
    {
        private string _role;

        public FormMenu(string role)
        {
            _role = role;
            InitializeComponent();

            // Скрываем статистику для баристы
            if (_role == "barista")
                btnStats.Visible = false;

            for (int i = 0; i < mainLayout.RowCount; i++)
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

            btnViewMenu.Click += BtnViewMenu_Click;
            btnAddOrder.Click += BtnAddOrder_Click;
            btnViewOrders.Click += BtnViewOrders_Click;
            btnStats.Click += BtnStats_Click;
        }

        private void BtnViewMenu_Click(object? sender, EventArgs e)
        {
            this.Hide();
            FormMenuView view = new FormMenuView(this);
            view.Show();
        }

        private void BtnAddOrder_Click(object? sender, EventArgs e)
        {
            this.Hide();
            FormOrders orderForm = new FormOrders(this);
            orderForm.Show();
        }

        private void BtnViewOrders_Click(object? sender, EventArgs e)
        {
            this.Hide();
            FormOrdersView ordersView = new FormOrdersView(this);
            ordersView.Show();
        }

        private void BtnStats_Click(object? sender, EventArgs e)
        {
            this.Hide();
            FormStats statsForm = new FormStats();
            statsForm.FormClosed += (s, args) => this.Show();
            statsForm.Show();
        }
    }
}