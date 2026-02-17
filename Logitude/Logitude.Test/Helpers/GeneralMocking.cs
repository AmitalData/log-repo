using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

using Telerik.JustMock;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.Helpers
{
    class GeneralMocking
    {
        public static HttpContext MockHttpContext(string email)
        {
            var MockUser = Mock.Create<IPrincipal>();
            var MockIdentity = Mock.Create<IIdentity>();
            var request = new HttpRequest(string.Empty, "http://telerik.com", null);
            var response = new HttpResponse(null);
            //var MockCache = Mock.Create<Cache>();
            
            
            HttpContext MockHttpContext = Mock.Create<HttpContext>(() => new HttpContext(request, response));
           
            Mock.Arrange(() => HttpContext.Current).Returns(MockHttpContext);
            Mock.Arrange(() => MockHttpContext.User).Returns(MockUser);
            //Mock.Arrange(() => MockHttpContext.Cache).Returns(MockCache);
            Mock.Arrange(() => MockUser.Identity).Returns(MockIdentity);
            Mock.Arrange(() => MockUser.Identity.Name).Returns(email);
            return MockHttpContext;
        }

        public static void FillInfrastructureData(IWebFreightContext webFreightContext)
        {
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), 0)).Returns(Guid.NewGuid().ToString());
          
            MetaDataUpdateClass metaDataUpdateClass = new MetaDataUpdateClass();

            Mock.NonPublic.Arrange(metaDataUpdateClass, "CreateTableCounters").DoNothing();
            Mock.NonPublic.Arrange(metaDataUpdateClass, "CreateTenantSettings").DoNothing();
            Mock.NonPublic.Arrange(metaDataUpdateClass, "CreateAllTablesTips", new object[] { new Dictionary<string,Tip>(),new Dictionary<string,TextCode>() }).DoNothing();
            Mock.NonPublic.Arrange(metaDataUpdateClass, "LoadCreateTestFields", new object[] { new Dictionary<string, ObjectField>(), new Dictionary<string, TextCode>() }).DoNothing();
            Mock.NonPublic.Arrange(metaDataUpdateClass, "LoadRolesAndFeatures",new object[]{Arg.IsAny<int>()}).DoNothing();
            Mock.NonPublic.Arrange(metaDataUpdateClass, "CreateMenuButtonsForTenant", new object[] { Arg.IsAny<int>() }).DoNothing();

            metaDataUpdateClass.LoadObjectsTenantZero(webFreightContext);
            metaDataUpdateClass.LoadOtherFields(webFreightContext);
        }
    }
}
