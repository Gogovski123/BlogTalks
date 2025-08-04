namespace BlogTalks.Domain.Entities
{
    public class FakeDataStore
    {
        private static List<Comment> _comments;
        private static List<BlogPost> _blogPosts;
        public FakeDataStore()
        {
            _comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "This is the first comment", CreatedAt = DateTime.Now, CreatedBy = 1 },
                new Comment { Id = 2, Text = "This is the second comment", CreatedAt = DateTime.Now, CreatedBy = 2 }
            };
            _blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = 1, Title = "First Post", Text = "This is the first post", CreatedBy = 1, CreatedAt = DateTime.Now, Tags = new List<string> { "tag1", "tag2" }, Comments = _comments },
                new BlogPost { Id = 2, Title = "Second Post", Text = "This is the second post", CreatedBy = 2, CreatedAt = DateTime.Now, Tags = new List<string> { "tag3" }, Comments = _comments }
            };
        }
        public async Task AddComment(Comment comment)
        {
            _comments.Add(comment);
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await Task.FromResult(_comments);
        }
        public async Task<Comment> GetCommentById(int id) => await Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));

        public async Task<Comment> DeleteCommentById(int id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _comments.Remove(comment);
            }
            return await Task.FromResult(comment);
        }
        public async Task<Comment> UpdateCommentById(Comment updatedComment)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == updatedComment.Id);
            if (comment != null)
            {
                comment.Text = updatedComment.Text;
                comment.CreatedAt = updatedComment.CreatedAt;
                comment.CreatedBy = updatedComment.CreatedBy;
                comment.BlogPostID = updatedComment.BlogPostID;
            }
            return await Task.FromResult(comment);
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPosts()
        {
            return await Task.FromResult(_blogPosts);
        }
        public async Task<BlogPost?> GetBlogPostById(int id)
        {
            return await Task.FromResult(_blogPosts.FirstOrDefault(bp => bp.Id == id));
        }

        public async Task AddBlogPost(BlogPost blogPost)
        {
            _blogPosts.Add(blogPost);
            await Task.CompletedTask;
        }

        public async Task<BlogPost> UpdateBlogPostById(BlogPost blogPost)
        {
            var bpost = _blogPosts.FirstOrDefault(b => b.Id == blogPost.Id);
            if (bpost != null)
            {
                bpost.Title = blogPost.Title;
                bpost.Text = blogPost.Text;
                bpost.Tags = blogPost.Tags ?? new List<string>();
                bpost.CreatedBy = blogPost.CreatedBy;
                bpost.CreatedAt = blogPost.CreatedAt;
                bpost.Comments = blogPost.Comments ?? new List<Comment>();
            }
            return await Task.FromResult(bpost);
        }

        public async Task<BlogPost> DeleteBlogPostById(int id)
        {
            var blogPost = _blogPosts.FirstOrDefault(b => b.Id == id);
            if (blogPost != null)
            {
                _blogPosts.Remove(blogPost);
            }
            return await Task.FromResult(blogPost);
        }

        public async Task<IEnumerable<Comment>> GetByBlogPostId(int blogPostId)
        {
            var comments = _comments.Where(c => c.BlogPostID == blogPostId);
            return comments;
        }
    }
}
