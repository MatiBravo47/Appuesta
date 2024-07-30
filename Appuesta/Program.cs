using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Appuesta
{
    internal class Program
    {   
        static void Main(string[] args)
        {
            char respuesta, seguir, confirmacion, opcionModificar;
            string nombreLocal, nombreVisitante, bajaLocal, bajaVisitante, competenciaLocal, competenciaVisitante, necesidadVisitante, necesidadLocal;
            char[] resultados1 = new char[5], resultados2 = new char[5];
            double porcentajeLocal, porcentajeVisitante, efectividadVisitante, efectividadLocal;
            //Empieza con 10 por ser local
            int ventajaLocal = 10, ventajaVisitante = 0;

            //Ingreso de nombres de equipos
            string ingresoNombre(string nombre)
            {
                string nombreEquipo; 
                Console.WriteLine($"Ingrese nombre de {nombre}(enter para omitir)");
                nombreEquipo = Console.ReadLine();
                return nombreEquipo == "" ? nombre : nombreEquipo;
            }

            //Porcentaje de victorias en los ultimos 5 partidos de cada equipo
            double calcularPorcentaje(char[] resultado) 
            {
                int partidosGanados = 0;
                for (int i = 0; i < 5; i++) 
                {
                    if (resultado[i] == 'G' || resultado[i] == 'g')
                    {
                        partidosGanados++;
                    }
                }
                return ((partidosGanados / 5.0) * 100);
            }

            double calcularPorcentajeEfectividad()
            {
                double fechasJugadas, puntosObtenidos, puntosTotales;
                Console.WriteLine("Ingrese cantidad de fechas jugadas en el torneo");
                fechasJugadas = int.Parse(Console.ReadLine());
                puntosTotales = fechasJugadas * 3; 
                Console.WriteLine("Ingrese cantidad de puntos obtenidos");
                puntosObtenidos = int.Parse(Console.ReadLine());
                return ((puntosObtenidos / puntosTotales) * 100.0);
            }

            //Ingreso de resultados de equipos en un arreglo. 
            void almacenarResultados(char[] resultado) 
            {
                for (int i = 0; i < 5; i++)
                {
                    do
                    {
                        resultado[i] = char.ToUpper(Console.ReadKey().KeyChar);
                        if (i != 4)
                        {
                            Console.Write(" - ");
                        }
                    } while (resultado[i] != 'G' && resultado[i] != 'E' && resultado[i] != 'P');
                }
                Console.WriteLine();
            }

            int menuSiNo(string mensaje,int valorSi, int valorNo) 
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine(mensaje);
                    Console.WriteLine("1. Si");
                    Console.WriteLine("2. No");
                    respuesta = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                } while (respuesta != '1' && respuesta != '2');
                return respuesta == '1' ? 10 : 20;
            }

            string respuestaMenu() 
            {
                return (respuesta == '1') ? "Si": "No";
            }

            int diferenciaNotoriaPorcentajes() 
            {
                return Math.Abs(porcentajeLocal - porcentajeVisitante) > 20 ? 20 : 0;
            }

            int diferenciaNotoria(int local, int visitante,int condicion, int valorTrue, int valorFalse) 
            {
                return (Math.Abs(local - visitante) > condicion) ? valorTrue : valorFalse;
            }

            void comparar(double opcionA, double opcionB, string textoComparacion) 
            {
                if (opcionA > opcionB)
                {
                    Console.WriteLine($"{nombreLocal} tiene mayor porcentaje de {textoComparacion}({opcionA.ToString("F2")}% vs {opcionB.ToString("F2")}%)");
                    ventajaLocal += diferenciaNotoriaPorcentajes();
                }
                else if (opcionA < opcionB)
                {
                    Console.WriteLine($"{nombreVisitante} tiene mayor porcentaje de {textoComparacion}({opcionA.ToString("F2")}% vs {opcionB.ToString("F2")}%)");
                    ventajaLocal += diferenciaNotoriaPorcentajes();
                }
                else 
                {
                    Console.WriteLine($"Igualan en porcentaje de {textoComparacion}");
                }
            }

            void compararVentajas()
            {
                if (ventajaLocal > ventajaVisitante)
                {
                    Console.WriteLine($"{nombreLocal} tiene mas ventaja ({ventajaLocal} vs {ventajaVisitante})");
                    if ((ventajaLocal - ventajaVisitante) >= 50)
                    {
                        Console.WriteLine($"Pronostico: Victoria holgada {nombreLocal}");
                    }
                    else if ((ventajaLocal - ventajaVisitante) < 29)
                    {
                        Console.WriteLine("Pronostico: Empate");
                    }
                    else
                    {
                        Console.WriteLine($"Pronostico: Victoria ajustada de {nombreLocal} ({ventajaLocal} vs {ventajaVisitante})");
                    }
                }
                else if (ventajaLocal < ventajaVisitante)
                {
                    Console.WriteLine($"{nombreVisitante} tiene mas ventaja ({ventajaVisitante} vs {ventajaLocal}) ");
                    if ((ventajaVisitante - ventajaLocal) >= 50)
                    {
                        Console.WriteLine($"Pronostico: Victoria holgada de {nombreVisitante}");
                    }
                    else if ((ventajaVisitante - ventajaLocal) < 29)
                    {
                        Console.WriteLine("Pronostico: Empate");
                    }
                    else
                    {
                        Console.WriteLine($"Pronostico: Victoria ajustada de {nombreVisitante}");
                    }
                }
                else
                {
                    Console.WriteLine("Pronostico: Empate");
                }
            }

            void mostrarIngresos(string nombre, string baja, string competencia, string necesidad, double efectividad, char[] resultados)
            {
                Console.WriteLine($"Nombre equipo local: {nombre}");
                Console.WriteLine($"Bajas importantes: {baja}");
                Console.WriteLine($"CompetenciaInternacional: {competencia} ");
                Console.WriteLine($"Necesidad de resultado: {necesidad} ");
                Console.WriteLine($"Efectividad local: {efectividad.ToString("F2")}%");
                Console.Write($"Ultimos resultados: ");
                for (int i = 0; i < 5; i++)
                {
                    if (i < 4)
                    {
                        Console.Write($"{resultados[i]} - ");
                    }
                    else
                    {
                        Console.Write($"{resultados[i]} ");
                    }
                }
                Console.WriteLine();
            }

            void menuModificar() 
            {
                Console.WriteLine("Elija la opcion que desea modificar");
                Console.WriteLine("1. Nombre Equipo");
                Console.WriteLine("2. Bajas importantes");
                Console.WriteLine("3. Competencia Internacional");
                Console.WriteLine("4. Necesidad resultado");
                Console.WriteLine("5. Puntos de liga actual");
                Console.WriteLine("6. Ultimos partidos ");
            }

            void eleccionModificar(ref string nombre,ref string baja,ref string competencia,ref string necesidad)
            {
                switch (opcionModificar)
                {
                    case '1': 
                        nombre = ingresoNombre("Equipo local");
                        break;
                    case '2':
                        cambiarRespuesta(ref baja);
                        break;
                    case '3':
                        cambiarRespuesta(ref competencia);
                        break;
                    case '4':
                        cambiarRespuesta(ref necesidad);
                        break;
                    case '6': 
                        ingresoResultados(nombre, resultados1);
                        break;
                    default: Console.WriteLine("Opcion incorrecta");
                        break;
                }
            }

            void ingresoResultados(string nombre,char[] resultado) 
            {
                Console.WriteLine($"Ingrese ultimos 5 resultados de {nombre} (G / E / P)");
                almacenarResultados(resultado);
            }

            void cambiarRespuesta(ref string res) 
            {
                if (res == "Si")
                {
                    res = "No";
                }
                else 
                {
                    res = "Si";
                }
            }

            void cargaEnfretamientos() 
            {
                int ganadosLocal, partidosEmpatados, ganadosVisitante;
                Console.WriteLine($"Ingrese partidos ganados por {nombreLocal}");
                ganadosLocal = int.Parse(Console.ReadLine());
                Console.WriteLine("Ingrese partidos empatados entre si");
                partidosEmpatados = int.Parse(Console.ReadLine());
                Console.WriteLine($"Ingrese partidos ganados por {nombreVisitante}");
                ganadosVisitante = int.Parse(Console.ReadLine());
            }

            void confirmarDatos(string nombre, string baja, string competencia, string necesidad, double efectividad, char[] resultado)
            {
                do
                {
                    Console.WriteLine("Confirmar datos? (S/N)");
                    confirmacion = char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();
                    if (confirmacion == 'N')
                    {
                        menuModificar();
                        opcionModificar = Console.ReadKey().KeyChar;
                        eleccionModificar(ref nombre, ref baja, ref competencia, ref necesidad);
                        Console.Clear();
                        mostrarIngresos(nombre, baja, competencia, necesidad, efectividad, resultado);
                    }
                    else
                    {
                        Console.WriteLine("Opcion incorrecta, ingrese otra opcion");
                    }
                } while (confirmacion != 'S');
            }
       
            char analizarOtro() 
            {
                do
                {
                    Console.WriteLine("Desea analizar otro partido?");
                    Console.WriteLine("1. Si");
                    Console.WriteLine("2. No");
                    seguir = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    if (seguir != '1' && seguir != '2')
                    {
                        Console.Clear();
                    }
                } while (seguir != '1' && seguir != '2');
                return seguir;
            }
            //Programa principal
            do
            {
                Console.Clear();

                //Equipo local 
                nombreLocal = ingresoNombre("Equipo local");
                ventajaLocal += menuSiNo("Tiene bajas importantes para el encuentro?(Convocatoria / lesion / suspension)",10 ,20);
                bajaLocal = respuestaMenu();
                ventajaLocal += menuSiNo("Competencia internacional proxima", 10, 20);
                competenciaLocal = respuestaMenu(); 
                ventajaLocal += menuSiNo("Tiene necesidad del resultado?", 20, 10);
                necesidadLocal = respuestaMenu();
                Console.Clear();

                ingresoResultados(nombreLocal, resultados1);
                efectividadLocal = calcularPorcentajeEfectividad();

                Console.Clear();
                mostrarIngresos(nombreLocal,bajaLocal,competenciaLocal,necesidadLocal, efectividadLocal, resultados1);
                confirmarDatos(nombreLocal, bajaLocal, competenciaLocal, necesidadLocal, efectividadLocal, resultados1);
                Console.Clear() ;

                //Equipo visitante
                nombreVisitante = ingresoNombre("Equipo visitante");
                ventajaVisitante += menuSiNo("Tiene bajas importantes para el encuentro?(Convocatoria / lesion / suspension)", 10, 20);
                bajaVisitante = respuestaMenu();
                ventajaVisitante += menuSiNo("Competencia internacional proxima", 10, 20);
                competenciaVisitante = respuestaMenu();
                ventajaVisitante += menuSiNo("Tiene necesidad del resultado?", 20, 10);
                necesidadVisitante = respuestaMenu();
                ingresoResultados(nombreVisitante, resultados2);
                Console.Clear();
                efectividadVisitante = calcularPorcentajeEfectividad();

                Console.Clear();
                mostrarIngresos(nombreVisitante, bajaVisitante, competenciaVisitante, necesidadVisitante, efectividadVisitante, resultados2);
                confirmarDatos(nombreVisitante, bajaVisitante, competenciaVisitante, necesidadVisitante, efectividadVisitante, resultados2);

                Console.Clear();
                
                //GENERAL
                porcentajeLocal = calcularPorcentaje(resultados1);
                porcentajeVisitante = calcularPorcentaje(resultados2);
                Console.Clear();
                comparar(efectividadLocal, efectividadVisitante, "efectividad en el torneo local");
                comparar(porcentajeLocal, porcentajeVisitante, "victorias en los ultimos 5 partidos");
                compararVentajas();
                analizarOtro();
            }
            while (seguir == '1');
        }
    }
}
