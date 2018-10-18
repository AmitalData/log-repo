using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logitude.Accounting.BL;
using System.Transactions;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class EladTester : System.Web.UI.Page
    {


   
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            //NewJournalMethod();
          //  NewBankAccountMethod(); 
            //GLAccountTotalsByMonthBatch.CalcTotalsForLastTwoMonthsInBatch("1-26", 1, 2,DateTime.Now);
            GLAccountEndOfTheYearBatch.GLAccountRevenueExpenseTransfer(1, 2015);
        }

        private void NewBankAccountMethod()
        {


            using (TransactionScope scope = new TransactionScope())
            {
                bool f = true;

                Logitude.Accounting.BL.EntityPMs.BankAccountPM bank = null;

                bank = new Logitude.Accounting.BL.EntityPMs.BankAccountPM()
                {
                    EnglishName = "Pohalim",
                    LocalName = "פועלים",
                    IBAN = "IBAN-671238",
                    CreatedByUserId = "1-1",
                    CreateDate = DateTime.Now,
                    Tenant = 1,
                    BranchNumber = "1003",
                    AccountNumber = "1749244",
                    BankId = "1",
                    GLAccountId = null,
                    DeferredGLAccountId = "1-31",
                    BranchAddress = "Tel Aviv",
                    UpdatedByUserId = "1-1",
                    UpdateDate = DateTime.Now,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                };



                //j.JournalLines = new List<Logitude.Accounting.BL.EntityPMs.JournalLinePM> 
                //    {
                //        new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                //    {
                //      AccountingDate = j.AccountingDate,
                //      ActionName = "1", 
                //      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                //      CreditAccountId = "35",
                //      DebitAccountId = "35",
 
                //      DocumentDate=DateTime.Now, 
                //      DueDate= DateTime.Now, 
                //      ForeignAmount = 25, 
                //      LocalAmount = 100, 
                //      JournalId=j.Id, 
                //      Line=1, 
                //      Tenant=1
                //    },
                //        new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                //    {
                //      AccountingDate = j.AccountingDate,
                //      ActionName = "2", 
                //      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                //      CreditAccountId = "35", 
                //      DebitAccountId = "35",
                //      DocumentDate=DateTime.Now, 
                //      DueDate= DateTime.Now, 
                //      ForeignAmount = 25, 
                //      LocalAmount = 100, 
                //      JournalId=j.Id, 
                //      Line=2, 
                //      Tenant=1
                //    },
                    
                //  new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                //    {
                //      AccountingDate = j.AccountingDate,
                //      ActionName = "4", 
                //      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                //      CreditAccountId = "35", 
                //      DebitAccountId = "35",
                //      DocumentDate=DateTime.Now, 
                //      DueDate= DateTime.Now, 
                //      ForeignAmount = 25, 
                //      LocalAmount = 100, 
                //      JournalId=j.Id, 
                //      Line=3, 
                //      Tenant=1
                //    } 
                //    };


                IAccountingContext MyContext = AccountingContext.GetContext(1);
                
                BankAccountUpdateService service = new BankAccountUpdateService(MyContext, new Dictionary<string, IContext>(),1);

               // var us = new Logitude.Accounting.BL.EntityUpdateServices.BankAccountUpdateService(new BankAccountDomainContext(), 1);
                service.Update(bank, true);
                scope.Complete();

            }
        }

        private static void NewJournalMethod()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                bool f = true;

                Logitude.Accounting.BL.EntityPMs.JournalPM j = null;

                j = new Logitude.Accounting.BL.EntityPMs.JournalPM()
                {
                    AccountingDate = DateTime.Now,
                    Tenant = 1,
                    JournalNumber = "1003",
                    StatusCode = "2",
                    UpdatedByUserId = "1-1",
                    UpdateDate = DateTime.Now,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = "1-1",




                };



                j.JournalLines = new List<Logitude.Accounting.BL.EntityPMs.JournalLinePM> 
                    {
                        new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      ActionName = "1", 
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      CreditAccountId = "35",
                      DebitAccountId = "35",
 
                      DocumentDate=DateTime.Now, 
                      DueDate= DateTime.Now, 
                      ForeignAmount = 25, 
                      LocalAmount = 100, 
                      JournalId=j.Id, 
                      Line=1, 
                      Tenant=1
                    },
                        new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      ActionName = "2", 
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      CreditAccountId = "35", 
                      DebitAccountId = "35",
                      DocumentDate=DateTime.Now, 
                      DueDate= DateTime.Now, 
                      ForeignAmount = 25, 
                      LocalAmount = 100, 
                      JournalId=j.Id, 
                      Line=2, 
                      Tenant=1
                    },
                    
                  new Logitude.Accounting.BL.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      ActionName = "4", 
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      CreditAccountId = "35", 
                      DebitAccountId = "35",
                      DocumentDate=DateTime.Now, 
                      DueDate= DateTime.Now, 
                      ForeignAmount = 25, 
                      LocalAmount = 100, 
                      JournalId=j.Id, 
                      Line=3, 
                      Tenant=1
                    } 
                    };


                var us = new Logitude.Accounting.BL.EntityUpdateServices.JournalUpdateService(1);
                us.Update(j, true);
                scope.Complete();

            }
        }
    }
}