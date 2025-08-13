using BlogTalks.API.DTOs;
using BlogTalks.Application.BlogPost.Queries;
using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Application.BlogPosts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BlogPostsController> _logger;
        public BlogPostsController(IMediator mediator, ILogger<BlogPostsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        // GET: api/<BlogPostsController>

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetAll()
        {
            _logger.LogInformation("Fetching all blog posts.");
            var blogPosts = await _mediator.Send(new GetAllRequest());
            return Ok(blogPosts);
        }

        // GET api/<BlogPostsController>/5
        [HttpGet("{id}", Name = "GetBlogPostById")]
        [Authorize]
        public async Task<ActionResult> Get([FromRoute] GetByIdRequest request)
        {
            _logger.LogInformation($"Fetching blog post with ID {request.id}.");
            var blogPost = await _mediator.Send(request);
            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID {request.id} not found." });
            }
            return Ok(blogPost);
        }

        // POST api/<BlogPostsController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] CreateRequest request)
        {
            _logger.LogInformation("Creating a new blog post.");
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
            _logger.LogInformation($"Updating blog post with ID {id}.");
            var response = _mediator.Send(new UpdateByIdRequest(id, request.Title, request.Text, request.Tags));
            if (response == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        // DELETE api/<BlogPostsController>/5
        [HttpDelete("{id}", Name = "DeleteBlogPostById")]
        [Authorize]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting blog post with ID {id}.");
            var blogPost = await _mediator.Send(new DeleteByIdRequest(id));

            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID { id } not found." });
            }

            return NoContent();
        }
    }
}
