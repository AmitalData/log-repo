update ticket
set ticket.GuidId = inbound.Id
from Tickets ticket
inner join InboundEmails inbound
on ticket.Id = inbound.EntityId
where ticket.GuidId is null