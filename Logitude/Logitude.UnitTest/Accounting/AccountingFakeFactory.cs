using FakeItEasy;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.UnitTest.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;

namespace Logitude.UnitTest.Accounting
{
    public class AccountingFakeFactory
    {
       

        public IAccountingContext CreateFakeIAccountingContext(
            List<Journal> pocoJournalList,
            List<JournalLine> pocoJournalLines,
            List<GLAccount> pocoGLAccountList
            )
        {
            pocoJournalList = pocoJournalList ?? new List<Journal>();
            pocoJournalLines = pocoJournalLines ?? new List<JournalLine>();

            pocoGLAccountList = pocoGLAccountList ?? new List<GLAccount>();
            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();
            A.CallTo(() => fakeIAccountingContext.Journals)
                .Returns(new MockObjectSet<Journal>(pocoJournalList));
            ;
            A.CallTo(() => fakeIAccountingContext.JournalLines)
                .Returns(new MockObjectSet<JournalLine>(pocoJournalLines));

            A.CallTo(() => fakeIAccountingContext.GLAccounts)
                .Returns(new MockObjectSet<GLAccount>(pocoGLAccountList));

            var JournalActionTypeList = new List<JournalActionType>()
            {
                new JournalActionType() { Tenant=1, Code="1", Id="1", EnglishName ="Credit" },
                new JournalActionType() { Tenant=1, Code="2", Id="2", EnglishName ="Debit " },
                new JournalActionType() { Tenant=1, Code="3", Id="3", EnglishName ="Debit And Credit" },
         //     new JournalActionType() { Tenant=1, Code="4", Id="4", EnglishName ="Debit, Credit And Vat deduction " },
            };
            A.CallTo(() => fakeIAccountingContext.JournalActionTypes)
                .Returns(new MockObjectSet<JournalActionType>(JournalActionTypeList));
            return fakeIAccountingContext;
        }

        public JournalUpdateService CreateFakeJournalUpdateService(int tenant, IAccountingContext fakeIAccountingContext, bool SuppressValidate = false)
        {

            var logitudeServerToolsFactory = new LogitudeServerToolsFactory();
            var simplogDataFakeFactory = new SimplogDataFakeFactory();
            
            var fakeFactory = new FakeFactory();
            var fakeObjectTableRepository = fakeFactory.Register<IObjectTableRepository>(
                simplogDataFakeFactory.IObjectTableRepository_ReturnObjectTable_Id1
            );




            int journalCounter = 1;
            fakeFactory.Register<IIdCounter>((fake) =>
                {
                    //A.CallTo(fake).WithReturnType<string>().Returns("newJournalId");
                    A.CallTo(() => fake.GetNumber(JournalUpdateOnCreating.GetNumberJournal(), 1)).Returns((journalCounter++).ToString());
                });

            IActivityLogger myIActivityLogger = fakeFactory.Register<IActivityLogger>(null);

            int myCodeCounter = 1;
            var codeNumberJournal = JournalUpdateOnCreating.GetCodeNumberJournal();
            fakeFactory.Register<ICodeCounter>((fake) =>
            {
                //A.CallTo(fake).WithReturnType<string>().Returns("newJournalId");
                A.CallTo(() => fake.GetNumber(JournalUpdateOnCreating.GetCodeNumberJournal(), 1))
                    .Returns(myCodeCounter++);
            });

            var loggedContact = new Contact() { Id="loggedContact " };
            fakeFactory.Register<IContactRepository>((fake) =>
            {
                A.CallTo(fake)
                    .WithReturnType<Contact>()
                    .Returns<Contact>(loggedContact);
            });

            var usdCurrencyPM = new CurrencyPM() { Id="01" };
            var eurCurrencyPM = new CurrencyPM() { Id = "02" };
            var nisCurrencyPM = new CurrencyPM() { Id = "18" };
            fakeFactory.Register<ICurrencyQuery>((fake) =>
            {
                A.CallTo(() => fake.GetSingleCurrencyByCode("USD",1))
                    .Returns<CurrencyPM>(usdCurrencyPM);
            
                A.CallTo(() => fake.GetSingleCurrencyByCode("NIS", 1))
                    .Returns<CurrencyPM>(nisCurrencyPM);
            });

ContainerAccessor.Container.ResolveSafe<IGLAccountQueryService>();

var GLAccountsList = new List<GLAccountPM>(){
 new   GLAccountPM()  { Id="1" , ControlAccountId="1" } 
};
fakeFactory.Register<IGLAccountQueryService>((fake) =>
{
    A.CallTo(fake)
        .WithReturnType<List<GLAccountPM>>()
        .Returns(GLAccountsList);
});
    

            var fakeGLAccountQueryService = A.Fake<GLAccountQueryService>();
            // fakeGLAccountQueryService.GetSinglePM
            int IdCounterGeNumber = 0;
            var fakeIIdCounter = A.Fake<IIdCounter>();
            A.CallTo(fakeIIdCounter)
                .Where(call => call.Method.Name == "GetNumber")
                .WithReturnType<string>()
                .ReturnsLazily(() =>
                {
                    var ret = IdCounterGeNumber++;
                    return ret.ToString();
                }
                );
            ContainerAccessor.Container.RegisterInstance<IIdCounter>(fakeIIdCounter);







            var fakeJournalUpdateService = A.Fake<JournalUpdateService>(
                opt => opt.
                    CallsBaseMethods().
                    WithArgumentsForConstructor(() =>
                        new JournalUpdateService(fakeIAccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant)));
            A.CallTo(fakeJournalUpdateService).Where(call => call.Method.Name == "Trace")
                .Invokes(
                objectCall =>
                //Logitude.Accounting.BL.EntityUpdateServices.Fakes.ShimJournalUpdateService.AllInstances.TraceJournalPMJournalString = (@this, a, b, c) =>
                {
                    Trace.Write("Override JournalUpdateService.AllInstances.TraceJournalPMJournalString !!!!");
                });

            



            //fakeJournalUpdateService.
            //A.CallTo(() => fakeJournalUpdateService.AddActivityGetLogContactId(A<int>.Ignored, A<string>.Ignored, A<string>.Ignored))
            //    .Returns("ItzikId");
            if (SuppressValidate)
            {
                //
                A.CallTo(fakeJournalUpdateService).Where(call => call.Method.Name == "Validate")
                .Invokes(
                objectCall =>
                {
                    Trace.Write("Override JournalUpdateService.AllInstances.Validate !!!!");
                });

            }

            return fakeJournalUpdateService;
        }

        


       

    }
}
