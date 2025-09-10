using MediatR;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

namespace Notes.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => 
            _validators = validators;

        public Task<TResponse> Handle(TRequest request, 
            RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            
            // ищем ошибки валидации
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failures => failures != null).
                ToList();

            // кидаем исключение если есть хоть одна ошибка валидации
            if (failures.Count !=  0)
            {
                throw new ValidationException(failures);
            }

            // если нет ошибок валидации - идем дальше
            return next(); 

        }

    }
}
