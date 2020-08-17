-- Create Index On CardSearchs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearchs_Keyword_Tenant_PartnerTypeId_InActive] ON [dbo].[CardSearchs]([Keyword],[Tenant],[PartnerTypeId],[InActive])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3d162135-7a93-4060-81d4-c11132173d1c', 'CardSearch.dxml', 'CardSearchs', 'Keyword,Tenant,PartnerTypeId,InActive', 'Create Index', GETDATE(), '-- Create Index On CardSearchs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearchs_Keyword_Tenant_PartnerTypeId_InActive] ON [dbo].[CardSearchs]([Keyword],[Tenant],[PartnerTypeId],[InActive])'');');

-- Create Index On CardSearchs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearchs_RecordDate] ON [dbo].[CardSearchs]([RecordDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2277bae7-cdcb-4684-b7c2-4e110cf90993', 'CardSearch.dxml', 'CardSearchs', 'RecordDate', 'Create Index', GETDATE(), '-- Create Index On CardSearchs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearchs_RecordDate] ON [dbo].[CardSearchs]([RecordDate])'');');


