using CQRS_Decorator.Infrastructure.Data;

namespace CQRS_Decorator.Infrastructure.SeedData
{
    public abstract class DataSeeder : IDataSeeder
    {
        protected readonly AppDbContext _context;

        protected DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public abstract Task SeedAsync();
    }
}
