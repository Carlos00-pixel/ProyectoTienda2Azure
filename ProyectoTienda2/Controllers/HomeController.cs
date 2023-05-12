using Microsoft.AspNetCore.Mvc;
using ProyectoTienda2.Extensions;
using ProyectoTienda2.Filters;
using ProyectoTienda2.Repositories;
using ProyectoTienda2.Services;
using PyoyectoNugetTienda;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoTienda2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private RepositoryInfoArte repo;
        private ServiceApi service;

        public HomeController
            (ILogger<HomeController> logger, RepositoryInfoArte repo, ServiceApi service)
        {
            _logger = logger;
            this.repo = repo;
            this.service = service;
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
            List<DatosArtista> infoArtes = await this.service.GetInfoArteAsync();
            return View(infoArtes);
        }

        [AuthorizeUsuarios]
        public IActionResult ProductosFavoritos(int? ideliminar)
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
                return View(infoArtes);
            }
        }

        public async Task<IActionResult> Details(int idproducto)
        {
            DatosArtista infoProduct = await this.service.FindInfoArteAsync(idproducto);
            return View(infoProduct);
        }

        public IActionResult NuevoProducto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoProducto
            (string titulo, int precio, string descripcion, string imagen, int idartista)
        {
            idartista = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await this.service.AgregarProductoAsync(titulo, precio, descripcion, imagen, idartista);
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