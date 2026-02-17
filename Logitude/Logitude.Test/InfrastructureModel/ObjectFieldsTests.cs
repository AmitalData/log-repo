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
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class ObjectFieldsTests
    {
        [TestMethod]
        public void GetSingleObjectFieldsPMTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            GeneralDomainService generalDomainService = new GeneralDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
        
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ObjectFieldPM objectFieldPM = generalDomainService.GetStandardFieldsByFieldId("1-1", 1);
            Assert.AreEqual("1-1", objectFieldPM.Id);
        }

        [TestMethod]
        public void CreateObjectFieldsTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockWebFreightContext webContext = new MockWebFreightContext();

            int tenant = 1;

            ObjectFieldPM newObjectField = new ObjectFieldPM()
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


            generalDomainService.InsertObjectField(newObjectField);
            ObjectField objectFields = webContext.ObjectFields.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(objectFields, null);


        }

        [TestMethod]
        public void UpdateObjectFieldTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockWebFreightContext webContext = new MockWebFreightContext();

            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            ObjectFieldPM objectFieldPm = generalDomainService.GetStandardFieldsByFieldId("1-1", 1);


            objectFieldPm.ConverterName = "Edited Name";
            generalDomainService.UpdateObjectField(objectFieldPm);

            ObjectField objectField = webContext.ObjectFields.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited Name", objectField.ConverterName);

        }

   

    }
}
