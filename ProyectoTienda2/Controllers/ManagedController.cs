using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoTienda2.Services;
using PyoyectoNugetTienda;
using System.Security.Claims;

namespace ProyectoTienda2.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceApi service;
        private ServiceStorageBlobs serviceBlob;
        private string containerName = "proyectotienda";

        public ManagedController(ServiceApi service, ServiceStorageBlobs serviceBlob)
        {
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        #region CLIENTE

        public IActionResult LoginCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginCliente(string email
            , string password)
        {
            Cliente cliente =
                await this.service.ExisteCliente(email, password);
            if (cliente != null)
            {
                ClaimsIdentity identity =
               new ClaimsIdentity
               (CookieAuthenticationDefaults.AuthenticationScheme
               , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim
                    (new Claim(ClaimTypes.Name, cliente.Email));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()));
                identity.AddClaim
                    (new Claim("Nombre", cliente.Nombre));
                identity.AddClaim
                    (new Claim("Apellidos", cliente.Apellidos));

               string imagenPerfil = await this.serviceBlob.GetBlobAsync(this.containerName, cliente.Imagen);
                identity.AddClaim
                    (new Claim("Imagen", imagenPerfil));
                identity.AddClaim
                    (new Claim(ClaimTypes.Role, "Cliente"));

                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public IActionResult RegisterCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCliente
            (string nombre, string apellidos, string email, string password, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlob.UploadBlobAsync(this.containerName, blobName, stream);
            }
            await this.service.RegistrarClienteAsync
                (nombre, apellidos, email, password, blobName);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
        }

        #endregion

        #region ARTISTA

        public IActionResult LoginArtista()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginArtista(string email
            , string password)
        {

            Artista artista =
            await this.service.ExisteArtista(email, password);
            if (artista != null)
            {
                ClaimsIdentity identity =
               new ClaimsIdentity
               (CookieAuthenticationDefaults.AuthenticationScheme
               , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim
                    (new Claim(ClaimTypes.Name, artista.Email));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, artista.IdArtista.ToString()));
                identity.AddClaim
                    (new Claim("Nombre", artista.Nombre));
                if (artista.Apellidos == null)
                {
                    identity.AddClaim
                    (new Claim("Apellidos", ""));
                }
                else
                {
                    identity.AddClaim
                    (new Claim("Apellidos", artista.Apellidos));
                }
                string imagenPerfil = await this.serviceBlob.GetBlobAsync(this.containerName, artista.Imagen);
                identity.AddClaim
                    (new Claim("Imagen", imagenPerfil));
                identity.AddClaim
                    (new Claim(ClaimTypes.Role, "Artista"));

                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , user);
                string blobName = artista.Imagen;
                if (blobName != null)
                {
                    BlobContainerClient blobContainerClient = await this.serviceBlob.GetContainerAsync(containerName);
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
                    ViewData["URI"] = uri;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public IActionResult RegisterArtista()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterArtista
            (string nombre, string apellidos, string nick, string descripcion,
            string email, string password, IFormFile file, string imagenfondo)
        {
            string blobName = file.FileName;

            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlob.UploadBlobAsync(this.containerName, blobName, stream);
            }
            imagenfondo = "default.jpg";
            await this.service.RegistrarArtistaAsync
                (nombre, apellidos, nick, descripcion,
                email, password, blobName, imagenfondo);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
        }

        #endregion

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
