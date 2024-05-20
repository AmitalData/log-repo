using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.AccountingModel.LedgerTransactionService;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Security;
using WebFreight.Web.Services;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using iTextSharp.text;
using Syncfusion.XlsIO;
using System.Data.SqlClient;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using System.Transactions;
using static Logitude.Customs.BL.Messaging.Amital.UnifreightQInvoiceList;
using NPOI.SS.Formula.Functions;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
	public class MonthlyBalancesLinesLoader
    {
		private int tenant;
		private MonthlyBalancesReportDataProvider dataProvider;
		private QueryOperations reportQueryOperations;

		public MonthlyBalancesLinesLoader(byte[] xmlFilters, int tenant)
		{
			this.tenant = tenant;
			reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

			
		}


		public MonthlyBalancesReportDataProvider LoadDataProviderByXML(byte[] xmlFilters)
		{
			reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

			BuildDataProvider();

			return dataProvider;
		}



		public byte[] GetData()
		{
			BuildDataProvider();
			return new ReportMemoryStreamService().Convert(dataProvider, typeof(MonthlyBalancesReportDataProvider), tenant);
		}
		private void BuildDataProvider()
		{
			dataProvider = new MonthlyBalancesReportDataProvider();

            SetMonthlyBalancesLine();

		}
		

		

		private void SetMonthlyBalancesLine()
		{
            ChartOfAccountRepository chartOfAccountRepository = new ChartOfAccountRepository(tenant);
            List<ChartOfAccount> chartOfAccountList =chartOfAccountRepository.GetAllByTenant(tenant);
            QueryFilterItem ChartOfAccountsIdList = reportQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountsIdList").FirstOrDefault();

			if (ChartOfAccountsIdList != null)
			{
                string[] ChartOfAccountsIdArray = ChartOfAccountsIdList.FieldValue.ToString().Split(',');
				 chartOfAccountList = chartOfAccountList.Where(a => ChartOfAccountsIdArray.Contains(a.Id)).ToList();
			 }
            var year = int.Parse(reportQueryOperations.QueryFilterItems.Where(d => d.FieldName == "NumberOfYear").FirstOrDefault()?.FieldValue?.ToString());
            List<MonthlyBalancesLine> monthlyBalancesLine=GetMonthlyBalancesReportByYearAndTenant(tenant, year);
            dataProvider.ChartOfAccountLine=new List<ChartOfAccountLine>();
            
            for (int i = 0; i < chartOfAccountList?.Count(); i++)
            {
                QueryFilterItem DetailedForJobs = reportQueryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForJobs").FirstOrDefault();
                List<MonthlyBalancesLine> monthlyBalancesLineOfChartOfAccount= monthlyBalancesLine.Where(a=>a.ChartOfAccount == chartOfAccountList[i].Id).ToList();

                ChartOfAccountLine chartOfAccountLine = 
                                new ChartOfAccountLine()
                                {
                                    QuantityForJanuary = monthlyBalancesLineOfChartOfAccount!=null? monthlyBalancesLineOfChartOfAccount.Sum(a=>a.QuantityForJanuary):0,
                                    QuantityForFebruary = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForFebruary) : 0,
                                    QuantityForMarch = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForMarch) : 0,
                                    QuantityForApril = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForApril) : 0,
                                    QuantityForMay = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForMay) : 0,
                                    QuantityForJune = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForJune) : 0,
                                    QuantityForJuly = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForJuly) : 0,
                                    QuantityForAugust = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForAugust) : 0,
                                    QuantityForSeptember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForSeptember) : 0,	
                                    QuantityForOctober = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForOctober) : 0,
                                    QuantityForNovember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForNovember) : 0,
                                    QuantityForDecember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForDecember) : 0,
                                    TotalReport = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.TotalReport) : 0,
                                    GLAcountLocalName = chartOfAccountList[i].LocalName,
                                    GLAcountNumber = chartOfAccountList[i].Code,
                                    GLAcountEnglishName = chartOfAccountList[i].EnglishName,
									MonthlyBalancesLine= (bool)DetailedForJobs?.FieldValue ? monthlyBalancesLineOfChartOfAccount: new List<MonthlyBalancesLine>(),
									LocalOpenBalance= monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.LocalOpenBalance) : 0,
                                    ForeignOpenBalance = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.ForeignOpenBalance) : 0,

                                };
				dataProvider.ChartOfAccountLine.Add(chartOfAccountLine);

            }
             
        }

        private List<MonthlyBalancesLine> GetMonthlyBalancesReportByYearAndTenant(int tenant , int year)
		{
            try
            {
                List<MonthlyBalancesLine> results = new List<MonthlyBalancesLine>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_MonthlyBalancesReport";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                   
                    command.Parameters.AddWithValue("@Tenant", tenant);
                    command.Parameters.AddWithValue("@Year", year);
                 
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MonthlyBalancesLine result = new MonthlyBalancesLine
                            {
                                QuantityForJanuary = reader["MONTH1"] != DBNull.Value ? (decimal)reader["MONTH1"] : 0,
                                QuantityForFebruary = reader["MONTH2"] != DBNull.Value ? (decimal)reader["MONTH2"] : 0,
                                QuantityForMarch = reader["MONTH3"] != DBNull.Value ? (decimal)reader["MONTH3"] : 0,
                                QuantityForApril = reader["MONTH4"] != DBNull.Value ? (decimal)reader["MONTH4"] : 0,
                                QuantityForMay = reader["MONTH5"] != DBNull.Value ? (decimal)reader["MONTH5"] : 0,
                                QuantityForJune = reader["MONTH6"] != DBNull.Value ? (decimal)reader["MONTH6"] : 0,
                                QuantityForJuly = reader["MONTH7"] != DBNull.Value ? (decimal)reader["MONTH7"] : 0,
                                QuantityForAugust= reader["MONTH8"] != DBNull.Value ? (decimal)reader["MONTH8"] : 0,
                                QuantityForSeptember= reader["MONTH9"] != DBNull.Value ? (decimal)reader["MONTH9"] : 0,
                                QuantityForOctober = reader["MONTH10"] != DBNull.Value ? (decimal)reader["MONTH10"] : 0,
                                QuantityForNovember = reader["MONTH11"] != DBNull.Value ? (decimal)reader["MONTH11"] : 0,
                                QuantityForDecember= reader["MONTH12"] != DBNull.Value ? (decimal)reader["MONTH12"] : 0,
                                TotalReport = reader["MONTH12"] != DBNull.Value ? (decimal)reader["TOTAL_MONTHS"] : 0,
								GLAcountLocalName = reader["LocalName"] != DBNull.Value ? (string)reader["LocalName"] : null,
								GLAcountNumber = reader["DisplayNumber"] != DBNull.Value ? (string)reader["DisplayNumber"] : null,
                                GLAcountEnglishName= reader["EnglishName"] != DBNull.Value ? (string)reader["EnglishName"] : null,
                                ChartOfAccount = reader["chartOfAccount"] != DBNull.Value ? (string)reader["chartOfAccount"] : null,
                                AccountId = reader["AccountId"] != DBNull.Value ? (string)reader["AccountId"] : null
                            };
                            results.Add(result);
                        }
                    }
                    connection.Close();
                }
                var ac = new AccountBalanceByDateCodeService(null, tenant, results.Select(a => a.AccountId).FirstOrDefault(), results.Select(a => a.AccountId).AsQueryable<string>());
                ac.CalculateBalance(true, null, new DateTime(year, 1, 1), false, false, true, false, false);
				foreach (var result in results)
				{
					var CurrencySumUntillMounth = ac.AccountBalance.verbose.CurrencySumUntillMounth.Where(A => A.AccountId == result.AccountId).FirstOrDefault();

                    result.LocalOpenBalance = CurrencySumUntillMounth?.LocalAmountDebit - CurrencySumUntillMounth?.LocalAmountCredit;
                    result.ForeignOpenBalance = CurrencySumUntillMounth?.ForeignAmountDebit - CurrencySumUntillMounth?.ForeignAmountDebit;

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

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        public IQueryable<ControlForInvoiceLinesDataView> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ControlForInvoiceLinesDataView> iQueryable, int tenant)
		{
			QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
			if (CreateDateFilter != null)
			{
				DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
				DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
				iQueryable = iQueryable.Where(x => x.CreateDate >= startDate && x.CreateDate < endDate);

			}
			QueryFilterItem InvoiceDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceDate").FirstOrDefault();
			if (InvoiceDateFilter != null)
			{
				DateTime startDate = ((DateTime)InvoiceDateFilter.FieldValue).Date;
				DateTime endDate = ((DateTime)InvoiceDateFilter.FieldValue2).Date.AddDays(1);
				iQueryable = iQueryable.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate < endDate);

			}
			QueryFilterItem TaxReportIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TaxReportId").FirstOrDefault();
			if (TaxReportIdFilter != null)
			{
				iQueryable = iQueryable.Where(x => x.TaxReportId == TaxReportIdFilter.FieldValue.ToString());

			}
			QueryFilterItem NotIncludedInAnyTaxReportFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "NotIncludedInAnyTaxReport").FirstOrDefault();
			if (NotIncludedInAnyTaxReportFilter != null)
			{
				iQueryable = iQueryable.Where(x => x.TaxReportId == null|| (x.StatusCode!="T" && x.StatusCode != "J"));

			}

			QueryFilterItem AmountInLocalCurrencyFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AmountInLocalCurrency").FirstOrDefault();
			if (AmountInLocalCurrencyFilter != null)
			{
				double amount;
				double amount2;
				double.TryParse(AmountInLocalCurrencyFilter.FieldValue.ToString(), out amount);
				double.TryParse(AmountInLocalCurrencyFilter.FieldValue2?.ToString(), out amount2);

				switch (AmountInLocalCurrencyFilter.Operator)
				{
					case "LargerThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount < (pageLine.AmountInLocalCurrency)
							//||
							//-1 * amount > (pageLine.AmountInLocalCurrency)
							);
							break;
						}

					case "GreaterThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount <= (pageLine.AmountInLocalCurrency)
							//||
							//-1 * amount >= (pageLine.AmountInLocalCurrency)
							);
							break;
						}

					case "LessThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount > (pageLine.AmountInLocalCurrency)));
							break;
						}

					case "LessThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount >= (pageLine.AmountInLocalCurrency)));
							break;
						}

					case "NotEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount != (pageLine.AmountInLocalCurrency))
							//&&
							//(-1 * amount != (pageLine.AmountInLocalCurrency))
							);
							break;
						}

					case "Between":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount <= (pageLine.AmountInLocalCurrency))
							&&
							(amount2 >= (pageLine.AmountInLocalCurrency)));
							break;
						}
					case "Equals":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.AmountInLocalCurrency))
							);
							break;
						}
					default:
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.AmountInLocalCurrency))
							||
							(amount2 == (pageLine.AmountInLocalCurrency))
							);
							break;
						}
				}

			}
			QueryFilterItem LineActionCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LineActionCode").FirstOrDefault();
			if (LineActionCodeFilter != null)
			{
				if (LineActionCodeFilter.FieldValue.ToString() == "1")
				{
					iQueryable = iQueryable.Where(x => x.LineActionCode == LineActionCodeFilter.FieldValue.ToString());
				}
				else
				{
					iQueryable = iQueryable.Where(x => x.LineActionCode == LineActionCodeFilter.FieldValue.ToString() || x.LineActionCode == LineActionCodeFilter.FieldValue2.ToString());

				}
			}
			QueryFilterItem IsExternalEntityFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsExternalEntity").FirstOrDefault();
			if (IsExternalEntityFilter != null)
			{
				bool value = IsExternalEntityFilter.FieldValue?.ToString() == "1" ? true : false; 
				iQueryable = iQueryable.Where(x => x.IsExternalEntity == value);

			}
			QueryFilterItem TotalExamptFortaxReportFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TotalExamptFortaxReport").FirstOrDefault();
			if (TotalExamptFortaxReportFilter != null)
			{
				decimal amount;
				decimal amount2;
				decimal.TryParse(TotalExamptFortaxReportFilter.FieldValue.ToString(), out amount);
				decimal.TryParse(TotalExamptFortaxReportFilter.FieldValue2?.ToString(), out amount2);

				switch (TotalExamptFortaxReportFilter.Operator)
				{
					case "LargerThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount < (pageLine.TotalExamptFortaxReport)
							//||
							//-1 * amount > (pageLine.TotalExamptFortaxReport)
							);
							break;
						}

					case "GreaterThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount <= (pageLine.TotalExamptFortaxReport)
							//||
							//-1 * amount >= (pageLine.TotalExamptFortaxReport)
							);
							break;
						}

					case "LessThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount > (pageLine.TotalExamptFortaxReport)));
							break;
						}

					case "LessThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount >= (pageLine.TotalExamptFortaxReport)));
							break;
						}

					case "NotEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount != (pageLine.TotalExamptFortaxReport))
							//&&
							//(-1 * amount != (pageLine.TotalExamptFortaxReport))						
							);
							break;
						}

					case "Between":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount <= (pageLine.TotalExamptFortaxReport))
							&&
							(amount2 >= (pageLine.TotalExamptFortaxReport)));
							break;
						}
					case "Equals":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalExamptFortaxReport))							
							);
							break;
						}
					default:
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalExamptFortaxReport))
							||
							(amount2 == (pageLine.TotalExamptFortaxReport))
							);
							break;
						}
				}

			}
			QueryFilterItem TotalVATFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TotalVAT").FirstOrDefault();
			if (TotalVATFilter != null)
			{
				decimal amount;
				decimal amount2;
				decimal.TryParse(TotalVATFilter.FieldValue.ToString(), out amount);
				decimal.TryParse(TotalVATFilter.FieldValue2?.ToString(), out amount2);

				switch (TotalVATFilter.Operator)
				{
					case "LargerThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount < (pageLine.TotalVAT)
							//||
							//-1 * amount > (pageLine.TotalVAT)
							);
							break;
						}

					case "GreaterThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount <= (pageLine.TotalVAT)
							//||
							//-1 * amount >= (pageLine.TotalVAT)
							);
							break;
						}

					case "LessThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount > (pageLine.TotalVAT)));
							break;
						}

					case "LessThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount >= (pageLine.TotalVAT)));
							break;
						}

					case "NotEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount != (pageLine.TotalVAT))
							//&&
							//(-1 * amount != (pageLine.TotalVAT))
							);
							break;
						}

					case "Between":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount <= (pageLine.TotalVAT))
							&&
							(amount2 >= (pageLine.TotalVAT)));
							break;
						}
					case "Equals":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalVAT))
							);
							break;

						}
					default:
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalVAT))
							||
							(amount2 == (pageLine.TotalVAT))
							);
							break;
						}
				}

			}
			QueryFilterItem TotalAmountForTaxReportFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TotalAmountForTaxReport").FirstOrDefault();
			if (TotalAmountForTaxReportFilter != null)
			{
				decimal amount;
				decimal amount2;
				decimal.TryParse(TotalAmountForTaxReportFilter.FieldValue.ToString(), out amount);
				decimal.TryParse(TotalAmountForTaxReportFilter.FieldValue2?.ToString(), out amount2);

				switch (TotalAmountForTaxReportFilter.Operator)
				{
					case "LargerThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount < (pageLine.TotalAmountForTaxReport)
							//||
							//-1 * amount > (pageLine.TotalAmountForTaxReport)
							);
							break;
						}

					case "GreaterThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							amount <= (pageLine.TotalAmountForTaxReport)
							//||
							//-1 * amount >= (pageLine.TotalAmountForTaxReport)
							);
							break;
						}

					case "LessThan":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount > (pageLine.TotalAmountForTaxReport)));
							break;
						}

					case "LessThanOrEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount >= (pageLine.TotalAmountForTaxReport)));
							break;
						}

					case "NotEqual":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount != (pageLine.TotalAmountForTaxReport))
							//&&
							//(-1 * amount != (pageLine.TotalAmountForTaxReport))
							);
							break;
						}

					case "Between":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount <= (pageLine.TotalAmountForTaxReport))
							&&
							(amount2 >= (pageLine.TotalAmountForTaxReport)));
							break;
						}
					case "Equals":
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalAmountForTaxReport))				
							);
							break;
						}
					default:
						{
							iQueryable = iQueryable.Where(pageLine =>
							(amount == (pageLine.TotalAmountForTaxReport))
							||
							(amount2 == (pageLine.TotalAmountForTaxReport))
							);
							break;
						}
				}

			}
			QueryFilterItem MainEntityReferenceFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainEntityReference").FirstOrDefault();
			if (MainEntityReferenceFilter != null)
			{
				switch (MainEntityReferenceFilter.Operator)
				{
					case "Equals":
						{
							iQueryable = iQueryable.Where(x => x.MainEntityReference == MainEntityReferenceFilter.FieldValue.ToString());
							break;
						}
					case "NotEqual":
						{
							iQueryable = iQueryable.Where(x => x.MainEntityReference != MainEntityReferenceFilter.FieldValue.ToString());
							break;
						}
				}
			}
			QueryFilterItem DescriptionFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Description").FirstOrDefault();
			if (DescriptionFilter != null)
			{
				iQueryable = iQueryable.Where(x => x.Description == DescriptionFilter.FieldValue.ToString());

			}
			return iQueryable;
		}

		private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
		{
			MemoryStream memorystream = new MemoryStream(xmlFilters);
			XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
			QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
			return queryOperations;
		}

	}

}

