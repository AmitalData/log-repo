

   declare @Code as varchar(2)
   declare @Name as varchar(20) 
   declare @AutomaticLastUpdateDate as datetime
    
	
	DECLARE InvoiceTypesCursor CURSOR READ_ONLY
	FOR

	with InvoiceTypes as(select Code,Name, AutomaticLastUpdateDate
	from(
		select Code, Name, AutomaticLastUpdateDate
		from dw_ARInvoiceTypes
         
		union all

		select Code, Name, AutomaticLastUpdateDate
		from dw_APInvoiceTypes t2
        
		where not exists(select Code from dw_ARInvoiceTypes t1 where t2.Code = t1.Code )

		union all

		select  Code, Name, AutomaticLastUpdateDate
		from dw_APInvoiceTypes t2
        where exists ( select Code from dw_ARInvoiceTypes t1  where t2.Code <> t1.Code and t2.Name = t1.Name )

	
)tt
)

	SELECT   InvoiceTypes.Code , InvoiceTypes.Name, InvoiceTypes.AutomaticLastUpdateDate
 
     From InvoiceTypes

OPEN InvoiceTypesCursor FETCH NEXT FROM InvoiceTypesCursor INTO @Code, @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_InvoiceTypesTemp (Code, Name, [Automatic Last Update Date]) values (@Code, @Name, @AutomaticLastUpdateDate)

	FETCH NEXT FROM InvoiceTypesCursor  INTO @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceTypesCursor
	DEALLOCATE InvoiceTypesCursor
 

	