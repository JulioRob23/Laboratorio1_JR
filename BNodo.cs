using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    public class BNodo
    {
        public int orden;
        public List<Libro> libros;
        public List<BNodo> hijos;
        public List<int> claves;
        public bool Eshoja;
        public BArbol elarbol;
        public int minclaves;
        public int maxclaves;

        public BNodo(int orden, bool eshoja, BArbol elarbol)
        {
            this.orden = orden;
            this.libros = new List<Libro>();
            this.claves = new List<int>();
            this.hijos = new List<BNodo>();
            this.Eshoja = eshoja;
            this.elarbol = elarbol;
            this.minclaves = ((int)Math.Ceiling(orden/2.0))-1;
            this.maxclaves = orden -1;
        }

        public void InsertarN(int key, Libro ellibro)
        {
            if (claves.Count == maxclaves)
            {
                InsertarLleno(key, ellibro);
            }
            else
            {
                InsertarNoLleno(key,ellibro);
            }
        }
        public void InsertarLleno(int key, Libro ellibro)
        { 
            int i = claves.Count -1;
            if(Eshoja)
            {
               while(i>= 0 && claves[i] > key) i--;
               claves.Insert(i+1,key);
                libros.Insert(i+1,ellibro);

            }
            else
            {
                while (i >= 0 && claves[i] > key) i--;
                hijos[i+1].InsertarN(key,ellibro);
            }

            if (claves.Count > maxclaves)
            {
                if (elarbol.raiz == this)
                {
                    BNodo raiznueva = new BNodo(orden, false, this.elarbol);
                    raiznueva.hijos.Add(this);
                    raiznueva.DividirHijo(0, elarbol.raiz);
                    elarbol.raiz = raiznueva;
                }
                else
                {
                    BNodo papa = EncontrarPapa(elarbol.raiz, this);
                    papa.DividirHijo(papa.hijos.IndexOf(this),this);
                }
            }
        }

        public void DividirHijo(int posicionhijo, BNodo hijoadividir)
        {
            int Medio = orden / 2;
            BNodo nuevonodo = new BNodo(hijoadividir.orden, hijoadividir.Eshoja, elarbol);

            nuevonodo.claves.AddRange(hijoadividir.claves.GetRange(Medio + 1,hijoadividir.claves.Count - (Medio+1)));
            nuevonodo.libros.AddRange(hijoadividir.libros.GetRange(Medio + 1, hijoadividir.libros.Count - (Medio + 1)));
            hijoadividir.claves.RemoveRange(Medio + 1, hijoadividir.claves.Count - (Medio + 1));
            hijoadividir.libros.RemoveRange(Medio + 1, hijoadividir.libros.Count - (Medio + 1));

            if (!hijoadividir.Eshoja)
            {
                nuevonodo.hijos.AddRange(hijoadividir.hijos.GetRange(Medio + 1, hijoadividir.claves.Count - (Medio + 1)));
                hijoadividir.hijos.RemoveRange(Medio + 1, hijoadividir.claves.Count - (Medio + 1));

            }

            hijos.Insert(posicionhijo+1,nuevonodo);
            claves.Insert(posicionhijo, hijoadividir.claves[Medio]);
            libros.Insert(posicionhijo, hijoadividir.libros[Medio]);
            hijoadividir.claves.RemoveAt(Medio);
            hijoadividir.libros.RemoveAt(Medio);

        }

        public BNodo EncontrarPapa(BNodo elnodo, BNodo hijo)
        {
            if(elnodo == null|| elnodo.Eshoja)
            {
                return null;
            }
            else
            {
                foreach(BNodo nodo in elnodo.hijos)
                {
                    if(nodo == hijo) return nodo;
                    else
                    {
                        BNodo temp = EncontrarPapa(nodo, hijo);
                        if (temp != null) return temp;
                    }
                }
            }
            return null;
        }
        public void InsertarNoLleno(int key, Libro ellibro)
        {
            int i = claves.Count - 1;
            if(Eshoja)
            {
                while (i >= 0 && claves[i] > key) i--;
                claves.Insert(i + 1, key);
                libros.Insert(i+1,ellibro);
            }
            else
            {
                while (i >= 0 && claves[i] > key) i--;
                hijos[i+1].InsertarN(key, ellibro);
            }
        }

        public void EliminarN(int key, Libro ellibro)
        {
            int indice = 0;
            while(indice<claves.Count && claves[indice]< key) indice++;
            if(indice < claves.Count && claves[indice] == key)
            {
                if (Eshoja) Eliminardehoja(indice);
                else EliminarNodehoja(indice);
            }
            else
            {
                if (Eshoja)
                {
                    Console.WriteLine("El valor buscado " + key + " no esta en el arbol");
                    return;
                }
                bool val = (indice == claves.Count);
                if (hijos[indice].claves.Count < minclaves) Llenar(indice);
                if(val&& indice> claves.Count) hijos[indice-1].EliminarN(key, ellibro);
                else hijos[indice].EliminarN(key,ellibro);
            }
        }
        
        public void Eliminardehoja(int indice)
        {
            claves.RemoveAt(indice);
            libros.RemoveAt(indice);

            if(claves.Count < minclaves && this != elarbol.raiz)
            {
                BNodo papa = EncontrarPapa(elarbol.raiz, this);
                int indicepapa = papa.hijos.IndexOf(this);
                papa.Llenar(indicepapa);
            }
        }
        public void EliminarNodehoja(int indice)
        {
            int clave = claves[indice];
            Libro ellbro = libros[indice];

            if (hijos[indice].claves.Count >= minclaves)
            {
                int anterior = ObtenerAnterior(indice);
                claves[indice] = anterior;
                Libro lanterior = ObtenerLAnterior(indice);
                libros[indice] = lanterior;
                hijos[indice].EliminarN(anterior, lanterior);

                if (hijos[indice].claves.Count > minclaves) Llenar(indice);
            }
            else if (hijos[indice+1].claves.Count >= minclaves)
            {
                int siguiente = ObtenerSiguiente(indice);
                claves[indice] = siguiente;
                Libro lsiguiente = ObtenerLSiguiente(indice);
                libros[indice] = lsiguiente;
                hijos[indice + 1].EliminarN(siguiente,lsiguiente);
                if (hijos[indice+1].claves.Count > minclaves) Llenar(indice+1);
            }
            else
            {
                Unir(indice);
                hijos[indice].EliminarN(clave, ellbro);
            }
        }
        public int ObtenerAnterior(int indice)
        {
            BNodo actual = hijos[indice];
            while (!actual.Eshoja) actual = actual.hijos[actual.claves.Count];
            return actual.claves[actual.claves.Count-1];
        }
        public Libro ObtenerLAnterior(int indice)
        {
            BNodo actual = hijos[indice];
            while (!actual.Eshoja) actual = actual.hijos[actual.claves.Count];
            return actual.libros[actual.claves.Count - 1];
        }

        public int ObtenerSiguiente(int indice)
        {
            BNodo actual = hijos[indice + 1];
            while (!actual.Eshoja) actual = actual.hijos[0];
            return actual.claves[0];

        }
        public Libro ObtenerLSiguiente(int indice)
        {
            BNodo actual = hijos[indice + 1];
            while (!actual.Eshoja) actual = actual.hijos[0];
            return actual.libros[0];

        }

        public void Llenar(int indice)
        {
            if (indice != 0 && hijos[indice - 1].claves.Count > minclaves) PrestarAnterior(indice);
            else if (indice != claves.Count && hijos[indice + 1].claves.Count > minclaves) PrestarSiguiente(indice);
            else
            {
                if (indice > 0) Unir(indice - 1);
                else Unir(indice);

                if(claves.Count < minclaves && this != elarbol.raiz)
                {
                    BNodo papa = EncontrarPapa(elarbol.raiz, this);
                    int indicepapa = papa.hijos.IndexOf(this);
                    papa.Llenar(indicepapa);
                }

            }
        }

        public void PrestarAnterior(int indice)
        {
            BNodo hijo = hijos[indice];
            BNodo hermano = hijos[indice - 1];

            hijo.claves.Insert(0, claves[indice - 1]);
            hijo.libros.Insert(0, libros[indice - 1]);

            if (!hijo.Eshoja) hijo.hijos.Insert(0, hermano.hijos[hermano.hijos.Count - 1]);

            claves[indice-1] = hermano.claves[hermano.claves.Count-1];
            hermano.claves.RemoveAt(hermano.claves.Count - 1);
            libros[indice-1] = hermano.libros[hermano.libros.Count-1];
            hermano.libros.RemoveAt(hermano.libros.Count - 1);

            if (!hermano.Eshoja) hermano.hijos.RemoveAt(hermano.hijos.Count - 1);
        }
        public void PrestarSiguiente(int indice)
        {
            BNodo hijo = hijos[indice];
            BNodo hermano = hijos[indice + 1];

            hijo.claves.Add(claves[indice]);
            hijo.libros.Add(libros[indice]);

            if (!hijo.Eshoja) hijo.hijos.Add(hermano.hijos[0]);

            claves[indice] = hermano.claves[0];
            hermano.claves.RemoveAt(0);
            libros[indice] = hermano.libros[0];
            hermano.libros.RemoveAt(0);

            if (!hermano.Eshoja) hermano.hijos.RemoveAt(0);
        }

        public void Unir(int indice)
        {
            BNodo hijo = hijos[indice];
            BNodo hermano = hijos[indice + 1];

            hijo.claves.Add(claves[indice]);
            hijo.libros.Add(libros[indice]);

            for(int i = 0; i < hermano.claves.Count; i++)
            {
                hijo.claves.Add(hermano.claves[i]);
                hijo.libros.Add(hermano.libros[i]);
            }

            if(!hijo.Eshoja)
            {
                for(int i = 0; i <= hijo.claves.Count; ++i)
                {
                    hijo.hijos.Add(hermano.hijos[i]);
                }
            }

            claves.RemoveAt(indice);
            libros.RemoveAt(indice);
            hijos.RemoveAt(indice + 1);
        }

        public void EditarLibN(Libro libroeditado)
        {
            int indice = 0;
            while (indice < claves.Count && claves[indice] < libroeditado.ISBN) indice++;
            if (indice < claves.Count && claves[indice] == libroeditado.ISBN)
            {
                if (libroeditado.name != null) libros[indice].name = libroeditado.name;
                if (libroeditado.author != null) libros[indice].author = libroeditado.author;
                if (libroeditado.Precio != -1) libros[indice].Precio = libroeditado.Precio;
                if (libroeditado.canStock != -1) libros[indice].canStock = libroeditado.canStock;
            }
            else
            {
                if (Eshoja)
                {
                    Console.WriteLine("El valor buscado " + libroeditado.name + " no esta en el arbol N");
                    return;
                }
                bool val = (indice == claves.Count);
                if (val && indice > claves.Count) hijos[indice - 1].EditarLibN(libroeditado);
                else hijos[indice].EditarLibN(libroeditado);
            }
        }

    }
}
