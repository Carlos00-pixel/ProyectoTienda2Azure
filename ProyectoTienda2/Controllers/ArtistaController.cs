using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProyectoTienda2.Repositories;
using PyoyectoNugetTienda;
using System.Drawing;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoTienda2.Controllers
{
    public class ArtistaController : Controller
    {
        private RepositoryArtista repo;
        private IMemoryCache _cache;

        public ArtistaController(RepositoryArtista repo, IMemoryCache _cache)
        {
            this.repo = repo;
            this._cache = _cache;
        }

        public IActionResult DetallesArtista(int idartista)
        {
            //string cacheKey = "IMAGEN";
            //if (!_cache.TryGetValue(cacheKey, out string imageUrl))
            //{
            //    // La URL de la imagen no existe en la caché, carga la URL de la imagen y guárdala en caché.
            //    imageUrl = HttpContext.Session.GetString("imagenDeFondo");
            //    _cache.Set(cacheKey, imageUrl);
            //}

            //ViewData["IMAGENFONDO"] = imageUrl;

            //string imagenDeFondo = HttpContext.Session.GetString("imagenDeFondo");
            //if (this._cache.Get("IMAGEN") == null)
            //{
            //    this._cache.Set("IMAGEN", imagenDeFondo);
            //    ViewData["IMAGENFONDO"] = this._cache.Get("IMAGEN");
            //}
            //else
            //{
            //    imagenDeFondo = this._cache.Get<string>("IMAGEN");
            //    ViewData["IMAGENFONDO"] = imagenDeFondo;
            //}

            var imagenDeFondo = HttpContext.Session.GetString("imagenDeFondo");
            ViewData["IMAGENFONDO"] = imagenDeFondo;

            DatosArtista artista = this.repo.DetailsArtista(idartista);
            ViewData["CONTARPRODUCT"] = artista.listaProductos.Count;
            return View(artista);
        }

        public async Task<IActionResult> PerfilArtista(int idartista, int? idInfoArteEliminado)
        {
            DatosArtista artista = new DatosArtista();

            if (idInfoArteEliminado != null)
            {
                await this.repo.DeleteInfoArteAsync(idInfoArteEliminado.Value);
            }

            artista = this.repo.DetailsArtista(idartista);
            ViewData["CONTARPRODUCT"] = artista.listaProductos.Count;

            return View(artista);
        }

        public IActionResult EditarPerfilArtista(int idartista)
        {
            DatosArtista artista = this.repo.DetailsArtista(idartista);
            return View(artista);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfilArtista
            (int idartista, string nombre, string apellidos, string nick, string descripcion,
            string email, string imagen)
        {
            idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.repo.PerfilArtista
                (idartista, nombre, apellidos, nick, descripcion,
                email, imagen);
            DatosArtista datosArtista = new DatosArtista();

            return View("PerfilArtista", new { idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) });
        }

        public async Task<IActionResult> Delete(int idInfoArte)
        {
            await this.repo.DeleteInfoArteAsync(idInfoArte);
            return View("PerfilArtista", new { idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) });
        }
    }
}
