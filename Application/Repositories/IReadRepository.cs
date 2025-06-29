using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Func<T, bool> filter, bool tracking = true);
        Task<T> GetSingleAsync(Func<T, bool> filter, bool tracking = true);
        Task<T> GetByIdAsync(int id, bool tracking = true);
    }
}
