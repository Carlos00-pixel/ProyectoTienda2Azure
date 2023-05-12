using ProyectoTienda2.Data;
using PyoyectoNugetTienda;
using System.Numerics;

namespace ProyectoTienda2.Repositories
{
    public class RepositoryInfoArte
    {
        private ProyectoTiendaContext context;

        public RepositoryInfoArte(ProyectoTiendaContext context)
        {
            this.context = context;
        }

        private int GetMaximoIdInfoProducto()
        {
            var maximo = (from datos in this.context.InfoProductos
                          select datos).Max(x => x.IdInfoArte) + 1;
            return maximo;
        }
        public DatosArtista GetInfoArte()
        {
            DatosArtista datosInfoArte= new DatosArtista();

            var consulta = (from datos in this.context.InfoProductos
                            select datos).OrderByDescending(x => x.IdInfoArte);
            datosInfoArte.listaProductos = consulta.ToList();
            return datosInfoArte;
        }

        public DatosArtista FindInfoArte(int idProducto)
        {
            DatosArtista datosInfoArte = new DatosArtista();

            var consulta = from datos in this.context.InfoProductos
                           where datos.IdInfoArte == idProducto
                           select datos;
            datosInfoArte.infoProducto = consulta.FirstOrDefault();
            return datosInfoArte;
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
            InfoArte prod = new InfoArte();

            int maximo = this.GetMaximoIdInfoProducto();

            prod.IdInfoArte = maximo;
            prod.Titulo = titulo;
            prod.Precio = precio;
            prod.Descripcion = descripcion;
            prod.Imagen = imagen;
            prod.IdArtista = idartista;

            this.context.InfoArtes.Add(prod);

            await this.context.SaveChangesAsync();
        }
    }
}
