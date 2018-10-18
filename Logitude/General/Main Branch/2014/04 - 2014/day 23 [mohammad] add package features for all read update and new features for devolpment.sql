
select * from features where Packagable=0 and( Code='read' or code='update' or code='new' ) 
update features set Packagable=1 where Packagable=0 and( Code='read' or code='update' or code='new' )  
-- do update tenant 0 and update crm and aupdate social before executing this script .

BEGIN;
declare @Tenant as int
declare @Id as varchar(15)

	DECLARE featuresCursor CURSOR READ_ONLY
	FOR
	SELECT Tenant, Id
	From Features
	where Packagable=1 and( Code='read' or code='update' or code='new' )
	OPEN featuresCursor FETCH NEXT FROM featuresCursor INTO @Tenant, @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	if(not (@Id=any(select FeatureId from PackageFeatures where FeatureId=@Id)))
	begin
	declare @packageFeatureId varchar(15)
	EXECUTE usp_GetNextTableIdValue @packageFeatureId OUTPUT,'PackageFeature'
	insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'DVMT',@Tenant)
    end
	FETCH NEXT FROM featuresCursor INTO @Tenant, @Id
	END
	CLOSE featuresCursor
	DEALLOCATE featuresCursor
END

