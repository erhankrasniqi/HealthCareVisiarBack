using CQRS_Decorator.SharedKernel;

namespace CQRS_Decorator.Domain.Aggregates.DoctorAggregate
{
    public class Doctor : AggregateRoot<int>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Specialization { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string LicenseNumber { get; private set; }
        public int YearsOfExperience { get; private set; }
        public bool IsAvailable { get; private set; }

        private Doctor() { }

        private Doctor(
            string firstName,
            string lastName,
            string specialization,
            string email,
            string phoneNumber,
            string licenseNumber,
            int yearsOfExperience)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                ThrowDomainException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName))
                ThrowDomainException("Last name is required");
            if (string.IsNullOrWhiteSpace(specialization))
                ThrowDomainException("Specialization is required");
            if (string.IsNullOrWhiteSpace(email))
                ThrowDomainException("Email is required");
            if (string.IsNullOrWhiteSpace(licenseNumber))
                ThrowDomainException("License number is required");

            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            Email = email;
            PhoneNumber = phoneNumber;
            LicenseNumber = licenseNumber;
            YearsOfExperience = yearsOfExperience;
            IsAvailable = true;
        }

        public static Doctor Create(
            string firstName,
            string lastName,
            string specialization,
            string email,
            string phoneNumber,
            string licenseNumber,
            int yearsOfExperience)
            => new(firstName, lastName, specialization, email, phoneNumber, licenseNumber, yearsOfExperience);

        public void UpdateDetails(
            string firstName,
            string lastName,
            string specialization,
            string email,
            string phoneNumber,
            int yearsOfExperience)
        {
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            Email = email;
            PhoneNumber = phoneNumber;
            YearsOfExperience = yearsOfExperience;
            SetModifiedOn(DateTime.UtcNow);
        }

        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
            SetModifiedOn(DateTime.UtcNow);
        }

        public string GetFullName() => $"Dr. {FirstName} {LastName}";
    }
}
