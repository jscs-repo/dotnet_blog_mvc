using System.Threading.Tasks;
using dotnet_blog_mvc.Data.FileManager;
using dotnet_blog_mvc.Data.Repository;
using dotnet_blog_mvc.Models;
using dotnet_blog_mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace dotnet_blog_mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;
        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View(new PostViewModel());
            }
            else
            {
                var postToEdit = _repo.GetPostById((int)id);
                return View(new PostViewModel
                {
                    Id = postToEdit.Id,
                    Title = postToEdit.Title,
                    Body = postToEdit.Body,
                    CurrentImage = postToEdit.Image,
                    Description = postToEdit.Description,
                    Tags = postToEdit.Tags,
                    Category = postToEdit.Category
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Description = vm.Description,
                Tags = vm.Tags,
                Category = vm.Category,
            };

            if (vm.Image is null)
            {
                post.Image = vm.CurrentImage;
            }
            else
            {
                post.Image = await _fileManager.SaveImageAsync(vm.Image);
            }

            if (post.Id > 0)
            {
                _repo.UpdatePost(post);
            }
            else
            {
                // if it is a new post, it will have an Id=0
                _repo.AddPost(post);
            }

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }

    }
}