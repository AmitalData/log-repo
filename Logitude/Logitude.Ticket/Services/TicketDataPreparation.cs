using System;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.TicketTests.Models;
using Logitude.TicketTests.Models.Builders;

namespace Logitude.TicketTests.Services
{
    public class TicketDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<TicketPM> response = APICaller.CallPost<TicketPM>(GetValidTicketPM(), Urls.TicketsController, UserTenant.Token);
                GetValidTicketPM(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Ticket Before Feature Run :" + e.InnerException);
            }
        }

        private TicketPM GetValidTicketPM()
        {
            TicketServices ticketServices = new TicketServices();
            TicketClassificationPM ticketClassification = ticketServices.GetGeneralTicketClassification();
            return new TicketBuilder()
                .WithDefualtValues()
                .EntityType(new ObjectTableService().GetIdByName("Shipment"))
                .Subject("pre specflow sub")
                .TicketDescription("pre specflow desc")
                .MainClassificationId(ticketClassification.Id)
                .SeverityId(ticketClassification.DefaultSeverityId)
                .EmployeeGroupId(ticketClassification.EmployeeGroupId)
                .StageId(ticketServices.GetStageIdByName("Open"))
                .Build();
        }

        private void GetValidTicketPM(TicketPM ticket)
        {
            TicketData.TicketId = ticket.Id;
        }
    }
}
