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
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System.Windows.Forms;

namespace Logitude.Accounting.BL.CoreBL
{
    public class DuplicatedJournalsService
    {
        public string LoggingText { get; set; } = "";

        public DuplicatedJournalsService()
        {

        }

        public List<ARPaymentsId> GetARPaymentsIdsHavingDuplicatedJournals(int tenant)
        {
            var context = AccountingContext.GetContext(tenant);

            IQueryable<ARPaymentsId> result = from journal in context.Journals
                                        where journal.Tenant == tenant
                                             && journal.AccountingEntityCode == AccountingEntityValues.ARPayment
                                             && journal.IsVoided == false
                                             && journal.OriginalJournalId == null
                                        group journal by journal.AccountingEntityId into grouped
                                        where grouped.Count() > 1
                                        select new ARPaymentsId() { ARPaymentId = grouped.Key };


            return result.ToList();
        }

        public List<Journal> GetJournalsToVoid(int tenant)
        {
            var context = AccountingContext.GetContext(tenant);
            var result = from journal in context.Journals
                         where journal.Tenant == tenant
                              && journal.AccountingEntityCode == AccountingEntityValues.ARPayment
                              && journal.IsVoided == false
                              && journal.OriginalJournalId == null
                         group journal by journal.AccountingEntityId into grouped
                         where grouped.Count() > 1
                         select grouped.FirstOrDefault();

            return result.ToList();
        }

        public void FixDuplicatedjournals(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                try
                {
                    var journals = GetJournalsToVoid(tenant);
                    foreach (var journal in journals)
                        VoidJournal(tenant, journal);

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }               

            }
        }

        private static void VoidJournal(int tenant, Journal journal)
        {
            var context = AccountingContext.GetContext(tenant);

            var service = new JournalVoidUpdateService(context, new Dictionary<string, IContext>(), tenant);

            var StornoOverrideM = new StornoOverrideM()
            {
                AccountingEntityCode = journal.AccountingEntityCode,
                AccountingEntityId = journal.AccountingEntityId,
                AccountingEntityReference = journal.AccountingEntityReference,
            };
            service.VoidJournal(journal.Id, tenant, StornoOverrideM);
        }
    }

    public class ARPaymentsId
    {
        public string ARPaymentId { get; set; }
    }
}
