using FluentValidation;

namespace CQRS_Decorator.Application.Features.Commands.CancelAppointment
{
    public class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
    {
        public CancelAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required");

            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient ID is required");
        }
    }
}
