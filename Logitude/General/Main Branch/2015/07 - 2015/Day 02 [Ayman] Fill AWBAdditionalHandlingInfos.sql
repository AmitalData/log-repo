
declare @Id as varchar(15)
declare @Tenant as int
declare @Code as varchar(5)
declare @Name as varchar(60)
declare @Description as varchar(60)
declare @SearchFields as nvarchar(1000)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- KCKC
		BEGIN
			set @Code = 'KCKC'
			set @Name = 'Known Cargo / Known Consignor'
			set @Description = 'Known Cargo / Known Consignor'
			set @SearchFields = @Code + ',' + @Name + ',' + @Description
			if exists (select * from AWBAdditionalHandlingInfos where Tenant = @Tenant AND Code = @Code)
			update AWBAdditionalHandlingInfos
			set Name = @Name, PrintDescription = @Description, SearchFields = @SearchFields
			Where Tenant = @Tenant AND Code = @Code
			else
			begin
			EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBAdditionalHandlingInfo'
			insert into AWBAdditionalHandlingInfos (Id, Tenant, Code, Name, PrintDescription, SearchFields)
			values	(@Id, @Tenant, @Code, @Name, @Description, @SearchFields)
			end
		END

		-- UNK
		BEGIN
			set @Code = 'UNK'
			set @Name = 'If not Known Consignor'
			set @Description = 'RA-UNK'
			set @SearchFields = @Code + ',' + @Name + ',' + @Description
			if exists (select * from AWBAdditionalHandlingInfos where Tenant = @Tenant AND Code = @Code)
			update AWBAdditionalHandlingInfos
			set Name = @Name, PrintDescription = @Description, SearchFields = @SearchFields
			Where Tenant = @Tenant AND Code = @Code
			else
			begin
			EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBAdditionalHandlingInfo'
			insert into AWBAdditionalHandlingInfos (Id, Tenant, Code, Name, PrintDescription, SearchFields)
			values	(@Id, @Tenant, @Code, @Name, @Description, @SearchFields)
			end
		END

		-- UKKC
		BEGIN
			set @Code = 'UKKC'
			set @Name = 'Unknown Cargo / Known Consignor'
			set @Description = 'Unknown Cargo / Known Consignor'
			set @SearchFields = @Code + ',' + @Name + ',' + @Description
			if exists (select * from AWBAdditionalHandlingInfos where Tenant = @Tenant AND Code = @Code)
			update AWBAdditionalHandlingInfos
			set Name = @Name, PrintDescription = @Description, SearchFields = @SearchFields
			Where Tenant = @Tenant AND Code = @Code
			else
			begin
			EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBAdditionalHandlingInfo'
			insert into AWBAdditionalHandlingInfos (Id, Tenant, Code, Name, PrintDescription, SearchFields)
			values	(@Id, @Tenant, @Code, @Name, @Description, @SearchFields)
			end
		END

	FETCH NEXT FROM DataCursor INTO  @Tenant
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END