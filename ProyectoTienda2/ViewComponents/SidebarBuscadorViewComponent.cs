using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ProyectoTienda2.Services;
using PyoyectoNugetTienda;
using System.Collections.Generic;

namespace ProyectoTienda2.ViewComponents
{
    [ViewComponent(Name = "SidebarBuscador")]
    public class SidebarBuscadorViewComponent : ViewComponent
    {
        private ServiceApi service;
        private ServiceStorageBlobs serviceBlob;
        private string containerName = "proyectotienda";

        public SidebarBuscadorViewComponent(ServiceApi service, ServiceStorageBlobs serviceBlob)
        {
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        public async Task<IViewComponentResult> InvokeAsync(string query)
        {
            DatosArtista artistas;
                artistas = await this.service.GetArtistasAsync();
            foreach (Artista c in artistas.listaArtistas)
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
            return View(artistas);
        }

    }
}
