using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostFeebBack.Models
{
    public class Post
    {
        public long PostID { get; set; } 
        public string Name { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
