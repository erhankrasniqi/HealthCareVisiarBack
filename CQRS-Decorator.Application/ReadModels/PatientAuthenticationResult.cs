namespace CQRS_Decorator.Application.DTOs
{
    public class PatientAuthenticationResult
    {
        public Guid PatientId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
