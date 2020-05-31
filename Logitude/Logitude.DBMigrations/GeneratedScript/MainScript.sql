-- Add New Column With Name Tenant
ALTER TABLE [dbo].[QueueMessages] ADD [Tenant] INT DEFAULT(-1) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dad29399-50c5-4a4e-9713-81902e8b7c71', 'QueueMessage.dxml', 'QueueMessages', 'Tenant', 'Add Column', GETDATE(), '-- Add New Column With Name TenantALTER TABLE [dbo].[QueueMessages] ADD [Tenant] INT DEFAULT(-1) NOT NULL;');


-- Add New Column With Name Tenant
ALTER TABLE [dbo].[QueueMessageMoreDetails] ADD [Tenant] INT DEFAULT(-1) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bb07463c-abf2-4c39-a34d-f2934bdfc737', 'QueueMessageMoreDetails.dxml', 'QueueMessageMoreDetails', 'Tenant', 'Add Column', GETDATE(), '-- Add New Column With Name TenantALTER TABLE [dbo].[QueueMessageMoreDetails] ADD [Tenant] INT DEFAULT(-1) NOT NULL;');


-- Procedure Script From QueueEnQueueProcedure.dxml
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
@HashCode nvarchar(1000)
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
values(@currentdate,@QueueDefinitionCode,0,@MessageBody,@Tenant,@nextRunDateTime,0,@HashCode)
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
values((SELECT SCOPE_IDENTITY()),@currentdate,@QueueDefinitionCode,0,@MessageBody,@Tenant,@nextRunDateTime,0,@CId,@BNo)
end;');


