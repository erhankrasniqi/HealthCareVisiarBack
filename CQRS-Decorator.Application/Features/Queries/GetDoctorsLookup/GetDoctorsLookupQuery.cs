using CQRS_Decorator.Application.ReadModels;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetDoctorsLookup
{
    public record GetDoctorsLookupQuery() : IQuery<IEnumerable<DoctorLookupReadModel>>;
}
