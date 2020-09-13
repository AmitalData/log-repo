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

        public List<SuppInvoiceItemsAbachStatementPM> GetSuppInvoiceItemsAbachStatementsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SuppInvoiceItemsAbachStatement> supplierInvoiceItemsPrices = repository.GetSuppInvoiceItemsAbachStatementsForDeclarationId(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SuppInvoiceItemsAbachStatementPM> supplierInvoiceItemsPricePMs = (from a in supplierInvoiceItemsPrices
                                                                                   select new SuppInvoiceItemsAbachStatementPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          IsStatementInd = a.IsStatementInd,
                                                                                          StatementTypeCode = a.StatementTypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          SequenceNumeric=a.SequenceNumeric,
                                                                                          Tenant = a.Tenant,
                                                                                          StatementTypeName = a.NbcDeclarationType != null ? a.NbcDeclarationType.LocalName : null,

                                                                                   }).ToList();


            return supplierInvoiceItemsPricePMs;

        }

   
    }
}
