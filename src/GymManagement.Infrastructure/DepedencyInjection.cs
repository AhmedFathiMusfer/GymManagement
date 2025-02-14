using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using GymManagement.Application.Services;

namespace GymManagement.Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {

            return service;
        }
    }
}