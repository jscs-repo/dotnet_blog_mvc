using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_mvc.Models;
using dotnet_blog_mvc.Models.Comments;
using dotnet_blog_mvc.ViewModels;

namespace dotnet_blog_mvc.Data.Repository
{
    public interface IRepository
    {
        Post GetPostById(int id);
        List<Post> GetAllPosts();
        IndexViewModel GetAllPosts(int pageNum, string category, string searchString);
        void AddPost(Post post);
        void RemovePost(int id);
        void UpdatePost(Post post);
        void AddSubComment(SubComment subcomment);

        Task<bool> SaveChangesAsync();
    }
}