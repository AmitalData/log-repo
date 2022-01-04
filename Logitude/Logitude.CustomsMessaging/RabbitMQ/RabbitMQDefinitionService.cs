using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.CustomsMessaging.RabbitMQ.Handlers;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{


    public class CustomRabbitMQQueue
    {
   

        public List<QueueDetails> GetAllQueueDetails()
        {
            var all = new List<QueueDetails>() { 
            //all.Add(new KeyValuePair<string, string>("", ""));

            new QueueDetails()
            {
                Code = "ucbud2lt",
                Name = "קישור מסמך לטיקט",
               Priority=1,
               AnalyzeQueueService= AnalyzeMQQueueServiceEnum.UniCourierBatchSendUCBUD2LT_MsgResponseService
            },
            new QueueDetails()
            {
                Code = "uw2l",
                Name = "פתיחת הצהרה מאינטגרטור",
                Priority=1,
                AnalyzeQueueService= AnalyzeMQQueueServiceEnum.DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService
            }
            };
        

             return all;
        }


        public CustomAnalyzerQueueBase GetCustomAnalyzerQueueService(QueueDetails queue)
        {
            switch (queue.AnalyzeQueueService)
            {
                case AnalyzeMQQueueServiceEnum.UniCourierBatchSendUCBUD2LT_MsgResponseService:
                    return new /*UCBUD2LT_ConnectDocToTicketQService*/UCBUD2LT_RabbitMQHandler(queue);
                    break;
                case AnalyzeMQQueueServiceEnum.DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService:
                    return new /*UCUW2L_OpenDeclarationsQService*/UCUW2L_RabbitMQHandler(queue);
                    break;
                default:

                    throw new Exception("No analyze service define " + queue.Code);
                    break;
            }
        }
 
    }
   


 
}
