
IF OBJECT_ID('[dbo].[usp_UpdateShipmentARInvoices]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateShipmentARInvoices]
GO

Create PROCEDURE [dbo].[usp_UpdateShipmentARInvoices]
(
	@ShipmentId varchar(15),
	@ConsolidationNumber varchar(50)
)
AS

declare @InvoiceNumber as varchar(50)
declare @ShipmentARInvoices as varchar(1000)

if (@ConsolidationNumber is not null)
set @ShipmentARInvoices = @ConsolidationNumber

BEGIN

	DECLARE EntitiesCursor CURSOR READ_ONLY
	FOR
	SELECT ARInvoices.InvoiceNumber
	FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
	WHERE
	ARInvoiceEntities.EntityId = @ShipmentId
	AND ARInvoices.StatusCode <> 'DR'
	AND ARInvoices.StatusCode <> 'VD'
	--AND ARInvoices.IsConstituentInvoice = 0
	OPEN EntitiesCursor FETCH NEXT FROM EntitiesCursor INTO @InvoiceNumber
	WHILE @@FETCH_STATUS = 0
	BEGIN

	if(@ShipmentARInvoices is null) set @ShipmentARInvoices = @InvoiceNumber
	else set @ShipmentARInvoices = @ShipmentARInvoices + ',' + @InvoiceNumber
	
	FETCH NEXT FROM EntitiesCursor INTO @InvoiceNumber
	END
	CLOSE EntitiesCursor
	DEALLOCATE EntitiesCursor

	if(len(@ShipmentARInvoices) >= 1000)
	begin
		set @ShipmentARInvoices = SUBSTRING(@ShipmentARInvoices, 1, 947) + ' ... for the full list check the shipment receivables'
	end

	update Shipments set ARInvoices = @ShipmentARInvoices where Id = @ShipmentId
END