
IF OBJECT_ID('[dbo].[usp_DeleteTicketsRecords]', 'P') IS NOT NULL
drop PROCEDURE [dbo].usp_DeleteTicketsRecords
GO

Create PROCEDURE [dbo].usp_DeleteTicketsRecords
(
	@Tenant int
)
AS

BEGIN

delete from InboundEmailLines where Tenant = @Tenant
delete from InboundEmails where Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = 'Ticket')

delete from Correspondences where Tenant =  @Tenant
delete from TicketEscalations where Tenant =  @Tenant
delete from Tickets where Tenant =  @Tenant

END