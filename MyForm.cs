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
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill
            });
            Controls.Add(table);

            grid1 = (new DataGridView
            {
                Height = 500,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false
            });
            table.Controls.Add(grid1);
            table.SetColumnSpan(grid1, 2);

            var buttonColumn1 = new DataGridViewButtonColumn
            {
                Text = "Add to cart",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0,25,0,25) }
            };
            grid1.Columns.Insert(3, buttonColumn1);

            var imagesColumn = new DataGridViewImageColumn();
            grid1.Columns.Insert(0, imagesColumn);
            //must fix error handling
            
            string path = @"C:\Users\Joe\Desktop\textfolder\Products.txt"; /* products list location  @"";*/
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
                ColumnCount = 5,
                Dock = DockStyle.Fill
            });
            table.Controls.Add(grid2);
            table.SetColumnSpan(grid2, 2);

            var buttonColumn2 = new DataGridViewButtonColumn
            {
                Text = "Remove from cart",
                UseColumnTextForButtonValue = true
            };
            grid2.Columns.Insert(5, buttonColumn2);

        }

        private void ClickedEventHandler(object sender, EventArgs e)
        {

        }
    }
}
