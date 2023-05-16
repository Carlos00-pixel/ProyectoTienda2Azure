using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using ProyectoTienda2.Extensions;
using ProyectoTienda2.Filters;
using ProyectoTienda2.Repositories;
using ProyectoTienda2.Services;
using PyoyectoNugetTienda;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoTienda2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ServiceApi service;
        private ServiceStorageBlobs serviceBlob;
        private string containerName = "proyectotienda";
        public HomeController
            (ILogger<HomeController> logger, ServiceApi service,
            ServiceStorageBlobs serviceBlob)
        {
            _logger = logger;
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        public async Task<IActionResult> Index(int? idfavorito)
        {
            if(idfavorito != null)
            {
                List<int> favoritos;
                if(HttpContext.Session.GetObject<List<int>>("FAVORITOS") == null)
                {
                    favoritos = new List<int>();
                }
                else
                {
                    favoritos = HttpContext.Session.GetObject<List<int>>("FAVORITOS");
                }
                favoritos.Add(idfavorito.Value);
                HttpContext.Session.SetObject("FAVORITOS", favoritos);
            }
            DatosArtista infoArtes = await this.service.GetInfoArteAsync();
            foreach (InfoProducto c in infoArtes.listaProductos)
            {
                string blobName = c.Imagen;
                if (blobName != null)
                {
                    BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(this.containerName);
                    BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = this.containerName,
                        BlobName = blobName,
                        Resource = "b",
                        StartsOn = DateTimeOffset.UtcNow,
                        ExpiresOn = DateTime.UtcNow.AddHours(1),
                    };

                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                    var uri = blobClient.GenerateSasUri(sasBuilder);
                    c.Imagen = uri.ToString();
                }
            }
            return View(infoArtes);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> ProductosFavoritos(int? ideliminar)
        {
            List<int> idsFavoritos =
                HttpContext.Session.GetObject<List<int>>("FAVORITOS");
            if (idsFavoritos == null)
            {
                ViewData["MENSAJE"] = "No existen favoritos almacenados";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    idsFavoritos.Remove(ideliminar.Value);
                    if (idsFavoritos.Count == 0)
                    {
                        HttpContext.Session.Remove("FAVORITOS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("FAVORITOS", idsFavoritos);
                    }
                }
                DatosArtista infoArtes = this.service.GetInfoArteSession(idsFavoritos);
                foreach (InfoProducto c in infoArtes.listaProductos)
                {
                    string blobName = c.Imagen;
                    if (blobName != null)
                    {
                        BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(this.containerName);
                        BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                        BlobSasBuilder sasBuilder = new BlobSasBuilder()
                        {
                            BlobContainerName = this.containerName,
                            BlobName = blobName,
                            Resource = "b",
                            StartsOn = DateTimeOffset.UtcNow,
                            ExpiresOn = DateTime.UtcNow.AddHours(1),
                        };

                        sasBuilder.SetPermissions(BlobSasPermissions.Read);
                        var uri = blobClient.GenerateSasUri(sasBuilder);
                        c.Imagen = uri.ToString();
                    }
                }
                return View(infoArtes);
            }
        }

        public async Task<IActionResult> Details(int idproducto)
        {
            DatosArtista infoProduct = await this.service.FindInfoArteAsync(idproducto);

            string blobName = infoProduct.infoProducto.Imagen;
            if (blobName != null)
            {
                BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(this.containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = this.containerName,
                    BlobName = blobName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                var uri = blobClient.GenerateSasUri(sasBuilder);
                infoProduct.infoProducto.Imagen = uri.ToString();
            }

            return View(infoProduct);
        }

        public IActionResult NuevoProducto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoProducto
            (string titulo, int precio, string descripcion, IFormFile file, int idartista)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlob.UploadBlobAsync(this.containerName, blobName, stream);
            }

            idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await this.service.AgregarProductoAsync(titulo, precio, descripcion, blobName, idartista);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}