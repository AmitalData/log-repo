using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CourierDeclarationUpdateService
    {
        protected override void OnCreating(CourierDeclarationPM entityPM, EntityPM entityParentPM)
        {


        }

        public void FastTotalDeleteComposition(List<string> declarationIds, int tenant, string courierMasterId, out List<string> deletedDeclarationIds)
        {
            (Repository as CourierDeclarationRepository).FastDeleteMulti(declarationIds, tenant, courierMasterId, out deletedDeclarationIds);

        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);
            return context.Database.Connection.ConnectionString;
        }

        public void FastInsert(List<string> declarationIds, int tenant, string id, int? sequenceNumericMax)
        {
            ICustomContext context = MainContext as CustomContext;
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            //string whereIn = "";
            List<CourierDeclaration> addedCourierDeclarations = new List<CourierDeclaration>();
            foreach (var declarationId in declarationIds)
            {
                sequenceNumericMax += 1;
                CourierDeclaration newCourierDeclaration = new CourierDeclaration()
                {
                    Tenant = tenant,
                    CourierMasterId = id,
                    DeclarationId = declarationId,
                    SequenceNumeric = sequenceNumericMax,
                };
                addedCourierDeclarations.Add(newCourierDeclaration);
            }
            if (addedCourierDeclarations != null && addedCourierDeclarations.Count() > 0)
            {
                var tableName = "CourierDeclarations";
                if(!CustomsSettingQueryService.GetSettingByTenant(Tenant).IsConnectedToUniFreight)
                    tableName ="Customs." + tableName;
                SqlBulkInsert.BulkInsert(tableName, addedCourierDeclarations);
            }
            /*
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    con.Open();
                    foreach (var declarationId in declarationIds)
                    {
                        sequenceNumericMax += 1;
                        string cmd = "Insert into COURIERDECLARATIONS(DECLARATIONID, TENANT, COURIERMASTERID, SEQUENCENUMERIC) values(:p1, :p2, :p3, :p4)";

                        OracleCommand sqlCommand = new OracleCommand(cmd, con);
                        sqlCommand.Parameters.Add(new OracleParameter("p1", declarationId));
                        sqlCommand.Parameters.Add(new OracleParameter("p2", tenant));
                        sqlCommand.Parameters.Add(new OracleParameter("p3", id));
                        sqlCommand.Parameters.Add(new OracleParameter("p2", sequenceNumericMax));
                        sqlCommand.ExecuteNonQuery();
                    }
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONS set " +
                        "ISCLOSE= 0  , ISCANCELLED =0 ";
                    cmd = cmd + " where ID IN " + "(" + declarations + ")";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }*/
        }
    }
}

