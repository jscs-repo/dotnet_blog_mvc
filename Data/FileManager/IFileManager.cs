using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace dotnet_blog_mvc.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string imageName);
        Task<string> SaveImageAsync(IFormFile image);
    }
}