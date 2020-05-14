using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SuppInvoiceItemsAbachStatementQueryService
    {

        public List<SuppInvoiceItemsAbachStatementPM> GetSupplierInvoiceItemsPricesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SuppInvoiceItemsAbachStatement> supplierInvoiceItemsPrices = repository.GetSuppInvoiceItemsAbachStatementsForDeclarationId(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SuppInvoiceItemsAbachStatementPM> supplierInvoiceItemsPricePMs = (from a in supplierInvoiceItemsPrices
                                                                                   select new SuppInvoiceItemsAbachStatementPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          StatementInd = a.StatementInd,
                                                                                          StatementType = a.StatementType,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,

                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemsPricePMs;

        }

   
    }
}
