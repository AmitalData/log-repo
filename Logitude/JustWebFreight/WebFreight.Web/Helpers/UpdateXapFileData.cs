using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class UpdateXapFileData
    {

        public void UpdateXapFile(string fileName, byte[] fileData, bool isStaging)
        {
            string dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
           // string strConnString = connection.ConnectionString;
            using (SqlConnection cn = new SqlConnection(connection.ConnectionString))
            {

                SqlParameter pfileName = new SqlParameter("@pFileName", SqlDbType.VarChar, 60);
                SqlParameter pfileData = new SqlParameter("@pFileData", SqlDbType.Binary);
                SqlParameter pisStaging = new SqlParameter("@pIsStaging", SqlDbType.Bit);


                pfileName.Direction = ParameterDirection.Input;
                pfileData.Direction = ParameterDirection.Input;
                pisStaging.Direction = ParameterDirection.Input;

                pfileName.Value =  fileName;
                pfileData.Value =  fileData;
                pisStaging.Value = isStaging;

                SqlCommand cmd = new SqlCommand("usp_UpdateXapFileData", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(pfileName);
                cmd.Parameters.Add(pfileData);
                cmd.Parameters.Add(pisStaging);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            
            }

       
        }

        public static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();

            }

            string dbConnectionInfo = currentDb.DBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }



      
    }
}