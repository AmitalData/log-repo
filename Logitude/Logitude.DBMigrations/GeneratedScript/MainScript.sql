-- General Script From 202008241136_EventTypesConverteLocalNameToEnglishName.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'bba3ab7a2bd27d86acaac6d024f32f55', [Version] = 4445 WHERE [SxmlFileName] = '202008241136_EventTypesConverteLocalNameToEnglishName.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008211829_FillPickupDeliveryCWeightUnitCodeQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'fbaafbb02241fc82493d48480eb14e83', [Version] = 424 WHERE [SxmlFileName] = '202008211829_FillPickupDeliveryCWeightUnitCodeQuoteField.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008221040_FillPickupDeliveryRatioQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'addca55b711fa95e757950665ee2e9da', [Version] = 324 WHERE [SxmlFileName] = '202008221040_FillPickupDeliveryRatioQuoteField.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008221130_FillPickupDeliveryChargeableWeightQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'fab1005b08bb718de1c36d0dec55edd5', [Version] = 234 WHERE [SxmlFileName] = '202008221130_FillPickupDeliveryChargeableWeightQuoteField.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

