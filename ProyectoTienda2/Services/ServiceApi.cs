using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoTienda2.Data;
using ProyectoTienda2.Helpers;
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

        //public async Task<string> GetTokenAsync
        //    (string email, string password)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string request = "/api/AuthArtista/Login";
        //        client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(this.Header);
        //        LoginModel model = new LoginModel
        //        {
        //            Email = email,
        //            Password = password
        //        };
        //        string jsonModel = JsonConvert.SerializeObject(model);
        //        StringContent content =
        //            new StringContent(jsonModel, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response =
        //            await client.PostAsync(request, content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data =
        //                await response.Content.ReadAsStringAsync();
        //            JObject jsonObject = JObject.Parse(data);
        //            string token =
        //                jsonObject.GetValue("response").ToString();
        //            return token;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

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

        //public async Task<DatosArtista> DeleteInfoArteAsync(int idartista, string token)
        //{
        //    string request = "/api/Artista/" + idartista;
        //    DatosArtista producto =
        //        await this.CallApiAsync<DatosArtista>(request, token);
        //    return producto;
        //}

        //METODOS LIBRES
        #region INFO ARTE

        public async Task<DatosArtista> GetInfoArteAsync()
        {
            string request = "/api/InfoArte";
            DatosArtista productos =
                await this.CallApiAsync<DatosArtista>(request);
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
        public async Task<DatosArtista> GetArtistasAsync()
        {
            string request = "/api/Artista";
            DatosArtista productos =
                await this.CallApiAsync<DatosArtista>(request);
            return productos;
        }

        public async Task<DatosArtista> DetailsArtistaAsync(int idartista)
        {
            string request = "/api/Artista/" + idartista;
            DatosArtista producto =
                await this.CallApiAsync<DatosArtista>(request);
            return producto;
        }

        public async Task RegistrarArtistaAsync
            (string nombre, string apellidos, string nick, string descripcion,
            string email, string password, string imagen, string imagenfondo)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Managed/RegisterArtista/"
                    + nombre + "/" + apellidos + "/" + nick + "/"
                    + descripcion + "/" + email + "/" + password + "/" + imagen
                    + "/" + imagenfondo;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Artista artist = new Artista();
                artist.Nombre = nombre;
                artist.Apellidos = apellidos;
                artist.Nick = nick;
                artist.Descripcion = descripcion;
                artist.Email = email;
                artist.Password =
                    HelperCryptography.EncryptPassword(password, artist.Salt);
                artist.Imagen = imagen;
                artist.ImagenFondo = imagenfondo;

                string json = JsonConvert.SerializeObject(artist);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task<Artista> FindEmailArtistaAsync(string email)
        {
            Artista usuario =
            await this.context.Artistas.FirstOrDefaultAsync
            (x => x.Email == email);
            return usuario;
        }

        public async Task<Artista> ExisteArtista
            (string email, string password)
        {
            Artista artista = await this.FindEmailArtistaAsync(email);
            var usuario = await this.context.Artistas.Where
                (x => x.Email == email && x.Password ==
                HelperCryptography.EncryptPassword(password, artista.Salt)).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task PerfilArtista
            (int idartista, string nombre, string apellidos, string nick, string descripcion,
            string email, string imagen)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Artista/EditarArtista/" + idartista + "/"
                    + nombre + "/" + apellidos + "/" + nick + "/" +
                    descripcion + "/" + email + "/" + imagen;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                DatosArtista artista = new DatosArtista();

                artista = await this.DetailsArtistaAsync(idartista);

                artista.artista.Nombre = nombre;
                artista.artista.Apellidos = apellidos;
                artista.artista.Nick = nick;
                artista.artista.Descripcion = descripcion;
                artista.artista.Email = email;
                artista.artista.Imagen = imagen;

                string json = JsonConvert.SerializeObject(artista);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task<DatosArtista> DeleteInfoArteAsync(int idartista)
        {
            string request = "/api/Artista/BorrarProducto/" + idartista;
            DatosArtista producto =
                await this.CallApiAsync<DatosArtista>(request);
            return producto;
        }

        public async Task CambiarImagenFondoAsync(int idartista, string imagenfondo)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Artista/CambiarImagenFondo/" + idartista + "/"
                    + imagenfondo;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                DatosArtista artista = new DatosArtista();

                artista = await this.DetailsArtistaAsync(idartista);
                artista.artista.ImagenFondo = imagenfondo;

                string json = JsonConvert.SerializeObject(artista);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        #endregion
        #region CLIENTES
        public async Task RegistrarClienteAsync
            (string nombre, string apellidos, string email, string password, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Managed/RegisterCliente/"
                    + nombre + "/" + apellidos + "/" + email + "/"
                    + password + "/" + imagen;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Cliente cliente = new Cliente();
                cliente.Nombre = nombre;
                cliente.Apellidos = apellidos;
                cliente.Email = email;
                cliente.Password =
                    HelperCryptography.EncryptPassword(password, cliente.Salt);
                cliente.Imagen = imagen;

                string json = JsonConvert.SerializeObject(cliente);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task<Cliente> FindEmailClienteAsync(string email)
        {
            Cliente usuario =
            await this.context.Clientes.FirstOrDefaultAsync
            (x => x.Email == email);
            return usuario;
        }

        public async Task<Cliente> ExisteCliente
            (string email, string password)
        {
            Cliente cliente = await this.FindEmailClienteAsync(email);
            var usuario = await this.context.Clientes.Where
                (x => x.Email == email && x.Password ==
                HelperCryptography.EncryptPassword(password, cliente.Salt)).FirstOrDefaultAsync();
            return usuario;
        }
        public async Task<DatosArtista> FindCliente(int idcliente)
        {
            string request = "/api/Cliente/" + idcliente;
            DatosArtista producto =
                await this.CallApiAsync<DatosArtista>(request);
            return producto;
        }

        public async Task EditarClienteAsync
            (int idcliente, string nombre, string apellidos, string email, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Cliente/EditarCliente/" + idcliente + "/"
                    + nombre + "/" + apellidos + "/" + email + "/" + imagen;
                client.BaseAddress = new Uri(this.UrlApiProyectoTienda);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                DatosArtista cliente = new DatosArtista();

                cliente = await this.FindCliente(idcliente);

                cliente.cliente.Nombre = nombre;
                cliente.cliente.Apellidos = apellidos;
                cliente.cliente.Email = email;
                cliente.cliente.Imagen = imagen;

                string json = JsonConvert.SerializeObject(cliente);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        #endregion
    }
}
