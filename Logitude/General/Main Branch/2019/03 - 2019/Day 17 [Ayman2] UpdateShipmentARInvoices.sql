
-- Run DataViews And Procedures\Stored Procedures\UpdateShipmentARInvoices.sql File First

declare @EntityId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT EntityId
	FROM ARInvoiceEntities
	group by EntityId
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		EXECUTE usp_UpdateShipmentARInvoices @EntityId

	FETCH NEXT FROM DataCursor INTO @EntityId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END