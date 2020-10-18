-- Set Nullable For Column IsFromInterestBatchInvoice
ALTER TABLE [dbo].[ARInvoices] ALTER COLUMN [IsFromInterestBatchInvoice] BIT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5e5d779f-0d47-494d-9c0d-f38f21b5c5eb', 'ARInvoice.dxml', 'ARInvoices', 'IsFromInterestBatchInvoice', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column IsFromInterestBatchInvoiceALTER TABLE [dbo].[ARInvoices] ALTER COLUMN [IsFromInterestBatchInvoice] BIT NULL;');

-- Drop Column IsFromInterestBatchInvoice
EXEC SP_RENAME 'dbo.ARInvoices.IsFromInterestBatchInvoice', 'Drop_IsFromInterestBatchInvoice', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7b91dfe2-8573-4d57-aa3b-55404a4f1dba', 'ARInvoice.dxml', 'ARInvoices', 'IsFromInterestBatchInvoice', 'Drop Column', GETDATE(), '-- Drop Column IsFromInterestBatchInvoiceEXEC SP_RENAME ''dbo.ARInvoices.IsFromInterestBatchInvoice'', ''Drop_IsFromInterestBatchInvoice'', ''COLUMN'';');


-- General Script From 202009152200_FixHorseEventTypes.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'ff1b86d341b0edfd8440c2fb8b42da15', [Version] = 434242 WHERE [SxmlFileName] = '202009152200_FixHorseEventTypes.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

