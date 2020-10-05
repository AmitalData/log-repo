-- Add New Column With Name NoPaymentForChildTenants
ALTER TABLE [dbo].[TenantManagements] ADD [NoPaymentForChildTenants] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7d7b8a8e-83f7-4c2d-a9ab-f62ab80d04f1', 'TenantManagement.dxml', 'TenantManagements', 'NoPaymentForChildTenants', 'Add Column', GETDATE(), '-- Add New Column With Name NoPaymentForChildTenantsALTER TABLE [dbo].[TenantManagements] ADD [NoPaymentForChildTenants] BIT DEFAULT(0) NOT NULL;');


