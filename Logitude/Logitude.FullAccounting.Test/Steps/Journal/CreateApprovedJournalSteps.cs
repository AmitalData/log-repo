using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Journal
{
    [Binding]
    public class CreateApprovedJournalSteps
    {
        private readonly FullAccountingContext context;
        private readonly JournalService journalService;
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
            context.ApprovedJournal = journalService.Create(table, context.Journallines);
        }

        [When(@"create approved journal")]
        public void WhenCreateApprovedJournal()
        {
            context.AddedApprovedJournal = journalService.Add(context.ApprovedJournal);
        }

        [Then(@"the journal should create successfully")]
        public void ThenTheJournalShouldCreateSuccessfully()
        {
            context.AddedApprovedJournal.Id.Should().NotBeNullOrEmpty();
        }
    }
}
