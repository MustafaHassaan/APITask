using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class TaskDbcontext : IdentityDbContext<Users>
    {
        public TaskDbcontext(DbContextOptions<TaskDbcontext> options) : base(options) { }
        public virtual DbSet<Products> Products { get; set; }
    }
}
