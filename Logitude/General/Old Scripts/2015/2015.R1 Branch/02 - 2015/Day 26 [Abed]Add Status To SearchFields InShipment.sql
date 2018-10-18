
BEGIN
declare @StatusId as varchar(15)
declare @Tenant as int
declare @Id as varchar(15)
declare @SearchFields as nvarchar(1000)
       DECLARE ShipmentCursor CURSOR READ_ONLY
       FOR
       SELECT Id,Tenant,StatusId,SearchFields
       From Shipments 
       OPEN ShipmentCursor FETCH NEXT FROM ShipmentCursor INTO @Id,@Tenant,@StatusId,@SearchFields
       WHILE @@FETCH_STATUS = 0
       BEGIN

  declare @statussearch as varchar(1000)
  
     set @statussearch =  (Select  Name 
     From EntityStatus where id= @StatusId
     )


	declare @SearchFieldsReslut as varchar(1000)

   set	@SearchFieldsReslut = @SearchFields + ','+ @statussearch;



		IF CHARINDEX(@statussearch,@SearchFields) !> 0
	    BEGIN
		
        set @SearchFields = @SearchFields + ','+ @statussearch 
        update  Shipments  set SearchFields= @SearchFields where Id =@Id 

     END
	ELSE


       FETCH NEXT FROM ShipmentCursor INTO @Id,@Tenant,@StatusId,@SearchFields
       END 
       CLOSE ShipmentCursor
       DEALLOCATE ShipmentCursor
END



