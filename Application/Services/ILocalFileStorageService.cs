using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class LocalFileStorageService : ILocalFileStorageService
    {
        private readonly string _storagePath;
        private readonly string _publicBaseUrlPath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorageService(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            string configuredPath = configuration.GetValue<string>("FileStorageSettings:LocalPath") ?? "Uploads/Images";
            _storagePath = Path.Combine(env.ContentRootPath, configuredPath);

            _publicBaseUrlPath = configuration.GetValue<string>("FileStorageSettings:PublicBaseUrlPath") ?? "/static-images";
            _httpContextAccessor = httpContextAccessor;

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Arquivo inválido ou vazio.", nameof(file));
            }

            var subfolderPath = Path.Combine(_storagePath, subfolder);
            if (!Directory.Exists(subfolderPath))
            {
                Directory.CreateDirectory(subfolderPath);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var physicalFilePath = Path.Combine(subfolderPath, uniqueFileName);

            using (var stream = new FileStream(physicalFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(subfolder, uniqueFileName).Replace(Path.DirectorySeparatorChar, '/');
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var physicalFilePath = Path.Combine(_storagePath, relativePath.TrimStart('/'));
            if (File.Exists(physicalFilePath))
            {
                File.Delete(physicalFilePath);
            }
        }

        public string GetFileUrl(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return string.Empty;
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return $"{_publicBaseUrlPath.TrimEnd('/')}/{relativePath.TrimStart('/')}";
            }

            var scheme = request.Scheme;
            var host = request.Host.Value;

            return $"{scheme}://{host}{_publicBaseUrlPath.TrimEnd('/')}/{relativePath.TrimStart('/')}";
        }
    }
}