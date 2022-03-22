using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace dotnet_blog_mvc.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly AppDbContext _ctx;
        private string _imagePath;

        public FileManager(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string imageName)
        {
            return new FileStream(Path.Combine(_imagePath, imageName), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            var savePath = Path.Combine(_imagePath);
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var mime = Path.GetExtension(image.FileName);
            var fileName = Path.GetFileNameWithoutExtension(image.FileName);
            var fullFileName = fileName + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + mime;
            var filePath = Path.Combine(_imagePath, fullFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return fullFileName;
        }
    }
}