using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;


namespace GymManagement.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(
                options => options.RegisterServicesFromAssemblyContaining(typeof(DepedencyInjection))
            );


            return service;
        }
    }
}