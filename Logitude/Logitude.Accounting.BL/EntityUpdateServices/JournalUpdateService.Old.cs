using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalUpdateService
    {
#if false// good to remember
              private void Case_2(JournalPM entityPM)
        {

            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            localAccountingCurrencyId = tPM.CurrencyId;

            List<LedgerTransactionPM> myLedgerTransactions = new List<LedgerTransactionPM>();
            List<GLAccountTotalByMonthPM> myGLAccountTotalByMonths = new List<GLAccountTotalByMonthPM>();


            foreach (JournalLinePM item in entityPM.JournalLines)
            {

                switch (item.ActionTypeCode)
                {
                    case "1":  //Credit
                        {
                            myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                        }
                        break;
                    case "2": //Debit
                        {
                            myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths));
                        }
                        break;
                    case "3": //Credit & Debit
                        {
                            myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                            myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths));
                        }
                        break;
                    case "4"://Credit, Debit & VAT 
                        {
                            myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                            myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths, true));
                            myLedgerTransactions.Add(TaxTransaction(item, entityPM, myGLAccountTotalByMonths));
                        }
                        break;
                    default:
                        break;
                }

            }


            var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
            myLedgerTransactionUpdateService.UpdateMulti(myLedgerTransactions, new List<LedgerTransactionPM>(), entityPM, false); // was true




        }
       
        protected override void AfterUpdatingOld(JournalPM entityPM, EntityPM entityParentPM)
        {
            string journalOldStatusCode = oldsJournalStatus();

            try
            {

                if (entityPM.StatusCode == "3") //Voided
                {
                    JournalPM Storno = new JournalPM();

                    Storno.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    Storno.AccountingDate = entityPM.AccountingDate;
                    Storno.AccountingEntityCode = entityPM.AccountingEntityCode;
                    //  Storno.AccountingEntityId = entityPM.AccountingEntityId;
                    //Storno.AccountingEntityName = entityPM.AccountingEntityName;
                    Storno.CreateDate = entityPM.CreateDate;
                    Storno.StatusCode = "2";
                    Storno.VoidedBy = true;
                    Storno.CreatedByUserId = entityPM.UpdatedByUserId;
                    Storno.UpdatedByUserId = entityPM.UpdatedByUserId;
                    Storno.Tenant = entityPM.Tenant;
                    Storno.TypeCode = entityPM.TypeCode;
                    Storno.UpdateDate = DateTime.Now;
                    Storno.UpdatedByUserId = entityPM.UpdatedByUserId;
                    Storno.OriginalJournalId = entityPM.Id;

                    foreach (JournalLinePM item in entityPM.JournalLines)
                    {
                        JournalLinePM newStornoJournalLine = new JournalLinePM();
                        newStornoJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        newStornoJournalLine.AccountingDate = item.AccountingDate;
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
                        newStornoJournalLine.Reference1 = item.Reference1;
                        newStornoJournalLine.Reference2 = item.Reference2;
                        newStornoJournalLine.Reference3 = item.Reference3;
                        newStornoJournalLine.Tenant = item.Tenant;


                        Storno.JournalLines.Add(newStornoJournalLine);

                    }

                    var accountingContext = AccountingContext.GetContext(entityPM.Tenant);
                    JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    journalUpdateService.Update(Storno, true);

                }
                else if (entityPM.StatusCode == "2") //Pending Approval  
                {

                    TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
                    TenantPM tPM = tenantQuery.GetSinglePM(entityPM.Tenant);
                    localAccountingCurrencyId = tPM.CurrencyId;

                    List<LedgerTransactionPM> myLedgerTransactions = new List<LedgerTransactionPM>();
                    List<GLAccountTotalByMonthPM> myGLAccountTotalByMonths = new List<GLAccountTotalByMonthPM>();


                    foreach (JournalLinePM item in entityPM.JournalLines)
                    {

                        switch (item.ActionTypeCode)
                        {
                            case "1":  //Credit
                                {
                                    myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                                }
                                break;
                            case "2": //Debit
                                {
                                    myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths));
                                }
                                break;
                            case "3": //Credit & Debit
                                {
                                    myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                                    myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths));
                                }
                                break;
                            case "4"://Credit, Debit & VAT 
                                {
                                    myLedgerTransactions.Add(CreditTransaction(item, entityPM, myGLAccountTotalByMonths));
                                    myLedgerTransactions.Add(DebitTransaction(item, entityPM, myGLAccountTotalByMonths, true));
                                    myLedgerTransactions.Add(TaxTransaction(item, entityPM, myGLAccountTotalByMonths));
                                }
                                break;
                            default:
                                break;
                        }

                    }


                    var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                    myLedgerTransactionUpdateService.UpdateMulti(myLedgerTransactions, new List<LedgerTransactionPM>(), entityPM, true);




                }
            }
            catch (Exception) // return to old values 
            {
                entityPM.StatusCode = journalOldStatusCode;
                throw;
            }


        }

        private GLAccountTotalByMonthPM GLAccountTotalByMonthLine(JournalLinePM journalLinePM, LedgerTransactionPM ledgerTransactionPM)
        {
            GLAccountTotalByMonthPM myGLAccountTotalByMonth = new GLAccountTotalByMonthPM();

            myGLAccountTotalByMonth.AccountId = ledgerTransactionPM.AccountId;
            myGLAccountTotalByMonth.CurrencyId = ledgerTransactionPM.CurrencyId;
            myGLAccountTotalByMonth.CurrencyName = ledgerTransactionPM.CurrencyCode;
            myGLAccountTotalByMonth.Month = ledgerTransactionPM.AccountingDate.Month;
            myGLAccountTotalByMonth.Tenant = ledgerTransactionPM.Tenant;



            switch (journalLinePM.ActionTypeCode)
            {
                case "1":  //Credit
                    {
                        myGLAccountTotalByMonth.ForeignAmountCredit = journalLinePM.ForeignAmount;
                        myGLAccountTotalByMonth.LocalAmountCredit = (decimal)journalLinePM.LocalAmount;
                    }
                    break;
                case "2": //Debit
                    {
                        myGLAccountTotalByMonth.ForeignAmountDebit = journalLinePM.ForeignAmount;
                        myGLAccountTotalByMonth.LocalAmountDebit = (decimal)journalLinePM.LocalAmount;
                    }
                    break;
                case "3": // Debit & Credit 
                    {
                        myGLAccountTotalByMonth.ForeignAmountCredit = journalLinePM.ForeignAmount;
                        myGLAccountTotalByMonth.LocalAmountCredit = (decimal)journalLinePM.LocalAmount;
                    }
                    break;
                default:
                    break;
            }





            return myGLAccountTotalByMonth;

        }

        private LedgerTransactionPM TaxTransaction(JournalLinePM item, JournalPM entityPM, List<GLAccountTotalByMonthPM> myGLAccountTotalByMonths)
        {
            LedgerTransactionPM myTaxTransaction = new LedgerTransactionPM();


                myTaxTransaction.ChangeSetOp = ChangeSetOperation.Insert;
                myTaxTransaction.Id = IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
                myTaxTransaction.Tenant = item.Tenant;
                myTaxTransaction.JournalId = item.JournalId;
                myTaxTransaction.JournalLineNumber = item.Line;
                myTaxTransaction.CreateDate = entityPM.CreateDate;
                myTaxTransaction.ControlAccountId = null;
                myTaxTransaction.AccountId = "1-30"; //Tax Account
               
                myTaxTransaction.AccountingDate = entityPM.AccountingDate;
                myTaxTransaction.DocumentDate = item.DocumentDate;
                myTaxTransaction.DueDate = (DateTime)item.DueDate;
                decimal myVat = Convert.ToDecimal(1.18);
                decimal localAmount = (decimal)item.LocalAmount / myVat;
                decimal foreignAmount = (decimal)item.ForeignAmount / myVat;
                myTaxTransaction.LocalAmountDebit = (decimal)item.LocalAmount - localAmount;
                myTaxTransaction.LocalAmountCredit = 0;
                myTaxTransaction.CurrencyId = item.CurrencyId;

                myTaxTransaction.OppositeAccountId = item.CreditAccountId;
                myTaxTransaction.ForeignAmountDebit = (decimal)item.ForeignAmount - foreignAmount;
                myTaxTransaction.ForeignAmountCredit = 0;
                if (item.ExchangeRate != null)
                {
                    myTaxTransaction.ExchangeRate = (decimal)item.ExchangeRate;
                }
                else
                {
                    myTaxTransaction.ExchangeRate = myTaxTransaction.LocalAmountDebit / myTaxTransaction.ForeignAmountDebit;
                }
                myTaxTransaction.Reference1 = item.Reference1;
                myTaxTransaction.Reference2 = item.Reference2;
                myTaxTransaction.Reference3 = item.Reference3;
                myTaxTransaction.Notes = item.Notes;

                decimal localVat = (decimal)item.LocalAmount - localAmount;
                myTaxTransaction.OpenAmount = -localVat;


                GLAccountTotalByMonthPM mylist = (from a in myGLAccountTotalByMonths
                                                  where a.AccountId == myTaxTransaction.AccountId &&
                                a.CurrencyId == myTaxTransaction.CurrencyId &&
                                a.Year == myTaxTransaction.AccountingDate.Year &&
                                a.Month == myTaxTransaction.AccountingDate.Month
                            select a).FirstOrDefault();

                if (mylist == null)
                {


                    GLAccountTotalByMonthPM myGLAccountTotalByMonth = new GLAccountTotalByMonthPM();
                    myGLAccountTotalByMonth.ChangeSetOp = ChangeSetOperation.Insert;
                    myGLAccountTotalByMonth.AccountId = myTaxTransaction.AccountId;
                    myGLAccountTotalByMonth.CurrencyId = myTaxTransaction.CurrencyId;
                    myGLAccountTotalByMonth.CurrencyName = myTaxTransaction.CurrencyCode;
                    myGLAccountTotalByMonth.Month = myTaxTransaction.AccountingDate.Month;
                    myGLAccountTotalByMonth.Year = myTaxTransaction.AccountingDate.Year;
                    myGLAccountTotalByMonth.Tenant = myTaxTransaction.Tenant;
                    myGLAccountTotalByMonth.ForeignAmountCredit = 0;
                    myGLAccountTotalByMonth.LocalAmountCredit = 0;
                    myGLAccountTotalByMonth.LocalAmountDebit = myTaxTransaction.LocalAmountDebit;
                    myGLAccountTotalByMonth.ForeignAmountDebit = myTaxTransaction.ForeignAmountDebit;

                    myGLAccountTotalByMonths.Add(myGLAccountTotalByMonth);

                }
                else
                {

                    mylist.ForeignAmountDebit += myTaxTransaction.ForeignAmountDebit; ;
                    mylist.LocalAmountDebit += myTaxTransaction.LocalAmountDebit;
                }

                return myTaxTransaction;
        }

        private LedgerTransactionPM DebitTransaction(JournalLinePM item, JournalPM entityPM,  List<GLAccountTotalByMonthPM> myGLAccountTotalByMonths, bool vatExtract = false)
        {

            LedgerTransactionPM myDebitTransaction = new LedgerTransactionPM();

            myDebitTransaction.ChangeSetOp = ChangeSetOperation.Insert;
            myDebitTransaction.Id = IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
           
            myDebitTransaction.Tenant = item.Tenant;
            myDebitTransaction.JournalId = item.JournalId;
            myDebitTransaction.JournalLineNumber = item.Line;
            myDebitTransaction.CreateDate = entityPM.CreateDate;
            // myDebitTransaction.ControlAccountId = item.DebitControlAccountId;

            myDebitTransaction.ControlAccountId = null;
            myDebitTransaction.AccountId = item.DebitAccountId;
            myDebitTransaction.OppositeAccountId = item.CreditAccountId;
            myDebitTransaction.AccountingDate = entityPM.AccountingDate;
            myDebitTransaction.DocumentDate = item.DocumentDate;
            myDebitTransaction.DueDate = (DateTime)item.DueDate;


            decimal myVat = Convert.ToDecimal(1.18);
            decimal localAmountWithoutVat = (decimal)item.LocalAmount / myVat;
            decimal foreignAmountWithoutVat = (decimal)item.ForeignAmount / myVat;

            if (vatExtract)
            {


                myDebitTransaction.LocalAmountDebit = localAmountWithoutVat;
                myDebitTransaction.ForeignAmountDebit = foreignAmountWithoutVat;
              
            }
            else
            {
                myDebitTransaction.LocalAmountDebit = (decimal)item.LocalAmount;
                myDebitTransaction.ForeignAmountDebit = (decimal)item.ForeignAmount;
               
          
            }
          
            myDebitTransaction.LocalAmountCredit = 0;
            myDebitTransaction.CurrencyId = item.CurrencyId;
           
            myDebitTransaction.ForeignAmountCredit = 0;
            if (item.ExchangeRate != null)
            {
                myDebitTransaction.ExchangeRate = (decimal)item.ExchangeRate;
            }
            else
            {
                myDebitTransaction.ExchangeRate = myDebitTransaction.LocalAmountDebit / myDebitTransaction.ForeignAmountDebit;
            }
            myDebitTransaction.Reference1 = item.Reference1;
            myDebitTransaction.Reference2 = item.Reference2;
            myDebitTransaction.Reference3 = item.Reference3;
            myDebitTransaction.Notes = item.Notes;
          

            if (!string.IsNullOrWhiteSpace(item.DebitAccountId))
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(item.Tenant);
                GLAccountPM parent = gLAccountQueryService.GetSingle(item.DebitAccountId, false, false);


                if ((!string.IsNullOrWhiteSpace(parent.AccountTypeCode)) && (parent.AccountTypeCode != "1"))
                {
                    myDebitTransaction.ControlAccountId = item.DebitControlAccountId;
                }
                else
                {
                    myDebitTransaction.ControlAccountId = null;
                }

                if (vatExtract)
                {

                    if (parent.ReconcileMethodCode == "0") //Local Currency
                    {
                        
                        myDebitTransaction.OpenAmount = -localAmountWithoutVat;
                        myDebitTransaction.OpenAmountCurrencyId = localAccountingCurrencyId;
                    }
                    else
                    {
                        myDebitTransaction.OpenAmount = -foreignAmountWithoutVat;
                         myDebitTransaction.OpenAmountCurrencyId = item.CurrencyId;
                    }
                }
                else
                {

                    if (parent.ReconcileMethodCode == "0") //Local Currency
                    {
                        myDebitTransaction.OpenAmount = -(decimal)item.LocalAmount;
                        myDebitTransaction.OpenAmountCurrencyId = localAccountingCurrencyId;
                    }
                    else
                    {
                        myDebitTransaction.OpenAmount = -(decimal)item.ForeignAmount;
                         myDebitTransaction.OpenAmountCurrencyId = item.CurrencyId;
                    }
                }
            }


              GLAccountTotalByMonthPM mylist = (from a in myGLAccountTotalByMonths
                          where a.AccountId == myDebitTransaction.AccountId && 
                                a.CurrencyId == myDebitTransaction.CurrencyId && 
                                a.Year == myDebitTransaction.AccountingDate.Year &&
                                a.Month == myDebitTransaction.AccountingDate.Month
                            select a).FirstOrDefault();

              if (mylist == null)
              {

                  GLAccountTotalByMonthPM myGLAccountTotalByMonth = new GLAccountTotalByMonthPM();
                  myGLAccountTotalByMonth.ChangeSetOp = ChangeSetOperation.Insert;
                  myGLAccountTotalByMonth.AccountId = myDebitTransaction.AccountId;
                  myGLAccountTotalByMonth.CurrencyId = myDebitTransaction.CurrencyId;
                  myGLAccountTotalByMonth.CurrencyName = myDebitTransaction.CurrencyCode;
                  myGLAccountTotalByMonth.Year = myDebitTransaction.AccountingDate.Year;
                  myGLAccountTotalByMonth.Month = myDebitTransaction.AccountingDate.Month;
                  myGLAccountTotalByMonth.Tenant = myDebitTransaction.Tenant;
                  myGLAccountTotalByMonth.ForeignAmountDebit = myDebitTransaction.ForeignAmountDebit;
                  myGLAccountTotalByMonth.LocalAmountDebit = myDebitTransaction.LocalAmountDebit;
                  myGLAccountTotalByMonth.ForeignAmountCredit = 0;
                  myGLAccountTotalByMonth.LocalAmountCredit = 0;

                  myGLAccountTotalByMonths.Add(myGLAccountTotalByMonth);
              }
              else
              {

                  mylist.ForeignAmountDebit += myDebitTransaction.ForeignAmountDebit;
                  mylist.LocalAmountDebit += myDebitTransaction.LocalAmountDebit;    
              }



            return myDebitTransaction;
        }

        private LedgerTransactionPM CreditTransaction(JournalLinePM item, JournalPM entityPM,  List<GLAccountTotalByMonthPM> myGLAccountTotalByMonths)
        {
            LedgerTransactionPM myCreditTransaction = new LedgerTransactionPM();

            myCreditTransaction.ChangeSetOp = ChangeSetOperation.Insert;
            myCreditTransaction.Id = IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
            myCreditTransaction.Tenant = item.Tenant;
            myCreditTransaction.JournalId = item.JournalId;
            myCreditTransaction.JournalLineNumber = item.Line;
            myCreditTransaction.CreateDate = entityPM.CreateDate;
            myCreditTransaction.AccountId = item.CreditAccountId;
            myCreditTransaction.OppositeAccountId = item.DebitAccountId;
            myCreditTransaction.AccountingDate = entityPM.AccountingDate;
            myCreditTransaction.DocumentDate = item.DocumentDate;
            myCreditTransaction.DueDate = (DateTime)item.DueDate;
            myCreditTransaction.LocalAmountDebit = 0;
            myCreditTransaction.LocalAmountCredit = (decimal)item.LocalAmount;
            myCreditTransaction.CurrencyId = item.CurrencyId;
            myCreditTransaction.ForeignAmountDebit = 0;
            myCreditTransaction.ForeignAmountCredit = (decimal)item.ForeignAmount;
            if (item.ExchangeRate != null)
            {
                myCreditTransaction.ExchangeRate = (decimal)item.ExchangeRate;
            }
            else
            {
                myCreditTransaction.ExchangeRate = myCreditTransaction.LocalAmountCredit / myCreditTransaction.ForeignAmountCredit;
            }
            myCreditTransaction.Reference1 = item.Reference1;
            myCreditTransaction.Reference2 = item.Reference2;
            myCreditTransaction.Reference3 = item.Reference3;
            myCreditTransaction.Notes = item.Notes;


          
            if (!string.IsNullOrWhiteSpace(item.CreditAccountId))
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(item.Tenant);
                GLAccountPM parent = gLAccountQueryService.GetSingle(item.CreditAccountId, false, false);


                if ((!string.IsNullOrWhiteSpace(parent.AccountTypeCode)) && (parent.AccountTypeCode != "1"))
                {
                    myCreditTransaction.ControlAccountId = item.CreditControlAccountId;
                }
                else
                {
                    myCreditTransaction.ControlAccountId = null;
                }
                if (parent.ReconcileMethodCode == "0") //Local Currency
                {
                    myCreditTransaction.OpenAmount = (decimal)item.LocalAmount;
                    myCreditTransaction.OpenAmountCurrencyId = localAccountingCurrencyId;
                }
                else
                {
                    myCreditTransaction.OpenAmount = (decimal)item.ForeignAmount;
                    myCreditTransaction.OpenAmountCurrencyId = item.CurrencyId;
                }

            }

            GLAccountTotalByMonthPM mylist = (from a in myGLAccountTotalByMonths
                                              where a.AccountId == myCreditTransaction.AccountId &&
                                                    a.CurrencyId == myCreditTransaction.CurrencyId &&
                                                    a.Year == myCreditTransaction.AccountingDate.Year &&
                                                    a.Month == myCreditTransaction.AccountingDate.Month
                                              select a).FirstOrDefault();

            if (mylist == null)
            {


                GLAccountTotalByMonthPM myGLAccountTotalByMonth = new GLAccountTotalByMonthPM();
                myGLAccountTotalByMonth.ChangeSetOp = ChangeSetOperation.Insert;
                myGLAccountTotalByMonth.AccountId = myCreditTransaction.AccountId;
                myGLAccountTotalByMonth.CurrencyId = myCreditTransaction.CurrencyId;
                myGLAccountTotalByMonth.CurrencyName = myCreditTransaction.CurrencyCode;
                myGLAccountTotalByMonth.Year = myCreditTransaction.AccountingDate.Year;
                myGLAccountTotalByMonth.Month = myCreditTransaction.AccountingDate.Month;
                myGLAccountTotalByMonth.Tenant = myCreditTransaction.Tenant;
                myGLAccountTotalByMonth.ForeignAmountCredit = myCreditTransaction.ForeignAmountCredit;
                myGLAccountTotalByMonth.LocalAmountCredit = myCreditTransaction.LocalAmountCredit;
                myGLAccountTotalByMonth.LocalAmountDebit = 0;
                myGLAccountTotalByMonth.ForeignAmountDebit = 0;

                myGLAccountTotalByMonths.Add(myGLAccountTotalByMonth);
            }
            else 
            {
                mylist.ForeignAmountCredit += myCreditTransaction.ForeignAmountCredit;
                mylist.LocalAmountCredit += myCreditTransaction.LocalAmountCredit;
            }

            return myCreditTransaction;
        }
#endif
    }
}
