using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile.Utils;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class CreateAutoExternalReconcileWhileStreamingService: WhileStreamingBase
    {
        //private IAccountingContext _AccountingContext;
        //private JournalPM _JournalPM;
        //private List<LedgerTransactionPM> _NewLedgerTransactionsWithCounters;
        //IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        //public List<ExternalReconciliationPM> ExternalReconciliationList { get; private set; }

        //internal void MustInit(ExternalReconcileDataProvider externalReconcileDataProvider, JournalPM journalPM, List<LedgerTransactionPM> myNewLedgerTransactionsWithCounters)
        //{
        //    _ExternalReconcileDataProvider =externalReconcileDataProvider;
        //    this._JournalPM = journalPM;
        //    this._NewLedgerTransactionsWithCounters =myNewLedgerTransactionsWithCounters;
        //}
        /// <summary>
        /// יוצר 2 התאמות כנגד הפק יומן
        /// 1- בנק לשלם 
        /// 1.1 זכות תנועה ישנה
        /// 1.2 חובה תנועה חדשה
        /// 
        /// 2.1 - בנק עו"ש -זיכוי תנועה חדשה 
        /// 2.2 חובה תנועה ישנה  -דף בנק
        /// </summary>
        internal void CreateAutoExternalReconcileWhileStreaming()
        {

            if (_JournalPM.JournalExternalReconciles.Count == 0)
            {
                return;//nothing to do !!!
            }

            var myExternalReconcileTypeService = new ExternalReconcileTypeService();
            myExternalReconcileTypeService.MustInit(_ExternalReconcileDataProvider);


            bool isCreateAutoExternalReconcileMoveBankCheckFromTransferService = //TypeIs_CreateAutoExternalReconcileMoveBankCheckFromTransfer();
                myExternalReconcileTypeService.GetExternalReconcileTypeFromJournal(_JournalPM)
                == ExternalReconcileType.MoveBankCheckFromTransferExternalReconcile;

            //if (_JournalPM.JournalExternalReconciles.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionId)))
            if (!isCreateAutoExternalReconcileMoveBankCheckFromTransferService)
            {
                //while create  ExternalReconcileAdjustBankFeesService the bank lines are  string.IsNullOrWhiteSpace(r.LedgerTransactionId) 
                var command = new CreateAutoExternalReconcileWhileStreamingFeesService();
                command.MustInit(_ExternalReconcileDataProvider, _JournalPM, _NewLedgerTransactionsWithCounters);
                command.AdjustBankFees();
                this.ExternalReconciliationList = command.ExternalReconciliationList;
            }
            else
            {

                var command = new CreateAutoExternalReconcileMoveBankCheckFromTransferService();
                command.MustInit(_ExternalReconcileDataProvider, _JournalPM, _NewLedgerTransactionsWithCounters);
                command.MoveBankCheckFromTransfer();
                this.ExternalReconciliationList = command.ExternalReconciliationList;

            }
        }
#if true
        private bool TypeIs_CreateAutoExternalReconcileMoveBankCheckFromTransfer()
        {
            
            if (_JournalPM.JournalExternalReconciles.Count() != 1)
            {
                return false;
            }
            var myJournalExternalReconciles = _JournalPM.JournalExternalReconciles.First();
            if (String.IsNullOrWhiteSpace(myJournalExternalReconciles.LedgerTransactionId))
            {
                return false;
            }
            var listLedger =
            _ExternalReconcileDataProvider.GetLedgerTransactionList(new List<string>() { myJournalExternalReconciles.LedgerTransactionId }, _JournalPM.Tenant);
            var myLedgerTransactionTransferInCredit = listLedger.First();
            if (myLedgerTransactionTransferInCredit.LocalAmountDebit != 0)
            {
                return false;// not in credit
            }

            var BankAccountFromTransferAccount = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _JournalPM.Tenant);
            return BankAccountFromTransferAccount != null;
        }


#endif
        public static string GetAdjustGLAccountId(JournalPM myJournalPM)
        {
            string adjustGLAccountId = "";
            var journalLineToadjustGLAccount = myJournalPM.JournalLines.Last();
            if (journalLineToadjustGLAccount.ActionTypeCodeEnum == JournalActionTypeEnum.Credit)
            {
                adjustGLAccountId = journalLineToadjustGLAccount.CreditAccountId;
            }
            else if (journalLineToadjustGLAccount.ActionTypeCodeEnum == JournalActionTypeEnum.Debit)
            {
                adjustGLAccountId = journalLineToadjustGLAccount.DebitAccountId;
            }

            return adjustGLAccountId;
        }




    }

    public class WhileStreamingBase
    {
        protected JournalPM _JournalPM;
        protected List<LedgerTransactionPM> _NewLedgerTransactionsWithCounters;
        protected IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        public List<ExternalReconciliationPM> ExternalReconciliationList { get; protected set; }

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider, JournalPM journalPM, List<LedgerTransactionPM> myNewLedgerTransactionsWithCounters)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
            this._JournalPM = journalPM;
            this._NewLedgerTransactionsWithCounters = myNewLedgerTransactionsWithCounters;
        }

    }
}
