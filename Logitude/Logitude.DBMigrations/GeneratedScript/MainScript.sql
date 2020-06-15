-- General Script From 202006151350_DeleteOldPaymentMethodsMetadata.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod'))
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod'))
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTables where Name in ('APPaymentMethod', 'ARPaymentMethod')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151350_DeleteOldPaymentMethodsMetadata.sxml', GETDATE(), 'delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod''))
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod''))
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTables where Name in (''APPaymentMethod'', ''ARPaymentMethod'')', DATEDIFF(MS,@StartTime,@EndTime), '1692d6d174ab08bd4e6effcbbd1ccc7d', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006132005_AddDepartureFrequencyToQuoteDetails.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @QuoteTemplateId as varchar(15)
declare @QuoteTemplateTextCodeId as varchar(15)
declare @Tenant as int
declare @NewEntityId as varchar(15)
DECLARE QuoteTemplateCursor CURSOR READ_ONLY
FOR
SELECT Id , Tenant
From QuoteTemplates
OPEN QuoteTemplateCursor FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = 'DEPARTUREFREQUENCY' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
if(@QuoteTemplateTextCodeId is null)
begin
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'DEPARTUREFREQUENCY', 'Departure Frequency' , 'Departure Frequency' , @QuoteTemplateId ,'QuoteDetails' ,'Departure Frequency','Departure Frequency' );
end
FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
End
CLOSE QuoteTemplateCursor
DEALLOCATE QuoteTemplateCursor
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006132005_AddDepartureFrequencyToQuoteDetails.sxml', GETDATE(), 'declare @QuoteTemplateId as varchar(15)
declare @QuoteTemplateTextCodeId as varchar(15)
declare @Tenant as int
declare @NewEntityId as varchar(15)
DECLARE QuoteTemplateCursor CURSOR READ_ONLY
FOR
SELECT Id , Tenant
From QuoteTemplates
OPEN QuoteTemplateCursor FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = ''DEPARTUREFREQUENCY'' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
if(@QuoteTemplateTextCodeId is null)
begin
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,''QuoteTemplateTextCode''
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , ''DEPARTUREFREQUENCY'', ''Departure Frequency'' , ''Departure Frequency'' , @QuoteTemplateId ,''QuoteDetails'' ,''Departure Frequency'',''Departure Frequency'' );
end
FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
End
CLOSE QuoteTemplateCursor
DEALLOCATE QuoteTemplateCursor', DATEDIFF(MS,@StartTime,@EndTime), '37293753384c284c15beaf1c8fd60701', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Create New Table With Name BIFoldersPermissions
CREATE TABLE [dbo].[BIFoldersPermissions](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[FolderId] VARCHAR(15) NULL,
[UserId] VARCHAR(15) NULL,
CONSTRAINT [PK_BIFoldersPermissions] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5e510049-05bb-4c16-b5b6-2d6fc3e952f2', 'BIFoldersPermission.dxml', 'BIFoldersPermissions', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name BIFoldersPermissionsCREATE TABLE [dbo].[BIFoldersPermissions]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[FolderId] VARCHAR(15) NULL,[UserId] VARCHAR(15) NULL,CONSTRAINT [PK_BIFoldersPermissions] PRIMARY KEY([Id]));');


-- Add New Column With Name PermissionForAll
ALTER TABLE [dbo].[BIReportFolders] ADD [PermissionForAll] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dc62589f-4cb6-4d2e-9365-a3ddf5f1edcc', 'BIReportFolder.dxml', 'BIReportFolders', 'PermissionForAll', 'Add Column', GETDATE(), '-- Add New Column With Name PermissionForAllALTER TABLE [dbo].[BIReportFolders] ADD [PermissionForAll] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name PermittedByUserId
ALTER TABLE [dbo].[BIReportFolders] ADD [PermittedByUserId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f32477ee-6047-4491-b685-7096c8cb5dda', 'BIReportFolder.dxml', 'BIReportFolders', 'PermittedByUserId', 'Add Column', GETDATE(), '-- Add New Column With Name PermittedByUserIdALTER TABLE [dbo].[BIReportFolders] ADD [PermittedByUserId] VARCHAR(15) NULL;');


-- Add New Column With Name DuplicateMessagesAutoRemove
ALTER TABLE [dbo].[QueueDefinitions] ADD [DuplicateMessagesAutoRemove] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ef8ad58c-1005-4460-a9e6-74fb2760c473', 'QueueDefinition.dxml', 'QueueDefinitions', 'DuplicateMessagesAutoRemove', 'Add Column', GETDATE(), '-- Add New Column With Name DuplicateMessagesAutoRemoveALTER TABLE [dbo].[QueueDefinitions] ADD [DuplicateMessagesAutoRemove] BIT DEFAULT(0) NOT NULL;');


-- Add New Column With Name HashCode
ALTER TABLE [dbo].[QueueMessages] ADD [HashCode] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('943813a1-2e26-4376-b76e-cbe62e34eb91', 'QueueMessage.dxml', 'QueueMessages', 'HashCode', 'Add Column', GETDATE(), '-- Add New Column With Name HashCodeALTER TABLE [dbo].[QueueMessages] ADD [HashCode] NVARCHAR(MAX) NULL;');


-- Drop Column ShipmentSubTypeId
EXEC SP_RENAME 'dbo.Quotes.ShipmentSubTypeId', 'Drop_ShipmentSubTypeId', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0fd25b86-9d96-4762-9b3b-235bd023b337', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Drop Column', GETDATE(), '-- Drop Column ShipmentSubTypeIdEXEC SP_RENAME ''dbo.Quotes.ShipmentSubTypeId'', ''Drop_ShipmentSubTypeId'', ''COLUMN'';');


-- Unset Nullable For Column IsMultiHarmonize
ALTER TABLE [dbo].[InsideShipmentPackages] ALTER COLUMN [IsMultiHarmonize] BIT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ca364dd9-1c26-4578-b828-e323310df46f', 'InsideShipmentPackage.dxml', 'InsideShipmentPackages', 'IsMultiHarmonize', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column IsMultiHarmonizeALTER TABLE [dbo].[InsideShipmentPackages] ALTER COLUMN [IsMultiHarmonize] BIT NOT NULL;');


