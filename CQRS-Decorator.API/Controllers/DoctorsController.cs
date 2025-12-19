using CQRS_Decorator.Application.Features.Queries.GetAllDoctors;
using CQRSDecorate.Net.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Decorator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DoctorsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Get all doctors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _queryDispatcher.SendAsync(new GetAllDoctorsQuery());
            return Ok(doctors);
        }
    }
}
