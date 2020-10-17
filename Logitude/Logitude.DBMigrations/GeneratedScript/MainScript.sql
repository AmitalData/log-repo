-- Add New Column With Name Line
ALTER TABLE [dbo].[CalculatedChartsOfAccounts] ADD [Line] INT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('78281e0e-7e6d-4195-a8c5-bbe9bbe9babe', 'CalculatedChartsOfAccount.dxml', 'CalculatedChartsOfAccounts', 'Line', 'Add Column', GETDATE(), '-- Add New Column With Name LineALTER TABLE [dbo].[CalculatedChartsOfAccounts] ADD [Line] INT NOT NULL;');


-- Add New Column With Name Line
ALTER TABLE [dbo].[CalculatedChartsOfAccountLines] ADD [Line] INT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b8e06c0d-e4fb-4280-87f9-c9f1970ca22e', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountLines', 'Line', 'Add Column', GETDATE(), '-- Add New Column With Name LineALTER TABLE [dbo].[CalculatedChartsOfAccountLines] ADD [Line] INT NOT NULL;');


