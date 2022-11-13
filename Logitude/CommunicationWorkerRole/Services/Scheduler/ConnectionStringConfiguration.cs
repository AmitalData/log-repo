using CommunicationWorkerRole.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CommunicationWorkerRole.Services.Scheduler
{
    public class ConnectionStringConfiguration
    {
        public static string GetConnection(string dbConnectionStringName)
        {
            return GetConnectionString(dbConnectionStringName);
        }

        private static string GetConnectionString(string dbConnectionStringName)
        {
            string dbConnectionTo = ConfigurationManager.ConnectionStrings[dbConnectionStringName].ConnectionString;
            string destinationConnectionString = BuildConnectionString(GetConnectionStringArguments(dbConnectionTo));
            return destinationConnectionString;
        }

        private static string BuildConnectionString(ConnectionStringArguments connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server + ";Initial Catalog=" + connectionStringArguments.Catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName + ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        private static ConnectionStringArguments GetConnectionStringArguments(string dbConnectionTo)
        {
            string[] destinationConnectionArray = dbConnectionTo.Split(',');
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = destinationConnectionArray[0],
                UserName = destinationConnectionArray[1],
                Password = destinationConnectionArray[2],
                Server = destinationConnectionArray[3],
            };

            return connectionStringArguments;
        }
    }
}
