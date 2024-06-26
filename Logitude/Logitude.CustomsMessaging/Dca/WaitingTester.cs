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

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SendWaiting:");
            int succ = 0;

            try
            {
                //var sYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService = new Logitude.CustomsMessaging.MessagingServices.SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService();
                //var messageRestoreRequestParams= new MessageRestoreRequestParams
                //{
                //    //SuppressSplitWR = true,
                //    Tenant = 6,
                //    FromDate = DateTime.Now.AddHours(-15),
                //    ToDate = DateTime.Now,
                //    //InterfaceManagementsCode = "2470",
                //    InterfaceManagementsCode = "SendDF_MSG2470_ReleaseGoodsMessage_EX",
                //    RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive
                //};
                //var res1= sYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService.Send(messageRestoreRequestParams);


                var o = new Logitude.CustomsMessaging.MessagingServices.NG_9100_MSG_OutgoingMessageRequestMessagingService();
                var messageWaitingRequestParams = new MessageWaitingRequestParams
                {
                    //SuppressSplitWR = true,
                    Tenant = 6,
                    FromDate = DateTime.Now.AddDays(-30),
                    ToDate = DateTime.Now,
                    //InterfaceManagementsCode = "2470",
                    //InterfaceManagementsCode = "SendDF_MSG2470_ReleaseGoodsMessage",
                    ///InterfaceManagementsCode = "SendCOLT_MSG_8213_CollateralAnswerApprovalMsg_EX",
                    //InterfaceManagementsCode= "SendDF_MSG2470_ReleaseGoodsMessage_Out.",
                    //RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
                    RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive
                };

                var res = o.Send(messageWaitingRequestParams);

            }
            finally
            {

            }

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("GetDeclarationsThatCanResend:succ:" + succ.ToString());
        }
    }
}
