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
using Logitude.BL.Resolvers;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    [TestClass]
    public class BankAccountValidateUpdateServiceUnitTest:TestBase
    {
        [TestMethod]
        public void ValidateBankAccountExist_BankAccountExists_ReturnsValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "Bank account exist";
            string actualErrorMessage = "";
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber="12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
         
            A.CallTo(() => bankAccountValidateService.GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant)).Returns(list);

            bankAccountValidateService.CheckBankAccountExists(entityPM);
            
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }

            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
            
        }
       
        [TestMethod]
        public void ValidateBankAccountExist_BankAccountDoesnotExist_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;
            
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
           // A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant)).Returns(null);
            bankAccountValidateService.CheckBankAccountExists(entityPM);
            actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
            // bool ok = true;

            //            NUnit.Framework.Assert.DoesNotThrow(() => bankAccountValidateService.CheckBankAccountExists(entityPM));
            //Exception ex =  NUnit.Framework.Assert.Throws<Exception>(() => bankAccountValidateService.CheckBankAccountExists(entityPM));
            //NUnit.Framework.Assert.That(ex.Message, Is.EqualTo("bar"));

            //try
            //{
            //    bankAccountValidateService.CheckBankAccountExists(entityPM);
            //}
            //catch (Exception ex)
            //{
            //    ok = false;
            //}
            //finally
            //{
            //    Assert.AreEqual(true, ok);
            //}
        }

        [TestMethod]
        public void CheckGLAccountAlreadyConnectedToBankAccountOnInsert_GLAccountConnectedToBankAccount_ReturnsAValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "Accounting.General.O.GLAccountAlreadyConnectedToBankAccount"; //textCodeCode + " " + tenant + " " + getLocalDefaultText
            string actualErrorMessage = "";

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(list);
            //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", entityPM.Tenant,A<bool>.Ignored)).Returns("GLAccountAlreadyConnectedToBankAccount");
            bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM, true);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM,true);
            //}, "GLAccountAlreadyConnectedToBankAccount");

        }

        [TestMethod]
        //[Row]
        public void CheckGLAccountAlreadyConnectedToBankAccountOnInsert_GLAccountNotConnectedToBankAccount_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(null);
            //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", entityPM.Tenant, A<bool>.Ignored)).Returns("GLAccountAlreadyConnectedToBankAccount");
            bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM, true);
            actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);

            //bool ok = true;
            //try
            //{
            //    bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM, true);
            //}
            //catch
            //{
            //    ok = false;
            //}
            //finally
            //{
            //    NUnit.Framework.Assert.AreEqual(ok, true);
            //}

        }

        [TestMethod]
        public void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert_DefferedGLAccountConnectedToBankAccount_ReturnsAValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "The Deferred GLAccount is already connected to a Bank Account";
            string actualErrorMessage = "";
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant)).Returns(list);
            bankAccountValidateService.Validate(entityPM);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.Validate(entityPM);
            //}, "The Deferred GLAccount is already connected to a Bank Account");

        }

        [TestMethod]
        public void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert_DefferedGLAccountNotConnectedToBankAccount_ErrorCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant)).Returns(null);
            bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM);
            actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);

            //bool ok = true;
            //try
            //{
            //    bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM);
            //}
            //catch
            //{
            //    ok = false;
            //}
            //finally
            //{
            //    NUnit.Framework.Assert.AreEqual(ok, true);
            //}

        }

        [TestMethod]
        public void CheckGLAccountAlreadyConnectedToBankAccountOnUpdate_GLAccountConnectedToBankAccount_ReturnsAValidationMessage() {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "Accounting.General.O.GLAccountAlreadyConnectedToBankAccount";
            string actualErrorMessage = "";

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

           


            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(list);
            //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", entityPM.Tenant, A<bool>.Ignored)).Returns("GLAccountAlreadyConnectedToBankAccount");
            A.CallTo(() => bankAccountValidateService.GetSingleBankAccount(entityPM.Id, entityPM.Tenant)).Returns(poco);

            bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco, true);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);

            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM,poco, true);
            //}, "GLAccountAlreadyConnectedToBankAccount");
        }

        [TestMethod]
        public void CheckGLAccountAlreadyConnectedToBankAccountOnUpdate_GLAccountNotConnectedToBankAccount_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;
            var listOfState = new List<Tuple<BankAccountPM,BankAccountList,BankAccount>>();

            listOfState.Add(new Tuple<BankAccountPM, BankAccountList, BankAccount>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            }, 
            new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            }, 
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            }));

            listOfState.Add(new Tuple<BankAccountPM, BankAccountList, BankAccount>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = null,
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },
            new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            }));

            foreach(var item in listOfState)
            {
                var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
                //A.CallTo(() => bankAccountValidateService.GetLoggedContact(item.Item1.Tenant)).Returns(loggedcontact);
                A.CallTo(() => bankAccountValidateService.GetByGLAccount(item.Item1.GLAccountId, item.Item1.Tenant)).Returns(item.Item2);
                //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", item.Item1.Tenant, A<bool>.Ignored)).Returns("GLAccountAlreadyConnectedToBankAccount");
                A.CallTo(() => bankAccountValidateService.GetSingleBankAccount(item.Item1.Id, item.Item1.Tenant)).Returns(item.Item3);
                bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(item.Item1, item.Item3, true);
                actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
                Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
                //bool success = true;

                //try
                //{
                //    bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(item.Item1, item.Item3, true);
                //}
                //catch
                //{
                //    success = false;
                //}
                //finally
                //{
                //    NUnit.Framework.Assert.AreEqual(success, true);
                //}
            }


        }

        [TestMethod]
        public void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate_DefferedGLAccountConnectedToBankAccount_ReturnsAValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "The Deferred GLAccount is already connected to a Bank Account";
            string actualErrorMessage = "";

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA22",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };




            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant)).Returns(list);
            A.CallTo(() => bankAccountValidateService.GetSingleBankAccount(entityPM.Id, entityPM.Tenant)).Returns(poco);
            bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);

            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco);
            //}, "The Deferred GLAccount is already connected to a Bank Account");
        }

        [TestMethod]
        public void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate_DefferedGLAccountNotConnectedToBankAccount_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;

            var listOfState = new List<Tuple<BankAccountPM, BankAccountList, BankAccount>>();

            listOfState.Add(new Tuple<BankAccountPM, BankAccountList, BankAccount>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },
            new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            }));

            listOfState.Add(new Tuple<BankAccountPM, BankAccountList, BankAccount>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = null,
                DeferredGLAccountId = null,
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },
            new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            }));

            foreach (var item in listOfState)
            {
                actualErrorsCount = 0;
                var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
                //A.CallTo(() => bankAccountValidateService.GetLoggedContact(item.Item1.Tenant)).Returns(loggedcontact);
                A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(item.Item1.DeferredGLAccountId, item.Item1.Tenant)).Returns(item.Item2);
                A.CallTo(() => bankAccountValidateService.GetSingleBankAccount(item.Item1.Id, item.Item1.Tenant)).Returns(item.Item3);
                bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(item.Item1, item.Item3);
                actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
                Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
                //bool success = true;

                //try
                //{
                //    bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(item.Item1, item.Item3);
                //}
                //catch
                //{
                //    success = false;
                //}
                //finally
                //{
                //    NUnit.Framework.Assert.AreEqual(success, true);
                //}
            }


        }

        [TestMethod]
        public void CheckGLAccountHasTransactionsOnUpdate_GLAccountHasTransactions_ReturnsAValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            string expectedErrorMessage = "Accounting.O.ThereRTransactions4GLAccountCantUpdated";
            string actualErrorMessage = "";

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA22",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            List<LedgerTransaction> transactions = new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", entityPM.Tenant, A<bool>.Ignored)).Returns("ThereRTransactions4GLAccountCantUpdated");
            A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(transactions);
            bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(entityPM, poco, true);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(entityPM, poco, true);
            //}, "ThereRTransactions4GLAccountCantUpdated");

        }

        [TestMethod]
        public void CheckGLAccountHasTransactionsOnUpdate_GLAccountDosntHaveTransactions_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;

            var listOfState = new List<Tuple<BankAccountPM, BankAccount,List<LedgerTransaction>>>();

            listOfState.Add(new Tuple<BankAccountPM, BankAccount, List<LedgerTransaction>>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },
           
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
            new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            }
            ));

            listOfState.Add(new Tuple<BankAccountPM, BankAccount, List<LedgerTransaction>>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = null,
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },
          
            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
             new List<LedgerTransaction>()));

            foreach (var item in listOfState)
            {
                actualErrorsCount = 0;
                var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
                //A.CallTo(() => bankAccountValidateService.GetLoggedContact(item.Item1.Tenant)).Returns(loggedcontact);
                //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", item.Item1.Tenant, A<bool>.Ignored)).Returns("ThereRTransactions4GLAccountCantUpdated");
                A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(item.Item3);
                bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(item.Item1, item.Item2, true);
                actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
                Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
                //bool success = true;

                //try
                //{
                //    bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(item.Item1, item.Item2, true);
                //}
                //catch
                //{
                //    success = false;
                //}
                //finally
                //{
                //    NUnit.Framework.Assert.AreEqual(success, true);
                //}
            }

        }


        [TestMethod]
        public void CheckDefferedGLAccountTransactionsOnUpdate_DefferedGLAccountHasTransactions_ReturnsAValidationMessage()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;

            string expectedErrorMessage = "Accounting.O.ThereRTransactions4GLAccountCantUpdated";
            string actualErrorMessage = "";

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA22",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            List<LedgerTransaction> transactions = new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            };

            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", entityPM.Tenant, A<bool>.Ignored)).Returns("ThereRTransactions4GLAccountCantUpdated");
            A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(transactions);

            bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(entityPM, poco, true);
            if (bankAccountValidateService.ErrorsList.Count > 0)
            {
                actualErrorMessage = bankAccountValidateService.ErrorsList.FirstOrDefault();
            }
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
            //TestsUtil.AssertThrows<Exception>(() =>
            //{
            //    bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(entityPM, poco, true);
            //}, "ThereRTransactions4GLAccountCantUpdated");

        }

        [TestMethod]
        public void CheckDefferedGLAccountTransactionsOnUpdate_DefferedGLAccountDosntHaveTransactions_ErrorsCountIsZero()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            int actualErrorsCount = 0;
            int expectedErrorsCount = 0;
            var listOfState = new List<Tuple<BankAccountPM, BankAccount, List<LedgerTransaction>>>();

            listOfState.Add(new Tuple<BankAccountPM, BankAccount, List<LedgerTransaction>>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },

            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
            new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            }
            ));

            listOfState.Add(new Tuple<BankAccountPM, BankAccount, List<LedgerTransaction>>(
            new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA22",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            },

            new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            },
             new List<LedgerTransaction>()));

            foreach (var item in listOfState)
            {
                actualErrorsCount = 0;
                var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
                //A.CallTo(() => bankAccountValidateService.GetLoggedContact(item.Item1.Tenant)).Returns(loggedcontact);
                //A.CallTo(() => bankAccountValidateService.GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", item.Item1.Tenant, A<bool>.Ignored)).Returns("ThereRTransactions4GLAccountCantUpdated");
                A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(item.Item3);
                bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(item.Item1, item.Item2, true);
                actualErrorsCount = bankAccountValidateService.ErrorsList.Count;
                Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
                //bool success = true;

                //try
                //{
                //    bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(item.Item1, item.Item2, true);
                //}
                //catch
                //{
                //    success = false;
                //}
                //finally
                //{
                //    NUnit.Framework.Assert.AreEqual(success, true);
                //}
            }

        }

        [TestMethod]
        public void Validate_AllChecksHappenOnInsert_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            List<LedgerTransaction> transactions = new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            };


            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
           // A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(transactions);
            bankAccountValidateService.Validate(entityPM);

            A.CallTo(() => bankAccountValidateService.CheckBankAccountExists(entityPM)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM,A<bool>.Ignored)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM,A<BankAccount>.Ignored ,A<bool>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, A<BankAccount>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(entityPM, A<BankAccount>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(entityPM, A<BankAccount>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();

        }

        [TestMethod]
        public void Validate_AllChecksHappenOnUpdate_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);//GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };


            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            BankAccountList list = new BankAccountList()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            List<LedgerTransaction> transactions = new List<LedgerTransaction>() {
                new LedgerTransaction(){Id="1",OpenAmount=1000 },
            };


            var bankAccountValidateService = A.Fake<BankAccountValidateService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountValidateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountValidateService.GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant)).Returns(null);
            A.CallTo(() => bankAccountValidateService.GetSingleBankAccount(entityPM.Id, entityPM.Tenant)).Returns(poco);
            A.CallTo(() => bankAccountValidateService.GetLedgerTransactions(A<string>.Ignored, A<int>.Ignored)).Returns(transactions);
            bankAccountValidateService.Validate(entityPM);

            A.CallTo(() => bankAccountValidateService.CheckBankAccountExists(entityPM)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM, A<bool>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco, A<bool>.Ignored)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckGLAccountHasTransactionsOnUpdate(entityPM, poco,A<bool>.Ignored)).MustHaveHappened();
            A.CallTo(() => bankAccountValidateService.CheckDefferedGLAccountTransactionsOnUpdate(entityPM, poco, A<bool>.Ignored)).MustHaveHappened();


        }

    }
}
