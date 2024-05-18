 
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_CustomsItemRepository:IRepository<CB_CustomsItem>
   {
        
		public List<CB_CustomsItem> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<CB_CustomsItemList> GetCustomsBookMainViewSearchByText(string searchFields, string customsBookType, string customsItemHierarchic, bool isReamarks, bool isRules, int skippedRows, int pageSize)
        {
            try
            {
                List<CB_CustomsItemList> results = new List<CB_CustomsItemList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookMainViewSearchByText";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@SearchFields", searchFields);
                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    AddSqlParameter(command, "@CustomsItemHierarchic", customsItemHierarchic);
                    command.Parameters.AddWithValue("@Reamarks", isReamarks);
                    command.Parameters.AddWithValue("@Rules", isRules);
                    command.Parameters.AddWithValue("@SkippedRows", skippedRows);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemList
                            {
                                ID = reader["CustomItemId"] != DBNull.Value ? (int)reader["CustomItemId"] : 0,
                                Parent_CustomsItemID = reader["Parent_CustomsItemId"] != DBNull.Value ? (int?)reader["Parent_CustomsItemId"] : null,
                                CustomsBookTypeID = reader["CustomsBookTypeID"] != DBNull.Value ? (string)reader["CustomsBookTypeID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CustomsItemHierarchicLocatioID = reader["CustomsItemHierarchicLocationID"] != DBNull.Value ? (string)reader["CustomsItemHierarchicLocationID"] : null,
                                GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                Rules = reader["Rules"] != DBNull.Value ? (int?)reader["Rules"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnit = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
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

        public List<CB_CustomsItemList> GetCustomsBookMainViewSearchByClassification(string searchFields, string customsBookType, string customsItemHierarchic, int skippedRows, int pageSize)
        {
            try
            {
                List<CB_CustomsItemList> results = new List<CB_CustomsItemList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookMainViewSearchByClassification";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@FullClassification", searchFields);
                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    AddSqlParameter(command, "@CustomsItemHierarchic", customsItemHierarchic);
                    command.Parameters.AddWithValue("@SkippedRows", skippedRows);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemList
                            {
                                ID = reader["CustomItemId"] != DBNull.Value ? (int)reader["CustomItemId"] : 0,
                                Parent_CustomsItemID = reader["Parent_CustomsItemId"] != DBNull.Value ? (int?)reader["Parent_CustomsItemId"] : null,
                                CustomsBookTypeID = reader["CustomsBookTypeID"] != DBNull.Value ? (string)reader["CustomsBookTypeID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CustomsItemHierarchicLocatioID = reader["CustomsItemHierarchicLocationID"] != DBNull.Value ? (string)reader["CustomsItemHierarchicLocationID"] : null,
                                GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                Rules = reader["Rules"] != DBNull.Value ? (int?)reader["Rules"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnit = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
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

        public List<CB_CustomsItemList> GetCustomsBookMainView(string customsBookType, int skippedRows, int pageSize)
        {
            try
            {
                List<CB_CustomsItemList> results = new List<CB_CustomsItemList>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_CustomsBookMainView";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    AddSqlParameter(command, "@CustomsBookType", customsBookType);
                    command.Parameters.AddWithValue("@SkippedRows", skippedRows);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CB_CustomsItemList
                            {
                                ID = reader["CustomItemId"] != DBNull.Value ? (int)reader["CustomItemId"] : 0,
                                Parent_CustomsItemID = reader["Parent_CustomsItemId"] != DBNull.Value ? (int?)reader["Parent_CustomsItemId"] : null,
                                CustomsBookTypeID = reader["CustomsBookTypeID"] != DBNull.Value ? (string)reader["CustomsBookTypeID"] : null,
                                FullClassification = reader["FullClassification"] != DBNull.Value ? (string)reader["FullClassification"] : null,
                                CustomsItemHierarchicLocatioID = reader["CustomsItemHierarchicLocationID"] != DBNull.Value ? (string)reader["CustomsItemHierarchicLocationID"] : null,
                                GoodsDescription = reader["GoodsDescription"] != DBNull.Value ? (string)reader["GoodsDescription"] : null,
                                Rules = reader["Rules"] != DBNull.Value ? (int?)reader["Rules"] : null,
                                Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : null,
                                Agreements = reader["Agreements"] != DBNull.Value ? (int?)reader["Agreements"] : null,
                                CustomsRate = reader["CustomsRate"] != DBNull.Value ? (string)reader["CustomsRate"] : null,
                                PurchaseTax = reader["PurchaseTax"] != DBNull.Value ? (string)reader["PurchaseTax"] : null,
                                OptionalTaxAddition = reader["OptionalTaxAddition"] != DBNull.Value ? (decimal?)reader["OptionalTaxAddition"] : null,
                                MeasurementUnit = reader["MeasurementUnitName"] != DBNull.Value ? (string)reader["MeasurementUnitName"] : null,
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
   