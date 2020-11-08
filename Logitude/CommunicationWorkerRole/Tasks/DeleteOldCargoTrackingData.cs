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
            string strConnString = BuildConnectionString();
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
        private string BuildConnectionString()
        {
             string dbConnectionTo = ConfigurationManager.ConnectionStrings["CargoTrackingStr"].ConnectionString;
             string[] destinationConnectionArray = dbConnectionTo.Split(',');
             string destinationConnectionString = BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

            return destinationConnectionString;
        }
        private string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
    }
}
