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

    public partial class SupplierInvoiceListQueryService
    {
	    private IQueryable<SupplierInvoiceList> GetIqueryableList(IQueryable<SupplierInvoice> iQueryable)
        {
            IQueryable<SupplierInvoiceList> query = (from a in iQueryable.Include("PreferenceDocumentType").Include("IssueCountry").Include("Vendor")
                                                     select new SupplierInvoiceList()
                                                     {
                                                         AccountTypeCode = a.AccountTypeCode,
                                                         ActualPayedAmount = a.ActualPayedAmount,
                                                         ActualPayedCurrencyTypeCode = a.ActualPayedCurrencyTypeCode,
                                                         DeclarationId = a.DeclarationId,
                                                         ExchangeRate = a.ExchangeRate,
                                                         IncotermCode = a.IncotermCode,
                                                         InvoiceAmount = a.InvoiceAmount,
                                                         InvoiceCounterKey = a.InvoiceCounterKey,
                                                         InvoiceCurrencyTypeCode = a.InvoiceCurrencyTypeCode,
                                                         InvoiceNumber = a.InvoiceNumber,
                                                         IsPreference = a.IsPreference,
                                                         IssueCountryCode = a.IssueCountryCode,
                                                         IssueDate = a.IssueDate,
                                                         PaymentTermsCode = a.PaymentTermsCode,
                                                         PaymentTypeCode = a.PaymentTypeCode,
                                                         PreferenceDocumentTypeCode = a.PreferenceDocumentTypeCode,
                                                         SequenceNumeric = a.SequenceNumeric,
                                                         Tenant = a.Tenant,
                                                         TotalFreightInFreightCurrency = a.TotalFreightInFreightCurrency,
                                                         TotalFreightInNIS = a.TotalFreightInNIS,
                                                        InsruanceCurrencyTypeCode = a.InsruanceCurrencyTypeCode,
                                                        InsuranceAmount = a.InsuranceAmount,
                                                         VendorId = a.VendorId,
                                                         VendorName = a.Vendor != null? a.Vendor.VendorName :null,
                                                         IssueCountryName = a.IssueCountry != null? a.IssueCountry.LocalName : null,
                                                         PreferenceDocumentTypeName = a.TradeAgreement.LocalName,
                                                         InsruancePercentage = a.InsruancePercentage,
                                                         ChangeInSupplierInvoice = a.ChangeInSupplierInvoice,
                                                     });
            return query;
		}

        private IQueryable<SupplierInvoice> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoice> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	