
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

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class GLAccountUpdateServiceUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {


            try
            {


                var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
                A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
                    .ReturnsLazily(
                    (string textCodeCode, int tenant) =>
                    {
                        return textCodeCode;
                    }
                );
                JournalValidator.OverrideITextCodeTranslator = textCodeTranslatorFake;



                JournalValidator.OverrideGetLoggedContactFunc =
                    new Func<int, BL.CommonDataModel.EntityPMs.ContactPM>(
                        (tenant) => new BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true }
                     );
                try
                {


                    var entityPM = new JournalPM() { };
                    //var validationResults = new List<ValidationResult>();
                    //var actual = Validator.TryValidateObject(entityPM, new ValidationContext(entityPM), validationResults);

                    System.ComponentModel.DataAnnotations.ValidationContext validationcontext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
                    JournalValidator.OverrideGetLoggedContactFunc =
                new Func<int, BL.CommonDataModel.EntityPMs.ContactPM>(
                    (tenant) => new BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true }
                 );
                    try
                    {
                        ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, validationcontext);
                        Assert.IsNotNull(validationresult, "eXPCETED validationresult ErrListContainsYouShouldHaveOneLineAtLeast");
                        var errList = new List<String>(validationresult.MemberNames);
                        //Assert
                        //Assert.IsFalse(validationResults, "Expected validation to fail.");
                        Assert.AreEqual<int>(1, validationresult.MemberNames.Count(), "Unexpected number of validation errors.");
                        var msg = errList[0];
                        //msg =validationresult[0];
                        Assert.AreEqual<string>(JournalValidator.M_YouShouldHaveOneLineAtLeast, validationresult.MemberNames.ElementAt(0));
                    }
                    finally
                    {
                        JournalValidator.OverrideGetLoggedContactFunc =
                null;
                    }
                    
                }
                finally
                {
                    JournalValidator.OverrideGetLoggedContactFunc = null;

                }
            }
            finally
            {
                JournalValidator.OverrideITextCodeTranslator = null;
            }
        }

#if GLAccMoreData
        [TestMethod]
        public void OnUpdatingCheckBalance_UpdateFromGLAccountUpdateService_ThrowExceptionnoprivtochangeBalanceInLocalCurrency()

        {
            try
            {
             TestsUtil.AssertThrows<Exception>(delegate
            {


                // Arrange
                int tenant = 1;
                string acclId = "1-1";
                var oldPoco = new GLAccount() { Tenant = tenant, Id = acclId, BalanceInLocalCurrency = 5.02m };
                var newPm = new GLAccountPM() { Tenant = tenant, Id = acclId, BalanceInLocalCurrency = 5.02m };
                var mock = new MockObjectSet<GLAccount>() { oldPoco }; ;


                var fakeIAccountingContext = A.Fake<IAccountingContext>();
                A.CallTo(() => fakeIAccountingContext.GLAccounts)
                    .Returns(mock);
                var fakeGlobalContext = A.Fake<IGlobalContext>();



                //A.CallTo( ()=> fakeGLAccountUpdateService.GetDebugTrace

                var realGLAccountUpdateService = new GLAccountUpdateService(fakeIAccountingContext, new System.Collections.Generic.Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                GlobalContext.OverrideIGlobalContextFake = fakeGlobalContext;
                var fakeGLAccountUpdateService = A.Fake<GLAccountUpdateService>(
                    //op => op.CallsBaseMethods
                    //OP => OP.Wrapping(realGLAccountUpdateService)
                    options => options.CallsBaseMethods().WithArgumentsForConstructor(() => 
                        new GLAccountUpdateService(fakeIAccountingContext, new System.Collections.Generic.Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant)
                    ));

                A.CallTo(fakeGLAccountUpdateService)
                    .Where(call => call.Method.Name == "OnUpdating" & call.Arguments.Count == 1)
                    .Invokes(
                    (@this) =>
                    {
                        var up = @this as GLAccountUpdateService;
                        var a = 1;
                    }
                    );

                newPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                var delta = 102.52m;
                newPm.BalanceInLocalCurrency += delta;

                // Act: Run the method under test:
                fakeGLAccountUpdateService.Update(newPm, true);
                //var fakeGLAccountRepository = A.Fake<GLAccountRepository>(optionsBuilder
            },
                 // Assert: Verify the result:
                         expectedContainsMessage: "no priv to change BalanceInLocalCurrency");          


            }
            finally
            {
                GlobalContext.OverrideIGlobalContextFake = null;
            }

        }

        [TestMethod]
        public void OnUpdatingCheckBalanceWithBalancePriv_UpdateFromGLAccountUpdateService_PocoBalanceInLocalCurrencyChanged()
        {
            try
            {
               

                   // Arrange

                //var fakeGlobalContext = A.Fake<GlobalContext>(opt => opt.CallsBaseMethods());
                //A.CallTo((() => fakeGlobalContext.GlobalDBs).R  
                //GlobalContext.OverrideIGlobalContextFake = fakeGlobalContext;

                   int tenant = 1;
                   string acclId = "1-1";
                   var mocPoco = new GLAccount() { Tenant = tenant, Id = acclId, BalanceInLocalCurrency = 5.02m };
                   var newPm = new GLAccountPM() { Tenant = tenant, Id = acclId, BalanceInLocalCurrency = 5.02m };
                   var mock = new MockObjectSet<GLAccount>() { mocPoco }; ;


                   var fakeIAccountingContext = A.Fake<IAccountingContext>();
                   A.CallTo(() => fakeIAccountingContext.GLAccounts)
                       .Returns(mock);
                   


                   //A.CallTo( ()=> fakeGLAccountUpdateService.GetDebugTrace

                  

                   
                   var fakeGLAccountUpdateService = A.Fake<GLAccountUpdateServiceBalancePriv>(
                       //op => op.CallsBaseMethods
                       //OP => OP.Wrapping(realGLAccountUpdateService)
                       options => options.CallsBaseMethods().WithArgumentsForConstructor(() =>
                           new GLAccountUpdateServiceBalancePriv(fakeIAccountingContext, new System.Collections.Generic.Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant)
                       ));

                   A.CallTo(fakeGLAccountUpdateService)
                          .Where(call => call.Method.Name == "Validate")
                          .DoesNothing();
                   A.CallTo(fakeGLAccountUpdateService)
                             .Where(call => call.Method.Name == "Trace")
                             .DoesNothing();
                   A.CallTo(fakeGLAccountUpdateService)
                       .Where(call => call.Method.Name == "OnUpdating" & call.Arguments.Count == 1)
                       .Invokes(
                       (pm) =>
                       {
                          
                           var a = 1;
                       }
                       );

                   newPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                   var delta = 102.52m;
                   fakeGLAccountUpdateService.Init(delta, null, null);  //newPm.CurrentContextTag = delta;
                   newPm.BalanceInLocalCurrency += delta;
                   
                   // Act: Run the method under test:
                   fakeGLAccountUpdateService.Update(newPm, true);
                   //var fakeGLAccountRepository = A.Fake<GLAccountRepository>(optionsBuilder
               
                    // Assert: Verify the result:
                   Assert.AreEqual(newPm.BalanceInLocalCurrency, mocPoco.BalanceInLocalCurrency);


            }
            finally
            {
                GlobalContext.OverrideIGlobalContextFake = null;
            }
        }


#endif
    }
}
