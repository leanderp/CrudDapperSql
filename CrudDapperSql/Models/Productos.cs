using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDapperSql.Models
{
    public class Productos
    {
        public string CodigoArticulo { get; set; }
        public string Seccion { get; set; }
        public string NombreArticulo { get; set; }
        public float Precio { get; set; }
        public DateTime Fecha { get; set; }
        public string Importado { get; set; }
        public string PaisDeOrigen { get; set; }
        public string Foto { get; set; }

    }
}
