using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoApp.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Evento { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Precio { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
