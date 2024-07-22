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
            string nombreLocal, nombreVisitante;
            string bajaLocal , competenciaLocal, necesidadLocal;
            string bajaVisitante, competenciaVisitante, necesidadVisitante;
            int puntosLocal, puntosVisitante;
            char[] resultados1 = new char[5];
            char[] resultados2 = new char[5];
            double porcentajeLocal, porcentajeVisitante;
            //Empieza con 10 por ser local
            int ventajaLocal = 15;
            int ventajaVisitante = 0;

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
          
            //Ingreso de resultados de equipos en un arreglo. 
            void ingresoResultados(char[] resultado) 
            {
                for (int i = 0; i < 5; i++)
                {
                    resultado[i] = Console.ReadKey().KeyChar;
                    if (i != 4) 
                    {
                        Console.Write(" - ");
                    }
                }
                Console.WriteLine();
            }

            int bajasImportantes() 
            {
                Console.WriteLine("Tiene bajas importantes para el encuentro?(Convocatoria / lesion / suspension)");
                Console.WriteLine("1. Si");
                Console.WriteLine("2. No");
                respuesta = Console.ReadKey().KeyChar;
                Console.WriteLine();
                return respuesta == '1' ? 10 : 20;
            }

            string respuestaMenu() 
            {
                return (respuesta == '1') ? "Si": "No";
            }

            int competenciaInternacional() 
            {
                Console.WriteLine("Competencia internacional proxima");
                Console.WriteLine("1. Si");
                Console.WriteLine("2. No");
                respuesta = Console.ReadKey().KeyChar;
                Console.WriteLine();
                return respuesta == '1' ? 10 : 20;
            }

            int necesidadResultado() 
            {
                Console.WriteLine("Tiene necesidad del resultado?");
                Console.WriteLine("1. Si");
                Console.WriteLine("2. No");
                respuesta = Console.ReadKey().KeyChar;
                Console.WriteLine();
                return respuesta == '1' ? 20 : 10;
            }

            int diferenciaNotoriaPorcentajes() 
            {
                return Math.Abs(porcentajeLocal - porcentajeVisitante) > 20 ? 20 : 0;
            }

            int diferenciaNotoria(int local, int visitante,int condicion, int valorTrue, int valorFalse) 
            {
                return (Math.Abs(local - visitante) > condicion) ? valorTrue : valorFalse;
            }

            void compararPuntos()
            {
                if (puntosLocal > puntosVisitante)
                {
                    Console.WriteLine($"{nombreLocal} tiene mas puntos ({puntosLocal} vs {puntosVisitante})");
                    ventajaLocal += diferenciaNotoria(puntosLocal, puntosVisitante, 7, 20, 0);
                }
                else if (puntosLocal < puntosVisitante)
                {
                    Console.WriteLine($"{nombreVisitante} tiene mas puntos ({puntosVisitante} vs {puntosLocal})");
                    ventajaVisitante += diferenciaNotoria(puntosLocal, puntosVisitante, 7, 20, 0);
                }
                else
                {
                    Console.WriteLine("Igualan en puntos");
                }

            }

            void compararPorcentajes()
            {
                if (porcentajeLocal > porcentajeVisitante)
                {
                    Console.WriteLine($"{nombreLocal} tiene mayor porcentaje de victorias ({porcentajeLocal}% vs {porcentajeVisitante}%)");
                    ventajaLocal += diferenciaNotoriaPorcentajes();
                }
                else if (porcentajeLocal < porcentajeVisitante)
                {
                    Console.WriteLine($"{nombreVisitante} tiene mayor porcentaje de victorias ({porcentajeVisitante}% vs {porcentajeLocal}%)");
                    ventajaVisitante += diferenciaNotoriaPorcentajes();
                }
                else
                {
                    Console.WriteLine("Igualan en porcentaje de victorias");
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

            void mostrarIngresosLocal() 
            {
                Console.WriteLine($"Nombre equipo local: {nombreLocal}");
                Console.WriteLine($"Bajas importantes: {bajaLocal}");
                Console.WriteLine($"CompetenciaInternacional: {competenciaLocal} ");
                Console.WriteLine($"Necesidad de resultado: {necesidadLocal} ");
                Console.WriteLine($"Puntos en liga actual: {puntosLocal} ");
                Console.Write($"Ultimos resultados: ");
                for (int i = 0; i < 5; i++)
                {
                    if (i < 4)
                    {
                        Console.Write($"{resultados1[i]} - ");
                    }
                    else 
                    {
                        Console.Write($"{resultados1[i]} ");
                    }
                }
                Console.WriteLine();
            }
            void mostrarIngresosVisitante()
            {
                Console.WriteLine($"Nombre equipo local: {nombreVisitante}");
                Console.WriteLine($"Bajas importantes: {bajaVisitante}");
                Console.WriteLine($"CompetenciaInternacional: {competenciaVisitante} ");
                Console.WriteLine($"Necesidad de resultado: {necesidadVisitante} ");
                Console.WriteLine($"Puntos en liga actual: {puntosVisitante} ");
                Console.Write($"Ultimos resultados: ");
                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"{resultados2[i]} - ");
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

            void eleccionModificar()
            {
                switch (opcionModificar)
                {
                    case '1': 
                        nombreLocal = ingresoNombre("Equipo local");
                        break;
                    case '2': 
                        bajasImportantes();
                        break;
                    case '3': 
                        competenciaInternacional();
                        break;
                    case '4': 
                        necesidadResultado();
                        break;
                    case '5': 
                        ingresoPuntosLocal(); ;
                        break;
                    case '6': 
                        ingresoResultadoslocal();
                        break;
                    default: Console.WriteLine("Opcion incorrecta");
                        break;
                }
            }

            void ingresoPuntosLocal() 
            {
                Console.WriteLine("Indique cantidad de puntos liga actual");
                puntosLocal = int.Parse(Console.ReadLine());
            }
            void ingresoResultadoslocal() 
            {
                Console.WriteLine($"Ingrese ultimos 5 resultados de {nombreLocal} (G / E / P)");
                ingresoResultados(resultados1);
            }
            //Programa principal
            do
            {
                //Equipo local 
                nombreLocal = ingresoNombre("Equipo local");
                ventajaLocal += bajasImportantes();
                bajaLocal = respuestaMenu();
                ventajaLocal += competenciaInternacional();
                competenciaLocal = respuestaMenu(); 
                ventajaLocal += necesidadResultado();
                necesidadLocal = respuestaMenu();

                ingresoPuntosLocal();

                ingresoResultadoslocal();

                Console.Clear();
                mostrarIngresosLocal();
                Console.WriteLine("Confirmar datos? (S/N)");
                confirmacion = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (confirmacion == 'N' || confirmacion == 'n') 
                {
                    menuModificar();
                    opcionModificar = Console.ReadKey().KeyChar;
                    eleccionModificar();
                }
                

                //Equipo visitante
                nombreVisitante = ingresoNombre("Equipo visitante");
                ventajaVisitante += bajasImportantes();
                bajaVisitante = respuestaMenu();
                ventajaVisitante += competenciaInternacional();
                competenciaVisitante = respuestaMenu();
                ventajaVisitante += necesidadResultado();
                necesidadVisitante = respuestaMenu();

                Console.WriteLine("Indique cantidad de puntos liga actual");
                puntosVisitante = int.Parse(Console.ReadLine());

                Console.WriteLine($"Ingrese ultimos 5 resultados de {nombreVisitante} (G / E / P)");
                ingresoResultados(resultados2);

                mostrarIngresosVisitante();

                porcentajeLocal = calcularPorcentaje(resultados1);
                porcentajeVisitante = calcularPorcentaje(resultados2);
                
                Console.Clear();
                
                compararPorcentajes();
                compararPuntos();
                compararVentajas();
                Console.WriteLine("Desea analizar otro partido?");
                Console.WriteLine("1. Si");
                Console.WriteLine("2. No");
                seguir = Console.ReadKey().KeyChar;
            }
            while (seguir == '1');
        }
    }
}
