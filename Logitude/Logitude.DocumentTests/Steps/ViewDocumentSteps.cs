using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps
{
    [Binding]
    public class ViewDocumentSteps
    {
        private readonly DocumentContext context;
        private readonly ViewDocumentService service;
        private readonly DocumentService documentService;
        public ViewDocumentSteps(DocumentContext context, ViewDocumentService service, DocumentService documentService)
        {
            this.context = context;
            this.service = service;
        }
        [Given(@"a document")]
        public void GivenADocument()
        {
            context.DocumentSecurityId = service.GetDocumentWithfileSecurityId();
        }

        [When(@"get document")]
        public void WhenGetDocument()
        {
            context.StatusCode = APICaller.CallGet<object>(Urls.DownloadPage(context.DocumentSecurityId, UserTenant.DocumentDownloadToken), UserTenant.Token,0,false)?.StatusCode;
        }

        [Then(@"the document should be available")]
        public void ThenTheDocumentShouldBeAvailable()
        {
            context.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
