using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDapperSql.Models
{
    public class Producto
    {
        public string CodigoArticulo { get; set; }
        [StringLength(40)]
        [Required]
        public string Seccion { get; set; }
        [Required]
        public string NombreArticulo { get; set; }
        [Range(0.01,10000,ErrorMessage ="El monto del articulo no es permitido debe ser mayor a 0,01 y menor a 10.000")]
        public float Precio { get; set; }
        public DateTime Fecha { get; set; }
        public string Importado { get; set; }
        public string PaisDeOrigen { get; set; }
        public string Foto { get; set; }

    }
}
