using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldCargoTrackingData : TaskManagerBase
    {
        public DeleteOldCargoTrackingData(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            DeleteCargoTrackingShipmentSearchesOlderOneYear();
            DeleteCargoTrackingShipmentsOlderOneYear();
            DeleteCargoTrackingIncrementalStatsOlderOneYear();

        }
        private void DeleteCargoTrackingIncrementalStatsOlderOneYear()
        {
            string cmd = "DELETE FROM [dbo].[CargoTrackingIncrementalStats] WHERE DATEADD(year, 1, EndDate) < getdate()";
            ExecuteCommand(cmd);
        }
        private void DeleteCargoTrackingShipmentSearchesOlderOneYear()
        {
            string cmd = "DELETE FROM [dbo].[CargoTrackingShipmentSearches] WHERE DATEADD(year, 1, ShipmentDate) < getdate()";
            ExecuteCommand(cmd);
        }
        private void DeleteCargoTrackingShipmentsOlderOneYear()
        {
            string cmd = "DELETE FROM [dbo].[CargoTrackingShipments] WHERE DATEADD(year, 1, CreateDate) < getdate()";
            ExecuteCommand(cmd);
        }
        private void ExecuteCommand(string sql)
        {
            string strConnString = GetCargoTrackingConnectionString();
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        private string GetCargoTrackingConnectionString()
        {
            string dbConnectionTo = ConfigurationManager.ConnectionStrings["CargoTrackingStr"].ConnectionString;
            string destinationConnectionString = BuildConnectionString(GetConnectionStringArguments(dbConnectionTo));
            return destinationConnectionString;
        }
        private ConnectionStringArguments GetConnectionStringArguments(string dbConnectionTo)
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
        private string BuildConnectionString(ConnectionStringArguments connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server + ";Initial Catalog=" + connectionStringArguments.Catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName + ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
    }

    public class ConnectionStringArguments
    {
        public string Server { get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Catalog { get; set; }


    }
}
