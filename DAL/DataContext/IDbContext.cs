using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.DataContext
{
    public interface IDbContext
    {
        public IList<T> Set<T>();
        public Task Save();
    }
}