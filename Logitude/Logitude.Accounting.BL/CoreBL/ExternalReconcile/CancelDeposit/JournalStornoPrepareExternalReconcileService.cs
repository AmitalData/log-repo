using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile.CancelDeposit
{
    public class JournalStornoPrepareExternalReconcileService : IJournalStornoPrepareExternalReconcileService
    {
        private readonly IAccountingContext _AccountingContext;
        private readonly JournalPM _JournalToVoidPM;



        public JournalExternalReconcilePM JournalExternalReconcilePM { get; private set; }

        public JournalStornoPrepareExternalReconcileService(IAccountingContext accountingContext,
            JournalPM journalToVoidPM)
        {
            this._AccountingContext = accountingContext;
            this._JournalToVoidPM = journalToVoidPM;
        }
        private bool IsStornoJournal(JournalPM theStorno)
        {
            if (String.IsNullOrWhiteSpace(theStorno.OriginalJournalId))
            {
                return false;
            }

            if (theStorno.OriginalJournalId != _JournalToVoidPM.Id)
            {
                return false;
            }
            return true;
        }
        private List<LedgerTransactionPM> FetchlTransactionOfOriginalJournal(string OriginalJournalId, int Tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);
            var orginalJornalLedgerTransactions = qs.GetByJournalId(OriginalJournalId, Tenant);
            return orginalJornalLedgerTransactions;
        }



        public bool CreateJournalExternalReconcileFromStorno(JournalPM theStorno)
        {
            const string depositAccountingEntityCode = "6";
            if (
                theStorno.AccountingEntityCode != depositAccountingEntityCode
                ||
                this._JournalToVoidPM.AccountingEntityCode != depositAccountingEntityCode
                )
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("no need to create AccountingEntityCode is not Bank Deposit");
                return false;
            }

            if (!IsStornoJournal(theStorno))
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("The original void and the storno have not been properly initalized.");
                return false;
            }
            var orginalJornalLedgerTransactions = FetchlTransactionOfOriginalJournal(_JournalToVoidPM.Id, _JournalToVoidPM.Tenant);

           
            var journalLineDebitBank = _JournalToVoidPM.JournalLines.Where(r => r.ActionTypeCodeEnum == JournalActionTypeEnum.Debit).First();
            var theStornoDebitLines = theStorno.JournalLines.Where(r => r.ActionTypeCodeEnum == JournalActionTypeEnum.Debit).ToList();
            var theVoidDebitLines = _JournalToVoidPM.JournalLines.Where(r => r.ActionTypeCodeEnum == JournalActionTypeEnum.Debit).ToList();
          
            if (
                theStornoDebitLines.First().DebitAccountId 
                !=
                theVoidDebitLines.First().DebitAccountId)
            {
                throw new ApplicationException("DebitAccountId have to be the same ");
                return false;
            }
            BankAccountPM bankAccount =
                GetBankAccountByGLAccountId(theStornoDebitLines.First().DebitAccountId, theStorno.Tenant);
            if (bankAccount == null)
            {
                throw new ApplicationException("We need bank account id inorder to set in ExternalReconciliation !!");
                
                return false;

            }


            if (
                 Math.Abs(theStornoDebitLines.First().LocalAmount + theVoidDebitLines.First().LocalAmount) != 0
                 ||
                 Math.Abs(theStornoDebitLines.First().ForeignAmount + theVoidDebitLines.First().ForeignAmount) != 0

                 )
            {
                throw new ApplicationException("storno and voided are not balanced");
                return false;
            }

            var ledgerTransactionDebitBank = orginalJornalLedgerTransactions.Where(r => r.AccountId == journalLineDebitBank.DebitAccountId).FirstOrDefault();

            if (ledgerTransactionDebitBank == null)
            {
                throw new ApplicationException("ledgerTransactionDebitBank FROM orginalJornalLedgerTransactions NOT FOUND ");
                return false;// did not stream to Accounting !!
            }
            if (ledgerTransactionDebitBank.IsExternalReconcile)
            {
                throw new ApplicationException("the old ledgerTransaction that DebitBank  is already ExternalReconcile");
                return false;

            }
            if (ledgerTransactionDebitBank.InProgressExternalReconcile)
            {
                throw new ApplicationException("the old ledgerTransaction that DebitBank  is InProgressExternalReconcile");
                return false;
            }
            

            this.JournalExternalReconcilePM =
                 new JournalExternalReconcilePM()
                 {
                     ChangeSetOp = ChangeSetOperation.Insert,
                     JournalId = "up.OnCreate",
                     LedgerTransactionId = ledgerTransactionDebitBank.Id,
                     Line = 1,
                     Tenant = _JournalToVoidPM.Tenant,

                 };

            return true;
        }

        private BankAccountPM GetBankAccountByGLAccountId(string debitAccountId, int tenant)
        {
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccount = bankAccountQS.GetByGLAccountId(debitAccountId, tenant);
            return bankAccount;
        }
    }


    public class JournalStornoPrepareExternalReconcileServiceTests
    {
        
        public void CreateJournalExternalReconcileList_ReturnsExpectedResult()
        {
            // Arrange
            IAccountingContext accountingContextMock = null;//new Mock<IAccountingContext>();
            var journalToVoidPM = new JournalPM
            {
                Id = "journalToVoidPMId",
                Tenant = 1,
                AccountingEntityCode = "6",
                JournalLines = new List<JournalLinePM>
            {
                new JournalLinePM
                {
                    ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                    DebitAccountId = "debitAccountId"
                }
            }
            };
            var service = new JournalStornoPrepareExternalReconcileService(accountingContextMock, journalToVoidPM);
            var myOrginalJournalTransaction = new List<LedgerTransactionPM>
        {
            new LedgerTransactionPM { Id = "ledgerTransactionId1", AccountId = "debitAccountId", JournalNumber = "1", JournalLineNumber = 1 },
            new LedgerTransactionPM { Id = "ledgerTransactionId2", AccountId = "creditAccountId", JournalNumber = "1", JournalLineNumber = 2 },
            new LedgerTransactionPM { Id = "ledgerTransactionId3", AccountId = "debitAccountId", JournalNumber = "2", JournalLineNumber = 1 }
        };

            // Act
            var result = service.CreateJournalExternalReconcileFromStorno(null);

            // Assert
            //Assert.NotNull(result);
            //Assert.Equal(2, result.Count);

            //var firstItem = result.FirstOrDefault();
            //Assert.NotNull(firstItem);
            //Assert.Equal(ChangeSetOperation.Insert, firstItem.ChangeSetOp);
            //Assert.Equal("up.OnCreate", firstItem.JournalId);
            //Assert.Equal("ledgerTransactionId1", firstItem.LedgerTransactionId);
            //Assert.Equal(1, firstItem.Line);
            //Assert.Equal(1, firstItem.Tenant);

            //var secondItem = result.Skip(1).FirstOrDefault();
            //Assert.NotNull(secondItem);
            //Assert.Equal(ChangeSetOperation.Insert, secondItem.ChangeSetOp);
            //Assert.Equal("up.OnCreate", secondItem.JournalId);
            //Assert.Equal("ledgerTransactionId3", secondItem.LedgerTransactionId);
            //Assert.Equal(2, secondItem.Line);
            //Assert.Equal(1, secondItem.Tenant);
        }
    }

}

