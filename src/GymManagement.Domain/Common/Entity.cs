using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymManagement.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; init; }
        protected readonly List<IDomainEvent> _adminEvents = [];

        protected Entity(Guid id) => Id = id;
        public List<IDomainEvent> PopdomainEvents()
        {
            var copy = _adminEvents.ToList();
            _adminEvents.Clear();
            return copy;
        }
        protected Entity()
        {

        }
    }
}