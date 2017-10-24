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
    class Items : MyForm
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public List<double> ItemsList { get; set; } 

        public void GetTotalPrice()
        {
            foreach(DataGridViewRow row in grid2.Rows)
            {
                ProductName = Convert.ToString(row.Cells[1].Value);
                ProductQuantity = Convert.ToInt32(row.Cells[3].Value);
                ProductPrice = Convert.ToDouble(row.Cells[4].Value);
                ItemsList.Add(ProductPrice);

                
            } 
        }
    }
}
