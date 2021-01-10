
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'QuoteClosingReason' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_QuoteClosingReasons )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15) 
   declare @Name as varchar(60)
   declare @Code as varchar(2)   
   declare @Tenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE QuoteClosingReasonsCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Name, Code, dw_DWHSettings.Tenant, dw_QuoteClosingReasons.AutomaticLastUpdateDate
	From dw_QuoteClosingReasons
	inner JOIN dw_DWHSettings ON dw_QuoteClosingReasons.Tenant = dw_DWHSettings.Tenant
	where dw_QuoteClosingReasons.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN QuoteClosingReasonsCursor FETCH NEXT FROM QuoteClosingReasonsCursor INTO   @Id , @Name, @Code, @Tenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_QuoteClosingReasons where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_QuoteClosingReasons (Id,[Name], Code, [Tenant],[Automatic Last Update Date])
	values(@Id , @Name, @Code, @Tenant, @AutomaticLastUpdateDate) end
	else begin update  
	DIM_QuoteClosingReasons set [Code] =@Code, [Name] =@Name, [Tenant] = @Tenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate 
	Where Id = @Id end


	FETCH NEXT FROM QuoteClosingReasonsCursor INTO   @Id ,@Name, @Code,@Tenant, @AutomaticLastUpdateDate
		End
	CLOSE QuoteClosingReasonsCursor
	DEALLOCATE QuoteClosingReasonsCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'QuoteClosingReason'
End

