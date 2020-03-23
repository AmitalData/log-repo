using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Logitude.Accounting.BL.InterestEntityQueryServices;
using Logitude.Accounting.BL.InterestEntityQueryServices.InterestFactory;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.InterestService
{
    public   class InterestService
    {
        public InterestTransactionsWithTotal GetAllInterestTransactionByDate(string ReportId, DateTime InterestCalculationDate, int Tenant, IAccountingContext MyContext=null)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(Tenant);
            }
            InterestTransactionsWithTotal interestTransactionsWithTotal = new InterestTransactionsWithTotal();
            decimal TotalLocalAmount=0; 
            List<InterestTransactionList> interestTransactionLists = (from a in MyContext.InterestReportLines
                                                                      where a.Tenant == Tenant && a.InterestReportId == ReportId && DbFunctions.TruncateTime(a.InterestTransaction.InterestValueDate) == InterestCalculationDate.Date
                                                                      select new InterestTransactionList()
                                                                      {
                                                                          Tenant = a.Tenant,

                                                                          CurrencyId = a.InterestTransaction.CurrencyId,

                                                                          InterestEntityTypeCode = a.InterestTransaction.InterestEntityTypeCode,

                                                                          EntityId = a.InterestTransaction.EntityId,

                                                                          LocalAmount = a.InterestTransaction.LocalAmount,

                                                                          ForeignAmount = a.InterestTransaction.ForeignAmount,

                                                                          InterestValueDate = a.InterestTransaction.InterestValueDate,

                                                                      }).ToList();
            if (interestTransactionLists != null && interestTransactionLists.Count > 0)
            {
                JournalQueryService journalQueryService = new JournalQueryService(Tenant);
                List<string> JournalNumbers = journalQueryService.GetJournalNumbersByTransactionsList(interestTransactionLists, Tenant);

                for (int i = 0; i < interestTransactionLists.Count; i++)
                {
                    IInterestEntityQueryService interestEntityQueryService = InterestQueryFactory.CreateInstance(interestTransactionLists[i].InterestEntityTypeCode);
                    InterestEntityResult entityResult = interestEntityQueryService.GetInterestEntity(interestTransactionLists[i].EntityId, interestTransactionLists[i].Tenant);
                    if (entityResult.JournalNumber == null)
                    {
                        interestTransactionLists[i].JournalNumber = (from a in JournalNumbers
                                                                     where interestTransactionLists[i].InterestEntityTypeCode == a.Split(',')[2]
                                                                            && interestTransactionLists[i].EntityId == a.Split(',')[1]
                                                                            select a.Split(',')[0]).FirstOrDefault();
                    }
                    else
                    {
                        interestTransactionLists[i].JournalNumber = entityResult.JournalNumber;
                    }

                    interestTransactionLists[i].InterestEntityNumber = entityResult.EntityNumber;
                    interestTransactionLists[i].InterestEntityType = entityResult.EntityType;
                    interestTransactionLists[i].InterestEntityIconCode = entityResult.EntityTypeCode;
                    interestTransactionLists[i].JournalId = entityResult.JournalId;
                    interestTransactionLists[i].AccountEntityCode = entityResult.AccountCode;
                    TotalLocalAmount += interestTransactionLists[i].LocalAmount;
                    if (interestTransactionLists[i].CurrencyId != null)
                    {
                        CurrencyRepository currencyRepository = new CurrencyRepository();
                        Currency Currency = currencyRepository.GetSingleCurrencyById(interestTransactionLists[i].CurrencyId, interestTransactionLists[i].Tenant, true);
                        interestTransactionLists[i].CurrencyCode = Currency.Code;
                    }
                }
            }
            interestTransactionsWithTotal.interestTransactionLists = interestTransactionLists;
            decimal? TotalLocal = MethodHelper.Round(TotalLocalAmount, 2);
            if (TotalLocal == null){
                interestTransactionsWithTotal.TotalLocalAmount = 0;
            }
            else
            {
                interestTransactionsWithTotal.TotalLocalAmount = (decimal)MethodHelper.Round(TotalLocalAmount, 2);
            }

            return interestTransactionsWithTotal;
        }


        public InterestReportPM PutConfirmCreateInvoice(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext)
        {
            if (MainContext == null)
            {
                MainContext = AccountingContext.GetContext(Tenant);
            }

            ContactQuery contactQuery = new ContactQuery(Tenant);
            ContactPM contact = GetLoggedContact(Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;
            InterestReportUpdateService InterestReportUpdate = new InterestReportUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            InterestTransactionUpdateService InterestTransactionUpdate = new InterestTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            interestReportPM.CloseBalance = 0;
            if (interestReportPM.OpenBalance != null )
            {
                interestReportPM.CloseBalance += interestReportPM.OpenBalance;
            }
            if (interestReportPM.TotalAmount != null)
            {
                interestReportPM.CloseBalance += interestReportPM.TotalAmount;
            }
             if (interestReportPM.InterestReportStatusCode != "2")
            {
                interestReportPM.InterestReportStatusCode = "4";
            }
            if (interestReportPM.InterestReportStatusCode != null)
            {
                InterestReportStatuseRepository interestReportStatuseRepository = new InterestReportStatuseRepository(Tenant);
                InterestReportStatuse interestReportStatuse = interestReportStatuseRepository.GetSingle(interestReportPM.InterestReportStatusCode);
                interestReportPM.InterestReportStatusName = showLocals ? interestReportStatuse.LocalName : interestReportStatuse.EnglishName;
                interestReportPM.InterestReportStatusLocalName = showLocals ? interestReportStatuse.LocalName : interestReportStatuse.EnglishName;
            }

            interestReportPM.ChangeSetOp = ChangeSetOperation.Update;
            InterestReportUpdate.Update(interestReportPM, true);
            List<InterestTransactionPM> interestTransactionPMs = GetAllInterestTransactionForReport(interestReportPM.Id, Tenant, MainContext).ToList();

            for (int i =0; i < interestTransactionPMs.Count; i++)
            {
                interestTransactionPMs[i].IsClosed = true;
                interestTransactionPMs[i].InterestReportId = interestReportPM.Id;
                interestTransactionPMs[i].ChangeSetOp = ChangeSetOperation.Update;
                InterestTransactionUpdate.Update(interestTransactionPMs[i], true);
            }

            return interestReportPM;
        }

        private IQueryable<InterestTransactionPM> GetAllInterestTransactionForReport( string InterestReportId,int Tenant, IAccountingContext MyContext)
        {
            IQueryable<InterestTransactionPM> interestTransactionLists = (from a in MyContext.InterestReportLines
                                                                          where a.Tenant == Tenant && a.InterestReportId == InterestReportId
                                                                          select new InterestTransactionPM()
                                                                          {
                                                                              Id =a.InterestTransaction.Id,

                                                                              Tenant = a.Tenant,

                                                                              CurrencyId = a.InterestTransaction.CurrencyId,

                                                                              InterestEntityTypeCode = a.InterestTransaction.InterestEntityTypeCode,

                                                                              EntityId = a.InterestTransaction.EntityId,

                                                                              LocalAmount = a.InterestTransaction.LocalAmount,

                                                                              ForeignAmount = a.InterestTransaction.ForeignAmount,

                                                                              InterestValueDate = a.InterestTransaction.InterestValueDate,

                                                                              InterestReportId = a.InterestTransaction.InterestReportId,

                                                                              IsClosed = a.InterestTransaction.IsClosed,

                                                                              GLAccountId = a.InterestTransaction.GLAccountId,

                                                                              CreateDateTime = a.InterestTransaction.CreateDateTime,

                                                                              UpdateDateTime = a.InterestTransaction.UpdateDateTime,
                                                                        
                                                                              OriginalEntityLineNumber = a.InterestTransaction.OriginalEntityLineNumber,

                                                                              SearchFields = a.InterestTransaction.SearchFields,
 
                                                                          });
            return interestTransactionLists;

        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public bool SuppressFetchOpenReconcilation { get; internal set; }

        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }


    public class InterestTransactionsWithTotal
    {
        public List<InterestTransactionList> interestTransactionLists { get; set; }
        public decimal TotalLocalAmount { get; set; }

    }
}