using System.Collections.Generic;
using dotnet_blog_mvc.Models;

namespace dotnet_blog_mvc.ViewModels
{
    public class IndexViewModel
    {
        public int PageNum { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public string Category { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}