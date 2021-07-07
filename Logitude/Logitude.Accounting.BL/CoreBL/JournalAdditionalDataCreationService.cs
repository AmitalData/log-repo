using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
  public   class JournalAdditionalDataCreationService
    {
        public int tenant;
        public DateTime documentDate;
        private static IAccountingContext accountingContext;
        private static  FullAccountingSettingPM setting;
        public JournalAdditionalDataCreationService(int Tenant, DateTime DocumentDate)
        {
            tenant = Tenant;
            documentDate = DocumentDate;
            accountingContext= AccountingContext.GetContext(tenant);
            setting = GetTenantFullAccountingSetting();

        }
        public void CreateJournalAdditionalDataforTenantAndDate(string taxReportLineType)
        {
            if(taxReportLineType == "input")
            {
                CreateJournalAdditionalDataFroJournalInputLines();
            }
            else
            {
                CreateJournalAdditionalDataForOutputJournals();
            }

        }

        private void CreateJournalAdditionalDataForOutputJournals()
        {
            List<JournalAdditionalData> outputLines= GetJournalOutputLines();
            foreach (JournalAdditionalData line in outputLines)
            {
                JournalAdditionalDataPM journalAdditionalDataPM = MapJournalAdditionalLinePMFields(null, line);
                SaveJournalAdditionalData(journalAdditionalDataPM);

            }
        }
        private List<JournalAdditionalData> GetJournalOutputLines()
        {
           return (from a in accountingContext.JournalAdditionalDatas
                                                              join journal in accountingContext.Journals on a.JournalId equals journal.Id

                                                              where journal.AccountingEntityCode == "2" && journal.DocumentDate <=documentDate && a.JournalLineNumber == 0 && a.Tenant == tenant
                                                              select a).ToList();

        }
        private  void CreateJournalAdditionalDataFroJournalInputLines()
        {
           
            List<TaxReportData> inputLines = GetTenantJournalInputLinesByDateDate(tenant, documentDate);
            foreach (TaxReportData line in inputLines)
            {
                JournalAdditionalDataPM journalAdditionalDataPM = MapJournalAdditionalLinePMFields(line, null);
                SaveJournalAdditionalData(journalAdditionalDataPM);

            }
        }

        private static void SaveJournalAdditionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {
         
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(accountingContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }
        private static JournalAdditionalDataPM MapJournalAdditionalLinePMFields(TaxReportData reportData, JournalAdditionalData journalAdditionalData)
        {
            return new JournalAdditionalDataPM()
            {
                TaxReportTransmitStatusCode = reportData!=null? reportData.TransmitStatusCode: journalAdditionalData.TaxReportTransmitStatusCode,
                TaxReportId = reportData!= null? reportData.Id : journalAdditionalData.TaxReportId,
                JournalLineNumber = reportData != null?reportData.JournalLineNumber: journalAdditionalData.JournalLineNumber,
                JournalId = reportData!= null? reportData.JournalId: journalAdditionalData.JournalId,
                Tenant = reportData != null ? reportData.Tenant : journalAdditionalData.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert
            };

        }

        private static List<TaxReportData> GetTenantJournalInputLinesByDateDate(int tenant, DateTime documentDate)
        {
           
            return (from transaction in accountingContext.LedgerTransactions
                    join d in accountingContext.JournalAdditionalDatas on transaction.JournalId equals d.JournalId
                    where transaction.AccountId == setting.VATInputsGLAccountId && transaction.Tenant == tenant && transaction.DocumentDate <= documentDate && d.JournalLineNumber == 0 && transaction.OppositeAccountId != setting.VATInputsGLAccountId
                    select new TaxReportData()
                    {
                        Id = d.TaxReportId,
                        JournalId = transaction.JournalId,
                        JournalLineNumber = transaction.JournalLineNumber,
                        TransmitStatusCode = d.TaxReportTransmitStatusCode,
                        Tenant = transaction.Tenant

                    }).Distinct().ToList();
        }

        private  FullAccountingSettingPM GetTenantFullAccountingSetting()
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
        }


    }
}
