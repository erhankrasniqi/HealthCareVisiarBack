namespace CQRS_Decorator.Application.ReadModels
{
    public class DoctorLookupReadModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
    }
}
