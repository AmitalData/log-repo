
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoiceStatus' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoiceStatus )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Code as varchar(2)
   declare @Name as varchar(20)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE InvoiceStatusCursor CURSOR READ_ONLY
	FOR
    SELECT Code, Name, dw_ARInvoiceStatus.AutomaticLastUpdateDate
	From dw_ARInvoiceStatus 
	where dw_ARInvoiceStatus.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN InvoiceStatusCursor FETCH NEXT FROM InvoiceStatusCursor INTO   @Code, @Name, @AutomaticLastUpdateDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Name from DIM_InvoiceStatus where Name = @Name)
	
	if(@Key is  null) begin     insert into DIM_InvoiceStatus (Code, Name,[Automatic Last Update Date]) values(@Code, @Name, @AutomaticLastUpdateDate) end
	else begin update   DIM_InvoiceStatus set [Code] =@Code, [Name] =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Name = @Name end


	FETCH NEXT FROM InvoiceStatusCursor INTO   @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceStatusCursor
	DEALLOCATE InvoiceStatusCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ARInvoiceStatus'
End

