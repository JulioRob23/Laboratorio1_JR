using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Laboratorio1_JR
{
    public class BArbol
    {
        public BNodo? raiz;
        public int orden;

        public BArbol(int orden)
        {
            this.raiz = null;
            this.orden = orden;
        }

        public void InsertarN(int clave, Libro ellibro)
        {
            if (raiz == null)
            {
                raiz = new BNodo(orden, true, this);
                raiz.claves.Add(clave);
                raiz.libros.Add(ellibro);
            }
            else
            {
                raiz.InsertarN(clave, ellibro);
            }
        }

        public void EliminarN(int clave, Libro ellibro)
        {
            if (raiz == null)
            {
                Console.WriteLine("El arbol esta vacio");
                return;
            }
            raiz.EliminarN(clave, ellibro);

            if (raiz.claves.Count == 0)
            {
                if (raiz.Eshoja) raiz = null;
                else raiz = raiz.hijos[0];
            }
        }

        public void EditarLibroN(Libro libroeditado)
        {
            if (raiz == null)
            {
                Console.WriteLine("El arbol esta vacio");
                return;
            }
            raiz.EditarLibN(libroeditado);
        }
    }
}
