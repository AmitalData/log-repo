
-- select PackageCode from PackageFeatures group by PackageCode

--update PackageFeatures set PackageCode = 'BASC' where PackageCode = 'BASCB'
--update PackageFeatures set PackageCode = 'BUSN' where PackageCode = 'BUSNB'
--update PackageFeatures set PackageCode = 'CUST' where PackageCode = 'CUSTB'
--update PackageFeatures set PackageCode = 'DVMT' where PackageCode = 'DVMTB'
--update PackageFeatures set PackageCode = 'EAWB' where PackageCode = 'EAWBB'
--update PackageFeatures set PackageCode = 'ECON' where PackageCode = 'ECONB'
--update PackageFeatures set PackageCode = 'TNT0' where PackageCode = 'TNT0B'
--update PackageFeatures set PackageCode = 'BUCH' where PackageCode = 'BUCHB'
--update PackageFeatures set PackageCode = 'HBRD' where PackageCode = 'HBRDB'
--update PackageFeatures set PackageCode = 'IMPO' where PackageCode = 'IMPOB'
--select PackageCode from PackageFeatures group by PackageCode

--delete from PackageConnectedPackages
--delete from Packages where Code in ('BASCB','BUSNB', 'CUSTB', 'DVMTB', 'EAWBB', 'ECONB', 'IMPOB', 'BUCHB', 'CRMMB', 'HBRDB', 'LOGIB', 'AIRLB', 'TNT0B', 'ARTNB')

if not exists (select * from Packages where len(Code) = 5)
BEGIN

declare @Code as varchar(5)
declare @Name as varchar(40)
declare @NewCode as varchar(5)
declare @NewId as varchar(15)

declare @MemoryTable table
(
  Code varchar(5),
  Name varchar(40)
)

BEGIN
	insert into @MemoryTable
	select Code, Name
	from Packages
END

BEGIN
	update Packages set FeaturePackageTypeCode = 'PK'
END

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	FROM @MemoryTable
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Code, @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @NewCode = @Code + 'B'
	if not exists (select * from Packages where Code = @NewCode)
	begin
		insert into Packages (Code, Name, SearchFields, FeaturePackageTypeCode, InActive)
		values
		(
		@NewCode,
		@Name,
		@NewCode + ',' + @Name,
		'BS',
		0
		)
	end

	if not exists (select * from PackageConnectedPackages where PackageCode = @Code AND ConnectedPackageCode = @NewCode)
	begin
		EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'PackageConnectedPackage'
		insert into PackageConnectedPackages (Id, PackageCode, ConnectedPackageCode) values (@NewId, @Code, @NewCode)
	end
	
	FETCH NEXT FROM DataCursor INTO @Code, @Name
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END

END