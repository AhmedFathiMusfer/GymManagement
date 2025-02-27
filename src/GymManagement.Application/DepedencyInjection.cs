
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MediatR;
using GymManagement.Application.Gyms.Commands.CreateGym;
using ErrorOr;
using GymManagement.Domain.Gyms;
using FluentValidation;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;


namespace GymManagement.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(
                options =>
                {
                    options.RegisterServicesFromAssemblyContaining(typeof(DepedencyInjection));
                    options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                    options.AddOpenBehavior(typeof(ValidationBehavior<,>));

                }

            );
            service.AddValidatorsFromAssemblyContaining(typeof(DepedencyInjection));


            return service;
        }
    }
}