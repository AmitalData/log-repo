-- main db

DECLARE @PackageCode AS VARCHAR
BEGIN;

	DECLARE PackageCursor CURSOR READ_ONLY
	FOR	
	SELECT Code
	FROM Packages	 
	OPEN PackageCursor FETCH NEXT FROM PackageCursor INTO @PackageCode
	WHILE @@FETCH_STATUS = 0
		BEGIN

		delete from PackageFeatures where FeatureId in (select Id from Features where code = 'CHAMP')
		delete from RoleFeatures where FeatureId in (select Id from Features where code = 'CHAMP')
		delete from ObjectTableTabs where FeatureId in (select Id from Features where code = 'CHAMP')

	FETCH NEXT FROM PackageCursor INTO @PackageCode
	END
	CLOSE PackageCursor
	DEALLOCATE PackageCursor	
END
go

delete from Features where code = 'champ'
go

