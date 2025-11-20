using book.Models;
using Microsoft.EntityFrameworkCore;

namespace book.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Books> Books { get; set; } 
    }
}
