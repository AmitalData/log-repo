using System;
using System.Linq;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.CommonDataModel.Repositories;

using Telerik.JustMock;

using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class QueryGroupTests
    {
        [TestMethod]
        public void GetSingleQueryGroupPMTest()
        {
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QueryGroupPM queryGroupPM = webFreightDomainService.GetSingleQueryGroup("QG", 0);
            Assert.AreEqual("QG", queryGroupPM.Code);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQueryGroupPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());


            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QueryGroupPM queryGroupPM = webFreightDomainService.GetSingleQueryGroup("QG",1);
           
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQueryGroupPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockWebFreightContext webContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QueryGroupPM queryGroupPM = webFreightDomainService.GetSingleQueryGroup("QG", 2);
           
        }



    }
}
