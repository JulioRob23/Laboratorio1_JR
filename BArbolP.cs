using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    public class BArbolP
    {
        public BNodoP? raiz;
        public int orden;
        public BArbolP(int orden)
        {
            this.raiz = null;
            this.orden = orden;
        }
        public void InsertarP(string clave, Libro ellibro)
        {
            if (raiz == null)
            {
                raiz = new BNodoP(orden, true, this);
                raiz.claves.Add(clave);
                raiz.libros.Add(ellibro);
            }
            else
            {
                raiz.InsertarP(clave, ellibro);
            }
        }

        public void EliminarN(string clave, Libro ellibro)
        {
            if (raiz == null)
            {
                Console.WriteLine("El arbol esta vacio");
                return;
            }
            raiz.EliminarP(clave, ellibro);

            if (raiz.claves.Count == 0)
            {
                if (raiz.Eshoja) raiz = null;
                else raiz = raiz.hijos[0];
            }
        }
    }
}
