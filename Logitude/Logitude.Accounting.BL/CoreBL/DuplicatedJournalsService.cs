using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL
{
    public class DuplicatedJournalsService
    {
        public string LoggingText { get; set; } = "";

        public DuplicatedJournalsService()
        {

        }

        public List<string> GetARPaymentsIdsHavingDuplicatedJournals(int tenant)
        {
            var context = AccountingContext.GetContext(tenant);

            IQueryable<string> result = from journal in context.Journals
                                        where journal.Tenant == tenant
                                             && journal.AccountingEntityCode == AccountingEntityValues.ARPayment
                                             && journal.IsVoided == false
                                        group journal by journal.AccountingEntityId into grouped
                                        where grouped.Count() > 1
                                        select grouped.Key;

            return result.ToList();
        }
    }
}
