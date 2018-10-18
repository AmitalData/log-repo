DECLARE @Tenant AS INT
declare @stateid1 varchar(15)
declare @stateid2 varchar(15)
declare @stateid3 varchar(15)
declare @stateid4 varchar(15)
declare @stateid5 varchar(15)
declare @stateid6 varchar(15)
declare @stateid7 varchar(15)
declare @stateid8 varchar(15)
declare @stateid9 varchar(15)
declare @stateid10 varchar(15)
declare @stateid11 varchar(15)
declare @stateid12 varchar(15)
declare @stateid13 varchar(15)
declare @stateid14 varchar(15)
declare @stateid15 varchar(15)
declare @stateid16 varchar(15)
declare @stateid17 varchar(15)
declare @stateid18 varchar(15)
declare @stateid19 varchar(15)
declare @stateid20 varchar(15)
declare @stateid21 varchar(15)
declare @stateid22 varchar(15)
declare @stateid23 varchar(15)
declare @stateid24 varchar(15)
declare @stateid25 varchar(15)
declare @stateid26 varchar(15)
declare @stateid27 varchar(15)
declare @stateid28 varchar(15)
declare @stateid29 varchar(15)
declare @stateid30 varchar(15)
declare @stateid31 varchar(15)
declare @stateid32 varchar(15)
declare @countryid varchar(15)


	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	print @Tenant
	set @countryid=(select id from Countries where code='MX' and Tenant=@Tenant)
	print @countryid
	if(@countryid is not null)
	begin
	update countries set HasStates=1 where Id=@countryid
    update countries set IsStateRequired=1 where Id=@countryid


	declare @AGU varchar(10)
	set @AGU=(select code from States where Code='AGU' and Tenant=@Tenant and CountryId = @countryid)
	if(@AGU is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid1 OUTPUT,'State'
	print @stateid1
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid1,@Tenant,'AGU','Aguascalientes','Aguascalientes',0,NULL,@countryid,0,'AGU,Aguascalientes')
	end

	declare @BCN varchar(10)
	set @BCN=(select code from States where Code='BCN' and Tenant=@Tenant and CountryId = @countryid)
	if(@BCN is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid2 OUTPUT,'State'
	print @stateid2
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid2,@Tenant,'BCN','Baja California','Baja California',0,NULL,@countryid,0,'BCN,Baja California')
	end

	declare @BCS varchar(10)
	set @BCS=(select code from States where Code='BCS' and Tenant=@Tenant and CountryId = @countryid)
	if(@BCS is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid3 OUTPUT,'State'
	print @stateid3
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid3,@Tenant,'BCS','Baja California Sur','Baja California Sur',0,NULL,@countryid,0,'BCS,Baja California Sur')
	end

	declare @CAM varchar(10)
	set @CAM=(select code from States where Code='CAM' and Tenant=@Tenant and CountryId = @countryid)
	if(@CAM is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid4 OUTPUT,'State'
	print @stateid4
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid4,@Tenant,'CAM','Campeche','Campeche',0,NULL,@countryid,0,'CAM,Campeche')
	end

	declare @CHP varchar(10)
	set @CHP=(select code from States where Code='CHP' and Tenant=@Tenant and CountryId = @countryid)
	if(@CHP is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid5 OUTPUT,'State'
	print @stateid5
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid5,@Tenant,'CHP','Chiapas','Chiapas',0,NULL,@countryid,0,'CHP,Chiapas')
	end

	declare @CHH varchar(10)
	set @CHH=(select code from States where Code='CHH' and Tenant=@Tenant and CountryId = @countryid)
	if(@CHH is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid6 OUTPUT,'State'
	print @stateid6
	insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid6,@Tenant,'CHH','Chihuahua','Chihuahua',0,NULL,@countryid,0,'CHH,Chihuahua')
	end

	declare @COA varchar(10)
	set @COA=(select code from States where Code='COA' and Tenant=@Tenant and CountryId = @countryid)
	if(@COA is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid7 OUTPUT,'State'
	print @stateid7
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid7,@Tenant,'COA','Coahuila','Coahuila',0,NULL,@countryid,0,'COA,Coahuila')
	end


	declare @COL varchar(10)
	set @COL=(select code from States where Code='COL' and Tenant=@Tenant and CountryId = @countryid)
	if(@COL is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid8 OUTPUT,'State'
	print @stateid8
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid8,@Tenant,'COL','Colima','Colima',0,NULL,@countryid,0,'COL,Colima')
	end


	declare @CMX varchar(10)
	set @CMX=(select code from States where Code='CMX' and Tenant=@Tenant and CountryId = @countryid)
	if(@CMX is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid9 OUTPUT,'State'
	print @stateid9
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid9,@Tenant,'CMX','Mexico City','Mexico City',0,NULL,@countryid,0,'CMX,Mexico City')
	end


	declare @DUR varchar(10)
	set @DUR=(select code from States where Code='DUR' and Tenant=@Tenant and CountryId = @countryid)
	if(@DUR is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid10 OUTPUT,'State'
	print @stateid10
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid10,@Tenant,'DUR','Durango','Durango',0,NULL,@countryid,0,'DUR,Durango')
	end

	declare @GUA varchar(10)
	set @GUA=(select code from States where Code='GUA' and Tenant=@Tenant and CountryId = @countryid)
	if(@GUA is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid11 OUTPUT,'State'
	print @stateid11
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid11,@Tenant,'GUA','Guanajuato','Guanajuato',0,NULL,@countryid,0,'GUA,Guanajuato')
	end

	declare @GRO varchar(10)
	set @GRO=(select code from States where Code='GRO' and Tenant=@Tenant and CountryId = @countryid)
	if(@GRO is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid12 OUTPUT,'State'
	print @stateid12
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid12,@Tenant,'GRO','Guerrero','Guerrero',0,NULL,@countryid,0,'GRO,Guerrero')
	end


	declare @HID varchar(10)
	set @HID=(select code from States where Code='HID' and Tenant=@Tenant and CountryId = @countryid)
	if(@HID is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid13 OUTPUT,'State'
	print @stateid13
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid13,@Tenant,'HID','Hidalgo','Hidalgo',0,NULL,@countryid,0,'HID,Hidalgo')
	end

	declare @JAL varchar(10)
	set @JAL=(select code from States where Code='JAL' and Tenant=@Tenant and CountryId = @countryid)
	if(@JAL is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid14 OUTPUT,'State'
	print @stateid14
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid14,@Tenant,'JAL','Jalisco','Jalisco',0,NULL,@countryid,0,'JAL,Jalisco')
	end

	declare @MEX varchar(10)
	set @MEX=(select code from States where Code='MEX' and Tenant=@Tenant and CountryId = @countryid)
	if(@MEX is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid15 OUTPUT,'State'
	print @stateid15
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid15,@Tenant,'MEX','México','México',0,NULL,@countryid,0,'MEX,México')
	end

	declare @MIC varchar(10)
	set @MIC=(select code from States where Code='MIC' and Tenant=@Tenant and CountryId = @countryid)
	if(@MIC is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid16 OUTPUT,'State'
	print @stateid16
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid16,@Tenant,'MIC','Michoacán','Michoacán',0,NULL,@countryid,0,'MIC,Michoacán')
	end

	declare @MOR varchar(10)
	set @MOR=(select code from States where Code='MOR' and Tenant=@Tenant and CountryId = @countryid)
	if(@MOR is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid17 OUTPUT,'State'
	print @stateid17
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid17,@Tenant,'MOR','Morelos','Morelos',0,NULL,@countryid,0,'MOR,Morelos')
	end

	declare @NAY varchar(10)
	set @NAY=(select code from States where Code='NAY' and Tenant=@Tenant and CountryId = @countryid)
	if(@NAY is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid18 OUTPUT,'State'
	print @stateid18
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid18,@Tenant,'NAY','Nayarit','Nayarit',0,NULL,@countryid,0,'NAY,Nayarit')
	end

	declare @NLE varchar(10)
	set @NLE=(select code from States where Code='NLE' and Tenant=@Tenant and CountryId = @countryid)
	if(@NLE is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid19 OUTPUT,'State'
	print @stateid19
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid19,@Tenant,'NLE','Nuevo León','Nuevo León',0,NULL,@countryid,0,'NLE,Nuevo León')
	end


	declare @OAX varchar(10)
	set @OAX=(select code from States where Code='OAX' and Tenant=@Tenant and CountryId = @countryid)
	if(@OAX is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid20 OUTPUT,'State'
	print @stateid20
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid20,@Tenant,'OAX','Oaxaca','Oaxaca',0,NULL,@countryid,0,'OAX,Oaxaca')
	end


	declare @PUE varchar(10)
	set @PUE=(select code from States where Code='PUE' and Tenant=@Tenant and CountryId = @countryid)
	if(@PUE is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid21 OUTPUT,'State'
	print @stateid21
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid21,@Tenant,'PUE','Puebla','Puebla',0,NULL,@countryid,0,'PUE,Puebla')
	end


	declare @QUE varchar(10)
	set @QUE=(select code from States where Code='QUE' and Tenant=@Tenant and CountryId = @countryid)
	if(@QUE is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid22 OUTPUT,'State'
	print @stateid22
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid22,@Tenant,'QUE','Querétaro','Querétaro',0,NULL,@countryid,0,'QUE,Querétaro')
	end


	declare @ROO varchar(10)
	set @ROO=(select code from States where Code='ROO' and Tenant=@Tenant and CountryId = @countryid)
	if(@ROO is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid23 OUTPUT,'State'
	print @stateid23
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid23,@Tenant,'ROO','Quintana Roo','Quintana Roo',0,NULL,@countryid,0,'ROO,Quintana Roo')
	end

	declare @SLP varchar(10)
	set @SLP=(select code from States where Code='SLP' and Tenant=@Tenant and CountryId = @countryid)
	if(@SLP is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid24 OUTPUT,'State'
	print @stateid24
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid24,@Tenant,'SLP','San Luis Potosí','San Luis Potosí',0,NULL,@countryid,0,'SLP,San Luis Potosí')
	end


	declare @SIN varchar(10)
	set @SIN=(select code from States where Code='SIN' and Tenant=@Tenant and CountryId = @countryid)
	if(@SIN is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid25 OUTPUT,'State'
	print @stateid25
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid25,@Tenant,'SIN','Sinaloa','Sinaloa',0,NULL,@countryid,0,'SIN,Sinaloa')
	end

	
	declare @SON varchar(10)
	set @SON=(select code from States where Code='SON' and Tenant=@Tenant and CountryId = @countryid)
	if(@SON is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid26 OUTPUT,'State'
	print @stateid26
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid26,@Tenant,'SON','Sonora','Sonora',0,NULL,@countryid,0,'SON,Sonora')
	end

	declare @TAB varchar(10)
	set @TAB=(select code from States where Code='TAB' and Tenant=@Tenant and CountryId = @countryid)
	if(@TAB is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid27 OUTPUT,'State'
	print @stateid27
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid27,@Tenant,'TAB','Tabasco','Tabasco',0,NULL,@countryid,0,'TAB,Tabasco')
	end

	declare @TLA varchar(10)
	set @TLA=(select code from States where Code='TLA' and Tenant=@Tenant and CountryId = @countryid)
	if(@TLA is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid28 OUTPUT,'State'
	print @stateid28
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid28,@Tenant,'TLA','Tlaxcala','Tlaxcala',0,NULL,@countryid,0,'TLA,Tlaxcala')
	end


	declare @VER varchar(10)
	set @VER=(select code from States where Code='VER' and Tenant=@Tenant and CountryId = @countryid)
	if(@VER is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid29 OUTPUT,'State'
	print @stateid29
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid29,@Tenant,'VER','Veracruz','Veracruz',0,NULL,@countryid,0,'VER,Veracruz')
	end

	declare @YUC varchar(10)
	set @YUC=(select code from States where Code='YUC' and Tenant=@Tenant and CountryId = @countryid)
	if(@YUC is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid30 OUTPUT,'State'
	print @stateid30
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid30,@Tenant,'YUC','Yucatán','Yucatán',0,NULL,@countryid,0,'YUC,Yucatán')
	end


	declare @ZAC varchar(10)
	set @ZAC=(select code from States where Code='ZAC' and Tenant=@Tenant and CountryId = @countryid)
	if(@ZAC is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid31 OUTPUT,'State'
	print @stateid31
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid31,@Tenant,'ZAC','Zacatecas','Zacatecas',0,NULL,@countryid,0,'ZAC,Zacatecas')
	end

	declare @TAM varchar(10)
	set @TAM=(select code from States where Code='TAM' and Tenant=@Tenant and CountryId = @countryid)
	if(@TAM is null)
	begin
	EXECUTE usp_GetNextTableIdValue @stateid32 OUTPUT,'State'
	print @stateid32
    insert into States(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,CountryId,AddedManually,SearchFields) values(@stateid32,@Tenant,'TAM',' Tamaulipas',' Tamaulipas',0,NULL,@countryid,0,'TAM, Tamaulipas')
	end
	
	
	end
	FETCH NEXT FROM TEUCursor INTO @Tenant
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor