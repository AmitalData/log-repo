using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Transactions;
using WebFreight.Web.Helpers;

namespace Logitude.Update.Helper
{
    public static class BluesnapTransactionUppdateOld
    {
        public static void Run()
        {
            IQueryable<BluesnapTransaction> bluesnapTransactions;
            IGlobalContext globalContext = GlobalContext.GetContext();
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                bluesnapTransactions = (from a in globalContext.BluesnapTransactions select a);
                scope.Complete();
            }

            foreach (BluesnapTransaction transaction in bluesnapTransactions)
            {
                var queryParameters = BluesnapHelper.DeserializeDocumentBody(transaction.DocumentId, transaction.Tenant);
                if (queryParameters != null)
                {
                    if (queryParameters.ContainsKey("contractId"))
                    {
                        transaction.ContractNumber = (queryParameters["contractId"]).ToString();
                    }

                    if (queryParameters.ContainsKey("invoiceAmountUSD"))
                    {
                        if (queryParameters["invoiceAmountUSD"] != null)
                        {
                            double result = 0;
                            Double.TryParse(queryParameters["invoiceAmountUSD"], out result);
                            transaction.InvoiceAmountInUSD = result;
                        }
                    }

                    if (queryParameters.ContainsKey("taxAmountUSD"))
                    {
                        if (queryParameters["taxAmountUSD"] != null)
                        {
                            double result = 0;
                            Double.TryParse(queryParameters["taxAmountUSD"], out result);
                            transaction.TaxAmountInUSD = result;
                        }
                    }

                }
            }
            globalContext.SaveChanges();
        }
   
    }
}
