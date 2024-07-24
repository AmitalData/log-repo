using Logitude.CustomsMessaging.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQSRV.Testers
{
    public class PooledPublish
    {
        private byte[] message;
        private RabbitPublishService rabbitPublishService;

        public string InterfaceTypeCode { get; private set; }

        private string rabbitMQCode;

        public void Test()
        {






            message = Encoding.UTF8.GetBytes(Get("CorrelationId", "ExternalId", "body"));


            InterfaceTypeCode = "ucbud2lt";
            rabbitMQCode = RabbitmqHelper.GetRabbitMQCode(1);
            rabbitMQCode = "itziktest_" + rabbitMQCode;

            //rabbitPublishService = new RabbitPublishService();
            //RabbitPublishService.Publish(message, "communicationLogId", InterfaceTypeCode, rabbitMQCode, 5);
            //RabbitPublishService.Publish(message, "communicationLogId", InterfaceTypeCode, rabbitMQCode, 5);
            int tCount=0;
            if (true)
            {
                var worker = new List<Thread>();
                for (int i = 0; i < 80; i++)
                {
                    string threadParam = null;
                    var thread = new Thread(new ParameterizedThreadStart(MainThreadMethod));
                    thread.Name = "My:" + i;
                    thread.Start(i);
                    worker.Add(thread);
                }
                foreach (var item in worker)
                {
                    item.Join();
                }


            }
            else
            {
                var tasks = new List<Task>();

            for (int i = 0; i < 80; i++)
                {
                    int index = i;
                    tasks.Add(Task.Run(
                        () =>
                        {
                            MainThreadMethod(null);

                        }));
                }
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

            }

            try
            {
                

                // We should never get to this point
                Console.WriteLine("WaitAll() has not thrown exceptions. THIS WAS NOT EXPECTED.");
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS EXPECTED)");
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", e.InnerExceptions[j].ToString());
                }
            }




        }
        static bool delay = false;
        private void MainThreadMethod(object threadParam)
        {
            
            for (int x = 0; x < 50; x++)
            {
                try
                {
                    if (delay)
                    {
                        Thread.Sleep(10000);
                    }
                    //RabbitPublishService.Publish(message, "communicationLogId", InterfaceTypeCode, rabbitMQCode, 5);
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"{Thread.CurrentThread.Name}:{x}:success=true");
                }
                catch (Exception eee)
                {

                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"{Thread.CurrentThread.Name}:{x}:success=false,{eee.Message}");
                }
             
                
                
            }
        }

        public string Get(string CorrelationId, string ExternalId, string body)
        {
            var xml =
@"
<ns0:ESBResponse xmlns:ns0=""http://MalamTeam.Inf.ESB.Schemas.ESBResponse"">
  <ns1:ResponseHeader xmlns:ns1=""http://MalamTeam.Inf.ESB.Schemas.ResponseHeader"">
    <ns1:CorrelationId>@CorrelationId@</ns1:CorrelationId>
    <ns1:Status>Success</ns1:Status>
    <ns1:ErrorDescription></ns1:ErrorDescription>
    <ns1:ErrorCode>None</ns1:ErrorCode>
    <ns1:ExternalId>@ExternalId@</ns1:ExternalId>
  </ns1:ResponseHeader>
  <Body>
@Body@
  </Body>
</ns0:ESBResponse>";
            return xml
                .Replace("@CorrelationId@", CorrelationId)
                .Replace("@ExternalId@", ExternalId)
                .Replace("@Body@", body);

        }
    }
}
