-- General Script From AddMigrationHistoryToCargoTracking.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
CREATE TABLE [dbo].[DBScriptsHistory](
[SxmlFileName] VARCHAR(500) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[ScriptBody] NVARCHAR(MAX) NOT NULL,
[ElapsedTimeInMs] BIGINT NOT NULL,
[HashValue] NVARCHAR(MAX) NOT NULL,
[Version] INT NOT NULL,
CONSTRAINT [PK_DBScriptsHistory] PRIMARY KEY([SxmlFileName])
);
CREATE TABLE [dbo].[DBMigrationsHistory](
[Id] NVARCHAR(128) NOT NULL,
[DxmlFileName] VARCHAR(500) NOT NULL,
[TableName] VARCHAR(500) NOT NULL,
[ColumnName] VARCHAR(MAX) NULL,
[MigrationType] VARCHAR(50) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[MigrationScript] NVARCHAR(MAX) NOT NULL,
CONSTRAINT [PK_DBMigrationsHistory] PRIMARY KEY([Id])
);
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('AddMigrationHistoryToCargoTracking.sxml', GETDATE(), 'CREATE TABLE [dbo].[DBScriptsHistory](
[SxmlFileName] VARCHAR(500) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[ScriptBody] NVARCHAR(MAX) NOT NULL,
[ElapsedTimeInMs] BIGINT NOT NULL,
[HashValue] NVARCHAR(MAX) NOT NULL,
[Version] INT NOT NULL,
CONSTRAINT [PK_DBScriptsHistory] PRIMARY KEY([SxmlFileName])
);
CREATE TABLE [dbo].[DBMigrationsHistory](
[Id] NVARCHAR(128) NOT NULL,
[DxmlFileName] VARCHAR(500) NOT NULL,
[TableName] VARCHAR(500) NOT NULL,
[ColumnName] VARCHAR(MAX) NULL,
[MigrationType] VARCHAR(50) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[MigrationScript] NVARCHAR(MAX) NOT NULL,
CONSTRAINT [PK_DBMigrationsHistory] PRIMARY KEY([Id])
);', DATEDIFF(MS,@StartTime,@EndTime), '901f6b2d317b58aba1f1a2b45418b013', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Drop Index IX_CargoTrackingShipments_CustomerReference From Table CargoTrackingShipments
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_CargoTrackingShipments_CustomerReference'' AND object_id = OBJECT_ID(''[dbo].[CargoTrackingShipments]'', ''U'')) BEGIN DROP INDEX [IX_CargoTrackingShipments_CustomerReference] ON [dbo].[CargoTrackingShipments] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1154eb0a-709d-4ec3-9082-3fffcc54889d', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_CargoTrackingShipments_CustomerReference From Table CargoTrackingShipmentsEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_CargoTrackingShipments_CustomerReference'''' AND object_id = OBJECT_ID(''''[dbo].[CargoTrackingShipments]'''', ''''U'''')) BEGIN DROP INDEX [IX_CargoTrackingShipments_CustomerReference] ON [dbo].[CargoTrackingShipments] END'');');

-- Change Size From 100 To 101 For Column CustomerReference
ALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [CustomerReference] VARCHAR(101);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8c452f66-111f-433c-a8b0-edae5d8b4f37', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CustomerReference', 'Alter Column Size', GETDATE(), '-- Change Size From 100 To 101 For Column CustomerReferenceALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [CustomerReference] VARCHAR(101);');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CustomerReference] ON [dbo].[CargoTrackingShipments]([CustomerReference])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f27210bf-7a01-4ccf-ad9b-b30c5546be9d', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CustomerReference', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CustomerReference] ON [dbo].[CargoTrackingShipments]([CustomerReference])'');');


