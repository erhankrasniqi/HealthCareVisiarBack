using CQRS_Decorator.Application.DTOs;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetAllDoctors
{
    public record GetAllDoctorsQuery() : IQuery<IEnumerable<DoctorReadModel>>;
}
