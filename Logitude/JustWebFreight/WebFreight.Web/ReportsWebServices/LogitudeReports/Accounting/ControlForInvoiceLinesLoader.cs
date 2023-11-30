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

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
	public class ControlForInvoiceLinesLoader
	{
		private int tenant;
		private ControlForInvoiceLinesDataProvider dataProvider;
		private QueryOperations reportQueryOperations;

		public ControlForInvoiceLinesLoader(byte[] xmlFilters, int tenant)
		{
			this.tenant = tenant;
			reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

			
		}


		public ControlForInvoiceLinesDataProvider LoadDataProviderByXML(byte[] xmlFilters)
		{
			reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

			BuildDataProvider();

			return dataProvider;
		}



		public byte[] GetData()
		{
			BuildDataProvider();
			return new ReportMemoryStreamService().Convert(dataProvider, typeof(ControlForInvoiceLinesDataProvider), tenant);
		}
		private void BuildDataProvider()
		{
			dataProvider = new ControlForInvoiceLinesDataProvider();

			SetReportHeaderFields();

			SetArInvoice();

		}
		private void SetReportHeaderFields()
		{
			QueryOperationsFilterValueGetter valueGetter = new QueryOperationsFilterValueGetter(this.reportQueryOperations);
           

			dataProvider.InvoiceDateFrom  = valueGetter.GetFilterValue<DateTime?>("InvoiceDate");
			dataProvider.InvoiceDateTo = valueGetter.GetFilterValue2<DateTime?>("InvoiceDate");	
			dataProvider.CreateDateFrom = valueGetter.GetFilterValue<DateTime?>("CreateDate");
			dataProvider.CreateDateTo = valueGetter.GetFilterValue2<DateTime?>("CreateDate");
			dataProvider.TaxReportId = valueGetter.GetFilterValue<string>("TaxReportId");
			dataProvider.TaxReportName = valueGetter.GetFilterValue2<string>("TaxReportId");

			dataProvider.NotIncludedInAnyTaxReport = valueGetter.GetFilterValue<bool>("NotIncludedInAnyTaxReport");
			dataProvider.AmountInLocalCurrency = valueGetter.GetFilterValue<double?>("AmountInLocalCurrency");
			dataProvider.AmountInLocalCurrency2 = valueGetter.GetFilterValue2<double?>("AmountInLocalCurrency");
			dataProvider.AmountInLocalCurrencyOp = valueGetter.GetOperatorByFieldName("AmountInLocalCurrency");

			dataProvider.LineActionCode = valueGetter.GetFilterValue<string>("LineActionCode");
			dataProvider.IsExternalEntity = valueGetter.GetFilterValue<string>("IsExternalEntity")?.ToString();
			dataProvider.TotalExamptFortaxReport = valueGetter.GetFilterValue<Decimal?>("TotalExamptFortaxReport");
			dataProvider.TotalVAT = valueGetter.GetFilterValue<Decimal?>("TotalVAT");
			dataProvider.TotalAmountForTaxReport = valueGetter.GetFilterValue<Decimal?>("TotalAmountForTaxReport");
			dataProvider.TotalExamptFortaxReport2 = valueGetter.GetFilterValue2<Decimal?>("TotalExamptFortaxReport");
			dataProvider.TotalVAT2 = valueGetter.GetFilterValue2<Decimal?>("TotalVAT");
			dataProvider.TotalAmountForTaxReport2 = valueGetter.GetFilterValue2<Decimal?>("TotalAmountForTaxReport");
			dataProvider.TotalExamptFortaxReportOp = valueGetter.GetOperatorByFieldName("TotalExamptFortaxReport");
			dataProvider.TotalVATOp = valueGetter.GetOperatorByFieldName("TotalVAT");
			dataProvider.TotalAmountForTaxReportOp = valueGetter.GetOperatorByFieldName("TotalAmountForTaxReport");
			dataProvider.MainEntityReference = valueGetter.GetFilterValue<string>("MainEntityReference");
			dataProvider.MainEntityReferenceOp = valueGetter.GetOperatorByFieldName("MainEntityReference");

			dataProvider.Description = valueGetter.GetFilterValue<string>("Description");


		}

		List<InvoiceLine> invoiceLines = new List<InvoiceLine>();

		private void SetArInvoice()
		{


			ARInvoiceRepository arInvoiceRepository = new ARInvoiceRepository(tenant);
			var invoicesLines = arInvoiceRepository.GetControlForInvoiceLinesDataView(tenant);
			invoicesLines = this.ApplyCustomFilters(reportQueryOperations, invoicesLines, tenant);
			invoiceLines = (from a in invoicesLines
							select new InvoiceLine()
							{
								InvoiceDate = a.InvoiceDate,
								InvoiceNumber = a.InvoiceNumber,
								Description = a.Description,
								LineActionCode = a.LineActionCode,
								VatPercentage = a.VatPercentage,
								LocalCurrencyAmount = a.LocalCurrencyAmount,
								AccountTypeCode = a.AccountTypeCode,
								Displaynumber = a.Displaynumber,
								LocalName1 = a.LocalName1,
								LocalName2 = a.LocalName2,
								Code = a.Code,
								TypeCode = a.TypeCode,
								LocalName3 = a.LocalName3,
								TaxReportMonth = a.TaxReportMonth,

							}).ToList();
			dataProvider.InvoiceLine = invoiceLines;
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

