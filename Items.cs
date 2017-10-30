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
    class Product : MyForm
    {
        public string ItemName { get; set; }
        public string Info { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public double CalculateQuantityAndPrice()
        {
            return Price * Quantity;
        }
    }
}
