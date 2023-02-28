using System.Net.Http.Headers;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Responses;
using Microsoft.AspNetCore.StaticFiles;
using TokonyadiaRestAPI.Exceptions;

namespace Enigma.DatingNet.Services.Impls;

public class FileService : IFileService
{
    private readonly List<string> _contentTypes = new() { "image/png", "image/jpeg", "image/jpg" };

    public async Task<string> SaveFile(IFormFile file)
    {
        var folderName = Path.Combine("Resources", "Images");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (!_contentTypes.Contains(file.ContentType)) throw new BadRequestException("File must be image");

        if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);

        if (file.Length <= 0) throw new Exception("File Required");

        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');

        if (fileName is null) throw new Exception("fileName cannot be null");

        var uniqueFileName = GetUniqueFileName(fileName);

        var fullPath = Path.Combine(pathToSave, uniqueFileName);
        var dbPath = Path.Combine(folderName, uniqueFileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return dbPath;
    }

    public async Task<FileDownloadResponse> DownloadFile(string filepath)
    {
        if (!CheckIfExist(filepath)) throw new NotFoundException("File not found");

        var memory = new MemoryStream();
        await using var stream = new FileStream(filepath, FileMode.Open);
        await stream.CopyToAsync(memory);

        memory.Position = 0;
        return new FileDownloadResponse
        {
            MemoryStream = memory,
            ContentType = GetContentType(filepath),
            Filename = Guid.NewGuid().ToString()
        };
    }

    public void RemoveFile(string filepath)
    {
        if (!CheckIfExist(filepath)) throw new NotFoundException("File not found");
        File.Delete(filepath);
    }

    public bool CheckIfExist(string filepath) => File.Exists(filepath);

    private static string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }

    private static string GetUniqueFileName(string fileName)
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds() + "_" + fileName;
    }
}