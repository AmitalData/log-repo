-- Add New Column With Name ResultType
ALTER TABLE [dbo].[TasksScheduler] ADD [ResultType] VARCHAR(10) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('75f0e48a-3cf8-4b18-9884-3c6b7194af8e', 'TasksScheduler.dxml', 'TasksScheduler', 'ResultType', 'Add Column', GETDATE(), '-- Add New Column With Name ResultTypeALTER TABLE [dbo].[TasksScheduler] ADD [ResultType] VARCHAR(10) NULL;');


-- Add New Column With Name IsFromInterestBatchInvoice
ALTER TABLE [dbo].[ARInvoices] ADD [IsFromInterestBatchInvoice] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('78e41ec3-6041-4b88-969a-cf181d9d8bee', 'ARInvoice.dxml', 'ARInvoices', 'IsFromInterestBatchInvoice', 'Add Column', GETDATE(), '-- Add New Column With Name IsFromInterestBatchInvoiceALTER TABLE [dbo].[ARInvoices] ADD [IsFromInterestBatchInvoice] BIT DEFAULT(0) NOT NULL;');


-- Change Size From 1000 To 2000 For Column ContainersNumbersandTypesArray
ALTER TABLE [dbo].[ShipmentComputedFields] ALTER COLUMN [ContainersNumbersandTypesArray] NVARCHAR(2000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('60ea5d62-f3cc-4d4e-91d9-0a76e7f1ce7d', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ContainersNumbersandTypesArray', 'Alter Column Size', GETDATE(), '-- Change Size From 1000 To 2000 For Column ContainersNumbersandTypesArrayALTER TABLE [dbo].[ShipmentComputedFields] ALTER COLUMN [ContainersNumbersandTypesArray] NVARCHAR(2000);');


