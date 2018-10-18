update Tickets 
set StageId = (select id from TicketStages where code = 'OP' and Tenant = Tickets.Tenant)
where StageId in (select id from TicketStages where code = 'WT' or code = 'WC' )

delete from TicketStages where code = 'WT' or code = 'WC' 