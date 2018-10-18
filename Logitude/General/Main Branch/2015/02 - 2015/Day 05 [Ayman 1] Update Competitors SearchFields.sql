
declare @Tenant as int
declare @Id as varchar(15)
declare @Name as nvarchar(60)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Name
		FROM Competitors
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @Name
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @MySearchFields = ''

				if (@Name is not null)
				begin
					set @MySearchFields = @Name
				end		

			update Competitors set SearchFields = @MySearchFields where Id = @Id AND Tenant = @Tenant

			FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @Name	
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END