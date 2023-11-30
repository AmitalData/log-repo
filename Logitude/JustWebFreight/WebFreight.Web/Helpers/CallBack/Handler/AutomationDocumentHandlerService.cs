using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CallBack.Handler
{
    public class AutomationDocumentHandlerService: IHandlerService
    {
        public void Handel(string handlerArgs , object result)
        {
            AutomationHandlerArgs automationHandlerArgs = GetAutomationHandlerArgs(handlerArgs);
            new GeneralAutomationResultService().AddAutomationQueue(automationHandlerArgs.AutomationQueueArgs);
        }

        private AutomationHandlerArgs GetAutomationHandlerArgs(object handlerArgs)
        {
            if (handlerArgs == null) return null;
            var handlerArgsXml =  LogitudeXmlSerializer.SerializeObjectToXmlString(handlerArgs, true);
            return LogitudeXmlSerializer.DeserializeObject<AutomationHandlerArgs>(handlerArgsXml);
        }
    }


    public class AutomationHandlerArgs
    {
        public AutomationQueueArgs AutomationQueueArgs { get; set; }
        public int  NumberOfDocuments { get; set; }
        public string AutomationKey { get; set; }
    }
}