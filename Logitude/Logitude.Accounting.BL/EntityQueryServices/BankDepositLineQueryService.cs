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
   public partial class BankDepositLineQueryService
    {
        public List<BankDepositLinePM> GetDepositLinePMsByDepositIds(List<string> depositIds, int tenant)
        {
            List<BankDepositLine> depositLines = (from a in context.BankDepositLines
                                                  where depositIds.Contains(a.DepositId) && a.Tenant == tenant
                                                  select a).ToList();
            return depositLines.Select(rec => this.GetEntityPM(rec)).ToList();
        

        }

        public List<BankDepositLinePM> GetLinesJoinedWithCheques(List<string> depositIds, int tenant)
        {
            bool showLocals = LoggedContactResolver.GetLoggedContactShowLocal(tenant);

            List<BankDepositLinePM> depositLines = (from dpLine in context.BankDepositLines 
                                                  join  arpChequeLine in context.ARPaymentCheques
                                                  on    dpLine.ARPaymentChequeId equals arpChequeLine.Id
                                                  where depositIds.Contains(dpLine.DepositId) && dpLine.Tenant == tenant
                                                  select new BankDepositLinePM()
                                                  {
                                                      // cheque fields
                                                      Bank = arpChequeLine.BankId,
                                                      ChequeNumber = arpChequeLine.ChequeNumber,
                                                      ARPaymentNumber = arpChequeLine.Payment.PaymentNo,
                                                      DueDate = arpChequeLine.ValueDate,
                                                      LocalAmount = arpChequeLine.LocalAmount,
                                                      Currency = arpChequeLine.Currency.Code,
                                                      ForeignAmount = arpChequeLine.ForeignAmount,
                                                      AccountNumber = arpChequeLine.BankAccount,
                                                      Branch = arpChequeLine.BankBranch,
                                                      ARPaymentId = arpChequeLine.PaymentId,
                                                      ARPaymentChequeId = arpChequeLine.Id,
                                                      ChequeStatusCode = arpChequeLine.ARPaymentChequeStatus.Code,
                                                      ChequeStatusName = arpChequeLine.ARPaymentChequeStatus.EnglishName,
                                                      SearchFields = arpChequeLine.ChequeNumber,
                                                      DepositId = dpLine.DepositId,

                                                      // line mapping
                                                      Line = dpLine.Line,
                                                      Notes = dpLine.Notes,
                                                      IsOutOfDeposit = dpLine.IsOutOfDeposit,
                                                      Tenant = dpLine.Tenant,
                                                      OutOfDepositeDate = dpLine.OutOfDepositeDate,
                                                      

                                                  }).ToList();

            return depositLines;


        }

    }
}
