using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymManagement.Contracts.Gyms
{
    public record GymResponse(Guid Id, string Name);
}