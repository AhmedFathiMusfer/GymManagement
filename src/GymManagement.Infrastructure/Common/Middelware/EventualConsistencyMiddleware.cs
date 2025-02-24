using System.Data.Common;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Common;
using GymManagement.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Infrastructure.Common.Middelware
{
    public class EventualConsistencyMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IPublisher publisher, ApplicationDbContext dbContext)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            context.Response.OnCompleted(async () =>
            {
                try
                {
                    if (context.Items.TryGetValue("DominEventQueue", out var value) && value is Queue<IDomainEvent> dominEventQueue)
                    {
                        while (dominEventQueue!.TryDequeue(out var domainEvent))
                        {
                            await publisher.Publish(domainEvent);
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {

                }
                finally
                {
                    await transaction.DisposeAsync();
                }




            });

            await _next(context);

        }
    }
}