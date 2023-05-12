using Microsoft.EntityFrameworkCore;
using ProyectoTienda2.Data;
using ProyectoTienda2.Helpers;
using PyoyectoNugetTienda;

namespace ProyectoTienda2.Repositories
{
    public class RepositoryCliente
    {
        private ProyectoTiendaContext context;

        public RepositoryCliente(ProyectoTiendaContext context)
        {
            this.context = context;
        }
        private int GetMaximoIdCliente()
        {
            var maximo = (from datos in this.context.Clientes
                          select datos).Max(x => x.IdCliente) + 1;
            return maximo;
        }

        public async Task RegistrarClienteAsync
            (string nombre, string apellidos, string email, string password, string imagen)
        {
            Cliente cliente = new Cliente();

            int maximo = this.GetMaximoIdCliente();

            cliente.IdCliente = maximo;
            cliente.Nombre = nombre;
            cliente.Apellidos = apellidos;
            cliente.Email = email;
            cliente.Salt =
                HelperCryptography.GenerateSalt();
            cliente.Password =
                HelperCryptography.EncryptPassword(password, cliente.Salt);
            cliente.Imagen = imagen;

            this.context.Clientes.Add(cliente);

            await this.context.SaveChangesAsync();
        }

        public async Task<Cliente> FindEmailAsync(string email)
        {
            Cliente usuario =
            await this.context.Clientes.FirstOrDefaultAsync
            (x => x.Email == email);
            return usuario;
        }

        public async Task<Cliente> ExisteCliente
            (string email, string password)
        {
            Cliente cliente = await this.FindEmailAsync(email);
            var usuario = await this.context.Clientes.Where
                (x => x.Email == email && x.Password ==
                HelperCryptography.EncryptPassword(password, cliente.Salt)).FirstOrDefaultAsync();
            return usuario;
        }

        public DatosArtista FindCliente(int idCliente)
        {
            DatosArtista datosArtista = new DatosArtista();
            datosArtista.cliente = this.context.Clientes.FirstOrDefault(x => x.IdCliente == idCliente);

            return datosArtista;
        }

        public async Task EditarClienteAsync
            (int idcliente, string nombre, string apellidos, string email, string imagen)
        {
            DatosArtista cliente = new DatosArtista();

            cliente = this.FindCliente(idcliente);

            cliente.cliente.Nombre = nombre;
            cliente.cliente.Apellidos = apellidos;
            cliente.cliente.Email = email;
            cliente.cliente.Imagen = imagen;

            await this.context.SaveChangesAsync();
        }
    }
}
