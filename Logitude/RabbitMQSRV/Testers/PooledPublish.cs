using Logitude.CustomsMessaging.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSRV.Testers
{
    public class PooledPublish
    {
        public void Test()
        {






            var message = Encoding.UTF8.GetBytes(Get("CorrelationId", "ExternalId", "body"));


            string InterfaceTypeCode = "ucbud2lt";
            String rabbitMQCode = RabbitmqHelper.GetRabbitMQCode(1);
            rabbitMQCode = "itziktest_" + rabbitMQCode;

            var rabbitPublishService = new RabbitPublishService();
            rabbitPublishService.Publish(message, "communicationLogId", InterfaceTypeCode, rabbitMQCode, 5);
            var tasks = new List<Task>();
            for (int i = 0; i < 80; i++)
            {
                int index = i;
                tasks.Add(Task.Run(
                    ()=> {

                        while (true)
                        {
                            rabbitPublishService.Publish(message, "communicationLogId", InterfaceTypeCode, rabbitMQCode, 5);
                        }
                        
                        
                    }));
            }

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

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
