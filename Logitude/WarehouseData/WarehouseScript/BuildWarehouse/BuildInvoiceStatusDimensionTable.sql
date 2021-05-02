

   declare @Code as varchar(2)
   declare @Name as varchar(20) 
   declare @AutomaticLastUpdateDate as datetime
    
	
	DECLARE InvoiceStatusCursor CURSOR READ_ONLY
	FOR

	with InvoiceStatus as(select Code,Name, AutomaticLastUpdateDate
	from(
		select Code, Name, AutomaticLastUpdateDate
		from dw_ARInvoiceStatus
         
		union all

		select Code, Name, AutomaticLastUpdateDate
		from dw_APInvoiceStatus t2
        
		where not exists(select Code from dw_ARInvoiceStatus t1 where t2.Code = t1.Code )

		union all
  

		select  Code, Name, AutomaticLastUpdateDate
		from dw_APInvoiceStatus t2
        where exists ( select Code from dw_ARInvoiceStatus t1 where t2.Code = t1.Code and t2.Name <> t1.Name )

	
)tt
)

	SELECT   InvoiceStatus.Code, InvoiceStatus.Name, InvoiceStatus.AutomaticLastUpdateDate
 
     From InvoiceStatus

OPEN InvoiceStatusCursor FETCH NEXT FROM InvoiceStatusCursor INTO @Code, @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_InvoiceStatusTemp (Code, Name, [Automatic Last Update Date]) values (@Code, @Name, @AutomaticLastUpdateDate)

	FETCH NEXT FROM InvoiceStatusCursor  INTO @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceStatusCursor
	DEALLOCATE InvoiceStatusCursor
 

	