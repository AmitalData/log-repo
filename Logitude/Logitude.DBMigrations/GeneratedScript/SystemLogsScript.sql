-- Create New Table With Name DBMigrationsHistory
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


-- Create New Table With Name DBScriptsHistory
CREATE TABLE [dbo].[DBScriptsHistory](
[SxmlFileName] VARCHAR(500) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[ScriptBody] NVARCHAR(MAX) NOT NULL,
[ElapsedTimeInMs] BIGINT NOT NULL,
[HashValue] NVARCHAR(MAX) NOT NULL,
[Version] INT NOT NULL,
CONSTRAINT [PK_DBScriptsHistory] PRIMARY KEY([SxmlFileName])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d26eee6a-74fb-4cd0-a7f2-730bdae1e70c', 'DBScriptsHistory.dxml', 'DBScriptsHistory', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name DBScriptsHistoryCREATE TABLE [dbo].[DBScriptsHistory]([SxmlFileName] VARCHAR(500) NOT NULL,[ExecutionDate] DATETIME NOT NULL,[ScriptBody] NVARCHAR(MAX) NOT NULL,[ElapsedTimeInMs] BIGINT NOT NULL,[HashValue] NVARCHAR(MAX) NOT NULL,[Version] INT NOT NULL,CONSTRAINT [PK_DBScriptsHistory] PRIMARY KEY([SxmlFileName]));');


-- General Script From 202006171015_TruncateErrorLogs.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
truncate table ErrorLogs
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006171015_TruncateErrorLogs.sxml', GETDATE(), 'truncate table ErrorLogs', DATEDIFF(MS,@StartTime,@EndTime), '4b0ef6ced1a3cd4c85a6be7db124f40c', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Add Default Value For Column IsSharedLogisticsContact
ALTER TABLE [dbo].[ContactActivityLogs] ADD DEFAULT 0 FOR [IsSharedLogisticsContact];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aec5d96c-a7d0-4358-b9f9-8e78e4672a54', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'IsSharedLogisticsContact', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsSharedLogisticsContactALTER TABLE [dbo].[ContactActivityLogs] ADD DEFAULT 0 FOR [IsSharedLogisticsContact];');


-- Unset Nullable For Column Exception
ALTER TABLE [dbo].[ErrorLogs] ALTER COLUMN [Exception] VARCHAR(7000) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5813eb8c-d7ab-46d5-aa48-269451c31940', 'ErrorLog.dxml', 'ErrorLogs', 'Exception', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column ExceptionALTER TABLE [dbo].[ErrorLogs] ALTER COLUMN [Exception] VARCHAR(7000) NOT NULL;');


-- Procedure Script From DeleteOldErrorLog.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldErrorLog]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldErrorLog] END');
EXEC('Create procedure [dbo].[DeleteOldErrorLog]
as
begin
delete from [dbo].[ErrorLogs] where [LogDate] < GETDATE() - 30
end');


