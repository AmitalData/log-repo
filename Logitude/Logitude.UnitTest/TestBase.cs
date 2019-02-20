using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using Logitude.Server.Tools;
using Logitude.BL.Interfaces;
using Logitude.BL.Security;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Resolvers;

namespace Logitude.UnitTest
{
    [TestClass]
    public class TestBase
    {
        [TestInitialize]
        public void InitializeTests()
        {
            LoggedContactResolver.RegisterMockLoggedContactUtil();
            DateTimeUtilResolver.RegisterMockDateTimeUtil();
            TranslateTextsClassUtilResolver.RegisterMockTranslateTextsClassUtil();
            IdCounterUtilResolver.RegisterMockIdCounterUtil();
        }


        public static void Aggregate(params Action[] actions)
        {
            var exceptions = new List<AssertFailedException>();

            foreach (var action in actions)
            {
                try
                {
                    action();
                }
                catch (AssertFailedException ex)
                {
                    exceptions.Add(ex);
                }
            }

            var assertionTexts =
                exceptions.Select(assertFailedException => assertFailedException.Message);
            if (0 != assertionTexts.Count())
            {
                throw new
                    AssertFailedException(
                    assertionTexts.Aggregate(
                        (aggregatedMessage, next) => aggregatedMessage + Environment.NewLine + next));
            }
        }
    }
}
