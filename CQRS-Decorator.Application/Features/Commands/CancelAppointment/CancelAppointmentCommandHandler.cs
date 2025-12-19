using CQRS_Decorator.Application.Responses;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.CancelAppointment
{
    public class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand, GeneralResponse<bool>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<GeneralResponse<bool>> HandleAsync(CancelAppointmentCommand command)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(command.AppointmentId);

            if (appointment == null)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Appointment not found"
                };
            }

            try
            {
                appointment.Cancel();
                await _appointmentRepository.UpdateAsync(appointment);

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Appointment cancelled successfully",
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
