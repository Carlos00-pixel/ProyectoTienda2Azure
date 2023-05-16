using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoTienda2.Repositories;
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

            string blobName = cliente.cliente.Imagen;
            if (blobName != null)
            {
                BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = blobName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                var uri = blobClient.GenerateSasUri(sasBuilder);
                ViewData["URI"] = uri;
            }
            return View(cliente);
        }

        public async Task<IActionResult> EditarCliente(int idcliente)
        {
            DatosArtista cliente = new DatosArtista();

            idcliente = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            cliente = await this.service.FindCliente(idcliente);
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
