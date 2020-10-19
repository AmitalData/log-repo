-- Drop Primary Key Constraint
EXEC('IF (OBJECT_ID(''[dbo].[PK_CalculatedChartsOfAccountsLines]'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CalculatedChartsOfAccountLines] DROP CONSTRAINT [PK_CalculatedChartsOfAccountsLines] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7547f0e0-dd84-4887-9620-e982b90c2477', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountLines', NULL, 'Drop Primary Key Constraint', GETDATE(), '-- Drop Primary Key ConstraintEXEC(''IF (OBJECT_ID(''''[dbo].[PK_CalculatedChartsOfAccountsLines]'''', ''''PK'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CalculatedChartsOfAccountLines] DROP CONSTRAINT [PK_CalculatedChartsOfAccountsLines] END'');');

-- Add Primary Key Constraint
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountLines] ADD CONSTRAINT [PK_CalculatedChartsOfAccountsLines] PRIMARY KEY ([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f621936a-fe32-44a3-bfb5-6410f2d052fb', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountLines', 'Id', 'Add Primary Key Constraint', GETDATE(), '-- Add Primary Key ConstraintEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountLines] ADD CONSTRAINT [PK_CalculatedChartsOfAccountsLines] PRIMARY KEY ([Id])'');');


