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
    public class UploadFilewithChunksSteps
    {
        private readonly DocumentContext context;
        private readonly DocumentFileService service;
        private readonly DocumentInService documentService;
        public UploadFilewithChunksSteps(DocumentContext context, DocumentFileService service, DocumentInService documentService)
        {
            this.context = context;
            this.service = service;
            this.documentService = documentService;
        }
        [Given(@"Document Filing")]
        public void GivenDocumentFiling()
        {
            context.Document = documentService.CreateDocument();
        }

        [Given(@"file size (.*) bytes")]
        public void GivenFileSizeBytes(int bytes)
        {
            context.FileSize = bytes;
        }

        [When(@"get file size format")]
        public void WhenGetFileSizeFormat()
        {
            context.FileSizeFormat = APICaller.CallGet<string>(Urls.GetFileSizeFormat(context.FileSize), UserTenant.Token)?.Data;
        }

        [Then(@"file size format should be not null")]
        public void ThenFileSizeFormatShouldBeNotNull()
        {
            context.FileSizeFormat.Should().NotBeNull();
        }

        [When(@"Upload File")]
        public void WhenUploadFile()
        {
            context.UploadedFileSize = service.Uploadfile(context.Document);
        }

        [Then(@"uploaded file size should equal file size")]
        public void ThenUploadedFileSizeShouldEqualFileSize()
        {
            context.FileSize.Should().Be(context.UploadedFileSize);
        }

        [When(@"update document with file")]
        public void WhenUpdateDocumentWithFile()
        {

            context.Document = service.UpdateDocumentWithFile(context.Document);
        }

        [Then(@"the document should be updated")]
        public void ThenTheDocumentShouldBeUpdated()
        {
            service.AssertDocument(context.Document);
        }
    }
}
