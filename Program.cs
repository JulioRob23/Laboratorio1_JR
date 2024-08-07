using System.Text.Json;
using Laboratorio1_JR;

internal class Program
{
    private static void Main(string[] args)
    {
        string csvRutadeArchivo = @"C:\Users\rayrt\source\repos\Laboratorio1_JR\lab01_books.csv";
        BArbol arbolN = new BArbol(11);
        BArbolP arbolP = new BArbolP(11);

        try
        {
            using (StreamReader lector = new StreamReader(csvRutadeArchivo))
            {
                string linea;

                while ((linea = lector.ReadLine()) != null)
                {
                    string[] partes = linea.Split(';');
                    if (partes.Length > 1)
                    {
                        string proceso = partes[0].Trim();
                        string JSON = partes[1].Trim();

                        switch (proceso)
                        {
                            case "INSERT":
                                Libro ellibro = JsonSerializer.Deserialize<Libro>(JSON);

                                if (ellibro != null)
                                {
                                    ellibro.datos();
                                    arbolN.InsertarN(ellibro.ISBN, ellibro);
                                    arbolP.InsertarP(ellibro.name, ellibro);
                                }
                                break;
                            case "PATCH":
                                Libro libroeditar = JsonSerializer.Deserialize<Libro>(JSON);
                                if (libroeditar != null)
                                {
                                    Libro librooriginal = BuscarLibro(arbolP, libroeditar.ISBN);
                                    if (librooriginal == null)
                                    {
                                        Console.WriteLine("El libro no está en el árbol");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Editando libro: {libroeditar.name} con ISBN: {libroeditar.ISBN}");
                                        arbolP.EditarLibroP(libroeditar);
                                        arbolN.EditarLibroN(libroeditar);
                                    }
                                }
                                break;
                            case "DELETE":
                                EE eliminar = JsonSerializer.Deserialize<EE>(JSON);
                                if (eliminar != null)
                                {
                                    Libro ellibroelimi = BuscarLibro(arbolP, eliminar.ISBN);
                                    if (ellibroelimi == null)
                                    {
                                        Console.WriteLine("La clave no está en el árbol");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Eliminando libro con ISBN: {eliminar.ISBN}");
                                        arbolP.EliminarN(ellibroelimi.name, ellibroelimi);
                                        arbolN.EliminarN(eliminar.ISBN, ellibroelimi);
                                    }
                                }
                                break;
                            default:
                                Console.WriteLine("Método no reconocido");
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al leer el archivo: " + e.Message);
        }


        Console.WriteLine("Archivo Cargado");
        string rutabusqueda = @"C:\Users\rayrt\source\repos\Laboratorio1_JR\lab01_search.csv";
        List<Libro> libros = new List<Libro>();
        try
        {
            StreamReader lector = new StreamReader(rutabusqueda);

            string linea;

            while ((linea = lector.ReadLine()) != null)
            {
                string[] partes = linea.Split(';');
                if (partes.Length > 1)
                {
                    string proceso = partes[0].Trim();
                    string JSON = partes[1].Trim();

                    switch (proceso)
                    {
                        case "SEARCH":
                            LB buscar = JsonSerializer.Deserialize<LB>(JSON);
                            if (buscar != null)
                            {
                                libros.AddRange(BuscarLibroL(arbolP, buscar.name));
                            }
                            break;
                        default:
                            Console.WriteLine("Método no reconocido");
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("");
        }
        string jsonString = JsonSerializer.Serialize(libros);
        File.WriteAllText("search.txt", jsonString);
        Console.Write("Archivo creado");
    }

    public static Libro BuscarLibro(BArbolP elarbol, int ISBN)
    {
        Libro milibro = elarbol.raiz.BuscarLibro(ISBN);
        return milibro;
    }
    public static List<Libro> BuscarLibroL(BArbolP elarbol, string nombre)
    {
        return elarbol.BuscarPorNombre(nombre);
    }

}