using Logitude.TicketTests.Models;
using Logitude.TicketTests.Services;
using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using FluentAssertions;

namespace Logitude.TicketTests.Steps
{
    [Binding]
    public class CreateTicketSteps
    {

        private readonly TicketContext ticketContext;
        private readonly TicketServices ticketServices;

        public CreateTicketSteps(TicketContext ticketContext, TicketServices ticketServices)
        {
            this.ticketContext = ticketContext;
            this.ticketServices = ticketServices;
        }

        [Given(@"a ticket with the following properties")]
        public void GivenATicketWithTheFollowingProperties(Table table)
        {
            ticketContext.Ticket = ticketServices.CreateInstance(table);
        }
        
        [When(@"create ticket")]
        public void WhenCreateTicket()
        {
            ticketContext.Ticket = APICaller.CallPost<TicketPM>(ticketContext.Ticket, Urls.CRMDomainControllerInserNewTicket, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ticket should create successfully")]
        public void ThenTheTicketShouldCreateSuccessfully()
        {
            ticketContext.Ticket.Should().NotBeNull();
            ticketContext.Ticket.Id.Should().NotBeNull();
            TicketData.Ticket = ticketContext.Ticket;
        }
    }
}
