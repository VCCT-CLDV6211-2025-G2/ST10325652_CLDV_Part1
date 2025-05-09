using Azure.Storage.Blobs;

namespace WebApplicationPOECLDV.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
      //  private readonly string _VenueContainerName;
       // private readonly string _EventContainerName;

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config["AzureStorage:ConnectionString"];
           // _VenueContainerName = config["AzureStorage:VenueContainerName"];
           // _EventContainerName = config["AzureStorage:EventContainerName"];
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
        //public async Task<string> EventUploadFileAsync(IFormFile file)
        //{
        //    var blobServiceClient = new BlobServiceClient(_connectionString);
        //    var containerClient = blobServiceClient.GetBlobContainerClient(_EventContainerName);

        //    await containerClient.CreateIfNotExistsAsync();

        //    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
        //    var blobClient = containerClient.GetBlobClient(uniqueFileName);

        //    using (var stream = file.OpenReadStream())
        //    {
        //        await blobClient.UploadAsync(stream, overwrite: true);
        //    }

        //    return blobClient.Uri.ToString();
        //}

    }
}

