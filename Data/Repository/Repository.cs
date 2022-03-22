using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_mvc.Models;
using dotnet_blog_mvc.Models.Comments;
using dotnet_blog_mvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace dotnet_blog_mvc.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;
        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public IndexViewModel GetAllPosts(int pageNum, string category, string searchString)
        {
            int postsPerPage = 5;
            int skipAmount = postsPerPage * (pageNum - 1);


            var query = _ctx.Posts.AsNoTracking().OrderBy(p => p.Created).AsQueryable();

            // .Skip(skipAmount).Take(postsPerPage);

            if (!String.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.ToLower()
                            .Equals(category.ToLower()))
                            .Skip(skipAmount)
                            .Take(postsPerPage);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{searchString}%")
                || EF.Functions.Like(x.Body, $"%{searchString}%")
                || EF.Functions.Like(x.Category, $"%{searchString}%")
               );
            }

            int postsCount = query.Count();

            return new IndexViewModel
            {
                PageNum = pageNum,
                PageCount = (int)Math.Ceiling((double)postsCount / postsPerPage),
                NextPage = postsCount > (skipAmount + postsPerPage),
                Category = category,
                SearchString = searchString,
                //Posts = _ctx.Posts.OrderBy(p => p.Created).Skip(skipAmount).Take(postsPerPage).ToList()
                Posts = query.Skip(skipAmount).Take(postsPerPage).ToList()
            };
        }

        public Post GetPostById(int id)
        {
            return _ctx.Posts
                .Include(p => p.MainComments)
                .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(post => post.Id == id);
        }

        public void RemovePost(int id)
        {
            var postToRemove = GetPostById(id);
            _ctx.Posts.Remove(postToRemove);
        }

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        public void AddSubComment(SubComment subcomment)
        {
            _ctx.SubComments.Add(subcomment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;

        }


    }
}