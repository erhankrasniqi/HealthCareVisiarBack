using CQRS_Decorator.Application.ReadModels;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetDoctorsLookup
{
    public class GetDoctorsLookupQueryHandler : IQueryHandler<GetDoctorsLookupQuery, IEnumerable<DoctorLookupReadModel>>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorsLookupQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorLookupReadModel>> HandleAsync(GetDoctorsLookupQuery query)
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return doctors.Select(d => new DoctorLookupReadModel
            {
                Id = d.Id,
                FullName = d.GetFullName(),
                Specialization = d.Specialization
            });
        }
    }
}
