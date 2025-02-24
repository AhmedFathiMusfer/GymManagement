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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions, IHttpContextAccessor httpContextAccessor) : base(dbContextOptions)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Subscription> subscriptions { get; set; }
        public DbSet<Gym> gyms { get; set; }
        public DbSet<Admin> admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task CommitChangesAsync()
        {
            //get hold all domin events
            var domainEvents = ChangeTracker.Entries<Entity>().Select(entity => entity.Entity.PopdomainEvents()).SelectMany(x => x).ToList();
            addDominEventsToOfflineProcessingQueue(domainEvents);
            await base.SaveChangesAsync();
        }
        private void addDominEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
        {
            var domainEventsQueue = _httpContextAccessor.HttpContext!.Items.TryGetValue("DominEventQueue", out var value) && value is Queue<IDomainEvent> existingDominEvents
            ? existingDominEvents : new Queue<IDomainEvent>();

            domainEvents.ForEach(domainEventsQueue.Enqueue);

            _httpContextAccessor.HttpContext.Items["DominEventQueue"] = domainEventsQueue;
        }
    }
}