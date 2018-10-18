 
BEGIN;
declare @Id as varchar(15)
declare @Tenant as int
declare @NameTextCodeId as varchar(30)
declare @Code as varchar(30)
declare @ObjectTableId as varchar(15)
declare @textcodecode varchar(200)
	DECLARE queiresCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,NameTextCodeId,Code,ObjectTableId
	From Queries
	where Tenant <> 0
	OPEN queiresCursor FETCH NEXT FROM queiresCursor INTO @Id,@Tenant,@NameTextCodeId,@Code,@ObjectTableId
	WHILE @@FETCH_STATUS = 0
	BEGIN


	
	if(@NameTextCodeId is null)
	begin
	    
	 
		declare @tableName varchar(50)
		declare @textCodeId varchar(15)
		

		set @tableName = (select name from ObjectTables where Id = @ObjectTableId)
	    EXECUTE usp_GetNextTableIdValue @textCodeId OUTPUT,'TextCode'
	 

		set @textcodecode = @tableName + '.Q.' + @textCodeId

		 --delete from TextCodes where Code = @textcodecode
		insert into TextCodes(Id,Code,DefaultText,ObjectTableId,TextCodeTypeCode,Tenant,DefaultTextPlural,IsSpellChecked,InActive,LocalDefaultText)
		values(@textCodeId,@textcodecode,@Code,@ObjectTableId,'Q',@Tenant,@Code,0,0,@Code)

		update Queries set NameTextCodeId = @textCodeId where code = @Code
   
   print @textcodecode 
    print @Code 
   end
   else
   begin
    print @textcodecode + ' ================================> has text code'
	 
   end
   
	FETCH NEXT FROM queiresCursor INTO  @Id,@Tenant,@NameTextCodeId,@Code,@ObjectTableId
	END
	CLOSE queiresCursor
	DEALLOCATE queiresCursor
 End