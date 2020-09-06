-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId]) INCLUDE([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('77860797-3059-4416-98b0-12168e79e565', 'CardSearch.dxml', 'CardSearches', 'Tenant,InActive,Keyword,PartnerTypeId', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId]) INCLUDE([CardId])'');');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_Weight] ON [dbo].[CardSearches]([Tenant],[Weight])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3fd0b483-145d-4d5e-bef7-22825d520360', 'CardSearch.dxml', 'CardSearches', 'Tenant,Weight', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_Weight] ON [dbo].[CardSearches]([Tenant],[Weight])'');');


