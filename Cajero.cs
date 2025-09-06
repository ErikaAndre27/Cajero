using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public async Task Inicio()
        {
            //Imprime mensaje inicial y solicita credenciales de ingreso
            Console.WriteLine("Bienvenido al cajero");
            Console.Write("Ingrese su número de cuenta: ");
            string IdUsuario = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Ingrese su clave: ");
            string Clave = Console.ReadLine();

            //Acepta acceso si ha ingresado las credenciales correctas
            if (await EsValido(IdUsuario, Clave)==true)
            {
                Console.WriteLine($"Credenciales correctas, bienvenido {UsuarioConectado.NombreCompleto}");
            }

            else {
                Console.WriteLine("Acceso denegado.");
            }
        }

        public async Task<bool> EsValido(string IdUsuario, string ClaveUsuario)
        {
            using (StreamReader sr = new StreamReader(RUTA_ARCHIVO))
            {   
                //Lee línea por línea el archivo de usuarios
                await sr.ReadLineAsync();
                string Linea = await sr.ReadLineAsync();
                                
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
                            int Intentos = 3;
                            
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
                    Linea = await sr.ReadLineAsync();

                }
                Console.WriteLine("El usuario no existe.");
                return false;
            }
            
        }
        public void IniciarSesion(string id, string clave)
        {

        }
        public void HacerMenu()
        {
            Console.WriteLine("");

        }
    }
}
