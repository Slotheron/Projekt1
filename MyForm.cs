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
        private DataGridView productsGrid; 
        private DataGridView cartGrid;
        private TableLayoutPanel receiptTable;
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
            TableLayoutPanel tableMaster = CreateTable(2, 1);
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            tableMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            Controls.Add(tableMaster);

            var orderTable = CreateTable(2, 4);
            orderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            orderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            orderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            orderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            orderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            orderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableMaster.Controls.Add(orderTable);

            receiptTable = CreateTable(4, 0);
            receiptTable.AutoScroll = true;
            receiptTable.BackColor = Color.Gray;
            receiptTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            receiptTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            receiptTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            receiptTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tableMaster.Controls.Add(receiptTable);
            
            productsGrid = (new DataGridView
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
            productsGrid.Columns[0].Name = "Product";
            productsGrid.Columns[1].Name = "Info";
            productsGrid.Columns[2].Name = "Price Per Item";
            productsGrid.Columns["Product"].ReadOnly = true;
            productsGrid.Columns["Info"].ReadOnly = true;
            productsGrid.Columns["Price Per Item"].ReadOnly = true;
            orderTable.Controls.Add(productsGrid);
            orderTable.SetColumnSpan(productsGrid, 2);
            orderTable.SetRowSpan(productsGrid, 2);
            
            //creates a button column in productsGrid
            var buttonColumn1 = new DataGridViewButtonColumn
            {
                Text = "Add to cart",
                //uses Text as the button's text ( which is seperate without this )
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0,25,0,25) }
            };
            productsGrid.Columns.Insert(3, buttonColumn1);
            //creates image column in productsGrid
            var imagesColumn1 = new DataGridViewImageColumn();
            productsGrid.Columns.Insert(0, imagesColumn1);

            string path = "Products.txt"; 
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
                    productsGrid.Rows.Add(bitmap, product, info, "$" + price);
                }
                catch
                {
                    MessageBox.Show("Check File! Missing image files.");
                    string product = parts[1];
                    string info = parts[2];
                    string price = parts[3];
                    productsGrid.Rows.Add(null, product, info, "$" + price);
                }
                

            }
            
            foreach(DataGridViewRow rows in productsGrid.Rows)
            {
                rows.Height = 75;
                productsGrid.Columns[2].Width = 125;
            }
            productsGrid.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            cartGrid = (new DataGridView
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
            //cartGrid headers
            cartGrid.Columns[0].Name = "Product";
            cartGrid.Columns[1].Name = "Info";
            cartGrid.Columns[2].Name = "Qty.";
            cartGrid.Columns[3].Name = "Total Price";
            cartGrid.Columns["Product"].ReadOnly = true;
            cartGrid.Columns["Info"].ReadOnly = true;
            cartGrid.Columns["Qty."].ReadOnly = true;
            cartGrid.Columns["Total Price"].ReadOnly = true;
            orderTable.Controls.Add(cartGrid);
            orderTable.SetRowSpan(cartGrid, 2);

            //makes the info column's text multilined
            cartGrid.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //set the button column for the cart grid (cartGrid)
            var buttonColumn2 = new DataGridViewButtonColumn
            {
                Text = "Remove from cart",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(0, 25, 0, 25) }
            };
            cartGrid.Columns.Insert(4, buttonColumn2);

            //sets image column for the cart grid (cartGrid)
            var imagesColumn2 = new DataGridViewImageColumn();
            cartGrid.Columns.Insert(0, imagesColumn2);

            subtotalLabel = new Label
            {
                Text = Subtotal(),
                Dock = DockStyle.Fill,
                Font = new Font("", 14)
            };
            orderTable.Controls.Add(subtotalLabel);

            var receiptLabel = new Label
            {
                Text = "Triple Js' NetShop",
                Font = new Font("", 14),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top,
                Height = 80
            };
            receiptTable.SetColumnSpan(receiptLabel, 4);
            receiptTable.Controls.Add(receiptLabel);

            receiptTable.Controls.Add(CreateHeaderLabel("Product "));
            receiptTable.Controls.Add(CreateHeaderLabel("Quantity"));
            receiptTable.Controls.Add(CreateHeaderLabel("Price Per Item"));
            receiptTable.Controls.Add(CreateHeaderLabel("Total Price"));
            
            buttonOrder = new Button
            {
                Text = "Place Order",
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.White
            };
            receiptTable.Controls.Add(buttonOrder);
            receiptTable.SetColumnSpan(buttonOrder, 4);

            var table3 = (new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Gray,
                Dock = DockStyle.Bottom
            });
            orderTable.Controls.Add(table3);
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

            productsGrid.CellContentClick += AddToCartButton;
            cartGrid.CellContentClick += RemoveFromCartButton;
            buttonOrder.Click += PlaceOrderButton;
            buttonCode.Click += RebateCodeButtonClick;
        }

        //click methods for adding and removing items to the cart grid (cartGrid)
        private void AddToCartButton(object sender, DataGridViewCellEventArgs e)
        {
            bool productBool = true;
            bool productTime = false;
            int quantity = 1;
            var senderGrid = (DataGridView)sender;
            if(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var image = productsGrid.Rows[e.RowIndex].Cells[0].Value;
                var name = productsGrid.Rows[e.RowIndex].Cells[1].Value;
                var info = productsGrid.Rows[e.RowIndex].Cells[2].Value;
                var price = productsGrid.Rows[e.RowIndex].Cells[3].Value;
                string priceToString = Convert.ToString(price);
                priceToString = priceToString.Remove(0, 1);

                if (firstTime == false)
                {
                    foreach (DataGridViewRow rows in cartGrid.Rows)
                    {
                        if (name == rows.Cells[1].Value)
                        {
                            quantity = Convert.ToInt32(rows.Cells[3].Value);
                            products[rows.Index].Quantity = quantity += 1;
                            rows.Cells[3].Value = quantity;
                            //needed to remove the '$' before converting to double
                            double priceAmount = Convert.ToDouble(priceToString);
                            priceAmount = priceAmount * quantity;
                            string priceString = "$" + Convert.ToString(priceAmount);
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
                            Price = Convert.ToDouble(priceToString)
                        });
                        cartGrid.Rows.Add(image, name, info, quantity,"$" + priceToString);
                    }
                }
                else
                {
                    products.Add(new Product
                    {
                        ItemName = Convert.ToString(name),
                        Info = Convert.ToString(info),
                        Quantity = 1,
                        Price = Convert.ToDouble(priceToString)
                    });
                    cartGrid.Rows.Add(image, name, info, quantity, price);
                    firstTime = false;
                }
                foreach (DataGridViewRow rows in cartGrid.Rows)
                {
                    rows.Height = 75;
                    cartGrid.Columns[2].Width = 125;
                    cartGrid.Columns[3].Width = 30;
                    cartGrid.Columns[4].Width = 70;
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
                int itemQuantity = Convert.ToInt32(cartGrid.Rows[e.RowIndex].Cells[3].Value);
                if(itemQuantity > 1)
                {
                    string stringPrice = Convert.ToString(cartGrid.Rows[e.RowIndex].Cells[4].Value);
                    stringPrice = stringPrice.Remove(0, 1);
                    double combinedPrice = Convert.ToDouble(stringPrice);
                    singlePrice = combinedPrice / itemQuantity;
                    itemQuantity--;
                    cartGrid.Rows[e.RowIndex].Cells[3].Value = itemQuantity;
                    cartGrid.Rows[e.RowIndex].Cells[4].Value = "$" + (combinedPrice - singlePrice);
                    products[e.RowIndex].Quantity = itemQuantity;
                }
                else
                {
                    products.RemoveAt(e.RowIndex);
                    cartGrid.Rows.RemoveAt(e.RowIndex);
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
            string path1 = "Codes.txt";
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
            receiptTable.Controls.Add(new Label
            {
                Text = product.ItemName
            });
        }
        private void CreateQuantityLabel(Product product)
        {
            receiptTable.Controls.Add(new Label
            {
                Text = Convert.ToString(product.Quantity)
            });
        }
        private void CreatePriceLabel(Product product)
        {
            receiptTable.Controls.Add(new Label
            {
                Text = "$" + string.Format("{0:0.00}", product.Price)
            }); 
        }
        private void CreateTotalLabel(Product product)
        {
            receiptTable.Controls.Add(new Label
            {
                Text = "$" + string.Format("{0:0.00}", product.CalculateQuantityAndPrice())
            });
        }
        private void CreateEndingLabels()
        {
            receiptTable.Controls.Add(new Label
            {
                Name = "0",
                Text = "Subtotal: $" + string.Format("{0:0.00}", subtotalVariable),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopRight
            });
            receiptTable.SetColumnSpan(receiptTable.Controls["0"], 4);
            receiptTable.Controls.Add(new Label
            {
                Name = "1",
                Text = "Tax(6%): $" + string.Format("{0:0.00}", taxAmount),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.TopRight
            });
            receiptTable.SetColumnSpan(receiptTable.Controls["1"], 4);
            receiptTable.Controls.Add(new Label
            {
                Name = "2",
                Text = "Total: $" + string.Format("{0:0.00}", total),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.TopRight
            });
            receiptTable.SetColumnSpan(receiptTable.Controls["2"], 4);
        }
        private Label CreateHeaderLabel(string y)
        {
            return (new Label
            {
                Text = y
            });
        }
        private TableLayoutPanel CreateTable(int x, int y)
        {
            return (new TableLayoutPanel
            {
                ColumnCount = x,
                RowCount = y,
                Dock = DockStyle.Fill
            });
        }
        
    }
}
