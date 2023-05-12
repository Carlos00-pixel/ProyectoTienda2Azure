using Microsoft.EntityFrameworkCore;
using ProyectoTienda2.Data;
using ProyectoTienda2.Helpers;
using PyoyectoNugetTienda;
using System.Security.Claims;

namespace ProyectoTienda2.Repositories
{
    public class RepositoryArtista
    {
        private ProyectoTiendaContext context;

        public RepositoryArtista(ProyectoTiendaContext context)
        {
            this.context = context;
        }
        public DatosArtista GetArtistas()
        {
            DatosArtista datosArtista = new DatosArtista();

            var consulta = from datos in this.context.Artistas
                           select datos;
            datosArtista.listaArtistas = consulta.ToList();

            return datosArtista;
        }

        public DatosArtista DetailsArtista(int idArtista)
        {
            DatosArtista datosArtista = new DatosArtista();

            var consulta = from datos in this.context.InfoProductos
                           where datos.IdArtista == idArtista
                           select datos;
            datosArtista.listaProductos = consulta.ToList();
            datosArtista.artista = this.context.Artistas.FirstOrDefault(x => x.IdArtista == idArtista);

            return datosArtista;
        }

        public DatosArtista BuscarArtistas(string query)
        {
            DatosArtista datosArtista = new DatosArtista();

            var consulta = from datos in this.context.Artistas
                           where datos.Nick.Contains(query)
                           select datos;
            datosArtista.listaArtistas = consulta.ToList();
            return datosArtista;
        }

        private int GetMaximoIdArtista()
        {
            var maximo = (from datos in this.context.Artistas
                          select datos).Max(x => x.IdArtista) + 1;
            return maximo;
        }

        public async Task RegistrarArtistaAsync
            (string nombre, string apellidos, string nick, string descripcion,
            string email, string password, string imagen)
        {
            Artista artista = new Artista();

            int maximo = this.GetMaximoIdArtista();

            artista.IdArtista = maximo;
            artista.Nombre = nombre;
            artista.Apellidos = apellidos;
            artista.Nick = nick;
            artista.Descripcion = descripcion;
            artista.Email = email;
            artista.Salt =
                HelperCryptography.GenerateSalt();
            artista.Password =
                HelperCryptography.EncryptPassword(password, artista.Salt);
            artista.Imagen = imagen;

            this.context.Artistas.Add(artista);

            await this.context.SaveChangesAsync();
        }

        public async Task<Artista> FindEmailAsync(string email)
        {
            Artista usuario =
            await this.context.Artistas.FirstOrDefaultAsync
            (x => x.Email == email);
            return usuario;
        }

        public async Task<Artista> ExisteArtista
            (string email, string password)
        {
            Artista artista = await this.FindEmailAsync(email);
            var usuario = await this.context.Artistas.Where
                (x => x.Email == email && x.Password ==
                HelperCryptography.EncryptPassword(password, artista.Salt)).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task PerfilArtista
            (int idartista, string nombre, string apellidos, string nick, string descripcion,
            string email, string imagen)
        {
            DatosArtista artista = new DatosArtista();
                
            artista = this.DetailsArtista(idartista);

            artista.artista.Nombre = nombre;
            artista.artista.Apellidos = apellidos;
            artista.artista.Nick = nick;
            artista.artista.Descripcion = descripcion;
            artista.artista.Email = email;
            artista.artista.Imagen = imagen;

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteInfoArteAsync(int id)
        {
            InfoArte datosArtista = this.context.InfoArtes.FirstOrDefault(a => a.IdInfoArte == id);

            this.context.InfoArtes.Remove(datosArtista);

            await this.context.SaveChangesAsync();
        }

    }
}
