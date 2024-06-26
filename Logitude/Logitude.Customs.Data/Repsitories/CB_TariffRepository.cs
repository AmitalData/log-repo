 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;
using System.Data.SqlClient;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using System.Transactions;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_TariffRepository:IRepository<CB_Tariff>
   {
        
		public List<CB_Tariff> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CB_TariffList> GetCustomsBookAgreementLevelData(int customsItemID, int measurementUnitMalamId)
        {
            try
            {
                List<CB_TariffList> results = new List<CB_TariffList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CB_AgreementLevelData";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomsItemIDNum", customsItemID);
                    command.Parameters.AddWithValue("@MeasurementUnitMalamId", measurementUnitMalamId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_TariffList
                            {

                                // following to match the actual column names from file 202413061852_usp_AgreementLevelData.sxml:
                                CustomsItemID = reader["CustomsItemIDNum"] != DBNull.Value ? (int)reader["CustomsItemIDNum"] : 0,
                                ID = reader["TariffID"] != DBNull.Value ? (int)reader["TariffID"] : 0,
                                CustomsRate = reader["CustomsRateWithout"] != DBNull.Value ? (string)reader["CustomsRateWithout"] : null,
                                CustomsRateWithinQuota = reader["CustomsRateWithin"] != DBNull.Value ? (string)reader["CustomsRateWithin"] : null,
                                QuotaID = reader["QuotaID"] != DBNull.Value ? (int?)reader["QuotaID"] : null,
                                MeasurementUnitName = reader["MeasurementUnit"] != DBNull.Value ? (string)reader["MeasurementUnit"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                StartDate = reader["StartDate"] != DBNull.Value ? (DateTime?)reader["StartDate"] : null,
                                EndDate = reader["EndDate"] != DBNull.Value ? (DateTime?)reader["EndDate"] : null,
                                TradeAgreementName = reader["TradeAgreementName"] != DBNull.Value ? (string)reader["TradeAgreementName"] : null,

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
        
        public List<CB_TariffList> GetCustomsBookTaxRates(int customsItemID)
        {
            try
            {
                List<CB_TariffList> results = new List<CB_TariffList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookTaxRates";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomsItemID", customsItemID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_TariffList
                            {
                                ID = reader["TarrifID"] != DBNull.Value ? (int)reader["TarrifID"] : 0,
                                TradeAgreementID = reader["TradeAgreementID"] != DBNull.Value ? (int?)reader["TradeAgreementID"] : null,
                                CustomsItemID = reader["CustomsItemID"] != DBNull.Value ? (int)reader["CustomsItemID"] : 0,
                                Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : null,
                                Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                CustomsRateWithinQuota = reader["CustomsRateWithinQuota"] != DBNull.Value ? (string)reader["CustomsRateWithinQuota"] : null,
                                QuotaID = reader["QuotaID"] != DBNull.Value ? (int?)reader["QuotaID"] : null,
                                MeasurementUnitName = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                StartDate = reader["StartDate"] != DBNull.Value ? (DateTime?)reader["StartDate"] : null,
                                EndDate = reader["EndDate"] != DBNull.Value ? (DateTime?)reader["EndDate"] : null,
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
   