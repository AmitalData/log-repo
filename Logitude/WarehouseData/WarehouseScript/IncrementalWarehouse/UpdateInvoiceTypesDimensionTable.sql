
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoiceType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoiceTypes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Code as varchar(2)
   declare @Name as varchar(20)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE InvoiceTypesCursor CURSOR READ_ONLY
	FOR
    SELECT Code, Name, dw_ARInvoiceTypes.AutomaticLastUpdateDate
	From dw_ARInvoiceTypes 
	where dw_ARInvoiceTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN InvoiceTypesCursor FETCH NEXT FROM InvoiceTypesCursor INTO   @Code, @Name, @AutomaticLastUpdateDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Name from DIM_InvoiceTypes where Name = @Name)
	
	if(@Key is  null) begin     insert into DIM_InvoiceTypes (Code, Name,[Automatic Last Update Date]) values(@Code, @Name, @AutomaticLastUpdateDate) end
	else begin update   DIM_InvoiceTypes set [Code] =@Code, [Name] =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Name = @Name end


	FETCH NEXT FROM InvoiceTypesCursor INTO   @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceTypesCursor
	DEALLOCATE InvoiceTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ARInvoiceTypes'
End


 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'APInvoiceType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_APInvoiceTypes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Code as varchar(2)
   declare @Name as varchar(20)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE InvoiceTypesCursor CURSOR READ_ONLY
	FOR
    SELECT Code, Name, dw_APInvoiceTypes.AutomaticLastUpdateDate
	From dw_APInvoiceTypes 
	where dw_APInvoiceTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN InvoiceTypesCursor FETCH NEXT FROM InvoiceTypesCursor INTO   @Code, @Name, @AutomaticLastUpdateDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Name from DIM_InvoiceTypes where Name = @Name)
	
	if(@Key is  null) begin     insert into DIM_InvoiceTypes (Code, Name,[Automatic Last Update Date]) values(@Code, @Name, @AutomaticLastUpdateDate) end
	else begin update   DIM_InvoiceTypes set [Code] =@Code, [Name] =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Name = @Name end


	FETCH NEXT FROM InvoiceTypesCursor INTO   @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceTypesCursor
	DEALLOCATE InvoiceTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'APInvoiceTypes'
End
