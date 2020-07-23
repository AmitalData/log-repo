-- Add New Column With Name LastRun
ALTER TABLE [dbo].[CargoTrackingWatermarks] ADD [LastRun] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('989f67fa-e476-4e14-98c3-c77cec8875f9', 'CargoTrackingWatermark.dxml', 'CargoTrackingWatermarks', 'LastRun', 'Add Column', GETDATE(), '-- Add New Column With Name LastRunALTER TABLE [dbo].[CargoTrackingWatermarks] ADD [LastRun] DATETIME NULL;');


