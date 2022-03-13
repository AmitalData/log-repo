using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.DocumentTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps
{
    [Binding]
    public class CreateOutDocumentSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentOutService service;
        public CreateOutDocumentSteps(DocumentContext context, DocumentOutService service)
        {
            this.context = context;
            this.service = service;
        }
        [When(@"create Air Manifest out document")]
        public void WhenCreateAirManifestOutDocument()
        {
            context.DocumentOut = service.CreateDocumentOut();
        }
        [Then(@"the Air Manifest out document should be created successfully")]
        public void ThenTheAirManifestOutDocumentShouldBeCreatedSuccessfully()
        {
            context.DocumentOut.Should().NotBeNull();
        }
        [When(@"get Air Manifest out document copy")]
        public void WhenGetAirManifestOutDocumentCopy()
        {
            context.DocumentOutCopyId = service.GetDocumentOutCopyId(DocumentData.DocumentTypeAirManifestId, context.DocumentOut.Id, UserTenant.Tenant);
        }

        [Then(@"the Air Manifest out document copy should be available")]
        public void ThenTheAirManifestOutDocumentCopyShouldBeAvailable()
        {
            context.DocumentOutCopyId.Should().NotBeNull();
        }


        [When(@"update Air Manifest out document properties")]
        public void WhenUpdateAirManifestOutDocumentProperties()
        {
            context.DocumentOut = service.UpdateOutDocument(context.DocumentOut, context.DocumentOutCopyId);
        }



        [Then(@"Air Manifest out document should be update")]
        public void ThenAirManifestOutDocumentShouldBeUpdate()
        {
            context.DocumentOut.Should().NotBeNull();
            context.DocumentOut.DocumentOutCopies.Should().NotBeEmpty();
        }
    }
}
