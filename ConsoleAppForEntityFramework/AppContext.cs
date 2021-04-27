using Microsoft.EntityFrameworkCore;


namespace ConsoleAppForEntityFramework
{
    public class AppContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=sampledb;Trusted_Connection=True;");
        }
    }
}
