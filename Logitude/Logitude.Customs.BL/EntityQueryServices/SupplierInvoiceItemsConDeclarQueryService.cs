using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsConDeclarQueryService
    {
        public List<SupplierInvoiceItemsConDeclarPM> GetSupplierInvoiceItemsConDeclarPMsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant,List<int> FilterLine=null)
        {
            List<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclars = repository.GetSupplierInvoiceItemsConDeclarsForSupplierInvoice(declarationId, invoiceCounterKey,tenant, FilterLine);
            List<SupplierInvoiceItemsConDeclarPM> SupplierInvoiceItemsConDeclarPms = (from a in SupplierInvoiceItemsConDeclars
                                                                                      select new SupplierInvoiceItemsConDeclarPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          DeclarationNumber = a.DeclarationNumber,
                                                                                          DeclarationTypeCode = a.DeclarationTypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          InvoiceNumber = a.InvoiceNumber,
                                                                                          ItemSequence = a.ItemSequence,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Quantity = a.Quantity,
                                                                                          Tenant = a.Tenant,
                                                                                          DeclarationTypeName=a.LeadDocumentType!=null?a.LeadDocumentType.LocalName:null,
                                                                                          QuantityTypeCode = a.QuantityTypeCode,
                                                                                          QuantityTypeName = a.MeasurmentUnit  != null? a.MeasurmentUnit.LocalName : null,
                                                                                      }).ToList();
            return SupplierInvoiceItemsConDeclarPms;
        }

        public List<SupplierInvoiceItemsConDeclarPM> GetSupplierInvoiceItemsConDeclarePMsForInvoiceItem(string declarationId, int invoiceCounterKey, int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclars = repository.GetMulti(new SupplierInvoiceItemKeys() { CounterKey=invoiceCounterKey, DeclarationId=declarationId, LineNumber=lineNumber });
            List<SupplierInvoiceItemsConDeclarPM> SupplierInvoiceItemsConDeclarPms = (from a in SupplierInvoiceItemsConDeclars
                                                                                      select new SupplierInvoiceItemsConDeclarPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          DeclarationNumber = a.DeclarationNumber,
                                                                                          DeclarationTypeCode = a.DeclarationTypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          InvoiceNumber = a.InvoiceNumber,
                                                                                          ItemSequence = a.ItemSequence,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Quantity = a.Quantity,
                                                                                          Tenant = a.Tenant,
                                                                                          DeclarationTypeName = a.LeadDocumentType != null ? a.LeadDocumentType.LocalName : null,
                                                                                          QuantityTypeCode = a.QuantityTypeCode,
                                                                                          QuantityTypeName = a.MeasurmentUnit != null ? a.MeasurmentUnit.LocalName : null,
                                                                                      }).ToList();
            return SupplierInvoiceItemsConDeclarPms;
        }

        public List<SupplierInvoiceItemsConDeclarPM> GetSupplierInvoiceItemsConDeclarPMsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey,List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclars = repository.GetSupplierInvoiceItemsConDeclarsForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsConDeclarPM> SupplierInvoiceItemsConDeclarPms = (from a in SupplierInvoiceItemsConDeclars
                                                                                      select new SupplierInvoiceItemsConDeclarPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          DeclarationNumber = a.DeclarationNumber,
                                                                                          DeclarationTypeCode = a.DeclarationTypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          InvoiceNumber = a.InvoiceNumber,
                                                                                          ItemSequence = a.ItemSequence,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Quantity = a.Quantity,
                                                                                          Tenant = a.Tenant,
                                                                                          DeclarationTypeName = a.LeadDocumentType != null ? a.LeadDocumentType.LocalName : null,
                                                                                          QuantityTypeCode = a.QuantityTypeCode,
                                                                                          QuantityTypeName = a.MeasurmentUnit != null ? a.MeasurmentUnit.LocalName : null,
                                                                                      }).ToList();
            return SupplierInvoiceItemsConDeclarPms;
        }
 
    }
}
