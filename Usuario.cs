using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajero
{
    public class Usuario
    {
        public string Identificacion {  get; set; }
        public string NombreCompleto { get; set; }

        public string Clave {  get; set; }

        public decimal Saldo {  get; set; }

        public string FormatearUsuario(Usuario usuario)
        {
            //Este método organiza los datos del usuario para que se alinie con el encabezado
            return usuario.Identificacion.PadRight(20) +
                   usuario.NombreCompleto.PadRight(40) +
                   usuario.Clave.PadRight(10) +
                   usuario.Saldo.ToString().PadRight(30);
        }
    }
}
