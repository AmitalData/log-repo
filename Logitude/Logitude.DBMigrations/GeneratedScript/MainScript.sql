-- Add New Column With Name IsFromInterestBatchInvoice
ALTER TABLE [dbo].[ARInvoices] ADD [IsFromInterestBatchInvoice] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f959ea34-b0cd-4797-bf35-8284785d7446', 'ARInvoice.dxml', 'ARInvoices', 'IsFromInterestBatchInvoice', 'Add Column', GETDATE(), '-- Add New Column With Name IsFromInterestBatchInvoiceALTER TABLE [dbo].[ARInvoices] ADD [IsFromInterestBatchInvoice] BIT DEFAULT(0) NOT NULL;');


