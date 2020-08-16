-- Create Index On CardSearchs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearchs_Keyword_Tenant] ON [dbo].[CardSearchs]([Keyword],[Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8d710c3f-fdea-440e-9b6d-3d07179038bd', 'CardSearch.dxml', 'CardSearchs', 'Keyword,Tenant', 'Create Index', GETDATE(), '-- Create Index On CardSearchs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearchs_Keyword_Tenant] ON [dbo].[CardSearchs]([Keyword],[Tenant])'');');


