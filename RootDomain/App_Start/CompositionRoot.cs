using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;

namespace RootDomain.App_Start
{
    public class CompositionRoot : LightInject.ICompositionRoot
    {
        public CompositionRoot() { }

        public void Compose(IServiceRegistry serviceRegistry)
        {
            
        }
    }
}
