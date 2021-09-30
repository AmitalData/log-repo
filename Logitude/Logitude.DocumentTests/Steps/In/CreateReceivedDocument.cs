using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.DocumentTests.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps
{
    [Binding]
    public class CreateReceivedDocument
    {
        private readonly DocumentContext context;
        private readonly DocumentInService service;
        public CreateReceivedDocument(DocumentContext context, DocumentInService service)
        {
            this.context = context;
            this.service = service;
        }
        [When(@"create document")]
        public void WhenCreateDocument()
        {
            context.Document = service.CreateDocument();
        }

        [Then(@"document should be available")]
        public void ThenDocumentShouldBeAvailable()
        {
            context.Document.Should().NotBeNull();
        }

        [Given(@"following new document properties")]
        public void GivenFollowingNewDocumentProperties(Table table)
        {
            service.UpdateDocumentReceived(context.Document);
        }
        
        [When(@"update document")]
        public void WhenUpdateDocument()
        {
            context.Document = APICaller.CallPut<DocumentsFilingPM>(context.Document,Urls.DocumentsFilingsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the document should update successfully")]
        public void ThenTheDocumentShouldUpdateSuccessfully()
        {
            context.Document.Received.Should().BeTrue();
        }
    }
}
