using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Azure
{
    public class AzureRedisCacheDetails
    {
        public static string GetConnectionString()
        {

			string result = ConfigurationManager.AppSettings["AmitalRedisCache"];
			return result;

			
        }
    }
}
