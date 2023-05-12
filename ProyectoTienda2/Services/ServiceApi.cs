using ApiProyectoTienda.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoTienda2.Data;
using PyoyectoNugetTienda;
using System.Net.Http.Headers;
using System.Text;

namespace ProyectoTienda2.Services
{
    public class ServiceApi
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiProyectoTienda;
        private ProyectoTiendaContext context;

        public ServiceApi(IConfiguration configuration, ProyectoTiendaContext context)
        {
            this.UrlApiProyectoTienda =
                configuration.GetValue<string>("ApiUrls:ApiProyectoTienda");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.context = context;
        }

        public async Task<string> GetTokenAsync
            (string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Email = email,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //METODO PROTEGIDO
        //public async Task<List<Empleado>> GetEmpleadosAsync(string token)
        //{
        //    string request = "/api/empleados";
        //    List<Empleado> empleados =
        //        await this.CallApiAsync<List<Empleado>>(request, token);
        //    return empleados;
        //}

        //METODOS LIBRES
        #region INFO ARTE

        public async Task<List<DatosArtista>> GetInfoArteAsync()
        {
            string request = "/api/InfoArte";
            List<DatosArtista> productos =
                await this.CallApiAsync<List<DatosArtista>>(request);
            return productos;
        }
        public async Task<DatosArtista> FindInfoArteAsync(int idproducto)
        {
            string request = "/api/InfoArte/" + idproducto;
            DatosArtista producto =
                await this.CallApiAsync<DatosArtista>(request);
            return producto;
        }

        public DatosArtista GetInfoArteSession(List<int> ids)
        {
            DatosArtista datosInfoArte = new DatosArtista();

            var consulta = from datos in this.context.InfoProductos
                           where ids.Contains(datos.IdInfoArte)
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            datosInfoArte.listaProductos = consulta.ToList();
            return datosInfoArte;
        }

        public async Task AgregarProductoAsync
            (string titulo, int precio, string descripcion,
            string imagen, int idartista)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/InfoArte/AgregarProducto/"
                    + titulo + "/" + precio + "/" + descripcion + "/" 
                    + imagen + "/" + idartista;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                InfoArte prod = new InfoArte();
                prod.Titulo = titulo;
                prod.Precio = precio;
                prod.Descripcion = descripcion;
                prod.Imagen = imagen;
                prod.IdArtista = idartista;

                string json = JsonConvert.SerializeObject(prod);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        #endregion
        #region ARTISTAS
        public async Task<List<DatosArtista>> GetArtistasAsync()
        {
            string request = "/api/Artista";
            List<DatosArtista> productos =
                await this.CallApiAsync<List<DatosArtista>>(request);
            return productos;
        }

        public async Task<DatosArtista> DetailsArtista(int idartista)
        {
            string request = "/api/Artista/" + idartista;
            DatosArtista producto =
                await this.CallApiAsync<DatosArtista>(request);
            return producto;
        }

        public async Task RegistrarArtistaAsync
            (string titulo, int precio, string descripcion,
            string imagen, int idartista)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/InfoArte/AgregarProducto/"
                    + titulo + "/" + precio + "/" + descripcion + "/"
                    + imagen + "/" + idartista;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                InfoArte prod = new InfoArte();
                prod.Titulo = titulo;
                prod.Precio = precio;
                prod.Descripcion = descripcion;
                prod.Imagen = imagen;
                prod.IdArtista = idartista;

                string json = JsonConvert.SerializeObject(prod);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        #endregion
    }
}
