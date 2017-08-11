using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Filter
{
    /// <summary>
    /// Filters Entities based on global logic
    /// Used by the Implementations of IService
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilter<T>
    {
        IQueryable<T> Filter(IQueryable<T> query);
    }
}
