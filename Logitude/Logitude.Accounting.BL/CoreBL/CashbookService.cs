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
        public string RecalculateCashbookTotals(int tenant)
        {
            int updatedCount = 0;
            CashBookQueryService query = new CashBookQueryService(tenant);
            CashBookRepository repo = new CashBookRepository(tenant);

            CashBookLineRepository lineRepo = new CashBookLineRepository(AccountingContext.GetContext(tenant));
            List<CashBookPM> cashbooksList = query.GetAll(tenant);
            cashbooksList = cashbooksList.Where(d => d.CashBookTypeCode == "2").ToList(); // 2- Cheques

            try
            {
                foreach (CashBookPM cashbook in cashbooksList)
                {
                    List<CashBookLinePM> cashbookLines = cashbook.CashBookLines;
                    List<CashBookLinePM> filteredCashbookLines = cashbookLines.Where(d => d.ARPChequeStatusCode != "5").ToList(); //5- Returned to Customer
                    filteredCashbookLines = filteredCashbookLines.Where(d => d.IsDeposited == false).ToList(); //5- Returned to Customer
                    decimal total = 0;
                    total = filteredCashbookLines.Sum(d => d.ForeignAmount);

                    if(total != cashbook.TotalAmount)
                    {
                        updatedCount++;
                        CashBook cshbk = repo.GetSingle(cashbook.Id, tenant);
                        cshbk.TotalAmount = total;
                        repo.Update(cshbk);
                        repo.SubmitChanges();
                    }

                }

                return updatedCount.ToString();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message, ex);
            }





        }
    }
}
