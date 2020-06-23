-- Set Nullable For Column AllowMultiRatesInInvoiceLines
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [AllowMultiRatesInInvoiceLines] BIT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('62b6fc94-7623-4bb8-ad99-c3e9ffcf8ea4', 'FullAccountingSetting.dxml', 'FullAccountingSettings', 'AllowMultiRatesInInvoiceLines', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column AllowMultiRatesInInvoiceLinesALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [AllowMultiRatesInInvoiceLines] BIT NULL;');

-- Drop Column AllowMultiRatesInInvoiceLines
EXEC SP_RENAME 'dbo.FullAccountingSettings.AllowMultiRatesInInvoiceLines', 'Drop_AllowMultiRatesInInvoiceLines', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a57bb98d-30e6-47b3-97bd-f69b8ab7543c', 'FullAccountingSetting.dxml', 'FullAccountingSettings', 'AllowMultiRatesInInvoiceLines', 'Drop Column', GETDATE(), '-- Drop Column AllowMultiRatesInInvoiceLinesEXEC SP_RENAME ''dbo.FullAccountingSettings.AllowMultiRatesInInvoiceLines'', ''Drop_AllowMultiRatesInInvoiceLines'', ''COLUMN'';');


-- Add Default Value For Column MarkForDelete
ALTER TABLE [dbo].[Documents] ADD DEFAULT 0 FOR [MarkForDelete];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e3440364-4b61-420d-b98a-f234cae11e0f', 'Document.dxml', 'Documents', 'MarkForDelete', 'Add Default Value', GETDATE(), '-- Add Default Value For Column MarkForDeleteALTER TABLE [dbo].[Documents] ADD DEFAULT 0 FOR [MarkForDelete];');

-- Unset Nullable For Column MarkForDelete
ALTER TABLE [dbo].[Documents] ALTER COLUMN [MarkForDelete] BIT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d1155470-efe6-4aea-82fa-86112db0afca', 'Document.dxml', 'Documents', 'MarkForDelete', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column MarkForDeleteALTER TABLE [dbo].[Documents] ALTER COLUMN [MarkForDelete] BIT NOT NULL;');


-- Set Nullable For Column AnalyzeQueueId
ALTER TABLE [dbo].[InboundEmails] ALTER COLUMN [AnalyzeQueueId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ea04b7bf-fd92-4dd7-8296-343328f9ca8f', 'InboundEmail.dxml', 'InboundEmails', 'AnalyzeQueueId', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column AnalyzeQueueIdALTER TABLE [dbo].[InboundEmails] ALTER COLUMN [AnalyzeQueueId] VARCHAR(15) NULL;');


-- Drop Unique Constraint UQ_InboundEmails_AnalyzeQueueId_Tenant From Table InboundEmails
EXEC('IF (OBJECT_ID(''[dbo].[UQ_InboundEmails_AnalyzeQueueId_Tenant]'', ''UQ'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[InboundEmails] DROP CONSTRAINT [UQ_InboundEmails_AnalyzeQueueId_Tenant] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('94703263-6cac-4484-b56c-039cfa6e1456', 'InboundEmail.dxml', 'InboundEmails', NULL, 'Drop Unique Constraint', GETDATE(), '-- Drop Unique Constraint UQ_InboundEmails_AnalyzeQueueId_Tenant From Table InboundEmailsEXEC(''IF (OBJECT_ID(''''[dbo].[UQ_InboundEmails_AnalyzeQueueId_Tenant]'''', ''''UQ'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[InboundEmails] DROP CONSTRAINT [UQ_InboundEmails_AnalyzeQueueId_Tenant] END'');');


-- Change Size From -1 To 1000 For Column ContainersNumbers
ALTER TABLE [dbo].[ShipmentComputedFields] ALTER COLUMN [ContainersNumbers] NVARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dd31e857-d128-404c-bf40-4511a3197b1b', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ContainersNumbers', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 1000 For Column ContainersNumbersALTER TABLE [dbo].[ShipmentComputedFields] ALTER COLUMN [ContainersNumbers] NVARCHAR(1000);');


-- Procedure Script From Queue_Enqueue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_Enqueue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_Enqueue] END');
EXEC('CREATE procedure [dbo].[Queue_Enqueue]
(
@QueueDefinitionCode varchar(255),
@MessageBody varchar(1000),
@Tenant int,
@DelaySeconds int,
@CustomerId varchar(15),
@BatchNumber varchar(15),
@NextRunDTime datetime = null,
@HashCode nvarchar(1000),
@WatingStatus int
)
as begin
declare @currentdate  datetime
declare @nextRunDateTime  datetime
set @currentdate =getdate()
if @NextRunDTime is null
set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())
else
set @nextRunDateTime = @NextRunDTime
--set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())
insert into [dbo].[QueueMessages] ([CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[Tenant],[NextRunDateTime],RetryNumber,HashCode)
values(@currentdate,@QueueDefinitionCode,@WatingStatus,@MessageBody,@Tenant,@nextRunDateTime,0,@HashCode)
declare @CId as varchar(15);
declare @BNo as varchar(15);
if @CustomerId = ''''
set @CId = NULL
else
set @CId = @CustomerId
if @BatchNumber = ''''
set @BNo = NULL
else
set @BNo = @BatchNumber
insert into [dbo].[QueueMessageMoreDetails] ([Id],[CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[Tenant],[NextRunDateTime],RetryNumber,Field1,Field2)
values((SELECT SCOPE_IDENTITY()),@currentdate,@QueueDefinitionCode,@WatingStatus,@MessageBody,@Tenant,@nextRunDateTime,0,@CId,@BNo)
end;');


