
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using System.Linq.Expressions;
using CHAMP17;
using NPOI.SS.Formula.Functions;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Contexts;
using Logitude.Customs.Data;
using System.Data.Entity;
using NPOI.Util;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting;

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
            BuildDataProvider();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportDeclarationDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, dataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void BuildDataProvider()
        {
            dataProvider = new ExportDeclarationDataProvider();
			SetReportHeaderFields();

			SetExportDeclaration(tenant, reportQueryOperations);

        }
		private void SetReportHeaderFields()
		{
			QueryOperationsFilterValueGetter valueGetter = new QueryOperationsFilterValueGetter(this.reportQueryOperations);
	
			dataProvider.CreateDateFrom = valueGetter.GetFilterValue<DateTime?>("CreateDate");
			dataProvider.CreateDateTo = valueGetter.GetFilterValue2<DateTime?>("CreateDate");
			dataProvider.TransportModeId = valueGetter.GetFilterValue<string>("TransportModeId");
			dataProvider.DeclarationStatusTypeCode = valueGetter.GetFilterValue<string>("DeclarationStatusTypeCode");
			dataProvider.DeclarationStatusTypeName = valueGetter.GetFilterValue2<string>("DeclarationStatusTypeCode");
			dataProvider.DeclarationTypeCode = valueGetter.GetFilterValue<string>("DeclarationTypeCode");
			dataProvider.DeclarationTypeName = valueGetter.GetFilterValue2<string>("DeclarationTypeCode");
			dataProvider.ReferentUserId = valueGetter.GetFilterValue<string>("ReferentUserId");
			dataProvider.ReferentUserName = valueGetter.GetFilterValue2<string>("ReferentUserId");
			dataProvider.DestinationCountryCode = valueGetter.GetFilterValue<string>("DestinationCountryCode");
			dataProvider.DestinationCountryName = valueGetter.GetFilterValue2<string>("DestinationCountryCode");
			dataProvider.Customer = valueGetter.GetFilterValue<string>("Customer");
			dataProvider.CustomerName = valueGetter.GetFilterValue2<string>("Customer");
			dataProvider.IsShowInvoices = Convert.ToBoolean(valueGetter.GetFilterValue<string>("ShowInvoices"));
			dataProvider.IsShowConsignments = Convert.ToBoolean(valueGetter.GetFilterValue<string>("ShowConsignments"));

		}


		public void SetExportDeclaration(int tenant, QueryOperations queryOperations)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            bool showInvoices = false;
            QueryFilterItem ShowInvoicesFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowInvoices").FirstOrDefault();
            if (ShowInvoicesFilter != null && ShowInvoicesFilter.FieldValue.ToString() == "True")
            {
                showInvoices = true;
            }

            var declarations = (from a in context.Declarations
                                                   .Include(a => a.TransportMode)
                                                   .Include(a => a.DeclarationType)
                                                   .Include(a => a.GovernmentProcedureCurrent)
                                                   .Include(a => a.CustomsCountry)
                                                   .Include(a => a.DeclarationStatusType)
                                join de in context.DeclarationExportRecipients
                                .Select(x => new { x.DeclarationId, x.RecipientName })
                                on a.Id equals de.DeclarationId into deJoin
            from der in deJoin.DefaultIfEmpty().Take(1)

                                join s in context.SupplierInvoices.Include(a => a.CurrencyType) 
                                on a.Id equals showInvoices? s.DeclarationId : default(string) into sJoin
            from si in sJoin.DefaultIfEmpty()

                                join item in context.SupplierInvoiceItems
                                .Include(a => a.InvoiceMeasurmentUnit).Include(a => a.OriginCountry).Include(a => a.TransactionNatureType)
                                .Select(x => new { x.DeclarationId, x.CounterKey, x.ItemCode, x.ClassificationCode, x.PackageQuantity, x.InvoiceQuantityType, x.ItemPrice, OriginCountryName = x.OriginCountry.LocalName, TransactionNatureName = x.TransactionNatureType.LocalName, x.LineNumber })
                                on new { DeclarationId = a.Id, CounterKey = si.InvoiceCounterKey } equals new { DeclarationId = item.DeclarationId, CounterKey = item.CounterKey } into itemJoin
                                from sItem in itemJoin.DefaultIfEmpty()

                                join c in context.ExportDeclarationClosingDatas.Include(a => a.FinalCargoType)
                                .Select(x => new { x.DeclarationId, x.FinalCargoTypeCode, x.FinalManifestNumber, x.FinalSecondCargoId, x.FinalThirdCargoId, x.FinalCargoType.LocalName })
                                on a.Id equals c.DeclarationId into cJoin
            from closing in cJoin.DefaultIfEmpty()

                                where a.Tenant == tenant && a.Direction == "E"
                                select new
                                {
                                    a.Id,
                                    a.CreateDateTime,
                                    a.TaxationDateTime,
                                    a.ExportFile,
                                    a.TransportModeId,
                                    TransportModeName = a.TransportMode != null ? a.TransportMode.LocalName : null,
                                    a.CustomFileNo,
                                    a.DeclarationNumber,
                                    DeclarationTypeName = a.DeclarationType != null ? a.DeclarationType.LocalName : null,
                                    ProcedureCurrentName = a.GovernmentProcedureCurrent != null ? a.GovernmentProcedureCurrent.LocalName : null,
                                    ExporterImporterCode = a.ImporterCode,
									ExporterImporterName = a.Importer != null ? a.Importer.FullName : null,
									RecipientName = der != null && !string.IsNullOrEmpty(der.RecipientName) ? der.RecipientName : null,
                                    DestinationCountryName = a.CustomsCountry != null ? a.CustomsCountry.LocalName : null,
                                    a.DestinationCountryCode,
                                    a.DeclarationStatusTypeCode,
                                    a.DeclarationTypeCode,
                                    DeclarationStatusTypeName = a.DeclarationStatusType != null ? a.DeclarationStatusType.LocalName : null,
                                    a.ReferentUserId,
                                    a.CustomerId,
                                    FinalCargoTypeName = closing != null ? closing.LocalName : null,
                                    FinalManifestNumber = closing != null && !string.IsNullOrEmpty(closing.FinalManifestNumber) ? closing.FinalManifestNumber : null,
                                    FinalSecondCargoId = closing != null && !string.IsNullOrEmpty(closing.FinalSecondCargoId) ? closing.FinalSecondCargoId : null,
                                    FinalThirdCargoId = closing != null && !string.IsNullOrEmpty(closing.FinalThirdCargoId) ? closing.FinalThirdCargoId : null,

                                    //supplierInvoice
                                    si.InvoiceNumber,
                                    si.IssueDate,
                                    si.IncotermCode,
                                    si.InvoiceAmount,
                                    InvoiceCurrencyTypeName = si != null  && si.CurrencyType != null ? si.CurrencyType.LocalName: si.InvoiceCurrencyTypeCode,
                                    InvoiceCounterKey = si != null ? si.InvoiceCounterKey : 0,

									//supplierInvoiceItem
									sItem.ItemCode,
                                    sItem.ClassificationCode,
                                    itemPackageQuantity = sItem.PackageQuantity,
                                    sItem.InvoiceQuantityType,
                                    sItem.ItemPrice,
                                    sItem.OriginCountryName,
                                    sItem.TransactionNatureName,
                                    LineNumber = sItem != null ? sItem.LineNumber : 0,
                                });


            #region  ApplyCustomFilters
            bool showConsignments = false;

            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                declarations = declarations.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }

            QueryFilterItem TransportModeIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            if (TransportModeIdFilter != null)
            {
                declarations = declarations.Where(x => x.TransportModeId == TransportModeIdFilter.FieldValue.ToString());
            }

            QueryFilterItem DeclarationStatusFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DeclarationStatusTypeCode").FirstOrDefault();
            if (DeclarationStatusFilter != null)
            {
                declarations = declarations.Where(x => x.DeclarationStatusTypeCode == DeclarationStatusFilter.FieldValue.ToString());
            }

            QueryFilterItem DeclarationTypeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DeclarationTypeCode").FirstOrDefault();
            if (DeclarationTypeFilter != null)
            {
                declarations = declarations.Where(x => x.DeclarationTypeCode == DeclarationTypeFilter.FieldValue.ToString());
            }

            QueryFilterItem ReferentUserIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ReferentUserId").FirstOrDefault();
            if (ReferentUserIdFilter != null)
            {
                declarations = declarations.Where(x => x.ReferentUserId == ReferentUserIdFilter.FieldValue.ToString());
            }

            QueryFilterItem DestinationCountryCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DestinationCountryCode").FirstOrDefault();
            if (DestinationCountryCodeFilter != null)
            {
                declarations = declarations.Where(x => x.DestinationCountryCode == DestinationCountryCodeFilter.FieldValue.ToString());
            }

            QueryFilterItem CustomerFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Customer").FirstOrDefault();
            if (CustomerFilter != null)
            {
                declarations = declarations.Where(x => x.CustomerId == CustomerFilter.FieldValue.ToString());
            }

            QueryFilterItem ShowConsignmentsFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowConsignments").FirstOrDefault();
            if (ShowConsignmentsFilter != null && ShowConsignmentsFilter.FieldValue.ToString() == "True")
            {
                showConsignments = true;
            }
            #endregion

            #region map to data provider

            var query = declarations

            .Select(g => new ExportDeclaration()
            {
                DeclarationId = g.Id,
                //g.Key.CreateDateTime,
                TaxationDateTime = g.TaxationDateTime,
                ExportFile = g.ExportFile,
                //g.Key.TransportModeId,
                TransportModeName = g.TransportModeName,
                CustomFileNo = g.CustomFileNo,
                DeclarationNumber = g.DeclarationNumber,
                DeclarationTypeName = g.DeclarationTypeName,
                ProcedureCurrentName = g.ProcedureCurrentName,
                ExporterImporterCode = g.ExporterImporterCode,
                RecipientName = g.RecipientName,
                DestinationCountryName = g.DestinationCountryName,

                //g.Key.DestinationCountryCode,
                // g.Key.DeclarationStatusTypeCode,
                // g.Key.DeclarationTypeCode,
                DeclarationStatusTypeName = g.DeclarationStatusTypeName,
                //g.Key.ReferentUserId,
                // g.Key.CustomerId,
                FinalCargoTypeName = g.FinalCargoTypeName,
                FinalManifestNumber = g.FinalManifestNumber,
                FinalSecondCargoId = g.FinalSecondCargoId,
                FinalThirdCargoId = g.FinalThirdCargoId,

                SupplierInvoiceAmount = g.InvoiceAmount,
                SupplierInvoiceIncotermCode = g.IncotermCode,
                SupplierInvoiceIssueDate = g.IssueDate,
                SupplierInvoiceNumber = g.InvoiceNumber,
                SupplierInvoiceCurrencyTypeName = g.InvoiceCurrencyTypeName,

                SupplierInvoiceItemCode = g.ItemCode,
                SupplierInvoiceItemClassificationCode = g.ClassificationCode,
                SupplierInvoiceItemInvoiceQuantityType = g.InvoiceQuantityType,
                SupplierInvoiceItemPrice = g.ItemPrice,
                SupplierInvoiceItemOriginCountryName = g.OriginCountryName,
                SupplierInvoiceItemPackageQuantity = g  .itemPackageQuantity,
                SupplierInvoiceItemTransactionNatureName = g.TransactionNatureName,
                SupplierInvoiceItemLineNumber = g.LineNumber
            });

            var exportDeclarations = query.ToList();

            if (showConsignments)
            {
                List<string> DeclarationIds = exportDeclarations.Select(x => x.DeclarationId).Distinct().ToList();

                // get the consignments for the retrieved declarations
                var consignments = (from consignment in context.Consignments
                                    .Include(a => a.CargoType)
                                    .Include(a => a.FinalDestinationPort)
                                    .Select(x => new { x.DeclarationId, x.ConsignmentNumber, x.ConsignmentType, CargoTypeName = x.CargoType.LocalName, x.ManifestNumber, x.SecondCargoID, x.ThirdCargoID, x.CargoDescription, FinalDestinationPort = x.FinalDestinationPort.LocalName })

                                    join cp in context.ConsignmentPackages on new { DeclarationId = consignment.DeclarationId, ConsignmentNumber = consignment.ConsignmentNumber } equals new { DeclarationId = cp.DeclarationId, ConsignmentNumber = cp.ConsignmentNumber } into cpJoin
                                    from cPackage in cpJoin.DefaultIfEmpty().Take(1)

                                    where DeclarationIds.Contains(consignment.DeclarationId) && consignment.ConsignmentNumber != null
                                    select new
                                    {
                                        DeclarationId = consignment.DeclarationId,
                                        ConsignmentType = consignment != null ? consignment.ConsignmentType : null,
                                        ConsignmentNumber = consignment != null ? consignment.ConsignmentNumber : null,
                                        CargoTypeName = consignment != null? consignment.CargoTypeName: null,
                                        ManifestNumber = consignment != null ? consignment.ManifestNumber : null,
                                        SecondCargoID = consignment != null ? consignment.SecondCargoID : null,
                                        ThirdCargoID = consignment != null ? consignment.ThirdCargoID : null,
                                        CargoDescription = consignment != null ? consignment.CargoDescription : null,
                                        FinalDestinationPortName = consignment != null? consignment.FinalDestinationPort : null,
                                        cPackage.PackageQuantity,
                                        cPackage.GrossMassMeasure,
                                    }).ToList();

                foreach (var declarationId in DeclarationIds)
                {
                    var declarationRows = exportDeclarations.Where(x => x.DeclarationId == declarationId);
                    var consignmentRows = consignments.Where(x => x.DeclarationId == declarationId);

                    int declarationRowsCount = declarationRows.Count();
                    int consignmentRowsCount = consignmentRows.Count();

                    // according to spec, all consignment rows must be shown on declaration rows
                    // if there is more consignment rows than declaration rows, we must add declaration rows
                    if (declarationRowsCount < consignmentRowsCount)
                    {
                        // duplicate the last declaration row as many times it is needed and insert them after all the declaration rows
                        int rowCountToAdd = consignmentRowsCount - declarationRowsCount;

                        ExportDeclaration lastDeclaration = exportDeclarations.Where(x => x.DeclarationId == declarationId).LastOrDefault();
                        int lastDeclarationIndex = exportDeclarations.IndexOf(lastDeclaration);

                        for (var i = 1; i < rowCountToAdd + 1; i++)

                        {
                            exportDeclarations.Insert(lastDeclarationIndex + i, lastDeclaration.Copy());
                        }
                    }

                    // add the consignment data to the declaration rows
                    for (var i = 0; i < consignmentRowsCount; i++)

                    {
                        var consignment = consignmentRows.ElementAtOrDefault(i);

                        ExportDeclaration dec = exportDeclarations.Where(x => x.DeclarationId == declarationId).ElementAtOrDefault(i);

                        dec.ConsignmentCargoDescription = consignment.CargoDescription;
                        dec.ConsignmentCargoTypeName = consignment.CargoTypeName;
                        dec.ConsignmentFinalDestinationPortName = consignment.FinalDestinationPortName;
                        dec.ConsignmentGrossMassMeasure = consignment.GrossMassMeasure;
                        dec.ConsignmentManifestNumber = consignment.ManifestNumber;
                        dec.ConsignmentNumber = consignment.ConsignmentNumber;
                        dec.ConsignmentPackageQuantity = consignment.PackageQuantity;
                        dec.ConsignmentSecondCargoID = consignment.SecondCargoID;
                        dec.ConsignmentThirdCargoID = consignment.ThirdCargoID;
                        dec.ConsignmentType = consignment.ConsignmentType;
                    }
                }
            }

            dataProvider.ExportDeclaration = exportDeclarations;
            #endregion

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

