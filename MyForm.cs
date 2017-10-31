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
        TableLayoutPanel table2;
        public Label subtotalLabel;
        private List<Product> products;
        private bool firstTime = true;
        private Button buttonOrder;
 
        public MyForm()
        {
            products = new List<Product> { };
            var tableMaster = (new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill
            });
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            Controls.Add(tableMaster);

            var table1 = (new TableLayoutPanel
            {
               ColumnCount = 2,
               RowCount = 2,
               Dock = DockStyle.Fill
            });
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90));
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            tableMaster.Controls.Add(table1);

            table2 = (new TableLayoutPanel
            {
                AutoScroll = true,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.Gray
            });
            tableMaster.Controls.Add(table2);
            

            grid1 = (new DataGridView
            {
                Height = 500,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                //sets the grid to a static size that the user can not change.
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            });
            grid1.Columns[0].Name = "Product";
            grid1.Columns[1].Name = "Info";
            grid1.Columns[2].Name = "Price Per Item";
            grid1.Columns["Product"].ReadOnly = true;
            grid1.Columns["Info"].ReadOnly = true;
            grid1.Columns["Price Per Item"].ReadOnly = true;
            table1.Controls.Add(grid1);
            table1.SetColumnSpan(grid1, 2);
            

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
            string path = @"C:\Users\Joakim\Documents\GitHub\Projekt1\Products.txt"; /* products list location  @"";*/
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
                Height = 500, 
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                //sets the grid to a static size that the user can not change.
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            });
            //grid2 headers
            grid2.Columns[0].Name = "Product";
            grid2.Columns[1].Name = "Info";
            grid2.Columns[2].Name = "Quantity";
            grid2.Columns[3].Name = "Total Price";
            grid2.Columns["Product"].ReadOnly = true;
            grid2.Columns["Info"].ReadOnly = true;
            grid2.Columns["Quantity"].ReadOnly = true;
            grid2.Columns["Total Price"].ReadOnly = true;
            table1.Controls.Add(grid2);
            //table1.SetRowSpan(grid2, 2);
            
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


            subtotalLabel = new Label
            {
                Text = Subtotal(),
                Dock = DockStyle.Fill,
                Font = new Font ("", 12)
            };
            table1.Controls.Add(subtotalLabel);

            var receiptLabel = new Label
            {
                Text = "Triple Js' NetShop",
                Font = new Font("", 14),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top,
                Height = 80
            };
            table2.SetColumnSpan(receiptLabel, 4);
            var header1Label = new Label
            {
                Text = "Product ",
                Height = 45
            };
            var header2Label = new Label
            {
                Text = "Quantity"
            };
            var header3Label = new Label
            {
                Text = "Price Per Item"
            };
            var header4Label = new Label
            {
                Text = "Total Price"
            };
            table2.Controls.Add(receiptLabel);
            table2.Controls.Add(header1Label);
            table2.Controls.Add(header2Label);
            table2.Controls.Add(header3Label);
            table2.Controls.Add(header4Label);

            buttonOrder = new Button
            {
                Text = "Place Order",
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.White
            };
            table2.Controls.Add(buttonOrder);
            table2.SetColumnSpan(buttonOrder, 4);

            grid1.CellContentClick += Grid1_CellContentClicked;
            grid2.CellContentClick += Grid2_CellContentClicked;
            buttonOrder.Click += ClickedEventHandler1;
        }

        //click methods for adding and removing items to the cart grid (grid2)
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
                            break;
                        }
                        else
                        {
                            productTime = true;
                        }     
                    }
                    productBool = false;
                    
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
                subtotalLabel.Text = Subtotal();
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
                    if(products.Count < 1)
                    {
                        firstTime = true;
                    }
                }
                subtotalLabel.Text = Subtotal();
            }
 
        }
        
        private void ClickedEventHandler1(object sender, EventArgs e)
        {
            foreach(Product product in products)
            {
                product.CalculateQuantityAndPrice();
                CreateNameLabel(product);
                CreateQuantityLabel(product);
                CreatePriceLabel(product);
                CreateTotalLabel(product);
            }
            buttonOrder.Visible = false;
        }

        private string Subtotal()
        { 
            double x = 0;

            foreach (Product product in products)
            {
                x += product.CalculateQuantityAndPrice();
            }
            return "Subtotal: " + Environment.NewLine + "$" + x;
        }


        //Receipt labels
        private void CreateNameLabel(Product product)
        {
            table2.Controls.Add(new Label
            {
                Text = product.ItemName
            });
        }
        private void CreateQuantityLabel(Product product)
        {
            table2.Controls.Add(new Label
            {
                Text = Convert.ToString(product.Quantity)
            });
        }
        private void CreatePriceLabel(Product product)
        {
            table2.Controls.Add(new Label
            {
                Text = "$" + Convert.ToString(product.Price)
            }); 
        }
        private void CreateTotalLabel(Product product)
        {
            table2.Controls.Add(new Label
            {
                Text = "$" + Convert.ToString(product.CalculateQuantityAndPrice())
            });
        }
    }
}
