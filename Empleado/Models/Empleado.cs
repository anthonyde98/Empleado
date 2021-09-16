using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Empleado.Models
{
    public class Empleado
    {
        public int Codigo { set; get; }
        public string Nombre { set; get; }
        public decimal SueldoBruto { set; get; }
        public DateTime FechaIngreso { set; get; }
    }
}
