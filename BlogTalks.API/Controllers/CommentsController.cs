using BlogTalks.API.DTOs;
using BlogTalks.Application.Comment.Queries;
using BlogTalks.Application.Comments.Commands;
using BlogTalks.Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        // GET: api/<CommentsController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var comments = await _mediator.Send(new GetAllRequest());
            return Ok(comments);
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}", Name = "GetCommentById")]
        public async Task<ActionResult> GetComment([FromRoute] GetByIdRequest request)
        {
            var comment = await _mediator.Send(request);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with ID {request.id} not found." });
            }
            return Ok(comment);
        }

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateResponse response)
        {
            var comment = await _mediator.Send(new AddCommentCommand(response));
            return CreatedAtRoute("GetCommentById", new { id = comment.Id }, comment);
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}", Name = "UpdateCommentById")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateCommentByIdCommand request)
        {
            if(id != request.Id)
            {
                return BadRequest(new { message = "Comment ID in the URL does not match the ID in the request body." });
            }
            try
            {
                var comment = await _mediator.Send(request);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Not found!" });
            }


        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}", Name = "DeleteCommentById")]
        public async Task<ActionResult> Delete([FromRoute] DeleteCommentByIdCommand request)
        {
            var comment = await _mediator.Send(request);

            if (comment == null)
            {
                return NotFound(new { message = $"Comment with ID {request.id} not found." });
            }
            return Ok(comment);
        }

        // GET api/<CommentsController>/blogpost/5
        [HttpGet("blogposts/{blogPostId}/comments", Name = "GetCommentsByBlogPostId")]
        public async Task<ActionResult> GetCommentsByBlogPostId([FromRoute] int blogPostId)
        {
            var comments = await _mediator.Send(new GetByBlogPostIdRequest(blogPostId));
            if (comments == null)
            {
                return NotFound(new { message = $"No comments found for Blog Post ID {blogPostId}." });
            }
            return Ok(comments);
        }
    }
}
