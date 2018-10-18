
 -- select PackageCode from PackageFeatures group by PackageCode

if not exists (select * from PackageFeatures where len(PackageCode) = 5)
BEGIN

declare @Id as varchar(15)
declare @Code as varchar(5)
declare @NewCode as varchar(5)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, PackageCode
	FROM PackageFeatures
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Code
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @NewCode = @Code + 'B'

	update PackageFeatures set PackageCode = @NewCode where Id = @Id

	FETCH NEXT FROM DataCursor INTO @Id, @Code
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END

END

