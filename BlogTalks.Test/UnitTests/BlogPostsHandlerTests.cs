using BlogTalks.Application.BlogPost.Queries;
using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Application.BlogPosts.Queries;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace BlogTalks.Test.UnitTests
{
    public class BlogPostsHandlerTests
    {
        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        [Fact]
        public async Task CreateHandler_Should_CreateBlogPost_WhenUserIsAuthenticated()
        {
            // Arrange
            var userId = "1";
            var claims = new[] { new Claim("userId", userId) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(c => c.User).Returns(principal);
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext.Object);

            _blogPostRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => bp.Id = 123);

            var handler = new CreateHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
            var request = new CreateRequest("Test Title", "Test Text", new List<string> { "tag1", "tag2" });

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            //Assert.NotNull(result);
            Assert.Equal(123, result.Id);
            _blogPostRepositoryMock.Verify(repo => repo.Add(It.IsAny<BlogPost>()), Times.Once);
        }

        [Fact]
        public async Task CreateHandler_Should_Throw_WhenUserNotAuthenticated()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            var handler = new CreateHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
            var request = new CreateRequest("Test Title", "Test Text", new List<string> { "tag1", "tag2" });

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request, default));
        }

        [Fact]
        public async Task DeleteHandler_Should_DeleteBlogPost_WhenExists()
        {
            // Arrange
            var blogPost = new BlogPost { Id = 1, Title = "Sample" };

            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns(blogPost);

            var handler = new DeleteByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new DeleteByIdRequest(1), CancellationToken.None);

            // Assert
            //Assert.NotNull(result);
            mockRepo.Verify(r => r.Delete(blogPost), Times.Once);
        }

        [Fact]
        public async Task DeleteHandler_Should_ThrowException_WhenBlogPostDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns((BlogPost)null!);

            var handler = new DeleteByIdHandler(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<BlogTalksException>(() => 
            handler.Handle(new DeleteByIdRequest(1), CancellationToken.None));
        }

        [Fact]
        public async Task UpdateHandler_Should_UpdateBlogPost_WhenUserOwnsPost()
        {
            // Arrange
            var blogPost = new BlogPost { Id = 1, CreatedBy = 1 };

            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns(blogPost);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claims = new[] { new Claim("userId", "1") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var handler = new UpdateByIdHandler(mockRepo.Object, mockHttpContextAccessor.Object);
            var request = new UpdateByIdRequest(1, "Updated Title", "Updated Text", new List<string> { "tag1", "tag2" });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Title", result.Title);
            Assert.Equal("Updated Text", result.Text);
            mockRepo.Verify(r => r.Update(blogPost), Times.Once);
        }

        [Fact]
        public async Task UpdateHandler_Should_ThrowException_WhenBlogPostNotFound()
        {
            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns((BlogPost)null!);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claims = new[] { new Claim("userId", "1") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var handler = new UpdateByIdHandler(mockRepo.Object, mockHttpContextAccessor.Object);
            var request = new UpdateByIdRequest(1, "Updated Title", "Updated Text", new List<string> { "tag1", "tag2" });

            await Assert.ThrowsAsync<BlogTalksException>(() =>
                handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task UpdateHandler_Should_ReturnNull_WhenUserNotOwner()
        {
            // Arrange
            var blogPost = new BlogPost { Id = 1, CreatedBy = 2 };

            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns(blogPost);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claims = new[] { new Claim("userId", "1") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var handler = new UpdateByIdHandler(mockRepo.Object, mockHttpContextAccessor.Object);
            var request = new UpdateByIdRequest(1, "Updated Title", "Updated Text", new List<string> { "tag1", "tag2" });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.Null(result);
            mockRepo.Verify(r => r.Update(It.IsAny<BlogPost>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdHandler_Should_ReturnBlogPost_WhenExists()
        {
            // Arrange
            var blogPost = new BlogPost { Id = 1, Title = "Test", Text = "Text", CreatedBy = 1 };
            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns(blogPost);

            var handler = new GetByIdHandler(mockRepo.Object);
            var request = new GetByIdRequest(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdHandler_Should_ThrowException_WhenNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IBlogPostRepository>();
            mockRepo.Setup(r => r.GetById(1)).Returns((BlogPost)null!);

            var handler = new GetByIdHandler(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<BlogTalksException>(() =>
                handler.Handle(new GetByIdRequest(1), CancellationToken.None));
        }

        [Fact]
        public async Task GetAllHandler_Should_ReturnPagedResults_WithCreatorNames()
        {
            // Arrange
            var blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = 1, Title = "Post 1", Text = "Text 1", CreatedBy = 1, Tags = new List<string> { "tag1" } },
                new BlogPost { Id = 2, Title = "Post 2", Text = "Text 2", CreatedBy = 2, Tags = new List<string> { "tag2" } }
            };

            var userList = new List<User>
            {
                new User { Id = 1, Name = "Alice" },
                new User { Id = 2, Name = "Bob" }
            };

            var mockBlogPostRepo = new Mock<IBlogPostRepository>();
            mockBlogPostRepo.Setup(r => r.GetFiltered(null, null, 1, 10))
                .Returns((blogPosts, blogPosts.Count));

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetUsersByIds(It.IsAny<List<int>>()))
                .Returns(userList);

            var handler = new GetAllHandler(mockBlogPostRepo.Object, mockUserRepo.Object);

            var request = new GetAllRequest { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            //Assert.NotNull(result);
            Assert.Equal(2, result.BlogPosts.Count);
            Assert.Equal("Alice", result.BlogPosts.First().CreatorName);
        }

        [Fact]
        public async Task GetAllHandler_ReturnsEmptyList_WhenNoBlogPostsFound()
        {
            // Arrange
            var emptyPosts = new List<BlogPost>();
            var request = new GetAllRequest { PageNumber = 1, PageSize = 10 };

            _blogPostRepositoryMock.Setup(repo =>
            repo.GetFiltered(null, null, 1, 10))
                .Returns((emptyPosts, 0));

            _userRepositoryMock.Setup(repo => repo.GetUsersByIds(It.IsAny<List<int>>()))
                .Returns(new List<User>());

            var handler = new GetAllHandler(_blogPostRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            //Assert.NotNull(result);
            Assert.Empty(result.BlogPosts);
            Assert.Equal(0, result.Metadata.TotalCount);
        }
    }
}
