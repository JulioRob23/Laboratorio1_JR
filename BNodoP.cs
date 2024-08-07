using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio1_JR
{
    public class BNodoP
    {
        public int orden;
        public List<Libro> libros;
        public List<BNodoP> hijos;
        public List<string> claves;
        public bool Eshoja;
        public BArbolP elarbol;
        public int minclaves;
        public int maxclaves;

        public BNodoP(int orden, bool eshoja, BArbolP elarbol)
        {
            this.orden = orden;
            this.libros = new List<Libro>();
            this.claves = new List<string>();
            this.hijos = new List<BNodoP>();
            this.Eshoja = eshoja;
            this.elarbol = elarbol;
            this.minclaves = ((int)Math.Ceiling(orden / 2.0)) - 1;
            this.maxclaves = orden - 1;
        }

        public void BuscarPorNombre(string nombre, List<Libro> resultados)
        {
            int i = 0;
            while (i < claves.Count && string.Compare(nombre, libros[i].name) > 0) i++;

            if (i < claves.Count && string.Compare(nombre, libros[i].name) == 0)
            {
                resultados.Add(libros[i]);
            }

            if (!Eshoja)
            {
                for (int j = 0; j < hijos.Count; j++)
                {
                    hijos[j].BuscarPorNombre(nombre, resultados);
                }
            }
        }

        public void InsertarP(string nombre, Libro ellibro)
        {
            if (claves.Count == maxclaves)
            {
                InsertarLleno(nombre, ellibro);
            }
            else
            {
                InsertarNoLleno(nombre, ellibro);
            }
        }


        private void InsertarLleno(string key, Libro ellibro)
        {
            Console.WriteLine($"Insertar lleno clave {key} en nodo con claves {string.Join(", ", claves)}");

            int i = claves.Count - 1;
            if (Eshoja)
            {
                while (i >= 0 && string.Compare(claves[i], key, true) > 0) i--;
                claves.Insert(i + 1, key);
                libros.Insert(i + 1, ellibro);
            }
            else
            {
                while (i >= 0 && string.Compare(claves[i], key, true) > 0) i--;
                hijos[i + 1].InsertarP(key, ellibro);
            }

            if (claves.Count > maxclaves)
            {
                if (elarbol.raiz == this)
                {
                    BNodoP raiznueva = new BNodoP(orden, false, this.elarbol);
                    raiznueva.hijos.Add(this);
                    raiznueva.DividirHijo(0, elarbol.raiz);
                    elarbol.raiz = raiznueva;
                }
                else
                {
                    BNodoP papa = EncontrarPapa(elarbol.raiz, this);
                    if (papa != null)
                    {
                        int posicion = papa.hijos.IndexOf(this);
                        if (posicion >= 0)
                        {
                            papa.DividirHijo(posicion, this);
                        }
                        else
                        {
                            Console.WriteLine("Error: no se pudo encontrar la posición del nodo en el padre.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: el nodo padre es null.");
                    }
                }
            }
        }


        public void DividirHijo(int posicionhijo, BNodoP hijoadividir)
        {
            int Medio = hijoadividir.claves.Count / 2;
            BNodoP nuevonodo = new BNodoP(hijoadividir.orden, hijoadividir.Eshoja, elarbol);

            nuevonodo.claves.AddRange(hijoadividir.claves.GetRange(Medio + 1, hijoadividir.claves.Count - (Medio + 1)));
            nuevonodo.libros.AddRange(hijoadividir.libros.GetRange(Medio + 1, hijoadividir.libros.Count - (Medio + 1)));

            hijoadividir.claves.RemoveRange(Medio + 1, hijoadividir.claves.Count - (Medio + 1));
            hijoadividir.libros.RemoveRange(Medio + 1, hijoadividir.libros.Count - (Medio + 1));


            if (!hijoadividir.Eshoja)
            {
                nuevonodo.hijos.AddRange(hijoadividir.hijos.GetRange(Medio + 1, hijoadividir.hijos.Count - (Medio + 1)));
                hijoadividir.hijos.RemoveRange(Medio + 1, hijoadividir.hijos.Count - (Medio + 1));
            }
            if (posicionhijo + 1 <= hijos.Count)
            {
                hijos.Insert(posicionhijo + 1, nuevonodo);
            }
            else
            {
                return;
            }
            // Insertar la clave mediana en el padre
            if (posicionhijo < claves.Count)
            {
                claves.Insert(posicionhijo, hijoadividir.claves[Medio]);
                libros.Insert(posicionhijo, hijoadividir.libros[Medio]);
            }
            else
            {
                return;
            }

            // Remover la clave y libro mediano del nodo dividido
            hijoadividir.claves.RemoveAt(Medio);
            hijoadividir.libros.RemoveAt(Medio);
        }



        public BNodoP EncontrarPapa(BNodoP elnodo, BNodoP hijo)
        {
            if (elnodo == null || elnodo.Eshoja)
            {
                return null;
            }
            else
            {
                foreach (BNodoP nodo in elnodo.hijos)
                {
                    if (nodo == hijo) return nodo;
                    else
                    {
                        BNodoP temp = EncontrarPapa(nodo, hijo);
                        if (temp != null) return temp;
                    }
                }
            }
            return null;
        }
        public void InsertarNoLleno(string key, Libro ellibro)
        {
            int i = claves.Count - 1;
            if (Eshoja)
            {
                while (i >= 0 && string.Compare(claves[i], key, true) > 0) i--;
                claves.Insert(i + 1, key);
                libros.Insert(i + 1, ellibro);
            }
            else
            {
                while (i >= 0 && string.Compare(claves[i], key, true) > 0) i--;
                hijos[i + 1].InsertarP(key, ellibro);
            }
        }

        public void EliminarP(string nombre, Libro ellibro)
        {
            Console.WriteLine($"Intentando eliminar clave {nombre} en nodo con claves {string.Join(", ", claves)}");
            int indice = 0;
            while (indice < claves.Count && string.Compare(claves[indice], nombre) < 0) indice++;
            if (indice < claves.Count && string.Compare(claves[indice], nombre) == 0)
            {
                if (Eshoja)
                {
                    EliminardehojaP(indice);
                }
                else
                {
                    EliminarNodehojaP(indice);
                }
            }
            else
            {
                if (Eshoja)
                {
                    Console.WriteLine($"El valor buscado {nombre} no está en el árbol");
                    return;
                }
                bool val = (indice == claves.Count);
                if (hijos[indice].claves.Count < minclaves) Llenar(indice);
                if (val && indice > claves.Count) hijos[indice - 1].EliminarP(nombre, ellibro);
                else hijos[indice].EliminarP(nombre, ellibro);
            }
        }
        private void EliminardehojaP(int indice)
        {
            Console.WriteLine($"Eliminando de hoja en índice {indice} con claves {string.Join(", ", claves)}");
            claves.RemoveAt(indice);
            libros.RemoveAt(indice);

            if (claves.Count < minclaves && this != elarbol.raiz)
            {
                BNodoP papa = EncontrarPapa(elarbol.raiz, this);
                int indicepapa = papa.hijos.IndexOf(this);
                papa.Llenar(indicepapa);
            }
        }
        private void EliminarNodehojaP(int indice)
        {
            Console.WriteLine($"Eliminando de nodo no hoja en índice {indice} con claves {string.Join(", ", claves)}");
            string clave = claves[indice];
            Libro ellbro = libros[indice];

            if (hijos[indice].claves.Count >= minclaves)
            {
                string anterior = ObtenerAnterior(indice);
                claves[indice] = anterior;
                Libro lanterior = ObtenerLAnterior(indice);
                libros[indice] = lanterior;
                hijos[indice].EliminarP(anterior, lanterior);

                if (hijos[indice].claves.Count > minclaves) Llenar(indice);
            }
            else if (hijos[indice + 1].claves.Count >= minclaves)
            {
                string siguiente = ObtenerSiguiente(indice);
                claves[indice] = siguiente;
                Libro lsiguiente = ObtenerLSiguiente(indice);
                hijos[indice + 1].EliminarP(siguiente, lsiguiente);
                if (hijos[indice + 1].claves.Count > minclaves) Llenar(indice + 1);
            }
            else
            {
                Unir(indice);
                hijos[indice].EliminarP(clave, ellbro);
            }
        }
        public string ObtenerAnterior(int indice)
        {
            BNodoP actual = hijos[indice];
            while (!actual.Eshoja) actual = actual.hijos[actual.claves.Count];
            return actual.claves[actual.claves.Count - 1];
        }
        public Libro ObtenerLAnterior(int indice)
        {
            BNodoP actual = hijos[indice];
            while (!actual.Eshoja) actual = actual.hijos[actual.claves.Count];
            return actual.libros[actual.claves.Count - 1];
        }

        public string ObtenerSiguiente(int indice)
        {
            BNodoP actual = hijos[indice + 1];
            while (!actual.Eshoja) actual = actual.hijos[0];
            return actual.claves[0];

        }
        public Libro ObtenerLSiguiente(int indice)
        {
            BNodoP actual = hijos[indice + 1];
            while (!actual.Eshoja) actual = actual.hijos[0];
            return actual.libros[0];

        }

        public void Llenar(int indice)
        {
            if (indice != 0 && hijos[indice - 1].claves.Count > minclaves)
            {
                PrestarAnterior(indice);
            }
            else if (indice != claves.Count && hijos[indice + 1].claves.Count > minclaves)
            {
                PrestarSiguiente(indice);
            }
            else
            {
                if (indice > 0)
                {
                    Unir(indice - 1);
                }
                else
                {
                    Unir(indice);
                }

                if (claves.Count < minclaves && this != elarbol.raiz)
                {
                    BNodoP papa = EncontrarPapa(elarbol.raiz, this);
                    int indicepapa = papa.hijos.IndexOf(this);
                    papa.Llenar(indicepapa);
                }
            }
        }


        public void PrestarAnterior(int indice)
        {
            BNodoP hijo = hijos[indice];
            BNodoP hermano = hijos[indice - 1];

            hijo.claves.Insert(0, claves[indice - 1]);
            hijo.libros.Insert(0, libros[indice - 1]);

            if (!hijo.Eshoja) hijo.hijos.Insert(0, hermano.hijos[hermano.hijos.Count - 1]);

            claves[indice - 1] = hermano.claves[hermano.claves.Count - 1];
            hermano.claves.RemoveAt(hermano.claves.Count - 1);
            libros[indice - 1] = hermano.libros[hermano.libros.Count - 1];
            hermano.libros.RemoveAt(hermano.libros.Count - 1);

            if (!hermano.Eshoja) hermano.hijos.RemoveAt(hermano.hijos.Count - 1);
        }
        public void PrestarSiguiente(int indice)
        {
            BNodoP hijo = hijos[indice];
            BNodoP hermano = hijos[indice + 1];

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
            BNodoP hijo = hijos[indice];
            BNodoP hermano = hijos[indice + 1];

            hijo.claves.Add(claves[indice]);
            hijo.libros.Add(libros[indice]);

            for (int i = 0; i < hermano.claves.Count; i++)
            {
                hijo.claves.Add(hermano.claves[i]);
                hijo.libros.Add(hermano.libros[i]);
            }

            if (!hijo.Eshoja)
            {
                for (int i = 0; i <= hijo.claves.Count; ++i)
                {
                    hijo.hijos.Add(hermano.hijos[i]);
                }
            }

            claves.RemoveAt(indice);
            libros.RemoveAt(indice);
            hijos.RemoveAt(indice + 1);
        }
        public Libro BuscarLibro(int ISBN)
        {
            int i = 0;
            while (i < claves.Count && ISBN > libros[i].ISBN) i++;
            if (i < claves.Count && libros[i].ISBN == ISBN) return libros[i];
            if (Eshoja) return null;
            return hijos[i].BuscarLibro(ISBN);

        }
        public Libro BuscarLibroP(string nombre)
        {
            int i = 0;
            while (i < claves.Count && string.Compare(nombre, libros[i].name) > 0) i++;

            if (i < claves.Count && string.Compare(nombre, libros[i].name) == 0)
                return libros[i];
            if (Eshoja)
            {
                return hijos[i].BuscarLibroP(nombre);
            }
            return null;
        }

        public void EditarLibP(Libro libroeditado)
        {
            int indice = 0;
            while (indice < claves.Count && string.Compare(claves[indice], libroeditado.name) < 0) indice++;
            if (indice < claves.Count && string.Compare(claves[indice], libroeditado.name) == 0)
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
                    Console.WriteLine("El valor buscado " + libroeditado.name + " no esta en el arbol P");
                    return;
                }
                bool val = (indice == claves.Count);
                if (val && indice > claves.Count) hijos[indice - 1].EditarLibP(libroeditado);
                else hijos[indice].EditarLibP(libroeditado);
            }
        }
    }
}
