using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostFeebBack.Models
{
    public class PostDto
    {
        public long PostID { get; set; }
        public string PostName { get; set; }
        public string PostCreatorName { get; set; }
        public DateTime PostCreationDate { get; set; }
        public long NumOfComments { get; set; }
        public List<CommentsDto> Comments { get; set; }
    }
    public class CommentsDto
    {
        public long PostID { get; set; }
        public long CommentID { get; set; }
        public string CommentDescription { get; set; }
        public string CommentCreatorName { get; set; }
        public DateTime CommentCreationDate { get; set; }
        public long NumOfLike { get; set; }
        public long NumOfDislike { get; set; }
    }
}
