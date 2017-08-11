using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces
{
    public interface IService<T> : IDisposable
    {
        IQueryable<T> Query(ClaimsPrincipal userContext);

        IQueryable<T> Query(ClaimsPrincipal userContext, int key);

        Task<T> FindAsync(ClaimsPrincipal claimsPrincipal, int key);

        Task UpdateAsync(ClaimsPrincipal claimsPrincipal, T item);

        Task CreateAsync(ClaimsPrincipal claimsPrincipal, T item);

        Task RemoveAsync(ClaimsPrincipal claimsPrincipal, T item);

        bool Exists(int key);
    }
}
