using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces
{
    public interface IViewService<T>
    {
        IQueryable<T> Query(ClaimsPrincipal userContext);

        Task<T> FindAsync(ClaimsPrincipal claimsPrincipal, int key);
    }
}
