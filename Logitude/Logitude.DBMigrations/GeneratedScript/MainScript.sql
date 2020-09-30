-- General Script From 202009152200_FixHorseEventTypes.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009152200_FixHorseEventTypes.sxml', GETDATE(), 'NULL', DATEDIFF(MS,@StartTime,@EndTime), 'f51b19219b64a436bc97a4c53f011604', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051322_UpdateNotAirShipmentsSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
-- If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
--      Begin
--        Drop Table #tempTable
--      End
--      If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
--      Begin
--        Drop Table #temp_Shipments
--      End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    ShipmentSubTypeId varchar(15)  null,
--	ShipmentTypeId varchar(15)  null
--    )
--	select
--	Id,
--	Tenant,
--	TransportModeId,
--	ShipmentTypeId,
--	(
--		CASE
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'MyGO' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGO' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'MyGI' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGI' and Tenant = Shipments.Tenant)
--		END
--	 ) as ShipmentSubTypeId
--	into #tempTable
--	FROM Shipments where TransportModeId <> 'A'
--	  declare @Tenant as int
--      declare @EntityId as varchar(15)
--	  declare @ShipmentTypeId as varchar(4)
--	  declare @TransportModeId as varchar(4)
--	  declare @ShipmentSubTypeId as varchar(15)
--	  declare @Count as int
--      set @Count = 0;
--    BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--       		insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--	  drop table #tempTable
--      drop table #temp_Shipments
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = '-- If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
--      Begin
--        Drop Table #tempTable
--      End
--      If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
--      Begin
--        Drop Table #temp_Shipments
--      End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    ShipmentSubTypeId varchar(15)  null,
--	ShipmentTypeId varchar(15)  null
--    )
--	select
--	Id,
--	Tenant,
--	TransportModeId,
--	ShipmentTypeId,
--	(
--		CASE
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''MyGO'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGO'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''MyGI'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGI'' and Tenant = Shipments.Tenant)
--		END
--	 ) as ShipmentSubTypeId
--	into #tempTable
--	FROM Shipments where TransportModeId <> ''A''
--	  declare @Tenant as int
--      declare @EntityId as varchar(15)
--	  declare @ShipmentTypeId as varchar(4)
--	  declare @TransportModeId as varchar(4)
--	  declare @ShipmentSubTypeId as varchar(15)
--	  declare @Count as int
--      set @Count = 0;
--    BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--       		insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--	  drop table #tempTable
--      drop table #temp_Shipments', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'aa0b6efe01efd811137af38dc287e4ad', [Version] = 2 WHERE [SxmlFileName] = '202007051322_UpdateNotAirShipmentsSubType.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009230840_AddNewSpecialHandlingCode.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Id as varchar(15)
if not exists (select Id from AWBSpecialHandlingCodes where Code = 'NSC')
begin
EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBSpecialHandlingCode'
insert into AWBSpecialHandlingCodes(Code, Name, SearchFields, IsIATA, AirlineId, Id, InActive)
values('NSC', 'Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft', 'NSC,Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft', 1, NULL, @Id, 0)
end
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'AWBSpecialHandlingCode')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009230840_AddNewSpecialHandlingCode.sxml', GETDATE(), 'declare @Id as varchar(15)
if not exists (select Id from AWBSpecialHandlingCodes where Code = ''NSC'')
begin
EXECUTE usp_GetNextTableIdValue @Id OUTPUT,''AWBSpecialHandlingCode''
insert into AWBSpecialHandlingCodes(Code, Name, SearchFields, IsIATA, AirlineId, Id, InActive)
values(''NSC'', ''Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft'', ''NSC,Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft'', 1, NULL, @Id, 0)
end
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''AWBSpecialHandlingCode'')', DATEDIFF(MS,@StartTime,@EndTime), 'd445435d375c617293df430dadb953d2', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009281700_MapValuesFromIsBondedToIsCFS.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009281700_MapValuesFromIsBondedToIsCFS.sxml', GETDATE(), 'update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1', DATEDIFF(MS,@StartTime,@EndTime), '65f6bddc686b4dc02b7b0ad7d82ce96d', 3);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

