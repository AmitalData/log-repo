 
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
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_CustomsItemComputedDataRepository:IRepository<CB_CustomsItemComputedData>
   {
        
		public List<CB_CustomsItemComputedData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainViewSearchByText(string searchFields, string customsBookType, string customsItemHierarchic, bool isReamarks, bool isRules, int tenant)
        {
            try
            {
                List<CB_CustomsItemComputedDataList> results = new List<CB_CustomsItemComputedDataList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_NewCustomsBookMainViewSearchByText";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@SearchFields", searchFields);
                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    AddSqlParameter(command, "@CustomsItemHierarchic", customsItemHierarchic);
                    command.Parameters.AddWithValue("@Remarks", isReamarks);
                    command.Parameters.AddWithValue("@Rules", isRules);
                    command.Parameters.AddWithValue("@Tenant", tenant);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemComputedDataList
                            {
                                CustomsItemID = reader["CustomsItemId"] != DBNull.Value ? (int)reader["CustomsItemId"] : 0,
                                CI_Parent_CustomsItemIDNum = reader["Parent_CustomsItemID"] != DBNull.Value ? (int?)reader["Parent_CustomsItemID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CI_BaseFullClassification = reader["BaseFullClassification"] != DBNull.Value ? (string)reader["BaseFullClassification"] : null,
                                CI_ComputedCheckDigit = reader["ComputedCheckDigit"] != DBNull.Value ? (string)reader["ComputedCheckDigit"] : null,
                                ItemHierarchicLocationID = reader["ItemHierarchicLocationID"] != DBNull.Value ? (string)reader["ItemHierarchicLocationID"] : null,
                                IsLeaf = reader["IsLeaf"] != DBNull.Value && (int)reader["IsLeaf"]  == 1 ? true : false,
                                CIH_GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                IsRulesExists = reader["Rules"] != DBNull.Value && (int)reader["Rules"] == 1 ? true : false,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnitName = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
                                SearchByTextResult = reader["Result"] != DBNull.Value ? (string)reader["Result"] : null,
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

        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainViewSearchByClassification(string customsBookType, string fullClassification, int tenant)
        {
            try
            {
                List<CB_CustomsItemComputedDataList> results = new List<CB_CustomsItemComputedDataList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_NEWCustomsBookMainViewSearchByClassification";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    AddSqlParameter(command, "@FullClassification", fullClassification);
                    command.Parameters.AddWithValue("@Tenant", tenant);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemComputedDataList
                            {
                                CustomsItemID = reader["CustomsItemId"] != DBNull.Value ? (int)reader["CustomsItemId"] : 0,
                                CI_Parent_CustomsItemIDNum = reader["Parent_CustomsItemID"] != DBNull.Value ? (int?)reader["Parent_CustomsItemID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CI_BaseFullClassification = reader["BaseFullClassification"] != DBNull.Value ? (string)reader["BaseFullClassification"] : null,
                                CI_ComputedCheckDigit = reader["ComputedCheckDigit"] != DBNull.Value ? (string)reader["ComputedCheckDigit"] : null,
                                ItemHierarchicLocationID = reader["ItemHierarchicLocationID"] != DBNull.Value ? (string)reader["ItemHierarchicLocationID"] : null,
                                IsLeaf = reader["IsLeaf"] != DBNull.Value ? (bool)reader["IsLeaf"] : false,
                                CIH_GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                IsRulesExists = reader["Rules"] != DBNull.Value ? (bool)reader["Rules"] : false,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnitName = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
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

        public List<CB_CustomsItemComputedDataList> GetCustomsBookMainView(string customsBookType, int tenant)
        {
            try
            {
                List<CB_CustomsItemComputedDataList> results = new List<CB_CustomsItemComputedDataList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookMainView";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    command.Parameters.AddWithValue("@Tenant", tenant);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemComputedDataList
                            {
                                CustomsItemID = reader["CustomsItemId"] != DBNull.Value ? (int)reader["CustomsItemId"] : 0,
                                CI_Parent_CustomsItemIDNum = reader["Parent_CustomsItemID"] != DBNull.Value ? (int?)reader["Parent_CustomsItemID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CI_BaseFullClassification = reader["BaseFullClassification"] != DBNull.Value ? (string)reader["BaseFullClassification"] : null,
                                CI_ComputedCheckDigit = reader["ComputedCheckDigit"] != DBNull.Value ? (string)reader["ComputedCheckDigit"] : null,
                                ItemHierarchicLocationID = reader["ItemHierarchicLocationID"] != DBNull.Value ? (string)reader["ItemHierarchicLocationID"] : null,
                                IsLeaf = reader["IsLeaf"] != DBNull.Value ? (bool)reader["IsLeaf"] : false,
                                CIH_GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                IsRulesExists = reader["Rules"] != DBNull.Value ? (bool)reader["Rules"] : false,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnitName = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
                                PH_MeasurementUnitID = reader["MeasurementUnitID"] != DBNull.Value ? (int?)reader["MeasurementUnitID"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
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


        private void AddSqlParameter(SqlCommand command, string paramName, string paramValue)
        {
            if (paramValue == null)
            {
                command.Parameters.AddWithValue(paramName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(paramName, paramValue);
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
   