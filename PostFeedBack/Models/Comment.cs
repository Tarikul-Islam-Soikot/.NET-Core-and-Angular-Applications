using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostFeebBack.Models
{
    public class Comment
    {
        public long CommentID { get; set; }
        public string Remarks { get; set; }
        public long LikeQuantity { get; set; }
        public long DislikeQuantity { get; set; }
        public long PostID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
