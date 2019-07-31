----update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'D'  
----update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'C'  

----declare @Tenant as int
----declare @EntityId as varchar(15)
----declare @MasterShipmentId as varchar(15)
----declare @ParentShipmentNumber as varchar(30)
----declare @ShipmentNumber as varchar(30)

----BEGIN
----	DECLARE DataCursor CURSOR READ_ONLY
----	FOR
----	SELECT Id, Tenant,
----	ShipmentNumber,MasterShipmentDataId
----	FROM Shipments where ShipmentLevelCode In ('H')
----	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId
----	WHILE @@FETCH_STATUS = 0
----	BEGIN
----			if (@MasterShipmentId is null )
----			begin 
----			update shipments set ComputedShipmentNumber=null where Tenant=@Tenant and Id=@EntityId
----			end

----			if (@MasterShipmentId is not null )
----			begin 
----			set @ParentShipmentNumber = (SELECT ShipmentNumber
----			FROM Shipments where Tenant=@Tenant and Id=@MasterShipmentId)			
----			update shipments set ComputedShipmentNumber=@ParentShipmentNumber where Tenant=@Tenant and Id=@EntityId
----			end
											
----	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId
----	END
----	CLOSE DataCursor
----	DEALLOCATE DataCursor
----END



--select count(*) from shipments
--update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'D'  


If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
    Drop Table #tempTable
End

If(OBJECT_ID('tempdb..#temp_ShipmentComputedFields') Is Not Null)
Begin
    Drop Table #temp_ShipmentComputedFields
End


CREATE TABLE #temp_ShipmentComputedFields (
	Id varchar(15) not null ,-- primary keyz
    ComputedShipmentNumber varchar(100)  null
    )
	--drop table #tempTable
	
	select  Id, Tenant,ShipmentNumber,MasterShipmentDataId,ShipmentLevelCode,ComputedShipmentNumber
	into #tempTable
	FROM Shipments --where ShipmentLevelCode In ('C','H')
	
   CREATE INDEX IDX_tempTable_Id ON dbo.#tempTable(Id)
   CREATE INDEX IDX_tempTable_Id_Tenant ON dbo.#tempTable(Id,Tenant)
   CREATE INDEX IDX_tempTable_MasterShipmentDataId_Tenant ON dbo.#tempTable(MasterShipmentDataId,Tenant)
   CREATE INDEX IDX_tempTable_ShipmentLevelCode ON dbo.#tempTable(ShipmentLevelCode)
   
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ComputedShipmentNumber as varchar(1000)
declare @ShipmentNumber as varchar(30)
declare @MasterShipmentId as varchar(15)
declare @ShipmentLevelCode as varchar(15)
declare @ParentShipmentNumber as varchar(30)
declare @Count as int
set @Count = 0;
BEGIN
       DECLARE DataCursor CURSOR READ_ONLY
       FOR
       SELECT Id, Tenant,ShipmentNumber,MasterShipmentDataId,ShipmentLevelCode
       FROM #tempTable
       where ShipmentLevelCode In ('C','H','D')
       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId,@ShipmentLevelCode
       WHILE @@FETCH_STATUS = 0
       BEGIN
       
        if (@ShipmentLevelCode = 'D' or @ShipmentLevelCode = 'C')
                     begin 
                     
                     --update #tempTable set ComputedShipmentNumber=@ShipmentNumber where Tenant=@Tenant and Id=@EntityId
						insert into #temp_ShipmentComputedFields(Id,ComputedShipmentNumber) values(@EntityId,@ShipmentNumber)
                     end
     else
     begin
       if (@MasterShipmentId is null )
			begin 
			--update shipments set ComputedShipmentNumber=null where Tenant=@Tenant and Id=@EntityId
			insert into #temp_ShipmentComputedFields(Id,ComputedShipmentNumber) values(@EntityId,null)
			end

			if (@MasterShipmentId is not null )
			begin 
			set @ParentShipmentNumber = (SELECT ShipmentNumber
			FROM #tempTable where Tenant=@Tenant and Id=@MasterShipmentId)	
			insert into #temp_ShipmentComputedFields(Id,ComputedShipmentNumber) values(@EntityId,@ParentShipmentNumber)		
			 --update shipments set ComputedShipmentNumber=@ParentShipmentNumber where Tenant=@Tenant and Id=@EntityId
			end
    set @Count = @Count + 1;
	print @Count 
	if(@Count = 1000)
		begin print @Count

		update shipmentsTable
		set ComputedShipmentNumber= T.ComputedShipmentNumber 
		FROM Shipments shipmentsTable
		INNER JOIN #temp_ShipmentComputedFields T on shipmentsTable.Id = T.Id

		truncate table #temp_ShipmentComputedFields
		set @Count = 0
	end

	

end
       FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId,@ShipmentLevelCode
       END
       CLOSE DataCursor
       DEALLOCATE DataCursor
END



--WHILE (2 > 1) 
--  BEGIN 
--    BEGIN TRANSACTION 
--    UPDATE TOP ( 100000 ) shipmentsTable 
--    set ComputedShipmentNumber= T.ComputedShipmentNumber 
--	FROM Shipments shipmentsTable
--	INNER JOIN #tempTable T on shipmentsTable.Id = T.Id
     
--    IF @@ROWCOUNT = 0 
--      BEGIN 
--        COMMIT TRANSACTION 
--         BREAK 
--      END 
--    COMMIT TRANSACTION 
--    -- 1 second delay
--    WAITFOR DELAY '00:00:01'
--  END -- WHILE
--GO

	--update shipmentsTable
	--set ComputedShipmentNumber= T.ComputedShipmentNumber 
	--FROM Shipments shipmentsTable
	--INNER JOIN #tempTable T on shipmentsTable.Id = T.Id
		
		
	update shipmentsTable
	set ComputedShipmentNumber= T.ComputedShipmentNumber 
	FROM Shipments shipmentsTable
	INNER JOIN #temp_ShipmentComputedFields T on shipmentsTable.Id = T.Id
	
	drop table #tempTable
    drop table #temp_ShipmentComputedFields