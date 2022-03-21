using FluentAssertions;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.TicketTests.Models;
using Logitude.TicketTests.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.TicketTests.Steps
{
    [Binding]
    public class UpdateTicketSteps
    {
        private readonly TicketContext ticketContext;
        private readonly TicketServices ticketServices;
        private TicketPM updatedTicket;

        public UpdateTicketSteps(TicketContext ticketContext, TicketServices ticketServices)
        {
            this.ticketContext = ticketContext;
            this.ticketServices = ticketServices;
        }

        [Given(@"a ticket")]
        public void GivenATicket()
        {
            ticketContext.Ticket = APICaller.CallGet<TicketPM>(Urls.TicketSingle(TicketData.TicketId), UserTenant.Token).Data;
        }
        
        [Given(@"following ticket properties")]
        public void GivenFollowingTicketProperties(Table table)
        {
           ticketServices.UpdateInstance(table, ticketContext.Ticket);
        }
        
        [When(@"update ticket")]
        public void WhenUpdateTicket()
        {
            updatedTicket = APICaller.CallPut<TicketPM>(ticketContext.Ticket, Urls.TicketsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ticket should update successfully")]
        public void ThenTheTicketShouldUpdateSuccessfully()
        {
            updatedTicket.Id.Should().NotBeNull();
            updatedTicket.EntityType.Should().Equals(ticketContext.Ticket.EntityType);
            updatedTicket.TicketDescription.Should().Equals(ticketContext.Ticket.TicketDescription);
            updatedTicket.Subject.Should().Equals(ticketContext.Ticket.Subject);
        }
    }
}
