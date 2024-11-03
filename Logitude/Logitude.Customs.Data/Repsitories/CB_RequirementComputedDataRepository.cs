
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CB_RequirementComputedDataRepository : IRepository<CB_RequirementComputedData>
    {

        public List<CB_RequirementComputedData> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }


        public List<CB_RequirementComputedDataList> GetCustomsBookRegularityRequirementData(int customsItemID)
        {
            try
            {
                List<CB_RequirementComputedDataList> results = new List<CB_RequirementComputedDataList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CB_RegularityRequirementData";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomsItemID", customsItemID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_RequirementComputedDataList
                            {
                                // following to match the actual column names from file 202413061852_usp_RegularityRequirementData.sxml:
                                CustomsItemID = reader["CustomsItemID"] != DBNull.Value ? (int)reader["CustomsItemID"] : 0,
                                ID = reader["ID"] != DBNull.Value ? (int)reader["ID"] : 0,
                                RequirementValidOrigin = reader["RequirementValidOrigin"] != DBNull.Value ? (string)reader["RequirementValidOrigin"] : null,
                                RequirementGoodsDescription = reader["RequirementGoodsDescription"] != DBNull.Value ? (string)reader["RequirementGoodsDescription"] : null,
                                Authority = reader["Authority"] != DBNull.Value ? (string)reader["Authority"] : null,
                                ConfirmationType = reader["ConfirmationType"] != DBNull.Value ? (string)reader["ConfirmationType"] : null,
                                InterConditionsRelationship = reader["InterConditionsRelationship"] != DBNull.Value ? (string)reader["InterConditionsRelationship"] : null,
                                TextualCondition = reader["TextualCondition"] != DBNull.Value ? (string)reader["TextualCondition"] : null,
                                IsPersonalImportIncluded = reader["IsPersonalImportIncluded"] != DBNull.Value ? (bool)reader["IsPersonalImportIncluded"] : false,
                                IsCarnetIncluded = reader["IsCarnetIncluded"] != DBNull.Value ? (bool)reader["IsCarnetIncluded"] : false,
                                IsVoluntaryOrImporterOfTrust = reader["IsVoluntaryOrImporterOfTrust"] != DBNull.Value ? (bool)reader["IsVoluntaryOrImporterOfTrust"] : false,

                                // TODO: add the rest of the fields after the requirements are clear:
                                // FromEpisodeDetail = reader["FromEpisodeDetail"] != DBNull.Value ? (string)reader["FromEpisodeDetail"] : null,
                                // AutonomyRegion = reader["AutonomyRegion"] != DBNull.Value ? (string)reader["AutonomyRegion"] : null,
                            };
                            results.Add(result);
                        }
                    }
                    connection.Close();
                }

                return results;
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
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

    }

}
