using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.CallBack.Handler;
using System.Runtime.Serialization;
namespace WebFreight.Web.Helpers.CallBack
{
  public  class CallBackService
    {

        public static void Notifiy(string callBackDetailsXml , object result)
        {

            Parallel.Invoke(() => Execute(callBackDetailsXml , result));
        }


        private static void Execute(string callBackDetailsXml, object result)
        {

            var callBackDetails = LogitudeXmlSerializer.DeserializeObject<CallBackDetails>(callBackDetailsXml);
            IHandlerService handlerService = HandlerServiceFactory.Create(callBackDetails.HandlerServiceName);
            handlerService.Handel(callBackDetails.HandlerArgs, result);
        }


    }


    [DataContract(Namespace = "")]
    public class CallBackDetails
    {
        [DataMember]
        public string HandlerServiceName { get; set; }
        [DataMember]
        public string HandlerArgs { get; set; }
    }
}
