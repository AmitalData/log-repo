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
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class TermsofUseTests
    {
        [TestMethod]
        public void GetSingleTermsofUsePMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            TermsofUsePM termsofUsePM = partnerDomainService.GetSingleTermsofUse(DateTime.Now.Date, 1);
            Assert.AreEqual(1, termsofUsePM.Version);
        }


        //[TestMethod]
        //[ExpectedException(typeof(AutenticationException))]
        //public void GetSingleTermsofUsePMWithAutenticationOnTenantTest()
        //{
        //    GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
        //    MockCommonContext commonContext = new MockCommonContext();
        //    CacheManager.CacheWrapper = new MockCacheWrapper();
        //    PartnersDomainService partnerDomainService = new PartnersDomainService();
        //    CommonDataDomainService commonDomainService = new CommonDataDomainService();
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
        //    TermsofUsePM termsofUsePM = partnerDomainService.GetSingleTermsofUse(DateTime.Now.Date, 1);
          
        //}

        //[TestMethod]
        //[ExpectedException(typeof(AutenticationException))]
        //public void GetSingleTermsofUsePMFromAnoterTenantTest()
        //{
        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    CacheManager.CacheWrapper = new MockCacheWrapper();
        //    PartnersDomainService partnerDomainService = new PartnersDomainService();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    CommonDataDomainService commonDomainService = new CommonDataDomainService();
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
        //    TermsofUsePM termsofUsePM = partnerDomainService.GetSingleTermsofUse(DateTime.Now, 1);
          
        //}


        [TestMethod]
        public void CreateTermsofUseTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
         
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            TermsofUseRepository termsofUseRepository = new TermsofUseRepository(commonContext);
            TermsofUsePM newTermsofUse = new TermsofUsePM()
            {
                Version = 4,
                Date = DateTime.Today


            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => CodeCounter.GetNumber("Tenant", 0)).Returns(5);
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());


            partnerDomainService.InsertTermsofUse(newTermsofUse);
            TermsofUse termsofUse = commonContext.TermsofUses.Where(d => d.Version == 4).FirstOrDefault();
            Assert.AreNotEqual(termsofUse, null);
        }



        //[TestMethod]
        //public void UpdateTermsofUseTest()
        //{
        //    MockCommonContext commonContext = new MockCommonContext();
        //    PartnersDomainService partnerDomainService = new PartnersDomainService();
        //    CacheManager.CacheWrapper = new MockCacheWrapper();
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
        //    Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
        //    Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());

        //    TermsofUsePM termsofUsePM = partnerDomainService.GetSingleTermsofUse(DateTime.Now.Date, 1);

        //    termsofUsePM.Date = new DateTime(2013, 1, 18);
        //    partnerDomainService.UpdateTermsofUse(termsofUsePM);

        //    TermsofUse termsofUse = commonContext.TermsofUses.Where(d => d.Version==1).FirstOrDefault();

        //    Assert.AreEqual(new DateTime(2013, 1, 18), termsofUse.Date);

        //}
    }
}
