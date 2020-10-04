-- Add New Column With Name DisplayName
ALTER TABLE [dbo].[DWObjectTables] ADD [DisplayName] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('daf5abe0-b8a0-405b-9bef-249c17589afe', 'DWObjectTable.dxml', 'DWObjectTables', 'DisplayName', 'Add Column', GETDATE(), '-- Add New Column With Name DisplayNameALTER TABLE [dbo].[DWObjectTables] ADD [DisplayName] VARCHAR(100) NULL;');


-- General Script From 202009281700_MapValuesFromIsBondedToIsCFS.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '65f6bddc686b4dc02b7b0ad7d82ce96d', [Version] = 3 WHERE [SxmlFileName] = '202009281700_MapValuesFromIsBondedToIsCFS.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

