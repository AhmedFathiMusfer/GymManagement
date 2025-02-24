
using GymManagement.Infrastructure.Common.Middelware;
using Microsoft.AspNetCore.Builder;

namespace GymManagement.Infrastructure
{
    public static class RequestPipeline
    {
        public static IApplicationBuilder addInfrastructureMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<EventualConsistencyMiddleware>();
            return builder;
        }
    }
}