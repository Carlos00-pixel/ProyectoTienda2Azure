using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using PyoyectoNugetTienda;
using ProyectoTienda2.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace ProyectoTienda2.Controllers
{
    public class ClienteController : Controller
    {
        private ServiceApi service;
        private ServiceStorageBlobs serviceBlob;
        private string containerName = "proyectotienda";

        public ClienteController(ServiceApi service, ServiceStorageBlobs serviceBlob)
        {
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        public async Task<IActionResult> DetallesCliente(int idcliente)
        {
            idcliente = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            DatosArtista cliente = await this.service.FindCliente(idcliente);

            ViewData["PERFIL"] = await this.serviceBlob.GetBlobAsync(this.containerName, cliente.cliente.Imagen);

            return View(cliente);
        }

        public async Task<IActionResult> EditarCliente(int idcliente)
        {
            DatosArtista cliente = new DatosArtista();
            idcliente = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            cliente = await this.service.FindCliente(idcliente);
            ViewData["PERFIL"] = await this.serviceBlob.GetBlobAsync(this.containerName, cliente.cliente.Imagen);
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente
            (int idcliente, string nombre, string apellidos, string email, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlob.UploadBlobAsync(this.containerName, blobName, stream);
            }
            idcliente = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.service.EditarClienteAsync
                (idcliente, nombre, apellidos, email, blobName);
            return RedirectToAction("DetallesCliente");
        }

        public IActionResult ErrorAccesoCliente()
        {
            return View();
        }
    }
}
