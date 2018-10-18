 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Branche' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Branches )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE BranchesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, Tenant,Tenant
	From dw_Branches
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Branches where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_Branches values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant) end
	else begin update   DIM_Branches set Name =@EnglishName,  LocalName =@LocalName ,  Code = @Code , SourceTenant = @SourceTenant , ParentTenant = @ParentTenant where Id = @Id; end

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor
	
	End


	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Branche'