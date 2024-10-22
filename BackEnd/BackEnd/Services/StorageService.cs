using BackEnd.Configurations;
using BackEnd.Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net;
using System.Security.AccessControl;

namespace BackEnd.Services
{
    public class StorageService : IStorageService
    {
        GoogleCredential? credential = null;

        public StorageService()
        {
            string credential_path_gcp = GlobalConfigurations.Settings.Gcp.PathRelative ? AppDomain.CurrentDomain.BaseDirectory + GlobalConfigurations.Settings.Gcp.PathJson : GlobalConfigurations.Settings.Gcp.PathJson;
            if (File.Exists(credential_path_gcp))
            {
                credential = GoogleCredential.FromFile(credential_path_gcp);
            }
        }

        public async Task<string> UploadCnh(string folder, string userId, string contentType, string base64)
        {
            if(credential == null)
            {
                Console.WriteLine("Credenciais GCP não encontrada");
                return "";
            }

            StorageClient _storageClient = StorageClient.Create(credential);
            try
            {
                await _storageClient.DeleteObjectAsync(GlobalConfigurations.Settings.Gcp.Bucket, folder + "/" + userId.ToString());
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                Console.WriteLine("Objeto não encontrado. Continuando com o upload.");
            }

            var base64Data = base64.Substring(base64.IndexOf(',') + 1);
            var imageBytes = Convert.FromBase64String(base64Data);

            using var stream = new MemoryStream(imageBytes);
            var uploadObject = await _storageClient.UploadObjectAsync(GlobalConfigurations.Settings.Gcp.Bucket, folder + "/" + userId.ToString(), contentType, stream);

            return $"https://storage.googleapis.com/{uploadObject.Bucket}/{uploadObject.Name}";
        }
    }
}
