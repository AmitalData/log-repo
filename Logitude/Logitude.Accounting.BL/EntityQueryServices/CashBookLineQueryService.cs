using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class CashBookLineQueryService
    {
        public List<CashBookLinePM> GetLinesByChequesIds(List<string> chequesIds, string cashbookId, int tenant)
        {
            List<CashBookLinePM> lines = (from cbLine in context.CashBookLines
                                          join cheque in context.ARPaymentCheques on cbLine.ARPChequeId equals cheque.Id
                                          where
                                              chequesIds.Contains(cbLine.ARPChequeId)
                                              && cbLine.CashBookId == cashbookId
                                              && cbLine.Tenant == tenant
                                          select new CashBookLinePM()
                                          {
                                              CashBookId = cbLine.CashBookId,
                                              Tenant = cbLine.Tenant,
                                              ARPChequeId = cbLine.ARPChequeId,
                                              ChequeNumber = cheque != null ? cheque.ChequeNumber : null,
                                              IsDeposited = cbLine.IsDeposited,
                                              Currency = cheque.Currency.Code,
                                              DueDate = cheque.ValueDate,
                                              LocalAmount = cheque.LocalAmount,
                                              ForeignAmount = cheque.ForeignAmount,
                                              Bank = cheque.BankAccount,
                                              Branch = cheque.BankBranch,
                                              AccountNumber = cheque.BankId,
                                              ARPaymentNumber = cheque.Payment.PaymentNo,
                                              ARPaymentId = cheque.PaymentId,
                                              ARPChequeStatusCode = cheque.StatusCode,
                                              ARPChequeStatusName = cheque.ARPaymentChequeStatus.LocalName,
                                              SearchFields = cbLine.SearchFields,

                                          }).ToList();



            return lines;
        

        }

    }
}
