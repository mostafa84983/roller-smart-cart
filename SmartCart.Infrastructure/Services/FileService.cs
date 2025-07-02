using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SmartCart.Application.Common;
using SmartCart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<GenericResult<string>> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return GenericResult<string>.Failure("No file uploaded");

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string fullPath = Path.Combine(_environment.WebRootPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            string relativePath = "/" + fileName; 
            return GenericResult<string>.Success(relativePath);
        }

        public async Task<Result> DeleteImage(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Result.Failure("Image path is empty");

            string fullPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                return Result.Failure("Image file not found");

            File.Delete(fullPath);
            return Result.Success();
        }

    }
}
