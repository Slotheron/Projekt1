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
    class Products : MyForm
    { 
        public List<Product> ProductList{ get; set; }
        public void AddProduct(string name, string info, double price, int quantity)
        {
            ProductList.Add(new Product
            {
                ItemName = name,
                Info = info,
                Price = price,
                Quantity = quantity
            });
        }
    }
    class Product : MyForm
    {
        public string ItemName { get; set; }
        public string Info { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    
}
