using FluentValidation;

namespace CQRS_Decorator.Application.Features.Commands.BookAppointment
{
    public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
    {
        public BookAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient ID is required");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required")
                .Must(BeInFuture).WithMessage("Appointment date must be today or in the future");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason for appointment is required")
                .MaximumLength(500);
        }

        private bool BeInFuture(DateTime appointmentDate)
        {
            // Compare only date parts, ignore time
            return appointmentDate.Date >= DateTime.Today;
        }
    }
}
