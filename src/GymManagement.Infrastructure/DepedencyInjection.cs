using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;


namespace GymManagement.Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddScoped<ISubscriptionsRepository, SubscriptionRepository>();
            service.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source= GymManagement.db")

          );
            service.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
            return service;
        }
    }
}