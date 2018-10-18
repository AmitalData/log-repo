
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @QuoteNumber as varchar(15)
declare @ShipperName as nvarchar(100)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)
declare @ConsigneeName as nvarchar(100)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)
declare @CustomerName as nvarchar(100)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)
declare @CustomerContactId as varchar(15)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @MainCarriageCarrierId as varchar(15)
declare @MySearchFields as nvarchar(1000)
declare @Subject as nvarchar(60)

BEGIN
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, QuoteNumber, ShipperName, ShipperReference1, ShipperReference2, ConsigneeName, ConsigneeReference1, ConsigneeReference2, CustomerName, CustomerReference1, CustomerReference2, CustomerContactId, FromPortId, ToPortId, MainCarriageCarrierId, Subject
		FROM Quotes
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteNumber, @ShipperName, @ShipperReference1, @ShipperReference2, @ConsigneeName, @ConsigneeReference1, @ConsigneeReference2, @CustomerName, @CustomerReference1, @CustomerReference2, @CustomerContactId, @FromPortId, @ToPortId, @MainCarriageCarrierId, @Subject
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@QuoteNumber is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @QuoteNumber
				else set @MySearchFields = @MySearchFields + ',' + @QuoteNumber	
			end

			if (@ShipperName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ShipperName
				else set @MySearchFields = @MySearchFields + ',' + @ShipperName	
			end

			if (@ShipperReference1 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ShipperReference1
				else set @MySearchFields = @MySearchFields + ',' + @ShipperReference1	
			end

			if (@ShipperReference2 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ShipperReference2
				else set @MySearchFields = @MySearchFields + ',' + @ShipperReference2	
			end

			if (@ConsigneeName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ConsigneeName
				else set @MySearchFields = @MySearchFields + ',' + @ConsigneeName	
			end

			if (@ConsigneeReference1 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ConsigneeReference1
				else set @MySearchFields = @MySearchFields + ',' + @ConsigneeReference1	
			end

			if (@ConsigneeReference2 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ConsigneeReference2
				else set @MySearchFields = @MySearchFields + ',' + @ConsigneeReference2	
			end

			if (@CustomerName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @CustomerName
				else set @MySearchFields = @MySearchFields + ',' + @CustomerName	
			end

			if (@CustomerReference1 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @CustomerReference1
				else set @MySearchFields = @MySearchFields + ',' + @CustomerReference1	
			end

			if (@CustomerReference2 is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @CustomerReference2
				else set @MySearchFields = @MySearchFields + ',' + @CustomerReference2	
			end

			if (@CustomerContactId is not null)
			begin
				set @MySearchFields = @MySearchFields + ',' + (select Email from Contacts where Id = @CustomerContactId)
			end

			if (@FromPortId is not null)
			begin
				set @MySearchFields = @MySearchFields + ',' + (select Code from Ports where Id = @FromPortId)
				set @MySearchFields = @MySearchFields + ',' + (select EnglishName from Ports where Id = @FromPortId)
			end

			if (@ToPortId is not null)
			begin
				set @MySearchFields = @MySearchFields + ',' + (select Code from Ports where Id = @ToPortId)
				set @MySearchFields = @MySearchFields + ',' + (select EnglishName from Ports where Id = @ToPortId)
			end

			if (@MainCarriageCarrierId is not null)
			begin
				set @MySearchFields = @MySearchFields + ',' + (select Code from Cards where Id = @MainCarriageCarrierId)
				set @MySearchFields = @MySearchFields + ',' + (select EnglishName from Cards where Id = @MainCarriageCarrierId)
			end

			if (@Subject is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Subject
				else set @MySearchFields = @MySearchFields + ',' + @Subject	
			end

			update Quotes set SearchFields = @MySearchFields where Id = @QuoteId AND Tenant = @Tenant

		FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteNumber, @ShipperName, @ShipperReference1, @ShipperReference2, @ConsigneeName, @ConsigneeReference1, @ConsigneeReference2, @CustomerName, @CustomerReference1, @CustomerReference2, @CustomerContactId, @FromPortId, @ToPortId, @MainCarriageCarrierId, @Subject
		END				
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END