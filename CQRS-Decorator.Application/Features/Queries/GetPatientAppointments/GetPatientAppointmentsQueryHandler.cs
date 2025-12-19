using CQRS_Decorator.Application.DTOs;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetPatientAppointments
{
    public class GetPatientAppointmentsQueryHandler : IQueryHandler<GetPatientAppointmentsQuery, IEnumerable<AppointmentReadModel>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public GetPatientAppointmentsQueryHandler(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<AppointmentReadModel>> HandleAsync(GetPatientAppointmentsQuery query)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(query.PatientId);
            var result = new List<AppointmentReadModel>();

            foreach (var appointment in appointments)
            {
                var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
                var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);

                result.Add(new AppointmentReadModel
                {
                    Id = appointment.Id,
                    PatientId = appointment.PatientId,
                    PatientName = patient?.GetFullName() ?? "Unknown",
                    DoctorId = appointment.DoctorId,
                    DoctorName = doctor?.GetFullName() ?? "Unknown",
                    DoctorSpecialization = doctor?.Specialization ?? "",
                    AppointmentDate = appointment.AppointmentDate,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Status = appointment.AppointmentStatus.ToString(),
                    Reason = appointment.Reason,
                    CreatedOn = appointment.CreatedOn
                });
            }

            return result;
        }
    }
}
