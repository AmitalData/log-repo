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
using WebFreight.Web.CommonDataModel.Tools.EntityService;
using WebFreight.Web.CommonDataModel.Tools.TraceEvents;
using WebFreight.Web.CommonDataModel.Tools.Validating;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class VesselTests
    {
        [TestMethod]
        public void GetSingleVesselPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            VesselPM vesselPM = commonDomainService.GetSingleVessel("1-1",1);
            Assert.AreEqual("1-1", vesselPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleVesselPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistedUser@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            VesselPM vesselPM = commonDomainService.GetSingleVessel("1-1", 1);
        
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleVesselPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            VesselPM vesselPM = commonDomainService.GetSingleVessel("1-2", 2);
        
        }

        [TestMethod]
        public void CreateVesselTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            VesselService service = new VesselService(commonContext, tenant);
            VesselRepository vesselRepository = new VesselRepository(commonContext);
            VesselPM newVessel = new VesselPM()
            {
                Id = "1-5",
                AddedManually = true,
                Code = "PS",
                EnglishName = "NewVessel",
                
                InActive = false,
                LocalName ="Vessel",
                Tenant = tenant,
                
                

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => VesselValidating.Validate(Arg.IsAny<VesselPM>())).DoNothing();
            Mock.Arrange(() => VesselTracing.Trace(Arg.IsAny<VesselPM>(), Arg.IsAny<Vessel>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();



            service.Create(newVessel);
            Vessel vessel = commonContext.Vessels.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(vessel, null);

        
        }


        [TestMethod]
        public void UpdateVesselTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            VesselService service = new VesselService(commonContext, tenant);
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => VesselValidating.Validate(Arg.IsAny<VesselPM>())).DoNothing();
            Mock.Arrange(() => VesselTracing.Trace(Arg.IsAny<VesselPM>(), Arg.IsAny<Vessel>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            VesselPM vesselPM = commonDomainService.GetSingleVessel("1-1", 1);

            vesselPM.EnglishName = "Edited Name";

            service.Update(vesselPM);

            Vessel vessel = commonContext.Vessels.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited Name", vessel.EnglishName);

        }

        [TestMethod]
        public void GetVesselFiltersTest()
        {

            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();

            MockWebFreightContext webContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();


            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "Vessel";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "InActive", FieldValue = false, Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<VesselList> list = commonDomainService.GetVesselFilters(bytearray, 1);


        }
    }
}
