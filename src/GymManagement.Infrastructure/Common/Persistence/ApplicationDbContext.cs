using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using GymManagement.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions
        , IHttpContextAccessor httpContextAccessor
        , IPublisher publisher) : base(dbContextOptions)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPublisher _publisher;
        public DbSet<Subscription> subscriptions { get; set; }
        public DbSet<Gym> gyms { get; set; }
        public DbSet<Admin> admins { get; set; }

        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task CommitChangesAsync()
        {
            //get hold all domin events
            var domainEvents = ChangeTracker.Entries<Entity>().Select(entity => entity.Entity.PopdomainEvents()).SelectMany(x => x).ToList();
            if (IsUserWatingOnline)
            {
                addDominEventsToOfflineProcessingQueue(domainEvents);

            }
            else
            {
                await PublishDominEvents(domainEvents, _publisher);
            }

            await base.SaveChangesAsync();
        }
        private static async Task PublishDominEvents(List<IDomainEvent> domainEvents, IPublisher publisher)
        {
            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent);
            }
        }
        private bool IsUserWatingOnline => _httpContextAccessor.HttpContext is not null;
        private void addDominEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
        {
            var domainEventsQueue = _httpContextAccessor.HttpContext!.Items.TryGetValue("DominEventQueue", out var value) && value is Queue<IDomainEvent> existingDominEvents
            ? existingDominEvents : new Queue<IDomainEvent>();

            domainEvents.ForEach(domainEventsQueue.Enqueue);

            _httpContextAccessor.HttpContext.Items["DominEventQueue"] = domainEventsQueue;
        }
    }
}