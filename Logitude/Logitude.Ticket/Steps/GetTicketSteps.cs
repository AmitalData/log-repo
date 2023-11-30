using FluentAssertions;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.TicketTests.Models;
using System;
using TechTalk.SpecFlow;

namespace Logitude.TicketTests.Steps
{
    [Binding]
    public class GetTicketSteps
    {
        private readonly TicketContext ticketContext;

        public GetTicketSteps(TicketContext ticketContext)
        {
            this.ticketContext = ticketContext;
        }

        [When(@"get ticket with TicketId")]
        public void WhenGetTicketWithTicketId()
        {
            ticketContext.Ticket = APICaller.CallGet<TicketPM>(Urls.TicketSingle(TicketData.TicketId), UserTenant.Token).Data;
        }
        
        [Then(@"ticket should be avaliable")]
        public void ThenTicketShouldBeAvaliable()
        {
            ticketContext.Ticket.Should().NotBeNull();
            ticketContext.Ticket.Id.Should().NotBeNull();
        }
    }
}
