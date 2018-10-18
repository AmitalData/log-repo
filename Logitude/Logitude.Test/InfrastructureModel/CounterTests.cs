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
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class CounterTests
    {
        [TestMethod]
        public void GetSingleCounterPMTest()
        {
            GeneralDomainService generalDomainService = new GeneralDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            CounterPM CounterPM = generalDomainService.GetObjectTableCounter("1-1", 1);
            Assert.AreEqual("1-1", CounterPM.ObjectTableId);
        }

        [TestMethod]
        public void CreateCounterTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();

            int tenant = 1;

            CounterPM newCounter = new CounterPM()
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


            generalDomainService.InsertCounter(newCounter);
            Counter counter = webContext.Counters.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(counter, null);


        }


        [TestMethod]
        public void UpdateCounterTest()
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


            CounterPM counterPM = generalDomainService.GetObjectTableCounter("1-1", 1);

            counterPM.Name = "Edited Name";
            generalDomainService.UpdateCounter(counterPM);

            Counter counter = webContext.Counters.Where(d => d.Id == "1").FirstOrDefault();

            Assert.AreEqual("Edited Name", counter.Name);

        }




    }
}
