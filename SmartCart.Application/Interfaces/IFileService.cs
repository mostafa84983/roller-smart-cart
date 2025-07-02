using Microsoft.AspNetCore.Http;
using SmartCart.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IFileService
    {
        Task<GenericResult<string>> SaveImage(IFormFile imageFile);
        Task<Result> DeleteImage(string relativePath);
    }
}
