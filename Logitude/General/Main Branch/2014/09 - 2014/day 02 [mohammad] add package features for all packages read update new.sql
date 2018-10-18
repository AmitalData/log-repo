
--select * from features where Packagable=0 and( Code='read' or code='update' or code='new' ) 
--select * from features where Packagable=1 and( Code='read' or code='update' or code='new' ) 
--select * from Packages
  
---- do update tenant 0 and update crm and aupdate social before executing this script .
---- only one time.
update features set Packagable=1 where Packagable=0 and( Code='read' or code='update' or code='new' )
--for all packages.
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
	
	if(not (@Id = any(select FeatureId from PackageFeatures where FeatureId=@Id and PackageCode='DVMT')))
	begin
	declare @packageFeatureId varchar(15)
	EXECUTE usp_GetNextTableIdValue @packageFeatureId OUTPUT,'PackageFeature'
	insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'DVMT',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'BUSN',@Tenant)
	--insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'BASC',@Tenant)
	--insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'BUCH',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'CRMM',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'CUST',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'EAWB',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'ECON',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'HBRD',@Tenant)
    --insert into PackageFeatures (Id,FeatureId,PackageCode,Tenant) values(@packageFeatureId,@Id,'LOGI',@Tenant)

   end
	FETCH NEXT FROM featuresCursor INTO @Tenant, @Id
	END
	CLOSE featuresCursor
	DEALLOCATE featuresCursor
END
--select * from PackageFeatures where PackageCode='LOGI' and FeatureId in(select id from Features where Packagable=1 and( Code='read' or code='update' or code='new' ))
