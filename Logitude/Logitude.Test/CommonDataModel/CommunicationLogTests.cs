using System;
using System.Linq;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.Mocks;

using Telerik.JustMock;

using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.CommonDataModel.Tools.EntityService;
using WebFreight.Web.CommonDataModel.Tools.TraceEvents;
using WebFreight.Web.CommonDataModel.Tools.Validating;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using WebFreight.Web.GlobalModelDB;
using Simplog.Global.Data.GlobalModel;
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
    public class CommunicationLogTests
    {
        [TestMethod]
        public void GetSingleCommunicationLogPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            
            CommunicationLogPM communicationLogPM = commonDomainService.GetSingleCommunicationLog("1-1", 1);
            Assert.AreEqual("1-1", communicationLogPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleCommunicationLogPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            CommunicationLogPM communicationLogPM = commonDomainService.GetSingleCommunicationLog("1-1", 1);
          
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleCommunicationLogPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            CommunicationLogPM communicationLogPM = commonDomainService.GetSingleCommunicationLog("1-2", 2);
          
        }

        [TestMethod]
        public void CreateCommunicationLogTest()
        {

        
            MockCommonContext commonContext = new MockCommonContext();
         
            int tenant = 1;
            CommunicationLogService service = new CommunicationLogService(commonContext, tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            CommunicationLogPM newCommunicationLog = new CommunicationLogPM()
            {
                Id = "1-5",
                Tenant =tenant,
                EntityId = "1-2",
                ObjectTableId = "1-1",
                DocumentId = "1-1",
                DocumentInId = "1-2",
                DocumentOutId ="1-3",
                CreatedByUserId ="1-3",
                CreatedByUserName = "user1",
 


            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => CommunicationLogValidating.Validate(Arg.IsAny<CommunicationLogPM>())).DoNothing();
            Mock.Arrange(() => CommunicationLogTracing.Trace(Arg.IsAny<CommunicationLogPM>(), Arg.IsAny<CommunicationLog>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
           




            service.Create(newCommunicationLog);
            CommunicationLog communicationLog = commonContext.CommunicationLogs.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(communicationLog, null);


        }

        [TestMethod]
        public void UpdateCommunicationLogTest()
        {

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            CommunicationLogService service = new CommunicationLogService(commonContext, tenant);
            CommonDataDomainService commonDomainService = new CommonDataDomainService();

            Mock.Arrange(() => CommunicationLogValidating.Validate(Arg.IsAny<CommunicationLogPM>())).DoNothing();
            Mock.Arrange(() => CommunicationLogTracing.Trace(Arg.IsAny<CommunicationLogPM>(), Arg.IsAny<CommunicationLog>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            CommunicationLogPM communicationLogPM = commonDomainService.GetSingleCommunicationLog("1-1", 1);

            communicationLogPM.From = "me";
            service.Update(communicationLogPM);

            CommunicationLog communicationLog = commonContext.CommunicationLogs.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("me", communicationLog.From);

        }

        [TestMethod]
        public void GetCommunicationLogFiltersTest()
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
            queryoperations.ObjectTableName = "CommunicationLog";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "Subject" , Operator="Equals"});


            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<CommunicationLogList> list = commonDomainService.GetCommunicationLogFilters(bytearray, 1);


        }
    }
}
