
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


            SetExportDeclaration(tenant, reportQueryOperations);

        }
        


        public void SetExportDeclaration(int tenant, QueryOperations queryOperations)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var declarations = (from a in context.Declarations
                                                   .Include(a => a.CustomsTransportMode)
                                                   .Include(a => a.DeclarationType)
                                                   .Include(a => a.GovernmentProcedureCurrent)
                                                   .Include(a => a.CustomsCountry)
                                                   .Include(a => a.DeclarationStatusType)
                                join de in context.DeclarationExportRecipients
                                .Select(x => new { x.DeclarationId, x.RecipientName })
                                on a.Id equals de.DeclarationId into deJoin
            from der in deJoin.DefaultIfEmpty().Take(1)

                                join s in context.SupplierInvoices on a.Id equals s.DeclarationId into sJoin
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

                                join con in context.Consignments
                                .Include(a => a.CargoType)
                                .Include(a => a.FinalDestinationPort)
                                .Select(x => new { x.DeclarationId, x.ConsignmentNumber, x.ConsignmentType, CargoTypeName = x.CargoType.LocalName, x.ManifestNumber, x.SecondCargoID, x.ThirdCargoID, x.CargoDescription, FinalDestinationPort = x.FinalDestinationPort.LocalName })
                                on a.Id equals con.DeclarationId into conJoin
                                from consignment in conJoin.DefaultIfEmpty()

                                join cp in context.ConsignmentPackages on new { DeclarationId = a.Id, ConsignmentNumber = consignment.ConsignmentNumber } equals new { DeclarationId = cp.DeclarationId, ConsignmentNumber = cp.ConsignmentNumber } into cpJoin
                                from cPackage in cpJoin.DefaultIfEmpty().Take(1)

                                where a.Tenant == tenant && a.Direction == "E"
                                select new
                                {
                                    a.Id,
                                    a.CreateDateTime,
                                    a.TaxationDateTime,
                                    a.ExportFile,
                                    a.TransportModeId,
                                    TransportModeName = a.CustomsTransportMode != null ? a.CustomsTransportMode.LocalName : null,
                                    a.CustomFileNo,
                                    a.DeclarationNumber,
                                    DeclarationTypeName = a.DeclarationType != null ? a.DeclarationType.LocalName : null,
                                    ProcedureCurrentName = a.GovernmentProcedureCurrent != null ? a.GovernmentProcedureCurrent.LocalName : null,
                                    ExporterImporterCode = a.ImporterCode,
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
                                    //consignment
                                    ConsignmentType = consignment != null ? consignment.ConsignmentType : null,
                                    ConsignmentNumber = consignment != null ? consignment.ConsignmentNumber : null,
                                    CargoTypeName = consignment != null ? consignment.CargoTypeName : null,
                                    ManifestNumber = consignment != null ? consignment.ManifestNumber : null,
                                    SecondCargoID = consignment != null ? consignment.SecondCargoID : null,
                                    ThirdCargoID = consignment != null ? consignment.ThirdCargoID : null,
                                    CargoDescription = consignment != null ? consignment.CargoDescription : null,
                                    FinalDestinationPortName = consignment != null ? consignment.FinalDestinationPort : null,
                                    cPackage.PackageQuantity,
                                    cPackage.GrossMassMeasure,
                                    //supplierInvoice
                                    si.InvoiceNumber,
                                    si.IssueDate,
                                    si.IncotermCode,
                                    si.InvoiceAmount,
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

            bool showInvoices = false;
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

            QueryFilterItem ShowInvoicesFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowInvoices").FirstOrDefault();
            if (ShowInvoicesFilter != null && ShowInvoicesFilter.FieldValue.ToString() == "True")
            {
                showInvoices = true;
            }
            QueryFilterItem ShowConsignmentsFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowConsignments").FirstOrDefault();
            if (ShowConsignmentsFilter != null && ShowConsignmentsFilter.FieldValue.ToString() == "True")
            {
                showConsignments = true;
            }
            #endregion

            #region map to data provider


            dataProvider.ExportDeclaration = declarations.GroupBy(d => new
            {
                d.Id,
                //d.CreateDateTime,
                d.TaxationDateTime,
                d.ExportFile,
                //d.TransportModeId,
                d.TransportModeName,
                d.CustomFileNo,
                d.DeclarationNumber,
                d.DeclarationTypeName,
                d.ProcedureCurrentName,
                d.ExporterImporterCode,
                d.RecipientName,
                d.DestinationCountryName,
                //d.DestinationCountryCode,
                //d.DeclarationStatusTypeCode,
                //d.DeclarationTypeCode,
                d.DeclarationStatusTypeName,
                //d.ReferentUserId,
                //d.CustomerId,
                d.FinalCargoTypeName,
                d.FinalManifestNumber,
                d.FinalSecondCargoId,
                d.FinalThirdCargoId,
            })
            .Select(g => new ExportDeclaration()
            {
                DeclarationId = g.Key.Id,
                //g.Key.CreateDateTime,
                TaxationDateTime = g.Key.TaxationDateTime,
                ExportFile = g.Key.ExportFile,
                //g.Key.TransportModeId,
                TransportModeName = g.Key.TransportModeName,
                CustomFileNo = g.Key.CustomFileNo,
                DeclarationNumber = g.Key.DeclarationNumber,
                DeclarationTypeName = g.Key.DeclarationTypeName,
                ProcedureCurrentName = g.Key.ProcedureCurrentName,
                ExporterImporterCode = g.Key.ExporterImporterCode,
                RecipientName = g.Key.RecipientName,
                DestinationCountryName = g.Key.DestinationCountryName,
                //g.Key.DestinationCountryCode,
                // g.Key.DeclarationStatusTypeCode,
                // g.Key.DeclarationTypeCode,
                DeclarationStatusTypeName = g.Key.DeclarationStatusTypeName,
                //g.Key.ReferentUserId,
                // g.Key.CustomerId,
                FinalCargoTypeName = g.Key.FinalCargoTypeName,
                FinalManifestNumber = g.Key.FinalManifestNumber,
                FinalSecondCargoId = g.Key.FinalSecondCargoId,
                FinalThirdCargoId = g.Key.FinalThirdCargoId,
                Consignment = g.GroupBy(d => new { d.ConsignmentNumber, d.ConsignmentType, d.CargoTypeName, d.ManifestNumber, d.SecondCargoID, d.ThirdCargoID, d.CargoDescription, d.FinalDestinationPortName, d.PackageQuantity, d.GrossMassMeasure })
                .Select(con => new Consignment()
                {
                    ConsignmentNumber = con.Key.ConsignmentNumber,
                    ConsignmentType = con.Key.ConsignmentType,
                    CargoTypeName = con.Key.CargoTypeName,
                    ManifestNumber = con.Key.ManifestNumber,
                    SecondCargoID = con.Key.SecondCargoID,
                    ThirdCargoID = con.Key.ThirdCargoID,
                    CargoDescription = con.Key.CargoDescription,
                    FinalDestinationPortName = con.Key.FinalDestinationPortName,
                    PackageQuantity = con.Key.PackageQuantity,
                    GrossMassMeasure = con.Key.GrossMassMeasure,
                })
                .Where(c => c.ConsignmentNumber != null && showConsignments)
                .ToList(),
                SupplierInvoices = g.GroupBy(d => new { d.InvoiceNumber, d.IssueDate, d.IncotermCode, d.InvoiceAmount, d.InvoiceCounterKey })
                            .Select(groupedInvoice => new SupplierInvoices()
                            {
                                InvoiceNumber = groupedInvoice.Key.InvoiceNumber,
                                IssueDate = groupedInvoice.Key.IssueDate,
                                IncotermCode =groupedInvoice.Key.IncotermCode,
                                InvoiceAmount = groupedInvoice.Key.InvoiceAmount,
                                InvoiceCounterKey = groupedInvoice.Key.InvoiceCounterKey,
                                InvoiceItems = groupedInvoice
                                                .Where(item => item.LineNumber != 0)
                                                .Select(item => new InvoiceItems()
                                                {
                                                    ItemCode = item.ItemCode,
                                                    ClassificationCode = item.ClassificationCode,
                                                    InvoiceQuantityType = item.InvoiceQuantityType,
                                                    ItemPrice = item.ItemPrice,
                                                    OriginCountryName = item.OriginCountryName,
                                                    PackageQuantity = item.PackageQuantity,
                                                    TransactionNatureName = item.TransactionNatureName,
                                                    LineNumber = item.LineNumber
                                                }).Distinct().ToList()
                            }).Where(i => i.InvoiceCounterKey != 0 && showInvoices)
                            .ToList()
            }).ToList();
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

