using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    internal interface IPhotoService
    {
        Task<string> AddPhoto(IFormFile file);
        Task<DeletionResult> DeletPhoto(string file);
        Task<string> WriteFile(IFormFile file);
        void DeleteFile(string filename);

    }
}
