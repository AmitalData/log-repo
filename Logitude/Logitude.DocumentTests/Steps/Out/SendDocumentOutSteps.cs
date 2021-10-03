using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Services;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps.Out
{
    [Binding]
    public class SendDocumentOutSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentOutService service;
        public SendDocumentOutSteps(DocumentContext context, DocumentOutService service)
        {
            this.context = context;
            this.service = service;
        }
        [Given(@"a document out for send")]
        public void GivenADocumentOutForSend()
        {
            context.DocumentOut = service.GetDocumentOut();
        }
        
        [When(@"send document")]
        public void WhenSendDocument()
        {
            context.SendDocumentOutResult = service.SendDocumentOut(context.DocumentOut);
        }
        
        [Then(@"the document should be send successfully")]
        public void ThenTheDocumentShouldBeSendSuccessfully()
        {
            context.SendDocumentOutResult.StatusCode.Should().Be(HttpStatusCode.OK);
            context.SendDocumentOutResult.Data.Should().NotBeNullOrEmpty();
        }
    }
}
