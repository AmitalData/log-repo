using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.GLAccount
{
    [Binding]
    public class UpdateGlAccountSteps
    {
        private readonly FullAccountingContext context;
        private readonly GlAccountService glAccountService;
        public UpdateGlAccountSteps(FullAccountingContext context, GlAccountService glAccountService)
        {
            this.context = context;
            this.glAccountService = glAccountService;
        }

        [Given(@"glAccount")]
        public void GivenGlAccount()
        {
            context.GLAccount = APICaller.CallGet<GLAccountPM>(Urls.GLAccountsGetSingle(FullAccountingData.GLAccountId), UserTenant.Token)?.Data;
        }

        [Given(@"following new glAccount properties")]
        public void GivenFollowingNewGlAccountProperties(Table table)
        {
            context.GLAccount = glAccountService.Update(context.GLAccount, table);
        }
        
        [When(@"update glAccount")]
        public void WhenUpdateGlAccount()
        {
            context.UpdatedGLAccount = APICaller.CallPut<GLAccountPM>(context.GLAccount, Urls.GLAccountsController, UserTenant.Token)?.Data;
        }

        [Then(@"the glAccount should update successfully")]
        public void ThenTheGlAccountShouldUpdateSuccessfully()
        {
            glAccountService.Assert(context.GLAccount,context.UpdatedGLAccount);
        }
    }
}
