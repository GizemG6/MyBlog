using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Features.Queries.Blogs.GetAllBlog
{
    public class GetAllBlogsQueryRequest : IRequest<List<GetAllBlogsQueryResponse>>
    {
    }
}
