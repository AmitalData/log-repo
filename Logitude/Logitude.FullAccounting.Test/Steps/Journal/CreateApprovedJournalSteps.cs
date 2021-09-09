using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Journal
{
    [Binding]
    public class CreateApprovedJournalSteps
    {
        public FullAccountingContext context { get; set; }
        public JournalService journalService { get; set; }
        public CreateApprovedJournalSteps(FullAccountingContext context, JournalService journalService)
        {
            this.context = context;
            this.journalService = journalService;
        }
        [Given(@"I have the following Journal lines:")]
        public void GivenIHaveTheFollowingJournalLines(Table table)
        {
            context.Journallines = journalService.CreateLines(table);
        }

        [Given(@"a journal with the following properties")]
        public void GivenAJournalWithTheFollowingJournalProperties(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"create approved journal")]
        public void WhenCreateApprovedJournal()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the journal should create successfully")]
        public void ThenTheJournalShouldCreateSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
