using System.Collections.Generic;
using System.Threading.Tasks;

namespace efcore.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get(string sortParams, string filterParams, int? pageSize, int? pageNumber);

    }
}