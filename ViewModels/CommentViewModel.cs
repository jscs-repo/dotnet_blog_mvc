using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_mvc.ViewModels
{
    public class CommentViewModel
    {

        // want to know which post we are commenting on
        [Required]
        public int PostId { get; set; }
        // want to know if it is a main comment or a subcomment
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}