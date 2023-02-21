using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IFileService
{
    Task<string> SaveFile(IFormFile file);
    Task<FileDownloadResponse> DownloadFile(string filepath, string filename);
    void RemoveFile(string filepath);
    bool CheckIfExist(string filepath);
}