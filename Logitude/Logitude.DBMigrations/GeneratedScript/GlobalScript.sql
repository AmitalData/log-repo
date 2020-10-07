-- Add New Column With Name NoPaymentForChildTenants
ALTER TABLE [dbo].[TenantManagements] ADD [NoPaymentForChildTenants] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7bc75e5f-c4eb-4f34-8150-276144c5b759', 'TenantManagement.dxml', 'TenantManagements', 'NoPaymentForChildTenants', 'Add Column', GETDATE(), '-- Add New Column With Name NoPaymentForChildTenantsALTER TABLE [dbo].[TenantManagements] ADD [NoPaymentForChildTenants] BIT DEFAULT(0) NOT NULL;');


