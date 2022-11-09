using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CallBack.Handler.EmailDocument
{
    public class AutomationDocumentOutHandlerService : IHandlerService
    {
        public void Handel(string handlerArgs, object result)
        {
            AutomationQueueArgs automationQueueArgs = GetAutomationQueueArgs(handlerArgs);
            automationQueueArgs.CameFromCallBack = true;
            new GeneralAutomationResultService().AddAutomationQueue(automationQueueArgs);
        }

        private AutomationQueueArgs GetAutomationQueueArgs(string handlerArgs)
        {
            if (handlerArgs == null) return null;
            return LogitudeXmlSerializer.DeserializeObject<AutomationQueueArgs>(handlerArgs);
        }
    }
}