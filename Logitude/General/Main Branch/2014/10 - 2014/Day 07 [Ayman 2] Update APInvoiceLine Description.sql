
DECLARE @Tenant AS INT
DECLARE @LineNumber AS INT
DECLARE @APInvoiceId AS varchar(15)
DECLARE @ChargesTypeId AS varchar(15)

DECLARE @Description AS varchar(250)
DECLARE @LocalDescription AS varchar(250)

BEGIN;
	DECLARE APInvoiceLinesCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant, LineNumber, APInvoiceId, ChargesTypeId
	FROM APInvoiceLines
	OPEN APInvoiceLinesCursor FETCH NEXT FROM APInvoiceLinesCursor INTO @Tenant, @LineNumber, @APInvoiceId, @ChargesTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	select
	@Description = EnglishName,
	@LocalDescription = LocalName
	from ChargesTypes
	where Id = @ChargesTypeId AND Tenant = @Tenant

	update APInvoiceLines
	set
	Description = @Description,
	LocalDescription = @LocalDescription
	where APInvoiceId = @APInvoiceId AND LineNumber = @LineNumber AND Tenant = @Tenant

	FETCH NEXT FROM APInvoiceLinesCursor INTO @Tenant, @LineNumber, @APInvoiceId, @ChargesTypeId
	END
	CLOSE APInvoiceLinesCursor
	DEALLOCATE APInvoiceLinesCursor	
END