
using CQRSDecorate.Net.Abstractions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Decorator.Decorators
{
    public class ValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _inner;
        private readonly IEnumerable<IValidator<TCommand>> _validators;

        public ValidationDecorator(ICommandHandler<TCommand, TResult> inner, IEnumerable<IValidator<TCommand>> validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(command);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            return await _inner.HandleAsync(command);
        }
    }

}
