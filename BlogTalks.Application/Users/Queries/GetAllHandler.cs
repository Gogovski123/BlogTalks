using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Queries
{
    public class GetAllHandler : IRequestHandler<GetAllRequest, IEnumerable<GetAllResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<GetAllResponse>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAll();
            var response = users.Select(u => new GetAllResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
            });

            return Task.FromResult(response);
        }
    }
}
