

	
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoiceTransferStatus' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoiceTransferStatus )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Code as varchar(2)
   declare @Name as nvarchar(20)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ARInvoiceTransferStatusCursor CURSOR READ_ONLY
	FOR
    SELECT Code, [Name], dw_ARInvoiceTransferStatus.AutomaticLastUpdateDate
	From dw_ARInvoiceTransferStatus
	where dw_ARInvoiceTransferStatus.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN ARInvoiceTransferStatusCursor FETCH NEXT FROM ARInvoiceTransferStatusCursor INTO  @Code, @Name,@AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	set @Key = (select Name from DIM_ARInvoiceTransferStatus where Name = @Name)
	
	if(@Key is  null) begin     insert into DIM_ARInvoiceTransferStatus (Code, Name,[Automatic Last Update Date]) values(@Code, @Name, @AutomaticLastUpdateDate) end
	else begin update   DIM_ARInvoiceTransferStatus set [Code] =@Code, [Name] =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Name = @Name end


	FETCH NEXT FROM ARInvoiceTransferStatusCursor INTO  @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE ARInvoiceTransferStatusCursor
	DEALLOCATE ARInvoiceTransferStatusCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ARInvoiceTransferStatus'
End


