using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProyectoTienda2.Filters;
using ProyectoTienda2.Repositories;
using ProyectoTienda2.Services;
using PyoyectoNugetTienda;
using System.Drawing;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoTienda2.Controllers
{
    public class ArtistaController : Controller
    {
        private ServiceApi service;
        private ServiceStorageBlobs serviceBlob;
        private string containerName = "proyectotienda";

        public ArtistaController(ServiceApi service, ServiceStorageBlobs serviceBlob)
        {
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        public async Task<IActionResult> DetallesArtista(int idartista)
        {

            DatosArtista artista = await this.service.DetailsArtistaAsync(idartista);
            ViewData["CONTARPRODUCT"] = artista.listaProductos.Count;
            string blobName = HttpContext.User.FindFirst("Imagen").Value;
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
            return View(artista);
        }

        public async Task<IActionResult> PerfilArtista(int idartista, int? idInfoArteEliminado)
        {
            DatosArtista artista = new DatosArtista();

            if (idInfoArteEliminado != null)
            {
                await this.service.DeleteInfoArteAsync(idInfoArteEliminado.Value);
            }

            artista = await this.service.DetailsArtistaAsync(idartista);
            ViewData["CONTARPRODUCT"] = artista.listaProductos.Count;

            string blobImagenArtist = HttpContext.User.FindFirst("Imagen").Value;
            if (blobImagenArtist != null)
            {
                BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(this.containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobImagenArtist);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = this.containerName,
                    BlobName = blobImagenArtist,
                    Resource = "a",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                var uri = blobClient.GenerateSasUri(sasBuilder);
                ViewData["URI"] = uri;
            }

            //foreach (InfoProducto c in artista.listaProductos)
            //{
            //    string blobName = c.Imagen;
            //    if (blobName != null)
            //    {
            //        BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(this.containerName);
            //        BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            //        BlobSasBuilder sasBuilder = new BlobSasBuilder()
            //        {
            //            BlobContainerName = this.containerName,
            //            BlobName = blobName ,
            //            Resource = "b",
            //            StartsOn = DateTimeOffset.UtcNow,
            //            ExpiresOn = DateTime.UtcNow.AddHours(1),
            //        };

            //        sasBuilder.SetPermissions(BlobSasPermissions.Read);
            //        var uri = blobClient.GenerateSasUri(sasBuilder);
            //        c.Imagen = uri.ToString();
            //    }
            //}

            return View(artista);
        }

        public async Task<IActionResult> EditarPerfilArtista(int idartista)
        {
            DatosArtista artista = await this.service.DetailsArtistaAsync(idartista);
            return View(artista);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfilArtista
            (int idartista, string nombre, string apellidos, string nick, string descripcion,
            string email, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlob.UploadBlobAsync(this.containerName, blobName, stream);
            }
            idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.service.PerfilArtista
                (idartista, nombre, apellidos, nick, descripcion,
                email, blobName);

            return View("PerfilArtista", new { idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) });
        }

        //[AuthorizeUsuarios]
        //public async Task<IActionResult> Delete(int idInfoArte)
        //{
        //    string token = HttpContext.Session.GetString("TOKEN");
        //    await this.service.DeleteInfoArteAsync(idInfoArte);
        //    return View("PerfilArtista", new { idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) });
        //}
    }
}
