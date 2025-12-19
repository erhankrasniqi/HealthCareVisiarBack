using CQRS_Decorator.Application.DTOs;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetAllDoctors
{
    public class GetAllDoctorsQueryHandler : IQueryHandler<GetAllDoctorsQuery, IEnumerable<DoctorReadModel>>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorReadModel>> HandleAsync(GetAllDoctorsQuery query)
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return doctors.Select(d => new DoctorReadModel
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                FullName = d.GetFullName(),
                Specialization = d.Specialization,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                YearsOfExperience = d.YearsOfExperience,
                IsAvailable = d.IsAvailable
            });
        }
    }
}
