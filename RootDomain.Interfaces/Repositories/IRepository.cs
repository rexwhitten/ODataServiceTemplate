using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IChangeSet<T> Create { get; set; }

        IChangeSet<T> Update { get; set; }

        IChangeSet<T> Remove { get; set; }

        IQueryable<T> Query { get; set; }
    }
}