-- Drop Column ShipmentSubTypeId
EXEC SP_RENAME 'dbo.Shipments.ShipmentSubTypeId', 'Drop_ShipmentSubTypeId', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('22f67524-e367-4847-a9b7-b0f90659b599', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Drop Column', GETDATE(), '-- Drop Column ShipmentSubTypeIdEXEC SP_RENAME ''dbo.Shipments.ShipmentSubTypeId'', ''Drop_ShipmentSubTypeId'', ''COLUMN'';');


-- Add Foreign Key Constraint For Column FolderId In Table BIFoldersPermissions As Reference To Column Id In Table BIReportFolders
EXEC('ALTER TABLE [dbo].[BIFoldersPermissions] ADD CONSTRAINT [FK_BIFoldersPermissions_BIReportFolders_FolderId] FOREIGN KEY([FolderId]) REFERENCES [dbo].[BIReportFolders]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('91fefcb2-b51e-4816-baaf-65f0138e8baa', 'BIFoldersPermission.dxml', 'BIFoldersPermissions', 'FolderId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column FolderId In Table BIFoldersPermissions As Reference To Column Id In Table BIReportFoldersEXEC(''ALTER TABLE [dbo].[BIFoldersPermissions] ADD CONSTRAINT [FK_BIFoldersPermissions_BIReportFolders_FolderId] FOREIGN KEY([FolderId]) REFERENCES [dbo].[BIReportFolders]([Id])'');');

-- Create Index On BIFoldersPermissions Table
EXEC('CREATE NONCLUSTERED INDEX [IX_BIFoldersPermissions_FolderId] ON [dbo].[BIFoldersPermissions]([FolderId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3a77d38f-6328-476d-b8ea-8143dbac3997', 'BIFoldersPermission.dxml', 'BIFoldersPermissions', 'FolderId', 'Create Index', GETDATE(), '-- Create Index On BIFoldersPermissions TableEXEC(''CREATE NONCLUSTERED INDEX [IX_BIFoldersPermissions_FolderId] ON [dbo].[BIFoldersPermissions]([FolderId])'');');

-- Add Foreign Key Constraint For Column UserId In Table BIFoldersPermissions As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[BIFoldersPermissions] ADD CONSTRAINT [FK_BIFoldersPermissions_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d1499e96-b1dd-41c5-a668-1edf9ffaa8a1', 'BIFoldersPermission.dxml', 'BIFoldersPermissions', 'UserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UserId In Table BIFoldersPermissions As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[BIFoldersPermissions] ADD CONSTRAINT [FK_BIFoldersPermissions_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On BIFoldersPermissions Table
EXEC('CREATE NONCLUSTERED INDEX [IX_BIFoldersPermissions_UserId] ON [dbo].[BIFoldersPermissions]([UserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('37dc2901-60d1-43cb-81cb-466f43a1040b', 'BIFoldersPermission.dxml', 'BIFoldersPermissions', 'UserId', 'Create Index', GETDATE(), '-- Create Index On BIFoldersPermissions TableEXEC(''CREATE NONCLUSTERED INDEX [IX_BIFoldersPermissions_UserId] ON [dbo].[BIFoldersPermissions]([UserId])'');');


-- Add Foreign Key Constraint For Column PermittedByUserId In Table BIReportFolders As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[BIReportFolders] ADD CONSTRAINT [FK_BIReportFolders_Users_PermittedByUserId] FOREIGN KEY([PermittedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4b6a6172-bb79-464b-b2e7-45d3f23635dc', 'BIReportFolder.dxml', 'BIReportFolders', 'PermittedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column PermittedByUserId In Table BIReportFolders As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[BIReportFolders] ADD CONSTRAINT [FK_BIReportFolders_Users_PermittedByUserId] FOREIGN KEY([PermittedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On BIReportFolders Table
EXEC('CREATE NONCLUSTERED INDEX [IX_BIReportFolders_PermittedByUserId] ON [dbo].[BIReportFolders]([PermittedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f1a8d0a4-60b4-478f-80df-bc83a87f9dd6', 'BIReportFolder.dxml', 'BIReportFolders', 'PermittedByUserId', 'Create Index', GETDATE(), '-- Create Index On BIReportFolders TableEXEC(''CREATE NONCLUSTERED INDEX [IX_BIReportFolders_PermittedByUserId] ON [dbo].[BIReportFolders]([PermittedByUserId])'');');


-- Drop Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes That Reference To Column Id In Table ShipmentSubTypes
EXEC('IF (OBJECT_ID(''[dbo].[FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d6fb3afe-ac5e-4650-8ac7-12d678b6cd73', 'Quote.dxml', 'Quotes', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes That Reference To Column Id In Table ShipmentSubTypesEXEC(''IF (OBJECT_ID(''''[dbo].[FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] END'');');

-- Drop Index IX_Quotes_ShipmentSubTypeId From Table Quotes
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_Quotes_ShipmentSubTypeId'' AND object_id = OBJECT_ID(''[dbo].[Quotes]'', ''U'')) BEGIN DROP INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ed56f0d5-4ce4-487b-bce5-054ed8f8a1eb', 'Quote.dxml', 'Quotes', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_Quotes_ShipmentSubTypeId From Table QuotesEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_Quotes_ShipmentSubTypeId'''' AND object_id = OBJECT_ID(''''[dbo].[Quotes]'''', ''''U'''')) BEGIN DROP INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes] END'');');


-- Add Foreign Key Constraint For Column Tenant In Table QuoteTemplates As Reference To Column Id In Table Tenants
EXEC('ALTER TABLE [dbo].[QuoteTemplates] ADD CONSTRAINT [FK_QuoteTemplates_Tenants_Tenant] FOREIGN KEY([Tenant]) REFERENCES [dbo].[Tenants]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97c71161-3175-4486-be36-a59ca0872d69', 'QuoteTemplate.dxml', 'QuoteTemplates', 'Tenant', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column Tenant In Table QuoteTemplates As Reference To Column Id In Table TenantsEXEC(''ALTER TABLE [dbo].[QuoteTemplates] ADD CONSTRAINT [FK_QuoteTemplates_Tenants_Tenant] FOREIGN KEY([Tenant]) REFERENCES [dbo].[Tenants]([Id])'');');

-- Create Index On QuoteTemplates Table
EXEC('CREATE NONCLUSTERED INDEX [IX_QuoteTemplates_Tenant] ON [dbo].[QuoteTemplates]([Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c1eeb8c6-8b1e-4626-a545-c3ecc27aab03', 'QuoteTemplate.dxml', 'QuoteTemplates', 'Tenant', 'Create Index', GETDATE(), '-- Create Index On QuoteTemplates TableEXEC(''CREATE NONCLUSTERED INDEX [IX_QuoteTemplates_Tenant] ON [dbo].[QuoteTemplates]([Tenant])'');');


-- Drop Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments That Reference To Column Id In Table ShipmentSubTypes
EXEC('IF (OBJECT_ID(''[dbo].[FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('15c730e4-729d-4f77-b675-26cfa78ae989', 'Shipment.dxml', 'Shipments', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments That Reference To Column Id In Table ShipmentSubTypesEXEC(''IF (OBJECT_ID(''''[dbo].[FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] END'');');

-- Drop Index IX_Shipments_ShipmentSubTypeId From Table Shipments
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_Shipments_ShipmentSubTypeId'' AND object_id = OBJECT_ID(''[dbo].[Shipments]'', ''U'')) BEGIN DROP INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d30e8220-6ef0-412e-8eae-6e3a80827660', 'Shipment.dxml', 'Shipments', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_Shipments_ShipmentSubTypeId From Table ShipmentsEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_Shipments_ShipmentSubTypeId'''' AND object_id = OBJECT_ID(''''[dbo].[Shipments]'''', ''''U'''')) BEGIN DROP INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments] END'');');


-- Add Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize As Reference To Column Id In Table InsideShipmentPackages
EXEC('ALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] FOREIGN KEY([InsidePackageId]) REFERENCES [dbo].[InsideShipmentPackages]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('15e28a2b-231a-41c6-891f-98db1fed5b9f', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize As Reference To Column Id In Table InsideShipmentPackagesEXEC(''ALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] FOREIGN KEY([InsidePackageId]) REFERENCES [dbo].[InsideShipmentPackages]([Id])'');');

-- Create Index On ShipmentPackageHarmonize Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize]([InsidePackageId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('edba83ad-f565-434e-b825-f0525e018822', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Create Index', GETDATE(), '-- Create Index On ShipmentPackageHarmonize TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize]([InsidePackageId])'');');


-- DataView Script From ShipmentDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentDataView] END');
EXEC('CREATE VIEW [dbo].[ShipmentDataView]
AS
SELECT        dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber,dbo.Shipments.DeclarationNumber,dbo.Shipments.IncludesCustoms,dbo.Shipments.CustomsClearanceDate,dbo.Shipments.DeclarationDate, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,dbo.Shipments.ValueOfGoods,
dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId, dbo.shipments.SLAC,
dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId, dbo.Shipments.FirstARInvoiceApprovalDate,
dbo.Shipments.LastFinalDestination, [dbo].[Shipments].[From], [dbo].[Shipments].[To], dbo.Shipments.Origin, dbo.Shipments.FirstPickupETA, dbo.Shipments.FirstPickupETD,
dbo.Shipments.ExceptionDescription,dbo.Shipments.ValueOfGoodsCurrencyId,dbo.Shipments.ExceptionDate, dbo.Shipments.HasException, dbo.Shipments.IsManifestSentToAgent, dbo.Shipments.ManifestLastSharingDate,
dbo.Shipments.AgentSharedManifestRef , dbo.Shipments.ExceptionResolvedDescription,dbo.Shipments.LastExceptionDescription, dbo.Shipments.OperationalDate, dbo.ShipmentMasterDatas.CutoffDate,
dbo.Shipments.ComputedStatusId, dbo.Shipments.ComputedStatusDate, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
dbo.Shipments.CustomConnectToShipment, dbo.Shipments.ForeignPartnerCountryCode, dbo.Shipments.IsNewARInvoiceBlocked,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId, dbo.ShipmentMasterDatas.Transshipment3CarrierId,
dbo.ShipmentMasterDatas.Transshipment2CarrierId, dbo.ShipmentMasterDatas.Transshipment1CarrierId, dbo.ShipmentMasterDatas.MainCarriageCarrierId,
dbo.ShipmentMasterDatas.MainCarriageIsFromStack, dbo.ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dbo.ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment3VesselId, dbo.ShipmentMasterDatas.Transshipment2VesselId, dbo.ShipmentMasterDatas.Transshipment1VesselId,
dbo.ShipmentMasterDatas.MainCarriageVesselId, dbo.ShipmentMasterDatas.BookingConfirmedBy, dbo.ShipmentMasterDatas.BookingConfirmationNotes,
dbo.ShipmentMasterDatas.BookingConfirmationNumber, dbo.ShipmentMasterDatas.MAWBOBLDate, dbo.ShipmentMasterDatas.Transshipment3CarrierNumber,
dbo.ShipmentMasterDatas.Transshipment3ETA, dbo.ShipmentMasterDatas.Transshipment3ETD, dbo.ShipmentMasterDatas.Transshipment3ATA,
dbo.ShipmentMasterDatas.Transshipment3ATD, dbo.ShipmentMasterDatas.Transshipment3ToPortId, dbo.ShipmentMasterDatas.Transshipment3FromPortId,
dbo.ShipmentMasterDatas.Transshipment2CarrierNumber, dbo.ShipmentMasterDatas.Transshipment2ETA, dbo.ShipmentMasterDatas.Transshipment2ETD,
dbo.ShipmentMasterDatas.Transshipment2ATA, dbo.ShipmentMasterDatas.Transshipment2ATD, dbo.ShipmentMasterDatas.Transshipment2ToPortId,
dbo.ShipmentMasterDatas.Transshipment2FromPortId, dbo.ShipmentMasterDatas.Transshipment1CarrierNumber, dbo.ShipmentMasterDatas.Transshipment1ETA,
dbo.ShipmentMasterDatas.Transshipment1ETD, dbo.ShipmentMasterDatas.Transshipment1ATA, dbo.ShipmentMasterDatas.Transshipment1ATD,
dbo.ShipmentMasterDatas.Transshipment1ToPortId, dbo.ShipmentMasterDatas.Transshipment1FromPortId, dbo.ShipmentMasterDatas.Master,
dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA, dbo.ShipmentMasterDatas.TrailerNumber,
dbo.ShipmentMasterDatas.MainCarriageFromAddressId, dbo.ShipmentMasterDatas.MainCarriageToAddressId,
dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2,
dbo.ShipmentMasterDatas.InterlineId, dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix,
dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails, dbo.Shipments.RegistryDate, dbo.Shipments.IsAssembly, dbo.Shipments.FirstOperationalCloseDate,
dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.FirstAccountingCloseDate,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.EstimateProfitInProfitCurrency, dbo.Shipments.ProfitCurrencyId, dbo.Shipments.SCI,
dbo.Shipments.AWBHandlingInformation, dbo.Shipments.AWBInsurrenceValue, dbo.Shipments.AWBAccountingInformation, dbo.Shipments.ProductCode,
dbo.Shipments.AWBDeclaredValueForCustoms, dbo.Shipments.AWBDeclaredValueForCarriage, dbo.Shipments.AWBCarrierTarrifReference,
dbo.Shipments.FreightForwarderContactId, dbo.Shipments.FreightForwarderAddressId, dbo.Shipments.CustomAgentExportContactId,
dbo.Shipments.CustomAgentExportAddressId, dbo.Shipments.ShipmentCustomerTypeCode, dbo.Shipments.CustomerReference1, dbo.Shipments.CustomerReference2, dbo.Shipments.CustomerContactId,
dbo.Shipments.CustomerAddressId, dbo.Shipments.CustomerId, dbo.Shipments.FreightForwarderReference, dbo.Shipments.FreightForwarderId,
dbo.Shipments.CustomAgentExportReference, dbo.Shipments.CustomAgentExportId, dbo.Shipments.CustomAgentImportReference,
dbo.Shipments.ConsigneeAddressOneTime, dbo.Shipments.ShipperAddressOneTime, dbo.Shipments.EstimateProfitInLocalCurrency, dbo.Shipments.AWBCurrencyId,
dbo.Shipments.OrderChargeableWeight, dbo.Shipments.OnCarriageCarrierId, dbo.Shipments.PreCarriageCarrierId, dbo.Shipments.ProfitInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency, dbo.Shipments.LastUpdateDate,
dbo.Shipments.UpdatedByUserId, dbo.Shipments.IsCancelled, dbo.Shipments.CancelledDate, dbo.Shipments.IsAccountingClosed, dbo.Shipments.ShipmentPayableStatusCode,
dbo.Shipments.ShipmentReceivableStatusCode, dbo.Shipments.QuoteId, dbo.Shipments.ShipmentDeliveryIndex, dbo.Shipments.ShipmentPickUpIndex,
dbo.Shipments.LTCWEdited, dbo.Shipments.OnCarriageVesselId, dbo.Shipments.PreCarriageVesselId, dbo.Shipments.DangerousMaterialDescription,
dbo.Shipments.DangerousPackagingGroup, dbo.Shipments.DangerousClassNumber, dbo.Shipments.DangerousUnNumber, dbo.Shipments.DangerousIMDGCode,
dbo.Shipments.DangerousFlashPoint, dbo.Shipments.AWBFreightAmountPrepaid, dbo.Shipments.AWBFreightAmountCollect, dbo.Shipments.IsDangerous,
dbo.Shipments.MainHarmonize, dbo.Shipments.VolumeUnitCode, dbo.Shipments.ChargeableWeightEdited,
dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, dbo.Shipments.ColoaderId,
dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps,
dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId,
dbo.Shipments.ConsigneeNotImporterContactId, dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId,
dbo.Shipments.ConsigneeNotImporterId, dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId,
dbo.Shipments.OrderIsDangerouseGoods, dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight,
dbo.Shipments.Field10, dbo.Shipments.Field9, dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4,
dbo.Shipments.Field3, dbo.Shipments.Field2, dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight,
dbo.Shipments.Field11, dbo.Shipments.Field12, dbo.Shipments.Field13, dbo.Shipments.Field14, dbo.Shipments.Field15,
dbo.Shipments.Field16, dbo.Shipments.Field17, dbo.Shipments.Field18, dbo.Shipments.Field19, dbo.Shipments.Field20,
dbo.Shipments.Field21, dbo.Shipments.Field22, dbo.Shipments.Field23, dbo.Shipments.Field24, dbo.Shipments.Field25,
dbo.Shipments.Field26, dbo.Shipments.Field27, dbo.Shipments.Field28, dbo.Shipments.Field29, dbo.Shipments.Field30,
dbo.Shipments.Field31, dbo.Shipments.Field32, dbo.Shipments.Field33, dbo.Shipments.Field34, dbo.Shipments.Field35,
dbo.Shipments.Field36, dbo.Shipments.Field37, dbo.Shipments.Field38, dbo.Shipments.Field39, dbo.Shipments.Field40,
dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId, dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId,
dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId, dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId,
dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId, dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD,
dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD, dbo.Shipments.AgentId,dbo.Shipments.AgentComputed, dbo.Shipments.OnCarriageCarrierNumber,
dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD, dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId,
dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber, dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD,
dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId, dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate,
dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId, dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId,
dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id, dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId,
dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId, dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId,
dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House, dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1,
dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
dbo.Shipments.ARInvoices,
dbo.Shipments.NotInvoicedReceivablesAmount,
dbo.Shipments.ReleasingAgentId,
dbo.Shipments.ContainerLastStatusDate,
dbo.Shipments.Notify1Reference,
dbo.Shipments.Notify2Reference,
dbo.Shipments.ShipperNotExporterReference,
dbo.Shipments.ConsigneeNotImporterReference,
dbo.Shipments.ProjectNumber,
dbo.Shipments.INTTRALastStatusDate,
dbo.Shipments.INTTRASIError,
dbo.Shipments.INTTRASIStatusCode,
dbo.Shipments.INTTRASIStatusDate,
dbo.Shipments.INTTRABookingError,
dbo.Shipments.INTTRALastBookingResponse,
dbo.INTTRASIStatus.Name AS INTTRASIStatusName,
dbo.Shipments.CreatedByPartner AS CreatedByPartner,
dbo.Shipments.INTTRABookingStatusCode,
dbo.INTTRABookingStatuses.Name AS INTTRABookingStatusName,
dbo.Shipments.INTTRABookingTransStatusCode,
dbo.INTTRABookingTransStatuses.Name AS INTTRABookingTransStatusName,
dbo.Shipments.AccountManagerUserId,
dbo.Shipments.CustomsDeclarationNumber,
dbo.Shipments.ShipperName,dbo.Shipments.FBLIsFromStock,
dbo.Shipments.FreightRelease, dbo.Shipments.TerminalAvailable, dbo.Shipments.ISFDate, dbo.Shipments.ISFNumber, dbo.Shipments.ITDate, dbo.Shipments.ITNumber,
dbo.ShipmentMasterDatas.OBLTypeCode, dbo.ShipmentMasterDatas.DocumentsClosingDate,
dbo.Shipments.ShipperName as Shipper, dbo.Shipments.ENSNumber, dbo.Shipments.ENSDate, dbo.Shipments.WarehouseLegWarehouseId, dbo.Shipments.WarehouseLegAddressId, dbo.Shipments.WarehouseLegTerminalCode, dbo.Shipments.WarehouseLegExpectedEntryDate,
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,
WarehouseLegCard.EnglishName AS WarehouseLegTerminalName,
dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,
dbo.EventTypes.EnglishName as LastSharedEventName,
WarehouseLegCard.CountryCode AS WarehouseLegAddressCountryCode,
WarehouseLegCard.CountryName AS WarehouseLegAddressCountryName,
--ShipperCards.EnglishName AS ShipperName,
MainCarriageFromPorts.Code AS MainCarriageFromPortCode, MainCarriageToPorts.Code AS MainCarriageToPortCode,
Transshipment1FromPorts.Code AS Transshipment1FromPortCode, Transshipment1ToPorts.Code AS Transshipment1ToPortCode,
Transshipment2FromPorts.Code AS Transshipment2FromPortCode, Transshipment2ToPorts.Code AS Transshipment2ToPortCode,
Transshipment3FromPorts.Code AS Transshipment3FromPortCode, Transshipment3ToPorts.Code AS Transshipment3ToPortCode,
PreCarriageFromPorts.Code AS PreCarriageFromPortCode, PreCarriageToPorts.Code AS PreCarriageToPortCode,
OnCarriageFromPorts.Code AS OnCarriageFromPortCode, OnCarriageToPorts.Code AS OnCarriageToPortCode,
MainCarriageToPorts.EnglishName AS MainCarriageToPortName, MainCarriageFromPorts.EnglishName AS MainCarriageFromPortName,
Transshipment1FromPorts.EnglishName AS Transshipment1FromPortName, Transshipment1ToPorts.EnglishName AS Transshipment1ToPortName,
Transshipment2ToPorts.EnglishName AS Transshipment2ToPortName, Transshipment2FromPorts.EnglishName AS Transshipment2FromPortName,
PreCarriageFromPorts.EnglishName AS PreCarriageFromPortName, OnCarriageFromPorts.EnglishName AS OnCarriageFromPortName,
PreCarriageToPorts.EnglishName AS PreCarriageToPortName, Transshipment3FromPorts.EnglishName AS Transshipment3FromPortName,
Transshipment3ToPorts.EnglishName AS Transshipment3ToPortName, OnCarriageToPorts.EnglishName AS OnCarriageToPortName,
CustomerCards.EnglishName AS CustomerName, CustomerCards.Notes AS CustomerNote,
ConsolidatorCards.EnglishName AS ConsolidatorName, ConsolidatorCards.Notes AS ConsolidatorNote,
FreightForwarderCards.EnglishName AS FreightForwarderName,
FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.Notes AS ShipperNote, ReleasingAgentCards.EnglishName AS ReleasingAgentName,
ConsigneeCards.EnglishName AS ConsigneeName,ConsigneeCards.EnglishName AS Consignee, ConsigneeCards.Notes AS ConsigneeNote, AgentComputedCards.EnglishName AS AgentName,
AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote,
CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote,
Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note,
ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote,
ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote,
ToPorts.Code AS ToPortCode, FromPorts.Code AS FromPortCode,
MainCarriageFromCountries.Code AS MainCarriageFromPortCountryCode, MainCarriageFromCountries.EnglishName AS MainCarriageFromPortCountryName,
MainCarriageToCountries.Code AS MainCarriageToPortCountryCode, MainCarriageToCountries.EnglishName AS MainCarriageToPortCountryName,
Transshipment1FromCountries.Code AS Transshipment1FromPortCountryCode,
Transshipment1FromCountries.EnglishName AS Transshipment1FromPortCountryName,
Transshipment2FromCountries.Code AS Transshipment2FromPortCountryCode,
Transshipment2FromCountries.EnglishName AS Transshipment2FromPortCountryName,
Transshipment3FromCountries.Code AS Transshipment3FromPortCountryCode,
Transshipment3FromCountries.EnglishName AS Transshipment3FromPortCountryName, PreCarriageFromCountries.Code AS PreCarriageFromPortCountryCode,
PreCarriageFromCountries.EnglishName AS PreCarriageFromPortCountryName, OnCarriageFromCountries.Code AS OnCarriageFromPortCountryCode,
OnCarriageFromCountries.EnglishName AS OnCarriageFromPortCountryName, Transshipment1ToCountries.Code AS Transshipment1ToPortCountryCode,
Transshipment1ToCountries.EnglishName AS Transshipment1ToPortCountryName, OnCarriageToCountries.Code AS OnCarriageToPortCountryCode,
OnCarriageToCountries.EnglishName AS OnCarriageToPortCountryName, Transshipment2ToCountries.Code AS Transshipment2ToPortCountryCode,
Transshipment2ToCountries.EnglishName AS Transshipment2ToPortCountryName, PreCarriageToCountries.Code AS PreCarriageToPortCountryCode,
PreCarriageToCountries.EnglishName AS PreCarriageToPortCountryName, Transshipment3ToCountries.Code AS Transshipment3ToPortCountryCode,
Transshipment3ToCountries.EnglishName AS Transshipment3ToPortCountryName, MainCarriageCarrierCards.EnglishName AS MainCarriageCarrierName,
MainCarriageCarrierCards.Code AS MainCarriageCarrierCode, Transshipment1CarrierCards.EnglishName AS Transshipment1CarrierName,
Transshipment1CarrierCards.Code AS Transshipment1CarrierCode, Transshipment2CarrierCards.EnglishName AS Transshipment2CarrierName,
Transshipment2CarrierCards.Code AS Transshipment2CarrierCode, Transshipment3CarrierCards.EnglishName AS Transshipment3CarrierName,
Transshipment3CarrierCards.Code AS Transshipment3CarrierCode, PreCarriageCarrierCards.EnglishName AS PreCarriageCarrierName,
PreCarriageCarrierCards.Code AS PreCarriageCarrierCode, OnCarriageCarrierCards.EnglishName AS OnCarriageCarrierName,
OnCarriageCarrierCards.Code AS OnCarriageCarrierCode, dbo.NextLegs.Name AS NextLegName, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.ShipmentReceivableStatus.Name AS ShipmentReceivableStatusName, dbo.ShipmentPayableStatus.Name AS ShipmentPayableStatusName,
AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName,dbo.MoveTypes.MoveTypeEnglishName AS MoveTypeName ,
dbo.ShipmentLevels.Name AS ShipmentLevelName,
dbo.EntityStatus.Id AS ShipmentStatusId,
dbo.EntityStatus.Name AS ShipmentStatusName,
dbo.EntityStatus.StatusWeight AS ShipmentStatusWeight,
dbo.Shipments.StatusDate as ShipmentStatusDate,
dbo.Shipments.StatusLocation as ShipmentStatusLocation,
ShipmentMasterDataEntityStatus.Id AS ShipmentMasterDataStatusId,
ShipmentMasterDataEntityStatus.Name AS ShipmentMasterDataStatusName,
ShipmentMasterDataEntityStatus.StatusWeight AS ShipmentMasterDataStatusWeight,
dbo.ShipmentMasterDatas.StatusDate As ShipmentMasterDataStatusDate,
dbo.ShipmentMasterDatas.StatusLocation as ShipmentMasterDataStatusLocation,
MainCarriageFinalDestinationPorts.Code AS MainCarriageFinalDestinationPortCode,
MainCarriageFinalDestinationPorts.EnglishName AS MainCarriageFinalDestinationPortName,
FinalDestinationCountries.Code AS MainCarriageFinalDestinationCountryCode,
FinalDestinationCountries.EnglishName AS MainCarriageFinalDestinationCountryName,
dbo.ShipmentMasterDatas.MasterShipmentNumber,
dbo.ShipmentMasterDatas.CarrierTransportDocumentNumber,
dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,dbo.Shipments.OperationalClosedByUserId,
dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency,
dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.GrossWeightPerStorageDays,dbo.Shipments.GrossWeightPerTon,
dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight,
dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Incoterms.Code AS IncotermCode,
dbo.FHLStatus.Code AS FHLStatusCode,
dbo.FHLStatus.Name AS FHLStatusName,
dbo.Shipments.FHLStatusDate,
dbo.FWBStatus.Code AS FWBStatusCode,
dbo.FWBStatus.Name AS FWBStatusName,
dbo.ShipmentMasterDatas.FWBStatusDate,
dbo.Shipments.LastSentByUserId,
dbo.CustomsTransmissionsStatus.Code AS LocalCustomsTransmissionsStatusCode,
dbo.CustomsTransmissionsStatus.Name AS LocalCustomsTransmissionsStatusName,
dbo.shipments.LocalCustomsTransmissionsStatusError,
dbo.Shipments.LocalCustomsTransmissionsStatusDate,
dbo.Shipments.LocalCustomsSentByUserId,
LocalCustomsSentByUser.EnglishName as LocalCustomsSentByUserName,
CargonautFHLStatus.Code AS CargonautFHLStatusCode,
CargonautFHLStatus.Name AS CargonautFHLStatusName,
dbo.Shipments.CargonautFHLStatusDate,
CargonautFWBStatus.Code AS CargonautFWBStatusCode,
CargonautFWBStatus.Name AS CargonautFWBStatusName,
dbo.ShipmentMasterDatas.CargonautFWBStatusDate,
dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason, dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber,
dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges, dbo.Shipments.AccountNumber,
dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate, dbo.Shipments.LastFSRStatusRequestDate,
dbo.Shipments.FinalArrivalDate, dbo.Shipments.EstimatedFinalArrivalDate, dbo.Shipments.ActualFinalArrivalDate, FromPortCountries.Code AS FromPortCountryCode,
ToPortCountries.Code AS ToPortCountryCode, dbo.Shipments.TEU, MainCarriageFromAddresses.City AS MainCarriageFromCity,
MainCarriageToAddresses.City AS MainCarriageToCity, MainCarriageFromAddressCountries.Code AS MainCarriageFromCountryCode,
MainCarriageToAddressCountries.Code AS MainCarriageToCountryCode, dbo.Shipments.ProfitExchangeRate, dbo.Shipments.CASSCode, dbo.Shipments.AMSBL,
dbo.Shipments.SpecialServicesTypeId, dbo.SpecialServicesTypes.Code AS SpecialServicesTypeCode,
dbo.SpecialServicesTypes.EnglishName AS SpecialServicesTypeName, dbo.Shipments.CustomFileId, dbo.Shipments.CustomFileNumber,
dbo.Shipments.NoFreightFile, ToPortCountries.EnglishName AS ToPortCountryName, FromPortCountries.EnglishName AS FromPortCountryName,
dbo.Vessels.EnglishName AS MainCarriageVesselName,dbo.Shipments.LastStatusLogDate As LastStatusLogDate,
AccountManagerUserContacts.EnglishName as AccountManagerUserName,
SalesmanUserContact.EnglishName as SalesmanUserName,
CreatedByUserContact.EnglishName as CreatedByUserName,
LastSentByUserContact.EnglishName as LastSentByUserName,
ShipmentComputedFields.IsMissingDocuments as IsMissingDocument,
ShipmentComputedFields.DocumentsSearchFields as DocumentsSearchFields,
dbo.Shipments.ForwarderShipmentNumber as ForwarderShipmentNumber,
dbo.Shipments.CustomerShipmentNumber as CustomerShipmentNumber,
ShipmentComputedFields.LastDocumentDateTime as LastDocumentDateTime,
HybridPartner.SmallLogoId as PartnerLogoId,
ShipmentComputedFields.MissingDocumentsCount as MissingDocumentsCount,
ShipmentComputedFields.MissingDocumentsNames as MissingDocsNames,
ShipmentComputedFields.RequestedDocumentsCount as RequestedDocumentsCount,
ShipmentComputedFields.IsRequestedDocuments as IsRequestedDocuments,
ShipmentComputedFields.IsDigitalSignRequired as IsDigitalSignRequired,
ShipmentComputedFields.IsDepositionRequired as IsDepositionRequired,
ShipmentComputedFields.CreatedFromDigital as CreatedFromDigital,
ShipmentComputedFields.ImporterDepositionRequestDetails as ImporterDepositionRequestDetails,
ShipmentComputedFields.NumberOfHouses as NumberOfHouses,
ShipmentAdditionalCloudDatas.DeclarationXmlData as DeclarationXmlData,
ShipmentAdditionalCloudDatas.IsImporterApprovalRequried as IsImporterApprovalRequried,
ShipmentAdditionalCloudDatas.ApprovedByUserName as ApprovedByUserName,
ShipmentAdditionalCloudDatas.ApproveDateTime as ApproveDateTime,
ShipmentAdditionalCloudDatas.VersionApproved as VersionApproved,
HybridPartner.Name as PartnerName,
HybridPartner.Id as ForwarderPartnerId,
dbo.ShipmentMasterDatas.DepartureArrivalFromDate as DepartureArrivalFromDate,
dbo.ShipmentMasterDatas.DepartureArrivalToDate as DepartureArrivalToDate,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationETA as MainCarriageFinalDestinationETA,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationATA as MainCarriageFinalDestinationATA,
CASE WHEN ( NOT ((dbo.ShipmentTypes.Name IS NULL) OR ((LEN(dbo.ShipmentTypes.Name)) = 0))) THEN CASE WHEN (dbo.ShipmentTypes.Name IS NULL) THEN N'''' ELSE dbo.ShipmentTypes.Name END + N'' '' + CASE WHEN (dbo.ShipmentLevels.Name IS NULL) THEN N'''' ELSE dbo.ShipmentLevels.Name END ELSE dbo.ShipmentLevels.Name END AS ShipmentType,
CASE WHEN ( NOT ((MainCarriageFromPortId IS NULL) OR ((LEN(MainCarriageFromPortId)) = 0))) THEN MainCarriageFromPortId ELSE dbo.Shipments.FromPortId END AS FromPortId,
CASE WHEN ( NOT ((MainCarriageToPortId IS NULL) OR ((LEN(MainCarriageToPortId)) = 0))) THEN MainCarriageToPortId ELSE dbo.Shipments.ToPortId END AS ToPortId,
CASE WHEN ( NOT ((MainCarriageFromPorts.Code IS NULL) OR ((LEN(MainCarriageFromPorts.Code)) = 0))) THEN MainCarriageFromPorts.Code ELSE FromPorts.Code END AS FromPort,
CASE WHEN ( NOT ((MainCarriageFromPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFromPorts.EnglishName)) = 0))) THEN MainCarriageFromPorts.EnglishName ELSE FromPorts.EnglishName END AS FromPortName,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.Code IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.Code)) = 0))) THEN MainCarriageFinalDestinationPorts.Code ELSE ToPorts.Code END AS ToPort,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.EnglishName)) = 0))) THEN MainCarriageFinalDestinationPorts.EnglishName ELSE ToPorts.EnglishName END AS ToPortName,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (MainCarriageCarrierCards.Code IS NULL) THEN N'''' ELSE MainCarriageCarrierCards.Code END + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''O'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (dbo.Vessels.EnglishName IS NULL) THEN N'''' ELSE dbo.Vessels.EnglishName END + N''/'' + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''I'' = dbo.Shipments.TransportModeId) THEN MainCarriageCarrierNumber END AS CarrierNumber,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN MainCarriageATA ELSE MainCarriageETA END AS MainCarriageExpectedOrActual,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN N''ATA'' ELSE N''ETA'' END AS MainCarriageETAOrATA,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusDate ELSE dbo.Shipments.StatusDate END ELSE dbo.Shipments.StatusDate END AS StatusDate,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Name ELSE dbo.EntityStatus.Name END ELSE dbo.EntityStatus.Name END AS StatusName,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusLocation ELSE dbo.Shipments.StatusLocation END ELSE dbo.Shipments.StatusLocation END AS StatusLocation,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (( NOT ((AirlinePrefix IS NULL) OR ((LEN(AirlinePrefix)) = 0))) AND ( NOT ((Master IS NULL) OR ((LEN(Master)) = 0)))) THEN CASE WHEN (AirlinePrefix IS NULL) THEN N'''' ELSE AirlinePrefix END + N''-'' + CASE WHEN (Master IS NULL) THEN N'''' ELSE Master END ELSE N'''' END ELSE Master END AS LongMaster,
CAST( MissingDocumentsCount AS nvarchar(max)) + N'' Missing'' AS MissingDocumentsCountWords,
CASE WHEN (IsOperationalClosed = 1) THEN N''Archived'' ELSE N'''' END AS ArchivedText
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageFromAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageToAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
--ELSE MainCarriageFinalDestinationPortsStates.EnglishName END) END AS MainCarriageToState
FROM            dbo.Shipments LEFT OUTER JOIN
dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId LEFT OUTER JOIN
dbo.Ports AS MainCarriageFromPorts ON dbo.ShipmentMasterDatas.MainCarriageFromPortId = MainCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageToPorts ON dbo.ShipmentMasterDatas.MainCarriageToPortId = MainCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1FromPorts ON dbo.ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1ToPorts ON dbo.ShipmentMasterDatas.Transshipment1ToPortId = Transshipment1ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2FromPorts ON dbo.ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2ToPorts ON dbo.ShipmentMasterDatas.Transshipment2ToPortId = Transshipment2ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3FromPorts ON dbo.ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3ToPorts ON dbo.ShipmentMasterDatas.Transshipment3ToPortId = Transshipment3ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageFinalDestinationPorts ON dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageFromPorts ON dbo.Shipments.PreCarriageFromPortId = PreCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageToPorts ON dbo.Shipments.PreCarriageToPortId = PreCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageFromPorts ON dbo.Shipments.OnCarriageFromPortId = OnCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageToPorts ON dbo.Shipments.OnCarriageToPortId = OnCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Shipments.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Shipments.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Cards AS CustomerCards ON dbo.Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsolidatorCards ON dbo.Shipments.ConsolidatorId = ConsolidatorCards.Id LEFT OUTER JOIN
dbo.Cards AS FreightForwarderCards ON dbo.Shipments.FreightForwarderId = FreightForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Shipments.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Shipments.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentCards ON dbo.Shipments.AgentId = AgentCards.Id LEFT OUTER JOIN
dbo.Cards AS ReleasingAgentCards ON dbo.Shipments.ReleasingAgentId = ReleasingAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentComputedCards ON dbo.Shipments.AgentComputed = AgentComputedCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentExportCards ON dbo.Shipments.CustomAgentExportId = CustomAgentExportCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentImportCards ON dbo.Shipments.CustomAgentImportId = CustomAgentImportCards.Id LEFT OUTER JOIN
dbo.Cards AS Notify1Cards ON dbo.Shipments.Notify1Id = Notify1Cards.Id LEFT OUTER JOIN
dbo.Cards AS Notify2Cards ON dbo.Shipments.Notify2Id = Notify2Cards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperNotExporterCards ON dbo.Shipments.ShipperNotExporterId = ShipperNotExporterCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeNotImporterCards ON dbo.Shipments.ConsigneeNotImporterId = ConsigneeNotImporterCards.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromCountries ON MainCarriageFromPorts.CountryId = MainCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToCountries ON MainCarriageToPorts.CountryId = MainCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1FromCountries ON Transshipment1FromPorts.CountryId = Transshipment1FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2FromCountries ON Transshipment2FromPorts.CountryId = Transshipment2FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3FromCountries ON Transshipment3FromPorts.CountryId = Transshipment3FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageFromCountries ON PreCarriageFromPorts.CountryId = PreCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageFromCountries ON OnCarriageFromPorts.CountryId = OnCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1ToCountries ON Transshipment1ToPorts.CountryId = Transshipment1ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2ToCountries ON Transshipment2ToPorts.CountryId = Transshipment2ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3ToCountries ON Transshipment3ToPorts.CountryId = Transshipment3ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageToCountries ON PreCarriageToPorts.CountryId = PreCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageToCountries ON OnCarriageToPorts.CountryId = OnCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS FinalDestinationCountries ON MainCarriageFinalDestinationPorts.CountryId = FinalDestinationCountries.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarrierCards ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment1CarrierCards ON dbo.ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment2CarrierCards ON dbo.ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment3CarrierCards ON dbo.ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS PreCarriageCarrierCards ON dbo.Shipments.PreCarriageCarrierId = PreCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS OnCarriageCarrierCards ON dbo.Shipments.OnCarriageCarrierId = OnCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS WarehouseLegCard ON dbo.Shipments.WarehouseLegWarehouseId = WarehouseLegCard.Id LEFT OUTER JOIN
dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
dbo.MoveTypes ON dbo.Shipments.MoveTypeId = dbo.MoveTypes.Id LEFT OUTER JOIN
dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.CustomsTransmissionsStatus ON dbo.Shipments.LocalCustomsTransmissionsStatusCode = dbo.CustomsTransmissionsStatus.Code LEFT OUTER JOIN
dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
dbo.INTTRASIStatus ON dbo.Shipments.INTTRASIStatusCode = dbo.INTTRASIStatus.Code LEFT OUTER JOIN
dbo.INTTRABookingTransStatuses ON dbo.Shipments.INTTRABookingTransStatusCode = dbo.INTTRABookingTransStatuses.Code LEFT OUTER JOIN
dbo.INTTRABookingStatuses ON dbo.Shipments.INTTRABookingStatusCode = dbo.INTTRABookingStatuses.Code LEFT OUTER JOIN
dbo.Addresses AS MainCarriageFromAddresses ON dbo.ShipmentMasterDatas.MainCarriageFromAddressId = MainCarriageFromAddresses.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageToAddresses ON dbo.ShipmentMasterDatas.MainCarriageToAddressId = MainCarriageToAddresses.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageToPartners ON dbo.ShipmentMasterDatas.MainCarriageToPartnerId = MainCarriageToPartners.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageFromPartners ON dbo.ShipmentMasterDatas.MainCarriageFromPartnerId = MainCarriageFromPartners.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromAddressCountries ON MainCarriageFromAddresses.CountryId = MainCarriageFromAddressCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToAddressCountries ON MainCarriageToAddresses.CountryId = MainCarriageToAddressCountries.Id LEFT OUTER JOIN
dbo.SpecialServicesTypes ON dbo.Shipments.SpecialServicesTypeId = dbo.SpecialServicesTypes.Id LEFT OUTER JOIN
dbo.Vessels ON dbo.ShipmentMasterDatas.MainCarriageVesselId = dbo.Vessels.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Shipments.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContact ON dbo.Shipments.SalesmanUserId = SalesmanUserContact.Id LEFT OUTER JOIN
dbo.HybridPartners AS HybridPartner ON dbo.Shipments.ForwarderPartnerId = HybridPartner.Id LEFT OUTER JOIN
dbo.EventTypes ON dbo.Shipments.LastSharedEventId = dbo.EventTypes.Id LEFT OUTER JOIN
dbo.Contacts AS LastSentByUserContact ON dbo.Shipments.LastSentByUserId = LastSentByUserContact.Id LEFT OUTER JOIN
dbo.Contacts AS LocalCustomsSentByUser ON dbo.Shipments.LocalCustomsSentByUserId = LocalCustomsSentByUser.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContact ON dbo.Shipments.CreatedByUserId = CreatedByUserContact.Id INNER JOIN
dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id INNER JOIN
dbo.ShipmentAdditionalCloudDatas AS ShipmentAdditionalCloudDatas ON dbo.Shipments.Id = ShipmentAdditionalCloudDatas.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromAddressesStates ON MainCarriageFromAddresses.StateId = MainCarriageFromAddressesStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageToAddressesStates ON MainCarriageToAddresses.StateId = MainCarriageToAddressesStates.Id LEFT OUTER JOIN
dbo.States AS FromPortsStates ON FromPorts.StateId = FromPortsStates.Id  LEFT OUTER JOIN
dbo.States AS ToPortsStates ON ToPorts.StateId = ToPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromPortsStates ON MainCarriageFromPorts.StateId = MainCarriageFromPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id');


