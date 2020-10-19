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

-- Create New Table With Name CargoTrackingShipmentSearches
CREATE TABLE [dbo].[CargoTrackingShipmentSearches](
[Tenant] INT NOT NULL,
[SearchFields] NVARCHAR(100) NULL,
[ShipmentDate] DATETIME NOT NULL,
[Id] INT IDENTITY(1,1) NOT NULL,
[ShipmentId] VARCHAR(15) NULL,
[IsPublic] BIT DEFAULT(0) NULL,
CONSTRAINT [PK_CargoTrackingShipmentSearches] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('93de1fe6-71c4-4dbc-be25-be27e443f389', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CargoTrackingShipmentSearchesCREATE TABLE [dbo].[CargoTrackingShipmentSearches]([Tenant] INT NOT NULL,[SearchFields] NVARCHAR(100) NULL,[ShipmentDate] DATETIME NOT NULL,[Id] INT IDENTITY(1,1) NOT NULL,[ShipmentId] VARCHAR(15) NULL,[IsPublic] BIT DEFAULT(0) NULL,CONSTRAINT [PK_CargoTrackingShipmentSearches] PRIMARY KEY([Id]));');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('86c7abaf-5dac-4f98-8700-ee3d1eb74444', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,SearchFields,IsPublic', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])'');');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_ShipmentId] ON [dbo].[CargoTrackingShipmentSearches]([ShipmentId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ba6e2fa4-e176-41b8-8cca-0b58620c8dfe', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentId', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_ShipmentId] ON [dbo].[CargoTrackingShipmentSearches]([ShipmentId])'');');


