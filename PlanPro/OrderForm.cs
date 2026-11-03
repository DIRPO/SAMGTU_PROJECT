using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProductionPlanner
{
    public class OrderForm : Form
    {
        private List<Order> orders;
        private List<Product> products;
        private ListBox lb;
        private TextBox txtCustomer, txtPenalty;
        private ComboBox cboProduct;
        private DateTimePicker dtpDeadline, dtpEarliest;
        private CheckBox chkEarliest;
        private NumericUpDown nudQty;
        private Button btnAdd, btnDel, btnClose;

        public OrderForm(List<Order> orders, List<Product> products)
        {
            this.orders = orders;
            this.products = products;
            this.Text = "Управление заказами";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(28, 28, 35);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Заголовок
            Panel titleBar = new Panel
            {
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(20, 20, 26)
            };

            Label lblTitle = new Label
            {
                Text = "📋 Управление заказами",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(15, 12),
                Size = new Size(300, 30)
            };

            titleBar.Controls.Add(lblTitle);

            // Список заказов
            lb = new ListBox
            {
                Location = new Point(15, 70),
                Size = new Size(655, 200),
                BackColor = Color.FromArgb(35, 35, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11)
            };
            RefreshList();

            // Карточка добавления заказа
            Panel cardPanel = new Panel
            {
                Location = new Point(15, 290),
                Size = new Size(655, 200),
                BackColor = Color.FromArgb(35, 35, 42)
            };
            cardPanel.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRectangle(cardPanel.ClientRectangle, 10))
                    cardPanel.Region = new Region(path);
            };

            // Поля ввода
            int y = 15;
            Label lblCust = new Label { Text = "Заказчик:", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(15, y), Size = new Size(100, 20) };
            txtCustomer = new TextBox { Location = new Point(130, y - 2), Width = 180, Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblProduct = new Label { Text = "Изделие:", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(15, y + 40), Size = new Size(100, 20) };
            cboProduct = new ComboBox { Location = new Point(130, y + 38), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White };
            foreach (Product p in products) cboProduct.Items.Add(p);
            if (cboProduct.Items.Count > 0) cboProduct.SelectedIndex = 0;

            Label lblQty = new Label { Text = "Количество:", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(15, y + 80), Size = new Size(100, 20) };
            nudQty = new NumericUpDown { Location = new Point(130, y + 78), Width = 80, Minimum = 1, Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White };

            Label lblDeadline = new Label { Text = "Срок (до):", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(350, y), Size = new Size(100, 20) };
            dtpDeadline = new DateTimePicker { Location = new Point(460, y - 2), Width = 160, Format = DateTimePickerFormat.Short };

            Label lblPenalty = new Label { Text = "Штраф/день:", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(350, y + 40), Size = new Size(100, 20) };
            txtPenalty = new TextBox { Text = "500", Location = new Point(460, y + 38), Width = 160, Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            chkEarliest = new CheckBox { Text = "Не ранее", Font = new Font("Segoe UI", 9), ForeColor = Color.White, Location = new Point(350, y + 80), Size = new Size(100, 25) };
            dtpEarliest = new DateTimePicker { Location = new Point(460, y + 78), Width = 160, Format = DateTimePickerFormat.Short, Enabled = false };
            chkEarliest.CheckedChanged += (s, e) => dtpEarliest.Enabled = chkEarliest.Checked;

            cardPanel.Controls.AddRange(new Control[] { lblCust, txtCustomer, lblProduct, cboProduct, lblQty, nudQty, lblDeadline, dtpDeadline, lblPenalty, txtPenalty, chkEarliest, dtpEarliest });

            // Кнопки
            btnAdd = new Button
            {
                Text = "➕ Добавить заказ",
                Location = new Point(15, 510),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(0, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            btnDel = new Button
            {
                Text = "🗑 Удалить заказ",
                Location = new Point(230, 510),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(200, 60, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderSize = 0;

            btnClose = new Button
            {
                Text = "✖ Закрыть",
                Location = new Point(470, 510),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(55, 55, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;

            btnAdd.Click += (s, e) => AddOrder();
            btnDel.Click += (s, e) => DeleteOrder();
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(titleBar);
            this.Controls.Add(lb);
            this.Controls.Add(cardPanel);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnDel);
            this.Controls.Add(btnClose);
        }

        private void AddOrder()
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                MessageBox.Show("Введите заказчика", "Ошибка");
                return;
            }
            if (cboProduct.Items.Count == 0 || cboProduct.SelectedItem == null)
            {
                MessageBox.Show("Сначала создайте изделие в разделе 'Изделия'", "Ошибка");
                return;
            }

            double penalty;
            if (!double.TryParse(txtPenalty.Text, out penalty))
            {
                MessageBox.Show("Введите корректное число для штрафа", "Ошибка");
                return;
            }

            Order newOrder = new Order
            {
                Customer = txtCustomer.Text,
                Product = (Product)cboProduct.SelectedItem,
                Qty = (int)nudQty.Value,
                Deadline = dtpDeadline.Value,
                PenaltyPerDay = penalty,
                EarliestStart = chkEarliest.Checked ? dtpEarliest.Value : (DateTime?)null
            };

            orders.Add(newOrder);
            RefreshList();

            txtCustomer.Text = "";
            nudQty.Value = 1;
            txtPenalty.Text = "500";
            chkEarliest.Checked = false;

            MessageBox.Show($"Заказ для {newOrder.Customer} добавлен!", "PlanPro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteOrder()
        {
            if (lb.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Удалить этот заказ?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    orders.RemoveAt(lb.SelectedIndex);
                    RefreshList();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления", "Ошибка");
            }
        }

        private void RefreshList()
        {
            lb.Items.Clear();
            foreach (Order o in orders)
            {
                string earliest = o.EarliestStart.HasValue ? $" (c {o.EarliestStart.Value:dd.MM})" : "";
                lb.Items.Add($"{o.Customer} - {o.Product?.Name}{earliest} до {o.Deadline:dd.MM.yyyy}");
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}