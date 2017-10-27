using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Projekt1
{
    class MyForm : Form
    {
        public DataGridView grid1;
        public DataGridView grid2;
        private List<Product> products;
        private bool firstTime = true;
        public double result { get; set; }
 
        public MyForm()
        {
            products = new List<Product> { };
            var table = (new TableLayoutPanel
            {
                AutoScroll = true,
                ColumnCount = 4,
                RowCount = 4,
                Dock = DockStyle.Fill
            });
            Controls.Add(table);

            var table1 = (new TableLayoutPanel
            {
                Height = 350,
                Dock = DockStyle.Bottom
            });
            Controls.Add(table1);

            var buttonOrder = new Button
            {
                Text = "Place Order"
            };
            table1.Controls.Add(buttonOrder);

            var buttonSubTotal = new Button
            {
                Text = "SubTotal"
            };
            table1.Controls.Add(buttonSubTotal);

            grid1 = (new DataGridView
            {
                Height = 445,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                //sets the grid to a static size that the user can not change.
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false
            });
            grid1.Columns[0].Name = "Product";
            grid1.Columns[1].Name = "Info";
            grid1.Columns[2].Name = "Price Per Item";
            table.Controls.Add(grid1);
            table.SetRowSpan(grid1, 2);
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            //creates a button column in grid1
            var buttonColumn1 = new DataGridViewButtonColumn
            {
                Text = "Add to cart",
                //uses Text as the button's text ( which is seperate without this )
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0,25,0,25) }
            };
            grid1.Columns.Insert(3, buttonColumn1);
            //creates image column in grid1
            var imagesColumn1 = new DataGridViewImageColumn();
            grid1.Columns.Insert(0, imagesColumn1);

            //users' file paths
            //C:\Users\Joakim\Documents\GitHub\Projekt1\Products.txt
            //C:\Users\Joe\source\repos\Projekt1\Projekt1\Products.txt
            //C:\Users\Jacob\Documents\GitHub\Projekt1\Products.txt
            string path = @"C:\Users\Jacob\Documents\GitHub\Projekt1\Products.txt"; /* products list location  @"";*/
            string[] lines = File.ReadAllLines(path);

            //loop to grab values from a text file to create Products or Items.
            foreach (string x in lines)
            {
                //if file has image it will proceed as normal, otherwise sets the first argument in the row to null. 
                //could fix instead to handle the error of missing image file. but then all items will have to have an image.
                string[] parts = x.Split(',');
                if (parts[0].Contains(".jpg") || parts[0].Contains(".png"))
                {
                    Image image = Image.FromFile(parts[0]);
                    Bitmap bitmap = new Bitmap(image, new Size(75, 75));
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(bitmap, product, info, "$" + price);
                }
                else
                {
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(null, product, info, "$" + price);
                }
                
            }
            //resizing of each rows height to a static amount and the info column's width.
            foreach(DataGridViewRow rows in grid1.Rows)
            {
                rows.Height = 75;
                grid1.Columns[2].Width = 175;
            }
            //makes the info column's text multilined
            grid1.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            grid2 = (new DataGridView
            {
                Height = 445, 
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                //sets the grid to a static size that the user can not change.
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false
            });
            //grid2 headers
            grid2.Columns[0].Name = "Product";
            grid2.Columns[1].Name = "Info";
            grid2.Columns[2].Name = "Quantity";
            grid2.Columns[3].Name = "Total Price";
            table.Controls.Add(grid2);
            table.SetRowSpan(grid2, 2);
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
     
            //makes the info column's text multilined
            grid2.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //set the button column for the cart grid (grid2)
            var buttonColumn2 = new DataGridViewButtonColumn
            {
                Text = "Remove from cart",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0, 25, 0, 25) }
            };
            grid2.Columns.Insert(4, buttonColumn2);
            //sets image column for the cart grid (grid2)
            var imagesColumn2 = new DataGridViewImageColumn();
            grid2.Columns.Insert(0, imagesColumn2);

            grid1.CellContentClick += Grid1_CellContentClicked;
            grid2.CellContentClick += Grid2_CellContentClicked;
            buttonOrder.Click += ClickedEventHandler1;
            buttonSubTotal.Click += ClickedEventHandler2;
        }

        //click methods for adding and removing items to the cart grid (grid2)
        //private void Grid1_CellContentClicked(object sender, DataGridViewCellEventArgs e)
        //{
        //    int quantity;
        //    var senderGrid = (DataGridView)sender;

        //    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        //    {

        //        var image = grid1.Rows[e.RowIndex].Cells[0].Value;
        //        var name = grid1.Rows[e.RowIndex].Cells[1].Value;
        //        var info = grid1.Rows[e.RowIndex].Cells[2].Value;
        //        quantity = 1;
        //        var price = grid1.Rows[e.RowIndex].Cells[3].Value;
        //        string priceString = Convert.ToString(price);
        //        priceString = priceString.Remove(0, 1);
        //        price = priceString;
                
        //        foreach (DataGridViewRow rows in grid2.Rows)
        //        {
        //            //if product exists
        //            if (name == rows.Cells[1].Value && info == rows.Cells[2].Value)
        //            {
        //                double y = Convert.ToDouble(priceString);
        //                int x = Convert.ToInt16(rows.Cells[3].Value);
        //                x++;
        //                quantity = x;
        //                price = y * quantity;
        //                grid2.Rows.RemoveAt(rows.Index);
        //            }
        //        }
        //        grid2.Rows.Add(image, name, info, quantity,"$" + price);
        //    }

        //    //resizing of each rows height to a static amount and the info column's width.
        //    //must occur after button click because the rows do not exist before the click event.
        //    foreach (DataGridViewRow rows in grid2.Rows)
        //    {
        //        rows.Height = 75;
        //        grid2.Columns[2].Width = 175;
        //        grid2.Columns[3].Width = 50;
        //        grid2.Columns[4].Width = 70;
        //    }
        //}


        private void Grid1_CellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            bool productBool = true;
            bool productTime = false;
            int quantity = 1;
            var senderGrid = (DataGridView)sender;
            if(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var image = grid1.Rows[e.RowIndex].Cells[0].Value;
                var name = grid1.Rows[e.RowIndex].Cells[1].Value;
                var info = grid1.Rows[e.RowIndex].Cells[2].Value;
                var price = grid1.Rows[e.RowIndex].Cells[3].Value;
                string x = Convert.ToString(price);
                x = x.Remove(0, 1);

                if (firstTime == false)
                {
                    while (productBool == true)
                    {
                        foreach (DataGridViewRow rows in grid2.Rows)
                        {
                            if (name == rows.Cells[1].Value)
                            {
                                quantity = Convert.ToInt32(rows.Cells[3].Value);
                                products[rows.Index].Quantity = quantity += 1;
                                rows.Cells[3].Value = quantity;
                                double y = Convert.ToDouble(x);
                                y = y * quantity;
                                string priceString = "$" + Convert.ToString(y);
                                rows.Cells[4].Value = priceString;
                                productTime = false;
                                productBool = false;
                                break;
                            }
                            else
                            {
                                productTime = true;
                            }
                        }
                        productBool = false;
                    }
                    if (productTime == true && productBool == false)
                    {
                        quantity = 1;
                        products.Add(new Product
                        {
                            ItemName = Convert.ToString(name),
                            Info = Convert.ToString(info),
                            Quantity = Convert.ToInt32(quantity),
                            Price = Convert.ToDouble(x)
                        });
                        grid2.Rows.Add(image, name, info, quantity,"$" + x);
                    }
                }
                else
                {
                    products.Add(new Product
                    {
                        ItemName = Convert.ToString(name),
                        Info = Convert.ToString(info),
                        Quantity = 1,
                        Price = Convert.ToDouble(x)
                    });
                    grid2.Rows.Add(image, name, info, quantity, price);
                    firstTime = false;
                }
                foreach (DataGridViewRow rows in grid2.Rows)
                {
                    rows.Height = 75;
                    grid2.Columns[2].Width = 175;
                    grid2.Columns[3].Width = 50;
                    grid2.Columns[4].Width = 70;
                }
            }
        }

        private void Grid2_CellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            double singlePrice;
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                int x = Convert.ToInt32(grid2.Rows[e.RowIndex].Cells[3].Value);
                if(x > 1)
                {
                    string stringPrice = Convert.ToString(grid2.Rows[e.RowIndex].Cells[4].Value);
                    stringPrice = stringPrice.Remove(0, 1);
                    double y = Convert.ToDouble(stringPrice);
                    singlePrice = y / x;
                    x--;
                    grid2.Rows[e.RowIndex].Cells[3].Value = x;
                    grid2.Rows[e.RowIndex].Cells[4].Value = "$" + (y - singlePrice);
                    products[e.RowIndex].Quantity = x;
                }
                else
                {
                    products.RemoveAt(e.RowIndex);
                    grid2.Rows.RemoveAt(e.RowIndex);
                    if(e.RowIndex == 0)
                    {
                        firstTime = true;
                    }
                }
            }
 
        }
        
        private void ClickedEventHandler1(object sender, EventArgs e)
        {
            foreach(Product product in products)
            {
                product.CalculateQuantityAndPrice();
            }
        }

        private void ClickedEventHandler2(object sender, EventArgs e)
        {
            double x;

            foreach (Product product in products)
            {
                x = product.CalculateQuantityAndPrice();
                result += x;
            }
            MessageBox.Show("Total price to pay: " + result + "$");
        }
    }
}
