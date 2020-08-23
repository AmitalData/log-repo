-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_RecordDate] ON [dbo].[CardSearches]([Tenant],[RecordDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('330958c2-f2f8-4313-b245-923549f20358', 'CardSearch.dxml', 'CardSearches', 'Tenant,RecordDate', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_RecordDate] ON [dbo].[CardSearches]([Tenant],[RecordDate])'');');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_CardId] ON [dbo].[CardSearches]([Tenant],[CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4ec33096-69b0-48cc-a338-abcc2eefb605', 'CardSearch.dxml', 'CardSearches', 'Tenant,CardId', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_CardId] ON [dbo].[CardSearches]([Tenant],[CardId])'');');


