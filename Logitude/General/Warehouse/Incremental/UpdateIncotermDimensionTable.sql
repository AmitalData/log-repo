
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Incoterm' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Incoterms )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

    declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @SourceTenant int
   declare @ParentTenant int
   

	DECLARE IncotermsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name , LocalName ,Code, Tenant,Tenant
	From dw_Incoterms
	where AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN IncotermsCursor FETCH NEXT FROM IncotermsCursor INTO @Id , @Name, @LocalName, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_Incoterms where Id = @Id)
	if(@Key is  null) begin  insert into DIM_Incoterms values(@Id,@Name,@LocalName,@Code ,@SourceTenant , @ParentTenant); end
	else begin update   DIM_Currencies set Name =@Name,  LocalName =@LocalName ,  Code = @Code ,SourceTenant = @SourceTenant , ParentTenant = @ParentTenant  Where Id = @Id; end
    

	FETCH NEXT FROM IncotermsCursor  INTO @Id , @Name, @LocalName, @Code,@SourceTenant , @ParentTenant
		End
	CLOSE IncotermsCursor
	DEALLOCATE IncotermsCursor
	
	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Incoterm'

End
