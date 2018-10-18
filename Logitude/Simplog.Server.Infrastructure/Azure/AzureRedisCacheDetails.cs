using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Azure
{
    public class AzureRedisCacheDetails
    {
        public static string GetConnectionString()
        {

            string result = "";

            switch (LogitudeSettings.DeploymentStage)
            {
                case "Simplog":
                    result = "logitude.redis.cache.windows.net:6380,password=THa/1V0Iov/549r6n6HE1aDZ8N1RTMaWZFDV3+hemoo=,ssl=True,abortConnect=False";
                    break;

                case "amitalstorage":
                    result = "logitude.redis.cache.windows.net:6380,password=THa/1V0Iov/549r6n6HE1aDZ8N1RTMaWZFDV3+hemoo=,ssl=True,abortConnect=False";
                    break;

                case "Test2"://Added By Rabaia For Testing
                    result = "logitude.redis.cache.windows.net:6380,password=THa/1V0Iov/549r6n6HE1aDZ8N1RTMaWZFDV3+hemoo=,ssl=True,abortConnect=False";
                    // result = "Endpoint=sb://logitudetest2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Uo7BHCCC7xAQIs1gO27hmruaGpFvoXDhwqATqVsH6PY=";
                    break;

                case "logitudepreproduction":
                    //result = "Endpoint=sb://logitudepreproduction.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=I7E9gyzPLLpD5+Qqub6/97l0f8WyckwMWAwsRcfVyqo=";
                    break;

                case "logboxwe1":
                    result = "logitude.redis.cache.windows.net:6380,password=THa/1V0Iov/549r6n6HE1aDZ8N1RTMaWZFDV3+hemoo=,ssl=True,abortConnect=False";
                    // result = "Endpoint=sb://logboxwe1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1ICW1EzCyGOnpj8nkC3wklYdM/4WFwCZJPAiWTNJFvs=";
                    break;

                //case "Dev":
                //case "Test1":
                default:
                    result = "logitude.redis.cache.windows.net:6380,password=THa/1V0Iov/549r6n6HE1aDZ8N1RTMaWZFDV3+hemoo=,ssl=True,abortConnect=False";
                    break;
            }

            return result;
        }
    }
}
