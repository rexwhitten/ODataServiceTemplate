using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Interfaces.Channel
{
    public interface IChannel<T>
    {
        void Publish(T argument);

        void Subscribe(Action<T> subscriber);
    }
}
