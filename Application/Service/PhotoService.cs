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
using Microsoft.Extensions.Hosting;


using DomainLayer.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Application.Service
{
    public class PhotoService : IphotoService
    {
        private readonly Cloudinary _c;
        private IHostingEnvironment _hostEnvironment;
        public PhotoService(IOptions<CloudKey> config, IHostingEnvironment hostEnvironment)
        {
            var acc = new Account(
                config.Value.CloudName,
                  config.Value.ApiKey,
                    config.Value.ApiSecret
                );
            _c = new Cloudinary(acc);
            _hostEnvironment = hostEnvironment;
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
        try
        {
            // Generate a unique filename based on current timestamp
            string filename = GenerateUniqueFileName(file);

            // Get the wwwroot folder path
            string wwwrootPath = Path.Combine("wwwroot", "Upload/Files");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath);
            }

            // Combine the wwwroot path with the filename to get the exact path
            string exactPath = Path.Combine(wwwrootPath, filename);

            // Save the file to the specified path
            using (var stream = new FileStream(exactPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string res = Path.Combine("Upload/Files", filename);

            return res;
        }
        catch (Exception ex)
        {
            // Handle exceptions here (log, throw, etc.)
            Console.WriteLine($"Error saving file: {ex.Message}");
            return null; // You might want to return a specific value or throw an exception here
        }
    //     try
    //     {
    //         if (file == null || file.Length == 0)
    //         {
    //             throw new ArgumentNullException(nameof(file), "File is null or empty.");
    //         }

    //         string extension = Path.GetExtension(file.FileName);
    //         string filename = $"{DateTime.Now.Ticks}{extension}";

    //         string uploadFolderPath = GetUploadFolderPath();

    //         string exactPath = Path.Combine(uploadFolderPath, filename);

    //         using (var stream = new FileStream(exactPath, FileMode.Create))
    //         {
    //             await file.CopyToAsync(stream);
    //         }

    //         return exactPath;
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle exceptions here (log, throw, etc.)
    //         Console.WriteLine($"Error writing file: {ex.Message}");
    //         return null; // You might want to return a specific value or throw an exception here
    //     }
    }

        private string GenerateUniqueFileName(IFormFile file)
        {
            // Generate a unique filename based on current timestamp and file extension
            var extension = "." + file.FileName.Split('.')[^1];
            return DateTime.Now.Ticks.ToString() + extension;
        }
        private string GetUploadFolderPath()
        {
            // Get the wwwroot folder path
            string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Files");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            return uploadFolderPath;
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
