using ApiAngularApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiAngularApp.Data
{
    public class AppDBcontext : DbContext
    {
        public AppDBcontext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Blogpost> Blogposts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> GetEmployees { get; set; }
    }
}
