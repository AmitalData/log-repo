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
    public class DuplicatedJournalsVoidingService
    {
        public string LoggingText { get; set; } = "";

        public DuplicatedJournalsVoidingService()
        {

        }

        public List<Journal> GetARPaymentDuplicatedJournals(int tenant)
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

        public List<Journal> VoidDuplicatedJournalsOfARPayment(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    var journals = GetARPaymentDuplicatedJournals(tenant);
                    foreach (var journal in journals)
                        VoidJournal(tenant, journal);

                    scope.Complete();

                    return journals;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }               

            }
        }

        private void VoidJournal(int tenant, Journal journal)
        {
            StornoOverrideM journalStorno = BuildJournalStorno(journal);

            var journalVoidService = new JournalVoidUpdateService(AccountingContext.GetContext(tenant), new Dictionary<string, IContext>(), tenant);
            journalVoidService.VoidJournal(journal.Id, tenant, journalStorno);
        }

        private StornoOverrideM BuildJournalStorno(Journal journal)
        {
            return new StornoOverrideM()
            {
                AccountingEntityCode = journal.AccountingEntityCode,
                AccountingEntityId = journal.AccountingEntityId,
                AccountingEntityReference = journal.AccountingEntityReference,
            };
        }
    }
}
