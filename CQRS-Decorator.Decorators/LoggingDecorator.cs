
using CQRSDecorate.Net.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Decorator.Decorators
{
    public class LoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _inner;
        private readonly ILogger<LoggingDecorator<TCommand, TResult>> _logger;

        public LoggingDecorator(ICommandHandler<TCommand, TResult> inner, ILogger<LoggingDecorator<TCommand, TResult>> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            _logger.LogInformation("Executing command: {Command}", typeof(TCommand).Name);
            var result = await _inner.HandleAsync(command);
            _logger.LogInformation("Executed command: {Command}", typeof(TCommand).Name);
            return result;
        }
    }

}
