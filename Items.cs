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
        public List<Product>ItemList { get; set; }
    }
    class Product : Items
    {
        public string ItemName { get; set; }
        public string Info { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public void AddProduct(Product product)
    {
        
    }
}
