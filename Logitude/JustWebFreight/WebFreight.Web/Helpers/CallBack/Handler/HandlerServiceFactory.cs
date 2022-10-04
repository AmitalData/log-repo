using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CallBack.Handler
{
    public class HandlerServiceFactory
    {

        public static IHandlerService Create(string handlerServiceName)
        {
            IHandlerService handlerService = null; 
            switch (handlerServiceName)
            {
                case "AutomationHandlerService":
                    {
                        handlerService =  new AutomationDocumentHandlerService();
                        break;
                    }
                case "EmailDocumentHandlerService":
                    {
                        handlerService = new EmailDocumentHandlerService();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return handlerService;
        }
    }
}