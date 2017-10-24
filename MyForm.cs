using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace FormApp
{
    class MyForm : Form
    {
        DataGridView grid1;
        DataGridView grid2;

        public MyForm()
        {
            var table = (new TableLayoutPanel
            {
                AutoScroll = true,
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill
            });
            Controls.Add(table);

            grid1 = (new DataGridView
            {
                Height = 400,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false
            });
            grid1.Columns[0].Name = "Product";
            grid1.Columns[1].Name = "Info";
            grid1.Columns[2].Name = "Price Per Item";
            table.Controls.Add(grid1);
            table.SetColumnSpan(grid1, 2);

            var buttonColumn1 = new DataGridViewButtonColumn
            {
                Text = "Add to cart",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0,25,0,25) }
            };
            grid1.Columns.Insert(3, buttonColumn1);

            var imagesColumn1 = new DataGridViewImageColumn();
            grid1.Columns.Insert(0, imagesColumn1);
            //must fix error handling

            //C:\Users\Joakim\Documents\GitHub\Projekt1\Products.txt
            //C:\Users\Joe\source\repos\Projekt1\Projekt1\Products.txt
            //C:\Users\Jacob\Documents\GitHub\Projekt1\Products.txt
            string path = @"C:\Users\Jacob\Documents\GitHub\Projekt1\Products.txt"; /* products list location  @"";*/
            string[] lines = File.ReadAllLines(path);

            foreach (string x in lines)
            {
                
                string[] parts = x.Split(',');
                if (parts[0].Contains(".jpg") || parts[0].Contains(".png"))
                {
                    Image image = Image.FromFile(parts[0]);
                    Bitmap bitmap = new Bitmap(image, new Size(75, 75));
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(bitmap, product, info, price);
                }
                else
                {
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(null, product, info, price);
                }
                
            }
            foreach(DataGridViewRow rows in grid1.Rows)
            {
                rows.Height = 75;
            }

            grid2 = (new DataGridView
            {
                Height = 300,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false
            });
            grid2.Columns[0].Name = "Product";
            grid2.Columns[1].Name = "Info";
            grid2.Columns[2].Name = "Quantity";
            grid2.Columns[3].Name = "Total Price";
            table.Controls.Add(grid2);
            table.SetColumnSpan(grid2, 2);

            var buttonColumn2 = new DataGridViewButtonColumn
            {
                Text = "Remove from cart",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0, 25, 0, 25) }
            };
            grid2.Columns.Insert(4, buttonColumn2);

            var imagesColumn2 = new DataGridViewImageColumn();
            grid2.Columns.Insert(0, imagesColumn2);

            grid1.CellContentClick += grid1_CellContentClicked;
            grid2.CellContentClick += grid2_CellContentClicked;

            Label summaryLabel = new Label
            {
                Text = "Total price below",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            table.Controls.Add(summaryLabel);

            var textBox = new TextBox
            {

            };
            table.Controls.Add(textBox);
            table.SetColumnSpan(textBox, 4);

        }

        private void grid1_CellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            int quantity;
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {

                var image = grid1.Rows[e.RowIndex].Cells[0].Value;
                var name = grid1.Rows[e.RowIndex].Cells[1].Value;
                var info = grid1.Rows[e.RowIndex].Cells[2].Value;
                quantity = 1;
                var price = grid1.Rows[e.RowIndex].Cells[3].Value;

                foreach (DataGridViewRow rows in grid2.Rows)
                {
                    if (name == rows.Cells[1].Value && info == rows.Cells[2].Value)
                    {
                        double y = Convert.ToDouble(price);
                        int x = Convert.ToInt16(rows.Cells[3].Value);
                        x++;
                        quantity = x;
                        price = y * quantity;
                        grid2.Rows.RemoveAt(rows.Index);
                    }
                }
                grid2.Rows.Add(image, name, info, quantity, price);
                quantity = 1;
            }
            
            foreach (DataGridViewRow rows in grid2.Rows)
            {
                rows.Height = 75;
            }
        }

        private void grid2_CellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            double singlePrice;
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                double y = Convert.ToDouble(grid2.Rows[e.RowIndex].Cells[4].Value);
                int x = Convert.ToInt32(grid2.Rows[e.RowIndex].Cells[3].Value);
                if(x > 1)
                {
                    singlePrice = y / x;
                    x--;
                    grid2.Rows[e.RowIndex].Cells[3].Value = x;
                    grid2.Rows[e.RowIndex].Cells[4].Value = y - singlePrice;
                }
                else
                {
                    grid2.Rows.RemoveAt(e.RowIndex);
                }
            }
 
        }
    }
}
