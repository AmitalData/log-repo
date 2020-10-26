-- General Script From 202009152200_FixHorseEventTypes.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @WrongObjectTableId as varchar(15)
set @WrongObjectTableId = (select Id from ObjectTables where Name = 'QuoteClosingReason')
delete from EventTypes where Code in ('HRIN', 'HRRC') and Tenant = 0 and ObjectTableId = @WrongObjectTableId
declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from ObjectTables where Name = 'Horse')
update EventTypes set ObjectTableId = @ObjectTableId where Code = 'HRIN'
update EventTypes set ObjectTableId = @ObjectTableId where Code = 'HRRC'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009152200_FixHorseEventTypes.sxml', GETDATE(), 'declare @WrongObjectTableId as varchar(15)
set @WrongObjectTableId = (select Id from ObjectTables where Name = ''QuoteClosingReason'')
delete from EventTypes where Code in (''HRIN'', ''HRRC'') and Tenant = 0 and ObjectTableId = @WrongObjectTableId
declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from ObjectTables where Name = ''Horse'')
update EventTypes set ObjectTableId = @ObjectTableId where Code = ''HRIN''
update EventTypes set ObjectTableId = @ObjectTableId where Code = ''HRRC''', DATEDIFF(MS,@StartTime,@EndTime), '89ba7a8cbe3640c00cea8ae514286393', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

