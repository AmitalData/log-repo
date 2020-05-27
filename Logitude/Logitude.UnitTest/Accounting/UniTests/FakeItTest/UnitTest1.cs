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
using Logitude.BL.CommonDataModel.EntityPMs;

[TestClass]
public partial class JournalValidatorUnitTest
{
    const string C_ExternalNoExist = "ExternalNoExist";
    const string C_ExternalSystemExist = "ExternalSystemExist";

    [TestInitialize]
    public void TestInitialize1()
    {
        var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
        A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
            .ReturnsLazily(
            (string textCodeCode, int tenant) =>
            {
                return textCodeCode;
            }
        );
        JournalValidatorNotStatic.OverrideITextCodeTranslator = textCodeTranslatorFake;

    }
    [TestCleanup]
    public void TestCleanup1()
    {
        JournalValidatorNotStatic.OverrideITextCodeTranslator = null;
    }

    [TestMethod]
    public void IsJournalValid_01_Empty_ErrListContainsYouShouldHaveOneLineAtLeast()
    {


        var entityPM = new JournalPM() { };
        //var validationResults = new List<ValidationResult>();
        //var actual = Validator.TryValidateObject(entityPM, new ValidationContext(entityPM), validationResults);

        System.ComponentModel.DataAnnotations.ValidationContext validationcontext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);

        JournalValidatorNotStatic.OverrideGetLoggedContactFunc =
                new Func<int, ContactPM>(
                    (tenant) => new ContactPM() { DontShowLocal = true }
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
            JournalValidatorNotStatic.OverrideGetLoggedContactFunc =
                null;
        }

    }
}