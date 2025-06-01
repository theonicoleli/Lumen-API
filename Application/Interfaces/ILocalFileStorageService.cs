using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ILocalFileStorageService
    {
        /// <summary>
        /// Salva o arquivo em uma subpasta do caminho configurado e retorna o caminho relativo para ser armazenado.
        /// </summary>
        /// <param name="file">O arquivo a ser salvo.</param>
        /// <param name="subfolder">A subpasta dentro do diretório principal de uploads (ex: "users", "projects").</param>
        /// <returns>O caminho relativo do arquivo salvo (ex: "users/seu_arquivo.jpg").</returns>
        Task<string> SaveFileAsync(IFormFile file, string subfolder);

        /// <summary>
        /// Exclui um arquivo com base no seu caminho relativo.
        /// </summary>
        /// <param name="relativePath">O caminho relativo do arquivo a ser excluído.</param>
        void DeleteFile(string relativePath);

        /// <summary>
        /// Retorna a URL pública completa para um arquivo com base no seu caminho relativo.
        /// </summary>
        /// <param name="relativePath">O caminho relativo do arquivo.</param>
        /// <returns>A URL completa para acessar o arquivo.</returns>
        string GetFileUrl(string? relativePath);
    }
}