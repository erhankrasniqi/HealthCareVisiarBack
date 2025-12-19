using CQRS_Decorator.Application.Features.Commands.BookAppointment;
using CQRS_Decorator.Application.Features.Commands.CancelAppointment;
using CQRS_Decorator.Application.Features.Queries.GetPatientAppointments;
using CQRS_Decorator.Application.Contracts.Requests;
using CQRSDecorate.Net.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CQRS_Decorator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public AppointmentsController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Get all appointments for the logged-in patient
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyAppointments()
        {
            var patientId = GetPatientIdFromToken();
            if (patientId == Guid.Empty)
                return Unauthorized(new { message = "Invalid patient token" });

            var appointments = await _queryDispatcher.SendAsync(new GetPatientAppointmentsQuery(patientId));
            return Ok(appointments);
        }

        /// <summary>
        /// Book an appointment with a doctor
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentRequest request)
        {
            var patientId = GetPatientIdFromToken();
            if (patientId == Guid.Empty)
                return Unauthorized(new { message = "Invalid patient token" });

            var command = new BookAppointmentCommand(
                patientId,
                request.DoctorId,
                request.AppointmentDate,
                request.StartTime,
                request.EndTime,
                request.Reason);

            var result = await _commandDispatcher.SendAsync(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            var patientId = GetPatientIdFromToken();
            if (patientId == Guid.Empty)
                return Unauthorized(new { message = "Invalid patient token" });

            var command = new CancelAppointmentCommand(id, patientId);
            var result = await _commandDispatcher.SendAsync(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        private Guid GetPatientIdFromToken()
        {
            var patientIdClaim = User.FindFirst("patientId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            return Guid.TryParse(patientIdClaim, out var patientId) ? patientId : Guid.Empty;
        }
    }
}
