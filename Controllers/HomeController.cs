using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using dotnet_blog_mvc.Data.FileManager;
using dotnet_blog_mvc.Data.Repository;
using dotnet_blog_mvc.Models.Comments;
using dotnet_blog_mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_blog_mvc.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public IActionResult Index(int pageNum, string category, string searchString)
        {
            if (pageNum < 1) { return RedirectToAction("Index", new { pageNum = 1, category }); }

            var vm = _repo.GetAllPosts(pageNum, category, searchString);

            return View(vm);
        }


        public IActionResult Post(int id)
        {
            var post = _repo.GetPostById(id);
            return View(post);
        }

        [HttpGet("Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = Path.GetExtension(image);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction("Post", new { id = vm.PostId });

            var post = _repo.GetPostById(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };

                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }


    }
}