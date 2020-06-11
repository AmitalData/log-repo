-- Add New Column With Name ShipmentSubTypeId
ALTER TABLE [dbo].[Quotes] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b7cc81b7-2534-4bfb-be6b-8d12738434fd', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentSubTypeIdALTER TABLE [dbo].[Quotes] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;');


-- Drop Column Harmonize
EXEC SP_RENAME 'dbo.InsideShipmentPackages.Harmonize', 'Drop_Harmonize', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aa46475a-671f-466e-84f9-2b198d8edd98', 'InsideShipmentPackage.dxml', 'InsideShipmentPackages', 'Harmonize', 'Drop Column', GETDATE(), '-- Drop Column HarmonizeEXEC SP_RENAME ''dbo.InsideShipmentPackages.Harmonize'', ''Drop_Harmonize'', ''COLUMN'';');

-- Set Nullable For Column IsMultiHarmonize
ALTER TABLE [dbo].[InsideShipmentPackages] ALTER COLUMN [IsMultiHarmonize] BIT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ce256edd-c89b-41a4-bc42-a84b1de7f4be', 'InsideShipmentPackage.dxml', 'InsideShipmentPackages', 'IsMultiHarmonize', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column IsMultiHarmonizeALTER TABLE [dbo].[InsideShipmentPackages] ALTER COLUMN [IsMultiHarmonize] BIT NULL;');

-- Drop Column IsMultiHarmonize
EXEC SP_RENAME 'dbo.InsideShipmentPackages.IsMultiHarmonize', 'Drop_IsMultiHarmonize', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1679934d-1cdc-42ed-9ea1-061497b25396', 'InsideShipmentPackage.dxml', 'InsideShipmentPackages', 'IsMultiHarmonize', 'Drop Column', GETDATE(), '-- Drop Column IsMultiHarmonizeEXEC SP_RENAME ''dbo.InsideShipmentPackages.IsMultiHarmonize'', ''Drop_IsMultiHarmonize'', ''COLUMN'';');


-- Add New Column With Name ShipmentSubTypeId
ALTER TABLE [dbo].[Shipments] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('92cb890c-f95e-47d0-aea3-327a883c9670', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentSubTypeIdALTER TABLE [dbo].[Shipments] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;');


-- Drop Column InsidePackageId
EXEC SP_RENAME 'dbo.ShipmentPackageHarmonize.InsidePackageId', 'Drop_InsidePackageId', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fd631046-f220-4d49-a4f2-e66f1851196e', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Drop Column', GETDATE(), '-- Drop Column InsidePackageIdEXEC SP_RENAME ''dbo.ShipmentPackageHarmonize.InsidePackageId'', ''Drop_InsidePackageId'', ''COLUMN'';');


-- Create New Table With Name ShipmentSubTypes
CREATE TABLE [dbo].[ShipmentSubTypes](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CreateDate] DATETIME NOT NULL,
[CreatedByUserId] VARCHAR(15) NOT NULL,
[UpdateDate] DATETIME NOT NULL,
[UpdatedByUserId] VARCHAR(15) NOT NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Code] VARCHAR(5) NOT NULL,
[Name] VARCHAR(60) NOT NULL,
[Inactive] BIT DEFAULT(0) NOT NULL,
[ShipmentTypeCode] VARCHAR(4) NOT NULL,
CONSTRAINT [PK_ShipmentSubTypes] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('36893e91-89ed-4baf-8ae3-25e4fa88cb31', 'ShipmentSubType.dxml', 'ShipmentSubTypes', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name ShipmentSubTypesCREATE TABLE [dbo].[ShipmentSubTypes]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CreateDate] DATETIME NOT NULL,[CreatedByUserId] VARCHAR(15) NOT NULL,[UpdateDate] DATETIME NOT NULL,[UpdatedByUserId] VARCHAR(15) NOT NULL,[SearchFields] NVARCHAR(MAX) NULL,[Code] VARCHAR(5) NOT NULL,[Name] VARCHAR(60) NOT NULL,[Inactive] BIT DEFAULT(0) NOT NULL,[ShipmentTypeCode] VARCHAR(4) NOT NULL,CONSTRAINT [PK_ShipmentSubTypes] PRIMARY KEY([Id]));');


