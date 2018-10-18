-- do not run online - already exisit
DECLARE @Tenant AS INT
declare @stateid1 varchar(15)
declare @stateid2 varchar(15)
declare @stateid3 varchar(15)
declare @stateid4 varchar(15)
declare @stateid5 varchar(15)
declare @stateid6 varchar(15)
declare @stateid7 varchar(15)
declare @countryid varchar(15)


	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	print @Tenant
	set @countryid=(select id from Countries where code='AU' and Tenant=@Tenant)
	print @countryid
	if(@countryid is not null)
	begin

	declare @qld varchar(10)
	set @qld=(select code from States where Code='QLD' and Tenant=@Tenant and CountryId = @countryid)
	if(@qld is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid1 OUTPUT,'State'
	print @stateid1
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid1,@Tenant,'QLD','Queensland','Queensland',0,NULL,@countryid,0,'QLD,Queensland')
	end

	declare @nsw varchar(10)
	set @nsw=(select code from States where Code='NSW' and Tenant=@Tenant and CountryId = @countryid)
	if(@nsw is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid2 OUTPUT,'State'
	print @stateid2
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid2,@Tenant,'NSW','New South Wales','New South Wales',0,NULL,@countryid,0,'NSW,New South Wales')
	end

	declare @vic varchar(10)
	set @vic=(select code from States where Code='VIC' and Tenant=@Tenant and CountryId = @countryid)
	if(@vic is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid3 OUTPUT,'State'
	print @stateid3
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid3,@Tenant,'VIC','Victoria','Victoria',0,NULL,@countryid,0,'VIC,Victoria')
	end

	declare @tas varchar(10)
	set @tas=(select code from States where Code='TAS' and Tenant=@Tenant and CountryId = @countryid)
	if(@tas is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid4 OUTPUT,'State'
	print @stateid4
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid4,@Tenant,'TAS','Tasmania','Tasmania',0,NULL,@countryid,0,'TAS,Tasmania')
	end

	declare @sa varchar(10)
	set @sa=(select code from States where Code='SA' and Tenant=@Tenant and CountryId = @countryid)
	if(@sa is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid5 OUTPUT,'State'
	print @stateid5
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid5,@Tenant,'SA','South Australia','South Australia',0,NULL,@countryid,0,'SA,South Australia')
	end

	declare @wa varchar(10)
	set @wa=(select code from States where Code='WA' and Tenant=@Tenant and CountryId = @countryid)
	if(@wa is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid6 OUTPUT,'State'
	print @stateid6
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid6,@Tenant,'WA','Western Australia','Western Australia',0,NULL,@countryid,0,'WA,Western Australia')
	end

	declare @nt varchar(10)
	set @nt=(select code from States where Code='NT' and Tenant=@Tenant and CountryId = @countryid)
	if(@nt is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid7 OUTPUT,'State'
	print @stateid7
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid7,@Tenant,'NT','Northern Territory','Northern Territory',0,NULL,@countryid,0,'NT,Northern Territory')
	end
	
	
	
	
	
	
	

	end
	FETCH NEXT FROM TEUCursor INTO @Tenant
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor