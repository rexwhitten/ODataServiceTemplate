using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RootDomain.Host.App_Start
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            // serviceRegistry.Register<IWorkflowTaskAttachmentsService, WorkflowTaskAttachmentsService>();
        }
    }
}