using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoTienda2.Repositories;
using System.Numerics;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using PyoyectoNugetTienda;

namespace ProyectoTienda2.Controllers
{
    public class ClienteController : Controller
    {
        private RepositoryCliente repo;

        public ClienteController(RepositoryCliente repo)
        {
            this.repo = repo;
        }

        public IActionResult DetallesCliente(int idcliente)
        {
            DatosArtista cliente = this.repo.FindCliente(idcliente);
            return View(cliente);
        }

        public IActionResult EditarCliente(int idcliente)
        {
            DatosArtista cliente = this.repo.FindCliente(idcliente);
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente
            (int idcliente, string nombre, string apellidos, string email, string imagen)
        {
            DatosArtista cliente = new DatosArtista();
            idcliente = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.repo.EditarClienteAsync
                (idcliente, nombre, apellidos, email, imagen);
            return RedirectToAction("DetallesCliente");
        }

        public IActionResult ErrorAccesoCliente()
        {
            return View();
        }
    }
}
