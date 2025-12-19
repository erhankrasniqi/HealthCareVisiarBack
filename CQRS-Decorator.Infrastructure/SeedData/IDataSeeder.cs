namespace CQRS_Decorator.Infrastructure.SeedData
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
}
