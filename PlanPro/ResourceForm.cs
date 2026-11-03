using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProductionPlanner
{
    public class ResourceForm : Form
    {
        private List<Resource> resources;
        private ListBox lb;
        private TextBox txtName, txtType;
        private Button btnAdd, btnDel, btnClose;

        public ResourceForm(List<Resource> resources)
        {
            this.resources = resources;
            this.Text = "Управление ресурсами";
            this.Size = new Size(550, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(28, 28, 35);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Заголовок формы
            Panel titleBar = new Panel
            {
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(20, 20, 26)
            };

            Label lblTitle = new Label
            {
                Text = "📦 Управление ресурсами",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(15, 12),
                Size = new Size(300, 30)
            };

            titleBar.Controls.Add(lblTitle);

            // Список ресурсов
            lb = new ListBox
            {
                Location = new Point(15, 70),
                Size = new Size(505, 320),
                BackColor = Color.FromArgb(35, 35, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11),
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 40
            };
            lb.DrawItem += Lb_DrawItem;
            RefreshList();

            // Карточка добавления
            Panel cardPanel = new Panel
            {
                Location = new Point(15, 410),
                Size = new Size(505, 90),
                BackColor = Color.FromArgb(35, 35, 42)
            };
            cardPanel.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRectangle(cardPanel.ClientRectangle, 10))
                {
                    cardPanel.Region = new Region(path);
                }
            };

            Label lblName = new Label
            {
                Text = "Название",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 170),
                Location = new Point(15, 12),
                Size = new Size(200, 20)
            };

            txtName = new TextBox
            {
                Location = new Point(15, 35),
                Size = new Size(220, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblType = new Label
            {
                Text = "Тип",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 170),
                Location = new Point(250, 12),
                Size = new Size(200, 20)
            };

            txtType = new TextBox
            {
                Location = new Point(250, 35),
                Size = new Size(220, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            cardPanel.Controls.AddRange(new Control[] { lblName, txtName, lblType, txtType });

            // Кнопки
            btnAdd = new Button
            {
                Text = "➕ Добавить",
                Location = new Point(15, 515),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(0, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            btnDel = new Button
            {
                Text = "🗑 Удалить",
                Location = new Point(185, 515),
                Size = new Size(160, 40),
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
                Location = new Point(355, 515),
                Size = new Size(165, 40),
                BackColor = Color.FromArgb(55, 55, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;

            btnAdd.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtType.Text))
                {
                    resources.Add(new Resource { Name = txtName.Text, Type = txtType.Text });
                    RefreshList();
                    txtName.Text = "";
                    txtType.Text = "";
                }
                else
                {
                    MessageBox.Show("Заполните оба поля", "Ошибка");
                }
            };

            btnDel.Click += (s, e) =>
            {
                if (lb.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Удалить ресурс?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        resources.RemoveAt(lb.SelectedIndex);
                        RefreshList();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите ресурс для удаления", "Ошибка");
                }
            };

            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(titleBar);
            this.Controls.Add(lb);
            this.Controls.Add(cardPanel);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnDel);
            this.Controls.Add(btnClose);
        }

        private void Lb_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Resource res = resources[e.Index];
            e.DrawBackground();

            using (Font nameFont = new Font("Segoe UI", 11, FontStyle.Bold))
            using (Font typeFont = new Font("Segoe UI", 9, FontStyle.Regular))
            using (Brush nameBrush = new SolidBrush(Color.White))
            using (Brush typeBrush = new SolidBrush(Color.FromArgb(150, 150, 170)))
            {
                e.Graphics.DrawString(res.Name, nameFont, nameBrush, 15, e.Bounds.Y + 8);
                e.Graphics.DrawString(res.Type, typeFont, typeBrush, 15, e.Bounds.Y + 28);
            }
            e.DrawFocusRectangle();
        }

        private void RefreshList()
        {
            lb.Items.Clear();
            foreach (Resource r in resources)
                lb.Items.Add(r);
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