using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    public class Libro
    {
        public int ISBN;
        public string nombre;
        public string autor;
        public string categoria;
        public double precio;
        public int canStock;

        public Libro(int iSBN, string nombre, string autor, string categoria, double precio, int canStock)
        {
            this.ISBN = iSBN;
            this.nombre = nombre;
            this.autor = autor;
            this.categoria = categoria;
            this.precio = precio;
            this.canStock = canStock;
        }
    }
}
