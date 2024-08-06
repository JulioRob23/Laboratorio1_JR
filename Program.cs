using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using Laboratorio1_JR;

internal class Program
{
    private static void Main(string[] args)
    {
        string csvRutadeArchivo = @"C:\Users\Julio Javier\Desktop\Javi\U Landivar\Segundo semestre 2024\Estructura de datos II\lab\Laboratorio 1\lab01_books.csv";
        BArbol arbolN = new BArbol(11);
        BArbolP arbolP = new BArbolP(11);

        try
        {
            StreamReader lector = new StreamReader(csvRutadeArchivo);

            string linea; 

                while((linea = lector.ReadLine()) != null)
                {
                    string[] partes = linea.Split(';');
                    if(partes.Length > 1)
                    {
                        string proceso = partes[0].Trim();
                        string JSON = partes[1].Trim();

                        switch (proceso)
                        {
                            case "INSERT":
                                Libro ellibro = JsonSerializer.Deserialize<Libro>(JSON);
                                arbolN.InsertarN(ellibro.ISBN, ellibro);
                                        arbolP.InsertarP(ellibro.name, ellibro);
                                break;
                            case "PATCH":
                                Libro libroeditar = JsonSerializer.Deserialize<Libro>(JSON);
                                Libro librooriginal = BuscarLibro(arbolP, libroeditar.ISBN);
                                if (librooriginal == null) Console.WriteLine("El libro no esta en el arbol");
                                else
                                {
                                    arbolP.EditarLibroP(libroeditar);
                                    arbolN.EditarLibroN(libroeditar);
                                }
                                break;
                            case "DELETE":
                                EE eliminar = JsonSerializer.Deserialize<EE>(JSON);
                                Libro ellibroelimi = BuscarLibro(arbolP, eliminar.ISBN);
                                if (ellibroelimi == null) Console.WriteLine("La clave no esta en el arbol");
                                else
                                {
                                    arbolP.EliminarN(ellibroelimi.name, ellibroelimi);
                                    arbolN.EliminarN(eliminar.ISBN, ellibroelimi);
                                }
                                break;
                            default: Console.WriteLine("Metodo no reconocido");
                                break;
                        }
                    }
                }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al leer el archivo: " + e.Message);
        }
        Console.WriteLine("Archivo Cargado");
        string rutabusqueda = @"C:\Users\Julio Javier\Desktop\Javi\U Landivar\Segundo semestre 2024\Estructura de datos II\lab\Laboratorio 1\lab01_search.csv";
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
                            Libro ellibro = BuscarLibroL(arbolP,buscar.name);
                            libros.Add(ellibro);
                            break;
                        default:
                            Console.WriteLine("Metodo no reconocido");
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
        File.WriteAllText("Datos Busqueda",jsonString);
        Console.Write("Archivo creado");
    }

    public static Libro BuscarLibro(BArbolP elarbol, int ISBN)
    {
        Libro milibro = elarbol.raiz.BuscarLibro(ISBN);
        return milibro;
    }
    public static Libro BuscarLibroL(BArbolP elarbol, string nombre)
    {
        Libro milibro = elarbol.raiz.BuscarLibroP(nombre);
        return milibro;
    }
}