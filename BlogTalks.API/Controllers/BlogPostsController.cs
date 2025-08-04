using BlogTalks.API.DTOs;
using BlogTalks.Application.BlogPost.Queries;
using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Application.BlogPosts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public static List<BlogPostDTO> _blogPosts = new List<BlogPostDTO>
        {
            new BlogPostDTO
            {
                Id = 1,
                Title = "First Blog Post",
                Text = "This is the content of the first blog post.",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                Comments = new List<CommentDTO>
                {
                    new CommentDTO
                    {
                        Id = 1,
                        Text = "Great post!",
                        CreatedAt = DateTime.Now.AddMinutes(-5),
                        CreatedBy = 2
                    }
                },
                Tags = new List<string> { "Introduction", "Welcome" }
            },
            new BlogPostDTO
            {
                Id = 2,
                Title = "Second Blog Post",
                Text = "This is the content of the second blog post.",
                CreatedAt = DateTime.Now.AddMinutes(-10),
                CreatedBy = 1,
                Comments = new List<CommentDTO>
                {
                    new CommentDTO
                    {
                        Id = 2,
                        Text = "Interesting read!",
                        CreatedAt = DateTime.Now.AddMinutes(-3),
                        CreatedBy = 3
                    }
                },
                Tags = new List<string> { "Update", "News" }
            }
        };
        
        // GET: api/<BlogPostsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetAll()
        {
            var blogPosts = await _mediator.Send(new GetAllRequest());
            return Ok(blogPosts);
        }

        // GET api/<BlogPostsController>/5
        [HttpGet("{id}", Name = "GetBlogPostById")]
        public async Task<ActionResult> Get([FromRoute] GetByIdRequest request)
        {
            var blogPost = await _mediator.Send(request);
            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID {request.id} not found." });
            }
            return Ok(blogPost);
        }

        // POST api/<BlogPostsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateResponse createResponse)
        {
            var blogPost = await _mediator.Send(new CreateRequest(createResponse));
            return CreatedAtRoute("GetBlogPostById", new { id = blogPost.Id }, blogPost);
        }

        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        public Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
            if(id != request.Id)
            {
                return Task.FromResult<ActionResult>(BadRequest(new { message = "Blog post ID in the URL does not match the ID in the request body." }));
            }
            try
            {
                var updatedBlogPost = _mediator.Send(request);
                return Task.FromResult<ActionResult>(Ok(updatedBlogPost));
            }
            catch (Exception ex)
            {
                return Task.FromResult<ActionResult>(BadRequest(new { message = ex.Message }));
            }
        }

        // DELETE api/<BlogPostsController>/5
        [HttpDelete("{id}", Name = "DeleteBlogPostById")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var blogPost = await _mediator.Send(new DeleteByIdRequest(id));

            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID { id } not found." });
            }

            return NoContent();
        }
    }
}
