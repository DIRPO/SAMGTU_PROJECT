using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace ProductionPlanner
{
    public class MainForm : Form
    {
        private List<Resource> resources = new List<Resource>();
        private List<Product> products = new List<Product>();
        private List<Order> orders = new List<Order>();

        private DataGridView dgv;
        private Label lblPenalty, lblLogo, lblSubtitle;
        private Button btnResources, btnProducts, btnOrders, btnPlan;

        public MainForm()
        {
            this.Text = "PlanPro";
            this.Size = new Size(1300, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(32, 32, 38);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ========== ФИРМЕННАЯ НАДПИСЬ PlanPro ==========

            // Большой логотип PlanPro
            lblLogo = new Label
            {
                Text = "PlanPro",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(15, 12),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // ========== КНОПКИ ==========

            int y = 15;
            btnResources = new Button
            {
                Text = "📦 Ресурсы",
                Location = new Point(220, y + 5),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnProducts = new Button
            {
                Text = "⚙️ Изделия",
                Location = new Point(360, y + 5),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnOrders = new Button
            {
                Text = "📋 Заказы",
                Location = new Point(500, y + 5),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnPlan = new Button
            {
                Text = "▶️ Построить расписание",
                Location = new Point(650, y + 5),
                Size = new Size(180, 40),
                BackColor = Color.FromArgb(0, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnResources.Click += (s, e) => { new ResourceForm(resources).ShowDialog(); };
            btnProducts.Click += (s, e) => { new ProductForm(products).ShowDialog(); };
            btnOrders.Click += (s, e) => { new OrderForm(orders, products).ShowDialog(); };
            btnPlan.Click += (s, e) => BuildSchedule();

            // Штраф
            lblPenalty = new Label
            {
                Text = "💰 Общий штраф: 0 руб.",
                Location = new Point(860, y + 12),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 100, 100)
            };

            // ========== ТАБЛИЦА РАСПИСАНИЯ ==========

            dgv = new DataGridView
            {
                Location = new Point(15, 75),
                Size = new Size(1260, 635),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(40, 40, 48),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                RowTemplate = { Height = 35 },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(38, 38, 45) },
                EnableHeadersVisualStyles = false
            };

            // Стиль заголовков таблицы
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 25, 30);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(0, 150, 255);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 40;

            // Стиль ячеек
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 52);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 230);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 100, 180);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Добавляем все элементы на форму
            this.Controls.AddRange(new Control[] {
                lblLogo,
                lblSubtitle,
                btnResources,
                btnProducts,
                btnOrders,
                btnPlan,
                lblPenalty,
                dgv
            });
        }

        private void BuildSchedule()
        {
            // Проверка наличия ресурсов
            if (resources.Count == 0)
            {
                MessageBox.Show("Нет ресурсов! Добавьте ресурсы в разделе 'Ресурсы'.", "PlanPro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка наличия изделий
            if (products.Count == 0)
            {
                MessageBox.Show("Нет изделий! Добавьте изделия в разделе 'Изделия'.", "PlanPro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка наличия заказов
            if (orders.Count == 0)
            {
                MessageBox.Show("Нет заказов! Добавьте заказы в разделе 'Заказы'.", "PlanPro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Очистка таблицы
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            // Добавление колонок
            dgv.Columns.Add("Start", "📅 Начало");
            dgv.Columns.Add("End", "⏰ Конец");
            dgv.Columns.Add("Resource", "🔧 Ресурс");
            dgv.Columns.Add("Order", "🏭 Заказчик");
            dgv.Columns.Add("Operation", "⚙️ Операция");
            dgv.Columns.Add("Late", "⚠️ Просрочка (дн)");
            dgv.Columns.Add("Penalty", "💰 Штраф (руб)");

            // Построение расписания
            Scheduler scheduler = new Scheduler(resources, orders);
            List<ScheduleItem> schedule = scheduler.Build();

            double totalPenalty = 0;

            if (schedule.Count == 0)
            {
                MessageBox.Show("Расписание пусто!\n\nПроверьте, что:\n1. У ресурсов и операций совпадают типы\n2. Для каждой операции есть ресурс\n3. Заказы привязаны к изделиям",
                    "PlanPro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (ScheduleItem item in schedule)
            {
                dgv.Rows.Add(
                    item.Start.ToString("dd.MM HH:mm"),
                    item.End.ToString("dd.MM HH:mm"),
                    item.Resource?.Name ?? "—",
                    item.Order.Customer,
                    item.Operation.Name,
                    item.DaysLate,
                    item.Penalty
                );
                totalPenalty += item.Penalty;

                // Подсветка просроченных строк
                if (item.IsLate)
                {
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(80, 40, 40);
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.FromArgb(255, 150, 150);
                }
            }

            lblPenalty.Text = $"💰 Общий штраф: {totalPenalty:F0} руб.";
        }
    }
}