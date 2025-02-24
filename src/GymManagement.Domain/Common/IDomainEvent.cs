using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace GymManagement.Domain.Common
{
    public interface IDomainEvent : INotification
    {

    }
}