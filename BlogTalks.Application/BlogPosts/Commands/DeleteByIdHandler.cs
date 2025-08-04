using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdRequest, DeleteByIdResponse>
    {
        private readonly FakeDataStore _fakeDataStore;

        public DeleteByIdHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }

        public async Task<DeleteByIdResponse> Handle(DeleteByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _fakeDataStore.GetBlogPostById(request.Id);
            await _fakeDataStore.DeleteBlogPostById(request.Id);



            return new DeleteByIdResponse();
        }
    }
}
