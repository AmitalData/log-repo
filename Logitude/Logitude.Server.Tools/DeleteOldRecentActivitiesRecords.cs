using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Server.Tools
{
    public class DeleteOldRecentActivitiesRecords
    {
        public static void DeleteOldRecentActivitiesForTenant(int tenant)
        {
            string strConnString = GetConnection(tenant);

           
            using (SqlConnection cn = new SqlConnection(strConnString))
            {


                SqlParameter tenantPar = new SqlParameter("@pTenant", SqlDbType.Int);



                tenantPar.Direction = ParameterDirection.Input;

                tenantPar.Value = tenant;

                SqlCommand cmd = new SqlCommand("DeleteOldRecentShipmensRecords", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(tenantPar);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
               

            }
        }

        public static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
    }
}
