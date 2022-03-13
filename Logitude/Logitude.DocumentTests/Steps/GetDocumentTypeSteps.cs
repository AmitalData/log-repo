using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Logitude.DocumentTests.Steps
{
    [Binding]
    public class GetDocumentTypeSteps
    {
        private readonly DocumentContext context;
        public GetDocumentTypeSteps(DocumentContext context)
        {
            this.context = context;
        }
        [When(@"get document types")]
        public void WhenGetDocumentTypes()
        {
            var filter = new ApiQueryFilters() { GetAll = true,Tenant = UserTenant.Tenant };
            context.DocumentTypes = APICaller.CallGetByFilters<List<DocumentTypeList>>(Urls.DocumentTypeViewsGetByFilters, UserTenant.Token, filter).Data;
        }

        [Then(@"document types should be available")]
        public void ThenDocumentTypeShouldBeAvailable()
        {
            context.DocumentTypes.Should().NotBeEmpty();
        }
    }
}
