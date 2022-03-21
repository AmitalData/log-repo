using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class DeleteGlAccountNoteSteps
    {
        private readonly FullAccountingContext context;
        public DeleteGlAccountNoteSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get post delete gl account note")]
        public void WhenGetPostDeleteGlAccountNote()
        {
            context.Action = () => APICaller.CallPost<object>(null,Urls.DeleteGLAccountNote(FullAccountingData.GLAccountNoteID), UserOtherTenant.Token);
        }

        [Then(@"The post delete note API should return you have no permissions")]
        public void ThenThePostDeleteNoteAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
