using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Repositories
{
    public interface IChangeSet<T>
    {
        IEnumerable<T> Items { get; set; }

        void AddItem(T item);

        Task SaveAsync();
    }
}
