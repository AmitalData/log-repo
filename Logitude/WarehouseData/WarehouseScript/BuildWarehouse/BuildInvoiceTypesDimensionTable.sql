

   declare @Code as varchar(2)
   declare @Name as varchar(20) 
    
	
	DECLARE InvoiceTypesCursor CURSOR READ_ONLY
	FOR

	with InvoiceTypes as(select Code,Name
	from(
		select Code, Name
		from dw_ARInvoiceTypes
         
		union all

		select Code, Name
		from dw_APInvoiceTypes t2
        
		where not exists(select Code from dw_ARInvoiceTypes t1 where t2.Code = t1.Code )

		union all
  

		select  Code, Name
		from dw_APInvoiceTypes t2
        where exists ( select Code from dw_ARInvoiceTypes t1 where  t2.Name <> t1.Name )

	
)tt
)

	SELECT   InvoiceTypes.Code , InvoiceTypes.Name
 
     From InvoiceTypes

OPEN InvoiceTypesCursor FETCH NEXT FROM InvoiceTypesCursor INTO @Code, @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_InvoiceTypesTemp (Code, Name) values (@Code, @Name)

	FETCH NEXT FROM InvoiceTypesCursor  INTO @Code, @Name
		End
	CLOSE InvoiceTypesCursor
	DEALLOCATE InvoiceTypesCursor
 

	