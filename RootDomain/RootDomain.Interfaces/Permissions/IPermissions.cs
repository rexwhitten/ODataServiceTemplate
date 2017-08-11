using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Permissions
{
    public interface IPermissions<T>
    {
        IQueryable<T> Filter(ClaimsPrincipal userContext, IQueryable<T> query);
        /// <summary>
        /// throws a PermissionsException if the user can not create this item
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        void CanCreate(ClaimsPrincipal claimsPrincipal, T item);
        /// <summary>
        /// throws a PermissionsException if the user can not remove this item
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        void CanRemove(ClaimsPrincipal claimsPrincipal, T item);
        /// <summary>
        /// throws a PermissionsException if the user can not update this item
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="item"></param>
        void CanUpdate(ClaimsPrincipal claimsPrincipal, T item);
    }
}
