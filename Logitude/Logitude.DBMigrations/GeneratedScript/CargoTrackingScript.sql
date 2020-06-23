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

-- Create New Table With Name CargoTrackingPort2s
CREATE TABLE [dbo].[CargoTrackingPort2s](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
CONSTRAINT [PK_CargoTrackingPort2s] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ed68398b-9bae-4afa-a6d9-d83f412f0d1d', 'CargoTrackingPort2.dxml', 'CargoTrackingPort2s', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CargoTrackingPort2sCREATE TABLE [dbo].[CargoTrackingPort2s]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,CONSTRAINT [PK_CargoTrackingPort2s] PRIMARY KEY([Id]));');


