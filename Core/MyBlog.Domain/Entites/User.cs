using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Entites
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
