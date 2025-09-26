using Cajero;
using System.ComponentModel;
using System.Linq;

//Ruta del archivo en el que se guarda la información de las cuentas registradas
string RutaArchivo = "C:\\Users\\eandr\\source\\repos\\Cajero\\Usuarios.txt";
string RutaMovimientos = "C:\\Users\\eandr\\source\\repos\\Cajero\\Movimientos.txt";

CajeroAutomatico Cajero1 = new CajeroAutomatico();

//Instanciación y creación de datos de usuario
Usuario Usuario1 = new Usuario();
Usuario Usuario2 = new Usuario();
Usuario Usuario3 = new Usuario();

Usuario1.Identificacion = "1010067583";
Usuario1.NombreCompleto = "Erika Andrea Gonzalez Ramos"; 
Usuario1.Clave = "MTIzNA=="; //1234 
Usuario1.Saldo= 2000000.0m;

Usuario2.Identificacion = "1018510110";
Usuario2.NombreCompleto = "Lesdy Daniela Gonzalez Ramos";
Usuario2.Clave = "MTIzNA=="; //1234 
Usuario2.Saldo = 1000000.0m;

Usuario3.Identificacion = "1233492717";
Usuario3.NombreCompleto = "Marlon Aswin Baldermar Niño Turriago";
Usuario3.Clave = "MTIzNA=="; //1234 
Usuario3.Saldo = 200000.0m;

try
{
    if (!File.Exists(RutaArchivo))
    {
        //Si el archivo no existe lo crea y agrega la línea de encabezado y usuarios
        using (StreamWriter sw = File.CreateText(RutaArchivo))
        {
            sw.WriteLine("Identificación".PadRight(20) + "Nombre Completo".PadRight(40) + "Clave".PadRight(10) + "Saldo".PadRight(30));
            sw.WriteLine(Usuario1.FormatearUsuario(Usuario1));
            sw.WriteLine(Usuario2.FormatearUsuario(Usuario2));
            sw.WriteLine(Usuario3.FormatearUsuario(Usuario3));
        }
    }
    else
    {
        Cajero1.UsuariosALista();
    }

 
    if (!File.Exists(RutaMovimientos))
    {
        using (StreamWriter sw = File.CreateText(RutaMovimientos))
        {
            sw.WriteLine("FechaHora".PadRight(30) + "IdUsuario".PadRight(25) + "Tipo de movimiento".PadRight(28) + "Monto".PadRight(20)+ "Saldo Anterior".PadRight(28)+"Saldo Nuevo".PadRight(30));
        }
    }

    Cajero1.Inicio();



}
catch (Exception ex)
{
    Console.WriteLine("Ocurrió un error: " + ex.Message);
}

Console.ReadKey(true);

