using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace ZenBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
      
        public DbSet<Blog> Blogs {get;set;}
        public DbSet<Comment> Comments{get;set;}
        public DbSet<Categories> Categoriess {get;set;}
         public DbSet<Admin> Admins {get;set;}
          public DbSet<User> Users {get;set;}
       
    }
}