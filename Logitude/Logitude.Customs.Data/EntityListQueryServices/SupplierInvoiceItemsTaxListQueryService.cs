	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class SupplierInvoiceItemsTaxListQueryService
    {
	    private IQueryable<SupplierInvoiceItemsTaxList> GetIqueryableList(IQueryable<SupplierInvoiceItemsTax> iQueryable)
        {
            //ParagraphType

            //join prod in products on category.ID equals prod.CategoryID
            IQueryable<SupplierInvoiceItemsTaxList> query = (from tax in context.SupplierInvoiceItemsTaxes.Include("ParagraphType")
                                                             join invoiceItem in context.SupplierInvoiceItems on new {DeclarationId= tax.DeclarationId,CounterKey= tax.InvoiceCounterKey,LineNumber= tax.LineNumber } 
                                                             equals new {DeclarationId= invoiceItem.DeclarationId,CounterKey= invoiceItem.CounterKey,LineNumber= invoiceItem.LineNumber }
                                                             join invoice in context.SupplierInvoices on new { tax.DeclarationId, tax.InvoiceCounterKey } equals new { invoice.DeclarationId, invoice.InvoiceCounterKey } 
                                                             select new SupplierInvoiceItemsTaxList()
                                                                 {
                                                                     DeclarationId = tax.DeclarationId,
                                                                     InvoiceCounterKey = tax.InvoiceCounterKey,
                                                                     Tenant = tax.Tenant,
                                                                     AlternateDefinedPerUnitMeasure = tax.AlternateDefinedPerUnitMeasure,
                                                                     AlternateDefinedPerUnitQuant = tax.AlternateDefinedPerUnitQuant,
                                                                     AlternateMeasurementUnitCode = tax.AlternateMeasurementUnitCode,
                                                                     AlternateRate = tax.AlternateRate,
                                                                     DeferedTaxAmount = tax.DeferedTaxAmount,
                                                                     TaxAmount = tax.TaxAmount,
                                                                     TaxBaseAmount = tax.TaxBaseAmount,
                                                                     TradeAgreementTypeCode = tax.TradeAgreementTypeCode,
                                                                     DefinedPerUnitMeasure = tax.DefinedPerUnitMeasure,
                                                                     DefinedPerUnitQuantity = tax.DefinedPerUnitQuantity,
                                                                     LineNumber = tax.LineNumber,
                                                                     MeasurementUnitCode = tax.MeasurementUnitCode,
                                                                     TaxRate = tax.TaxRate,
                                                                     TaxTypeCode = tax.TaxTypeCode,
                                                                 TaxTypeName=tax.ParagraphType != null ? tax.ParagraphType.LocalName : tax.TaxTypeCode,
                                                                 TotalBtlCoverageNIS = tax.TotalBtlCoverageNIS,
                                                                     TradeLevyNumber = tax.TradeLevyNumber,
                                                                     InvoiceNumber=invoice.InvoiceNumber,
                                                                     ClassificationCode=invoiceItem.ClassificationCode,

                                                                 });
            return query;
		}

        private IQueryable<SupplierInvoiceItemsTax> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsTax> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	