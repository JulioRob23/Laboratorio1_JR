using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    public class Libro
    {
        public string isbn { get; set; }

        public int ISBN;
        public string name { get; set; }
        public string author { get; set; }
        public string price { get; set; } 
        public double Precio;
        public string quantity { get; set; }
        public int canStock;

        public Libro() 
        {
            ISBN = Convert.ToInt32(isbn);
            Precio = Convert.ToDouble(price);
            canStock = Convert.ToInt32(quantity);


        }
    }
}
