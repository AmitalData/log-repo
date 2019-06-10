
IF OBJECT_ID('[dbo].[usp_UpdateConstituentShipment]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateConstituentShipment]
GO

Create PROCEDURE [dbo].[usp_UpdateConstituentShipment]
(
	@ConstituentId varchar(15),
	@ConsolidationId varchar(15)
)
AS

declare @ConsolidationStatus as varchar(2)
select @ConsolidationStatus = StatusCode from ARInvoices where Id = @ConsolidationId

if (@ConsolidationStatus = 'AD')
BEGIN
	update ShipmentReceivables set ShipmentReceivableLineStatusCode = 'ACCT' where ARInvoiceId = @ConstituentId
END

ELSE
BEGIN
	update ShipmentReceivables set ShipmentReceivableLineStatusCode = 'OAMT' where ARInvoiceId = @ConstituentId
END