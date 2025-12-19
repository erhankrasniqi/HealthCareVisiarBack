using FluentValidation;

namespace CQRS_Decorator.Application.Features.Commands.LoginPatient
{
    public class LoginPatientCommandValidator : AbstractValidator<LoginPatientCommand>
    {
        public LoginPatientCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