-- Procedure Script From DeleteDuplicatedQueueMessagesProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[RemoveDuplicatedQueueMessages]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[RemoveDuplicatedQueueMessages] END');
EXEC('CREATE procedure [dbo].[RemoveDuplicatedQueueMessages]
as begin
declare @HashCode as varchar(1000)
declare duplicatedQueuesCursor Cursor read_only
for
select hashcode
from QueueMessages WITH (UPDLOCK, READPAST,ROWLOCK)  inner join QueueDefinitions on QueueMessages.QueueDefinitionCode = QueueDefinitions.Code
WHERE Status = 0 and QueueDefinitions.DuplicateMessagesAutoRemove = 1 and HashCode is not null
group by HashCode
having count(*) > 1
open duplicatedQueuesCursor
fetch next from duplicatedQueuesCursor into @HashCode
while @@FETCH_STATUS = 0
begin
--print @HashCode
declare @TopQueueMessageId as varchar(1000)
set @TopQueueMessageId = (select top 1 id from QueueMessages where hashcode = @HashCode order by NextRunDateTime ASC)
--print ''top queue message id:'' + @TopQueueMessageId
update QueueMessages set Status = 2 where hashcode = @HashCode and Id <> @TopQueueMessageId
fetch next from duplicatedQueuesCursor into @HashCode
end
close duplicatedQueuesCursor
deallocate duplicatedQueuesCursor
end;');


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


-- Procedure Script From Queue_Peek.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_Peek]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_Peek] END');
EXEC('CREATE procedure [dbo].[Queue_Peek]
(@MessageId  bigint output,
@MessageBody  varchar(1000) output,
@RetryNumber int output,
@QueueDefinitionCode varchar(255),
@WatingStatus int)
as
begin
--DECLARE @NextId INTEGER
Declare @frequency INTEGER
set @frequency = 60
-- Find next available item available where the status is enabled
SET @MessageId = (SELECT TOP 1 [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK, READPAST,ROWLOCK) WHERE [NextRunDateTime] <= getdate() and [Status] = @WatingStatus and QueueDefinitionCode = @QueueDefinitionCode ORDER BY [NextRunDateTime] ASC)
--If found, flag it to prevent being picked up again
IF (@MessageId IS NOT NULL)
BEGIN
Set @MessageBody = (Select MessageBody from [dbo].[QueueMessages] where [Id] = @MessageId)
Set @RetryNumber = (Select RetryNumber from [dbo].[QueueMessages] where [Id] = @MessageId)
Set @MessageBody = (Select MessageBody from [dbo].[QueueMessages] where [Id] = @MessageId)
UPDATE [dbo].[QueueMessages]
SET [ProcessingDateTime] = getdate(),[NextRunDateTime] = dateadd(second,@frequency,getdate()),RetryNumber = (RetryNumber+1)
WHERE [Id] = @MessageId
UPDATE [dbo].[QueueMessageMoreDetails]
SET [ProcessingDateTime] = getdate(),[NextRunDateTime] = dateadd(second,@frequency,getdate()),RetryNumber = (RetryNumber+1)
WHERE [Id] = @MessageId
END
-- return queue data
--IF (@NextId IS NOT NULL)
--select [QueueID],[QueueDateTime],[Title],[Status],[TextData],[NextRunTime],[ProcessingTime]
--from [dbo].[TasksQueue]
--where [QueueID] = @NextId
end');


-- General Script From 202006092059_SetPermissionToAllBIFolders.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update BIReportFolders set PermissionForAll = 1
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006092059_SetPermissionToAllBIFolders.sxml', GETDATE(), 'update BIReportFolders set PermissionForAll = 1', DATEDIFF(MS,@StartTime,@EndTime), '1f42afaf1e581cb5298189859895c618', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

