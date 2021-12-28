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
                var o = new Logitude.CustomsMessaging.MessagingServices.NG_9100_MSG_OutgoingMessageRequestMessagingService();

                var res = o.Send(
                    new MessageWaitingRequestParams
                    {
                        //SuppressSplitWR = true,
                        Tenant = 1,
                        FromDate = DateTime.Now.AddHours(-15),
                        ToDate = DateTime.Now,
                        InterfaceManagementsCode = "8347",
                        RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
                    });

            }

            finally
            {

            }

            Debug.WriteLine("GetDeclarationsThatCanResend:succ:" + succ.ToString());
        }
    }
}
