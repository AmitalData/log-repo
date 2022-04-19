using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalFailedService
    {
        private int _Tenant;
        private string _SeedJournalId;

        JournalFailedService()
        {

        }

        public JournalFailedService(int tenant, string seedJournalId)
        {
            _Tenant = tenant;
            _SeedJournalId = seedJournalId;
            //this._QMessageId = MessageId;
        }
        public  void MarkAsFailed(Exception ex)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {
                var accountingContext = AccountingContext.GetContext(_Tenant);
                bool fastAsPosible = true;
                if (!fastAsPosible)
                {
                    var up = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _Tenant);
                    var journalQueryService = new JournalQueryService(accountingContext);
                    var journalPM = journalQueryService.GetSingle(_SeedJournalId, true, false);
                    journalPM.StatusCodeEnum = Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Failed;
                    up.Update(journalPM, true);
                }
                else
                {


                    var repo = new JournalRepository(accountingContext);
                    
                    var up = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _Tenant);
                    up.SetStatusCodeFailed(_SeedJournalId, _Tenant);
                    //var poco = repo.GetSingle(_SeedJournalId, _Tenant);
                    //poco.StatusCode = ((int)Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Failed).ToString();
                    //repo.Update(poco);

                    
                    if (ex != null)
                    {
                        var sb = new StringBuilder();
                        sb
                            .AppendLine("MarkAsFailed")
                            .AppendLine($"At:{DateTime.Now}")
                            .AppendLine(ex.Message)
                            .AppendLine(ex.ToString());

                        InsertJournalMoreData(accountingContext, sb);

                    }
                    accountingContext.SaveChanges();

                }
                scope.Complete();
            }
        }

        public void InsertJournalMoreData(IAccountingContext accountingContext, StringBuilder sb)
        {
            var repoMD = new JournalMoreDataRepository(accountingContext);
            int last = 0;
            var q = repoMD.GetAll(_Tenant).Where(r => r.JournalId == _SeedJournalId).Select(r => (int?)r.Line);
            last = q.DefaultIfEmpty().Max() ?? 0;
            ++last;

            //var logCompressText = InjectionUtil.Instance.CompressText(sb.ToString(/*0, Math.Min(sb.Length, (4000 - 2))*/));
            string log = sb.ToString(0, Math.Min(sb.Length, (4000 - 2)));
            var pocoJournalMoreData = new Data.EntityPOCOs.JournalMoreData()
            {
                Line = last,
                JournalId = _SeedJournalId,
                Tenant = _Tenant,
                GeneralData = log
            };
            repoMD.Add(pocoJournalMoreData);
        }
    }
}
