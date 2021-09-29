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
    public class UpdateDocumentSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentService service;
        public UpdateDocumentSteps(DocumentContext context, DocumentService service)
        {
            this.context = context;
            this.service = service;
        }
        [Given(@"document")]
        public void GivenDocument()
        {
            context.Document = service.CreateDocument(DirectionCodes.In);
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
