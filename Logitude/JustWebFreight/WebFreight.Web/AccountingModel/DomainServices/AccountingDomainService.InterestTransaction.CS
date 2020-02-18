using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Logitude.Accounting.BL.InterestEntityQueryServices;
using Logitude.Accounting.BL.InterestEntityQueryServices.InterestFactory;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public List<InterestTransactionList> GetAllInterestTransactionByDate(string ReportId, DateTime InterestCalculationDate, int Tenant, IAccountingContext MyContext)
        {
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
                    if (interestTransactionLists[i].CurrencyId != null)
                    {
                        CurrencyRepository currencyRepository = new CurrencyRepository();
                        Currency Currency = currencyRepository.GetSingleCurrencyById(interestTransactionLists[i].CurrencyId, interestTransactionLists[i].Tenant, true);
                        interestTransactionLists[i].CurrencyCode = Currency.Code;
                    }
                }
            }

            return interestTransactionLists;
        }
    }
}