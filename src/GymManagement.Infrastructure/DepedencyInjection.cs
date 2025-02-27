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
using GymManagement.Infrastructure.Gyms;
using GymManagement.Infrastructure.Admins.Presistence;
using GymManagement.Infrastructure.Users;
using GymManagement.Api.Authentication.TokenGenerator;
using Microsoft.Extensions.Options;
using GymManagement.Domain.Common.Interfaces;
using GymManagement.Api.Authentication.PasswordHasher;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace GymManagement.Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddAuthentication(configuration)
                .AddPersistence();
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source = GymManagement.db"));

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IGymRepository, GymRepository>();
            services.AddScoped<ISubscriptionsRepository, SubscriptionRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.Section, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                });


            return services;
        }
    }
}