using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class PostFixIntegrityCheckErrorInBatchSteps
    {
        private readonly FullAccountingContext context;
        private readonly AccountingEntegrityCheckServices accountingEntegrityCheckServices;
        public PostFixIntegrityCheckErrorInBatchSteps(FullAccountingContext context, AccountingEntegrityCheckServices accountingEntegrityCheckServices)
        {
            this.context = context;
            this.accountingEntegrityCheckServices = accountingEntegrityCheckServices;
        }
        [When(@"get post fix integrity check error in batch")]
        public void WhenGetPostFixIntegrityCheckErrorInBatch()
        {
            var item = accountingEntegrityCheckServices.Create(UserTenant.Tenant);
            context.Action = () => APICaller.CallPost<AccountingIntegrityCheckPM>(item, Urls.PostFixEntegrityCheckErrorInBatch, UserOtherTenant.Token);

        }

        [Then(@"The post fix integrity check error in batch API should return you have no permissions")]
        public void ThenThePostFixIntegrityCheckErrorInBatchAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
