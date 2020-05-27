-- Unset Nullable For Column Id
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [Id] VARCHAR(15) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7c5ba955-cab8-4657-8da1-4238543764f0', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'Id', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column IdALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [Id] VARCHAR(15) NOT NULL;');

-- Unset Nullable For Column Tenant
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [Tenant] INT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('656340c1-5e92-464d-acd9-f29a8b0b210d', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'Tenant', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column TenantALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [Tenant] INT NOT NULL;');

-- Unset Nullable For Column CreateDate
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [CreateDate] DATETIME NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a1ce6000-42f0-4a54-b20c-374061298928', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreateDate', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CreateDateALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [CreateDate] DATETIME NOT NULL;');

-- Unset Nullable For Column UpdateDate
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [UpdateDate] DATETIME NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b56476c1-1d04-4685-9ede-882edcedbb15', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdateDate', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column UpdateDateALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [UpdateDate] DATETIME NOT NULL;');

-- Drop Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons That Reference To Column Id In Table Users
EXEC('IF (OBJECT_ID(''[dbo].[FK_QuoteClosingReasons_Users_CreatedByUserId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0ace7ea3-7f4a-42c1-b7b2-9bfcdcaf32be', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons That Reference To Column Id In Table UsersEXEC(''IF (OBJECT_ID(''''[dbo].[FK_QuoteClosingReasons_Users_CreatedByUserId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] END'');');

-- Drop Index IX_QuoteClosingReasons_CreatedByUserId From Table QuoteClosingReasons
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_QuoteClosingReasons_CreatedByUserId'' AND object_id = OBJECT_ID(''[dbo].[QuoteClosingReasons]'', ''U'')) BEGIN DROP INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('98a5f1f0-65b6-4df1-8fe1-36d1f3a2b86b', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_QuoteClosingReasons_CreatedByUserId From Table QuoteClosingReasonsEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_QuoteClosingReasons_CreatedByUserId'''' AND object_id = OBJECT_ID(''''[dbo].[QuoteClosingReasons]'''', ''''U'''')) BEGIN DROP INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons] END'');');

-- Unset Nullable For Column CreatedByUserId
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [CreatedByUserId] VARCHAR(15) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2dad61fa-33fd-47eb-bbc9-93d58be0773b', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreatedByUserId', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CreatedByUserIdALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [CreatedByUserId] VARCHAR(15) NOT NULL;');

-- Drop Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons That Reference To Column Id In Table Users
EXEC('IF (OBJECT_ID(''[dbo].[FK_QuoteClosingReasons_Users_UpdatedByUserId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('307d5abb-572d-4762-977a-75ddbf5f5f69', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons That Reference To Column Id In Table UsersEXEC(''IF (OBJECT_ID(''''[dbo].[FK_QuoteClosingReasons_Users_UpdatedByUserId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] END'');');

-- Drop Index IX_QuoteClosingReasons_UpdatedByUserId From Table QuoteClosingReasons
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_QuoteClosingReasons_UpdatedByUserId'' AND object_id = OBJECT_ID(''[dbo].[QuoteClosingReasons]'', ''U'')) BEGIN DROP INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4e2ff86d-53a3-4721-a4ad-893e01dee9ea', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_QuoteClosingReasons_UpdatedByUserId From Table QuoteClosingReasonsEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_QuoteClosingReasons_UpdatedByUserId'''' AND object_id = OBJECT_ID(''''[dbo].[QuoteClosingReasons]'''', ''''U'''')) BEGIN DROP INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons] END'');');

-- Unset Nullable For Column UpdatedByUserId
ALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [UpdatedByUserId] VARCHAR(15) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f2ab9a9f-7449-40b3-b0f6-fbaa21768d6d', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdatedByUserId', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column UpdatedByUserIdALTER TABLE [dbo].[QuoteClosingReasons] ALTER COLUMN [UpdatedByUserId] VARCHAR(15) NOT NULL;');

-- Drop Foreign Key Constraint For Column QuoteClosingReasonCode In Table Quotes That Reference To Column Code In Table QuoteClosingReasons
EXEC('IF (OBJECT_ID(''[dbo].[FK_Quotes_QuoteClosingReasons_QuoteClosingReasonCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_Quotes_QuoteClosingReasons_QuoteClosingReasonCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2c81a667-ed47-4e23-a919-7371cc139039', 'QuoteClosingReason.dxml', 'Quotes', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column QuoteClosingReasonCode In Table Quotes That Reference To Column Code In Table QuoteClosingReasonsEXEC(''IF (OBJECT_ID(''''[dbo].[FK_Quotes_QuoteClosingReasons_QuoteClosingReasonCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_Quotes_QuoteClosingReasons_QuoteClosingReasonCode] END'');');

-- Drop Primary Key Constraint
EXEC('IF (OBJECT_ID(''[dbo].[PK_dbo.QuoteClosingReasons]'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [PK_dbo.QuoteClosingReasons] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ff8b82b3-0d88-407a-b34f-3920505ff4bd', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Drop Primary Key Constraint', GETDATE(), '-- Drop Primary Key ConstraintEXEC(''IF (OBJECT_ID(''''[dbo].[PK_dbo.QuoteClosingReasons]'''', ''''PK'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteClosingReasons] DROP CONSTRAINT [PK_dbo.QuoteClosingReasons] END'');');

-- Add Primary Key Constraint
EXEC('ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [PK_dbo.QuoteClosingReasons] PRIMARY KEY ([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('badcabef-baaf-458a-b82f-6d1d00c6b280', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'Id', 'Add Primary Key Constraint', GETDATE(), '-- Add Primary Key ConstraintEXEC(''ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [PK_dbo.QuoteClosingReasons] PRIMARY KEY ([Id])'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c76fdd96-ee25-4b50-99f9-7bcf0a3888b0', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On QuoteClosingReasons Table
EXEC('CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('51b0defa-607d-4a46-8279-7c6b3b4231a1', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On QuoteClosingReasons TableEXEC(''CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b25a0cba-e02f-495f-b964-d4cafe514950', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On QuoteClosingReasons Table
EXEC('CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b9586024-a2e3-475e-8f46-22718d97ab9e', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On QuoteClosingReasons TableEXEC(''CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons]([UpdatedByUserId])'');');


-- General Script From FillQuoteClosingReasonsDefaultValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = 'system@tenant'+ @TenantString + '.com'
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'EQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('EQ', 'Expensive Quote', 'EQ,Expensive Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'GS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('GS', 'Given directly to the Shipping Line', 'GS,Given directly to the Shipping Line', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LC')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LC', 'Lost to Competitor', 'LC,Lost to Competitor', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LS', 'Lack of Service in the Last Shipment', 'LS,Lack of Service in the Last Shipment', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'XQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('XQ', 'Expired Quote', 'XQ,Expired Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FillQuoteClosingReasonsDefaultValues.sxml', GETDATE(), 'declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = ''system@tenant''+ @TenantString + ''.com''
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''EQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''EQ'', ''Expensive Quote'', ''EQ,Expensive Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''GS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''GS'', ''Given directly to the Shipping Line'', ''GS,Given directly to the Shipping Line'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LC'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LC'', ''Lost to Competitor'', ''LC,Lost to Competitor'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LS'', ''Lack of Service in the Last Shipment'', ''LS,Lack of Service in the Last Shipment'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''XQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''XQ'', ''Expired Quote'', ''XQ,Expired Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '437d3bcac978b2e9ae9ca4ad55a6a4ca', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

