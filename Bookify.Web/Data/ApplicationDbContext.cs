using Bookify.Web.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Books> Books{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookCategory>().HasKey(e => new { e.BookId, e.CategoryId });
            base.OnModelCreating(builder);
        }

    }
}