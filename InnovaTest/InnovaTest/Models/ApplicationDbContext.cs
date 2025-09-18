using Microsoft.EntityFrameworkCore;
using InnovaTest.Core.Models;

namespace InnovaTest.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        } 

        public DbSet<Person> People { get; set; }
    }

}
