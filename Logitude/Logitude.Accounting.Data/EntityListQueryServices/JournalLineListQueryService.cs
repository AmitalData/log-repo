using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityKeys;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class JournalLineListQueryService
    {
	    private IQueryable<JournalLineList> GetIqueryableList(IQueryable<JournalLine> iQueryable)
        {
            IQueryable<JournalLineList> query = (from a in iQueryable.Include("Journal").Include("Currency").Include("JournalActionType")
                                                 select new JournalLineList()
                                                      {
                                                          JournalId = a.JournalId,
                                                          Line = a.Line,
                                                          AccountingDate = a.AccountingDate,
                                                          ActionCode = a.ActionCode,
                                                          ActionTypeCode = a.JournalActionType != null ? a.JournalActionType.Code : null,
                                                          ActionName = a.JournalActionType != null ? a.JournalActionType.EnglishName : null,
                                                        
                                                          CreditControlAccountId = a.CreditControlAccountId,
                                                        
                                                          CreditAccountId = a.CreditAccountId,
                                                        
                                                          CurrencyId = a.CurrencyId,
                                                          CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                          CurrencyCode = a.Currency != null ? a.Currency.Code : null,

                                                          DebitAccountId = a.DebitAccountId,
                                                      
                                                          DebitControlAccountId = a.DebitControlAccountId,
                                                          DocumentDate = a.DocumentDate,
                                                          DueDate = a.DueDate,
                                                          ExchangeRate = a.ExchangeRate,
                                                          ForeignAmount = a.ForeignAmount,
                                                     
                                                          LocalAmount = a.LocalAmount,
                                                          Reference1 = a.Reference1,
                                                          Reference2 = a.Reference2,
                                                          Reference3 = a.Reference3,
                                                          Tenant = a.Tenant,
                                                          ExternalOpenAmount = a.ExternalOpenAmount,
                                                      });
            return query;
        }

		private IQueryable<JournalLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<JournalLine> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<JournalLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<JournalLine> iQueryable,int tenant)
        {
			return iQueryable;
		}

        public IQueryable<JournalLineList> GetJournalLinesForJournal(string JournalId, int tenant)
        {
            IQueryable<JournalLineList> Journallines;
        


            IQueryable<JournalLine> lines =  (from a in context.JournalLines.Include("CreditAccount")
                                                 where a.JournalId == JournalId && a.Tenant == tenant
                                                 select a);

            Journallines = (from a in lines
                            select new JournalLineList()
                            {
                                JournalId = a.JournalId,
                                Line = a.Line,
                                AccountingDate = a.AccountingDate,
                                DocumentDate = a.DocumentDate,
                                DueDate = a.DueDate,
                                ActionCode = a.ActionCode,
                                CreditAccountId = a.CreditAccountId,
                                CreditAccountName = a.CreditAccount != null ? a.CreditAccount.LocalName : null,
                                ActionName = a.JournalActionType != null ? a.JournalActionType.LocalName : null,
                            });

            return Journallines;
        }

    }


}
	