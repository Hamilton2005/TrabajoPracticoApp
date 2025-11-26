using Microsoft.AspNetCore.Mvc;
using TrabajoPracticoApp.Data;
using TrabajoPracticoApp.Models;

namespace TrabajoPracticoApp.Controllers
{
    public class ComprasController : Controller
    {
        private readonly AppDbContext _context;

        public ComprasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult GuardarCompra([FromBody] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Compras.Add(compra);
                _context.SaveChanges();
                return Ok(new { mensaje = "Compra guardada" });
            }

            return BadRequest(new { mensaje = "Datos inválidos" });
        }
    }
}
