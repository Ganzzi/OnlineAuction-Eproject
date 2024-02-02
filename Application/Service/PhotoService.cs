using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Application.Service.PhotoService;

using DomainLayer.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Application.Service
{
    public class PhotoService : IphotoService
    {
        private readonly Cloudinary _c;
        public PhotoService(IOptions<CloudKey> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                  config.Value.ApiKey,
                    config.Value.ApiSecret
                );
            _c = new Cloudinary(acc);
        }
          

        public async Task<DeletionResult> DeletPhoto(string publicId)
        {
            var deletParams = new DeletionParams(publicId);
            var result = await _c.DestroyAsync(deletParams);
            return result;
        }

        public async Task<string> addPhoto(IFormFile file)
        {
            var uploadresult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stre = file.OpenReadStream();
                var uploadparam = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stre),
                    Transformation = new Transformation().Height(500).Width(500)
                };
                uploadresult = await _c.UploadAsync(uploadparam);
            }
            return uploadresult.SecureUrl.ToString();
        }

        public async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            string exactpath = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return exactpath;
        }

        public async Task DeleteFile(string filename)
        {
            try
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
