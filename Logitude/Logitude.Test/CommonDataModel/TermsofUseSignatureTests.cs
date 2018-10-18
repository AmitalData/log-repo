using System;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;

using Telerik.JustMock;
using System.Linq;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;

using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class TermsofUseSignatureTests
    {
        [TestMethod]
        public void GetSingleTermsofUseSignaturePMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            TermsofUseSignaturePM termsofUseSignaturePM = partnerDomainService.GetSingleTermsofUseSignature("1-1", 1);
            Assert.AreEqual("1-1", termsofUseSignaturePM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleTermsofUseSignaturePMWithAutenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            TermsofUseSignaturePM termsofUseSignaturePM = partnerDomainService.GetSingleTermsofUseSignature("1-1", 1);
           
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleTermsofUseSignaturePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user2@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            TermsofUseSignaturePM termsofUseSignaturePM = partnerDomainService.GetSingleTermsofUseSignature("1-1", 1);
           
        }

        [TestMethod]
        public void CreateTermsofUseSignatureTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            TermsofUseSignatureRepository termsofUseSignatureRepository = new TermsofUseSignatureRepository(commonContext);
            TermsofUseSignaturePM newTermsofUseSignature = new TermsofUseSignaturePM()
            {
                Id = "1-7",
                Tenant=1,
                ContactId="1-1",
                


            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => CodeCounter.GetNumber("Tenant", 0)).Returns(5);
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-7");


            partnerDomainService.InsertTermsofUseSignature(newTermsofUseSignature);
            TermsofUseSignature termsofUseSignature = commonContext.TermsofUseSignatures.Where(d => d.Id == "1-7").FirstOrDefault();
            Assert.AreNotEqual(termsofUseSignature, null);
        }


        [TestMethod]
        public void UpdateTermsofUseSignatureTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());

            TermsofUseSignaturePM termsofUseSignaturePM = partnerDomainService.GetSingleTermsofUseSignature("1-1", 1);

            termsofUseSignaturePM.ContactId = "1-5";
            partnerDomainService.UpdateTermsofUseSignature(termsofUseSignaturePM);

            TermsofUseSignature termsofUseSignature = commonContext.TermsofUseSignatures.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("1-5", termsofUseSignature.ContactId);

        }


    }
}
