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
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.DataContracts;
using static Logitude.Customs.BL.Messaging.Amital.UnifreightQInvoiceList;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Customs
{
    public class ExportDeclarationLoader
    {
        private int tenant;
        private ExportDeclarationDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public ExportDeclarationLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);


        }


       


        public byte[] GetData()
        {
            ExportDeclarationDataProvider myDataProvider = new ExportDeclarationDataProvider();
           
            BuildDataProvider();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OpportunityMonthlyConversionDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void BuildDataProvider()
        {
            dataProvider = new ExportDeclarationDataProvider();


            SetExportDeclarationBase();

        }
        

        List<ExportDeclarationForReport> ExportDeclarationForReport = new List<ExportDeclarationForReport>();

        private void SetExportDeclarationBase()
        {


            DeclarationRepository declarationRepository = new DeclarationRepository(tenant);
            IQueryable<ExportDeclarationForReport> exportDeclarationsBase = declarationRepository.GetExportDeclarationsForReport(tenant, DateTime.Now, DateTime.Now);
            exportDeclarationsBase = this.ApplyCustomFilters(reportQueryOperations, exportDeclarationsBase, tenant);

            dataProvider.ExportDeclaration = (from ed in exportDeclarationsBase
                                          select new ExportDeclaration()
                                          {
                                              TaxationDateTime = ed.TaxationDateTime,
                                              ExportFile = ed.ExportFile,
                                              TransportModeName = ed.TransportModeName,
                                              CustomFileNo = ed.CustomFileNo,
                                              DeclarationNumber = ed.DeclarationNumber,
                                              DeclarationStatusTypeName = ed.DeclarationStatusTypeName,
                                              ProcedureCurrentName = ed.ProcedureCurrentName,
                                              ExporterImporterCode  = ed.ExporterImporterCode,
                                              RecipientName = ed.RecipientName,
                                              DestinationCountryName = ed.DestinationCountryName,
                                              DeclarationTypeName = ed.DeclarationTypeName,
                                              FinalCargoTypeName = ed.FinalCargoTypeName,
                                              FinalManifestNumber = ed.FinalManifestNumber,
                                              FinalSecondCargoId = ed.FinalSecondCargoId,
                                              FinalThirdCargoId = ed.FinalThirdCargoId,
                                              //Consignment = null,
                                              //Invoice = 
                                          }).ToList();
        }


        public IQueryable<ExportDeclarationForReport> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportDeclarationForReport> iQueryable, int tenant)
        {
            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                iQueryable = iQueryable.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }
            //QueryFilterItem InvoiceDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceDate").FirstOrDefault();
            //if (InvoiceDateFilter != null)
            //{
            //    DateTime startDate = ((DateTime)InvoiceDateFilter.FieldValue).Date;
            //    DateTime endDate = ((DateTime)InvoiceDateFilter.FieldValue2).Date.AddDays(1);
            //    iQueryable = iQueryable.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate < endDate);

            //}
            //QueryFilterItem TaxReportIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TaxReportId").FirstOrDefault();
            //if (TaxReportIdFilter != null)
            //{
            //    iQueryable = iQueryable.Where(x => x.TaxReportId == TaxReportIdFilter.FieldValue.ToString());

            //}
            //QueryFilterItem NotIncludedInAnyTaxReportFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "NotIncludedInAnyTaxReport").FirstOrDefault();
            //if (NotIncludedInAnyTaxReportFilter != null)
            //{
            //    iQueryable = iQueryable.Where(x => x.TaxReportId == null || (x.StatusCode != "T" && x.StatusCode != "J"));

            //}

            //QueryFilterItem AmountInLocalCurrencyFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AmountInLocalCurrency").FirstOrDefault();
            //if (AmountInLocalCurrencyFilter != null)
            //{
            //    double amount;
            //    double amount2;
            //    double.TryParse(AmountInLocalCurrencyFilter.FieldValue.ToString(), out amount);
            //    double.TryParse(AmountInLocalCurrencyFilter.FieldValue2?.ToString(), out amount2);

            //    switch (AmountInLocalCurrencyFilter.Operator)
            //    {
            //        case "LargerThan":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                amount < (pageLine.AmountInLocalCurrency)
            //                //||
            //                //-1 * amount > (pageLine.AmountInLocalCurrency)
            //                );
            //                break;
            //            }

            //        case "GreaterThanOrEqual":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                amount <= (pageLine.AmountInLocalCurrency)
            //                //||
            //                //-1 * amount >= (pageLine.AmountInLocalCurrency)
            //                );
            //                break;
            //            }

            //        case "LessThan":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount > (pageLine.AmountInLocalCurrency)));
            //                break;
            //            }

            //        case "LessThanOrEqual":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount >= (pageLine.AmountInLocalCurrency)));
            //                break;
            //            }

            //        case "NotEqual":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount != (pageLine.AmountInLocalCurrency))
            //                //&&
            //                //(-1 * amount != (pageLine.AmountInLocalCurrency))
            //                );
            //                break;
            //            }

            //        case "Between":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount <= (pageLine.AmountInLocalCurrency))
            //                &&
            //                (amount2 >= (pageLine.AmountInLocalCurrency)));
            //                break;
            //            }
            //        case "Equals":
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount == (pageLine.AmountInLocalCurrency))
            //                );
            //                break;
            //            }
            //        default:
            //            {
            //                iQueryable = iQueryable.Where(pageLine =>
            //                (amount == (pageLine.AmountInLocalCurrency))
            //                ||
            //                (amount2 == (pageLine.AmountInLocalCurrency))
            //                );
            //                break;
            //            }
            //    }

            //}
            //QueryFilterItem LineActionCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LineActionCode").FirstOrDefault();
            //if (LineActionCodeFilter != null)
            //{
            //    if (LineActionCodeFilter.FieldValue.ToString() == "1")
            //    {
            //        iQueryable = iQueryable.Where(x => x.LineActionCode == LineActionCodeFilter.FieldValue.ToString());
            //    }
            //    else
            //    {
            //        iQueryable = iQueryable.Where(x => x.LineActionCode == LineActionCodeFilter.FieldValue.ToString() || x.LineActionCode == LineActionCodeFilter.FieldValue2.ToString());

            //    }
            //}
            //QueryFilterItem IsExternalEntityFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsExternalEntity").FirstOrDefault();
            //if (IsExternalEntityFilter != null)
            //{
            //    bool value = IsExternalEntityFilter.FieldValue?.ToString() == "1" ? true : false;
            //    iQueryable = iQueryable.Where(x => x.IsExternalEntity == value);

            //}
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

