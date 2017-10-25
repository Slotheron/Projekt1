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
    class Items : MyForm
    { 
        List<Product>ItemList { get; set; }
    }
    class Product
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public void CreatProduct(string name, string info, double price, int quantity)
    {
        new Product
        {
            Name = name,
            Info = info,
            Price = price,
            Quantity = quantity
        };
    }
}
