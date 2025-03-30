using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Firebase.Storage;

namespace Infra.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }

    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucket;

        public FirebaseStorageService(IConfiguration config)
        {
            // Exemplo de configuração no appsettings.json:
            // "FirebaseStorage": { "Bucket": "seuprojeto.appspot.com" }
            _bucket = config["FirebaseStorage:Bucket"];
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            using var stream = file.OpenReadStream();
            var downloadUrl = await new FirebaseStorage(_bucket)
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            return downloadUrl;
        }
    }
}
