using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Exceptions
{
    public class PermissionException : Exception
    {
        public PermissionException() : base() { }

        public PermissionException(string message, Type modelType) : base(message)
        {
            ModelType = modelType;
        }

        public PermissionException(string message, Type modelType, Exception inner) : base(message, inner)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; private set; }
    }
}
