using Cajero;
using System.Linq;

//Ruta del archivo en el que se guarda la información de las cuentas registradas
string rutaArchivo = "C:\\Users\\eandr\\source\\repos\\Cajero\\Usuarios.txt";
string rutaMovimientos = "C:\\Users\\eandr\\source\\repos\\Cajero\\Movimientos.txt";

//Instanciación y creación de datos de usuario
Usuario Usuario1 = new Usuario();
Usuario Usuario2 = new Usuario();
Usuario Usuario3 = new Usuario();

Usuario1.Identificacion = "1010067583";
Usuario1.NombreCompleto = "Erika Andrea Gonzalez Ramos"; 
Usuario1.Clave = "1234"; 
Usuario1.Saldo= 2000000.0m;

Usuario2.Identificacion = "1018510110";
Usuario2.NombreCompleto = "Lesdy Daniela Gonzalez Ramos";
Usuario2.Clave = "1234";
Usuario2.Saldo = 1000000.0m;

Usuario3.Identificacion = "1233492717";
Usuario3.NombreCompleto = "Marlon Aswin Baldermar Niño Turriago";
Usuario3.Clave = "1234";
Usuario3.Saldo = 200000.0m;

try
{
    if (!File.Exists(rutaArchivo))
    {
        //Si el archivo no existe lo crea y agrega la línea de encabezado y usuarios
        using (StreamWriter sw = File.CreateText(rutaArchivo))
        {
            sw.WriteLine("Identificación".PadRight(20)+"Nombre Completo".PadRight(40)+"Clave".PadRight(10)+"Saldo");
            sw.WriteLine(Usuario1.FormatearUsuario(Usuario1));
            sw.WriteLine(Usuario2.FormatearUsuario(Usuario2));
            sw.WriteLine(Usuario3.FormatearUsuario(Usuario3));
        }
    }

    if (!File.Exists(rutaMovimientos))
    {
        using (StreamWriter sw = File.CreateText(rutaMovimientos))
        {
            sw.WriteLine("FechaHora".PadRight(20) + "IdUsuario".PadRight(25) + "Tipo de movimiento".PadRight(28) + "Monto".PadRight(20)+ "Saldo Anterior".PadRight(28)+"Saldo Nuevo");
        }
    }




}
catch (Exception ex)
{
    Console.WriteLine("Ocurrió un error: " + ex.Message);
}

