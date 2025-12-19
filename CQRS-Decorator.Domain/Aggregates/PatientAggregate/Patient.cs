using CQRS_Decorator.Domain.Events;
using CQRS_Decorator.Domain.Exceptions;
using CQRS_Decorator.SharedKernel;

namespace CQRS_Decorator.Domain.Aggregates.PatientAggregate
{
    public class Patient : AggregateRoot<int>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Address { get; private set; }
        public string PasswordHash { get; private set; }

        private Patient() { }

        private Patient(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            string address,
            string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ValidationException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ValidationException("Last name is required");
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email is required");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ValidationException("Password is required");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Address = address;
            PasswordHash = passwordHash;

            // Raise domain event
            AddDomainEvent(new PatientRegisteredEvent(Id, email, GetFullName()));
        }

        public static Patient Create(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            string address,
            string passwordHash)
            => new(firstName, lastName, email, phoneNumber, dateOfBirth, address, passwordHash);

        public void UpdateProfile(
            string firstName,
            string lastName,
            string phoneNumber,
            string address)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ValidationException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ValidationException("Last name is required");

            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
            SetModifiedOn(DateTime.UtcNow);
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ValidationException("Password hash cannot be empty");

            PasswordHash = newPasswordHash;
            SetModifiedOn(DateTime.UtcNow);
        }

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}
