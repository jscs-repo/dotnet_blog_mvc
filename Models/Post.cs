using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_mvc.Models.Comments;

namespace dotnet_blog_mvc.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        // image of type string because we know where the images will be stored
        public string Image { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public List<MainComment> MainComments { get; set; }
    }
}