using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProductionPlanner
{
    public class ProductForm : Form
    {
        private List<Product> products;
        private ListBox lbProducts;
        private ListBox lbOps;
        private TextBox txtName, txtOpName, txtOpRes;
        private NumericUpDown nudHours, nudSeq;
        private Product current;
        private Button btnSave, btnAddProd, btnDelProd, btnAddOp, btnDelOp, btnClose;

        public ProductForm(List<Product> products)
        {
            this.products = products;
            this.Text = "Управление изделиями";
            this.Size = new Size(950, 650);
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
                Text = "⚙️ Управление изделиями",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(15, 12),
                Size = new Size(300, 30)
            };

            titleBar.Controls.Add(lblTitle);

            // Левая панель
            Panel leftPanel = new Panel
            {
                Location = new Point(15, 70),
                Size = new Size(250, 480),
                BackColor = Color.FromArgb(35, 35, 42)
            };
            leftPanel.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRectangle(leftPanel.ClientRectangle, 10))
                    leftPanel.Region = new Region(path);
            };

            Label lblProducts = new Label
            {
                Text = "СПИСОК ИЗДЕЛИЙ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 170),
                Location = new Point(15, 15),
                Size = new Size(220, 20)
            };

            lbProducts = new ListBox
            {
                Location = new Point(15, 40),
                Size = new Size(220, 250),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11)
            };
            RefreshProducts();
            lbProducts.SelectedIndexChanged += (s, e) =>
            {
                if (lbProducts.SelectedIndex >= 0)
                {
                    current = products[lbProducts.SelectedIndex];
                    txtName.Text = current.Name;
                    RefreshOps();
                }
            };

            Label lblEditName = new Label
            {
                Text = "Название изделия",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 170),
                Location = new Point(15, 310),
                Size = new Size(220, 20)
            };

            txtName = new TextBox
            {
                Location = new Point(15, 335),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSave = new Button
            {
                Text = "💾 Сохранить",
                Location = new Point(170, 335),
                Size = new Size(65, 30),
                BackColor = Color.FromArgb(0, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => { if (current != null) { current.Name = txtName.Text; RefreshProducts(); } };

            btnAddProd = new Button
            {
                Text = "+ Новое изделие",
                Location = new Point(15, 385),
                Size = new Size(220, 35),
                BackColor = Color.FromArgb(0, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAddProd.FlatAppearance.BorderSize = 0;
            btnAddProd.Click += (s, e) => { products.Add(new Product { Name = "Новое изделие" }); RefreshProducts(); };

            btnDelProd = new Button
            {
                Text = "🗑 Удалить изделие",
                Location = new Point(15, 430),
                Size = new Size(220, 35),
                BackColor = Color.FromArgb(200, 60, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnDelProd.FlatAppearance.BorderSize = 0;
            btnDelProd.Click += (s, e) =>
            {
                if (current != null)
                {
                    if (MessageBox.Show("Удалить изделие?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        products.Remove(current);
                        current = null;
                        RefreshProducts();
                        lbOps.Items.Clear();
                    }
                }
            };

            leftPanel.Controls.AddRange(new Control[] { lblProducts, lbProducts, lblEditName, txtName, btnSave, btnAddProd, btnDelProd });

            // Правая панель
            Panel rightPanel = new Panel
            {
                Location = new Point(280, 70),
                Size = new Size(645, 480),
                BackColor = Color.FromArgb(35, 35, 42)
            };
            rightPanel.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRectangle(rightPanel.ClientRectangle, 10))
                    rightPanel.Region = new Region(path);
            };

            Label lblOps = new Label
            {
                Text = "ТЕХНОЛОГИЧЕСКИЙ ПРОЦЕСС",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 170),
                Location = new Point(15, 15),
                Size = new Size(250, 20)
            };

            lbOps = new ListBox
            {
                Location = new Point(15, 40),
                Size = new Size(300, 300),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 35
            };
            lbOps.DrawItem += LbOps_DrawItem;

            // Форма добавления операции
            int x = 335;
            Label lblOpName = new Label { Text = "Операция", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(x, 15), Size = new Size(280, 20) };
            txtOpName = new TextBox { Location = new Point(x, 38), Size = new Size(280, 30), Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblOpRes = new Label { Text = "Тип ресурса", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(x, 78), Size = new Size(280, 20) };
            txtOpRes = new TextBox { Location = new Point(x, 101), Size = new Size(280, 30), Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblHours = new Label { Text = "Длительность (часы)", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(x, 141), Size = new Size(140, 20) };
            nudHours = new NumericUpDown { Location = new Point(x, 164), Width = 120, Font = new Font("Segoe UI", 11), Minimum = 1, Maximum = 24, BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White };

            Label lblSeq = new Label { Text = "Порядок", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(150, 150, 170), Location = new Point(x + 140, 141), Size = new Size(100, 20) };
            nudSeq = new NumericUpDown { Location = new Point(x + 140, 164), Width = 80, Font = new Font("Segoe UI", 11), Minimum = 1, Maximum = 100, BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.White };

            btnAddOp = new Button
            {
                Text = "➕ Добавить операцию",
                Location = new Point(x, 210),
                Size = new Size(280, 40),
                BackColor = Color.FromArgb(0, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAddOp.FlatAppearance.BorderSize = 0;
            btnAddOp.Click += (s, e) =>
            {
                if (current != null && !string.IsNullOrWhiteSpace(txtOpName.Text))
                {
                    current.Operations.Add(new Operation
                    {
                        Seq = (int)nudSeq.Value,
                        Name = txtOpName.Text,
                        NeedResource = txtOpRes.Text,
                        Hours = (int)nudHours.Value
                    });
                    RefreshOps();
                    txtOpName.Text = "";
                    txtOpRes.Text = "";
                    nudHours.Value = 1;
                    nudSeq.Value = current.Operations.Count + 1;
                }
                else
                {
                    MessageBox.Show("Выберите изделие и введите название операции", "Ошибка");
                }
            };

            btnDelOp = new Button
            {
                Text = "🗑 Удалить операцию",
                Location = new Point(x, 260),
                Size = new Size(280, 40),
                BackColor = Color.FromArgb(200, 60, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnDelOp.FlatAppearance.BorderSize = 0;
            btnDelOp.Click += (s, e) =>
            {
                if (current != null && lbOps.SelectedIndex >= 0)
                {
                    current.Operations.RemoveAt(lbOps.SelectedIndex);
                    RefreshOps();
                }
            };

            rightPanel.Controls.AddRange(new Control[] { lblOps, lbOps, lblOpName, txtOpName, lblOpRes, txtOpRes, lblHours, nudHours, lblSeq, nudSeq, btnAddOp, btnDelOp });

            // Кнопка закрытия
            btnClose = new Button
            {
                Text = "✖ Закрыть",
                Location = new Point(800, 565),
                Size = new Size(125, 40),
                BackColor = Color.FromArgb(55, 55, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(titleBar);
            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);
            this.Controls.Add(btnClose);
        }

        private void LbOps_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Operation op = current.Operations[e.Index];
            e.DrawBackground();

            using (Font nameFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Font infoFont = new Font("Segoe UI", 8, FontStyle.Regular))
            using (Brush nameBrush = new SolidBrush(Color.White))
            using (Brush infoBrush = new SolidBrush(Color.FromArgb(150, 150, 170)))
            {
                e.Graphics.DrawString($"{op.Seq}. {op.Name}", nameFont, nameBrush, 15, e.Bounds.Y + 6);
                e.Graphics.DrawString($"{op.Hours} ч • {op.NeedResource}", infoFont, infoBrush, 15, e.Bounds.Y + 24);
            }
            e.DrawFocusRectangle();
        }

        private void RefreshProducts()
        {
            lbProducts.Items.Clear();
            foreach (Product p in products)
                lbProducts.Items.Add(p.Name);
        }

        private void RefreshOps()
        {
            lbOps.Items.Clear();
            if (current != null)
            {
                foreach (Operation op in current.Operations.OrderBy(o => o.Seq))
                    lbOps.Items.Add(op);
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