using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace GymManagement.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequset, TResponse>(IValidator<TRequset>? validator = null)
     : IPipelineBehavior<TRequset, TResponse>
     where TRequset : IRequest<TResponse>
     where TResponse : IErrorOr
    {

        private readonly IValidator<TRequset>? _validator = validator;

        public async Task<TResponse> Handle(TRequset request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validatorResult.IsValid)
            {
                return await next();
            }

            var error = validatorResult.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();

            return (dynamic)error;
        }
    }
}