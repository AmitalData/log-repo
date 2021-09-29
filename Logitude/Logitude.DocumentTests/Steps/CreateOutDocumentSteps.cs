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
    public class CreateOutDocumentSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentService service;
        public CreateOutDocumentSteps(DocumentContext context, DocumentService service)
        {
            this.context = context;
            this.service = service;
        }
        [When(@"create Air Manifest out document")]
        public void WhenCreateAirManifestOutDocument()
        {
            context.Document = service.CreateDocument(DirectionCodes.Out);
        }
        [Then(@"the Air Manifest out document should be created successfully")]
        public void ThenTheAirManifestOutDocumentShouldBeCreatedSuccessfully()
        {
            context.Document.Should().NotBeNull();
        }
        [When(@"get Air Manifest out document copy")]
        public void WhenGetAirManifestOutDocumentCopy()
        {
            context.DocumentCopy = service.GetDocumentCopyId(DocumentData.DocumentTypeAirManifestId, context.Document.Id, UserTenant.Tenant);
        }

        [Then(@"the Air Manifest out document copy should be available")]
        public void ThenTheAirManifestOutDocumentCopyShouldBeAvailable()
        {
            context.DocumentCopy.Should().NotBeNull();
        }


        [When(@"update Air Manifest out document properties")]
        public void WhenUpdateAirManifestOutDocumentProperties()
        {
            ScenarioContext.Current.Pending();
        }
        
        
        
        [Then(@"Air Manifest out document should be update")]
        public void ThenAirManifestOutDocumentShouldBeUpdate()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
