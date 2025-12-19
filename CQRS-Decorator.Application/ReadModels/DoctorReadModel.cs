namespace CQRS_Decorator.Application.DTOs
{
    public class DoctorReadModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsAvailable { get; set; }
    }
}
