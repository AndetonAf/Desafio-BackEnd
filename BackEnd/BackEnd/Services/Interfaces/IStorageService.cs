using BackEnd.DTOs;
using Data.Models;

namespace BackEnd.Services.Interfaces
{
    public interface IStorageService
    {
        public Task<string> UploadCnh(string bucketName, string fileName, string contentType, string base64);
    }
}
