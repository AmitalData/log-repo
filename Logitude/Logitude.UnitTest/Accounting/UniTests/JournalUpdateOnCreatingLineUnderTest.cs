using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using Logitude.Accounting.BL.Validators;
using Logitude.Server.Tools.Helpers;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.UnitTest.Utils;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.UnitTest.Accounting.UniTests
{

    
    [TestClass]
    public class JournalUpdateOnCreatingLineUnderTest
    {

        [TestMethod]
        public void OnCreate_JournalPMPropertyIdTenantSupressJournalLinePMPropreties()
        {
            int tenant = 1;

            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id="1-3" 
            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",
            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke
            Assert.AreEqual(journalPM.Tenant, journalLinePM.Tenant);
            Assert.AreEqual(journalPM.Id, journalLinePM.JournalId);

        }


        [TestMethod]
        public void OnCreate_AccountingDateTimeTruncate2Date()
        {
            int tenant = 1;

            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var accDatetime =new DateTime(2017,01,12,12,35,11);
            var journalLinePM = new JournalLinePM()
            {
                AccountingDate=accDatetime ,
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",

                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke
            var excpted=new DateTime(2017, 01, 12);
            Assert.AreEqual(excpted, journalLinePM.AccountingDate);
            
        }



        [TestMethod]
        public void OnCreate_EnsureAllDecimalPrecisionIfChangeChangeUpdateHappend()
        {
            int tenant = 1;

            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"
                
            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",

                LocalAmount=10.020202m,
                ForeignAmount = 10.020201m,

            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.LocalAmount, 10.02m);
            Assert.AreEqual(journalLinePM.ForeignAmount, 10.02m);
            Assert.AreEqual(journalLinePM.ExchangeRate, 1);
        }



        [TestMethod]
        public void OnCreate_ForeignAmountIsZero()
        {
            int tenant = 1;

            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",

                LocalAmount = 10.020202m,
                ForeignAmount = 0m,

            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(10.02m ,journalLinePM.LocalAmount);
            Assert.AreEqual(0m ,journalLinePM.ForeignAmount);
            Assert.AreEqual(0m, journalLinePM.ExchangeRate);
        }

        [TestMethod]
        public void OnCreate_CurrencyCodeTranslateToID()
        {
            int tenant=1;

            //arrange
            var journalPM= new JournalPM() 
                {
                    Tenant=tenant
                };
            var journalLinePM= new JournalLinePM() {
                Tenant=2,
                CurrencyCode="USD",
                CurrencyId="",
            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext >();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke
            Assert.AreEqual(currencyPM.Id, journalLinePM.CurrencyId);

        }


        [TestMethod]
        public void OnCreate_RemoveNewLine()
        {
            int tenant = 1;

            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant
            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",
                Notes = "123" + Environment.NewLine + "456",
            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke
            Assert.AreEqual(journalLinePM.Notes, "123" + " " /*Environment.NewLine*/ + "456");

        }

        [TestMethod]
        public void OnCreateRange_BadGLAccountId_debitDueVat_AccountIdBecomeNUll()
        {
            int tenant = 1;
            var badCard="badCard!!!";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",
                 ActionTypeCodeEnum =  MyJournalActionTypeEnum.DebitAndCredit,
                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

                CreditAccountNumber = "CreditAccountNumber",
                DebitAccountNumber = "DebitAccountNumber",
                CreditAccountId = badCard,
                DebitAccountId = badCard,

            };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );
            VerifyGLAccountManager mVerifyGLAccountManager = A.Fake<VerifyGLAccountManager>(opt => opt.CallsBaseMethods());

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(badCard))
                .Returns(null);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, A<string>.Ignored))
                .Returns(null);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { VATOutputGLAccountId = "Vat!!" });


            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetIVerifyGLAccountManager())
                .Returns(mVerifyGLAccountManager);
                
            
            


            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);

            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id = "",
                Tenant = tenant,
                Code = MyJournalActionTypeEnum.DebitAndCredit.ToString(),
                EnglishName = "Debit",


                //1	????	Credit
            };
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                            .Returns(myJournalActionTypeList);


            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.CreditAccountId, null);
            Assert.AreEqual(journalLinePM.DebitAccountId, null);
            
        }

        [TestMethod]
        public void OnCreateRange_BadGLAccountIdWithGoodAccountNumberCardType_AccountIdTranslated()
        {
            int tenant = 1;
            var badCard = "badCard!!!";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",
                 ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitAndCredit,
                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

                CreditAccountNumber = "CreditAccountNumber",
                DebitAccountNumber = "DebitAccountNumber",
                CreditAccountId = badCard,
                DebitAccountId = badCard,

            };
            var CreditAccount = new GLAccountPM() { Id = "1-1", Tenant = tenant, InternalNumber = "CreditAccountNumber", AccountTypeCode = "1" };
            var DebitAccount = new GLAccountPM() { Id = "1-2", Tenant = tenant, InternalNumber = "DebitAccountNumber", AccountTypeCode="1" };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );
            VerifyGLAccountManager mVerifyGLAccountManager = A.Fake<VerifyGLAccountManager>(opt => opt.CallsBaseMethods());

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(badCard))
                .Returns(null);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccount.Id))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(DebitAccount.Id))
                .Returns(DebitAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.CreditAccountNumber))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.DebitAccountNumber))
                .Returns(DebitAccount);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetIVerifyGLAccountManager())
                .Returns(mVerifyGLAccountManager);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });


            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id = "",
                Tenant = tenant,
                Code = MyJournalActionTypeEnum.DebitAndCredit.ToString(),
                EnglishName = "Debit",


                //1	זכות	Credit
            };
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                            .Returns(myJournalActionTypeList);

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.CreditAccountId, CreditAccount.Id);
            Assert.AreEqual(journalLinePM.DebitAccountId, DebitAccount.Id);

        }


        [TestMethod]
        public void OnCreateRange_BadGLAccountIdWithGoodAccountNumberCardTypeIsMultiCredit_AccountIdUSDSplitTranslated()
        {
            int tenant = 1;
            var badCard = "badCard!!!";
            var expectedCreditCardID = "1-1-usd";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "1-1",
                 ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

                CreditAccountNumber = "CreditAccountNumber",
                DebitAccountNumber = "DebitAccountNumber",
                CreditAccountId = badCard,
                DebitAccountId = badCard,

            };
            var CreditAccount = new GLAccountPM() { Id = "1-1", Tenant = tenant, InternalNumber = "CreditAccountNumber", AccountTypeCode = "1" , IsMultiCurrency= true };

            var CreditAccountUSD = new GLAccountPM() { Id = expectedCreditCardID, Tenant = tenant, InternalNumber = "CreditAccountNumberUSD", AccountTypeCode = "1"};
            var DebitAccount = new GLAccountPM() { Id = "1-2", Tenant = tenant, InternalNumber = "DebitAccountNumber", AccountTypeCode = "1" };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );
            var mVerifyGLAccountManager = 
                A.Fake<VerifyGLAccountManager>(opt => opt.CallsBaseMethods());

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(badCard))
                .Returns(null);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccount.Id))
                .Returns(CreditAccount);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccountUSD.Id))
               .Returns(CreditAccountUSD);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });


            A.CallTo(() => mVerifyGLAccountManager
            .GetRelatedCurrenciesAccount(CreditAccount.Id, 1))
            .Returns(
                new List<GLAccountCurrencyPM>() {
                    new GLAccountCurrencyPM() {  CurrencyId =  currencyPM.Id, GLAccountId = expectedCreditCardID }
                });



            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(DebitAccount.Id))
                .Returns(DebitAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.CreditAccountNumber))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.DebitAccountNumber))
                .Returns(DebitAccount);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetIVerifyGLAccountManager())
                .Returns(mVerifyGLAccountManager);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });



            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id = "",
                Tenant = tenant,
                Code = MyJournalActionTypeEnum.Credit.ToString(),
                EnglishName = "Credit" ,
                 
                 
                //1	זכות	Credit
            };
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                            .Returns(myJournalActionTypeList);

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.CreditAccountId, CreditAccountUSD.Id);
            Assert.AreEqual(journalLinePM.ChangeSetOp,  Simplog.Server.Infrastructure.ChangeSetOperation.Update);
            Assert.AreEqual(journalLinePM.DebitAccountId, badCard);

        }

        public void OnCreateRange_BadGLAccountIdWithGoodAccountNumberCardTypeIsMultiDebit_AccountIdUSDSplitTranslated()
        {
            int tenant = 1;
            var badCard = "badCard!!!";
            var expectedCreditCardID = "1-1-usd";
            var expectedDeditCardID = "1-2-usd";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "1-1",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

                CreditAccountNumber = "CreditAccountNumber",
                DebitAccountNumber = "DebitAccountNumber",
                CreditAccountId = badCard,
                DebitAccountId = badCard,

            };
            var CreditAccount = new GLAccountPM() { Id = "1-1", Tenant = tenant, InternalNumber = "CreditAccountNumber", AccountTypeCode = "1", IsMultiCurrency = true };

            var CreditAccountUSD = new GLAccountPM() { Id = expectedCreditCardID, Tenant = tenant, InternalNumber = "CreditAccountNumberUSD", AccountTypeCode = "1" };
            var DebitAccount = new GLAccountPM() { Id = "1-2", Tenant = tenant, InternalNumber = "DebitAccountNumber", AccountTypeCode = "1" };
            var DebitAccountUSD = new GLAccountPM() { Id = expectedDeditCardID, Tenant = tenant, InternalNumber = "expectedDeditUSD", AccountTypeCode = "1" };
            
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );
            var mVerifyGLAccountManager =
                A.Fake<VerifyGLAccountManager>(opt => opt.CallsBaseMethods());

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(badCard))
                .Returns(null);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccount.Id))
                .Returns(CreditAccount);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccountUSD.Id))
               .Returns(CreditAccountUSD);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });


            A.CallTo(() => mVerifyGLAccountManager
            .GetRelatedCurrenciesAccount(CreditAccount.Id, 1))
            .Returns(
                new List<GLAccountCurrencyPM>() {
                    new GLAccountCurrencyPM() {  CurrencyId =  currencyPM.Id, GLAccountId = expectedCreditCardID }
                });



            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(DebitAccount.Id))
                .Returns(DebitAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.CreditAccountNumber))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.DebitAccountNumber))
                .Returns(DebitAccount);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetIVerifyGLAccountManager())
                .Returns(mVerifyGLAccountManager);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });



            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id = "",
                Tenant = tenant,
                Code = MyJournalActionTypeEnum.Debit.ToString(),
                EnglishName = "Debit",


                //1	זכות	Credit
            };
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                            .Returns(myJournalActionTypeList);

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.DebitAccountId, expectedDeditCardID);
            Assert.AreEqual(journalLinePM.ChangeSetOp, Simplog.Server.Infrastructure.ChangeSetOperation.Update);
            Assert.AreEqual(journalLinePM.CreditAccountId, badCard);

        }




        [TestMethod]
        public void OnCreateRange_BadGLAccountIdWithGoodAccountNumberNotCardType_AccountIdControlTranslated()
        {
            int tenant = 1;
            var badCard = "badCard!!!";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = 2,
                CurrencyCode = "USD",
                CurrencyId = "",
                 ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitAndCredit,
                LocalAmount = 10.020202m,
                ForeignAmount = 10.020201m,

                CreditAccountNumber = "CreditAccountNumber",
                DebitAccountNumber = "DebitAccountNumber",
                CreditAccountId = badCard,
                DebitAccountId = badCard,

            };
            var CreditAccount = new GLAccountPM() { Id = "1-1", Tenant = tenant, InternalNumber = "CreditAccountNumber", AccountTypeCode = "2", ControlAccountId = "CreditControlAccountId" };
            var DebitAccount = new GLAccountPM() { Id = "1-2", Tenant = tenant, InternalNumber = "DebitAccountNumber", AccountTypeCode = "3", ControlAccountId = "DebitControlAccountId" };
            var currencyPM = new CurrencyPM() { Id = "1-1", Code = "USD", };
            IAccountingContext mainContext = A.Fake<IAccountingContext>();

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );
            VerifyGLAccountManager mVerifyGLAccountManager = A.Fake<VerifyGLAccountManager>(opt => opt.CallsBaseMethods());

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(badCard))
                .Returns(null);

            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(CreditAccount.Id))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetSingleGLAccount(DebitAccount.Id))
                .Returns(DebitAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.CreditAccountNumber))
                .Returns(CreditAccount);
            A.CallTo(() => mVerifyGLAccountManager.GetByInternalNumberGLAccount(tenant, journalLinePM.DebitAccountNumber))
                .Returns(DebitAccount);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetIVerifyGLAccountManager())
                .Returns(mVerifyGLAccountManager);

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.GetSingleCurrencyByCode(1, "USD"))
                .Returns(currencyPM);
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });



            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id = "",
                Tenant = tenant,
                Code = MyJournalActionTypeEnum.DebitAndCredit.ToString(),
                EnglishName = "Debit",


                //1	זכות	Credit
            };
            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                            .Returns(myJournalActionTypeList);


            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///checke

            Assert.AreEqual(journalLinePM.CreditAccountId, CreditAccount.Id);
            Assert.AreEqual(journalLinePM.CreditControlAccountId, CreditAccount.ControlAccountId);
            Assert.AreEqual(journalLinePM.DebitAccountId, DebitAccount.Id);
            Assert.AreEqual(journalLinePM.DebitControlAccountId, DebitAccount.ControlAccountId);

        }


        [TestMethod]
        public void OnCreateRange_ActionCodeIs1ActionTypeCodeIsNull_FillJournalActionType()
        {
            int tenant = 1;
            var ActionTypeID = "ActionTypeID";
            var ActionTypeCode = "ActionTypeCode";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                Id = "1-3"

            };
            var journalLinePM = new JournalLinePM()
            {
                Tenant = tenant,
                CurrencyId = "",
                ActionTypeCode = ActionTypeCode,
            };
            var myJournalActionTypeList = new JournalActionTypeList()
            {
                Id=ActionTypeID,
                Tenant = tenant,
                Code = ActionTypeCode,
                EnglishName ="Credit"
                //1	זכות	Credit
            };
            
            

            IJournalActionTypeListQueryService myIJournalActionTypeListQueryService =
                A.Fake<IJournalActionTypeListQueryService>();

            var fakeJournalUpdateOnCreatingLine = A.Fake<JournalLineOnUpdate>(
                option => option.CallsBaseMethods()
                    );

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.getFullAccountingSettings(tenant))
                .Returns(new FullAccountingSettingPM() { });

            A.CallTo(() => fakeJournalUpdateOnCreatingLine.JournalActionTypeListGetByCode(journalLinePM))
                .Returns(myJournalActionTypeList);

            fakeJournalUpdateOnCreatingLine.OnUpdate(journalLinePM, journalPM);


            ///check
            Assert.AreEqual(journalLinePM.ActionTypeCode, myJournalActionTypeList.Code);
            //Assert.AreEqual(journalLinePM.ActionCode, myJournalActionTypeList.Id);
            Assert.AreEqual(journalLinePM.ActionId, myJournalActionTypeList.Id);
            //Assert.AreEqual(journalLinePM.ActionId, myJournalActionTypeList.N);


        }
  
    }
 
}
