using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Features.Queries.Blogs.GetAllBlog
{
    public class GetAllBlogsQueryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
