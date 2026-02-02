using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.Validators;
using Logitude.Server.Tools.Helpers;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of GLAccountUpdateServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class GLAccountUpdateServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                // Create fake for ITextCodeTranslator using FakeItEasy
                var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
                A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
                    .ReturnsLazily(
                    (string textCodeCode, int tenant) =>
                    {
                        return textCodeCode;
                    }
                );
                JournalValidatorNotStatic.OverrideITextCodeTranslator = textCodeTranslatorFake;

                JournalValidatorNotStatic.OverrideGetLoggedContactFunc =
                    new Func<int, BL.CommonDataModel.EntityPMs.ContactPM>(
                        (tenant) => new BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true }
                     );
                try
                {
                    var entityPM = new JournalPM() { };

                    System.ComponentModel.DataAnnotations.ValidationContext validationcontext = 
                        new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
                    
                    JournalValidatorNotStatic.OverrideGetLoggedContactFunc =
                        new Func<int, BL.CommonDataModel.EntityPMs.ContactPM>(
                            (tenant) => new BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true }
                         );
                    try
                    {
                        ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, validationcontext);
                        Assert.IsNotNull(validationresult, "eXPCETED validationresult ErrListContainsYouShouldHaveOneLineAtLeast");
                        var errList = new List<String>(validationresult.MemberNames);
                        Assert.AreEqual<int>(1, validationresult.MemberNames.Count(), "Unexpected number of validation errors.");
                        var msg = errList[0];
                        Assert.AreEqual<string>(JournalValidator.M_YouShouldHaveOneLineAtLeast, validationresult.MemberNames.ElementAt(0));
                    }
                    finally
                    {
                        JournalValidatorNotStatic.OverrideGetLoggedContactFunc = null;
                    }
                }
                finally
                {
                    JournalValidatorNotStatic.OverrideGetLoggedContactFunc = null;
                }
            }
            finally
            {
                JournalValidatorNotStatic.OverrideITextCodeTranslator = null;
            }
        }
    }
}

