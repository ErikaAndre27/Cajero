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
        private const String RUTA_ARCHIVO = "./Usuarios.txt";
        private const string RUTA_MOVIMIENTOS = "./Movimientos.txt";

        public List<Usuario> ListaUsuarios = new List<Usuario>();
        public List<string[]> ListaMovimientos = new List<string[]>();

        //Este método muestra el mensaje inicial al usuario, si es ture muestra el menú, si no no permite el acceso.
        public void Inicio()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Clear();
            //Imprime mensaje inicial y solicita credenciales de ingreso

            Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                               Bienvenid@ al cajero                               |
            |__________________________________________________________________________________|                                                                                           

                        Ingrese su número de cuenta ->");
            
            string IdUsuario = Console.ReadLine();
            Console.WriteLine();
            Console.Write(@"

                        Ingrese su clave ->");
            string Clave = Console.ReadLine();
            Console.Clear();

            //Acepta acceso si ha ingresado las credenciales correctas
            if (EsValido(IdUsuario, Clave)==true)
            {
               
                Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                           Credenciales correctas                                 |
            |                                                                                  |
            |                   Bienvenid@ {UsuarioConectado.NombreCompleto.PadRight(52)}|
            |                                                                                  |
            |__________________________________________________________________________________|");
                System.Threading.Thread.Sleep(3000);
                Console.Clear();
                HacerMenu();
            }

            else {
                Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                Acceso denegado                                   |
            |                                                                                  |
            |__________________________________________________________________________________|");
            }
        }

        //Este método valida los datos ingresados (número de cuenta y clave) y devuelve un booleano para el método de inicio.
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
                                    Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                              Usuario válido                                      |
            |                                                                                  |
            |__________________________________________________________________________________|");
                                    
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
                                        Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                           Ha superado el número de intentos                      |
            |                                                                                  |
            |__________________________________________________________________________________|");
                                      
                                        return false;
                                    }

                                   
                                    Console.Write(@$"
                        Clave errónea. Le quedan {Intentos} intentos. Ingrese su clave nuevamente:");
                                    ClaveIngresada = Console.ReadLine();
                            
                                }
                            }

                        }
                    
                    }
                    Linea = sr.ReadLine();

                }
                Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                              El usuario no existe.                               |
            |                                                                                  |
            |__________________________________________________________________________________|");
               
                return false;
            }
            
        }

        //Método para mostrar el menú una vez el Usuario se haya autenticado exitosamente
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
            |                  4. Consulta últimos 5 movimientos                               |
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
                            break;

                        case '2':

                            Retiro();
                            break;

                        case '3':

                            ConsultaSaldo();
                            break;

                        case '4':

                            VerMovimientos();
                            break;

                        case '5':

                            CambiarClave();
                            break;

                        case '6':

                            existe = true;
                            Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                    Gracias por usar nuestros servicios                           |
            |                                                                                  |
            |    *presione cualquier tecla para terminar                                       |
            |__________________________________________________________________________________|");
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

        //Este método permite que el usuario aumente en saldo en la cuenta, hasta un máximo de 2'000.000
         public void Deposito()
        {
            Console.Clear();
            try
            {
                
                Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                           Depósito o Consignación                                |
            |                                                                                  |
            |                         Ingrese el monto a depositar                             |
            |                                                                                  |
            |                (Recuerde que el máximo a depositar es 2'000.000)                 |
            |__________________________________________________________________________________|                                                                                           

                        Ingrese el valor ->");

                decimal deposito = decimal.Parse(Console.ReadLine());
                string monto = deposito.ToString();

                if (deposito > 0 && deposito < 2000000)
                {
                    string TipoMovimiento = "Depósito";
                    decimal SaldoAnterior = UsuarioConectado.Saldo;
                    UsuarioConectado.Saldo += deposito;

                    Console.Write($@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                Depósito Existoso                                 |
            |                                                                                  |
            |                       Valor depositado: {deposito.ToString().PadRight(41)}|
            |                       Saldo actual: {UsuarioConectado.Saldo.ToString().PadRight(45)}|
            |                                                                                  |
            |__________________________________________________________________________________|   ");

                    AgregarMovimiento(TipoMovimiento, monto, SaldoAnterior);
                    ActualizarUsuarios();
                    Console.ReadKey(true);
                }
                else
                {   //Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Console.Clear();
                    Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                     ERROR                                        |
            |                                                                                  |
            |                       El valor ingresado está fuera del rango                    |
            |                                                                                  |
            |                (Recuerde que el máximo a depositar es 2'000.000)                 |
            |__________________________________________________________________________________|");
                    
                    Console.ReadKey(true);
                    
                }

            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Valor inválido");

            }
        }

        //Este método solicita al usuario un valor a retira, debe estar en el rango 10.000 y 7.000.000, y tener disponible ese saldo en la cuenta
        public void Retiro()
        {
            Console.Clear();
            try
            {

                Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                    Retiro                                        |
            |                                                                                  |
            |                         Ingrese el monto a retirar                               |
            |                                                                                  |
            |            (Mínimo 10.000 - máximo 7'000.000 por transferencia)                  |
            |                                                                                  |
            |__________________________________________________________________________________|                                                                                           

                        Ingrese el valor a retirar->");

                decimal Retiro = decimal.Parse(Console.ReadLine());
                string Monto = Retiro.ToString();

                if (Retiro >= 10000 && Retiro <= 7000000 && Retiro <= UsuarioConectado.Saldo)
                {
                    string TipoMovimiento = "Retiro";
                    decimal SaldoAnterior = UsuarioConectado.Saldo;
                    UsuarioConectado.Saldo -= Retiro;

                    Console.Write($@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                Retiro   Existoso                                 |
            |                                                                                  |
            |                       Valor retiro: {Retiro.ToString().PadRight(45)}|
            |                       Saldo actual: {UsuarioConectado.Saldo.ToString().PadRight(45)}|
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|   ");

                    AgregarMovimiento(TipoMovimiento, Monto, SaldoAnterior);
                    ActualizarUsuarios();
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else
                {
                    if (Retiro > UsuarioConectado.Saldo)
                    {
                        Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                     ERROR                                        |
            |                                                                                  |
            |                             ¡Fondos insuficientes!                               |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|");

                        Console.ReadKey(true);
                        Console.Clear();
                    }
                    else
                    {

                        Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                     ERROR                                        |
            |                                                                                  |
            |                       El valor ingresado está fuera del rango                    |
            |                                                                                  |
            |            (Recuerde: Mínimo 10.000 - máximo 7'000.000 por transferencia)        |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|");

                        Console.ReadKey(true);
                        Console.Clear();

                    }

                }

            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Valor inválido");

            }
        }

        //Este método se implementa para actualizar el archivo al hacer alguna modificación del archivo Usuarios
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

        //Este método devuelve el saldo actual del usuario 
        public void ConsultaSaldo()
        {
            Console.Clear();
            try
            {

                Console.Write(@$"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                 Consulta de saldo                                |
            |                                                                                  |
            |                         Saldo actual: ${UsuarioConectado.Saldo.ToString().PadRight(42)}|
            |                                                                                  |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|");

                Console.ReadKey(true);
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Valor inválido");

            }
        }

        //Este método agrega una línea al archivo de movimientos cada vez que se hace una transacción de retiro o depósito
        public void AgregarMovimiento(string TipoMovimiento, string monto, decimal SaldoAnterior)
        {
            using (StreamWriter sw = File.AppendText(RUTA_MOVIMIENTOS))
            {
                string FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").PadRight(30);
                string SaldoAnteriorString = SaldoAnterior.ToString();
                string SaldoNuevoString = UsuarioConectado.Saldo.ToString();
                sw.WriteLine(FechaHora + UsuarioConectado.Identificacion.PadRight(25) + TipoMovimiento.PadRight(28) + monto.PadRight(20) + SaldoAnteriorString.PadRight(28) + SaldoNuevoString.PadRight(30));
            }
        }

        //Este método se enplea para convertir el archivo de movimientos en una lista de arreglos
        public void MovimientosALista()
        {
            using (StreamReader sr = new StreamReader(RUTA_MOVIMIENTOS))
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
                        //sw.WriteLine("FechaHora".PadRight(30) + "IdUsuario".PadRight(25) + "Tipo de movimiento".PadRight(28) + "Monto".PadRight(20) + "Saldo Anterior".PadRight(28) + "Saldo Nuevo".PadRight(30));

                        string FechaHora = linea.Substring(0, 30).Trim();
                        string IdUsuario = linea.Substring(30, 25).Trim();
                        string TipoMovimiento = linea.Substring(55, 28).Trim();
                        string Monto = linea.Substring(83, 20).Trim();
                        string SaldoAnterior = linea.Substring(103, 28).Trim();
                        string SaldoNuevo = linea.Substring(131, 30).Trim();


                        string[] Movimiento = new string[6];

                        Movimiento[0] = FechaHora;
                        Movimiento[1] = IdUsuario;
                        Movimiento[2] = TipoMovimiento;
                        Movimiento[3] = Monto;
                        Movimiento[4] = SaldoAnterior;
                        Movimiento[5]= SaldoNuevo;
                        
                        ListaMovimientos.Add(Movimiento);

                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("No se pudo ejecutar la solicitud, ocurrió un error: " + ex);

                }


            }


        }

        //Este método devuelve al usuario una lista de los últimos 5 movimientos realizados en su cuenta
        public void VerMovimientos()
        {
            try 
            {
                MovimientosALista();
                var resultado = (from variable in ListaMovimientos
                                 where (variable[1] == UsuarioConectado.Identificacion)
                                 orderby DateTime.Parse(variable[0]) descending
                                 select variable).Take(5);
                Console.Clear();
                Console.WriteLine("_________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("A continuación puede observar la información de los últimos movimientos realizados en su cuenta");
                Console.WriteLine();

                foreach (var movimiento in resultado)
                {

                    Console.WriteLine($"Fecha y hora: {movimiento[0]} || Tipo de movimiento: {movimiento[2]} || Monto: {movimiento[3]} || Saldo anterior:{movimiento[4]} || Saldo nuevo: {movimiento[5]}");
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("*Presione cualquier tecla para volver al menú");
                Console.WriteLine();
                Console.ReadKey(true);
                Console.WriteLine("_________________________________________________________________________________");
                Console.Clear();

            }
            catch (Exception ex)
                {
                Console.Clear();
                Console.WriteLine("No se pudo ejecutar la solicitud, ocurrió un error: " + ex);

            }

        }
        public void CambiarClave()
        {
            try
            {
                Console.Clear();
                Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                Cambio de Clave                                   |
            |                                                                                  |
            |                    Recuerde que la clave debe tener 4 digitos                    |
            |                                                                                  |
            |__________________________________________________________________________________|                                                                                           

                        Ingrese la nueva clave ->");

                string ClaveNueva = Console.ReadLine();

                if (ClaveNueva.Length == 4 && ClaveNueva.All(char.IsDigit))
                {
                    Console.Write(@"
                        
                        Ingrese nuevamente la nueva clave ->");
                    string ClaveNueva2 = Console.ReadLine();

                    if (ClaveNueva == ClaveNueva2)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(ClaveNueva);
                        string base64 = Convert.ToBase64String(bytes);
                        UsuarioConectado.Clave = base64;
                        ActualizarUsuarios();
                        Console.Clear();
                        Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                       El cambio de Clave ha sido exitoso                         |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|");
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                      ERROR                                       |
            |                                                                                  |
            |           La clave ingresada no coincide o no cumple las condiciones.            |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |
            |__________________________________________________________________________________|");

                    }
                    Console.ReadKey(true);
                    Console.Clear();
                    
                }
                else
                {
                    Console.Write(@"
            ____________________________________________________________________________________
            |                                                                                  |
            |                                      ERROR                                       |
            |                                                                                  |
            |           La clave ingresada no coincide o no cumple las condiciones.            |
            |                                                                                  |
            |             *Presione cualquier tecla para volver al menú principal              |                                                                                  
            |__________________________________________________________________________________|");
                    Console.ReadKey(true);
                    Console.Clear();
                }



            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("No se pudo ejecutar la solicitud, ocurrió un error: " + ex);

            }

        }


    }
}
