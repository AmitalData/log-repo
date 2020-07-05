-- Create New Table With Name InterestLastBatchServices
CREATE TABLE [dbo].[InterestLastBatchServices](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CreateReportsBatchId] VARCHAR(15) NULL,
[CreateInvoicesBatchId] VARCHAR(15) NULL,
CONSTRAINT [PK_InterestLastBatchServices] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('641f3c91-94e5-4f92-8546-9bb8edf6efe7', 'InterestLastBatchService.dxml', 'InterestLastBatchServices', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name InterestLastBatchServicesCREATE TABLE [dbo].[InterestLastBatchServices]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CreateReportsBatchId] VARCHAR(15) NULL,[CreateInvoicesBatchId] VARCHAR(15) NULL,CONSTRAINT [PK_InterestLastBatchServices] PRIMARY KEY([Id]));');


-- Create Unique Constraint On InterestLastBatchServices Table
EXEC('ALTER TABLE [dbo].[InterestLastBatchServices] ADD CONSTRAINT [UQ_InterestLastBatchServices_Tenant] UNIQUE([Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('134d24e4-2cc5-4df7-9b66-2e0689f5e9d0', 'InterestLastBatchService.dxml', 'InterestLastBatchServices', 'Tenant', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On InterestLastBatchServices TableEXEC(''ALTER TABLE [dbo].[InterestLastBatchServices] ADD CONSTRAINT [UQ_InterestLastBatchServices_Tenant] UNIQUE([Tenant])'');');


-- Add New Column With Name InvoiceFailureReason
ALTER TABLE [dbo].[InterestReports] ADD [InvoiceFailureReason] VARCHAR(1024) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3ce3c518-9c22-4278-8b42-903636fc9c8d', 'InterestReport.dxml', 'InterestReports', 'InvoiceFailureReason', 'Add Column', GETDATE(), '-- Add New Column With Name InvoiceFailureReasonALTER TABLE [dbo].[InterestReports] ADD [InvoiceFailureReason] VARCHAR(1024) NULL;');


-- Create New Table With Name InterestReportsConnectInvoices
CREATE TABLE [dbo].[InterestReportsConnectInvoices](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[ReportId] VARCHAR(15) NULL,
[InvoiceId] VARCHAR(15) NULL,
CONSTRAINT [PK_InterestReportsConnectInvoices] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6a3626ed-bb83-48cd-b4a4-883eec75e769', 'InterestReportsConnectedInvoice.dxml', 'InterestReportsConnectInvoices', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name InterestReportsConnectInvoicesCREATE TABLE [dbo].[InterestReportsConnectInvoices]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[ReportId] VARCHAR(15) NULL,[InvoiceId] VARCHAR(15) NULL,CONSTRAINT [PK_InterestReportsConnectInvoices] PRIMARY KEY([Id]));');


-- Create Unique Constraint On InterestReportsConnectInvoices Table
EXEC('ALTER TABLE [dbo].[InterestReportsConnectInvoices] ADD CONSTRAINT [UQ_InterestReportsConnectInvoices_ReportId] UNIQUE([ReportId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f0f6042b-2b7b-4340-ba48-7c2e1bc704b1', 'InterestReportsConnectedInvoice.dxml', 'InterestReportsConnectInvoices', 'ReportId', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On InterestReportsConnectInvoices TableEXEC(''ALTER TABLE [dbo].[InterestReportsConnectInvoices] ADD CONSTRAINT [UQ_InterestReportsConnectInvoices_ReportId] UNIQUE([ReportId])'');');


-- Drop Default Value For Column AutomaticLastUpdateDate
EXEC('IF (OBJECT_ID(''[dbo].[DF__ARInvoice__Autom__1BFF8E0C]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ARInvoices] DROP CONSTRAINT [DF__ARInvoice__Autom__1BFF8E0C] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b67ad3a4-664c-4540-b67b-bb7473538357', 'ARInvoice.dxml', 'ARInvoices', 'AutomaticLastUpdateDate', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column AutomaticLastUpdateDateEXEC(''IF (OBJECT_ID(''''[dbo].[DF__ARInvoice__Autom__1BFF8E0C]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ARInvoices] DROP CONSTRAINT [DF__ARInvoice__Autom__1BFF8E0C] END'');');

-- Add New Column With Name PaidDate
ALTER TABLE [dbo].[ARInvoices] ADD [PaidDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0ddfcadf-9783-4387-8555-2be3a66b6538', 'ARInvoice.dxml', 'ARInvoices', 'PaidDate', 'Add Column', GETDATE(), '-- Add New Column With Name PaidDateALTER TABLE [dbo].[ARInvoices] ADD [PaidDate] DATETIME NULL;');


-- Add New Column With Name DeliveryDate
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1b055710-9e43-4cda-a493-3e29a10205cc', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryDate', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryDateALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryDate] DATETIME NULL;');

-- Add New Column With Name OnHandDate
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [OnHandDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ffb5f893-5e91-4da6-abc4-a351cde597bf', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'OnHandDate', 'Add Column', GETDATE(), '-- Add New Column With Name OnHandDateALTER TABLE [dbo].[ShipmentComputedFields] ADD [OnHandDate] DATETIME NULL;');

-- Add New Column With Name PODDate
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PODDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fb7dddd8-285c-424b-80c4-29ad7366c189', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PODDate', 'Add Column', GETDATE(), '-- Add New Column With Name PODDateALTER TABLE [dbo].[ShipmentComputedFields] ADD [PODDate] DATETIME NULL;');


-- General Script From 202006291330_FillPaidDateOfInvoices.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
-- not used, run manually
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006291330_FillPaidDateOfInvoices.sxml', GETDATE(), '-- not used, run manually', DATEDIFF(MS,@StartTime,@EndTime), '9c7d4b76a39e0183af6d3eaef3c13a20', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

