using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Dca
{
    public class WaitingTester
    {

        public static void SendWaiting()
        {
            //12 or 13

            Debug.WriteLine("SendWaiting:");
            int succ = 0;

            try
            {
                var sYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService = new Logitude.CustomsMessaging.MessagingServices.SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService();
                var messageRestoreRequestParams= new MessageRestoreRequestParams
                {
                    //SuppressSplitWR = true,
                    Tenant = 1,
                    FromDate = DateTime.Now.AddHours(-15),
                    ToDate = DateTime.Now,
                    InterfaceManagementsCode = "8347",
                    RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.Default
                };
                var res1= sYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService.Send(messageRestoreRequestParams);


                var o = new Logitude.CustomsMessaging.MessagingServices.NG_9100_MSG_OutgoingMessageRequestMessagingService();
                var messageWaitingRequestParams = new MessageWaitingRequestParams
                {
                    //SuppressSplitWR = true,
                    Tenant = 1,
                    FromDate = DateTime.Now.AddHours(-15),
                    ToDate = DateTime.Now,
                    InterfaceManagementsCode = "8347",
                    RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
                };
                var res = o.Send(messageWaitingRequestParams);

            }
            finally
            {

            }

            Debug.WriteLine("GetDeclarationsThatCanResend:succ:" + succ.ToString());
        }
    }
}
