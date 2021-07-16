using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL
{
  public class JournalAdditionalDataCreationService
    {
        public int tenant;
        public DateTime documentDate;
        private  IAccountingContext accountingContext;
        private   FullAccountingSettingPM setting;
        private  const int maxAllowedLinesCount=3000;
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
            List<TaxReportData> outputLines= GetJournalOutputLines();
            HandleJournalAdditionalLines(outputLines);



        }
        private bool CheckIfJournalAdditionalDataExist(JournalAdditionalDataPM journalAdditionalDataPM)
        {
            JournalAdditionalDataPM journalAdditionalData = GetJournalAdditionalData(journalAdditionalDataPM.JournalId,journalAdditionalDataPM.JournalLineNumber);
        //   if(journalAdditionalData!= null) UpdateJournalAdditionalData(journalAdditionalData);
            if(journalAdditionalData == null) return false;
            return true;
        }
        private JournalAdditionalDataPM GetJournalAdditionalData(string journalId, int lineNumber)
        {
            JournalAdditionalDataQueryService journalAdditionalDataQueryService = new JournalAdditionalDataQueryService(tenant);
            return journalAdditionalDataQueryService.GetSingle(journalId, lineNumber, false, false);

        }
        private void UpdateJournalAdditionalData(JournalAdditionalDataPM journalAdditionalData)
        {
            journalAdditionalData.Tenant = -100;
            journalAdditionalData.ChangeSetOp = ChangeSetOperation.Update;
            SaveJournalAdditionalData(journalAdditionalData);
        }
        private List<TaxReportData> GetJournalOutputLines()
        {
            List<TaxReportData> notIncludedJournals = new List<TaxReportData>();
            notIncludedJournals = GetJournalAdditionalLinesThatNotIncludedInAnyReport(tenant);
            List<TaxReportData> includedJournals = new List<TaxReportData>();
            includedJournals = GetJournalAdditionalLinesThatIncludedInTaxReports(tenant);        
            notIncludedJournals= notIncludedJournals.Concat(includedJournals).ToList();
            return notIncludedJournals;
        }
        private List<TaxReportData> GetJournalAdditionalLinesThatIncludedInTaxReports(int tenant)
        {
            return (from a in accountingContext.JournalAdditionalDatas
                    join journal in accountingContext.Journals on a.JournalId equals journal.Id
                    join taxreport in accountingContext.TaxReports on a.TaxReportId equals taxreport.Id
                    where journal.AccountingEntityCode == "2" && a.Tenant == tenant
                   && (a.TaxReportId != null && (taxreport.StatusCode == "D" || taxreport.StatusCode == "E"))
                    select new TaxReportData()
                    {
                        Id = a.TaxReportId,
                        JournalId = journal.Id,
                        JournalLineNumber = 1,
                        TransmitStatusCode = a.TaxReportTransmitStatusCode,
                        Tenant = a.Tenant

                    }).ToList();
        }
        private List<TaxReportData> GetJournalAdditionalLinesThatNotIncludedInAnyReport(int tenant)
        {
          return  (from a in accountingContext.JournalAdditionalDatas
             join journal in accountingContext.Journals on a.JournalId equals journal.Id
             where journal.AccountingEntityCode == "2" && a.Tenant == tenant
            && (a.TaxReportId == null)

             select new TaxReportData()
             {
                 Id = a.TaxReportId,
                 JournalId = journal.Id,
                 JournalLineNumber = 1,
                 TransmitStatusCode = a.TaxReportTransmitStatusCode,
                 Tenant = a.Tenant

             }).ToList();

        }

        private  void CreateJournalAdditionalDataFroJournalInputLines()
        {
            List<TaxReportData> inputLines = GetTenantJournalInputLinesByDateDate(tenant, documentDate);
            HandleJournalAdditionalLines(inputLines);
          
        }
        private  void HandleJournalAdditionalLines( List<TaxReportData> taxReportLines)
        {
            if (taxReportLines.Count > maxAllowedLinesCount)
            {
                 InsertMoreThanMaxAllowedLinesCount(taxReportLines);
            }
            else
            {
                 InsertTaxReportLines(taxReportLines, 0);
            }
        }


        List<JournalAdditionalDataPM> createdLines;
        private void InsertMoreThanMaxAllowedLinesCount(List<TaxReportData> taxReportLines)
        {
           createdLines = new List<JournalAdditionalDataPM>();

            for (int i = 0; i < taxReportLines.Count; i += maxAllowedLinesCount)
            {
               InsertTaxReportLinesInMaxAllowedListRange(taxReportLines, i);
            }
            
        }
        private void InsertTaxReportLinesInMaxAllowedListRange(List<TaxReportData> taxReportLines, int startIndex)
        {
             InsertTaxReportLines( taxReportLines.GetRange(startIndex, Math.Min(maxAllowedLinesCount, taxReportLines.Count - startIndex)), startIndex);
        }
        private void InsertTaxReportLines( List<TaxReportData> taxReportLines, int startIndex)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(60)))
            {
                int count = startIndex;
                foreach (TaxReportData line in taxReportLines)
                {
                    ++count;

                JournalAdditionalDataPM oldjournalAdditionalData = GetJournalAdditionalData(line.JournalId, 0);
                        if(oldjournalAdditionalData!= null)  UpdateJournalAdditionalData(oldjournalAdditionalData);
                     JournalAdditionalDataPM journalAdditionalDataPM = MapJournalAdditionalLinePMFields(line, null);
                   bool exist = CheckIfJournalAdditionalDataExist(journalAdditionalDataPM);
                  if(!exist)   SaveJournalAdditionalData(journalAdditionalDataPM);
                  //  createdLines.Add(journalAdditionalDataPM);
                }
                scope.Complete();
                //return createdLines;

            }
        }
        private  void SaveJournalAdditionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {         
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(accountingContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }
        private  JournalAdditionalDataPM MapJournalAdditionalLinePMFields(TaxReportData reportData, JournalAdditionalData journalAdditionalData)
        {
            return new JournalAdditionalDataPM()
            {
                TaxReportTransmitStatusCode = reportData!=null? reportData.TransmitStatusCode: journalAdditionalData.TaxReportTransmitStatusCode,
                TaxReportId = reportData!= null? reportData.Id : journalAdditionalData.TaxReportId,
                JournalLineNumber = reportData != null?reportData.JournalLineNumber: 1,
                JournalId = reportData!= null? reportData.JournalId: journalAdditionalData.JournalId,
                Tenant = reportData != null ? reportData.Tenant : journalAdditionalData.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert
            };

        }
      
        private   List<TaxReportData> GetTenantJournalInputLinesByDateDate(int tenant, DateTime documentDate)
        {
            List<TaxReportData> notIncludedTransactions = new List<TaxReportData>();
            notIncludedTransactions = GetInputLinesThatNotIncludedInAnyReport(tenant);
            List<TaxReportData> includedTransactions = new List<TaxReportData>();
            includedTransactions = GetInputLinesThatIncludedInTaxReport(tenant);          
            notIncludedTransactions = notIncludedTransactions.Concat(includedTransactions).ToList();
            return notIncludedTransactions;
        }
        private  List<TaxReportData> GetInputLinesThatNotIncludedInAnyReport(int tenant)
        {
       
           return (from transaction in accountingContext.LedgerTransactions
                                       join d in accountingContext.JournalAdditionalDatas on transaction.JournalId equals d.JournalId

                                       where transaction.AccountId == setting.VATInputsGLAccountId && transaction.Tenant == tenant &&
                                       transaction.LocalAmountDebit != 0 &&
                                       transaction.OppositeAccountId != setting.VATOutputGLAccountId
                                      && (d.TaxReportId == null)

                                       select new TaxReportData()
                                       {
                                           Id = d.TaxReportId,
                                           JournalId = transaction.JournalId,
                                           JournalLineNumber = transaction.JournalLineNumber,
                                           TransmitStatusCode = d.TaxReportTransmitStatusCode,
                                           Tenant = transaction.Tenant

                                       }).Distinct().ToList();
        }
        private List<TaxReportData> GetInputLinesThatIncludedInTaxReport(int tenant)
        {
           return(from transaction in accountingContext.LedgerTransactions
                                    join d in accountingContext.JournalAdditionalDatas on transaction.JournalId equals d.JournalId
                                    join taxreport in accountingContext.TaxReports on d.TaxReportId equals taxreport.Id
                                    where transaction.AccountId == setting.VATInputsGLAccountId && transaction.Tenant == tenant &&
                                    transaction.LocalAmountDebit != 0 &&
                                    transaction.OppositeAccountId != setting.VATOutputGLAccountId


                                   && (d.TaxReportId != null && (taxreport.StatusCode == "D" || taxreport.StatusCode == "E"))

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
