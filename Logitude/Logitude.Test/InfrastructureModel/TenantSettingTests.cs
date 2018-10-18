using Logitude.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Telerik.JustMock;
using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Security;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Helpers;
using System;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class TenantSettingTests
    {

        [TestMethod]
        public void GetSingleTenantSettingPMTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            TenantSettingPM tenantSettingPm = generalDomainService.GetObjectTableTenantSettings("1-1", 1);
            Assert.AreEqual("1-1", tenantSettingPm.ObjectTableId);
        }

        //[TestMethod]
        //[ExpectedException(typeof(AutenticationException))]
        //public void GetSingleTenantSettingPMWithAutenticationOnTenantTest()
        //{
     
        //    GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
        //    GeneralDomainService generalDomainService = new GeneralDomainService();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    MockWebFreightContext webFreightContext = new MockWebFreightContext();

        
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

        //    TenantSettingPM tenantSettingPm = generalDomainService.GetObjectTableTenantSettings("1-1", 1);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(AutenticationException))]
        //public void GetSingleTenantSettingPMFromAnoterTenantTest()
        //{
        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    GeneralDomainService generalDomainService = new GeneralDomainService();
        //    MockCommonContext commonContext = new MockCommonContext();

        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();


        //    TenantSettingPM tenantSettingPm = generalDomainService.GetObjectTableTenantSettings("1-1", 1);

        //}

        [TestMethod]
        public void CreateTenantSettingTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();

            int tenant = 1;

            TenantSettingPM newTenantSetting = new TenantSettingPM()
            {
                Id = "1-5",
                Tenant = tenant,

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());


            generalDomainService.InsertTenantSetting(newTenantSetting);
            TenantSetting tenantSetting = webContext.TenantSettings.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(tenantSetting, null);


        }

        [TestMethod]
        public void UpdateTenantSettingTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();

            MockWebFreightContext webContext = new MockWebFreightContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());


            TenantSettingPM tenantSettingPm = generalDomainService.GetObjectTableTenantSettings("1-1", 1);

            tenantSettingPm.Size = 1;
            generalDomainService.UpdateTenantSetting(tenantSettingPm);

            TenantSetting tenantSetting = webContext.TenantSettings.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual(1, tenantSetting.Size);

        }

     
    }

}
