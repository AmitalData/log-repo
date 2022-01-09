using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Logitude.Accounting.Def.EntityUpdateServicesExt;

[assembly: InternalsVisibleTo("Your.Test.Assembly.Name")]

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalStornoService : IJournalStornoService
    {
        private JournalPM _JournalPM;
        
        private StornoOverrideM _StornoOverrideM;
        private IJournalUpdateService _journalUpdateService;
        private IJournalStornoPrepareJReconcileService _JournalStornoPrepareJReconcileService;

        public void Init(
            JournalPM journalPM, StornoOverrideM stornoOverrideM, 
            IJournalUpdateService journalUpdateService,
            IJournalStornoPrepareJReconcileService journalStornoPrepareJReconcileService
            )
        {
            // TODO: Complete member initialization
            this._JournalPM = journalPM;
            _StornoOverrideM = stornoOverrideM;
            _journalUpdateService = journalUpdateService;
            _JournalStornoPrepareJReconcileService = journalStornoPrepareJReconcileService;


        }

        void ThrowIfStornoNotAllowed()
        {
            var typeregular = "1"; //1	Regular	רגיל	1,Regular,רגיל	0

            var accountingPeriodsByTypeRegular =
                GetAccountingPeriodByType(typeregular, _JournalPM.Tenant);
            if (accountingPeriodsByTypeRegular == null)
            {
                ThrowCloseMonth(_JournalPM.Tenant);
            }
            if (_JournalPM.APPaymentCancelDate == null)
            {
                if (!(JournalValidatorNotStatic.IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), _JournalPM.AccountingDate)))
                {
                    ThrowCloseMonth(_JournalPM.Tenant);
                }
            }
            else
            {
                if (!(JournalValidatorNotStatic.IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(),(DateTime)_JournalPM.APPaymentCancelDate)))
                {
                    ThrowCloseMonth(_JournalPM.Tenant);
                }
            }
  

        }

        private void ThrowCloseMonth(int tenant)
        {
            var closeMonth = TextCodesTranslatorTranslateText(JournalValidator.M_ClosedMonth, tenant);
            throw new Exception(closeMonth);//”Accounting period closed
        }
        public virtual string TextCodesTranslatorTranslateText(string textCodeCode, int tenant)
        {
            return TextCodesTranslator.TranslateText(textCodeCode, tenant);
        }

        public virtual List<AccountingPeriodPM> GetAccountingPeriodByType(string periodTypeCode, int tenant)
        {
            var accountingPeriodQueryService = new AccountingPeriodQueryService(tenant);
            return accountingPeriodQueryService.GetAccountingPeriodByType(periodTypeCode, tenant); ;
        }
        public JournalPM CreateStornoAndCommitUpdate()
        {
            if (_StornoOverrideM == null)
            {
                throw new Exception("stornoOverrideM is must (good2 remember values in properties r not Must )");
            }

            ThrowIfStornoNotAllowed();
            Storno = CreateStorno(_StornoOverrideM);
            
            if (_JournalStornoPrepareJReconcileService.CreateJournalReconcileFromStorno(Storno, _StornoOverrideM))
            {
                Storno.JournalReconciles.AddRange(_JournalStornoPrepareJReconcileService.JournalReconciles2Insert);
            }

            _journalUpdateService.Update(Storno, true);
            return Storno;
        }

        
        public JournalPM CreateStorno(StornoOverrideM stornoOverrideM)
        {
            if (stornoOverrideM==null)
            {
                throw new Exception("stornoOverrideM is must (good2 remember values in properties r not Must )");
            }
            JournalPM Storno = new JournalPM();

            Storno.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            Storno.AccountingDate = _JournalPM.AccountingDate;
            if (stornoOverrideM.AccountingDate.HasValue)
            {
                Storno.AccountingDate = stornoOverrideM.AccountingDate.Value;
            }

            Storno.AccountingEntityCode = _JournalPM.AccountingEntityCode;
            if (!String.IsNullOrWhiteSpace(stornoOverrideM.AccountingEntityCode))
            {
                Storno.AccountingEntityCode = stornoOverrideM.AccountingEntityCode;
            }
            Storno.AccountingEntityId = _JournalPM.AccountingEntityId;
            if (!String.IsNullOrWhiteSpace(stornoOverrideM.AccountingEntityId))
            {
                Storno.AccountingEntityId = stornoOverrideM.AccountingEntityId;
            }

            Storno.AccountingEntityReference = _JournalPM.AccountingEntityReference;
            if (!String.IsNullOrWhiteSpace(stornoOverrideM.AccountingEntityReference))
            {
                Storno.AccountingEntityReference = stornoOverrideM.AccountingEntityReference;
            }
            //  Storno.AccountingEntityId = entityPM.AccountingEntityId;
            //Storno.AccountingEntityName = entityPM.AccountingEntityName;


            Storno.CreateDate = _JournalPM.CreateDate;//irrelevant UpdateService>oncreate Supress it

            Storno.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Approved; //Storno.StatusCode = "2";
            Storno.VoidedByJournalId = null;
            string updatedByUserId = _JournalPM.UpdatedByUserId;
            if (!AuthenticationUtil.IsResolveUserIdentityNameEqualSystem(_JournalPM.Tenant))
            {
                updatedByUserId= AuthenticationUtil.ResolveUserId(_JournalPM.Tenant);
            }
            Storno.CreatedByUserId = //_JournalPM.UpdatedByUserId;//irrelevant UpdateService>oncreate Supress it
                updatedByUserId;
            Storno.UpdatedByUserId = //_JournalPM.UpdatedByUserId;//irrelevant UpdateService>oncreate Supress it
                updatedByUserId;
            Storno.Tenant = _JournalPM.Tenant;
            Storno.TypeCode = _JournalPM.TypeCode;
            Storno.UpdateDate = DateTime.Now;
            Storno.UpdatedByUserId = //_JournalPM.UpdatedByUserId;
                updatedByUserId;
            Storno.OriginalJournalId = _JournalPM.Id;
            var maybeTrue = true;
            if (maybeTrue)
            {
                Storno.ExternalNo = _JournalPM.ExternalNo;
            }

            var journalLines = _JournalPM.JournalLines;

            if (stornoOverrideM.ChequeNumbersToExcludeFromStorno != null && stornoOverrideM.ChequeNumbersToExcludeFromStorno.Count > 0)
            {
                RemoveChequesJournalLinesByNumber(stornoOverrideM.ChequeNumbersToExcludeFromStorno, journalLines);
                ResequenceLinesNumbers(journalLines);
            }



            foreach (JournalLinePM item in journalLines)
            {
                JournalLinePM newStornoJournalLine = new JournalLinePM();
                newStornoJournalLine.ChangeSetOp = ChangeSetOperation.Insert;
                newStornoJournalLine.AccountingDate = item.AccountingDate;
                if (stornoOverrideM.AccountingDate.HasValue)
                {
                    newStornoJournalLine.AccountingDate = stornoOverrideM.AccountingDate.Value;
                }


                newStornoJournalLine.ActionCode = item.ActionCode;
                newStornoJournalLine.ActionTypeCode = item.ActionTypeCode;
                newStornoJournalLine.ActionName = item.ActionName;
                newStornoJournalLine.CreditAccountId = item.CreditAccountId;
                newStornoJournalLine.CreditAccountName = item.CreditAccountName;
                newStornoJournalLine.CreditAccountNumber = item.CreditAccountNumber;
                newStornoJournalLine.CreditControlAccountId = item.CreditControlAccountId;
                newStornoJournalLine.CreditControlAccountName = item.CreditControlAccountName;
                newStornoJournalLine.CreditControlAccountNumber = item.CreditControlAccountNumber;
                newStornoJournalLine.CurrencyId = item.CurrencyId;
                newStornoJournalLine.CurrencyName = item.CurrencyName;
                newStornoJournalLine.CurrencyCode = item.CurrencyCode;
                newStornoJournalLine.DebitAccountId = item.DebitAccountId;
                newStornoJournalLine.DebitAccountName = item.DebitAccountName;
                newStornoJournalLine.DebitAccountNumber = item.DebitAccountNumber;
                newStornoJournalLine.DebitControlAccountId = item.DebitControlAccountId;
                newStornoJournalLine.DebitControlAccountName = item.DebitControlAccountName;
                newStornoJournalLine.DebitControlAccountNumber = item.DebitControlAccountNumber;
                newStornoJournalLine.DocumentDate = item.DocumentDate;
                newStornoJournalLine.DueDate = item.DueDate;
                newStornoJournalLine.ExchangeRate = item.ExchangeRate;
                newStornoJournalLine.ForeignAmount = -item.ForeignAmount;
                newStornoJournalLine.JournalId = Storno.Id;
                newStornoJournalLine.Line = item.Line;

                newStornoJournalLine.LocalAmount = -item.LocalAmount;
                newStornoJournalLine.Notes = item.Notes;
                if (!String.IsNullOrWhiteSpace(stornoOverrideM.LineNotes))
                {
                    newStornoJournalLine.Notes = stornoOverrideM.LineNotes;
                }
                newStornoJournalLine.Reference1 = item.Reference1;
                newStornoJournalLine.Reference2 = item.Reference2;
                newStornoJournalLine.Reference3 = item.Reference3;
                newStornoJournalLine.Tenant = item.Tenant;
                //newStornoJournalLine.ExternalOpenAmount = item.ExternalOpenAmount;

                Storno.JournalLines.Add(newStornoJournalLine);

            }
            
            return Storno;
        }

        private void RemoveChequesJournalLinesByNumber(List<string> chequeNumbersToExcludeFromStorno, List<JournalLinePM> journalLines)
        {
            journalLines.RemoveAll(line =>
            {
                return chequeNumbersToExcludeFromStorno.Contains(line.Reference2);
            });
        }

        private void ResequenceLinesNumbers(List<JournalLinePM> journalLines)
        {
            var lineNumber = 1;
            foreach (var line in journalLines)
            {
                line.Line = lineNumber++;
            }
        }

        public JournalPM Storno { get; set; }
    }
    public interface IJournalStornoService
    {
        JournalPM CreateStornoAndCommitUpdate();
    }
    //public class StornoOverrideM
    //{
    //    public string AccountingEntityCode { get; set; }
    //    public string AccountingEntityReference { get; set; }
    //    public string AccountingEntityId { get; set; }
    //}
}
