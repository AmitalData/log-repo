using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Services;
using Logitude.Test.Base.Models.Shared;
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
            var arguments = new GetCreateDocumentsFilingArgs() 
            { 
                documentTypeId = DocumentData.DocumentTypeAirManifestID,

            };
            context.Document = APICaller.CallGet<DocumentsFilingPM>(Urls.GetCreateDocumentsFiling(), UserTenant.Token)?.Data;
        }

        [Given(@"following new document properties")]
        public void GivenFollowingNewDocumentProperties(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"update document")]
        public void WhenUpdateDocument()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the document should update successfully")]
        public void ThenTheDocumentShouldUpdateSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
