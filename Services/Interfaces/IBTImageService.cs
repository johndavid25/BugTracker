using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTImageService
    {
        Task<byte[]> EncodeFileAsync(IFormFile formFile);

        string DecodeFile(byte[] imageData, string contentType);

        string RecordContentType(IFormFile formFile);
    }
}
