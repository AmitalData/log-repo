using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   public partial class JournalLineQueryService
    {
        Logitude.Accounting.BL.EntityQueryServices.JournalLineQueryService query;

        public JournalLineQueryService(int tenant)
        {
            query = new Logitude.Accounting.BL.EntityQueryServices.JournalLineQueryService(tenant);
        }

        public List<JournalLinePM> JournalLineCustomDataMappingAndValidatin(Journal MyEntity,List<JournalLine> journalLines, int Tenant, string ComputingPartnerName = "")
        {

            try
            {

                var lines = new List<JournalLinePM>();
                foreach (var item in journalLines)
                {

                    var temp = new JournalLinePM();
                    if (!string.IsNullOrEmpty(item.JournalNumber) && item.Line != null)
                    {
                        JournalQueryService journalQueryService = new JournalQueryService(Tenant);
                        Journal journal = journalQueryService.GetJournalByNumber(item.JournalNumber, Tenant);
                        if(journal != null)
                        {
                            temp = query.GetSingleJournalLine(journal.Id, item.Line, Tenant);
                        }
                       
                    }

                    if (temp == null)
                    {
                        throw new ApplicationException("JournalLine with Line " + item.Line + " doesn't exist");

                    }
                    temp.JournalId = item.JournalNumber;
                    temp.Line = item.Line;
                    temp.Tenant = item.Tenant;
                    Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService gLAccoutQueryService = new EntityQueryServices.GLAccountQueryService(Tenant);
                    if(!string.IsNullOrEmpty( item.CreditControlAccount))
                    {
                        GLAccountPM CreditControlAccount = gLAccoutQueryService.GetSinglePMByInternalNumber(item.CreditControlAccount, Tenant);

                        if (CreditControlAccount == null)
                        {
                            throw new ApplicationException("In Journal line " + item.Line + " GLAccount with internal number " + item.CreditControlAccount + " doesn't exist");

                        }
                        else
                        {
                            temp.CreditControlAccountId = CreditControlAccount.Id;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.CreditAccount) )
                    {
                        GLAccountPM CreditAccount = gLAccoutQueryService.GetSinglePMByInternalNumber(item.CreditAccount, Tenant);

                        if (CreditAccount == null)
                        {
                            throw new ApplicationException("In Journal line " + item.Line + " GLAccount with internal number " + item.CreditAccount + " doesn't exist");

                        }
                        else
                        {
                            temp.CreditAccountId = CreditAccount.Id;
                        }
                    }


                    if (!string.IsNullOrEmpty( item.DebitControlAccount))
                    {
                        GLAccountPM DebitControlAccount = gLAccoutQueryService.GetSinglePMByInternalNumber(item.DebitControlAccount, Tenant);

                        if (DebitControlAccount == null)
                        {
                            throw new ApplicationException("In Journal line " + item.Line + " GLAccount with internal number " + item.DebitControlAccount + " doesn't exist");

                        }
                        else
                        {

                            temp.DebitControlAccountId = DebitControlAccount.Id;
                        }
                    }


                    if (!string.IsNullOrEmpty(item.DebitAccount))
                    {
                        GLAccountPM DebitAccount = gLAccoutQueryService.GetSinglePMByInternalNumber(item.DebitAccount, Tenant);

                        if (DebitAccount == null)
                        {
                            throw new ApplicationException("In Journal line " + item.Line +" GLAccount with internal number " + item.DebitAccount + " doesn't exist");

                        }
                        else
                        {
                            temp.DebitAccountId = DebitAccount.Id;
                        }
                    }

                    temp.DocumentDate = item.ReferenceDate.HasValue ? item.ReferenceDate.Value : item.DocumentDate;
                    temp.AccountingDate = item.AccountingDate;
                    temp.DueDate = item.DueDate;
                    temp.LocalAmount = item.LocalAmount;
                    CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
                    if (item.Currency != null)
                    {
                        var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.Currency, Tenant, ComputingPartnerName);
                        if (myCurrencyPM != null)
                        {
                            temp.CurrencyId = myCurrencyPM.Id;
                        }

                    }


                    temp.ForeignAmount = item.ForeignAmount;
                    temp.ExchangeRate = item.ExchangeRate;
                    temp.Reference1 = item.Reference1;
                    temp.Reference2 = item.Reference2;
                    temp.Reference3 = item.Reference3;
                    temp.Notes = item.Notes;
                    if(item.ExternalOpenAmount != null)
                    temp.ExternalOpenAmount = item.ExternalOpenAmount;
                    temp.ExternalReconcileNumber = item.ExternalReconcileNumber;
                    temp.ConfirmationNumber = item.ConfirmationNumber; 
                    temp.ExcludeFromTaxReport = item.ExcludeFromTaxReport;

                    JournalActionTypeQueryService journalActionTypeService = new JournalActionTypeQueryService(Tenant);
                    temp.ActionCode = item.ActionCode;

                    lines.Add(temp);
                }

                return lines;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<JournalLine> JournalLineCustomDataMapping(JournalPM MyEntityPM, List<JournalLinePM> lines, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var MyList = new List<JournalLine>();
                JournalPM journalPM = null;

                JournalQueryService journalQueryService = new JournalQueryService(Tenant);

                if (lines != null && lines.Any())
                    journalPM = journalQueryService.GetJournalPMById(lines[0].JournalId, Tenant);

                foreach (var item in lines)
                {

                    var temp = new JournalLine();

                    if (journalPM != null)
                        temp.JournalNumber = journalPM.JournalNumber;
                    temp.Line = item.Line;
                    temp.Tenant = item.Tenant;
                    temp.DocumentDate = item.DocumentDate;
                    temp.ReferenceDate = item.DocumentDate;
                    temp.AccountingDate = item.AccountingDate;
                    temp.DueDate = item.DueDate;
                    temp.LocalAmount = item.LocalAmount;
                    if (item.CurrencyId != null)
                    {
                        CurrencyQueryService CurrencyService4 = new CurrencyQueryService(Tenant);
                        temp.Currency = CurrencyService4.GetCurrencyById(item.CurrencyId, Tenant);

                    }

                    temp.ForeignAmount = item.ForeignAmount;
                    temp.ExchangeRate = item.ExchangeRate;
                    temp.Reference1 = item.Reference1;
                    temp.Reference2 = item.Reference2;
                    temp.Reference3 = item.Reference3;
                    temp.CreditAccount = item.CreditAccountNumber;
                    temp.CreditControlAccount = item.CreditControlAccountNumber;
                    temp.DebitControlAccount = item.DebitControlAccountNumber;
                    temp.DebitAccount = item.DebitAccountNumber;
                    temp.Notes = item.Notes;
                    temp.ExternalOpenAmount = item.ExternalOpenAmount;
                    temp.ExternalReconcileNumber = item.ExternalReconcileNumber;
                    temp.ActionCode = item.ActionCode;
                    temp.ConfirmationNumber = item.ConfirmationNumber;
                    temp.ExcludeFromTaxReport = item.ExcludeFromTaxReport;
                    MyList.Add(temp);
                }

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
