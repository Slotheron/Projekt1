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
        private bool firstTime = true;
        private double subtotalVariable = 0;
        private double taxAmount = 0;
        private double total = 0;
        private DataGridView grid1;
        private DataGridView grid2;
        private TableLayoutPanel table2;
        private Label subtotalLabel;
        private Label rebateLabel;
        private Button buttonOrder;
        private TextBox textBox1;
        private Button buttonCode;
        private List<Product> products;
        private bool codeFound = false;

        public MyForm()
        {
            products = new List<Product> { };
            var tableMaster = (new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill
            });
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            Controls.Add(tableMaster);

            var table1 = (new TableLayoutPanel
            {
               ColumnCount = 2,
               RowCount = 4,
               Dock = DockStyle.Fill
            });
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table1.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table1.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table1.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table1.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableMaster.Controls.Add(table1);

            table2 = (new TableLayoutPanel
            {
                AutoScroll = true,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.Gray
            });
            table2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tableMaster.Controls.Add(table2);
            
            grid1 = (new DataGridView
            {
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
            table1.SetRowSpan(grid1, 2);
            
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
            string path = @"C:\Users\Joe\source\repos\Projekt1\Projekt1\Products.txt"; /* products list location  @"";*/
            string[] lines = File.ReadAllLines(path);

            //loop to grab values from a text file to create Products or Items.
            foreach (string x in lines)
            {
                string[] parts = x.Split(',');
                try
                {
                    Image image = Image.FromFile(parts[0]);
                    Bitmap bitmap = new Bitmap(image, new Size(75, 75));
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(bitmap, product, info, "$" + price);
                }
                catch
                {
                    MessageBox.Show("Check File! Missing image files.");
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    grid1.Rows.Add(null, product, info, "$" + price);
                }
                

            }
            //if(grid1.Rows.Count < lines.Length)
            //{
            //    //Application.Exit();
            //}
            //resizing of each rows height to a static amount and the info column's width.
            foreach(DataGridViewRow rows in grid1.Rows)
            {
                rows.Height = 75;
                grid1.Columns[2].Width = 125;
            }
            
            grid1.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            grid2 = (new DataGridView
            {
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
            grid2.Columns[2].Name = "Qty.";
            grid2.Columns[3].Name = "Total Price";
            grid2.Columns["Product"].ReadOnly = true;
            grid2.Columns["Info"].ReadOnly = true;
            grid2.Columns["Qty."].ReadOnly = true;
            grid2.Columns["Total Price"].ReadOnly = true;
            table1.Controls.Add(grid2);
            table1.SetRowSpan(grid2, 2);

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
                Font = new Font("", 14)
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
            table2.Controls.Add(receiptLabel);
            
            CreateHeaderLabel(table2, "Product ");
            CreateHeaderLabel(table2, "Quantity");
            CreateHeaderLabel(table2, "Price Per Item");
            CreateHeaderLabel(table2, "Total Price");
            
            buttonOrder = new Button
            {
                Text = "Place Order",
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.White
            };
            table2.Controls.Add(buttonOrder);
            table2.SetColumnSpan(buttonOrder, 4);

            var table3 = (new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Gray,
                Dock = DockStyle.Bottom
            });
            table1.Controls.Add(table3);
            rebateLabel = new Label
            {
                Text = "Enter Rebate Code Here:",
                Dock = DockStyle.Bottom
            };
            table3.Controls.Add(rebateLabel);

            textBox1 = new TextBox
            {
                Dock = DockStyle.Bottom
            };
            table3.Controls.Add(textBox1);

            buttonCode = new Button
            {
                Text = "Enter discount code",
                Dock = DockStyle.Bottom
            };
            table3.Controls.Add(buttonCode);

            grid1.CellContentClick += AddToCartButton;
            grid2.CellContentClick += RemoveFromCartButton;
            buttonOrder.Click += PlaceOrderButton;
            buttonCode.Click += RebateCodeButtonClick;
        }

        //click methods for adding and removing items to the cart grid (grid2)
        private void AddToCartButton(object sender, DataGridViewCellEventArgs e)
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
                    grid2.Columns[2].Width = 125;
                    grid2.Columns[3].Width = 30;
                    grid2.Columns[4].Width = 70;
                }
                subtotalLabel.Text = Subtotal();
            }
        }

        private void RemoveFromCartButton(object sender, DataGridViewCellEventArgs e)
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
        
        private void PlaceOrderButton(object sender, EventArgs e)
        {
            if (products.Count >= 1)
            {
                foreach (Product product in products)
                {
                    if(codeFound == true)
                    {
                        product.Price = product.Price * .8;
                    }
                    product.CalculateQuantityAndPrice();
                    CreateNameLabel(product);
                    CreateQuantityLabel(product);
                    CreatePriceLabel(product);
                    CreateTotalLabel(product);
                }
                taxAmount = subtotalVariable * 0.06;
                total = subtotalVariable + taxAmount;
                buttonOrder.Visible = false;
                CreateEndingLabels();
            }
            else
            {
                MessageBox.Show("You have no items currently in your cart.");
            }
        }

        //Click method for rebate code.
        private void RebateCodeButtonClick(object sender, EventArgs e)
        {
            string path1 = @"C:\Users\Joe\source\repos\Projekt1\Projekt1\Codes.txt";
            string[] validCodes = File.ReadAllLines(path1);
            string userCode = textBox1.Text;

            foreach (string code in validCodes)
            {
                if (code == userCode)
                {
                    codeFound = true;
                }
            }

            if (!codeFound)
            {
                MessageBox.Show("Invalid code, please enter a new one!");
            }
            else if (codeFound)
            {
                MessageBox.Show("You will receive a 20% discount!");
                subtotalLabel.Text = Subtotal();
                buttonCode.Visible = false;
            }
        }

        private string Subtotal()
        { 
            double x = 0;

            foreach (Product product in products)
            {
                x += product.CalculateQuantityAndPrice();
            }
            if (codeFound == true)
            {
                x = x * .8;
            }
            subtotalVariable = x;
            
            return "Subtotal: " + Environment.NewLine + "$" + string.Format("{0:0.00}",subtotalVariable);
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
                Text = "$" + string.Format("{0:0.00}", product.Price)
            }); 
        }
        private void CreateTotalLabel(Product product)
        {
            table2.Controls.Add(new Label
            {
                Text = "$" + string.Format("{0:0.00}", product.CalculateQuantityAndPrice())
            });
        }
        private void CreateEndingLabels()
        {
            table2.Controls.Add(new Label
            {
                Name = "0",
                Text = "Subtotal: $" + string.Format("{0:0.00}", subtotalVariable),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopRight
            });
            table2.SetColumnSpan(table2.Controls["0"], 4);
            table2.Controls.Add(new Label
            {
                Name = "1",
                Text = "Tax(6%): $" + string.Format("{0:0.00}", taxAmount),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.TopRight
            });
            table2.SetColumnSpan(table2.Controls["1"], 4);
            table2.Controls.Add(new Label
            {
                Name = "2",
                Text = "Total: $" + string.Format("{0:0.00}", total),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.TopRight
            });
            table2.SetColumnSpan(table2.Controls["2"], 4);
        }
        private void CreateHeaderLabel(Control x, string y)
        {
            x.Controls.Add(new Label
            {
                Text = y
            });
        }
    }
}
