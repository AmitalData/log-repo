using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.DocumentTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps.Out
{
    [Binding]
    public class PrintDocumentOutSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentOutService service;
        public PrintDocumentOutSteps(DocumentContext context, DocumentOutService service)
        {
            this.context = context;
            this.service = service;
        }
        [Given(@"a document out")]
        public void GivenADocumentOut()
        {
            context.DocumentOutSecurityId = service.GetDocumentOutSecurityId();
        }
        
        [When(@"print document")]
        public void WhenPrintDocument()
        {
            context.StatusCode = APICaller.CallGet<DocumentOutPM>(Urls.DownloadPage(context.DocumentOutSecurityId, UserTenant.DocumentDownloadToken), UserTenant.Token,0,false).StatusCode;

        }

        [Then(@"the document should be print successfully")]
        public void ThenTheDocumentShouldBePrintSuccessfully()
        {
            context.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
