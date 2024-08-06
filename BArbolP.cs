using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    //Definicion de la clase arbol de palabras y sus metodos
    public class BArbolP
    {
        public BNodoP? raiz;
        public int orden;
        public BArbolP(int orden)
        {
            this.raiz = null;
            this.orden = orden;
        }
        //Insertar un libro ordenado segun su nombre
        public void InsertarP(string clave, Libro ellibro)
        {
            //Si la raiz es nula crear una nueva raiz
            if (raiz == null)
            {
                raiz = new BNodoP(orden, true, this);
                raiz.claves.Add(clave);
                raiz.libros.Add(ellibro);
            }
            //sino insertar segun el metodo
            else
            {
                raiz.InsertarP(clave, ellibro);
            }
        }

        //Eliminar un libro
        public void EliminarN(string clave, Libro ellibro)
        {
            //Si la raiz es nula es porque el arbol esta vacio
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

        //Editar un valor en el arbol
        public void EditarLibroP(Libro libroeditado)
        {
            if (raiz == null)
            {
                Console.WriteLine("El arbol esta vacio");
                return;
            }
            raiz.EditarLibP(libroeditado);
        }
    }
}
