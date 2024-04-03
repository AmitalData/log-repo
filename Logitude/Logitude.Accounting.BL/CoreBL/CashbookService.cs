using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CloseTables;
using static Simplog.Server.Infrastructure.DbContextBase;
using System.Transactions;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;

namespace Logitude.Accounting.BL.CoreBL
{
    public class CashbookService
    {
        List<string> allowedChequesStatuses = new List<string>() { ARPaymentChequeStatusValues.InCashbook, ARPaymentChequeStatusValues.ReturnedFromBank };
        public void RecalculateCashbookTotal(string cashbookId, int tenant)
        {
            CashBookPM cashbook = GetCashbook(cashbookId, tenant);
           
            decimal calculatedTotal = CalculateCashbookTotal(cashbook);
            if (calculatedTotal != cashbook.TotalAmount)
                UpdateCashbookPMTotal(cashbook, calculatedTotal);
        }
        public string RecalculateCashbooksTotals(int tenant)
        {
            int updatedCount = 0;
            List<CashBookPM> chequeCashbooks = GetChequeCashbooks(tenant);
            try
            {
                foreach (CashBookPM cashbook in chequeCashbooks)
                {
                    decimal calculatedTotal = CalculateCashbookTotal(cashbook);
                    if (calculatedTotal != cashbook.TotalAmount)
                    {
                        updatedCount++;
                        UpdateCashbookTotal(tenant, cashbook, calculatedTotal);
                    }
                }
                return updatedCount.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        private decimal CalculateCashbookTotal(CashBookPM cashbook)
        {
            List<CashBookLinePM> filteredCashbookLines = GetFilteredCashbookLines(cashbook);

            decimal calculatedTotal = filteredCashbookLines.Sum(d => d.ForeignAmount);
            return calculatedTotal;
        }

        private List<CashBookLinePM> GetFilteredCashbookLines(CashBookPM cashbook)
        {
            List<CashBookLinePM> cashbookLines = cashbook.CashBookLines;
            List<CashBookLinePM> filteredCashbookLines = cashbookLines.Where(d => allowedChequesStatuses.Contains(d.ARPChequeStatusCode)).ToList();
            filteredCashbookLines = filteredCashbookLines.Where(d => d.IsDeposited == false).ToList();
            return filteredCashbookLines;
        }
        
        private void SubmitCashbook(CashBookPM cashbook)
        {
            cashbook.ChangeSetOp = ChangeSetOperation.Update;
            cashbook.IsTotalUpdatedByCC = true; ;

            IAccountingContext context = AccountingContext.GetContext(cashbook.Tenant);
            var cashBookUpdateService = new CashBookUpdateService(context, new Dictionary<string, IContext>(), cashbook.Tenant);
            cashBookUpdateService.Update(cashbook, true);
        }
        private void UpdateCashbookPMTotal(CashBookPM cashbook, decimal total)
        {
            cashbook.IsTotalUpdatedByCC = true;
            cashbook.TotalAmount = total;

            SubmitCashbook(cashbook);
        }
        private static void UpdateCashbookTotal(int tenant, CashBookPM cashbook, decimal total)
        {
            CashBookRepository repo = new CashBookRepository(tenant);
            CashBook cshbk = repo.GetSingle(cashbook.Id, tenant);
            cshbk.TotalAmount = total;
            repo.Update(cshbk);
            repo.SubmitChanges();
        }

        private static List<CashBookPM> GetChequeCashbooks(int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            CashBookLineRepository lineRepo = new CashBookLineRepository(AccountingContext.GetContext(tenant));
            List<CashBookPM> cashbooksList = query.GetAll(tenant);
            cashbooksList = cashbooksList.Where(d => d.CashBookTypeCode == CashBookTypeValues.Cheques).ToList();
            return cashbooksList;
        }
        private CashBookPM GetCashbook(string cashbookId, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.GetSingle(cashbookId, true, false);
        }
    }
}
