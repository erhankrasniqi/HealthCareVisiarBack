using CQRS_Decorator.Application.Features.Commands.LoginPatient;
using CQRS_Decorator.Application.Features.Commands.RegisterPatient;
using CQRS_Decorator.Application.Contracts.Requests;
using CQRS_Decorator.Application.Responses;
using CQRSDecorate.Net.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Decorator.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AuthController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Register a new patient
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterPatientRequest request)
        {
            var command = new RegisterPatientCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Address);

            var result = await _commandDispatcher.SendAsync(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Patient login (returns JWT)
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginPatientRequest request)
        {
            var command = new LoginPatientCommand(request.Email, request.Password);
            var result = await _commandDispatcher.SendAsync(command);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
