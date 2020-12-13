using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PostFeebBack.Models;


namespace PostFeebBack.Controllers
{
    [ApiController]
    [Route("api/Post")]
    public class PostController : ControllerBase
    {

        [HttpGet]
        [Route("GetAllPost")]
        public  IActionResult GetAllPost()
        {
            try
            {
                #region Initilize Data
                //I have use this instead of getting data from database table
                var posts = GetPosts();
                var comments = GetComments();
                var users = GetUsers(); 

                #endregion

                var postList = posts
                          .Join(
                               users,
                               pt => pt.CreatedBy,
                               u => u.UserID,
                               (pt, u) => new { posts = pt, users = u })
                           .Select(c => new PostDto {
                               PostID = c.posts.PostID,
                               PostName = c.posts.Name,
                               PostCreatorName =  c.users.Name,
                               PostCreationDate = c.posts.CreatedDate,
                           }).ToList();

                if(postList?.Count > 0)
                {
                    foreach (var item in postList)
                    {
                        var tempCommentList = comments
                                              .Join(
                                                  users,
                                                  cm => cm.CreatedBy,
                                                  u => u.UserID,
                                                  (cm, u) => new { comments = cm, users = u })
                                              .Where(x => x.comments.PostID == item.PostID)
                                              .Select(c => new CommentsDto
                                              {
                                                  PostID = c.comments.PostID,
                                                  CommentID = c.comments.CommentID,
                                                  CommentDescription = c.comments.Remarks,
                                                  CommentCreationDate = c.comments.CreatedDate,
                                                  NumOfLike = c.comments.LikeQuantity,
                                                  NumOfDislike = c.comments.DislikeQuantity,
                                                  CommentCreatorName = c.users.Name,
                                              }).ToList();

                        item.Comments = tempCommentList;
                        item.NumOfComments = tempCommentList.Count;

                    }
                }

                return Ok(postList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            User admin = new User();
            admin.UserID = -9;
            admin.Name = "Admin";
            admin.CreatedBy = -9;
            admin.CreatedDate = DateTime.Now;
            users.Add(admin);

            User user1 = new User();
            user1.UserID = 1;
            user1.Name = "User 1";
            user1.CreatedBy = -9;
            user1.CreatedDate = DateTime.Now;
            users.Add(user1);

            User user2 = new User();
            user2.UserID = 2;
            user2.Name = "User 2";
            user2.CreatedBy = -9;
            user2.CreatedDate = DateTime.Now;
            users.Add(user2);

            User user3 = new User();
            user3.UserID = 3;
            user3.Name = "User 3";
            user3.CreatedBy = -9;
            user3.CreatedDate = DateTime.Now;
            users.Add(user3);

            return users;

        }
        public List<Post> GetPosts()
        {
            List<Post> posts = new List<Post>();

            Post post1 = new Post();
            post1.PostID = 1;
            post1.Name = "Post 1";
            post1.CreatedBy = -9;
            post1.CreatedDate = DateTime.Now;
            posts.Add(post1);

            Post post2 = new Post();
            post2.PostID = 2;
            post2.Name = "Post 2";
            post2.CreatedBy = -9;
            post2.CreatedDate = DateTime.Now;
            posts.Add(post2);

            Post post3 = new Post();
            post3.PostID = 3;
            post3.Name = "Post 3";
            post3.CreatedBy = -9;
            post3.CreatedDate = DateTime.Now;

            posts.Add(post3);

            return posts;

        }

        public List<Comment> GetComments()
        {
            List<Comment> comments = new List<Comment>();

            Comment comment1 = new Comment();
            comment1.CommentID = 1;
            comment1.Remarks = "Comment 1";
            comment1.LikeQuantity = 168;
            comment1.DislikeQuantity = 128;
            comment1.PostID = 1;
            comment1.CreatedBy = 1;
            comment1.CreatedDate = DateTime.Now;
            comments.Add(comment1);

            Comment comment2 = new Comment();
            comment2.CommentID = 2;
            comment2.Remarks = "Comment 2";
            comment2.LikeQuantity = 168;
            comment2.DislikeQuantity = 128;
            comment2.PostID = 1;
            comment2.CreatedBy = 2;
            comment2.CreatedDate = DateTime.Now;
            comments.Add(comment2);

            Comment comment3 = new Comment();
            comment3.CommentID = 3;
            comment3.Remarks = "Comment 3";
            comment3.LikeQuantity = 168;
            comment3.DislikeQuantity = 128;
            comment3.PostID = 1;
            comment3.CreatedBy = 3;
            comment3.CreatedDate = DateTime.Now;
            comments.Add(comment3);

            Comment comment4 = new Comment();
            comment4.CommentID = 4;
            comment4.Remarks = "Comment 4";
            comment4.LikeQuantity = 168;
            comment4.DislikeQuantity = 128;
            comment4.PostID = 2;
            comment4.CreatedBy = 1;
            comment4.CreatedDate = DateTime.Now;
            comments.Add(comment4);

            Comment comment5 = new Comment();
            comment5.CommentID = 5;
            comment5.Remarks = "Comment 5";
            comment5.LikeQuantity = 168;
            comment5.DislikeQuantity = 128;
            comment5.PostID = 2;
            comment5.CreatedBy = 2;
            comment5.CreatedDate = DateTime.Now;
            comments.Add(comment5);

            Comment comment6 = new Comment();
            comment6.CommentID = 6;
            comment6.Remarks = "Comment 6";
            comment6.LikeQuantity = 168;
            comment6.DislikeQuantity = 128;
            comment6.PostID = 2;
            comment6.CreatedBy = 3;
            comment6.CreatedDate = DateTime.Now;
            comments.Add(comment6);

            Comment comment7 = new Comment();
            comment7.CommentID = 7;
            comment7.Remarks = "Comment 7";
            comment7.LikeQuantity = 168;
            comment7.DislikeQuantity = 128;
            comment7.PostID = 2;
            comment7.CreatedBy = 3;
            comment7.CreatedDate = DateTime.Now;
            comments.Add(comment7);

            Comment comment8 = new Comment();
            comment8.CommentID = 8;
            comment8.Remarks = "Comment 8";
            comment8.LikeQuantity = 168;
            comment8.DislikeQuantity = 128;
            comment8.PostID = 3;
            comment8.CreatedBy = 1;
            comment8.CreatedDate = DateTime.Now;
            comments.Add(comment8);

            return comments;

        }
      
    }
}
