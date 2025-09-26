using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cajero
{
    public class CajeroAutomatico
    {
        public Usuario? UsuarioConectado;
        private const String RUTA_ARCHIVO = "C:\\Users\\eandr\\source\\repos\\Cajero\\Usuarios.txt";
        public List<Usuario> ListaUsuarios = new List<Usuario>();
        public void Inicio()
        {
            //Imprime mensaje inicial y solicita credenciales de ingreso
            Console.WriteLine("Bienvenido al cajero");
            Console.Write("Ingrese su número de cuenta: ");
            string IdUsuario = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Ingrese su clave: ");
            string Clave = Console.ReadLine();

            //Acepta acceso si ha ingresado las credenciales correctas
            if (EsValido(IdUsuario, Clave)==true)
            {
                Console.WriteLine($"Credenciales correctas, bienvenido {UsuarioConectado.NombreCompleto}");
                System.Threading.Thread.Sleep(2000);
                Console.Clear();
                HacerMenu();
            }

            else {
                Console.WriteLine("Acceso denegado.");
            }
        }

        public bool EsValido(string IdUsuario, string ClaveUsuario)
        {
            using (StreamReader sr = new StreamReader(RUTA_ARCHIVO))
            {   
                //Lee línea por línea el archivo de usuarios
                sr.ReadLine();
                string Linea = sr.ReadLine();
                                
                while (Linea != null)
                {
                   
                    if (Linea.Length >= 70)
                    {
                        //Guarda en variables la información del usuario
                        String Id = Linea.Substring(0, 20).Trim();
                        String Nombre = Linea.Substring(20, 40).Trim();
                        String Clave = Linea.Substring(60 ,10).Trim();
                        decimal Saldo = decimal.Parse(Linea.Substring(70, 30).Trim());

                        //Compara si está el id ingresado en los registros
                        if (IdUsuario == Id)
                        {
                            string ClaveIngresada = ClaveUsuario;
                            int Intentos = 4;
                            
                            while (Intentos !=0)
                            {
                                // Compara la clave ingresada con la guardadada   
                                if (ClaveIngresada == System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(Clave)))
                                {
                                    //Instancia Usuario con valores del usuario conectado
                                    Console.WriteLine("Usuario válido");
                                    UsuarioConectado = new Usuario();
                                    UsuarioConectado.Identificacion = Id;
                                    UsuarioConectado.NombreCompleto = Nombre ;
                                    UsuarioConectado.Clave = Clave;
                                    UsuarioConectado.Saldo= Saldo;
                                    return true;
                                    
                                }
                                else
                                {
                                    // Controla el número de intentos realizados
                                    Intentos--;
                                    if (Intentos == 0)
                                    { 
                                        Console.WriteLine("Ha superado el número de intentos");
                                        return false;
                                    }

                                   
                                    Console.Write($"Clave errónea. Le quedan {Intentos} intentos. Ingrese su clave nuevamente: ");
                                    ClaveIngresada = Console.ReadLine();
                            
                                }
                            }

                        }
                    
                    }
                    Linea = sr.ReadLine();

                }
                Console.WriteLine("El usuario no existe.");
                return false;
            }
            
        }
       
        public void HacerMenu()
        {
            char opcion;
            char[] opcionesValidas = ['1','2', '3', '4' , '5', '6'];
            bool existe= false;

            while (!existe)
            {
                try
                {
                    Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                         Bienvenid@                               |
            |                                                                                  |
            |                     Seleccione la opción del trámite que desea hacer             |
            |                                                                                  |
            |                  1. Depósito o consignación                                      |
            |                  2. Retiro                                                       |
            |                  3. Consulta de saldo                                            | 
            |                  4. Consulta úlitmos 5 movimientos                               |
            |                  5. Cambio de clave                                              |
            |                  6. Cerrar sesión                                                |
            |                                                                                  |
            |                                                                                  |
            |__________________________________________________________________________________|                                                                                           

                        Ingrese el número ->");

                    opcion = char.Parse(Console.ReadLine());

                    switch (opcion)
                    {
                        case '1':
                            Deposito();
                            ActualizarUsuarios();
                            existe = true;
                            break;

                        case '2':

                            Retiro();
                            existe = true;
                            break;

                        case '3':

                            ConsultaSaldo();
                            existe = true;
                            break;

                        case '4':

                            VerMovimientos();
                            existe = true;
                            break;

                        case '5':

                            CambiarClave();
                            existe = true;
                            break;

                        case '6':

                            existe = true;
                            Console.WriteLine("Gracias por usar nuestros servicios, presione cualquier tecla para terminar");
                            Console.ReadKey();
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Opción incorrecta");
                            break;

                    } 
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Opción incorrecta");

                }

            }               
        }

        public void Deposito()
        {

        }
        public void Retiro()
        {

        }


        public void ActualizarUsuarios()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(RUTA_ARCHIVO, append: false))
                {
                    sw.WriteLine("Identificación".PadRight(20) + "Nombre Completo".PadRight(40) + "Clave".PadRight(10) + "Saldo".PadRight(30));

                    for (int i = 0; i < ListaUsuarios.Count; i++)
                    {
                        if (ListaUsuarios[i].Identificacion == UsuarioConectado.Identificacion)
                        {
                            ListaUsuarios[i] = UsuarioConectado;

                            sw.WriteLine(UsuarioConectado.FormatearUsuario(UsuarioConectado));

                        }
                        else
                        {
                            sw.WriteLine(ListaUsuarios[i].FormatearUsuario(ListaUsuarios[i]));
                        }

                    }


                }
            }
            catch (Exception ex)
                {
                Console.Clear();
                Console.WriteLine("Opción incorrecta");

            }
        }

        //Este método lee el archivo de Usuario.txt y guarda los registros en una lista
        public void UsuariosALista()
        {
            using (StreamReader sr = new StreamReader(RUTA_ARCHIVO))
            {
                try
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {

                        if (linea.StartsWith("Identificación"))
                        {
                            continue;
                        }

                        Usuario UsuarioTemporal = new Usuario();
                        UsuarioTemporal.Identificacion = linea.Substring(0, 20).Trim();
                        UsuarioTemporal.NombreCompleto = linea.Substring(20, 40).Trim();
                        UsuarioTemporal.Clave = linea.Substring(60, 10).Trim();
                        UsuarioTemporal.Saldo = decimal.Parse(linea.Substring(70, 30).Trim());

                        ListaUsuarios.Add(UsuarioTemporal);

                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Opción incorrecta");

                }


            }
          
        }

        public void ConsultaSaldo()
        {

        }

        public void VerMovimientos()
        {

        }
        public void CambiarClave()
        {

        }


    }
}
