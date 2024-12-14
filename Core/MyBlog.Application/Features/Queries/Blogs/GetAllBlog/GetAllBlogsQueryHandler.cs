using MediatR;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Features.Queries.Blogs.GetAllBlog
{
    public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQueryRequest, List<GetAllBlogsQueryResponse>>
    {
        private readonly IRepository<Blog> _repository;

        public GetAllBlogsQueryHandler(IRepository<Blog> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAllBlogsQueryResponse>> Handle(GetAllBlogsQueryRequest request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetAllBlogsQueryResponse
            {
                Title = x.Title,
                Content = x.Content,
                CreatedDate = x.CreatedDate,
                UserId = x.UserId,
                CategoryId = x.CategoryId
            }).ToList();
        }
    }
}