-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes As Reference To Column Id In Table ShipmentSubTypes
EXEC('ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('68b8fee7-bb34-448f-940b-3ee0768cc3c3', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes As Reference To Column Id In Table ShipmentSubTypesEXEC(''ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])'');');

-- Create Index On Quotes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes]([ShipmentSubTypeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a6688aa6-1a8d-4b5a-84ff-744368f3d6a7', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Create Index', GETDATE(), '-- Create Index On Quotes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes]([ShipmentSubTypeId])'');');


-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments As Reference To Column Id In Table ShipmentSubTypes
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7ca60297-0f60-435a-82d3-84b2510aba0f', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments As Reference To Column Id In Table ShipmentSubTypesEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments]([ShipmentSubTypeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('24ca98f2-8de7-46fb-9706-def94009803e', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments]([ShipmentSubTypeId])'');');


-- Drop Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize That Reference To Column Id In Table InsideShipmentPackages
EXEC('IF (OBJECT_ID(''[dbo].[FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ShipmentPackageHarmonize] DROP CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0ace69ea-64c9-435e-b3f7-c7c49b56ee18', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize That Reference To Column Id In Table InsideShipmentPackagesEXEC(''IF (OBJECT_ID(''''[dbo].[FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ShipmentPackageHarmonize] DROP CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] END'');');

-- Drop Index IX_ShipmentPackageHarmonize_InsidePackageId From Table ShipmentPackageHarmonize
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_ShipmentPackageHarmonize_InsidePackageId'' AND object_id = OBJECT_ID(''[dbo].[ShipmentPackageHarmonize]'', ''U'')) BEGIN DROP INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('12ea2cae-5347-4a02-8d76-eca13c3d2bae', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_ShipmentPackageHarmonize_InsidePackageId From Table ShipmentPackageHarmonizeEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_ShipmentPackageHarmonize_InsidePackageId'''' AND object_id = OBJECT_ID(''''[dbo].[ShipmentPackageHarmonize]'''', ''''U'''')) BEGIN DROP INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize] END'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('26e1ab5d-5451-4d0b-90ef-0689a72aad4a', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_CreatedByUserId] ON [dbo].[ShipmentSubTypes]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5561dc84-d8ae-4595-a51a-c47612c50d7f', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_CreatedByUserId] ON [dbo].[ShipmentSubTypes]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2190b06b-bb26-4430-a3c2-f50d0cb5863f', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_UpdatedByUserId] ON [dbo].[ShipmentSubTypes]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('096b3572-590d-424a-be53-b3a7365cf30d', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_UpdatedByUserId] ON [dbo].[ShipmentSubTypes]([UpdatedByUserId])'');');

-- Add Foreign Key Constraint For Column ShipmentTypeCode In Table ShipmentSubTypes As Reference To Column Id In Table ShipmentTypes
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_ShipmentTypes_ShipmentTypeCode] FOREIGN KEY([ShipmentTypeCode]) REFERENCES [dbo].[ShipmentTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('303ab68b-b1d3-47d4-b865-607dd7c7fad6', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'ShipmentTypeCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentTypeCode In Table ShipmentSubTypes As Reference To Column Id In Table ShipmentTypesEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_ShipmentTypes_ShipmentTypeCode] FOREIGN KEY([ShipmentTypeCode]) REFERENCES [dbo].[ShipmentTypes]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_ShipmentTypeCode] ON [dbo].[ShipmentSubTypes]([ShipmentTypeCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('377ab815-9c80-4b79-a73b-c388385f5dcd', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'ShipmentTypeCode', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_ShipmentTypeCode] ON [dbo].[ShipmentSubTypes]([ShipmentTypeCode])'');');


