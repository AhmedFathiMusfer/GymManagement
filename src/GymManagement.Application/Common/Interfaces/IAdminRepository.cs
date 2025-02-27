using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Admins;

namespace GymManagement.Application.Common.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByIdAsync(Guid adminId);

        Task AddAdminAsync(Admin admin);
        Task UpdateAsync(Admin admin);
    }
}