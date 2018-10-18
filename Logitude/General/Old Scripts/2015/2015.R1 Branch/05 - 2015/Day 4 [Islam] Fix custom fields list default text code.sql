
 
BEGIN;

declare @Tenant as int
declare @Id as varchar(15)
declare @FullNameTextCodeId as varchar(15)
declare @ListTextCodeId as varchar(15)
	DECLARE CustomFieldsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,tenant,fullnametextcodeid,listtextcodeid
	From ObjectFields
	Where IsCustom = 1
	OPEN CustomFieldsCursor FETCH NEXT FROM CustomFieldsCursor INTO @Id,@Tenant,@FullNameTextCodeId,@ListTextCodeId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	declare @FullNameDefaultText as varchar(250)
	declare @ListDefaultText as varchar(250)

	set @FullNameDefaultText = (select defaulttext from TextCodes where id = @FullNameTextCodeId and tenant = @Tenant)
	set @ListDefaultText = (select defaulttext from TextCodes where id = @ListTextCodeId and tenant = @Tenant)

	if(@FullNameDefaultText != @ListDefaultText)
	begin
	update TextCodes set DefaultText = @FullNameDefaultText where id  =  @ListTextCodeId and tenant = @Tenant

	print @FullNameDefaultText +'--' + @ListDefaultText
	 
	end
	
	
   
	FETCH NEXT FROM CustomFieldsCursor INTO  @Id,@Tenant,@FullNameTextCodeId,@ListTextCodeId
	END
	CLOSE CustomFieldsCursor
	DEALLOCATE CustomFieldsCursor
END

select * from ObjectFields where tenant = 211

select * from textcodes where id in (select fullnametextcodeid from objectfields where tenant = 211)
select * from textcodes where id in (select listtextcodeid from objectfields where tenant = 211)


