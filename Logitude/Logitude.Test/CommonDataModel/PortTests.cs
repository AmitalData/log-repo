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
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.CommonDataModel
{
     [TestClass]
  
      
   public class PortTests
    {
         MockWebFreightContext webFreightContext;

         [TestInitialize]
         public void InitializePortTests()
         {
             webFreightContext = new MockWebFreightContext();
            // GeneralMocking.FillInfrastructureData(webFreightContext);
         }
           [TestMethod]
        public void GetSinglePortPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            PortPM portPM = commonDomainService.GetPortsById("1-2", 2);
            Assert.AreEqual("1-2", portPM.Id);
        }


           [TestMethod]
           [ExpectedException(typeof(AutenticationException))]
           public void GetSinglePortPMWithAuthenticationOnTenantTest()
           {
               GeneralMocking.MockHttpContext("notexisteduser@fnarsoft.com");
               MockCommonContext commonContext = new MockCommonContext();
               CacheManager.CacheWrapper = new MockCacheWrapper();
               CommonDataDomainService commonDomainService = new CommonDataDomainService();
               Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
              
               Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
               PortPM portPM = commonDomainService.GetPortsById("1-2",2);

           }

           [TestMethod]
           [ExpectedException(typeof(AutenticationException))]
           public void GetSinglePortPMFromAnoterTenantTest()
           {
               GeneralMocking.MockHttpContext("user1@fnarsoft.com");
               MockCommonContext commonContext = new MockCommonContext();
               CacheManager.CacheWrapper = new MockCacheWrapper();
               CommonDataDomainService commonDomainService = new CommonDataDomainService();
               Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

               Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
               PortPM portPM = commonDomainService.GetPortsById("1-3", 2);

           }


           [TestMethod]
           public void CreatePortTest()
           {

               MockCommonContext commonContext = new MockCommonContext();
               CommonDataDomainService commonDomainService = new CommonDataDomainService();
               CacheManager.CacheWrapper = new MockCacheWrapper();
               int tenant = 1;
               PortRepository portRepository = new PortRepository(commonContext);
               PortPM newPort = new PortPM()
               {
                   Id = "1-5",
                   AddedManually = true,
                   Code = "PS",
                   CountryId = "1-1",
                   EnglishName = "Palestinian Port",
                   Field1 = "1",
                   Field2 = "2",
                   Field3 = "3",
                   Field4 = "4",
                   Field5 = "5",
                   Field6 = "6",
                   Field7 = "7",
                   Field8 = "8",
                   Field9 = "9",
                   Field10 = "10",
                   InActive = true,
                   IsAir = false,
                   IsInland = true,
                   IsOcean = true,
                   Latitude = 2.6,
                   LocalName = "aaa",
                   Longtitude = 23.3,
                   Notes = "nothing",
                   Tenant = tenant,
                   CountryName = "Pal",
                   CountryCode = "GB",
                   ComputedLocalName = "nothing",
                   //SearchFields = a.SearchFields,
                   CountryEC = false,
                   StateId = "1-1",

               };

               GeneralMocking.MockHttpContext("user1@fnarsoft.com");


               Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
               Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
               Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
               Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
               Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
               Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
               Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(),Arg.IsAny<string>())).DoNothing();
               Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
               Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
          
              
               commonDomainService.InsertPort(newPort);
               Port port = commonContext.Ports.Where(d => d.Id == "1-5").FirstOrDefault(); 
               Assert.AreNotEqual(port, null);
           }

           [TestMethod]
           public void UpdatePortTest()
           {
               MockCommonContext commonContext = new MockCommonContext();
               CommonDataDomainService commonDomainService = new CommonDataDomainService();
               CacheManager.CacheWrapper = new MockCacheWrapper();

               GeneralMocking.MockHttpContext("user1@fnarsoft.com");
               Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
               Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
               Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
               Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
               Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
               Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

               PortPM portPM = commonDomainService.GetPortsById("1-1", 1);

               portPM.EnglishName = "Edited Name";
               commonDomainService.UpdatePort(portPM);

               Port port = commonContext.Ports.Where(d => d.Id == "1-1").FirstOrDefault();

               Assert.AreEqual("Edited Name", port.EnglishName);

           }


           [TestMethod]
           public void GetPortFiltersTest()
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
               queryoperations.ObjectTableName = "Port";
               queryoperations.PageIndex = 0;
               queryoperations.PageSize = 10;
               queryoperations.QueryFilterItems = new List<QueryFilterItem>();
               queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "InActive", FieldValue =false, Operator = "Equals" });

               MemoryStream memorystream = new MemoryStream();
               XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
               serializer.Serialize(memorystream, queryoperations);
               memorystream.Seek(0, SeekOrigin.Begin);
               var reader = new StreamReader(memorystream);
               string content = reader.ReadToEnd();
               byte[] bytearray = Encoding.ASCII.GetBytes(content);

               List<PortList> list = commonDomainService.GetPortFilters(bytearray, 1);


           }
       
    }
}
