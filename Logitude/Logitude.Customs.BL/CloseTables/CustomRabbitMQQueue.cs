using Logitude.Customs.BL.Messaging;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
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
                Code = "UCUW2L",
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
                    return new UCBUD2LT_ConnectDocToTicketQService(queue);
                    break;
                case AnalyzeMQQueueServiceEnum.DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService:
                    return new UCUW2L_OpenDeclarationsQService(queue);
                    break;
                default:

                    throw new Exception("No analyze service define " + queue.Code);
                    break;
            }
        }
 
    }
    public enum AnalyzeMQQueueServiceEnum
    {
        none,
        UniCourierBatchSendUCBUD2LT_MsgResponseService,
        DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService
    }
 
    public class QueueDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public AnalyzeMQQueueServiceEnum AnalyzeQueueService { get; internal set; }

    }


 
}
