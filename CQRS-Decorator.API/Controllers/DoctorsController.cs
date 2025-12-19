using CQRS_Decorator.Application.Features.Queries.GetAllDoctors;
using CQRS_Decorator.Application.Features.Queries.GetDoctorsLookup;
using CQRSDecorate.Net.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CQRS_Decorator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        /// <summary>
        /// Get doctors lookup with ID, full name, and specialization
        /// </summary>
        [HttpGet("lookup")]
        public async Task<IActionResult> GetLookup()
        {
            var doctors = await _queryDispatcher.SendAsync(new GetDoctorsLookupQuery());
            return Ok(doctors);
        }
    }
}
