-- Add New Column With Name TaxReportDate
ALTER TABLE [dbo].[TaxReportLines] ADD [TaxReportDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TaxReportLine.dxml', 'TaxReportLines', 'TaxReportDate', 'Add Column', GETDATE(), '-- Add New Column With Name TaxReportDate
ALTER TABLE [dbo].[TaxReportLines] ADD [TaxReportDate] DATETIME NULL;');

-- Add New Column With Name IsExternalLine
ALTER TABLE [dbo].[TaxReportLines] ADD [IsExternalLine] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TaxReportLine.dxml', 'TaxReportLines', 'IsExternalLine', 'Add Column', GETDATE(), '-- Add New Column With Name IsExternalLine
ALTER TABLE [dbo].[TaxReportLines] ADD [IsExternalLine] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name TotalInvoiceAmount
ALTER TABLE [dbo].[TaxReportLines] ADD [TotalInvoiceAmount] DECIMAL(16, 2) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TaxReportLine.dxml', 'TaxReportLines', 'TotalInvoiceAmount', 'Add Column', GETDATE(), '-- Add New Column With Name TotalInvoiceAmount
ALTER TABLE [dbo].[TaxReportLines] ADD [TotalInvoiceAmount] DECIMAL(16, 2) NULL;');

-- Add New Column With Name OriginalReference
ALTER TABLE [dbo].[TaxReportLines] ADD [OriginalReference] VARCHAR(20) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TaxReportLine.dxml', 'TaxReportLines', 'OriginalReference', 'Add Column', GETDATE(), '-- Add New Column With Name OriginalReference
ALTER TABLE [dbo].[TaxReportLines] ADD [OriginalReference] VARCHAR(20) NULL;');


-- Rename Column From ExcelOnly To DisablePreview
EXEC SP_RENAME 'dbo.Reports.ExcelOnly', 'DisablePreview', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Report.dxml', 'Reports', 'ExcelOnly', 'Rename Column', GETDATE(), '-- Rename Column From ExcelOnly To DisablePreview
EXEC SP_RENAME ''dbo.Reports.ExcelOnly'', ''DisablePreview'', ''COLUMN'';');


-- Rename Column From ExcelOnly To DisablePreview
EXEC SP_RENAME 'dbo.ReportExecutionLogs.ExcelOnly', 'DisablePreview', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ReportExecutionLog.dxml', 'ReportExecutionLogs', 'ExcelOnly', 'Rename Column', GETDATE(), '-- Rename Column From ExcelOnly To DisablePreview
EXEC SP_RENAME ''dbo.ReportExecutionLogs.ExcelOnly'', ''DisablePreview'', ''COLUMN'';');


-- Create Unique Constraint On Queries Table
EXEC('ALTER TABLE [dbo].[Queries] ADD CONSTRAINT [UQ_Queries_UniqueCode] UNIQUE([UniqueCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Query.dxml', 'Queries', 'UniqueCode', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On Queries Table
EXEC(''ALTER TABLE [dbo].[Queries] ADD CONSTRAINT [UQ_Queries_UniqueCode] UNIQUE([UniqueCode])'');');


-- Change Size From 15 To 40 For Column CreatedBy
ALTER TABLE [dbo].[TasksScheduler] ALTER COLUMN [CreatedBy] VARCHAR(40) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TasksScheduler.dxml', 'TasksScheduler', 'CreatedBy', 'Alter Column Size', GETDATE(), '-- Change Size From 15 To 40 For Column CreatedBy
ALTER TABLE [dbo].[TasksScheduler] ALTER COLUMN [CreatedBy] VARCHAR(40) NOT NULL;');

-- Change Size From 15 To 40 For Column UpdatedBy
ALTER TABLE [dbo].[TasksScheduler] ALTER COLUMN [UpdatedBy] VARCHAR(40) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('TasksScheduler.dxml', 'TasksScheduler', 'UpdatedBy', 'Alter Column Size', GETDATE(), '-- Change Size From 15 To 40 For Column UpdatedBy
ALTER TABLE [dbo].[TasksScheduler] ALTER COLUMN [UpdatedBy] VARCHAR(40) NOT NULL;');


-- Change Size From 20 To 30 For Column EntityReference
ALTER TABLE [dbo].[AccountingTransferLines] ALTER COLUMN [EntityReference] VARCHAR(30);

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('AccountingTransferLine.dxml', 'AccountingTransferLines', 'EntityReference', 'Alter Column Size', GETDATE(), '-- Change Size From 20 To 30 For Column EntityReference
ALTER TABLE [dbo].[AccountingTransferLines] ALTER COLUMN [EntityReference] VARCHAR(30);');


-- Drop Foreign Key Constraint For Column CostMeasurementId In Table QuoteCharges That Reference To Column Id In Table Measurements
EXEC('IF (OBJECT_ID(''[dbo].[FK_CostQuoteChargesMeasurement]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_CostQuoteChargesMeasurement] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QuoteCharge.dxml', 'QuoteCharges', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column CostMeasurementId In Table QuoteCharges That Reference To Column Id In Table Measurements
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_CostQuoteChargesMeasurement]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_CostQuoteChargesMeasurement] END'');');

-- Drop Index IX_FK_CostQuoteChargesMeasurement From Table QuoteCharges
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_CostQuoteChargesMeasurement'' AND object_id = OBJECT_ID(''[dbo].[QuoteCharges]'', ''U'')) BEGIN DROP INDEX [IX_FK_CostQuoteChargesMeasurement] ON [dbo].[QuoteCharges] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QuoteCharge.dxml', 'QuoteCharges', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_CostQuoteChargesMeasurement From Table QuoteCharges
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_CostQuoteChargesMeasurement'''' AND object_id = OBJECT_ID(''''[dbo].[QuoteCharges]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_CostQuoteChargesMeasurement] ON [dbo].[QuoteCharges] END'');');

-- Unset Nullable For Column CostMeasurementId
ALTER TABLE [dbo].[QuoteCharges] ALTER COLUMN [CostMeasurementId] VARCHAR(15) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QuoteCharge.dxml', 'QuoteCharges', 'CostMeasurementId', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CostMeasurementId
ALTER TABLE [dbo].[QuoteCharges] ALTER COLUMN [CostMeasurementId] VARCHAR(15) NOT NULL;');


-- Change Size From 20 To 25 For Column PickUpDeliveryNumber
ALTER TABLE [dbo].[ShipmentPickUpDeliveries] ALTER COLUMN [PickUpDeliveryNumber] VARCHAR(25) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ShipmentPickUpDelivery.dxml', 'ShipmentPickUpDeliveries', 'PickUpDeliveryNumber', 'Alter Column Size', GETDATE(), '-- Change Size From 20 To 25 For Column PickUpDeliveryNumber
ALTER TABLE [dbo].[ShipmentPickUpDeliveries] ALTER COLUMN [PickUpDeliveryNumber] VARCHAR(25) NOT NULL;');


-- Drop Foreign Key Constraint For Column RuleFieldId In Table AirlineMessagingRules That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.AirlineMessagingRules_dbo.ObjectFields_RuleFieldId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[AirlineMessagingRules] DROP CONSTRAINT [FK_dbo.AirlineMessagingRules_dbo.ObjectFields_RuleFieldId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('AirlineMessagingRule.dxml', 'AirlineMessagingRules', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column RuleFieldId In Table AirlineMessagingRules That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.AirlineMessagingRules_dbo.ObjectFields_RuleFieldId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[AirlineMessagingRules] DROP CONSTRAINT [FK_dbo.AirlineMessagingRules_dbo.ObjectFields_RuleFieldId] END'');');

-- Drop Index IX_RuleFieldId From Table AirlineMessagingRules
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_RuleFieldId'' AND object_id = OBJECT_ID(''[dbo].[AirlineMessagingRules]'', ''U'')) BEGIN DROP INDEX [IX_RuleFieldId] ON [dbo].[AirlineMessagingRules] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('AirlineMessagingRule.dxml', 'AirlineMessagingRules', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_RuleFieldId From Table AirlineMessagingRules
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_RuleFieldId'''' AND object_id = OBJECT_ID(''''[dbo].[AirlineMessagingRules]'''', ''''U'''')) BEGIN DROP INDEX [IX_RuleFieldId] ON [dbo].[AirlineMessagingRules] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table CustomerFieldsUpdateSettings That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.CustomerFieldsUpdateSettings_dbo.ObjectFields_ObjectFieldId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CustomerFieldsUpdateSettings] DROP CONSTRAINT [FK_dbo.CustomerFieldsUpdateSettings_dbo.ObjectFields_ObjectFieldId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('CustomerFieldsUpdateSetting.dxml', 'CustomerFieldsUpdateSettings', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table CustomerFieldsUpdateSettings That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.CustomerFieldsUpdateSettings_dbo.ObjectFields_ObjectFieldId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CustomerFieldsUpdateSettings] DROP CONSTRAINT [FK_dbo.CustomerFieldsUpdateSettings_dbo.ObjectFields_ObjectFieldId] END'');');

-- Drop Index IX_ObjectFieldId From Table CustomerFieldsUpdateSettings
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_ObjectFieldId'' AND object_id = OBJECT_ID(''[dbo].[CustomerFieldsUpdateSettings]'', ''U'')) BEGIN DROP INDEX [IX_ObjectFieldId] ON [dbo].[CustomerFieldsUpdateSettings] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('CustomerFieldsUpdateSetting.dxml', 'CustomerFieldsUpdateSettings', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_ObjectFieldId From Table CustomerFieldsUpdateSettings
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_ObjectFieldId'''' AND object_id = OBJECT_ID(''''[dbo].[CustomerFieldsUpdateSettings]'''', ''''U'''')) BEGIN DROP INDEX [IX_ObjectFieldId] ON [dbo].[CustomerFieldsUpdateSettings] END'');');


-- Drop Foreign Key Constraint For Column NameTextCodeId In Table Features That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_FeatureTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Features] DROP CONSTRAINT [FK_FeatureTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Feature.dxml', 'Features', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column NameTextCodeId In Table Features That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_FeatureTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Features] DROP CONSTRAINT [FK_FeatureTextCode] END'');');

-- Drop Index IX_FK_FeatureTextCode From Table Features
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_FeatureTextCode'' AND object_id = OBJECT_ID(''[dbo].[Features]'', ''U'')) BEGIN DROP INDEX [IX_FK_FeatureTextCode] ON [dbo].[Features] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Feature.dxml', 'Features', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_FeatureTextCode From Table Features
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_FeatureTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[Features]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_FeatureTextCode] ON [dbo].[Features] END'');');


-- Drop Foreign Key Constraint For Column FeatureId In Table PackageFeatures That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_PackageFeatureFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[PackageFeatures] DROP CONSTRAINT [FK_PackageFeatureFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('PackageFeature.dxml', 'PackageFeatures', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table PackageFeatures That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_PackageFeatureFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[PackageFeatures] DROP CONSTRAINT [FK_PackageFeatureFeature] END'');');

-- Drop Index IX_FK_PackageFeatureFeature From Table PackageFeatures
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_PackageFeatureFeature'' AND object_id = OBJECT_ID(''[dbo].[PackageFeatures]'', ''U'')) BEGIN DROP INDEX [IX_FK_PackageFeatureFeature] ON [dbo].[PackageFeatures] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('PackageFeature.dxml', 'PackageFeatures', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_PackageFeatureFeature From Table PackageFeatures
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_PackageFeatureFeature'''' AND object_id = OBJECT_ID(''''[dbo].[PackageFeatures]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_PackageFeatureFeature] ON [dbo].[PackageFeatures] END'');');


-- Drop Foreign Key Constraint For Column FeatureId In Table Reports That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.Reports_dbo.Features_FeatureId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Reports] DROP CONSTRAINT [FK_dbo.Reports_dbo.Features_FeatureId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Report.dxml', 'Reports', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table Reports That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.Reports_dbo.Features_FeatureId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Reports] DROP CONSTRAINT [FK_dbo.Reports_dbo.Features_FeatureId] END'');');

-- Drop Index IX_FeatureId From Table Reports
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FeatureId'' AND object_id = OBJECT_ID(''[dbo].[Reports]'', ''U'')) BEGIN DROP INDEX [IX_FeatureId] ON [dbo].[Reports] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Report.dxml', 'Reports', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FeatureId From Table Reports
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FeatureId'''' AND object_id = OBJECT_ID(''''[dbo].[Reports]'''', ''''U'''')) BEGIN DROP INDEX [IX_FeatureId] ON [dbo].[Reports] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table Restrictions That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_RestrictionObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Restrictions] DROP CONSTRAINT [FK_RestrictionObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Restriction.dxml', 'Restrictions', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table Restrictions That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_RestrictionObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Restrictions] DROP CONSTRAINT [FK_RestrictionObjectField] END'');');

-- Drop Index IX_FK_RestrictionObjectField From Table Restrictions
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_RestrictionObjectField'' AND object_id = OBJECT_ID(''[dbo].[Restrictions]'', ''U'')) BEGIN DROP INDEX [IX_FK_RestrictionObjectField] ON [dbo].[Restrictions] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Restriction.dxml', 'Restrictions', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_RestrictionObjectField From Table Restrictions
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_RestrictionObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[Restrictions]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_RestrictionObjectField] ON [dbo].[Restrictions] END'');');


-- Drop Foreign Key Constraint For Column FeatureId In Table RoleFeatures That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_RoleFeatureFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[RoleFeatures] DROP CONSTRAINT [FK_RoleFeatureFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('RoleFeature.dxml', 'RoleFeatures', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table RoleFeatures That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_RoleFeatureFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[RoleFeatures] DROP CONSTRAINT [FK_RoleFeatureFeature] END'');');

-- Drop Index IX_FK_RoleFeatureFeature From Table RoleFeatures
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_RoleFeatureFeature'' AND object_id = OBJECT_ID(''[dbo].[RoleFeatures]'', ''U'')) BEGIN DROP INDEX [IX_FK_RoleFeatureFeature] ON [dbo].[RoleFeatures] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('RoleFeature.dxml', 'RoleFeatures', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_RoleFeatureFeature From Table RoleFeatures
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_RoleFeatureFeature'''' AND object_id = OBJECT_ID(''''[dbo].[RoleFeatures]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_RoleFeatureFeature] ON [dbo].[RoleFeatures] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table AdvancedQueryFilters That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectFieldAdvancedQueryFilter]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[AdvancedQueryFilters] DROP CONSTRAINT [FK_ObjectFieldAdvancedQueryFilter] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('AdvancedQueryFilter.dxml', 'AdvancedQueryFilters', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table AdvancedQueryFilters That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectFieldAdvancedQueryFilter]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[AdvancedQueryFilters] DROP CONSTRAINT [FK_ObjectFieldAdvancedQueryFilter] END'');');

-- Drop Index IX_FK_ObjectFieldAdvancedQueryFilter From Table AdvancedQueryFilters
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectFieldAdvancedQueryFilter'' AND object_id = OBJECT_ID(''[dbo].[AdvancedQueryFilters]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectFieldAdvancedQueryFilter] ON [dbo].[AdvancedQueryFilters] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('AdvancedQueryFilter.dxml', 'AdvancedQueryFilters', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectFieldAdvancedQueryFilter From Table AdvancedQueryFilters
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectFieldAdvancedQueryFilter'''' AND object_id = OBJECT_ID(''''[dbo].[AdvancedQueryFilters]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectFieldAdvancedQueryFilter] ON [dbo].[AdvancedQueryFilters] END'');');


-- Drop Foreign Key Constraint For Column LabelTextCodeId In Table MenuButtons That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_TextCodeMenuButton]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenuButtons] DROP CONSTRAINT [FK_TextCodeMenuButton] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenuButton.dxml', 'MenuButtons', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column LabelTextCodeId In Table MenuButtons That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_TextCodeMenuButton]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenuButtons] DROP CONSTRAINT [FK_TextCodeMenuButton] END'');');

-- Drop Index IX_FK_TextCodeMenuButton From Table MenuButtons
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_TextCodeMenuButton'' AND object_id = OBJECT_ID(''[dbo].[MenuButtons]'', ''U'')) BEGIN DROP INDEX [IX_FK_TextCodeMenuButton] ON [dbo].[MenuButtons] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenuButton.dxml', 'MenuButtons', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_TextCodeMenuButton From Table MenuButtons
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_TextCodeMenuButton'''' AND object_id = OBJECT_ID(''''[dbo].[MenuButtons]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_TextCodeMenuButton] ON [dbo].[MenuButtons] END'');');

-- Drop Foreign Key Constraint For Column FeatureId In Table MenuButtons That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_MenuButtonFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenuButtons] DROP CONSTRAINT [FK_MenuButtonFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenuButton.dxml', 'MenuButtons', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table MenuButtons That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_MenuButtonFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenuButtons] DROP CONSTRAINT [FK_MenuButtonFeature] END'');');

-- Drop Index IX_FK_MenuButtonFeature From Table MenuButtons
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_MenuButtonFeature'' AND object_id = OBJECT_ID(''[dbo].[MenuButtons]'', ''U'')) BEGIN DROP INDEX [IX_FK_MenuButtonFeature] ON [dbo].[MenuButtons] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenuButton.dxml', 'MenuButtons', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_MenuButtonFeature From Table MenuButtons
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_MenuButtonFeature'''' AND object_id = OBJECT_ID(''''[dbo].[MenuButtons]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_MenuButtonFeature] ON [dbo].[MenuButtons] END'');');


-- Drop Foreign Key Constraint For Column FeatureId In Table MenusTables That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_MenusTableFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenusTables] DROP CONSTRAINT [FK_MenusTableFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenusTable.dxml', 'MenusTables', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table MenusTables That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_MenusTableFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[MenusTables] DROP CONSTRAINT [FK_MenusTableFeature] END'');');

-- Drop Index IX_FK_MenusTableFeature From Table MenusTables
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_MenusTableFeature'' AND object_id = OBJECT_ID(''[dbo].[MenusTables]'', ''U'')) BEGIN DROP INDEX [IX_FK_MenusTableFeature] ON [dbo].[MenusTables] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('MenusTable.dxml', 'MenusTables', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_MenusTableFeature From Table MenusTables
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_MenusTableFeature'''' AND object_id = OBJECT_ID(''''[dbo].[MenusTables]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_MenusTableFeature] ON [dbo].[MenusTables] END'');');


-- Drop Foreign Key Constraint For Column HelpTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_TextCodeObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_TextCodeObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column HelpTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_TextCodeObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_TextCodeObjectField] END'');');

-- Drop Index IX_FK_TextCodeObjectField From Table ObjectFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_TextCodeObjectField'' AND object_id = OBJECT_ID(''[dbo].[ObjectFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_TextCodeObjectField] ON [dbo].[ObjectFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_TextCodeObjectField From Table ObjectFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_TextCodeObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_TextCodeObjectField] ON [dbo].[ObjectFields] END'');');

-- Drop Foreign Key Constraint For Column FullNameTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_TextCodeObjectField1]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_TextCodeObjectField1] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FullNameTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_TextCodeObjectField1]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_TextCodeObjectField1] END'');');

-- Drop Index IX_FK_TextCodeObjectField1 From Table ObjectFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_TextCodeObjectField1'' AND object_id = OBJECT_ID(''[dbo].[ObjectFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_TextCodeObjectField1] ON [dbo].[ObjectFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_TextCodeObjectField1 From Table ObjectFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_TextCodeObjectField1'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_TextCodeObjectField1] ON [dbo].[ObjectFields] END'');');

-- Drop Foreign Key Constraint For Column ListTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_ListFieldLableObjectFieldTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_ListFieldLableObjectFieldTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ListTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ListFieldLableObjectFieldTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_ListFieldLableObjectFieldTextCode] END'');');

-- Drop Index IX_FK_ListFieldLableObjectFieldTextCode From Table ObjectFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ListFieldLableObjectFieldTextCode'' AND object_id = OBJECT_ID(''[dbo].[ObjectFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_ListFieldLableObjectFieldTextCode] ON [dbo].[ObjectFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ListFieldLableObjectFieldTextCode From Table ObjectFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ListFieldLableObjectFieldTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ListFieldLableObjectFieldTextCode] ON [dbo].[ObjectFields] END'');');

-- Drop Foreign Key Constraint For Column ShortNameTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectFieldTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_ObjectFieldTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ShortNameTextCodeId In Table ObjectFields That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectFieldTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [FK_ObjectFieldTextCode] END'');');

-- Drop Index IX_FK_ObjectFieldTextCode From Table ObjectFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectFieldTextCode'' AND object_id = OBJECT_ID(''[dbo].[ObjectFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectFieldTextCode] ON [dbo].[ObjectFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectField.dxml', 'ObjectFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectFieldTextCode From Table ObjectFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectFieldTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectFieldTextCode] ON [dbo].[ObjectFields] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectFieldModifications That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectFieldModificationObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFieldModifications] DROP CONSTRAINT [FK_ObjectFieldModificationObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectFieldModification.dxml', 'ObjectFieldModifications', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectFieldModifications That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectFieldModificationObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFieldModifications] DROP CONSTRAINT [FK_ObjectFieldModificationObjectField] END'');');

-- Drop Index IX_FK_ObjectFieldModificationObjectField From Table ObjectFieldModifications
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectFieldModificationObjectField'' AND object_id = OBJECT_ID(''[dbo].[ObjectFieldModifications]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectFieldModificationObjectField] ON [dbo].[ObjectFieldModifications] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectFieldModification.dxml', 'ObjectFieldModifications', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectFieldModificationObjectField From Table ObjectFieldModifications
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectFieldModificationObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFieldModifications]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectFieldModificationObjectField] ON [dbo].[ObjectFieldModifications] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectFieldValidations That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectFieldValidationObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFieldValidations] DROP CONSTRAINT [FK_ObjectFieldValidationObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectFieldValidation.dxml', 'ObjectFieldValidations', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectFieldValidations That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectFieldValidationObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectFieldValidations] DROP CONSTRAINT [FK_ObjectFieldValidationObjectField] END'');');

-- Drop Index IX_FK_ObjectFieldValidationObjectField From Table ObjectFieldValidations
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectFieldValidationObjectField'' AND object_id = OBJECT_ID(''[dbo].[ObjectFieldValidations]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectFieldValidationObjectField] ON [dbo].[ObjectFieldValidations] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectFieldValidation.dxml', 'ObjectFieldValidations', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectFieldValidationObjectField From Table ObjectFieldValidations
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectFieldValidationObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectFieldValidations]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectFieldValidationObjectField] ON [dbo].[ObjectFieldValidations] END'');');


-- Drop Foreign Key Constraint For Column DescriptionTextCodeId In Table ObjectTables That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_DescriptionObjectTableTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [FK_DescriptionObjectTableTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTable.dxml', 'ObjectTables', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column DescriptionTextCodeId In Table ObjectTables That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_DescriptionObjectTableTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [FK_DescriptionObjectTableTextCode] END'');');

-- Drop Index IX_FK_DescriptionObjectTableTextCode From Table ObjectTables
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_DescriptionObjectTableTextCode'' AND object_id = OBJECT_ID(''[dbo].[ObjectTables]'', ''U'')) BEGIN DROP INDEX [IX_FK_DescriptionObjectTableTextCode] ON [dbo].[ObjectTables] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTable.dxml', 'ObjectTables', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_DescriptionObjectTableTextCode From Table ObjectTables
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_DescriptionObjectTableTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTables]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_DescriptionObjectTableTextCode] ON [dbo].[ObjectTables] END'');');

-- Drop Foreign Key Constraint For Column NewButtonTextCodeId In Table ObjectTables That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.ObjectTables_dbo.TextCodes_NewButtonTextCodeId]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [FK_dbo.ObjectTables_dbo.TextCodes_NewButtonTextCodeId] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTable.dxml', 'ObjectTables', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column NewButtonTextCodeId In Table ObjectTables That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.ObjectTables_dbo.TextCodes_NewButtonTextCodeId]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [FK_dbo.ObjectTables_dbo.TextCodes_NewButtonTextCodeId] END'');');

-- Drop Index IX_NewButtonTextCodeId From Table ObjectTables
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_NewButtonTextCodeId'' AND object_id = OBJECT_ID(''[dbo].[ObjectTables]'', ''U'')) BEGIN DROP INDEX [IX_NewButtonTextCodeId] ON [dbo].[ObjectTables] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTable.dxml', 'ObjectTables', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_NewButtonTextCodeId From Table ObjectTables
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_NewButtonTextCodeId'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTables]'''', ''''U'''')) BEGIN DROP INDEX [IX_NewButtonTextCodeId] ON [dbo].[ObjectTables] END'');');


-- Drop Foreign Key Constraint For Column TriggerFieldId In Table ObjectTableRules That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectTableRuleObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableRules] DROP CONSTRAINT [FK_ObjectTableRuleObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableRule.dxml', 'ObjectTableRules', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column TriggerFieldId In Table ObjectTableRules That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectTableRuleObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableRules] DROP CONSTRAINT [FK_ObjectTableRuleObjectField] END'');');

-- Drop Index IX_FK_ObjectTableRuleObjectField From Table ObjectTableRules
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectTableRuleObjectField'' AND object_id = OBJECT_ID(''[dbo].[ObjectTableRules]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectTableRuleObjectField] ON [dbo].[ObjectTableRules] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableRule.dxml', 'ObjectTableRules', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectTableRuleObjectField From Table ObjectTableRules
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectTableRuleObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTableRules]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectTableRuleObjectField] ON [dbo].[ObjectTableRules] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectTableRuleFields That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectTableRuleFieldObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableRuleFields] DROP CONSTRAINT [FK_ObjectTableRuleFieldObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableRuleField.dxml', 'ObjectTableRuleFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ObjectTableRuleFields That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectTableRuleFieldObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableRuleFields] DROP CONSTRAINT [FK_ObjectTableRuleFieldObjectField] END'');');

-- Drop Index IX_FK_ObjectTableRuleFieldObjectField From Table ObjectTableRuleFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectTableRuleFieldObjectField'' AND object_id = OBJECT_ID(''[dbo].[ObjectTableRuleFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectTableRuleFieldObjectField] ON [dbo].[ObjectTableRuleFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableRuleField.dxml', 'ObjectTableRuleFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectTableRuleFieldObjectField From Table ObjectTableRuleFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectTableRuleFieldObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTableRuleFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectTableRuleFieldObjectField] ON [dbo].[ObjectTableRuleFields] END'');');


-- Drop Foreign Key Constraint For Column TabNameTextCodeId In Table ObjectTableTabs That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectTableTabTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableTabs] DROP CONSTRAINT [FK_ObjectTableTabTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableTab.dxml', 'ObjectTableTabs', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column TabNameTextCodeId In Table ObjectTableTabs That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectTableTabTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableTabs] DROP CONSTRAINT [FK_ObjectTableTabTextCode] END'');');

-- Drop Index IX_FK_ObjectTableTabTextCode From Table ObjectTableTabs
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectTableTabTextCode'' AND object_id = OBJECT_ID(''[dbo].[ObjectTableTabs]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectTableTabTextCode] ON [dbo].[ObjectTableTabs] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableTab.dxml', 'ObjectTableTabs', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectTableTabTextCode From Table ObjectTableTabs
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectTableTabTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTableTabs]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectTableTabTextCode] ON [dbo].[ObjectTableTabs] END'');');

-- Drop Foreign Key Constraint For Column FeatureId In Table ObjectTableTabs That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectTableTabFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableTabs] DROP CONSTRAINT [FK_ObjectTableTabFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableTab.dxml', 'ObjectTableTabs', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table ObjectTableTabs That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectTableTabFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ObjectTableTabs] DROP CONSTRAINT [FK_ObjectTableTabFeature] END'');');

-- Drop Index IX_FK_ObjectTableTabFeature From Table ObjectTableTabs
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectTableTabFeature'' AND object_id = OBJECT_ID(''[dbo].[ObjectTableTabs]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectTableTabFeature] ON [dbo].[ObjectTableTabs] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ObjectTableTab.dxml', 'ObjectTableTabs', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectTableTabFeature From Table ObjectTableTabs
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectTableTabFeature'''' AND object_id = OBJECT_ID(''''[dbo].[ObjectTableTabs]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectTableTabFeature] ON [dbo].[ObjectTableTabs] END'');');


-- Drop Foreign Key Constraint For Column NameTextCodeId In Table Queries That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_QueryTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Queries] DROP CONSTRAINT [FK_QueryTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Query.dxml', 'Queries', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column NameTextCodeId In Table Queries That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_QueryTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Queries] DROP CONSTRAINT [FK_QueryTextCode] END'');');

-- Drop Index IX_FK_QueryTextCode From Table Queries
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_QueryTextCode'' AND object_id = OBJECT_ID(''[dbo].[Queries]'', ''U'')) BEGIN DROP INDEX [IX_FK_QueryTextCode] ON [dbo].[Queries] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Query.dxml', 'Queries', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_QueryTextCode From Table Queries
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_QueryTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[Queries]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_QueryTextCode] ON [dbo].[Queries] END'');');

-- Drop Foreign Key Constraint For Column FeatureId In Table Queries That Reference To Column Id In Table Features
EXEC('IF (OBJECT_ID(''[dbo].[FK_QueryFeature]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Queries] DROP CONSTRAINT [FK_QueryFeature] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Query.dxml', 'Queries', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column FeatureId In Table Queries That Reference To Column Id In Table Features
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_QueryFeature]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Queries] DROP CONSTRAINT [FK_QueryFeature] END'');');

-- Drop Index IX_FK_QueryFeature From Table Queries
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_QueryFeature'' AND object_id = OBJECT_ID(''[dbo].[Queries]'', ''U'')) BEGIN DROP INDEX [IX_FK_QueryFeature] ON [dbo].[Queries] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Query.dxml', 'Queries', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_QueryFeature From Table Queries
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_QueryFeature'''' AND object_id = OBJECT_ID(''''[dbo].[Queries]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_QueryFeature] ON [dbo].[Queries] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table QueryColumns That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_QueryColumnObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QueryColumns] DROP CONSTRAINT [FK_QueryColumnObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QueryColumn.dxml', 'QueryColumns', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table QueryColumns That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_QueryColumnObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[QueryColumns] DROP CONSTRAINT [FK_QueryColumnObjectField] END'');');

-- Drop Index IX_FK_QueryColumnObjectField From Table QueryColumns
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_QueryColumnObjectField'' AND object_id = OBJECT_ID(''[dbo].[QueryColumns]'', ''U'')) BEGIN DROP INDEX [IX_FK_QueryColumnObjectField] ON [dbo].[QueryColumns] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QueryColumn.dxml', 'QueryColumns', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_QueryColumnObjectField From Table QueryColumns
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_QueryColumnObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[QueryColumns]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_QueryColumnObjectField] ON [dbo].[QueryColumns] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table RuleConditionFields That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_RuleConditionFieldsObjectField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[RuleConditionFields] DROP CONSTRAINT [FK_RuleConditionFieldsObjectField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('RuleConditionField.dxml', 'RuleConditionFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table RuleConditionFields That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_RuleConditionFieldsObjectField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[RuleConditionFields] DROP CONSTRAINT [FK_RuleConditionFieldsObjectField] END'');');

-- Drop Index IX_FK_RuleConditionFieldsObjectField From Table RuleConditionFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_RuleConditionFieldsObjectField'' AND object_id = OBJECT_ID(''[dbo].[RuleConditionFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_RuleConditionFieldsObjectField] ON [dbo].[RuleConditionFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('RuleConditionField.dxml', 'RuleConditionFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_RuleConditionFieldsObjectField From Table RuleConditionFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_RuleConditionFieldsObjectField'''' AND object_id = OBJECT_ID(''''[dbo].[RuleConditionFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_RuleConditionFieldsObjectField] ON [dbo].[RuleConditionFields] END'');');


-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ScreenFields That Reference To Column Id In Table ObjectFields
EXEC('IF (OBJECT_ID(''[dbo].[FK_ObjectFieldScreenField]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ScreenFields] DROP CONSTRAINT [FK_ObjectFieldScreenField] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ScreenField.dxml', 'ScreenFields', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ObjectFieldId In Table ScreenFields That Reference To Column Id In Table ObjectFields
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_ObjectFieldScreenField]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[ScreenFields] DROP CONSTRAINT [FK_ObjectFieldScreenField] END'');');

-- Drop Index IX_FK_ObjectFieldScreenField From Table ScreenFields
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_ObjectFieldScreenField'' AND object_id = OBJECT_ID(''[dbo].[ScreenFields]'', ''U'')) BEGIN DROP INDEX [IX_FK_ObjectFieldScreenField] ON [dbo].[ScreenFields] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ScreenField.dxml', 'ScreenFields', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_ObjectFieldScreenField From Table ScreenFields
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_ObjectFieldScreenField'''' AND object_id = OBJECT_ID(''''[dbo].[ScreenFields]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_ObjectFieldScreenField] ON [dbo].[ScreenFields] END'');');


-- Drop Foreign Key Constraint For Column ShortTextCode In Table Tips That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_TipTextCode]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Tips] DROP CONSTRAINT [FK_TipTextCode] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Tip.dxml', 'Tips', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column ShortTextCode In Table Tips That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_TipTextCode]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Tips] DROP CONSTRAINT [FK_TipTextCode] END'');');

-- Drop Index IX_FK_TipTextCode From Table Tips
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_TipTextCode'' AND object_id = OBJECT_ID(''[dbo].[Tips]'', ''U'')) BEGIN DROP INDEX [IX_FK_TipTextCode] ON [dbo].[Tips] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Tip.dxml', 'Tips', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_TipTextCode From Table Tips
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_TipTextCode'''' AND object_id = OBJECT_ID(''''[dbo].[Tips]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_TipTextCode] ON [dbo].[Tips] END'');');


-- Drop Foreign Key Constraint For Column TextCodeId In Table Translations That Reference To Column Id In Table TextCodes
EXEC('IF (OBJECT_ID(''[dbo].[FK_TextCodeTranslation]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Translations] DROP CONSTRAINT [FK_TextCodeTranslation] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Translation.dxml', 'Translations', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column TextCodeId In Table Translations That Reference To Column Id In Table TextCodes
EXEC(''IF (OBJECT_ID(''''[dbo].[FK_TextCodeTranslation]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[Translations] DROP CONSTRAINT [FK_TextCodeTranslation] END'');');

-- Drop Index IX_FK_TextCodeTranslation From Table Translations
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_FK_TextCodeTranslation'' AND object_id = OBJECT_ID(''[dbo].[Translations]'', ''U'')) BEGIN DROP INDEX [IX_FK_TextCodeTranslation] ON [dbo].[Translations] END');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('Translation.dxml', 'Translations', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_FK_TextCodeTranslation From Table Translations
EXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_FK_TextCodeTranslation'''' AND object_id = OBJECT_ID(''''[dbo].[Translations]'''', ''''U'''')) BEGIN DROP INDEX [IX_FK_TextCodeTranslation] ON [dbo].[Translations] END'');');


-- Add Foreign Key Constraint For Column CostMeasurementId In Table QuoteCharges As Reference To Column Id In Table Measurements
EXEC('ALTER TABLE [dbo].[QuoteCharges] ADD CONSTRAINT [FK_QuoteCharges_Measurements_CostMeasurementId] FOREIGN KEY([CostMeasurementId]) REFERENCES [dbo].[Measurements]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('QuoteCharge.dxml', 'QuoteCharges', 'CostMeasurementId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CostMeasurementId In Table QuoteCharges As Reference To Column Id In Table Measurements
EXEC(''ALTER TABLE [dbo].[QuoteCharges] ADD CONSTRAINT [FK_QuoteCharges_Measurements_CostMeasurementId] FOREIGN KEY([CostMeasurementId]) REFERENCES [dbo].[Measurements]([Id])'');');


-- DataView Script From CustomersDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[CustomersDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[CustomersDataView] END');
EXEC('CREATE VIEW [dbo].[CustomersDataView]
AS
SELECT
dbo.Customers.Id, dbo.Customers.Tenant,dbo.cards.EnglishName,dbo.cards.ZipCode,dbo.cards.Address1,dbo.cards.Address2,dbo.cards.Phone,dbo.Cards.LocalName,dbo.Cards.ReceivablesAccountingCard, dbo.Cards.PayablesAccountingCard, dbo.Cards.ExternalId2,dbo.Cards.InActive,dbo.Cards.EnableConsolidationInvoices, dbo.Cards.ExternalAccountingBusinessArea, dbo.Cards.SATPaymentMethodCode,dbo.Cards.SATForeignRFC,dbo.Cards.MetodoPagoCode,dbo.Cards.UsoCFDICode
,dbo.Cards.Notes,dbo.Customers.BillToId,dbo.Cards.Website,dbo.Customers.SalesmanUserId,dbo.Cards.PaymentTermId,dbo.Cards.CreateDate,dbo.Cards.UpdateDate,dbo.Cards.CreatedByUserId,dbo.Cards.UpdatedByUserId
,dbo.Cards.VatNumber,dbo.Cards.SearchFields,dbo.PaymentTerms.EnglishName as PaymentTermEnglishName,dbo.Cards.InvoiceCurrencyId,dbo.Customers.LastShipmentDate,dbo.Customers.StartWorkingDate,dbo.Customers.StartWorkingManuallySet
,AccountManagerUserContacts.EnglishName as AccountManagerUserEnglishName,SalesmanUserContacts.EnglishName as SalesmanUserEnglishName,CollectorContacts.EnglishName as CollectorName,ClassifierContacts.EnglishName as ClassifierName
,dbo.Cards.CityName ,dbo.Cards.VatTypeId,BillToCards.EnglishName as BillToName,dbo.Customers.Field1,dbo.Customers.Field2,dbo.Customers.Field3,dbo.Customers.Field4,dbo.Customers.Field5,dbo.Customers.Field6,dbo.Customers.Field7,dbo.Customers.Field8
,dbo.Customers.Field9,dbo.Customers.Field10,dbo.Ranks.Code as RankCode,dbo.Ranks.Name as RankName
,dbo.Cards.SharedLogisticsInvitationStatusCode, dbo.Customers.ActivityWatch
,dbo.SharedLogisticsInvitationStatus.Name as SharedLogisticsInvitationStatusName
,dbo.cards.LastLoginDate,dbo.cards.InvitationDate,dbo.Industries.Name as IndustryName
,dbo.customers.LeadDescription,dbo.customers.ClassifierId ,dbo.Cards.IsCustomer,dbo.Customers.CollectorId,dbo.customers.FreelancerId,FreelancerContacts.EnglishName as FreelancerName,dbo.customers.ForwarderId,ForwarderCards.EnglishName as ForwarderName
,dbo.Customers.CustomsAgentId,CustomsAgentCards.EnglishName as CustomsAgentName,dbo.customers.MediatorId,MediatorCards.EnglishName as MediatorName,dbo.customers.BeforeDeactiveStatusCode,dbo.Customers.ReadyForActivationDate,CreatedByUserContacts.EnglishName as UpdatedByUserName
,PrimaryContacts.EnglishName as PrimaryContactName
,PrimaryContacts.Email as PrimaryContactEmail
,dbo.cards.PrimaryContactId,dbo.customers.RegionId,dbo.regions.Name as RegionName, dbo.customers.CustomerStatusCode,dbo.CustomerStatus.Name as CustomerStatusName
,dbo.Customers.RankId, dbo.Customers.LeadSourceId, LeadSources.Name as LeadSourceName, dbo.Customers.IndustryId
,dbo.cards.CountryId ,dbo.cards.CountryCode, dbo.cards.CountryName
,dbo.customers.AccountManagerUserId,CreatedByUserContacts.EnglishName as CreatedByUserName,dbo.cards.code,customers.FirstShipmentDate,dbo.cards.IsActiveForMobile as  IsActiveForMobile
,SalesmanUsers.BusinessUnitId as SalesmanBusinessUnitId,dbo.customers.FirstInvoiceDate,customers.LastOpportunityDate,customers.LastOpportunitySubject,customers.LastOpportunityStatus, customers.LastMeetingDate,customers.LastCallDate,customers.LastQuoteDate,customers.LastInteractionDate
,InvoiceCurrency.Code as InvoiceCurrencyCode
,dbo.Customers.KnownConsignor, dbo.Customers.KCExpirationDate,
dbo.Cards.PartnerTypeId, dbo.customers.CustomerSizeId, CustomerSizes.Name as CustomerSizeName, dbo.Cards.SupportNotes,
dbo.Customers.IsCreditLimitEnabled, dbo.Customers.CreditLimitAmount, dbo.Customers.CreditLimitOpenBalance, dbo.Customers.CreditLimitWarningPercentage,
dbo.Customers.BlockNewInvoiceCreation, dbo.Customers.BlockNewShipmentCreation,dbo.Customers.CompetitorFields,
dbo.Customers.ActivatedByUserId, dbo.Customers.ActivationRequestedByUserId, dbo.Customers.SetAsInactiveByUserId,
ActivatedByUserContacts.EnglishName as ActivatedByUserName, SetAsInactiveByUserContacts.EnglishName as SetAsInactiveByName,
ActivationRequestedByUserContacts.EnglishName as ActivationRequestedByUserName, dbo.Customers.ActivationDate, dbo.Customers.InactiveDate, dbo.Customers.ActivationRequestDate,dbo.cards.CreatedByPartner, dbo.cards.StateName
FROM            dbo.Customers Inner join
dbo.Cards ON  dbo.Customers.Id = dbo.Cards.Id LEFT OUTER JOIN
dbo.PaymentTerms ON dbo.Cards.PaymentTermId = dbo.PaymentTerms.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Customers.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContacts ON dbo.Customers.SalesmanUserId = SalesmanUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS CollectorContacts ON dbo.Customers.CollectorId = CollectorContacts.Id LEFT OUTER JOIN
dbo.Contacts AS ClassifierContacts ON dbo.Customers.ClassifierId = ClassifierContacts.Id LEFT OUTER JOIN
dbo.Cards AS BillToCards ON dbo.Customers.BillToId = BillToCards.Id LEFT OUTER JOIN
dbo.Ranks  ON dbo.Customers.RankId = dbo.Ranks.Id LEFT OUTER JOIN
dbo.SharedLogisticsInvitationStatus ON dbo.Cards.SharedLogisticsInvitationStatusCode = dbo.SharedLogisticsInvitationStatus.Code LEFT OUTER JOIN
dbo.Industries ON dbo.Customers.IndustryId = dbo.Industries.Id LEFT OUTER JOIN
dbo.Contacts AS FreelancerContacts ON dbo.Customers.FreelancerId = FreelancerContacts.Id LEFT OUTER JOIN
dbo.Cards AS ForwarderCards ON dbo.Customers.ForwarderId = ForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomsAgentCards ON dbo.Customers.CustomsAgentId = CustomsAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS MediatorCards ON dbo.Customers.MediatorId = MediatorCards.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContacts ON dbo.Cards.CreatedByUserId = CreatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS UpdatedByUserContacts ON dbo.Cards.UpdatedByUserId = UpdatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS PrimaryContacts ON dbo.Cards.PrimaryContactId = PrimaryContacts.Id LEFT OUTER JOIN
dbo.Regions ON dbo.Customers.RegionId = dbo.Regions.Id LEFT OUTER JOIN
dbo.CustomerStatus ON dbo.Customers.CustomerStatusCode = dbo.CustomerStatus.Code LEFT OUTER JOIN
dbo.Users as SalesmanUsers on dbo.customers.SalesmanUserId = SalesmanUsers.Id LEFT OUTER JOIN
dbo.Countries as MainAddressCountries on dbo.Cards.CountryId = MainAddressCountries.Id LEFT OUTER JOIN
dbo.CustomerSizes ON dbo.Customers.CustomerSizeId = CustomerSizes.Id LEFT OUTER JOIN
dbo.LeadSources ON dbo.Customers.LeadSourceId = LeadSources.Id LEFT OUTER JOIN
dbo.Currencies as InvoiceCurrency on dbo.Cards.InvoiceCurrencyId = InvoiceCurrency.Id LEFT OUTER JOIN
dbo.Contacts AS ActivatedByUserContacts ON dbo.Customers.ActivatedByUserId = ActivatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SetAsInactiveByUserContacts ON dbo.Customers.SetAsInactiveByUserId = SetAsInactiveByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS ActivationRequestedByUserContacts ON dbo.Customers.ActivationRequestedByUserId = ActivationRequestedByUserContacts.Id
where dbo.Cards.PartnerTypeId <> ''AC''');


-- DataView Script From AgingReportInvoiceDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[AgingReportInvoiceDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[AgingReportInvoiceDataView] END');
EXEC('CREATE VIEW [dbo].[AgingReportInvoiceDataView]
AS
SELECT        Id, Tenant, InvoiceNumber, GETDATE() AS CurrentDate, DueDate, CONVERT(varchar(40), CASE WHEN DueDate < GETDATE() THEN (CASE WHEN (((GETDATE())
- DueDate) < 30) THEN ''1-30'' ELSE (CASE WHEN (((GETDATE()) - DueDate) < 60) THEN ''31-60'' ELSE (CASE WHEN (((GETDATE()) - DueDate) < 90)
THEN ''61-90'' ELSE (CASE WHEN (((GETDATE()) - DueDate) > 90) THEN ''+90'' ELSE '''' END) END) END) END) ELSE ''Current'' END) AS DateRange,
AmountDueInLocalCurrency, AmountDueInProfitCurrency, CONVERT(int, CASE WHEN DueDate < GETDATE() THEN (CASE WHEN (((GETDATE()) - DueDate) < 30)
THEN 4 ELSE (CASE WHEN (((GETDATE()) - DueDate) < 60) THEN 3 ELSE (CASE WHEN (((GETDATE()) - DueDate) < 90) THEN 2 ELSE (CASE WHEN (((GETDATE())
- DueDate) > 90) THEN 1 ELSE '''' END) END) END) END) ELSE 5 END) AS IndexOrder, StatusCode, BillToId,BranchId
FROM            dbo.ARInvoices
where IsClosed=0 and IsAutoCredit=0 and IsCancelled=0');


-- DataView Script From APInvoiceAgingReportDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[APAgingReportDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[APAgingReportDataView] END');
EXEC('CREATE VIEW [dbo].[APAgingReportDataView]
AS
SELECT        Id, Tenant, InvoiceNumber, GETDATE() AS CurrentDate, DueDate, CONVERT(varchar(40), CASE WHEN DueDate < GETDATE() THEN (CASE WHEN (((GETDATE())
- DueDate) < 30) THEN ''1-30'' ELSE (CASE WHEN (((GETDATE()) - DueDate) < 60) THEN ''31-60'' ELSE (CASE WHEN (((GETDATE()) - DueDate) < 90)
THEN ''61-90'' ELSE (CASE WHEN (((GETDATE()) - DueDate) > 90) THEN ''+90'' ELSE '''' END) END) END) END) ELSE ''Current'' END) AS DateRange, AmountDueInLocalCurrency, AmountDueInProfitCurrency ,
CONVERT(int, CASE WHEN DueDate < GETDATE() THEN (CASE WHEN (((GETDATE()) - DueDate) < 30) THEN 4 ELSE (CASE WHEN (((GETDATE()) - DueDate) < 60)
THEN 3 ELSE (CASE WHEN (((GETDATE()) - DueDate) < 90) THEN 2 ELSE (CASE WHEN (((GETDATE()) - DueDate) > 90) THEN 1 ELSE '''' END) END) END) END)
ELSE 5 END) AS IndexOrder, StatusCode,BranchId
FROM            dbo.APInvoices
where IsClosed=0');


-- DataView Script From QuoteFollowUpDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[QuoteFollowUpDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[QuoteFollowUpDataView] END');
EXEC('CREATE VIEW [dbo].[QuoteFollowUpDataView]
AS
SELECT        dbo.Quotes.Id, dbo.Quotes.Tenant, dbo.Quotes.QuoteNumber, dbo.Quotes.ShipperReference1, dbo.Quotes.ShipperReference2, dbo.Quotes.ConsigneeReference1,
dbo.Quotes.ConsigneeReference2, dbo.Quotes.OpenDate, dbo.Quotes.Notes, dbo.Quotes.DescriptionOfGoods, dbo.Quotes.IsClosed, dbo.Quotes.ChargeableWeight,
dbo.Quotes.GrossWeight, dbo.Quotes.GrossWeightInKG, dbo.Quotes.GrossWeightPerTon, dbo.Quotes.LastModified, dbo.Quotes.Field1, dbo.Quotes.Field2, dbo.Quotes.Field3, dbo.Quotes.Field4, dbo.Quotes.Field5,
dbo.Quotes.Field6, dbo.Quotes.Field7, dbo.Quotes.Field8, dbo.Quotes.Field9, dbo.Quotes.Field10, dbo.Quotes.DimensionsUnitCode,
dbo.Quotes.GrossWeightUnitCode, dbo.Quotes.Volume, dbo.Quotes.NumberOfContainers, dbo.Quotes.NumberOfPackages, dbo.Quotes.Ratio,
dbo.Quotes.VolumeUnitCode, dbo.Quotes.ShipmentTypeId, dbo.Quotes.ShipperId, dbo.Quotes.ConsigneeId,
dbo.Quotes.BusinessUnitId, dbo.Quotes.QuoteClosingReasonCode, dbo.Quotes.ValueOfGoods,
dbo.Quotes.ShipperContactId, dbo.Quotes.ConsigneeContactId, dbo.Quotes.FromPortId, dbo.Quotes.ToPortId, dbo.Quotes.ProductCode,
dbo.Quotes.IncotermId, dbo.Quotes.SalesmanUserId, dbo.Quotes.CreatedByUserId, dbo.Quotes.DirectionId, dbo.Quotes.TransportModeId, dbo.Quotes.IsDangerous,
dbo.Quotes.ExpirationDays, dbo.Quotes.ExpirationDate, dbo.Quotes.VolumetricWeight, dbo.Quotes.StageId, dbo.Quotes.StageDueDate, dbo.Quotes.BranchId, dbo.Quotes.DepartmentId,
dbo.Quotes.PackageType1Id, dbo.Quotes.PackageType2Id, dbo.Quotes.PackageType3Id, dbo.Quotes.PackageType4Id, dbo.Quotes.PackageType5Id,
dbo.Quotes.PackageType1Quantity, dbo.Quotes.PackageType3Quantity, dbo.Quotes.PackageType2Quantity, dbo.Quotes.PackageType4Quantity,
dbo.Quotes.PackageType5Quantity, dbo.Quotes.IsByKG, dbo.Quotes.IsByContainer, dbo.Quotes.QuoteTypeCode, dbo.Quotes.EstimateProfit,
dbo.Quotes.EstimateProfitEdited, dbo.Quotes.MinimumFreightCost, dbo.Quotes.MinimumFreightSale, dbo.Quotes.MainCarriageCarrierId,
dbo.Quotes.IsFreightBySteps, dbo.Quotes.IsCancelled, dbo.Quotes.CustomerId,
dbo.Quotes.CustomerContactId, dbo.Quotes.CustomerReference1, dbo.Quotes.CustomerReference2, dbo.Quotes.QuoteCustomerTypeCode, dbo.Quotes.CustomerName,
dbo.Quotes.SaleCurrencyId, dbo.Quotes.ExchangeRate, dbo.Quotes.ShipperName, dbo.Quotes.Subject, dbo.Quotes.IsSubjectEdited,
dbo.Quotes.LastActivityDate, dbo.Quotes.LastActivitySubject, dbo.Quotes.LastActivityTypeCode,
dbo.Quotes.NextActivityDate, dbo.Quotes.NextActivitySubject, dbo.Quotes.NextActivityTypeCode,
dbo.Quotes.UpdateDate, dbo.Quotes.IsAutomaticallyClosed, dbo.Quotes.AutomaticallyCloseDate, dbo.Quotes.AutomaticallyCloseDays,
dbo.Quotes.ConsigneeName, dbo.Quotes.PickUpAddress, dbo.Quotes.DeliveryAddress, dbo.Quotes.RatingCode, dbo.Quotes.OpportunityId,
dbo.Quotes.IsFixedPrice, dbo.Quotes.SearchFields, dbo.Quotes.ChargeableWeightUnitCode, dbo.Quotes.NumberOfFollowUps,
dbo.Quotes.ConcurrencyGUID, dbo.Quotes.FromPartnerId, dbo.Quotes.ToPartnerId, dbo.Quotes.FromPartnerAddressId, dbo.Quotes.ToPartnerAddressId,
dbo.FollowUps.Id AS FollowUpId, dbo.FollowUps.Date AS FollowUpDate, dbo.FollowUps.Notes AS FollowUpNotes,
dbo.FollowUps.OwnerUserId AS FollowUpOwnerUserId, ShipperCards.EnglishName AS Shipper, ConsigneeCards.EnglishName AS Consignee,
FollowUpTypes.FollowUpEnglishName AS FollowUpType, FromPorts.Code AS FromPortCode, FromPorts.EnglishName AS FromPortName,
FromPortCountries.EnglishName AS FromPortCountry, ToPorts.Code AS ToPortCode, ToPorts.EnglishName AS ToPortName,
ToPortCountries.EnglishName AS ToPortCountry, dbo.Stages.Name AS StageName, CreateByContacts.EnglishName AS CreatedByUser,
OwnerContacts.EnglishName AS FollowUpOwner, dbo.QuoteTypes.Name AS QuoteTypeName, FollowUpTypes.Id AS FollowUpTypeId,
OwnerContacts.Id AS FollowUpOwnerId, MainCarriageCarriers.EnglishName AS MainCarriageCarrierName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.BusinessUnits.Name AS BusinessUnitName, dbo.QuoteClosingReasons.Name AS QuoteClosingReasonName,
dbo.Incoterms.Code as IncotermCode
FROM            dbo.Quotes INNER JOIN
dbo.FollowUps ON dbo.Quotes.Id = dbo.FollowUps.QuoteId LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Quotes.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Quotes.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.EventTypes AS FollowUpTypes ON dbo.FollowUps.EventTypeId = FollowUpTypes.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Quotes.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Quotes.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Stages ON dbo.Quotes.StageId = dbo.Stages.Id LEFT OUTER JOIN
dbo.Users AS CreateByUsers ON dbo.Quotes.CreatedByUserId = CreateByUsers.Id LEFT OUTER JOIN
dbo.Users AS OwnerUsers ON dbo.FollowUps.OwnerUserId = OwnerUsers.Id LEFT OUTER JOIN
dbo.Contacts AS CreateByContacts ON CreateByUsers.Id = CreateByContacts.Id LEFT OUTER JOIN
dbo.Contacts AS OwnerContacts ON OwnerUsers.Id = OwnerContacts.Id LEFT OUTER JOIN
dbo.QuoteTypes ON dbo.Quotes.QuoteTypeCode = dbo.QuoteTypes.Code LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarriers ON dbo.Quotes.MainCarriageCarrierId = MainCarriageCarriers.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Quotes.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.BusinessUnits ON dbo.Quotes.BusinessUnitId = dbo.BusinessUnits.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Quotes.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.QuoteClosingReasons ON dbo.Quotes.QuoteClosingReasonCode = dbo.QuoteClosingReasons.Code');


-- DataView Script From MessagingStockDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[MessagingStockDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[MessagingStockDataView] END');
EXEC('CREATE VIEW [dbo].[MessagingStockDataView]
AS
SELECT
dbo.MessagingStocks.Id,
dbo.MessagingStocks.TenantNumber,
dbo.MessagingStocks.StartDate,
dbo.MessagingStocks.EndDate,
dbo.MessagingStocks.Amount,
dbo.MessagingStocks.Remaining,
dbo.MessagingStocks.IsCancelled,
dbo.MessagingStocks.Notes,
dbo.MessagingStocks.CreateDate,
dbo.MessagingStocks.UpdateDate,
dbo.MessagingStocks.CreatedByUserId,
dbo.MessagingStocks.UpdatedByUserId,
dbo.MessagingStocks.SearchFields,
dbo.MessagingStocks.TotalPrice,
dbo.MessagingStocks.StockType,
dbo.Tenants.Company as TenantName
FROM         dbo.MessagingStocks Inner join
dbo.Tenants ON  dbo.MessagingStocks.TenantNumber = dbo.Tenants.Id');


-- DataView Script From ShipmentDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentDataView] END');
EXEC('CREATE VIEW [dbo].[ShipmentDataView]
AS
SELECT        dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber,dbo.Shipments.DeclarationNumber,dbo.Shipments.IncludesCustoms,dbo.Shipments.CustomsClearanceDate,dbo.Shipments.DeclarationDate, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,dbo.Shipments.ValueOfGoods,
dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId, dbo.shipments.SLAC,
dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId, dbo.Shipments.FirstARInvoiceApprovalDate,
dbo.Shipments.LastFinalDestination, [dbo].[Shipments].[From], [dbo].[Shipments].[To], dbo.Shipments.Origin, dbo.Shipments.FirstPickupETA, dbo.Shipments.FirstPickupETD,
dbo.Shipments.ExceptionDescription,dbo.Shipments.ValueOfGoodsCurrencyId,dbo.Shipments.ExceptionDate, dbo.Shipments.HasException, dbo.Shipments.IsManifestSentToAgent, dbo.Shipments.ManifestLastSharingDate,
dbo.Shipments.AgentSharedManifestRef , dbo.Shipments.ExceptionResolvedDescription,dbo.Shipments.LastExceptionDescription, dbo.Shipments.OperationalDate, dbo.ShipmentMasterDatas.CutoffDate,
dbo.Shipments.ComputedStatusId, dbo.Shipments.ComputedStatusDate, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
dbo.Shipments.CustomConnectToShipment, dbo.Shipments.ForeignPartnerCountryCode, dbo.Shipments.IsNewARInvoiceBlocked,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId, dbo.ShipmentMasterDatas.Transshipment3CarrierId,
dbo.ShipmentMasterDatas.Transshipment2CarrierId, dbo.ShipmentMasterDatas.Transshipment1CarrierId, dbo.ShipmentMasterDatas.MainCarriageCarrierId,
dbo.ShipmentMasterDatas.MainCarriageIsFromStack, dbo.ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dbo.ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment3VesselId, dbo.ShipmentMasterDatas.Transshipment2VesselId, dbo.ShipmentMasterDatas.Transshipment1VesselId,
dbo.ShipmentMasterDatas.MainCarriageVesselId, dbo.ShipmentMasterDatas.BookingConfirmedBy, dbo.ShipmentMasterDatas.BookingConfirmationNotes,
dbo.ShipmentMasterDatas.BookingConfirmationNumber, dbo.ShipmentMasterDatas.MAWBOBLDate, dbo.ShipmentMasterDatas.Transshipment3CarrierNumber,
dbo.ShipmentMasterDatas.Transshipment3ETA, dbo.ShipmentMasterDatas.Transshipment3ETD, dbo.ShipmentMasterDatas.Transshipment3ATA,
dbo.ShipmentMasterDatas.Transshipment3ATD, dbo.ShipmentMasterDatas.Transshipment3ToPortId, dbo.ShipmentMasterDatas.Transshipment3FromPortId,
dbo.ShipmentMasterDatas.Transshipment2CarrierNumber, dbo.ShipmentMasterDatas.Transshipment2ETA, dbo.ShipmentMasterDatas.Transshipment2ETD,
dbo.ShipmentMasterDatas.Transshipment2ATA, dbo.ShipmentMasterDatas.Transshipment2ATD, dbo.ShipmentMasterDatas.Transshipment2ToPortId,
dbo.ShipmentMasterDatas.Transshipment2FromPortId, dbo.ShipmentMasterDatas.Transshipment1CarrierNumber, dbo.ShipmentMasterDatas.Transshipment1ETA,
dbo.ShipmentMasterDatas.Transshipment1ETD, dbo.ShipmentMasterDatas.Transshipment1ATA, dbo.ShipmentMasterDatas.Transshipment1ATD,
dbo.ShipmentMasterDatas.Transshipment1ToPortId, dbo.ShipmentMasterDatas.Transshipment1FromPortId, dbo.ShipmentMasterDatas.Master,
dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA, dbo.ShipmentMasterDatas.TrailerNumber,
dbo.ShipmentMasterDatas.MainCarriageFromAddressId, dbo.ShipmentMasterDatas.MainCarriageToAddressId,
dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2,
dbo.ShipmentMasterDatas.InterlineId, dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix,
dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails, dbo.Shipments.RegistryDate, dbo.Shipments.IsAssembly, dbo.Shipments.FirstOperationalCloseDate,
dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.FirstAccountingCloseDate,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.EstimateProfitInProfitCurrency, dbo.Shipments.ProfitCurrencyId, dbo.Shipments.SCI,
dbo.Shipments.AWBHandlingInformation, dbo.Shipments.AWBInsurrenceValue, dbo.Shipments.AWBAccountingInformation, dbo.Shipments.ProductCode,
dbo.Shipments.AWBDeclaredValueForCustoms, dbo.Shipments.AWBDeclaredValueForCarriage, dbo.Shipments.AWBCarrierTarrifReference,
dbo.Shipments.FreightForwarderContactId, dbo.Shipments.FreightForwarderAddressId, dbo.Shipments.CustomAgentExportContactId,
dbo.Shipments.CustomAgentExportAddressId, dbo.Shipments.ShipmentCustomerTypeCode, dbo.Shipments.CustomerReference1, dbo.Shipments.CustomerReference2, dbo.Shipments.CustomerContactId,
dbo.Shipments.CustomerAddressId, dbo.Shipments.CustomerId, dbo.Shipments.FreightForwarderReference, dbo.Shipments.FreightForwarderId,
dbo.Shipments.CustomAgentExportReference, dbo.Shipments.CustomAgentExportId, dbo.Shipments.CustomAgentImportReference,
dbo.Shipments.ConsigneeAddressOneTime, dbo.Shipments.ShipperAddressOneTime, dbo.Shipments.EstimateProfitInLocalCurrency, dbo.Shipments.AWBCurrencyId,
dbo.Shipments.OrderChargeableWeight, dbo.Shipments.OnCarriageCarrierId, dbo.Shipments.PreCarriageCarrierId, dbo.Shipments.ProfitInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency, dbo.Shipments.LastUpdateDate,
dbo.Shipments.UpdatedByUserId, dbo.Shipments.IsCancelled, dbo.Shipments.CancelledDate, dbo.Shipments.IsAccountingClosed, dbo.Shipments.ShipmentPayableStatusCode,
dbo.Shipments.ShipmentReceivableStatusCode, dbo.Shipments.QuoteId, dbo.Shipments.ShipmentDeliveryIndex, dbo.Shipments.ShipmentPickUpIndex,
dbo.Shipments.LTCWEdited, dbo.Shipments.OnCarriageVesselId, dbo.Shipments.PreCarriageVesselId, dbo.Shipments.DangerousMaterialDescription,
dbo.Shipments.DangerousPackagingGroup, dbo.Shipments.DangerousClassNumber, dbo.Shipments.DangerousUnNumber, dbo.Shipments.DangerousIMDGCode,
dbo.Shipments.DangerousFlashPoint, dbo.Shipments.AWBFreightAmountPrepaid, dbo.Shipments.AWBFreightAmountCollect, dbo.Shipments.IsDangerous,
dbo.Shipments.MainHarmonize, dbo.Shipments.VolumeUnitCode, dbo.Shipments.ChargeableWeightEdited,
dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, dbo.Shipments.ColoaderId,
dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps,
dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId,
dbo.Shipments.ConsigneeNotImporterContactId, dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId,
dbo.Shipments.ConsigneeNotImporterId, dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId,
dbo.Shipments.OrderIsDangerouseGoods, dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight,
dbo.Shipments.Field10, dbo.Shipments.Field9, dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4,
dbo.Shipments.Field3, dbo.Shipments.Field2, dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight,
dbo.Shipments.Field11, dbo.Shipments.Field12, dbo.Shipments.Field13, dbo.Shipments.Field14, dbo.Shipments.Field15,
dbo.Shipments.Field16, dbo.Shipments.Field17, dbo.Shipments.Field18, dbo.Shipments.Field19, dbo.Shipments.Field20,
dbo.Shipments.Field21, dbo.Shipments.Field22, dbo.Shipments.Field23, dbo.Shipments.Field24, dbo.Shipments.Field25,
dbo.Shipments.Field26, dbo.Shipments.Field27, dbo.Shipments.Field28, dbo.Shipments.Field29, dbo.Shipments.Field30,
dbo.Shipments.Field31, dbo.Shipments.Field32, dbo.Shipments.Field33, dbo.Shipments.Field34, dbo.Shipments.Field35,
dbo.Shipments.Field36, dbo.Shipments.Field37, dbo.Shipments.Field38, dbo.Shipments.Field39, dbo.Shipments.Field40,
dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId, dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId,
dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId, dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId,
dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId, dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD,
dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD, dbo.Shipments.AgentId,dbo.Shipments.AgentComputed, dbo.Shipments.OnCarriageCarrierNumber,
dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD, dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId,
dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber, dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD,
dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId, dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate,
dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId, dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId,
dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id, dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId,
dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId, dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId,
dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House, dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1,
dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
dbo.Shipments.ARInvoices,
dbo.Shipments.NotInvoicedReceivablesAmount,
dbo.Shipments.ReleasingAgentId,
dbo.Shipments.ContainerLastStatusDate,
dbo.Shipments.Notify1Reference,
dbo.Shipments.Notify2Reference,
dbo.Shipments.ShipperNotExporterReference,
dbo.Shipments.ConsigneeNotImporterReference,
dbo.Shipments.ProjectNumber,
dbo.Shipments.INTTRALastStatusDate,
dbo.Shipments.INTTRASIError,
dbo.Shipments.INTTRASIStatusCode,
dbo.Shipments.INTTRASIStatusDate,
dbo.Shipments.INTTRABookingError,
dbo.Shipments.INTTRALastBookingResponse,
dbo.INTTRASIStatus.Name AS INTTRASIStatusName,
dbo.Shipments.CreatedByPartner AS CreatedByPartner,
dbo.Shipments.INTTRABookingStatusCode,
dbo.INTTRABookingStatuses.Name AS INTTRABookingStatusName,
dbo.Shipments.INTTRABookingTransStatusCode,
dbo.INTTRABookingTransStatuses.Name AS INTTRABookingTransStatusName,
dbo.Shipments.AccountManagerUserId,
dbo.Shipments.CustomsDeclarationNumber,
dbo.Shipments.ShipperName,dbo.Shipments.FBLIsFromStock,
dbo.Shipments.FreightRelease, dbo.Shipments.TerminalAvailable, dbo.Shipments.ISFDate, dbo.Shipments.ISFNumber, dbo.Shipments.ITDate, dbo.Shipments.ITNumber,
dbo.ShipmentMasterDatas.OBLTypeCode, dbo.ShipmentMasterDatas.DocumentsClosingDate,
dbo.Shipments.ShipperName as Shipper, dbo.Shipments.ENSNumber, dbo.Shipments.ENSDate, dbo.Shipments.WarehouseLegWarehouseId, dbo.Shipments.WarehouseLegAddressId, dbo.Shipments.WarehouseLegTerminalCode, dbo.Shipments.WarehouseLegExpectedEntryDate,
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,
WarehouseLegCard.EnglishName AS WarehouseLegTerminalName,
dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,
dbo.EventTypes.EnglishName as LastSharedEventName,
WarehouseLegCard.CountryCode AS WarehouseLegAddressCountryCode,
WarehouseLegCard.CountryName AS WarehouseLegAddressCountryName,
--ShipperCards.EnglishName AS ShipperName,
MainCarriageFromPorts.Code AS MainCarriageFromPortCode, MainCarriageToPorts.Code AS MainCarriageToPortCode,
Transshipment1FromPorts.Code AS Transshipment1FromPortCode, Transshipment1ToPorts.Code AS Transshipment1ToPortCode,
Transshipment2FromPorts.Code AS Transshipment2FromPortCode, Transshipment2ToPorts.Code AS Transshipment2ToPortCode,
Transshipment3FromPorts.Code AS Transshipment3FromPortCode, Transshipment3ToPorts.Code AS Transshipment3ToPortCode,
PreCarriageFromPorts.Code AS PreCarriageFromPortCode, PreCarriageToPorts.Code AS PreCarriageToPortCode,
OnCarriageFromPorts.Code AS OnCarriageFromPortCode, OnCarriageToPorts.Code AS OnCarriageToPortCode,
MainCarriageToPorts.EnglishName AS MainCarriageToPortName, MainCarriageFromPorts.EnglishName AS MainCarriageFromPortName,
Transshipment1FromPorts.EnglishName AS Transshipment1FromPortName, Transshipment1ToPorts.EnglishName AS Transshipment1ToPortName,
Transshipment2ToPorts.EnglishName AS Transshipment2ToPortName, Transshipment2FromPorts.EnglishName AS Transshipment2FromPortName,
PreCarriageFromPorts.EnglishName AS PreCarriageFromPortName, OnCarriageFromPorts.EnglishName AS OnCarriageFromPortName,
PreCarriageToPorts.EnglishName AS PreCarriageToPortName, Transshipment3FromPorts.EnglishName AS Transshipment3FromPortName,
Transshipment3ToPorts.EnglishName AS Transshipment3ToPortName, OnCarriageToPorts.EnglishName AS OnCarriageToPortName,
CustomerCards.EnglishName AS CustomerName, CustomerCards.Notes AS CustomerNote,
ConsolidatorCards.EnglishName AS ConsolidatorName, ConsolidatorCards.Notes AS ConsolidatorNote,
FreightForwarderCards.EnglishName AS FreightForwarderName,
FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.Notes AS ShipperNote, ReleasingAgentCards.EnglishName AS ReleasingAgentName,
ConsigneeCards.EnglishName AS ConsigneeName,ConsigneeCards.EnglishName AS Consignee, ConsigneeCards.Notes AS ConsigneeNote, AgentComputedCards.EnglishName AS AgentName,
AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote,
CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote,
Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note,
ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote,
ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote,
ToPorts.Code AS ToPortCode, FromPorts.Code AS FromPortCode,
MainCarriageFromCountries.Code AS MainCarriageFromPortCountryCode, MainCarriageFromCountries.EnglishName AS MainCarriageFromPortCountryName,
MainCarriageToCountries.Code AS MainCarriageToPortCountryCode, MainCarriageToCountries.EnglishName AS MainCarriageToPortCountryName,
Transshipment1FromCountries.Code AS Transshipment1FromPortCountryCode,
Transshipment1FromCountries.EnglishName AS Transshipment1FromPortCountryName,
Transshipment2FromCountries.Code AS Transshipment2FromPortCountryCode,
Transshipment2FromCountries.EnglishName AS Transshipment2FromPortCountryName,
Transshipment3FromCountries.Code AS Transshipment3FromPortCountryCode,
Transshipment3FromCountries.EnglishName AS Transshipment3FromPortCountryName, PreCarriageFromCountries.Code AS PreCarriageFromPortCountryCode,
PreCarriageFromCountries.EnglishName AS PreCarriageFromPortCountryName, OnCarriageFromCountries.Code AS OnCarriageFromPortCountryCode,
OnCarriageFromCountries.EnglishName AS OnCarriageFromPortCountryName, Transshipment1ToCountries.Code AS Transshipment1ToPortCountryCode,
Transshipment1ToCountries.EnglishName AS Transshipment1ToPortCountryName, OnCarriageToCountries.Code AS OnCarriageToPortCountryCode,
OnCarriageToCountries.EnglishName AS OnCarriageToPortCountryName, Transshipment2ToCountries.Code AS Transshipment2ToPortCountryCode,
Transshipment2ToCountries.EnglishName AS Transshipment2ToPortCountryName, PreCarriageToCountries.Code AS PreCarriageToPortCountryCode,
PreCarriageToCountries.EnglishName AS PreCarriageToPortCountryName, Transshipment3ToCountries.Code AS Transshipment3ToPortCountryCode,
Transshipment3ToCountries.EnglishName AS Transshipment3ToPortCountryName, MainCarriageCarrierCards.EnglishName AS MainCarriageCarrierName,
MainCarriageCarrierCards.Code AS MainCarriageCarrierCode, Transshipment1CarrierCards.EnglishName AS Transshipment1CarrierName,
Transshipment1CarrierCards.Code AS Transshipment1CarrierCode, Transshipment2CarrierCards.EnglishName AS Transshipment2CarrierName,
Transshipment2CarrierCards.Code AS Transshipment2CarrierCode, Transshipment3CarrierCards.EnglishName AS Transshipment3CarrierName,
Transshipment3CarrierCards.Code AS Transshipment3CarrierCode, PreCarriageCarrierCards.EnglishName AS PreCarriageCarrierName,
PreCarriageCarrierCards.Code AS PreCarriageCarrierCode, OnCarriageCarrierCards.EnglishName AS OnCarriageCarrierName,
OnCarriageCarrierCards.Code AS OnCarriageCarrierCode, dbo.NextLegs.Name AS NextLegName, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.ShipmentReceivableStatus.Name AS ShipmentReceivableStatusName, dbo.ShipmentPayableStatus.Name AS ShipmentPayableStatusName,
AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName,dbo.MoveTypes.MoveTypeEnglishName AS MoveTypeName ,
dbo.ShipmentLevels.Name AS ShipmentLevelName,
dbo.EntityStatus.Id AS ShipmentStatusId,
dbo.EntityStatus.Name AS ShipmentStatusName,
dbo.EntityStatus.StatusWeight AS ShipmentStatusWeight,
dbo.Shipments.StatusDate as ShipmentStatusDate,
dbo.Shipments.StatusLocation as ShipmentStatusLocation,
ShipmentMasterDataEntityStatus.Id AS ShipmentMasterDataStatusId,
ShipmentMasterDataEntityStatus.Name AS ShipmentMasterDataStatusName,
ShipmentMasterDataEntityStatus.StatusWeight AS ShipmentMasterDataStatusWeight,
dbo.ShipmentMasterDatas.StatusDate As ShipmentMasterDataStatusDate,
dbo.ShipmentMasterDatas.StatusLocation as ShipmentMasterDataStatusLocation,
MainCarriageFinalDestinationPorts.Code AS MainCarriageFinalDestinationPortCode,
MainCarriageFinalDestinationPorts.EnglishName AS MainCarriageFinalDestinationPortName,
FinalDestinationCountries.Code AS MainCarriageFinalDestinationCountryCode,
FinalDestinationCountries.EnglishName AS MainCarriageFinalDestinationCountryName,
dbo.ShipmentMasterDatas.MasterShipmentNumber,
dbo.ShipmentMasterDatas.CarrierTransportDocumentNumber,
dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,dbo.Shipments.OperationalClosedByUserId,
dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency,
dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.GrossWeightPerStorageDays,dbo.Shipments.GrossWeightPerTon,
dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight,
dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Incoterms.Code AS IncotermCode,
dbo.FHLStatus.Code AS FHLStatusCode,
dbo.FHLStatus.Name AS FHLStatusName,
dbo.Shipments.FHLStatusDate,
dbo.FWBStatus.Code AS FWBStatusCode,
dbo.FWBStatus.Name AS FWBStatusName,
dbo.ShipmentMasterDatas.FWBStatusDate,
dbo.Shipments.LastSentByUserId,
dbo.CustomsTransmissionsStatus.Code AS LocalCustomsTransmissionsStatusCode,
dbo.CustomsTransmissionsStatus.Name AS LocalCustomsTransmissionsStatusName,
dbo.shipments.LocalCustomsTransmissionsStatusError,
dbo.Shipments.LocalCustomsTransmissionsStatusDate,
dbo.Shipments.LocalCustomsSentByUserId,
LocalCustomsSentByUser.EnglishName as LocalCustomsSentByUserName,
CargonautFHLStatus.Code AS CargonautFHLStatusCode,
CargonautFHLStatus.Name AS CargonautFHLStatusName,
dbo.Shipments.CargonautFHLStatusDate,
CargonautFWBStatus.Code AS CargonautFWBStatusCode,
CargonautFWBStatus.Name AS CargonautFWBStatusName,
dbo.ShipmentMasterDatas.CargonautFWBStatusDate,
dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason, dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber,
dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges, dbo.Shipments.AccountNumber,
dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate, dbo.Shipments.LastFSRStatusRequestDate,
dbo.Shipments.FinalArrivalDate, dbo.Shipments.EstimatedFinalArrivalDate, dbo.Shipments.ActualFinalArrivalDate, FromPortCountries.Code AS FromPortCountryCode,
ToPortCountries.Code AS ToPortCountryCode, dbo.Shipments.TEU, MainCarriageFromAddresses.City AS MainCarriageFromCity,
MainCarriageToAddresses.City AS MainCarriageToCity, MainCarriageFromAddressCountries.Code AS MainCarriageFromCountryCode,
MainCarriageToAddressCountries.Code AS MainCarriageToCountryCode, dbo.Shipments.ProfitExchangeRate, dbo.Shipments.CASSCode, dbo.Shipments.AMSBL,
dbo.Shipments.SpecialServicesTypeId, dbo.SpecialServicesTypes.Code AS SpecialServicesTypeCode,
dbo.SpecialServicesTypes.EnglishName AS SpecialServicesTypeName, dbo.Shipments.CustomFileId, dbo.Shipments.CustomFileNumber,
dbo.Shipments.NoFreightFile, ToPortCountries.EnglishName AS ToPortCountryName, FromPortCountries.EnglishName AS FromPortCountryName,
dbo.Vessels.EnglishName AS MainCarriageVesselName,dbo.Shipments.LastStatusLogDate As LastStatusLogDate,
AccountManagerUserContacts.EnglishName as AccountManagerUserName,
SalesmanUserContact.EnglishName as SalesmanUserName,
CreatedByUserContact.EnglishName as CreatedByUserName,
LastSentByUserContact.EnglishName as LastSentByUserName,
ShipmentComputedFields.IsMissingDocuments as IsMissingDocument,
ShipmentComputedFields.DocumentsSearchFields as DocumentsSearchFields,
dbo.Shipments.ForwarderShipmentNumber as ForwarderShipmentNumber,
dbo.Shipments.CustomerShipmentNumber as CustomerShipmentNumber,
ShipmentComputedFields.LastDocumentDateTime as LastDocumentDateTime,
HybridPartner.SmallLogoId as PartnerLogoId,
ShipmentComputedFields.MissingDocumentsCount as MissingDocumentsCount,
ShipmentComputedFields.MissingDocumentsNames as MissingDocsNames,
ShipmentComputedFields.RequestedDocumentsCount as RequestedDocumentsCount,
ShipmentComputedFields.IsRequestedDocuments as IsRequestedDocuments,
ShipmentComputedFields.IsDigitalSignRequired as IsDigitalSignRequired,
ShipmentComputedFields.IsDepositionRequired as IsDepositionRequired,
ShipmentComputedFields.ImporterDepositionRequestDetails as ImporterDepositionRequestDetails,
ShipmentComputedFields.NumberOfHouses as NumberOfHouses,
ShipmentAdditionalCloudDatas.DeclarationXmlData as DeclarationXmlData,
ShipmentAdditionalCloudDatas.IsImporterApprovalRequried as IsImporterApprovalRequried,
ShipmentAdditionalCloudDatas.ApprovedByUserName as ApprovedByUserName,
ShipmentAdditionalCloudDatas.ApproveDateTime as ApproveDateTime,
ShipmentAdditionalCloudDatas.VersionApproved as VersionApproved,
HybridPartner.Name as PartnerName,
HybridPartner.Id as ForwarderPartnerId,
dbo.ShipmentMasterDatas.DepartureArrivalFromDate as DepartureArrivalFromDate,
dbo.ShipmentMasterDatas.DepartureArrivalToDate as DepartureArrivalToDate,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationETA as MainCarriageFinalDestinationETA,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationATA as MainCarriageFinalDestinationATA,
CASE WHEN ( NOT ((dbo.ShipmentTypes.Name IS NULL) OR ((LEN(dbo.ShipmentTypes.Name)) = 0))) THEN CASE WHEN (dbo.ShipmentTypes.Name IS NULL) THEN N'''' ELSE dbo.ShipmentTypes.Name END + N'' '' + CASE WHEN (dbo.ShipmentLevels.Name IS NULL) THEN N'''' ELSE dbo.ShipmentLevels.Name END ELSE dbo.ShipmentLevels.Name END AS ShipmentType,
CASE WHEN ( NOT ((MainCarriageFromPortId IS NULL) OR ((LEN(MainCarriageFromPortId)) = 0))) THEN MainCarriageFromPortId ELSE dbo.Shipments.FromPortId END AS FromPortId,
CASE WHEN ( NOT ((MainCarriageToPortId IS NULL) OR ((LEN(MainCarriageToPortId)) = 0))) THEN MainCarriageToPortId ELSE dbo.Shipments.ToPortId END AS ToPortId,
CASE WHEN ( NOT ((MainCarriageFromPorts.Code IS NULL) OR ((LEN(MainCarriageFromPorts.Code)) = 0))) THEN MainCarriageFromPorts.Code ELSE FromPorts.Code END AS FromPort,
CASE WHEN ( NOT ((MainCarriageFromPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFromPorts.EnglishName)) = 0))) THEN MainCarriageFromPorts.EnglishName ELSE FromPorts.EnglishName END AS FromPortName,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.Code IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.Code)) = 0))) THEN MainCarriageFinalDestinationPorts.Code ELSE ToPorts.Code END AS ToPort,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.EnglishName)) = 0))) THEN MainCarriageFinalDestinationPorts.EnglishName ELSE ToPorts.EnglishName END AS ToPortName,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (MainCarriageCarrierCards.Code IS NULL) THEN N'''' ELSE MainCarriageCarrierCards.Code END + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''O'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (dbo.Vessels.EnglishName IS NULL) THEN N'''' ELSE dbo.Vessels.EnglishName END + N''/'' + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''I'' = dbo.Shipments.TransportModeId) THEN MainCarriageCarrierNumber END AS CarrierNumber,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN MainCarriageATA ELSE MainCarriageETA END AS MainCarriageExpectedOrActual,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN N''ATA'' ELSE N''ETA'' END AS MainCarriageETAOrATA,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusDate ELSE dbo.Shipments.StatusDate END ELSE dbo.Shipments.StatusDate END AS StatusDate,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Name ELSE dbo.EntityStatus.Name END ELSE dbo.EntityStatus.Name END AS StatusName,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusLocation ELSE dbo.Shipments.StatusLocation END ELSE dbo.Shipments.StatusLocation END AS StatusLocation,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (( NOT ((AirlinePrefix IS NULL) OR ((LEN(AirlinePrefix)) = 0))) AND ( NOT ((Master IS NULL) OR ((LEN(Master)) = 0)))) THEN CASE WHEN (AirlinePrefix IS NULL) THEN N'''' ELSE AirlinePrefix END + N''-'' + CASE WHEN (Master IS NULL) THEN N'''' ELSE Master END ELSE N'''' END ELSE Master END AS LongMaster,
CAST( MissingDocumentsCount AS nvarchar(max)) + N'' Missing'' AS MissingDocumentsCountWords,
CASE WHEN (IsOperationalClosed = 1) THEN N''Archived'' ELSE N'''' END AS ArchivedText
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageFromAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageToAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
--ELSE MainCarriageFinalDestinationPortsStates.EnglishName END) END AS MainCarriageToState
FROM            dbo.Shipments LEFT OUTER JOIN
dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId LEFT OUTER JOIN
dbo.Ports AS MainCarriageFromPorts ON dbo.ShipmentMasterDatas.MainCarriageFromPortId = MainCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageToPorts ON dbo.ShipmentMasterDatas.MainCarriageToPortId = MainCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1FromPorts ON dbo.ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1ToPorts ON dbo.ShipmentMasterDatas.Transshipment1ToPortId = Transshipment1ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2FromPorts ON dbo.ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2ToPorts ON dbo.ShipmentMasterDatas.Transshipment2ToPortId = Transshipment2ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3FromPorts ON dbo.ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3ToPorts ON dbo.ShipmentMasterDatas.Transshipment3ToPortId = Transshipment3ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageFinalDestinationPorts ON dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageFromPorts ON dbo.Shipments.PreCarriageFromPortId = PreCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageToPorts ON dbo.Shipments.PreCarriageToPortId = PreCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageFromPorts ON dbo.Shipments.OnCarriageFromPortId = OnCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageToPorts ON dbo.Shipments.OnCarriageToPortId = OnCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Shipments.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Shipments.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Cards AS CustomerCards ON dbo.Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsolidatorCards ON dbo.Shipments.ConsolidatorId = ConsolidatorCards.Id LEFT OUTER JOIN
dbo.Cards AS FreightForwarderCards ON dbo.Shipments.FreightForwarderId = FreightForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Shipments.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Shipments.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentCards ON dbo.Shipments.AgentId = AgentCards.Id LEFT OUTER JOIN
dbo.Cards AS ReleasingAgentCards ON dbo.Shipments.ReleasingAgentId = ReleasingAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentComputedCards ON dbo.Shipments.AgentComputed = AgentComputedCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentExportCards ON dbo.Shipments.CustomAgentExportId = CustomAgentExportCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentImportCards ON dbo.Shipments.CustomAgentImportId = CustomAgentImportCards.Id LEFT OUTER JOIN
dbo.Cards AS Notify1Cards ON dbo.Shipments.Notify1Id = Notify1Cards.Id LEFT OUTER JOIN
dbo.Cards AS Notify2Cards ON dbo.Shipments.Notify2Id = Notify2Cards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperNotExporterCards ON dbo.Shipments.ShipperNotExporterId = ShipperNotExporterCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeNotImporterCards ON dbo.Shipments.ConsigneeNotImporterId = ConsigneeNotImporterCards.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromCountries ON MainCarriageFromPorts.CountryId = MainCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToCountries ON MainCarriageToPorts.CountryId = MainCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1FromCountries ON Transshipment1FromPorts.CountryId = Transshipment1FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2FromCountries ON Transshipment2FromPorts.CountryId = Transshipment2FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3FromCountries ON Transshipment3FromPorts.CountryId = Transshipment3FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageFromCountries ON PreCarriageFromPorts.CountryId = PreCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageFromCountries ON OnCarriageFromPorts.CountryId = OnCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1ToCountries ON Transshipment1ToPorts.CountryId = Transshipment1ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2ToCountries ON Transshipment2ToPorts.CountryId = Transshipment2ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3ToCountries ON Transshipment3ToPorts.CountryId = Transshipment3ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageToCountries ON PreCarriageToPorts.CountryId = PreCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageToCountries ON OnCarriageToPorts.CountryId = OnCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS FinalDestinationCountries ON MainCarriageFinalDestinationPorts.CountryId = FinalDestinationCountries.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarrierCards ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment1CarrierCards ON dbo.ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment2CarrierCards ON dbo.ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment3CarrierCards ON dbo.ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS PreCarriageCarrierCards ON dbo.Shipments.PreCarriageCarrierId = PreCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS OnCarriageCarrierCards ON dbo.Shipments.OnCarriageCarrierId = OnCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS WarehouseLegCard ON dbo.Shipments.WarehouseLegWarehouseId = WarehouseLegCard.Id LEFT OUTER JOIN
dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
dbo.MoveTypes ON dbo.Shipments.MoveTypeId = dbo.MoveTypes.Id LEFT OUTER JOIN
dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.CustomsTransmissionsStatus ON dbo.Shipments.LocalCustomsTransmissionsStatusCode = dbo.CustomsTransmissionsStatus.Code LEFT OUTER JOIN
dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
dbo.INTTRASIStatus ON dbo.Shipments.INTTRASIStatusCode = dbo.INTTRASIStatus.Code LEFT OUTER JOIN
dbo.INTTRABookingTransStatuses ON dbo.Shipments.INTTRABookingTransStatusCode = dbo.INTTRABookingTransStatuses.Code LEFT OUTER JOIN
dbo.INTTRABookingStatuses ON dbo.Shipments.INTTRABookingStatusCode = dbo.INTTRABookingStatuses.Code LEFT OUTER JOIN
dbo.Addresses AS MainCarriageFromAddresses ON dbo.ShipmentMasterDatas.MainCarriageFromAddressId = MainCarriageFromAddresses.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageToAddresses ON dbo.ShipmentMasterDatas.MainCarriageToAddressId = MainCarriageToAddresses.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageToPartners ON dbo.ShipmentMasterDatas.MainCarriageToPartnerId = MainCarriageToPartners.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageFromPartners ON dbo.ShipmentMasterDatas.MainCarriageFromPartnerId = MainCarriageFromPartners.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromAddressCountries ON MainCarriageFromAddresses.CountryId = MainCarriageFromAddressCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToAddressCountries ON MainCarriageToAddresses.CountryId = MainCarriageToAddressCountries.Id LEFT OUTER JOIN
dbo.SpecialServicesTypes ON dbo.Shipments.SpecialServicesTypeId = dbo.SpecialServicesTypes.Id LEFT OUTER JOIN
dbo.Vessels ON dbo.ShipmentMasterDatas.MainCarriageVesselId = dbo.Vessels.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Shipments.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContact ON dbo.Shipments.SalesmanUserId = SalesmanUserContact.Id LEFT OUTER JOIN
dbo.HybridPartners AS HybridPartner ON dbo.Shipments.ForwarderPartnerId = HybridPartner.Id LEFT OUTER JOIN
dbo.EventTypes ON dbo.Shipments.LastSharedEventId = dbo.EventTypes.Id LEFT OUTER JOIN
dbo.Contacts AS LastSentByUserContact ON dbo.Shipments.LastSentByUserId = LastSentByUserContact.Id LEFT OUTER JOIN
dbo.Contacts AS LocalCustomsSentByUser ON dbo.Shipments.LocalCustomsSentByUserId = LocalCustomsSentByUser.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContact ON dbo.Shipments.CreatedByUserId = CreatedByUserContact.Id INNER JOIN
dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id INNER JOIN
dbo.ShipmentAdditionalCloudDatas AS ShipmentAdditionalCloudDatas ON dbo.Shipments.Id = ShipmentAdditionalCloudDatas.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromAddressesStates ON MainCarriageFromAddresses.StateId = MainCarriageFromAddressesStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageToAddressesStates ON MainCarriageToAddresses.StateId = MainCarriageToAddressesStates.Id LEFT OUTER JOIN
dbo.States AS FromPortsStates ON FromPorts.StateId = FromPortsStates.Id  LEFT OUTER JOIN
dbo.States AS ToPortsStates ON ToPorts.StateId = ToPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromPortsStates ON MainCarriageFromPorts.StateId = MainCarriageFromPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id');


-- DataView Script From ShipmentFollowUpDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentFollowUpDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentFollowUpDataView] END');
EXEC('CREATE VIEW [dbo].[ShipmentFollowUpDataView]
AS
SELECT         dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,
dbo.Shipments.DeclarationNumber, dbo.Shipments.DeclarationDate,
dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId,
dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId,
dbo.Shipments.LastFinalDestination, [dbo].[Shipments].[From], [dbo].[Shipments].[To], dbo.Shipments.Origin, dbo.Shipments.FirstPickupETA, dbo.Shipments.FirstPickupETD,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId, dbo.ShipmentMasterDatas.Transshipment3CarrierId,
dbo.ShipmentMasterDatas.Transshipment2CarrierId, dbo.ShipmentMasterDatas.Transshipment1CarrierId, dbo.ShipmentMasterDatas.MainCarriageCarrierId,
dbo.ShipmentMasterDatas.MainCarriageIsFromStack, dbo.ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dbo.ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment3VesselId, dbo.ShipmentMasterDatas.Transshipment2VesselId, dbo.ShipmentMasterDatas.Transshipment1VesselId,
dbo.ShipmentMasterDatas.MainCarriageVesselId, dbo.ShipmentMasterDatas.BookingConfirmedBy, dbo.ShipmentMasterDatas.BookingConfirmationNotes,
dbo.ShipmentMasterDatas.BookingConfirmationNumber, dbo.ShipmentMasterDatas.MAWBOBLDate, dbo.ShipmentMasterDatas.Transshipment3CarrierNumber,
dbo.ShipmentMasterDatas.Transshipment3ETA, dbo.ShipmentMasterDatas.Transshipment3ETD, dbo.ShipmentMasterDatas.Transshipment3ATA,
dbo.ShipmentMasterDatas.Transshipment3ATD, dbo.ShipmentMasterDatas.Transshipment3ToPortId, dbo.ShipmentMasterDatas.Transshipment3FromPortId,
dbo.ShipmentMasterDatas.Transshipment2CarrierNumber, dbo.ShipmentMasterDatas.Transshipment2ETA, dbo.ShipmentMasterDatas.Transshipment2ETD,
dbo.ShipmentMasterDatas.Transshipment2ATA, dbo.ShipmentMasterDatas.Transshipment2ATD, dbo.ShipmentMasterDatas.Transshipment2ToPortId,
dbo.ShipmentMasterDatas.Transshipment2FromPortId, dbo.ShipmentMasterDatas.Transshipment1CarrierNumber, dbo.ShipmentMasterDatas.Transshipment1ETA,
dbo.ShipmentMasterDatas.Transshipment1ETD, dbo.ShipmentMasterDatas.Transshipment1ATA, dbo.ShipmentMasterDatas.Transshipment1ATD,
dbo.ShipmentMasterDatas.Transshipment1ToPortId, dbo.ShipmentMasterDatas.Transshipment1FromPortId, dbo.ShipmentMasterDatas.Master,
dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA,
dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2, dbo.Shipments.ToPortId,
dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
dbo.MoveTypes.MoveTypeEnglishName AS MoveTypeName,
dbo.Shipments.ARInvoices,
dbo.Shipments.NotInvoicedReceivablesAmount,
dbo.Shipments.ContainerLastStatusDate,
dbo.Shipments.Notify1Reference,
dbo.Shipments.Notify2Reference,
dbo.Shipments.ShipperNotExporterReference,
dbo.Shipments.ConsigneeNotImporterReference,
dbo.Shipments.ProjectNumber,
dbo.Shipments.INTTRALastStatusDate,
dbo.Shipments.INTTRASIError,
dbo.Shipments.INTTRASIStatusCode,
dbo.Shipments.INTTRASIStatusDate,
dbo.Shipments.INTTRABookingError,
dbo.Shipments.INTTRALastBookingResponse,
dbo.INTTRASIStatus.Name AS INTTRASIStatusName,
dbo.Shipments.CreatedByPartner AS CreatedByPartner,
dbo.Shipments.FromPortId, dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails, dbo.Shipments.OperationalDate, dbo.ShipmentMasterDatas.CutoffDate,
dbo.Shipments.FinalArrivalDate, dbo.Shipments.EstimatedFinalArrivalDate, dbo.Shipments.ActualFinalArrivalDate, dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.EstimateProfitInProfitCurrency, dbo.Shipments.ProfitCurrencyId, dbo.Shipments.SCI,
dbo.Shipments.AWBHandlingInformation, dbo.Shipments.AWBInsurrenceValue, dbo.Shipments.AWBAccountingInformation,
dbo.Shipments.AWBDeclaredValueForCustoms, dbo.Shipments.AWBDeclaredValueForCarriage, dbo.Shipments.AWBCarrierTarrifReference,
dbo.Shipments.FreightForwarderContactId, dbo.Shipments.FreightForwarderAddressId, dbo.Shipments.CustomAgentExportContactId,
dbo.Shipments.CustomAgentExportAddressId, dbo.Shipments.ShipmentCustomerTypeCode, dbo.Shipments.CustomerReference1, dbo.Shipments.CustomerReference2, dbo.Shipments.CustomerContactId,
dbo.Shipments.CustomerAddressId, dbo.Shipments.CustomerId, dbo.Shipments.FreightForwarderReference, dbo.Shipments.FreightForwarderId,
dbo.Shipments.CustomAgentExportReference, dbo.Shipments.CustomAgentExportId, dbo.Shipments.CustomAgentImportReference,
dbo.Shipments.ConsigneeAddressOneTime, dbo.Shipments.ShipperAddressOneTime, dbo.Shipments.EstimateProfitInLocalCurrency, dbo.Shipments.AWBCurrencyId,
dbo.Shipments.OrderChargeableWeight, dbo.Shipments.OnCarriageCarrierId, dbo.Shipments.PreCarriageCarrierId, dbo.Shipments.ProfitInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency, dbo.Shipments.LastUpdateDate,
dbo.Shipments.UpdatedByUserId, dbo.Shipments.IsCancelled, dbo.Shipments.IsAccountingClosed, dbo.Shipments.ShipmentPayableStatusCode,
dbo.Shipments.ShipmentReceivableStatusCode, dbo.Shipments.QuoteId, dbo.Shipments.ShipmentDeliveryIndex, dbo.Shipments.ShipmentPickUpIndex,
dbo.Shipments.LTCWEdited, dbo.Shipments.OnCarriageVesselId, dbo.Shipments.PreCarriageVesselId, dbo.Shipments.DangerousMaterialDescription,
dbo.Shipments.DangerousPackagingGroup, dbo.Shipments.DangerousClassNumber, dbo.Shipments.DangerousUnNumber, dbo.Shipments.DangerousIMDGCode,
dbo.Shipments.DangerousFlashPoint, dbo.Shipments.AWBFreightAmountPrepaid, dbo.Shipments.AWBFreightAmountCollect, dbo.Shipments.IsDangerous,
dbo.Shipments.MainHarmonize, dbo.Shipments.VolumeUnitCode, dbo.Shipments.ChargeableWeightEdited,
dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, dbo.Shipments.RegistryDate, dbo.Shipments.IsAssembly, dbo.Shipments.FirstOperationalCloseDate,
dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps, dbo.Shipments.FirstAccountingCloseDate,
dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.ValueOfGoods,
dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId, dbo.Shipments.ConsigneeNotImporterContactId,
dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId, dbo.Shipments.ConsigneeNotImporterId,
dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId, dbo.Shipments.OrderIsDangerouseGoods,
dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight, dbo.Shipments.Field10, dbo.Shipments.Field9,
dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4, dbo.Shipments.Field3, dbo.Shipments.Field2,
dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight, dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId,
dbo.Shipments.Field11, dbo.Shipments.Field12, dbo.Shipments.Field13, dbo.Shipments.Field14, dbo.Shipments.Field15,
dbo.Shipments.Field16, dbo.Shipments.Field17, dbo.Shipments.Field18, dbo.Shipments.Field19, dbo.Shipments.Field20,
dbo.Shipments.Field21, dbo.Shipments.Field22, dbo.Shipments.Field23, dbo.Shipments.Field24, dbo.Shipments.Field25,
dbo.Shipments.Field26, dbo.Shipments.Field27, dbo.Shipments.Field28, dbo.Shipments.Field29, dbo.Shipments.Field30,
dbo.Shipments.Field31, dbo.Shipments.Field32, dbo.Shipments.Field33, dbo.Shipments.Field34, dbo.Shipments.Field35,
dbo.Shipments.Field36, dbo.Shipments.Field37, dbo.Shipments.Field38, dbo.Shipments.Field39, dbo.Shipments.Field40,
dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId, dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId,
dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId, dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId,
dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD, dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD,
dbo.Shipments.AgentId,dbo.Shipments.AgentComputed, dbo.Shipments.OnCarriageCarrierNumber, dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD,
dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId, dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber,
dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD, dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId,
dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate, dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId,
dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId, dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id,
dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId, dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId,
dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId, dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House,
dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1, MainCarriageFromPorts.Code AS MainCarriageFromPortCode,
dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
dbo.Shipments.CustomsDeclarationNumber,dbo.Shipments.FBLIsFromStock,
dbo.Shipments.FreightRelease, dbo.Shipments.TerminalAvailable, dbo.Shipments.ISFDate, dbo.Shipments.ISFNumber, dbo.Shipments.ITDate, dbo.Shipments.ITNumber,
dbo.ShipmentMasterDatas.OBLTypeCode, dbo.ShipmentMasterDatas.DocumentsClosingDate, dbo.Shipments.ENSNumber, dbo.Shipments.ENSDate,
dbo.Shipments.WarehouseLegWarehouseId, dbo.Shipments.WarehouseLegAddressId, dbo.Shipments.WarehouseLegTerminalCode, dbo.Shipments.WarehouseLegExpectedEntryDate,
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,
dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,
dbo.EventTypes.EnglishName as LastSharedEventName,
MainCarriageToPorts.Code AS MainCarriageToPortCode, Transshipment1FromPorts.Code AS Transshipment1FromPortCode,
Transshipment1ToPorts.Code AS Transshipment1ToPortCode, Transshipment2FromPorts.Code AS Transshipment2FromPortCode,
Transshipment2ToPorts.Code AS Transshipment2ToPortCode, Transshipment3FromPorts.Code AS Transshipment3FromPortCode,
Transshipment3ToPorts.Code AS Transshipment3ToPortCode, PreCarriageFromPorts.Code AS PreCarriageFromPortCode,
PreCarriageToPorts.Code AS PreCarriageToPortCode, OnCarriageFromPorts.Code AS OnCarriageFromPortCode, OnCarriageToPorts.Code AS OnCarriageToPortCode,
MainCarriageToPorts.EnglishName AS MainCarriageToPortName, MainCarriageFromPorts.EnglishName AS MainCarriageFromPortName,
Transshipment1FromPorts.EnglishName AS Transshipment1FromPortName, Transshipment1ToPorts.EnglishName AS Transshipment1ToPortName,
Transshipment2ToPorts.EnglishName AS Transshipment2ToPortName, Transshipment2FromPorts.EnglishName AS Transshipment2FromPortName,
PreCarriageFromPorts.EnglishName AS PreCarriageFromPortName, OnCarriageFromPorts.EnglishName AS OnCarriageFromPortName,
PreCarriageToPorts.EnglishName AS PreCarriageToPortName, Transshipment3FromPorts.EnglishName AS Transshipment3FromPortName,
Transshipment3ToPorts.EnglishName AS Transshipment3ToPortName, OnCarriageToPorts.EnglishName AS OnCarriageToPortName,
CustomerCards.EnglishName AS CustomerName, CustomerCards.Notes AS CustomerNote,
ConsolidatorCards.EnglishName AS ConsolidatorName, ConsolidatorCards.Notes AS ConsolidatorNote,
FreightForwarderCards.EnglishName AS FreightForwarderName,
FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.EnglishName AS ShipperName, ShipperCards.Notes AS ShipperNote,
ConsigneeCards.EnglishName AS ConsigneeName, ConsigneeCards.Notes AS ConsigneeNote, AgentCards.EnglishName AS AgentName,
AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote,
CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote,
Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note,
ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote,
ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote,
ToPorts.Code AS ToPortCode, ToPorts.EnglishName AS ToPortName, FromPorts.Code AS FromPortCode, FromPorts.EnglishName AS FromPortName,
MainCarriageFromCountries.Code AS MainCarriageFromPortCountryCode, MainCarriageFromCountries.EnglishName AS MainCarriageFromPortCountryName,
MainCarriageToCountries.Code AS MainCarriageToPortCountryCode, MainCarriageToCountries.EnglishName AS MainCarriageToPortCountryName,
Transshipment1FromCountries.Code AS Transshipment1FromPortCountryCode,
Transshipment1FromCountries.EnglishName AS Transshipment1FromPortCountryName,
Transshipment2FromCountries.Code AS Transshipment2FromPortCountryCode,
Transshipment2FromCountries.EnglishName AS Transshipment2FromPortCountryName,
Transshipment3FromCountries.Code AS Transshipment3FromPortCountryCode,
Transshipment3FromCountries.EnglishName AS Transshipment3FromPortCountryName, PreCarriageFromCountries.Code AS PreCarriageFromPortCountryCode,
PreCarriageFromCountries.EnglishName AS PreCarriageFromPortCountryName, OnCarriageFromCountries.Code AS OnCarriageFromPortCountryCode,
OnCarriageFromCountries.EnglishName AS OnCarriageFromPortCountryName, Transshipment1ToCountries.Code AS Transshipment1ToPortCountryCode,
Transshipment1ToCountries.EnglishName AS Transshipment1ToPortCountryName, OnCarriageToCountries.Code AS OnCarriageToPortCountryCode,
OnCarriageToCountries.EnglishName AS OnCarriageToPortCountryName, Transshipment2ToCountries.Code AS Transshipment2ToPortCountryCode,
Transshipment2ToCountries.EnglishName AS Transshipment2ToPortCountryName, PreCarriageToCountries.Code AS PreCarriageToPortCountryCode,
PreCarriageToCountries.EnglishName AS PreCarriageToPortCountryName, Transshipment3ToCountries.Code AS Transshipment3ToPortCountryCode,
Transshipment3ToCountries.EnglishName AS Transshipment3ToPortCountryName, MainCarriageCarrierCards.EnglishName AS MainCarriageCarrierName,
MainCarriageCarrierCards.Code AS MainCarriageCarrierCode, Transshipment1CarrierCards.EnglishName AS Transshipment1CarrierName,
Transshipment1CarrierCards.Code AS Transshipment1CarrierCode, Transshipment2CarrierCards.EnglishName AS Transshipment2CarrierName,
Transshipment2CarrierCards.Code AS Transshipment2CarrierCode, Transshipment3CarrierCards.EnglishName AS Transshipment3CarrierName,
Transshipment3CarrierCards.Code AS Transshipment3CarrierCode, PreCarriageCarrierCards.EnglishName AS PreCarriageCarrierName,
PreCarriageCarrierCards.Code AS PreCarriageCarrierCode, OnCarriageCarrierCards.EnglishName AS OnCarriageCarrierName,
OnCarriageCarrierCards.Code AS OnCarriageCarrierCode, dbo.NextLegs.Name AS NextLegName, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.ShipmentReceivableStatus.Name AS ShipmentReceivableStatusName, dbo.ShipmentPayableStatus.Name AS ShipmentPayableStatusName,
AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName,
dbo.ShipmentLevels.Name AS ShipmentLevelName,
dbo.Shipments.StatusId,
dbo.EntityStatus.Id AS ShipmentStatusId,
dbo.EntityStatus.Name AS ShipmentStatusName,
dbo.EntityStatus.StatusWeight AS ShipmentStatusWeight,
dbo.Shipments.StatusDate as ShipmentStatusDate,
dbo.Shipments.StatusLocation as ShipmentStatusLocation,
ShipmentMasterDataEntityStatus.Id AS ShipmentMasterDataStatusId,
ShipmentMasterDataEntityStatus.Name AS ShipmentMasterDataStatusName,
ShipmentMasterDataEntityStatus.StatusWeight AS ShipmentMasterDataStatusWeight,
dbo.ShipmentMasterDatas.StatusDate As ShipmentMasterDataStatusDate,
dbo.ShipmentMasterDatas.StatusLocation as ShipmentMasterDataStatusLocation,
MainCarriageFinalDestinationPorts.Code AS MainCarriageFinalDestinationPortCode,
MainCarriageFinalDestinationPorts.EnglishName AS MainCarriageFinalDestinationPortName, dbo.ShipmentMasterDatas.MasterShipmentNumber,
dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,
dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency,
dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG,  dbo.Shipments.GrossWeightPerStorageDays,dbo.Shipments.GrossWeightPerTon,
dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight,
dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Shipments.ProductCode,
dbo.Incoterms.Code AS IncotermCode,
dbo.FHLStatus.Code AS FHLStatusCode,
dbo.FHLStatus.Name AS FHLStatusName,
dbo.Shipments.FHLStatusDate,
dbo.FWBStatus.Code AS FWBStatusCode,
dbo.FWBStatus.Name AS FWBStatusName,
dbo.ShipmentMasterDatas.FWBStatusDate,
dbo.CustomsTransmissionsStatus.Code AS LocalCustomsTransmissionsStatusCode,
dbo.CustomsTransmissionsStatus.Name AS LocalCustomsTransmissionsStatusName,
dbo.shipments.LocalCustomsTransmissionsStatusError,
dbo.Shipments.LocalCustomsTransmissionsStatusDate,
CargonautFHLStatus.Code AS CargonautFHLStatusCode,
CargonautFHLStatus.Name AS CargonautFHLStatusName,
dbo.Shipments.CargonautFHLStatusDate,
CargonautFWBStatus.Code AS CargonautFWBStatusCode,
CargonautFWBStatus.Name AS CargonautFWBStatusName,
dbo.ShipmentMasterDatas.CargonautFWBStatusDate,
dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason,
dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber, dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges,
dbo.Shipments.AccountNumber, dbo.FollowUps.Id AS FollowUpId, dbo.FollowUps.Date AS FollowUpDate, dbo.FollowUps.Notes AS FollowUpNotes,
dbo.FollowUps.EventTypeId AS FollowUpTypeId, dbo.FollowUps.OwnerUserId AS FollowUpOwnerId, FollowUpOwners.EnglishName AS FollowUpOwner,
FollowUpTypes.FollowUpEnglishName AS FollowUpType, dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate,
FromPortCountries.Code AS FromPortCountryCode,
ToPortCountries.Code AS ToPortCountryCode, MainCarriageFromAddresses.City AS MainCarriageFromCity, MainCarriageToAddresses.City AS MainCarriageToCity,
MainCarriageFromAddressCountries.Code AS MainCarriageFromCountryCode, MainCarriageToAddressCountries.Code AS MainCarriageToCountryCode,
dbo.Shipments.CASSCode, dbo.Shipments.SpecialServicesTypeId, dbo.SpecialServicesTypes.Code AS SpecialServicesTypeCode,
dbo.SpecialServicesTypes.EnglishName AS SpecialServicesTypeName,
ShipmentComputedFields.NumberOfHouses as NumberOfHouses
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageFromAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageToAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
--ELSE MainCarriageFinalDestinationPortsStates.EnglishName END) END AS MainCarriageToState
FROM            dbo.Shipments LEFT OUTER JOIN
dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId INNER JOIN
dbo.FollowUps ON dbo.Shipments.Id = dbo.FollowUps.ShipmentId LEFT OUTER JOIN
dbo.Ports AS MainCarriageFromPorts ON dbo.ShipmentMasterDatas.MainCarriageFromPortId = MainCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageToPorts ON dbo.ShipmentMasterDatas.MainCarriageToPortId = MainCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1FromPorts ON dbo.ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1ToPorts ON dbo.ShipmentMasterDatas.Transshipment1ToPortId = Transshipment1ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2FromPorts ON dbo.ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2ToPorts ON dbo.ShipmentMasterDatas.Transshipment2ToPortId = Transshipment2ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3FromPorts ON dbo.ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3ToPorts ON dbo.ShipmentMasterDatas.Transshipment3ToPortId = Transshipment3ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageFinalDestinationPorts ON
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageFromPorts ON dbo.Shipments.PreCarriageFromPortId = PreCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageToPorts ON dbo.Shipments.PreCarriageToPortId = PreCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageFromPorts ON dbo.Shipments.OnCarriageFromPortId = OnCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageToPorts ON dbo.Shipments.OnCarriageToPortId = OnCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Shipments.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Shipments.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Cards AS CustomerCards ON dbo.Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsolidatorCards ON dbo.Shipments.ConsolidatorId = ConsolidatorCards.Id LEFT OUTER JOIN
dbo.Cards AS FreightForwarderCards ON dbo.Shipments.FreightForwarderId = FreightForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Shipments.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Shipments.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentCards ON dbo.Shipments.AgentId = AgentCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentComputedCards ON dbo.Shipments.AgentComputed = AgentComputedCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentExportCards ON dbo.Shipments.CustomAgentExportId = CustomAgentExportCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentImportCards ON dbo.Shipments.CustomAgentImportId = CustomAgentImportCards.Id LEFT OUTER JOIN
dbo.Cards AS Notify1Cards ON dbo.Shipments.Notify1Id = Notify1Cards.Id LEFT OUTER JOIN
dbo.Cards AS Notify2Cards ON dbo.Shipments.Notify2Id = Notify2Cards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperNotExporterCards ON dbo.Shipments.ShipperNotExporterId = ShipperNotExporterCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeNotImporterCards ON dbo.Shipments.ConsigneeNotImporterId = ConsigneeNotImporterCards.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromCountries ON MainCarriageFromPorts.CountryId = MainCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToCountries ON MainCarriageToPorts.CountryId = MainCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1FromCountries ON Transshipment1FromPorts.CountryId = Transshipment1FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2FromCountries ON Transshipment2FromPorts.CountryId = Transshipment2FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3FromCountries ON Transshipment3FromPorts.CountryId = Transshipment3FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageFromCountries ON PreCarriageFromPorts.CountryId = PreCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageFromCountries ON OnCarriageFromPorts.CountryId = OnCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1ToCountries ON Transshipment1ToPorts.CountryId = Transshipment1ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2ToCountries ON Transshipment2ToPorts.CountryId = Transshipment2ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3ToCountries ON Transshipment3ToPorts.CountryId = Transshipment3ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageToCountries ON PreCarriageToPorts.CountryId = PreCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageToCountries ON OnCarriageToPorts.CountryId = OnCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarrierCards ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment1CarrierCards ON dbo.ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment2CarrierCards ON dbo.ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment3CarrierCards ON dbo.ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS PreCarriageCarrierCards ON dbo.Shipments.PreCarriageCarrierId = PreCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS OnCarriageCarrierCards ON dbo.Shipments.OnCarriageCarrierId = OnCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
dbo.MoveTypes ON dbo.Shipments.MoveTypeId = dbo.MoveTypes.Id LEFT OUTER JOIN
dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.CustomsTransmissionsStatus ON dbo.Shipments.LocalCustomsTransmissionsStatusCode = dbo.CustomsTransmissionsStatus.Code LEFT OUTER JOIN
dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
dbo.EventTypes ON dbo.Shipments.LastSharedEventId = dbo.EventTypes.Id LEFT OUTER JOIN
dbo.INTTRASIStatus ON dbo.Shipments.INTTRASIStatusCode = dbo.INTTRASIStatus.Code LEFT OUTER JOIN
dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
dbo.EventTypes AS FollowUpTypes ON dbo.FollowUps.EventTypeId = FollowUpTypes.Id LEFT OUTER JOIN
dbo.Contacts AS FollowUpOwners ON dbo.FollowUps.OwnerUserId = FollowUpOwners.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageFromAddresses ON dbo.ShipmentMasterDatas.MainCarriageFromAddressId = MainCarriageFromAddresses.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageToAddresses ON dbo.ShipmentMasterDatas.MainCarriageToAddressId = MainCarriageToAddresses.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageToPartners ON dbo.ShipmentMasterDatas.MainCarriageToPartnerId = MainCarriageToPartners.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageFromPartners ON dbo.ShipmentMasterDatas.MainCarriageFromPartnerId = MainCarriageFromPartners.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromAddressCountries ON MainCarriageFromAddresses.CountryId = MainCarriageFromAddressCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToAddressCountries ON MainCarriageToAddresses.CountryId = MainCarriageToAddressCountries.Id LEFT OUTER JOIN
dbo.SpecialServicesTypes ON dbo.Shipments.SpecialServicesTypeId = dbo.SpecialServicesTypes.Id INNER JOIN
dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromAddressesStates ON MainCarriageFromAddresses.StateId = MainCarriageFromAddressesStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageToAddressesStates ON MainCarriageToAddresses.StateId = MainCarriageToAddressesStates.Id LEFT OUTER JOIN
dbo.States AS FromPortsStates ON FromPorts.StateId = FromPortsStates.Id  LEFT OUTER JOIN
dbo.States AS ToPortsStates ON ToPorts.StateId = ToPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromPortsStates ON MainCarriageFromPorts.StateId = MainCarriageFromPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id');


-- Procedure Script From UpdateAllTenantsCustomersActualDataProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateAllTenantsCustomersActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData]
AS
declare @Tenant integer
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
From Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE usp_UpdateTenantCustomersActualData @Tenant
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END');


-- Procedure Script From UpdateCustomerActualDataProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCustomerActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCustomerActualData] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateCustomerActualData]
(
@CustomerId_PARAM varchar(15) = null,
@Tenant int
)
AS
-- Select the Firt day of the current date
-- in order to get date of the last month and bellow
declare @DateOfFirstDayOfCurrentDate as datetime
set @DateOfFirstDayOfCurrentDate = DATEADD(m, DATEDIFF(m, 0, GETDATE()), 0)
DECLARE @CustomerId AS varchar(15)
DECLARE @TypeCode AS varchar(2)
DECLARE @CountryId AS varchar(15)
DECLARE @Year AS int
DECLARE @Month AS int
DECLARE @TEU AS float
DECLARE @Revenue AS float
DECLARE @ChargeableWeight AS float
DECLARE @NumberOfShipments AS int
DECLARE @LastDate as datetime
declare @MemoryTable table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null,
CountryId varchar(15) null,
LastShipmentDate datetime not null
)
declare @MemoryTable_Customers table
(
Id varchar(15) not null
)
declare @MemoryTable_ActualData table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null
)
declare @MemoryTable_LocationActualData table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null,
CountryId varchar(15) not null
)
declare @MemoryTable_LastShipmentDate table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
LastShipmentDate datetime not null
)
declare @MemoryTable_CustomersLastShipmentDate table
(
CustomerId varchar(15) not null,
LastShipmentDate datetime not null
)
-- 1) Select Memory Data + Reset Actual Data
BEGIN
if (@CustomerId_PARAM is null)
BEGIN
BEGIN
insert into @MemoryTable
SELECT
CustomerId,
ProductCode,
Year(CreateDateTime),
Month(CreateDateTime),
sum(isnull(TEU,0)),
sum(ISNULL(OpenReceivablesInProfitCurrency,0) + ISNULL(AccountedReceivablesInProfitCurrency,0)),
sum(isnull(ChargeableWeightInKG,0)),
count(*),
CountryForStatisticsId,
max(CreateDateTime)
From Shipments
Where IsCancelled = 0 AND Tenant = @Tenant AND ProductCode is not null AND CustomerId is not null
group by CustomerId, ProductCode, Month(CreateDateTime), Year(CreateDateTime), CountryForStatisticsId
END
BEGIN
insert into @MemoryTable_Customers
SELECT
Id
From Customers
Where Tenant = @Tenant
END
BEGIN
update CustomerProductActualDatas
set
TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant
END
BEGIN
update CustomerProductLocationActualDatas
set
TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant
END
END
else
BEGIN
BEGIN
insert into @MemoryTable
SELECT
CustomerId,
ProductCode,
Year(CreateDateTime),
Month(CreateDateTime),
sum(isnull(TEU,0)),
sum(ISNULL(OpenReceivablesInProfitCurrency,0) + ISNULL(AccountedReceivablesInProfitCurrency,0)),
sum(isnull(ChargeableWeightInKG,0)),
count(*),
CountryForStatisticsId,
max(CreateDateTime)
From Shipments
Where IsCancelled = 0 AND Tenant = @Tenant AND ProductCode is not null AND CustomerId = @CustomerId_PARAM
group by CustomerId, ProductCode, Month(CreateDateTime), Year(CreateDateTime), CountryForStatisticsId
END
BEGIN
insert into @MemoryTable_Customers
SELECT
Id
From Customers
Where Tenant = @Tenant AND Id = @CustomerId_PARAM
END
BEGIN
update CustomerProductActualDatas
set TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant AND CustomerId = @CustomerId_PARAM
END
BEGIN
update CustomerProductLocationActualDatas
set TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant AND CustomerId = @CustomerId_PARAM
END
END
END
-- 2) Select Actual Data
BEGIN
insert into @MemoryTable_ActualData
SELECT
CustomerId,
ProductCode,
Year,
Month,
sum(isnull(TEU,0)),
sum(isnull(Revenue,0)),
sum(isnull(ChargeableWeight,0)),
sum(isnull(NumberOfShipments,0))
From @MemoryTable
where (DATEADD(year, Year-1900, DATEADD(month, Month-1, DATEADD(day, 20-1, 0)))) < @DateOfFirstDayOfCurrentDate
group by CustomerId, ProductCode, Month, Year
END
-- 3) Select Location Actual Data
BEGIN
insert into @MemoryTable_LocationActualData
SELECT
CustomerId,
ProductCode,
Year,
Month,
sum(isnull(TEU,0)),
sum(isnull(Revenue,0)),
sum(isnull(ChargeableWeight,0)),
sum(isnull(NumberOfShipments,0)),
CountryId
From @MemoryTable
Where CountryId is not null AND (DATEADD(year, Year-1900, DATEADD(month, Month-1, DATEADD(day, 20-1, 0)))) < @DateOfFirstDayOfCurrentDate
group by CustomerId, ProductCode, Month, Year, CountryId
END
-- 4) Select Last Date _ Customer Products
BEGIN
insert into @MemoryTable_LastShipmentDate
SELECT
CustomerId,
ProductCode,
max(LastShipmentDate)
From @MemoryTable
group by CustomerId, ProductCode
END
BEGIN
insert into @MemoryTable_CustomersLastShipmentDate
SELECT
CustomerId,
max(LastShipmentDate)
From @MemoryTable
group by CustomerId
END
-- 5) Update Product ActualDatas
BEGIN
DECLARE DataCursor1 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments
From @MemoryTable_ActualData
OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from @MemoryTable_Customers where Id = @CustomerId)
BEGIN
IF exists (
select * from CustomerProductActualDatas
where
Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
)
BEGIN
UPDATE CustomerProductActualDatas
set
TEU = isnull(@TEU,0),
Revenue = isnull(@Revenue,0),
ChargeableWeight =isnull(@ChargeableWeight,0),
NumberOfShipments = isnull(@NumberOfShipments,0)
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
END
else
BEGIN
INSERT INTO CustomerProductActualDatas(CustomerId, Tenant, ProductTypeCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments)
VALUES
(
@CustomerId,
@Tenant,
@TypeCode,
@Year,
@Month,
isnull(@TEU,0),
isnull(@Revenue,0),
isnull(@ChargeableWeight,0),
isnull(@NumberOfShipments,0)
)
END
if not exists (select * from CustomerProducts where CustomerId = @CustomerId AND Tenant = @Tenant AND ProductTypeCode = @TypeCode)
begin
insert into CustomerProducts(CustomerId, ProductTypeCode,Tenant) values(@CustomerId,@TypeCode,@Tenant)
end
END
FETCH NEXT FROM DataCursor1 INTO @CustomerId, @TypeCode,@Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments
END
CLOSE DataCursor1
DEALLOCATE DataCursor1
END
-- 6) Update Location ActualDatas
BEGIN
DECLARE DataCursor2 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments, CountryId
From @MemoryTable_LocationActualData
OPEN DataCursor2 FETCH NEXT FROM DataCursor2 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments, @CountryId
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from @MemoryTable_Customers where Id = @CustomerId)
BEGIN
IF EXISTS (
select * from CustomerProductLocationActualDatas
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
AND CountryId = @CountryId
)
BEGIN
UPDATE CustomerProductLocationActualDatas
set
TEU = isnull(@TEU,0),
Revenue = isnull(@Revenue,0),
ChargeableWeight =isnull(@ChargeableWeight,0),
NumberOfShipments = isnull(@NumberOfShipments,0)
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
AND CountryId = @CountryId
END
else
BEGIN
INSERT INTO CustomerProductLocationActualDatas(CustomerId, Tenant, ProductTypeCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments,CountryId)
VALUES
(
@CustomerId,
@Tenant,
@TypeCode,
@Year,
@Month,
isnull(@TEU,0),
isnull(@Revenue,0),
isnull(@ChargeableWeight,0),
isnull(@NumberOfShipments,0),
@CountryId
)
END
if not exists (select * from CustomerProductLocations where CustomerId = @CustomerId and ProductTypeCode = @TypeCode and CountryId  = @CountryId and Tenant = @Tenant)
begin
insert into CustomerProductLocations(CountryId, CustomerId, ProductTypeCode, Tenant) values(@CountryId, @CustomerId, @TypeCode,@Tenant)
end
END
FETCH NEXT FROM DataCursor2 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments, @CountryId
END
CLOSE DataCursor2
DEALLOCATE DataCursor2
END
-- 7) Update Products Last Shipment Date
BEGIN
DECLARE DataCursor3 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, LastShipmentDate
From @MemoryTable_LastShipmentDate
OPEN DataCursor3 FETCH NEXT FROM DataCursor3 INTO @CustomerId, @TypeCode, @LastDate
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from @MemoryTable_Customers where Id = @CustomerId)
BEGIN
update CustomerProducts
set LastShipmentDate = @LastDate
where Tenant = @Tenant
and CustomerId = @CustomerId
and ProductTypeCode = @TypeCode
END
FETCH NEXT FROM DataCursor3 INTO @CustomerId, @TypeCode, @LastDate
END
CLOSE DataCursor3
DEALLOCATE DataCursor3
END
-- 8) Update Customers Last Shipment Date
BEGIN
DECLARE DataCursor4 CURSOR READ_ONLY
FOR
SELECT CustomerId, LastShipmentDate
From @MemoryTable_CustomersLastShipmentDate
OPEN DataCursor4 FETCH NEXT FROM DataCursor4 INTO @CustomerId, @LastDate
WHILE @@FETCH_STATUS = 0
BEGIN
update Customers
set LastShipmentDate = @LastDate
where Tenant = @Tenant
and Id = @CustomerId
FETCH NEXT FROM DataCursor4 INTO @CustomerId, @LastDate
END
CLOSE DataCursor4
DEALLOCATE DataCursor4
END');


-- Procedure Script From UpdateTenantCustomersActualDataProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateTenantCustomersActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData]
(
@Tenant int
)
AS
declare @StartDateTime as datetime
declare @EndDateTime as datetime
if @Tenant is not null
BEGIN
declare @CustomerId varchar(15)
set @CustomerId = null
if not exists (select * from CustomerActualDataHistory where Tenant = @Tenant and CONVERT(date,StartDateTime) = CONVERT(date,getdate()))
begin
set @StartDateTime = getdate()
EXECUTE usp_UpdateCustomerActualData @CustomerId, @Tenant
set @EndDateTime = getdate()
insert into CustomerActualDataHistory(Tenant, StartDateTime, EndDateTime)
values(@Tenant,	@StartDateTime,	@EndDateTime)
end
END');


-- Procedure Script From ComputeShipmentFirstApprovalDateProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeShipmentFirstApprovalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeShipmentFirstApprovalDate] END');
EXEC('CREATE PROCEDURE [dbo].[usp_ComputeShipmentFirstApprovalDate]
(
@ShipmentId varchar(15)
)
AS
if (@ShipmentId is not null)
BEGIN
declare @Tenant as int
declare @HouseId as varchar(15)
declare @MasterDataId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @FirstApprovalDate_Master as DateTime
declare @ARInvoiceId as varchar(15)
declare @ApprovedDate as DateTime
declare @IsConstituent as bit
declare @StatusCode as varchar(2)
declare @ConsolidationInvoiceId as varchar(15)
declare @FirstApprovalDate as DateTime
select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentLevelCode = ShipmentLevelCode
from Shipments where Id = @ShipmentId
set @FirstApprovalDate = null
BEGIN
DECLARE ARInvoiceEntitiesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.ApprovedDate, ARInvoices.IsConstituentInvoice, ARInvoices.StatusCode, ARInvoices.ConsolidationInvoiceId
FROM ARInvoiceEntities
JOIN ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
WHERE ARInvoiceEntities.EntityId = @ShipmentId
AND ARInvoiceEntities.Tenant = @Tenant
AND ARInvoices.Tenant = @Tenant
AND ARInvoices.IsAutoCredit = 0
AND ARInvoices.StatusCode != ''DR''
AND ARInvoices.StatusCode != ''LL''
AND ARInvoices.StatusCode != ''VD''
AND ARInvoices.StatusCode != ''NT''
AND ARInvoices.StatusCode != ''AC''
OPEN ARInvoiceEntitiesCursor FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
WHILE @@FETCH_STATUS = 0
BEGIN
if (@IsConstituent = 1 AND @StatusCode = ''CN'' AND @ConsolidationInvoiceId is not null)
BEGIN
set @ApprovedDate = (select ApprovedDate from ARInvoices
where IsConsolidationInvoice = 1
AND Id = @ConsolidationInvoiceId
AND IsAutoCredit = 0
AND StatusCode != ''DR''
AND StatusCode != ''LL''
AND StatusCode != ''VD''
AND StatusCode != ''AC''
)
END
if (@ApprovedDate is not null)
BEGIN
if (@FirstApprovalDate is null)
set @FirstApprovalDate = @ApprovedDate
else if (@FirstApprovalDate > @ApprovedDate)
set @FirstApprovalDate = @ApprovedDate
END
FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
END
CLOSE ARInvoiceEntitiesCursor
DEALLOCATE ARInvoiceEntitiesCursor
END
if (@ShipmentLevelCode = ''H'' AND @MasterDataId is not null)
BEGIN
if exists (select * from ShipmentMasterDatas where Id = @MasterDataId AND ProrateReceivables = 1)
begin
set @FirstApprovalDate_Master = (select FirstARInvoiceApprovalDate from Shipments where Id = @MasterDataId AND Tenant = @Tenant)
if (@FirstApprovalDate_Master is not null)
begin
if (@FirstApprovalDate is null)
set @FirstApprovalDate = @FirstApprovalDate_Master
else if (@FirstApprovalDate > @FirstApprovalDate_Master)
set @FirstApprovalDate = @FirstApprovalDate_Master
end
end
END
update Shipments set FirstARInvoiceApprovalDate = @FirstApprovalDate where Id = @ShipmentId AND Tenant = @Tenant
if (@ShipmentLevelCode = ''C'')
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE [usp_ComputeShipmentFirstApprovalDate] @HouseId
FETCH NEXT FROM ShipmentsCursor INTO @HouseId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END
END');


-- Procedure Script From UpdateConstituentShipmentProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateConstituentShipment]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateConstituentShipment] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateConstituentShipment]
(
@ConstituentId varchar(15),
@ConsolidationId varchar(15)
)
AS
declare @ConsolidationStatus as varchar(2)
select @ConsolidationStatus = StatusCode from ARInvoices where Id = @ConsolidationId
if (@ConsolidationStatus = ''AD'')
BEGIN
update ShipmentReceivables set ShipmentReceivableLineStatusCode = ''ACCT'' where ARInvoiceId = @ConstituentId
END
ELSE
BEGIN
update ShipmentReceivables set ShipmentReceivableLineStatusCode = ''OAMT'' where ARInvoiceId = @ConstituentId
END');


-- Procedure Script From UpdatePayablesDataProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdatePayablesData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdatePayablesData] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdatePayablesData]
(
@ShipmentId_PARAM varchar(15),
@IsInvoiceUpdated_PARAM  bit
)
AS
if exists (select * from Shipments where Id = @ShipmentId_PARAM and ShipmentLevelCode != ''D'')
BEGIN
declare @Tenant as int
declare @MasterId as varchar(15)
select @Tenant = Tenant, @MasterId = MasterShipmentDataId from Shipments where Id = @ShipmentId_PARAM
if (@MasterId is not null)
BEGIN
-- Global Variables
BEGIN
-- 01
declare @GRWT_Id as varchar(15)
declare @CHWT_Id as varchar(15)
declare @FIXD_Id as varchar(15)
declare @VOLU_Id as varchar(15)
declare @BTEU_Id as varchar(15)
declare @GWTN_Id as varchar(15)
declare @PRVL_Id as varchar(15)
declare @PRFR_Id as varchar(15)
declare @QTY_Id as varchar(15)
declare @GWKG_Id as varchar(15)
declare @CWKG_Id as varchar(15)
declare @VCBM_Id as varchar(15)
declare @SCGW_Id as varchar(15)
set @GRWT_Id = (select Id from Measurements where Code = ''GRWT'' AND Tenant = @Tenant)
set @CHWT_Id = (select Id from Measurements where Code = ''CHWT'' AND Tenant = @Tenant)
set @FIXD_Id = (select Id from Measurements where Code = ''FIXD'' AND Tenant = @Tenant)
set @VOLU_Id = (select Id from Measurements where Code = ''VOLU'' AND Tenant = @Tenant)
set @BTEU_Id = (select Id from Measurements where Code = ''BTEU'' AND Tenant = @Tenant)
set @GWTN_Id = (select Id from Measurements where Code = ''GWTN'' AND Tenant = @Tenant)
set @PRVL_Id = (select Id from Measurements where Code = ''PRVL'' AND Tenant = @Tenant)
set @PRFR_Id = (select Id from Measurements where Code = ''PRFR'' AND Tenant = @Tenant)
set @QTY_Id = (select Id from Measurements where Code = ''QTY'' AND Tenant = @Tenant)
set @GWKG_Id = (select Id from Measurements where Code = ''GWKG'' AND Tenant = @Tenant)
set @CWKG_Id = (select Id from Measurements where Code = ''CWKG'' AND Tenant = @Tenant)
set @VCBM_Id = (select Id from Measurements where Code = ''VCBM'' AND Tenant = @Tenant)
set @SCGW_Id = (select Id from Measurements where Code = ''SCGW'' AND Tenant = @Tenant)
-- 02
declare @AllHousesCount as float
declare @AllHousesTotalTEU as float
declare @AllHousesTotalVolume as float
declare @AllHousesTotalGrossWeight as float
declare @AllHousesTotalVolumetrics as float
declare @AllHousesTotalChargeables as float
declare @AllHousesTotalGrossWeightPerTon as float
declare @AllHousesTotalNumberOfPackages as float
declare @AllHousesTotalNumberOfContainers as float
declare @AllHousesTotalGrossWeightInKG as float
declare @AllHousesTotalChargeableWeightInKG as float
declare @AllHousesTotalVolumeInCBM as float
declare @AllHousesGrossWeightPerStorageDays as float;
if exists (select * from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId)
begin
select
@AllHousesCount = count(*),
@AllHousesTotalTEU = sum(isnull(TEU,0)),
@AllHousesTotalVolume = sum(isnull(Volume,0)),
@AllHousesTotalGrossWeight = sum(isnull(GrossWeight,0)),
@AllHousesTotalVolumetrics = sum(isnull(VolumetricWeight,0)),
@AllHousesTotalChargeables = sum(isnull(ChargeableWeight,0)),
@AllHousesTotalGrossWeightPerTon = sum(isnull(GrossWeightPerTon,0)),
@AllHousesTotalNumberOfPackages = sum(isnull(NumberOfPackages,0)),
@AllHousesTotalNumberOfContainers = sum(isnull(NumberOfContainers,0)),
@AllHousesTotalGrossWeightInKG = sum(isnull(GrossWeightInKG,0)),
@AllHousesTotalChargeableWeightInKG = sum(isnull(ChargeableWeightInKG,0)),
@AllHousesTotalVolumeInCBM = sum(isnull(VolumeInCBM,0)),
@AllHousesGrossWeightPerStorageDays = sum(isnull(GrossWeightPerStorageDays,0))
from Shipments
where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId
end
else
begin
set @AllHousesTotalTEU = 0
set @AllHousesTotalVolume = 0
set @AllHousesTotalGrossWeight = 0
set @AllHousesTotalVolumetrics = 0
set @AllHousesTotalChargeables = 0
set @AllHousesTotalGrossWeightPerTon = 0
set @AllHousesTotalNumberOfPackages = 0
set @AllHousesTotalNumberOfContainers = 0
set @AllHousesTotalGrossWeightInKG = 0
set @AllHousesTotalChargeableWeightInKG = 0
set @AllHousesTotalVolumeInCBM = 0
set @AllHousesGrossWeightPerStorageDays = 0
end
-- 03
declare @MasterTypeId as varchar(5)
declare @MasterTransportModeId as varchar(1)
select
@MasterTypeId = ShipmentTypeId,
@MasterTransportModeId = TransportModeId
from Shipments where Tenant = @Tenant and Id = @MasterId
END
-- Master Payable Variables
BEGIN
declare @MasterPayableId as varchar(15)
declare @MasterPayableQuantity as float
declare @MasterPayableUnitPrice as float
declare @MasterPayableVendorId as varchar(15)
declare @MasterPayableChargesTypeId as varchar(15)
declare @MasterPayableMeasurementId as varchar(15)
declare @MasterPayablePrepaidCollectId as varchar(1)
declare @MasterPayableDueTypeCode as varchar(2)
declare @MasterPayableIATACodeId as varchar(15)
declare @MasterPayableAWBPrint as int
declare @MasterPayableCurrencyId as varchar(15)
declare @MasterPayableRate as float
declare @MasterPayableProfitRate as float
declare @MasterPayableLineStatusCode as varchar(4)
declare @MasterPayableAmountTypeCode as varchar(4)
declare @MasterPayableCreatedByUserId as varchar(15)
declare @MasterPayableUpdatedByUserId as varchar(15)
declare @MasterPayableCreateDate as datetime
declare @MasterPayableUpdateDate as datetime
declare @MasterPayableValueDate as datetime
declare @MasterPayablePackageTypeId as varchar(15)
declare @MasterPayableOpenAmount as float
declare @MasterPayableExpectedAmount as float
declare @MasterPayableAccountedAmount as float
END
-- House Payable Variables
BEGIN
declare @IsCreatingPayable as bit
declare @IsUpdatingPayable as bit
declare @NewId as varchar(15)
declare @HouseId as varchar(15)
declare @HouseTEU as float
declare @HouseVolume as float
declare @HouseGrossWeight as float
declare @HouseChargeableWeight as float
declare @HouseGrossWeightPerTon as float
declare @HouseValueOfGoods as float
declare @HouseFreightAmount as float
declare @HouseGrossWeightInKG as float
declare @HouseChargeableWeightInKG as float
declare @HouseVolumeInCBM as float
declare @HouseGrossWeightPerStorageDays as float
declare @HousePayableId as varchar(15)
declare @HousePayableMeasurementId as varchar(15)
declare @HousePayableLineStatusCode as varchar(4)
declare @HouseNumberOfPackages as float
declare @HouseNumberOfContainers as float
declare @HouseTypeId as varchar(5)
declare @HouseTransportModeId as varchar(1)
declare @Ratio as float
declare @Quantity as float
declare @UnitPrice as float
declare @ExpectedAmount as float
declare @ExpectedAmountLocal as float
declare @ExpectedAmountInProfitCurrency as float
declare @AccountedAmount as float
declare @AccountedAmountInLocalCurrency as float
declare @AccountedAmountInProfitCurrency as float
declare @OpenAmount as float
declare @OpenAmountInLocalCurrency as float
declare @OpenAmountInProfitCurrency as float
declare @ExpectedAmountRatio as float
END
-- Loop Master Payables (1: Not PRFR)
BEGIN
DECLARE MasterPayables1Cursor CURSOR READ_ONLY
FOR
SELECT Id, Quantity, UnitPrice, VendorId, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentPayableLineStatusCode, ShipmentPayableAmountTypeCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, ValueDate, OpenAmount, AccountedAmount, ExpectedAmount, IATACodeId
FROM ShipmentPayables
WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND (MeasurementId != @PRFR_Id OR MeasurementId is null)
OPEN MasterPayables1Cursor FETCH NEXT FROM MasterPayables1Cursor INTO @MasterPayableId, @MasterPayableQuantity, @MasterPayableUnitPrice, @MasterPayableVendorId, @MasterPayableChargesTypeId, @MasterPayableMeasurementId, @MasterPayablePrepaidCollectId, @MasterPayableDueTypeCode, @MasterPayableAWBPrint, @MasterPayableCurrencyId, @MasterPayableRate, @MasterPayableProfitRate, @MasterPayableLineStatusCode, @MasterPayableAmountTypeCode, @MasterPayableCreatedByUserId, @MasterPayableUpdatedByUserId, @MasterPayableCreateDate, @MasterPayableUpdateDate, @MasterPayableValueDate, @MasterPayableOpenAmount, @MasterPayableAccountedAmount, @MasterPayableExpectedAmount, @MasterPayableIATACodeId
WHILE @@FETCH_STATUS = 0
BEGIN
-- Loop Houses
BEGIN
DECLARE Houses1Cursor CURSOR READ_ONLY
FOR
SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods, NumberOfPackages, NumberOfContainers, TransportModeId, ShipmentTypeId, GrossWeightInKG, ChargeableWeightInKG, VolumeInCBM, GrossWeightPerStorageDays
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
OPEN Houses1Cursor FETCH NEXT FROM Houses1Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId,@HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsCreatingPayable = 0
set @HousePayableMeasurementId = @MasterPayableMeasurementId
-- IsCreatingPayable
BEGIN
if (@MasterPayableAmountTypeCode = ''NEXP'')
BEGIN
if not exists (select * from ShipmentPayables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentPayableParentId = @MasterPayableId and ChargesTypeId = @MasterPayableChargesTypeId)
set @IsCreatingPayable = 1
END
else if (@MasterPayableMeasurementId = @FIXD_Id
OR @MasterPayableMeasurementId = @BTEU_Id
OR @MasterPayableMeasurementId = @VOLU_Id
OR @MasterPayableMeasurementId = @GRWT_Id
OR @MasterPayableMeasurementId = @CHWT_Id
OR @MasterPayableMeasurementId = @GWTN_Id
OR @MasterPayableMeasurementId = @PRVL_Id
OR @MasterPayableMeasurementId = @QTY_Id
OR @MasterPayableMeasurementId = @GWKG_Id
OR @MasterPayableMeasurementId = @CWKG_Id
OR @MasterPayableMeasurementId = @VCBM_Id
OR @MasterPayableMeasurementId = @SCGW_Id
)
BEGIN
if not exists (select * from ShipmentPayables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentPayableParentId = @MasterPayableId and ChargesTypeId = @MasterPayableChargesTypeId)
set @IsCreatingPayable = 1
END
-- If Master Is FCL | FTL
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''FCLD'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''FTL''))
BEGIN
set @MasterPayablePackageTypeId = (select Id from PackageTypes where MeasurementId = @MasterPayableMeasurementId AND Tenant = @Tenant)
if exists (select * from ShipmentPackages where Tenant = @Tenant AND ShipmentId = @HouseId AND PackageTypeId = @MasterPayablePackageTypeId)
begin
if not exists (select Id from ShipmentPayables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentPayableParentId = @MasterPayableId AND ChargesTypeId = @MasterPayableChargesTypeId AND MeasurementId = @MasterPayableMeasurementId)
set @IsCreatingPayable = 1
end
END
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''MyGO'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''MyGI''))
BEGIN
if not exists (select Id from ShipmentPayables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentPayableParentId = @MasterPayableId AND ChargesTypeId = @MasterPayableChargesTypeId AND MeasurementId = @CHWT_Id)
set @IsCreatingPayable = 1
set @HousePayableMeasurementId = @CHWT_Id
END
if (@IsCreatingPayable = 1)
BEGIN
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN T1;
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''ShipmentPayable''
COMMIT TRAN T1;
INSERT INTO ShipmentPayables
(
Id,
Tenant,
ShipmentId,
ShipmentPayableParentId,
ShipmentPayableLineStatusCode,
ShipmentPayableAmountTypeCode,
VendorId,
ChargesTypeId,
MeasurementId,
PrepaidCollectId,
DueTypeCode,
AWBPrint,
CurrencyId,
Rate,
ProfitCurrencyExchangeRate,
ValueDate,
CreateDate,
UpdateDate,
CreatedByUserId,
UpdateByUserId,
IsFromQuote,
IsEditedByUser,
IATACodeId
)
VALUES
(
@NewId,
@Tenant,
@HouseId,
@MasterPayableId,
@MasterPayableLineStatusCode,
@MasterPayableAmountTypeCode,
@MasterPayableVendorId,
@MasterPayableChargesTypeId,
@HousePayableMeasurementId,
@MasterPayablePrepaidCollectId,
@MasterPayableDueTypeCode,
@MasterPayableAWBPrint,
@MasterPayableCurrencyId,
@MasterPayableRate,
@MasterPayableProfitRate,
@MasterPayableValueDate,
@MasterPayableCreateDate,
@MasterPayableUpdateDate,
@MasterPayableCreatedByUserId,
@MasterPayableUpdatedByUserId,
0,
0,
@MasterPayableIATACodeId
)
END
END
-- Loop House Payables / Amount Calculating & Updating
BEGIN
DECLARE HousePayables1Cursor CURSOR READ_ONLY
FOR
SELECT Id, ShipmentPayableLineStatusCode
FROM ShipmentPayables
WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentPayableParentId = @MasterPayableId
OPEN HousePayables1Cursor FETCH NEXT FROM HousePayables1Cursor INTO @HousePayableId, @HousePayableLineStatusCode
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsUpdatingPayable = 0
if (@IsCreatingPayable = 1)
begin
set @IsUpdatingPayable = 1
end
else if (@IsInvoiceUpdated_PARAM = 1)
begin
set @IsUpdatingPayable = 1
end
else
begin
if (@HousePayableLineStatusCode in (''ACCT'' , ''PACC'') AND @MasterPayableLineStatusCode in (''ACCT'' , ''PACC''))
begin
set @IsUpdatingPayable = 0
end
else
begin
set @IsUpdatingPayable = 1
end
end
--set @IsUpdatingPayable = 1
if (@IsUpdatingPayable = 1)
BEGIN
-- Reset Variables
BEGIN
set @Ratio = 0
set @Quantity = 0
set @UnitPrice = 0
set @ExpectedAmount = 0
set @ExpectedAmountLocal = 0
set @ExpectedAmountInProfitCurrency = 0
set @AccountedAmount = 0
set @AccountedAmountInLocalCurrency = 0
set @AccountedAmountInProfitCurrency = 0
set @OpenAmount = 0
set @OpenAmountInLocalCurrency = 0
set @OpenAmountInProfitCurrency = 0
END
-- Compute Ration, Quantity, UnitPrice
BEGIN
if (@MasterPayableAmountTypeCode = ''NEXP'')
BEGIN
set @Ratio = @MasterPayableAccountedAmount / @AllHousesCount
set @Quantity = 0
set @UnitPrice = 0
set @AccountedAmount = @Ratio
END
-- Fixed
else if (@MasterPayableMeasurementId = @FIXD_Id)
BEGIN
set @Ratio = @MasterPayableQuantity / @AllHousesCount
set @Quantity = 1
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- By TEU
else if (@MasterPayableMeasurementId = @BTEU_Id)
BEGIN
if (@AllHousesTotalTEU <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalTEU
end
set @Quantity = @HouseTEU
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- Volume
else if (@MasterPayableMeasurementId = @VOLU_Id)
BEGIN
if (@AllHousesTotalVolume <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalVolume
end
set @Quantity = @HouseVolume
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- Gross Weight
else if (@MasterPayableMeasurementId = @GRWT_Id)
BEGIN
if (@AllHousesTotalGrossWeight <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalGrossWeight
end
set @Quantity = @HouseGrossWeight
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- Chargeable Weight
else if (@MasterPayableMeasurementId = @CHWT_Id)
BEGIN
if (@AllHousesTotalChargeables <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalChargeables
end
set @Quantity = @HouseChargeableWeight
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- Gross Weight Per Ton
else if (@MasterPayableMeasurementId = @GWTN_Id)
BEGIN
if (@AllHousesTotalGrossWeightPerTon <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalGrossWeightPerTon
end
set @Quantity = @HouseGrossWeightPerTon
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- GrossWeightPerStorageDays
else if (@MasterPayableMeasurementId = @SCGW_Id)
BEGIN
if (@AllHousesGrossWeightPerStorageDays <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesGrossWeightPerStorageDays
end
set @Quantity = @HouseGrossWeightPerStorageDays
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- GWKG: Gross Weight in Kg
else if (@MasterPayableMeasurementId = @GWKG_Id)
BEGIN
if (@AllHousesTotalGrossWeightInKG <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalGrossWeightInKG
end
set @Quantity = @HouseGrossWeightInKG
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- CWKG: Chargeable Weight in Kg
else if (@MasterPayableMeasurementId = @CWKG_Id)
BEGIN
if (@AllHousesTotalChargeableWeightInKG <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalChargeableWeightInKG
end
set @Quantity = @HouseChargeableWeightInKG
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- VCBM: Volume in CBM
else if (@MasterPayableMeasurementId = @VCBM_Id)
BEGIN
if (@AllHousesTotalVolumeInCBM <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalVolumeInCBM
end
set @Quantity = @HouseVolumeInCBM
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
-- Percent of Value
else if (@MasterPayableMeasurementId = @PRVL_Id)
BEGIN
set @Quantity = @HouseValueOfGoods
set @UnitPrice = @MasterPayableUnitPrice
END
-- Quantity
else if (@MasterPayableMeasurementId = @QTY_Id)
BEGIN
if(@HouseTransportModeId = ''A'' OR (@HouseTransportModeId = ''O'' AND @HouseTypeId = ''LCLD'') OR (@HouseTransportModeId = ''I'' AND @HouseTypeId = ''LTL''))
begin
if (@AllHousesTotalNumberOfPackages <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalNumberOfPackages
end
set @Quantity = @HouseNumberOfPackages
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
end
else
begin
if (@AllHousesTotalNumberOfContainers <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalNumberOfContainers
end
set @Quantity = @HouseNumberOfContainers
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
end
END
----------
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''FCLD'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''FTL''))
BEGIN
set @Quantity = (select COUNT(Id) from ShipmentPackages where ShipmentId = @HouseId AND PackageTypeId = @MasterPayablePackageTypeId)
set @UnitPrice = @MasterPayableUnitPrice
END
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''MyGO'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''MyGI''))
BEGIN
if (@AllHousesTotalChargeables <> 0)
begin
set @Ratio = @MasterPayableQuantity / @AllHousesTotalChargeables
end
set @Quantity = @HouseChargeableWeight
set @UnitPrice = @Ratio * @MasterPayableUnitPrice
END
END
-- Compute Amounts
BEGIN
set @ExpectedAmount = @Quantity * @UnitPrice
if (@MasterPayableMeasurementId = @PRVL_Id)
BEGIN
set @ExpectedAmount = @Quantity * @UnitPrice / 100
END
if (@MasterPayableAmountTypeCode = ''NEXP'')
begin
set @ExpectedAmount = null
set @OpenAmount = null
end
else if (@MasterPayableLineStatusCode = ''ACCT'' OR @MasterPayableLineStatusCode = ''PACC'')
begin
set @ExpectedAmountRatio = @ExpectedAmount / @MasterPayableExpectedAmount
set @OpenAmount = @MasterPayableOpenAmount * @ExpectedAmountRatio --/ @AllHousesCount
set @AccountedAmount = @MasterPayableAccountedAmount * @ExpectedAmountRatio --/ @AllHousesCount
end
else
begin
set @OpenAmount = @ExpectedAmount
set @AccountedAmount = 0
end
set @ExpectedAmountLocal = @ExpectedAmount * @MasterPayableRate
set @AccountedAmountInLocalCurrency = @AccountedAmount * @MasterPayableRate
set @OpenAmountInLocalCurrency = @OpenAmount * @MasterPayableRate
set @ExpectedAmountInProfitCurrency = @ExpectedAmountLocal / @MasterPayableProfitRate
set @AccountedAmountInProfitCurrency= @AccountedAmountInLocalCurrency / @MasterPayableProfitRate
set @OpenAmountInProfitCurrency = @OpenAmountInLocalCurrency / @MasterPayableProfitRate
END
-- Update Payable
BEGIN
update ShipmentPayables
set
VendorId = @MasterPayableVendorId,
MeasurementId = @HousePayableMeasurementId,
PrepaidCollectId = @MasterPayablePrepaidCollectId,
DueTypeCode = @MasterPayableDueTypeCode,
--AWBPrint = 0, --@MasterPayableAWBPrint,
ValueDate = @MasterPayableValueDate,
UpdateDate = @MasterPayableUpdateDate,
UpdateByUserId = @MasterPayableUpdatedByUserId,
ShipmentPayableLineStatusCode = @MasterPayableLineStatusCode,
CurrencyId = @MasterPayableCurrencyId,
Rate = @MasterPayableRate,
ProfitCurrencyExchangeRate = @MasterPayableProfitRate,
Quantity = isnull(ROUND(@Quantity,3),0),
UnitPrice = isnull(Round(@UnitPrice,3),0),
ExpectedAmount = isnull(Round(@ExpectedAmount,3),0),
ExpectedAmountLocal = isnull(Round(@ExpectedAmountLocal,3),0),
ExpectedAmountInProfitCurrency = isnull(Round(@ExpectedAmountInProfitCurrency,3),0),
AccountedAmount = isnull(Round(@AccountedAmount,3),0),
AccountedAmountInLocalCurrency = isnull(Round(@AccountedAmountInLocalCurrency,3),0),
AccountedAmountInProfitCurrency = isnull(Round(@AccountedAmountInProfitCurrency,3),0),
OpenAmount = isnull(Round(@OpenAmount,3),0),
OpenAmountInLocalCurrency = isnull(Round(@OpenAmountInLocalCurrency,3),0),
OpenAmountInProfitCurrency = isnull(Round(@OpenAmountInProfitCurrency,3),0),
IATACodeId = @MasterPayableIATACodeId
where Id = @HousePayableId and Tenant = @Tenant
END
END
FETCH NEXT FROM HousePayables1Cursor INTO @HousePayableId, @HousePayableLineStatusCode
END
CLOSE HousePayables1Cursor
DEALLOCATE HousePayables1Cursor
END
FETCH NEXT FROM Houses1Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId, @HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
END
CLOSE Houses1Cursor
DEALLOCATE Houses1Cursor
END
FETCH NEXT FROM MasterPayables1Cursor INTO @MasterPayableId, @MasterPayableQuantity, @MasterPayableUnitPrice, @MasterPayableVendorId, @MasterPayableChargesTypeId, @MasterPayableMeasurementId, @MasterPayablePrepaidCollectId, @MasterPayableDueTypeCode, @MasterPayableAWBPrint, @MasterPayableCurrencyId, @MasterPayableRate, @MasterPayableProfitRate, @MasterPayableLineStatusCode, @MasterPayableAmountTypeCode, @MasterPayableCreatedByUserId, @MasterPayableUpdatedByUserId, @MasterPayableCreateDate, @MasterPayableUpdateDate, @MasterPayableValueDate, @MasterPayableOpenAmount, @MasterPayableAccountedAmount, @MasterPayableExpectedAmount, @MasterPayableIATACodeId
END
CLOSE MasterPayables1Cursor
DEALLOCATE MasterPayables1Cursor
END
-- Loop Master Payables (2: PRFR)
BEGIN
DECLARE MasterPayables2Cursor CURSOR READ_ONLY
FOR
SELECT Id, Quantity, UnitPrice, VendorId, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentPayableLineStatusCode, ShipmentPayableAmountTypeCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, ValueDate, OpenAmount, AccountedAmount, ExpectedAmount, IATACodeId
FROM ShipmentPayables
WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND MeasurementId = @PRFR_Id
OPEN MasterPayables2Cursor FETCH NEXT FROM MasterPayables2Cursor INTO @MasterPayableId, @MasterPayableQuantity, @MasterPayableUnitPrice, @MasterPayableVendorId, @MasterPayableChargesTypeId, @MasterPayableMeasurementId, @MasterPayablePrepaidCollectId, @MasterPayableDueTypeCode, @MasterPayableAWBPrint, @MasterPayableCurrencyId, @MasterPayableRate, @MasterPayableProfitRate, @MasterPayableLineStatusCode, @MasterPayableAmountTypeCode, @MasterPayableCreatedByUserId, @MasterPayableUpdatedByUserId, @MasterPayableCreateDate, @MasterPayableUpdateDate, @MasterPayableValueDate, @MasterPayableOpenAmount, @MasterPayableAccountedAmount, @MasterPayableExpectedAmount, @MasterPayableIATACodeId
WHILE @@FETCH_STATUS = 0
BEGIN
-- Loop Houses
BEGIN
DECLARE Houses2Cursor CURSOR READ_ONLY
FOR
SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
OPEN Houses2Cursor FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsCreatingPayable = 0
set @HousePayableMeasurementId = @MasterPayableMeasurementId
set @HouseFreightAmount = (select sum(isnull(ExpectedAmount,0)) from ShipmentPayables where ShipmentId = @HouseId AND ShipmentPayableParentId is not null AND ChargesTypeId in (select Id from ChargesTypes where ChargesGroupCode = ''FRT'' AND Tenant = @Tenant))
-- IsCreatingPayable
BEGIN
if (@MasterPayableAmountTypeCode = ''NEXP'')
BEGIN
if not exists (select * from ShipmentPayables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentPayableParentId = @MasterPayableId and ChargesTypeId = @MasterPayableChargesTypeId)
set @IsCreatingPayable = 1
END
else if (@MasterPayableMeasurementId = @PRFR_Id)
BEGIN
if not exists (select * from ShipmentPayables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentPayableParentId = @MasterPayableId and ChargesTypeId = @MasterPayableChargesTypeId)
set @IsCreatingPayable = 1
END
if (@IsCreatingPayable = 1)
BEGIN
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN T1;
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''ShipmentPayable''
COMMIT TRAN T1;
INSERT INTO ShipmentPayables
(
Id,
Tenant,
ShipmentId,
ShipmentPayableParentId,
ShipmentPayableLineStatusCode,
ShipmentPayableAmountTypeCode,
VendorId,
ChargesTypeId,
MeasurementId,
PrepaidCollectId,
DueTypeCode,
AWBPrint,
CurrencyId,
Rate,
ProfitCurrencyExchangeRate,
ValueDate,
CreateDate,
UpdateDate,
CreatedByUserId,
UpdateByUserId,
IsFromQuote,
IsEditedByUser,
IATACodeId
)
VALUES
(
@NewId,
@Tenant,
@HouseId,
@MasterPayableId,
@MasterPayableLineStatusCode,
@MasterPayableAmountTypeCode,
@MasterPayableVendorId,
@MasterPayableChargesTypeId,
@HousePayableMeasurementId,
@MasterPayablePrepaidCollectId,
@MasterPayableDueTypeCode,
@MasterPayableAWBPrint,
@MasterPayableCurrencyId,
@MasterPayableRate,
@MasterPayableProfitRate,
@MasterPayableValueDate,
@MasterPayableCreateDate,
@MasterPayableUpdateDate,
@MasterPayableCreatedByUserId,
@MasterPayableUpdatedByUserId,
0,
0,
@MasterPayableIATACodeId
)
END
END
-- Loop House Payables / Amount Calculating & Updating
BEGIN
DECLARE HousePayables2Cursor CURSOR READ_ONLY
FOR
SELECT Id, ShipmentPayableLineStatusCode
FROM ShipmentPayables
WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentPayableParentId = @MasterPayableId --AND ShipmentPayableLineStatusCode not in (''ACCT'' , ''PACC'')
OPEN HousePayables2Cursor FETCH NEXT FROM HousePayables2Cursor INTO @HousePayableId, @HousePayableLineStatusCode
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsUpdatingPayable = 0
if (@IsCreatingPayable = 1)
begin
set @IsUpdatingPayable = 1
end
else if (@IsInvoiceUpdated_PARAM = 1)
begin
set @IsUpdatingPayable = 1
end
else
begin
if (@HousePayableLineStatusCode in (''ACCT'' , ''PACC'') AND @MasterPayableLineStatusCode in (''ACCT'' , ''PACC''))
begin
set @IsUpdatingPayable = 0
end
else
begin
set @IsUpdatingPayable = 1
end
end
--set @IsUpdatingPayable = 1
if (@IsUpdatingPayable = 1)
BEGIN
-- Reset Variables
BEGIN
set @Ratio = 0
set @Quantity = 0
set @UnitPrice = 0
set @ExpectedAmount = 0
set @ExpectedAmountLocal = 0
set @ExpectedAmountInProfitCurrency = 0
set @AccountedAmount = 0
set @AccountedAmountInLocalCurrency = 0
set @AccountedAmountInProfitCurrency = 0
set @OpenAmount = 0
set @OpenAmountInLocalCurrency = 0
set @OpenAmountInProfitCurrency = 0
END
-- Compute Ration, Quantity, UnitPrice
BEGIN
if (@MasterPayableAmountTypeCode = ''NEXP'')
BEGIN
set @Ratio = @MasterPayableAccountedAmount / @AllHousesCount
set @Quantity = 0
set @UnitPrice = 0
set @AccountedAmount = @Ratio
END
-- Percent of Freight
else if (@MasterPayableMeasurementId = @PRFR_Id)
BEGIN
set @Quantity = @HouseFreightAmount
set @UnitPrice = @MasterPayableUnitPrice
END
END
-- Compute Amounts
BEGIN
set @ExpectedAmount = @Quantity * @UnitPrice
if (@MasterPayableMeasurementId = @PRFR_Id)
BEGIN
set @ExpectedAmount = @Quantity * @UnitPrice / 100
END
if (@MasterPayableAmountTypeCode = ''NEXP'')
begin
set @ExpectedAmount = null
set @OpenAmount = null
end
else if (@MasterPayableLineStatusCode = ''ACCT'' OR @MasterPayableLineStatusCode = ''PACC'')
begin
set @ExpectedAmountRatio = @ExpectedAmount / @MasterPayableExpectedAmount
set @OpenAmount = @MasterPayableOpenAmount * @ExpectedAmountRatio --/ @AllHousesCount
set @AccountedAmount = @MasterPayableAccountedAmount * @ExpectedAmountRatio --/ @AllHousesCount
end
else
begin
set @OpenAmount = @ExpectedAmount
set @AccountedAmount = 0
end
set @ExpectedAmountLocal = @ExpectedAmount * @MasterPayableRate
set @AccountedAmountInLocalCurrency = @AccountedAmount * @MasterPayableRate
set @OpenAmountInLocalCurrency = @OpenAmount * @MasterPayableRate
set @ExpectedAmountInProfitCurrency = @ExpectedAmountLocal / @MasterPayableProfitRate
set @AccountedAmountInProfitCurrency= @AccountedAmountInLocalCurrency / @MasterPayableProfitRate
set @OpenAmountInProfitCurrency = @OpenAmountInLocalCurrency / @MasterPayableProfitRate
END
-- Update Payable
BEGIN
update ShipmentPayables
set
VendorId = @MasterPayableVendorId,
MeasurementId = @HousePayableMeasurementId,
PrepaidCollectId = @MasterPayablePrepaidCollectId,
DueTypeCode = @MasterPayableDueTypeCode,
--AWBPrint = 0, --@MasterPayableAWBPrint,
ValueDate = @MasterPayableValueDate,
UpdateDate = @MasterPayableUpdateDate,
UpdateByUserId = @MasterPayableUpdatedByUserId,
ShipmentPayableLineStatusCode = @MasterPayableLineStatusCode,
CurrencyId = @MasterPayableCurrencyId,
Rate = @MasterPayableRate,
ProfitCurrencyExchangeRate = @MasterPayableProfitRate,
Quantity = isnull(ROUND(@Quantity,3),0),
UnitPrice = isnull(Round(@UnitPrice,3),0),
ExpectedAmount = isnull(Round(@ExpectedAmount,3),0),
ExpectedAmountLocal = isnull(Round(@ExpectedAmountLocal,3),0),
ExpectedAmountInProfitCurrency = isnull(Round(@ExpectedAmountInProfitCurrency,3),0),
AccountedAmount = isnull(Round(@AccountedAmount,3),0),
AccountedAmountInLocalCurrency = isnull(Round(@AccountedAmountInLocalCurrency,3),0),
AccountedAmountInProfitCurrency = isnull(Round(@AccountedAmountInProfitCurrency,3),0),
OpenAmount = isnull(Round(@OpenAmount,3),0),
OpenAmountInLocalCurrency = isnull(Round(@OpenAmountInLocalCurrency,3),0),
OpenAmountInProfitCurrency = isnull(Round(@OpenAmountInProfitCurrency,3),0),
IATACodeId = @MasterPayableIATACodeId
where Id = @HousePayableId and Tenant = @Tenant
END
END
FETCH NEXT FROM HousePayables2Cursor INTO @HousePayableId, @HousePayableLineStatusCode
END
CLOSE HousePayables2Cursor
DEALLOCATE HousePayables2Cursor
END
FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
END
CLOSE Houses2Cursor
DEALLOCATE Houses2Cursor
END
FETCH NEXT FROM MasterPayables2Cursor INTO @MasterPayableId, @MasterPayableQuantity, @MasterPayableUnitPrice, @MasterPayableVendorId, @MasterPayableChargesTypeId, @MasterPayableMeasurementId, @MasterPayablePrepaidCollectId, @MasterPayableDueTypeCode, @MasterPayableAWBPrint, @MasterPayableCurrencyId, @MasterPayableRate, @MasterPayableProfitRate, @MasterPayableLineStatusCode, @MasterPayableAmountTypeCode, @MasterPayableCreatedByUserId, @MasterPayableUpdatedByUserId, @MasterPayableCreateDate, @MasterPayableUpdateDate, @MasterPayableValueDate, @MasterPayableOpenAmount, @MasterPayableAccountedAmount, @MasterPayableExpectedAmount, @MasterPayableIATACodeId
END
CLOSE MasterPayables2Cursor
DEALLOCATE MasterPayables2Cursor
END
END
END');


-- Procedure Script From UpdateReceivablesDataProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateReceivablesData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateReceivablesData] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateReceivablesData]
(
@ShipmentId_PARAM varchar(15),
@IsInvoiceUpdated_PARAM  bit
)
AS
if exists (select * from Shipments where Id = @ShipmentId_PARAM and ShipmentLevelCode != ''D'')
BEGIN
declare @Tenant as int
declare @MasterId as varchar(15)
declare @ProrateReceivables as bit
select @Tenant = Tenant,
@MasterId = MasterShipmentDataId
from Shipments where Id = @ShipmentId_PARAM
if (@MasterId is not null)
BEGIN
set @ProrateReceivables = (select ProrateReceivables from ShipmentMasterDatas where Tenant = @Tenant AND Id = @MasterId)
if (@ProrateReceivables = 0)
BEGIN
declare @ShipmentHouseId as varchar(15)
DECLARE Houses1Cursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
OPEN Houses1Cursor FETCH NEXT FROM Houses1Cursor INTO @ShipmentHouseId
WHILE @@FETCH_STATUS = 0
BEGIN
delete from ShipmentReceivables
where Tenant = @Tenant
AND ShipmentId = @ShipmentHouseId
AND ShipmentReceivableParentId is not null
FETCH NEXT FROM Houses1Cursor INTO @ShipmentHouseId
END
CLOSE Houses1Cursor
DEALLOCATE Houses1Cursor
END
else
BEGIN
-- Global Variables
BEGIN
-- 01
declare @GRWT_Id as varchar(15)
declare @CHWT_Id as varchar(15)
declare @FIXD_Id as varchar(15)
declare @VOLU_Id as varchar(15)
declare @BTEU_Id as varchar(15)
declare @GWTN_Id as varchar(15)
declare @PRVL_Id as varchar(15)
declare @PRFR_Id as varchar(15)
declare @QTY_Id as varchar(15)
declare @GWKG_Id as varchar(15)
declare @CWKG_Id as varchar(15)
declare @VCBM_Id as varchar(15)
declare @SCGW_Id as varchar(15)
set @GRWT_Id = (select Id from Measurements where Code = ''GRWT'' AND Tenant = @Tenant)
set @CHWT_Id = (select Id from Measurements where Code = ''CHWT'' AND Tenant = @Tenant)
set @FIXD_Id = (select Id from Measurements where Code = ''FIXD'' AND Tenant = @Tenant)
set @VOLU_Id = (select Id from Measurements where Code = ''VOLU'' AND Tenant = @Tenant)
set @BTEU_Id = (select Id from Measurements where Code = ''BTEU'' AND Tenant = @Tenant)
set @GWTN_Id = (select Id from Measurements where Code = ''GWTN'' AND Tenant = @Tenant)
set @PRVL_Id = (select Id from Measurements where Code = ''PRVL'' AND Tenant = @Tenant)
set @PRFR_Id = (select Id from Measurements where Code = ''PRFR'' AND Tenant = @Tenant)
set @QTY_Id = (select Id from Measurements where Code = ''QTY'' AND Tenant = @Tenant)
set @GWKG_Id = (select Id from Measurements where Code = ''GWKG'' AND Tenant = @Tenant)
set @CWKG_Id = (select Id from Measurements where Code = ''CWKG'' AND Tenant = @Tenant)
set @VCBM_Id = (select Id from Measurements where Code = ''VCBM'' AND Tenant = @Tenant)
set @SCGW_Id = (select Id from Measurements where Code = ''SCGW'' AND Tenant = @Tenant)
-- 02
declare @AllHousesCount as float
declare @AllHousesTotalTEU as float
declare @AllHousesTotalVolume as float
declare @AllHousesTotalGrossWeight as float
declare @AllHousesTotalVolumetrics as float
declare @AllHousesTotalChargeables as float
declare @AllHousesTotalGrossWeightPerTon as float
declare @AllHousesTotalNumberOfPackages as float
declare @AllHousesTotalNumberOfContainers as float
declare @AllHousesTotalGrossWeightInKG as float
declare @AllHousesTotalChargeableWeightInKG as float
declare @AllHousesTotalVolumeInCBM as float
declare @AllHousesGrossWeightPerStorageDays as float;
if exists (select * from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId)
begin
select
@AllHousesCount = count(*),
@AllHousesTotalTEU = sum(isnull(TEU,0)),
@AllHousesTotalVolume = sum(isnull(Volume,0)),
@AllHousesTotalGrossWeight = sum(isnull(GrossWeight,0)),
@AllHousesTotalVolumetrics = sum(isnull(VolumetricWeight,0)),
@AllHousesTotalChargeables = sum(isnull(ChargeableWeight,0)),
@AllHousesTotalGrossWeightPerTon = sum(isnull(GrossWeightPerTon,0)),
@AllHousesTotalNumberOfPackages = sum(isnull(NumberOfPackages,0)),
@AllHousesTotalNumberOfContainers = sum(isnull(NumberOfContainers,0)),
@AllHousesTotalGrossWeightInKG = sum(isnull(GrossWeightInKG,0)),
@AllHousesTotalChargeableWeightInKG = sum(isnull(ChargeableWeightInKG,0)),
@AllHousesTotalVolumeInCBM = sum(isnull(VolumeInCBM,0)),
@AllHousesGrossWeightPerStorageDays = sum(isnull(GrossWeightPerStorageDays,0))
from Shipments
where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId
end
else
begin
set @AllHousesTotalTEU = 0
set @AllHousesTotalVolume = 0
set @AllHousesTotalGrossWeight = 0
set @AllHousesTotalVolumetrics = 0
set @AllHousesTotalChargeables = 0
set @AllHousesTotalGrossWeightPerTon = 0
set @AllHousesTotalNumberOfPackages = 0
set @AllHousesTotalNumberOfContainers = 0
set @AllHousesTotalGrossWeightInKG = 0
set @AllHousesTotalChargeableWeightInKG = 0
set @AllHousesTotalVolumeInCBM = 0
set @AllHousesGrossWeightPerStorageDays = 0
end
-- 03
declare @MasterTypeId as varchar(5)
declare @MasterTransportModeId as varchar(1)
select
@MasterTypeId = ShipmentTypeId,
@MasterTransportModeId = TransportModeId
from Shipments where Tenant = @Tenant and Id = @MasterId
END
-- Master Receivable Variables
BEGIN
declare @MasterReceivableId as varchar(15)
declare @MasterReceivableQuantity as float
declare @MasterReceivableUnitPrice as float
declare @MasterReceivableChargesTypeId as varchar(15)
declare @MasterReceivableMeasurementId as varchar(15)
declare @MasterReceivablePrepaidCollectId as varchar(1)
declare @MasterReceivableDueTypeCode as varchar(2)
declare @MasterReceivableIATACodeId as varchar(15)
declare @MasterReceivableAWBPrint as int
declare @MasterReceivableCurrencyId as varchar(15)
declare @MasterReceivableRate as float
declare @MasterReceivableProfitRate as float
declare @MasterReceivableLineStatusCode as varchar(4)
declare @MasterReceivableCreatedByUserId as varchar(15)
declare @MasterReceivableUpdatedByUserId as varchar(15)
declare @MasterReceivableCreateDate as datetime
declare @MasterReceivableUpdateDate as datetime
declare @MasterReceivablePackageTypeId as varchar(15)
declare @MasterReceivableAmount as float
declare @MasterReceivableARInvoiceId as varchar(15)
declare @MasterReceivableARInvoiceLineId as varchar(15)
declare @MasterReceivableIsFixedPrice as bit
declare @MasterReceivableIsExchangeRateFixed as bit
END
-- House Receivable Variables
BEGIN
declare @IsCreatingReceivable as bit
declare @IsUpdatingReceivable as bit
declare @NewId as varchar(15)
declare @HouseId as varchar(15)
declare @HouseTEU as float
declare @HouseVolume as float
declare @HouseGrossWeight as float
declare @HouseChargeableWeight as float
declare @HouseGrossWeightPerTon as float
declare @HouseValueOfGoods as float
declare @HouseFreightAmount as float
declare @HouseGrossWeightInKG as float
declare @HouseChargeableWeightInKG as float
declare @HouseVolumeInCBM as float
declare @HouseGrossWeightPerStorageDays as float
declare @HouseReceivableId as varchar(15)
declare @HouseReceivableMeasurementId as varchar(15)
declare @HouseReceivableARInvoiceId as varchar(15)
declare @HouseNumberOfPackages as int
declare @HouseNumberOfContainers as int
declare @HouseTypeId as varchar(5)
declare @HouseTransportModeId as varchar(1)
declare @Ratio as float
declare @Quantity as float
declare @UnitPrice as float
declare @Amount as float
declare @AmountLocal as float
declare @AmountInProfitCurrency as float
declare @AmountRatio as float
END
-- Loop Master Receivables (1: Not PRFR)
BEGIN
DECLARE MasterReceivables1Cursor CURSOR READ_ONLY
FOR
SELECT Id, Quantity, UnitPrice, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentReceivableLineStatusCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, TotalAmount, ARInvoiceId, ARInvoiceLineId, IsFixedPrice, IsExchangeRateFixed, IATACodeId
FROM ShipmentReceivables
WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND MeasurementId != @PRFR_Id
OPEN MasterReceivables1Cursor FETCH NEXT FROM MasterReceivables1Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
WHILE @@FETCH_STATUS = 0
BEGIN
-- Loop Houses
BEGIN
DECLARE Houses2Cursor CURSOR READ_ONLY
FOR
SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods, NumberOfPackages, NumberOfContainers, TransportModeId, ShipmentTypeId, GrossWeightInKG, ChargeableWeightInKG, VolumeInCBM, GrossWeightPerStorageDays
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
OPEN Houses2Cursor FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId,@HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsCreatingReceivable = 0
set @HouseReceivableMeasurementId = @MasterReceivableMeasurementId
-- IsCreatingReceivable
BEGIN
if (@MasterReceivableMeasurementId = @FIXD_Id
OR @MasterReceivableMeasurementId = @BTEU_Id
OR @MasterReceivableMeasurementId = @VOLU_Id
OR @MasterReceivableMeasurementId = @GRWT_Id
OR @MasterReceivableMeasurementId = @CHWT_Id
OR @MasterReceivableMeasurementId = @GWTN_Id
OR @MasterReceivableMeasurementId = @PRVL_Id
OR @MasterReceivableMeasurementId = @QTY_Id
OR @MasterReceivableMeasurementId = @GWKG_Id
OR @MasterReceivableMeasurementId = @CWKG_Id
OR @MasterReceivableMeasurementId = @VCBM_Id
OR @MasterReceivableMeasurementId = @SCGW_Id
)
BEGIN
if not exists (select * from ShipmentReceivables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentReceivableParentId = @MasterReceivableId and ChargesTypeId = @MasterReceivableChargesTypeId)
set @IsCreatingReceivable = 1
END
-- If Master Is FCL | FTL
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''FCLD'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''FTL''))
BEGIN
set @MasterReceivablePackageTypeId = (select Id from PackageTypes where MeasurementId = @MasterReceivableMeasurementId AND Tenant = @Tenant)
if exists (select * from ShipmentPackages where Tenant = @Tenant AND ShipmentId = @HouseId AND PackageTypeId = @MasterReceivablePackageTypeId)
begin
if not exists (select Id from ShipmentReceivables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentReceivableParentId = @MasterReceivableId AND ChargesTypeId = @MasterReceivableChargesTypeId AND MeasurementId = @MasterReceivableMeasurementId)
set @IsCreatingReceivable = 1
end
END
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''MyGO'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''MyGI''))
BEGIN
if not exists (select Id from ShipmentReceivables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentReceivableParentId = @MasterReceivableId AND ChargesTypeId = @MasterReceivableChargesTypeId AND MeasurementId = @CHWT_Id)
set @IsCreatingReceivable = 1
set @HouseReceivableMeasurementId = @CHWT_Id
END
if (@IsCreatingReceivable = 1)
BEGIN
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN T1;
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''ShipmentReceivable''
COMMIT TRAN T1;
INSERT INTO ShipmentReceivables
(
Id,
Tenant,
ShipmentId,
ShipmentReceivableParentId,
ShipmentReceivableLineStatusCode,
ChargesTypeId,
MeasurementId,
PrepaidCollectId,
DueTypeCode,
AWBPrint,
CurrencyId,
Rate,
ProfitCurrencyExchangeRate,
CreateDate,
UpdateDate,
CreatedByUserId,
UpdateByUserId,
IsFromQuote,
PayableLocal,
IsFixedPrice,
IsExchangeRateFixed,
ARInvoiceId,
ARInvoiceLineId,
IATACodeId
)
VALUES
(
@NewId,
@Tenant,
@HouseId,
@MasterReceivableId,
@MasterReceivableLineStatusCode,
@MasterReceivableChargesTypeId,
@HouseReceivableMeasurementId,
@MasterReceivablePrepaidCollectId,
@MasterReceivableDueTypeCode,
@MasterReceivableAWBPrint,
@MasterReceivableCurrencyId,
@MasterReceivableRate,
@MasterReceivableProfitRate,
@MasterReceivableCreateDate,
@MasterReceivableUpdateDate,
@MasterReceivableCreatedByUserId,
@MasterReceivableUpdatedByUserId,
0,
0,
@MasterReceivableIsFixedPrice,
@MasterReceivableIsExchangeRateFixed,
@MasterReceivableARInvoiceId,
@MasterReceivableARInvoiceLineId,
@MasterReceivableIATACodeId
)
END
END
-- Loop House Receivables / Amount Calculating & Updating
BEGIN
DECLARE HouseReceivables1Cursor CURSOR READ_ONLY
FOR
SELECT Id, ARInvoiceId
FROM ShipmentReceivables
WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentReceivableParentId = @MasterReceivableId
OPEN HouseReceivables1Cursor FETCH NEXT FROM HouseReceivables1Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsUpdatingReceivable = 0
if (@IsCreatingReceivable = 1)
begin
set @IsUpdatingReceivable = 1
end
else if (@IsInvoiceUpdated_PARAM = 1)
begin
set @IsUpdatingReceivable = 1
end
else
begin
if (@HouseReceivableARInvoiceId is not null AND @MasterReceivableARInvoiceId is not null)
begin
set @IsUpdatingReceivable = 0
end
else
begin
set @IsUpdatingReceivable = 1
end
end
--set @IsUpdatingReceivable = 1
if (@IsUpdatingReceivable = 1)
BEGIN
-- Reset Variables
BEGIN
set @Ratio = 0
set @Quantity = 0
set @UnitPrice = 0
set @Amount = 0
set @AmountLocal = 0
set @AmountInProfitCurrency = 0
END
-- Compute Ration, Quantity, UnitPrice
BEGIN
-- Fixed
if (@MasterReceivableMeasurementId = @FIXD_Id)
BEGIN
set @Ratio = @MasterReceivableQuantity / @AllHousesCount
set @Quantity = 1
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- By TEU
else if (@MasterReceivableMeasurementId = @BTEU_Id)
BEGIN
if (@AllHousesTotalTEU <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalTEU
end
set @Quantity = @HouseTEU
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- Volume
else if (@MasterReceivableMeasurementId = @VOLU_Id)
BEGIN
if (@AllHousesTotalVolume <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalVolume
end
set @Quantity = @HouseVolume
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- Gross Weight
else if (@MasterReceivableMeasurementId = @GRWT_Id)
BEGIN
if (@AllHousesTotalGrossWeight <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeight
end
set @Quantity = @HouseGrossWeight
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- Chargeable Weight
else if (@MasterReceivableMeasurementId = @CHWT_Id)
BEGIN
if (@AllHousesTotalChargeables <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeables
end
set @Quantity = @HouseChargeableWeight
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- Gross Weight Per Ton
else if (@MasterReceivableMeasurementId = @GWTN_Id)
BEGIN
if (@AllHousesTotalGrossWeightPerTon <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeightPerTon
end
set @Quantity = @HouseGrossWeightPerTon
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- GrossWeightPerStorageDays
else if (@MasterReceivableMeasurementId = @SCGW_Id)
BEGIN
if (@AllHousesGrossWeightPerStorageDays <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesGrossWeightPerStorageDays
end
set @Quantity = @HouseGrossWeightPerStorageDays
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- GWKG: Gross Weight in Kg
else if (@MasterReceivableMeasurementId = @GWKG_Id)
BEGIN
if (@AllHousesTotalGrossWeightInKG <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeightInKG
end
set @Quantity = @HouseGrossWeightInKG
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- CWKG: Chargeable Weight in Kg
else if (@MasterReceivableMeasurementId = @CWKG_Id)
BEGIN
if (@AllHousesTotalChargeableWeightInKG <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeableWeightInKG
end
set @Quantity = @HouseChargeableWeightInKG
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- VCBM: Volume in CBM
else if (@MasterReceivableMeasurementId = @VCBM_Id)
BEGIN
if (@AllHousesTotalVolumeInCBM <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalVolumeInCBM
end
set @Quantity = @HouseVolumeInCBM
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
-- Percent of Value
else if (@MasterReceivableMeasurementId = @PRVL_Id)
BEGIN
set @Quantity = @HouseValueOfGoods
set @UnitPrice = @MasterReceivableUnitPrice
END
-- Quantity
else if (@MasterReceivableMeasurementId = @QTY_Id)
BEGIN
if(@HouseTransportModeId = ''A'' OR (@HouseTransportModeId = ''O'' AND @HouseTypeId = ''LCLD'') OR (@HouseTransportModeId = ''I'' AND @HouseTypeId = ''LTL''))
begin
if (@AllHousesTotalNumberOfPackages <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalNumberOfPackages
end
set @Quantity = @HouseNumberOfPackages
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
end
else
begin
if (@AllHousesTotalNumberOfContainers <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalNumberOfContainers
end
set @Quantity = @HouseNumberOfContainers
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
end
END
----------
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''FCLD'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''FTL''))
BEGIN
set @Quantity = (select COUNT(Id) from ShipmentPackages where ShipmentId = @HouseId AND PackageTypeId = @MasterReceivablePackageTypeId)
set @UnitPrice = @MasterReceivableUnitPrice
END
else if ((@MasterTransportModeId = ''O'' AND @MasterTypeId = ''MyGO'') OR (@MasterTransportModeId = ''I'' AND @MasterTypeId = ''MyGI''))
BEGIN
if (@AllHousesTotalChargeables <> 0)
begin
set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeables
end
set @Quantity = @HouseChargeableWeight
set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
END
END
-- Compute Amounts
BEGIN
set @Amount = @Quantity * @UnitPrice
if (@MasterReceivableMeasurementId = @PRVL_Id)
BEGIN
set @Amount = @Quantity * @UnitPrice / 100
END
set @AmountLocal = @Amount * @MasterReceivableRate
set @AmountInProfitCurrency = @AmountLocal / @MasterReceivableProfitRate
END
-- Update Receivable
BEGIN
update ShipmentReceivables
set
MeasurementId = @HouseReceivableMeasurementId,
PrepaidCollectId = @MasterReceivablePrepaidCollectId,
DueTypeCode = @MasterReceivableDueTypeCode,
--AWBPrint = 0, --@MasterReceivableAWBPrint,
UpdateDate = @MasterReceivableUpdateDate,
UpdateByUserId = @MasterReceivableUpdatedByUserId,
ShipmentReceivableLineStatusCode = @MasterReceivableLineStatusCode,
CurrencyId = @MasterReceivableCurrencyId,
Rate = @MasterReceivableRate,
ProfitCurrencyExchangeRate = @MasterReceivableProfitRate,
Quantity = isnull(ROUND(@Quantity,3),0),
UnitPrice = isnull(Round(@UnitPrice,3),0),
TotalAmount = isnull(Round(@Amount,3),0),
TotalAmountLocal = isnull(Round(@AmountLocal,3),0),
AmountInProfitCurrency = isnull(Round(@AmountInProfitCurrency,3),0),
IsFixedPrice = @MasterReceivableIsFixedPrice,
IsExchangeRateFixed = @MasterReceivableIsExchangeRateFixed,
ARInvoiceId = @MasterReceivableARInvoiceId,
ARInvoiceLineId = @MasterReceivableARInvoiceLineId,
IATACodeId = @MasterReceivableIATACodeId
where Id = @HouseReceivableId and Tenant = @Tenant
END
END
FETCH NEXT FROM HouseReceivables1Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
END
CLOSE HouseReceivables1Cursor
DEALLOCATE HouseReceivables1Cursor
END
FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId, @HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
END
CLOSE Houses2Cursor
DEALLOCATE Houses2Cursor
END
FETCH NEXT FROM MasterReceivables1Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
END
CLOSE MasterReceivables1Cursor
DEALLOCATE MasterReceivables1Cursor
END
-- Loop Master Receivables (2: PRFR)
BEGIN
DECLARE MasterReceivables2Cursor CURSOR READ_ONLY
FOR
SELECT Id, Quantity, UnitPrice, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentReceivableLineStatusCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, TotalAmount, ARInvoiceId, ARInvoiceLineId, IsFixedPrice, IsExchangeRateFixed, IATACodeId
FROM ShipmentReceivables
WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND MeasurementId = @PRFR_Id
OPEN MasterReceivables2Cursor FETCH NEXT FROM MasterReceivables2Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
WHILE @@FETCH_STATUS = 0
BEGIN
-- Loop Houses
BEGIN
DECLARE Houses3Cursor CURSOR READ_ONLY
FOR
SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
OPEN Houses3Cursor FETCH NEXT FROM Houses3Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsCreatingReceivable = 0
set @HouseReceivableMeasurementId = @MasterReceivableMeasurementId
set @HouseFreightAmount = (select sum(isnull(TotalAmount,0)) from ShipmentReceivables where ShipmentId = @HouseId AND ShipmentReceivableParentId is not null AND ChargesTypeId in (select Id from ChargesTypes where ChargesGroupCode = ''FRT'' AND Tenant = @Tenant))
-- IsCreatingReceivable
BEGIN
if (@MasterReceivableMeasurementId = @PRFR_Id)
BEGIN
if not exists (select * from ShipmentReceivables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentReceivableParentId = @MasterReceivableId and ChargesTypeId = @MasterReceivableChargesTypeId)
set @IsCreatingReceivable = 1
END
if (@IsCreatingReceivable = 1)
BEGIN
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN T1;
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''ShipmentReceivable''
COMMIT TRAN T1;
INSERT INTO ShipmentReceivables
(
Id,
Tenant,
ShipmentId,
ShipmentReceivableParentId,
ShipmentReceivableLineStatusCode,
ChargesTypeId,
MeasurementId,
PrepaidCollectId,
DueTypeCode,
AWBPrint,
CurrencyId,
Rate,
ProfitCurrencyExchangeRate,
CreateDate,
UpdateDate,
CreatedByUserId,
UpdateByUserId,
IsFromQuote,
PayableLocal,
IsFixedPrice,
IsExchangeRateFixed,
ARInvoiceId,
ARInvoiceLineId,
IATACodeId
)
VALUES
(
@NewId,
@Tenant,
@HouseId,
@MasterReceivableId,
@MasterReceivableLineStatusCode,
@MasterReceivableChargesTypeId,
@HouseReceivableMeasurementId,
@MasterReceivablePrepaidCollectId,
@MasterReceivableDueTypeCode,
@MasterReceivableAWBPrint,
@MasterReceivableCurrencyId,
@MasterReceivableRate,
@MasterReceivableProfitRate,
@MasterReceivableCreateDate,
@MasterReceivableUpdateDate,
@MasterReceivableCreatedByUserId,
@MasterReceivableUpdatedByUserId,
0,
0,
@MasterReceivableIsFixedPrice,
@MasterReceivableIsExchangeRateFixed,
@MasterReceivableARInvoiceId,
@MasterReceivableARInvoiceLineId,
@MasterReceivableIATACodeId
)
END
END
-- Loop House Receivables / Amount Calculating & Updating
BEGIN
DECLARE HouseReceivables2Cursor CURSOR READ_ONLY
FOR
SELECT Id, ARInvoiceId
FROM ShipmentReceivables
WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentReceivableParentId = @MasterReceivableId
OPEN HouseReceivables2Cursor FETCH NEXT FROM HouseReceivables2Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
WHILE @@FETCH_STATUS = 0
BEGIN
set @IsUpdatingReceivable = 0
if (@IsCreatingReceivable = 1)
begin
set @IsUpdatingReceivable = 1
end
else if (@IsInvoiceUpdated_PARAM = 1)
begin
set @IsUpdatingReceivable = 1
end
else
begin
if (@HouseReceivableARInvoiceId is not null AND @MasterReceivableARInvoiceId is not null)
begin
set @IsUpdatingReceivable = 0
end
else
begin
set @IsUpdatingReceivable = 1
end
end
--set @IsUpdatingReceivable = 1
if (@IsUpdatingReceivable = 1)
BEGIN
set @Quantity = @HouseFreightAmount
set @UnitPrice = @MasterReceivableUnitPrice
set @Amount = @Quantity * @UnitPrice / 100
set @AmountLocal = @Amount * @MasterReceivableRate
set @AmountInProfitCurrency = @AmountLocal / @MasterReceivableProfitRate
-- Update Receivable
BEGIN
update ShipmentReceivables
set
MeasurementId = @HouseReceivableMeasurementId,
PrepaidCollectId = @MasterReceivablePrepaidCollectId,
DueTypeCode = @MasterReceivableDueTypeCode,
--AWBPrint = 0, --@MasterReceivableAWBPrint,
UpdateDate = @MasterReceivableUpdateDate,
UpdateByUserId = @MasterReceivableUpdatedByUserId,
ShipmentReceivableLineStatusCode = @MasterReceivableLineStatusCode,
CurrencyId = @MasterReceivableCurrencyId,
Rate = @MasterReceivableRate,
ProfitCurrencyExchangeRate = @MasterReceivableProfitRate,
Quantity = isnull(ROUND(@Quantity,3),0),
UnitPrice = isnull(Round(@UnitPrice,3),0),
TotalAmount = isnull(Round(@Amount,3),0),
TotalAmountLocal = isnull(Round(@AmountLocal,3),0),
AmountInProfitCurrency = isnull(Round(@AmountInProfitCurrency,3),0),
IsFixedPrice = @MasterReceivableIsFixedPrice,
IsExchangeRateFixed = @MasterReceivableIsExchangeRateFixed,
ARInvoiceId = @MasterReceivableARInvoiceId,
ARInvoiceLineId = @MasterReceivableARInvoiceLineId,
IATACodeId = @MasterReceivableIATACodeId
where Id = @HouseReceivableId and Tenant = @Tenant
END
END
FETCH NEXT FROM HouseReceivables2Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
END
CLOSE HouseReceivables2Cursor
DEALLOCATE HouseReceivables2Cursor
END
FETCH NEXT FROM Houses3Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
END
CLOSE Houses3Cursor
DEALLOCATE Houses3Cursor
END
FETCH NEXT FROM MasterReceivables2Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
END
CLOSE MasterReceivables2Cursor
DEALLOCATE MasterReceivables2Cursor
END
END
END
END');


-- Procedure Script From UpdateShipmentARInvoicesProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentARInvoices]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentARInvoices] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentARInvoices]
(
@ShipmentId varchar(15),
@ConsolidationNumber varchar(50)
)
AS
declare @InvoiceNumber as varchar(50)
declare @ShipmentARInvoices as varchar(1000)
if (@ConsolidationNumber is not null)
set @ShipmentARInvoices = @ConsolidationNumber
BEGIN
DECLARE EntitiesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.InvoiceNumber
FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
WHERE
ARInvoiceEntities.EntityId = @ShipmentId
AND ARInvoices.StatusCode <> ''DR''
AND ARInvoices.StatusCode <> ''VD''
--AND ARInvoices.IsConstituentInvoice = 0
OPEN EntitiesCursor FETCH NEXT FROM EntitiesCursor INTO @InvoiceNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if(@ShipmentARInvoices is null) set @ShipmentARInvoices = @InvoiceNumber
else set @ShipmentARInvoices = @ShipmentARInvoices + '','' + @InvoiceNumber
FETCH NEXT FROM EntitiesCursor INTO @InvoiceNumber
END
CLOSE EntitiesCursor
DEALLOCATE EntitiesCursor
if(len(@ShipmentARInvoices) >= 1000)
begin
set @ShipmentARInvoices = SUBSTRING(@ShipmentARInvoices, 1, 947) + '' ... for the full list check the shipment receivables''
end
update Shipments set ARInvoices = @ShipmentARInvoices where Id = @ShipmentId
END');


-- Procedure Script From UpdateShipmentFinalArrivalDateProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentFinalArrivalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
(
@ShipmentId varchar(15)
)
AS
if (@ShipmentId is not null)
BEGIN
-- Shipment Fields
declare @Tenant as int
declare @DirectionId as varchar(3)
declare @TransportModeId as varchar(3)
declare @ShipmentLevelCode as varchar(1)
declare @MasterShipmentDataId as varchar(15)
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @OnCarriageETA as datetime
declare @OnCarriageATA as datetime
SELECT
@Tenant = Tenant,
@DirectionId = DirectionId,
@TransportModeId = TransportModeId,
@ShipmentLevelCode = ShipmentLevelCode,
@MasterShipmentDataId = MasterShipmentDataId,
@OnCarriageFromPortId = OnCarriageFromPortId,
@OnCarriageToPortId = OnCarriageToPortId,
@OnCarriageETA = OnCarriageETA,
@OnCarriageATA = OnCarriageATA
from Shipments where Id = @ShipmentId
declare @FinalArrivalDate as datetime
declare @ActualFinalArrivalDate as datetime
declare @EstimatedFinalArrivalDate as datetime
declare @HasDelivery as bit
declare @HasOnCarriage as bit
set @HasDelivery = 0
set @HasOnCarriage= 0
-- @DeliveriesDate
if exists (select * from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and Tenant = @Tenant and PickUpDeliveryTypeCode = ''DELV'')
BEGIN
set @HasDelivery = 1
declare @ETA as datetime
declare @ATA as datetime
declare @DeliveryDate as datetime
declare @DeliveriesDate as datetime
declare @ActualDeliveriesDate as datetime
declare @EstimatedDeliveriesDate as datetime
DECLARE DeliveriesCursor CURSOR READ_ONLY
FOR
SELECT ETA, ATA
FROM ShipmentPickUpDeliveries
WHERE ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = ''DELV''
OPEN DeliveriesCursor FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
WHILE @@FETCH_STATUS = 0
BEGIN
set @DeliveryDate = null
if (@ATA is not null)
set @DeliveryDate = @ATA
else if (@ETA is not null)
set @DeliveryDate = @ETA
if (@ATA is not null)
begin
if (@ActualDeliveriesDate is null)
set @ActualDeliveriesDate = @ATA
else if (@ATA > @ActualDeliveriesDate)
set @ActualDeliveriesDate = @ATA
end
if (@ETA is not null)
begin
if (@EstimatedDeliveriesDate is null)
set @EstimatedDeliveriesDate = @ETA
else if (@ETA > @EstimatedDeliveriesDate)
set @EstimatedDeliveriesDate = @ETA
end
if (@DeliveryDate is not null)
begin
if (@DeliveriesDate is null)
set @DeliveriesDate = @DeliveryDate
else if (@DeliveryDate > @DeliveriesDate)
set @DeliveriesDate = @DeliveryDate
end
FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
END
CLOSE DeliveriesCursor
DEALLOCATE DeliveriesCursor
set @FinalArrivalDate = @DeliveriesDate
set @ActualFinalArrivalDate = @ActualDeliveriesDate
set @EstimatedFinalArrivalDate = @EstimatedDeliveriesDate
END
-- @OnCarriageDate
else if (@OnCarriageFromPortId is not null and @OnCarriageToPortId is not null)
BEGIN
set @HasOnCarriage = 1
if (@OnCarriageATA is not null)
begin
set @ActualFinalArrivalDate = @OnCarriageATA
set @FinalArrivalDate = @OnCarriageATA
end
if (@OnCarriageETA is not null)
begin
set @EstimatedFinalArrivalDate = @OnCarriageETA
if (@FinalArrivalDate is null)
set @FinalArrivalDate = @OnCarriageETA
end
END
-- House
if (@ShipmentLevelCode = ''H'' AND @MasterShipmentDataId is not null and @HasDelivery = 0)
BEGIN
declare @IsTakingMasterDates as bit
set @IsTakingMasterDates = 0;
if(@HasOnCarriage = 0)
begin
set @IsTakingMasterDates = 1
end
else
begin
if exists (select * from ShipmentPickUpDeliveries where ShipmentId = @MasterShipmentDataId and Tenant = @Tenant and PickUpDeliveryTypeCode = ''DELV'')
set @IsTakingMasterDates = 1
end
if (@IsTakingMasterDates = 1)
begin
SELECT
@FinalArrivalDate = FinalArrivalDate,
@EstimatedFinalArrivalDate = EstimatedFinalArrivalDate,
@ActualFinalArrivalDate = ActualFinalArrivalDate
from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant
end
END
else if (@HasDelivery = 0 AND @HasOnCarriage = 0)
BEGIN
--Transshipment1
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment1ETA as datetime
declare @Transshipment1ATA as datetime
--Transshipment2
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment2ETA as datetime
declare @Transshipment2ATA as datetime
--Transshipment3
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @Transshipment3ETA as datetime
declare @Transshipment3ATA as datetime
--MainCarriage (Inland demostic got no Ports)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @MainCarriageETA as datetime
declare @MainCarriageATA as datetime
select
@MainCarriageFromPortId = MainCarriageFromPortId,
@Transshipment1FromPortId = Transshipment1FromPortId,
@Transshipment2FromPortId = Transshipment2FromPortId,
@Transshipment3FromPortId = Transshipment3FromPortId,
@MainCarriageToPortId = MainCarriageToPortId,
@Transshipment1ToPortId = Transshipment1ToPortId,
@Transshipment2ToPortId = Transshipment2ToPortId,
@Transshipment3ToPortId = Transshipment3ToPortId,
@Transshipment1ETA = Transshipment1ETA,
@Transshipment2ETA = Transshipment2ETA,
@Transshipment3ETA = Transshipment3ETA,
@MainCarriageETA = MainCarriageETA,
@Transshipment1ATA = Transshipment1ATA,
@Transshipment2ATA = Transshipment2ATA,
@Transshipment3ATA = Transshipment3ATA,
@MainCarriageATA = MainCarriageATA
from ShipmentMasterDatas where Id = @ShipmentId
-- @Transshipment3
if (@Transshipment3FromPortId is not null and @Transshipment3ToPortId is not null)
BEGIN
if (@Transshipment3ATA is not null)
begin
set @ActualFinalArrivalDate = @Transshipment3ATA
set @FinalArrivalDate = @Transshipment3ATA
end
if (@Transshipment3ETA is not null)
begin
set @EstimatedFinalArrivalDate = @Transshipment3ETA
if (@FinalArrivalDate is null)
set @FinalArrivalDate = @Transshipment3ETA
end
END
-- @Transshipment2
else if (@Transshipment2FromPortId is not null and @Transshipment2ToPortId is not null)
BEGIN
if (@Transshipment2ATA is not null)
begin
set @ActualFinalArrivalDate = @Transshipment2ATA
set @FinalArrivalDate = @Transshipment2ATA
end
if (@Transshipment2ETA is not null)
begin
set @EstimatedFinalArrivalDate = @Transshipment2ETA
if (@FinalArrivalDate is null)
set @FinalArrivalDate = @Transshipment2ETA
end
END
-- @Transshipment1
else if (@Transshipment1FromPortId is not null and @Transshipment1ToPortId is not null)
BEGIN
if (@Transshipment1ATA is not null)
begin
set @ActualFinalArrivalDate = @Transshipment1ATA
set @FinalArrivalDate = @Transshipment1ATA
end
if (@Transshipment1ETA is not null)
begin
set @EstimatedFinalArrivalDate = @Transshipment1ETA
if (@FinalArrivalDate is null)
set @FinalArrivalDate = @Transshipment1ETA
end
END
-- @MainCarriage
else if (@MainCarriageFromPortId is not null and @MainCarriageToPortId is not null)
BEGIN
if (@MainCarriageATA is not null)
begin
set @ActualFinalArrivalDate = @MainCarriageATA
set @FinalArrivalDate = @MainCarriageATA
end
if (@MainCarriageETA is not null)
begin
set @EstimatedFinalArrivalDate = @MainCarriageETA
if (@FinalArrivalDate is null)
set @FinalArrivalDate = @MainCarriageETA
end
END
END
update Shipments
set
FinalArrivalDate = @FinalArrivalDate,
EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate,
ActualFinalArrivalDate = @ActualFinalArrivalDate
where Id = @ShipmentId and Tenant = @Tenant
if (@ShipmentLevelCode = ''C'')
BEGIN
-- Loop Houses
declare @HouseId as varchar(15)
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE usp_UpdateShipmentFinalArrivalDate @HouseId
FETCH NEXT FROM HousesCursor INTO @HouseId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
END');


-- Procedure Script From UpdateShipmentOperationalDateProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentOperationalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentOperationalDate] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentOperationalDate]
(
@ShipmentId varchar(15)
)
AS
declare @Tenant as int
declare @DirectionId as varchar(1)
declare @CreateDateTime as datetime
declare @ShipmentLevelCode as varchar(1)
declare @MasterShipmentDataId as varchar(15)
declare @MasterCreateDateTime as datetime
declare @MasterOperationalDate as datetime
declare @HouseId as varchar(15)
declare @MainCarriageETA as datetime
declare @Transshipment1ETA as datetime
declare @Transshipment2ETA as datetime
declare @Transshipment3ETA as datetime
declare @MainCarriageATA as datetime
declare @Transshipment1ATA as datetime
declare @Transshipment2ATA as datetime
declare @Transshipment3ATA as datetime
declare @MainCarriageETD as datetime
declare @Transshipment1ETD as datetime
declare @Transshipment2ETD as datetime
declare @Transshipment3ETD as datetime
declare @MainCarriageATD as datetime
declare @Transshipment1ATD as datetime
declare @Transshipment2ATD as datetime
declare @Transshipment3ATD as datetime
declare @OperationalDate as datetime
BEGIN
SELECT
@Tenant = Tenant,
@DirectionId = DirectionId,
@CreateDateTime = CreateDateTime,
@ShipmentLevelCode = ShipmentLevelCode,
@MasterShipmentDataId = MasterShipmentDataId
from Shipments where Id = @ShipmentId
set @OperationalDate = null
if(@ShipmentLevelCode = ''H'')
BEGIN
if(@MasterShipmentDataId is null)
begin
set @OperationalDate = @CreateDateTime
end
else
begin
select
@MasterCreateDateTime = CreateDateTime,
@MasterOperationalDate = OperationalDate
from Shipments where Tenant = @Tenant AND Id = @MasterShipmentDataId
if(@MasterOperationalDate is not null AND @MasterOperationalDate != @MasterCreateDateTime)
set @OperationalDate = @MasterOperationalDate
else
set @OperationalDate = @CreateDateTime
end
END
else
BEGIN
if(@DirectionId = ''I'')
begin
SELECT
@MainCarriageETA = MainCarriageETA,
@Transshipment1ETA = Transshipment1ETA,
@Transshipment2ETA = Transshipment2ETA,
@Transshipment3ETA = Transshipment3ETA,
@MainCarriageATA = MainCarriageATA,
@Transshipment1ATA = Transshipment1ATA,
@Transshipment2ATA = Transshipment2ATA,
@Transshipment3ATA = Transshipment3ATA
from ShipmentMasterDatas where Id = @ShipmentId AND Tenant = @Tenant
-- Actual Arrival
if (@Transshipment3ATA is not null)
set @OperationalDate = @Transshipment3ATA;
else if (@Transshipment2ATA is not null)
set @OperationalDate = @Transshipment2ATA;
else if (@Transshipment1ATA is not null)
set @OperationalDate = @Transshipment1ATA;
else if (@MainCarriageATA is not null)
set @OperationalDate = @MainCarriageATA;
-- Expected Arrival
else if (@Transshipment3ETA is not null)
set @OperationalDate = @Transshipment3ETA;
else if (@Transshipment2ETA is not null)
set @OperationalDate = @Transshipment2ETA;
else if (@Transshipment1ETA is not null)
set @OperationalDate = @Transshipment1ETA;
else if (@MainCarriageETA is not null)
set @OperationalDate = @MainCarriageETA;
else
set @OperationalDate = @CreateDateTime;
end
else
begin
SELECT
@MainCarriageETD = MainCarriageETD,
@Transshipment1ETD = Transshipment1ETD,
@Transshipment2ETD = Transshipment2ETD,
@Transshipment3ETD = Transshipment3ETD,
@MainCarriageATD = MainCarriageATD,
@Transshipment1ATD = Transshipment1ATD,
@Transshipment2ATD = Transshipment2ATD,
@Transshipment3ATD = Transshipment3ATD
from ShipmentMasterDatas where Id = @ShipmentId AND Tenant = @Tenant
-- Actual Departure
if (@MainCarriageATD is not null)
set @OperationalDate = @MainCarriageATD;
else if (@Transshipment1ATD is not null)
set @OperationalDate = @Transshipment1ATD;
else if (@Transshipment2ATD is not null)
set @OperationalDate = @Transshipment2ATD;
else if (@Transshipment3ATD is not null)
set @OperationalDate = @Transshipment3ATD;
-- Expected Departure
else if (@MainCarriageETD is not null)
set @OperationalDate = @MainCarriageETD;
else if (@Transshipment1ETD is not null)
set @OperationalDate = @Transshipment1ETD;
else if (@Transshipment2ETD is not null)
set @OperationalDate = @Transshipment2ETD;
else if (@Transshipment3ETD is not null)
set @OperationalDate = @Transshipment3ETD;
else
set @OperationalDate = @CreateDateTime;
end
END
update Shipments set OperationalDate = @OperationalDate where Id = @ShipmentId AND Tenant = @Tenant
if(@ShipmentLevelCode = ''C'')
BEGIN
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId AND Tenant = @Tenant
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE [usp_UpdateShipmentOperationalDate] @HouseId
FETCH NEXT FROM HousesCursor INTO @HouseId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
END');


-- Procedure Script From UpdateShipmentProfitFunctionProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentProfitFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentProfitFunction] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentProfitFunction]
(
@Tenant int,
@ShipmentId varchar(15),
@IsConsoleShipment bit
)
AS
if (@ShipmentId is not null)
BEGIN
declare @ProrateReceivables as bit
if (@IsConsoleShipment = 1)
begin
set @ProrateReceivables = (select ProrateReceivables from ShipmentMasterDatas where Tenant = @Tenant AND Id = @ShipmentId)
end
-- Variables
BEGIN
declare @ProfitInLocalCurrency as float
declare @ProfitInProfitCurrency as float
declare @AllPayablesInLocalCurrency as float
declare @AllPayablesInProfitCurrency as float
declare @AllReceivablesInLocalCurrency as float
declare @AllReceivablesInProfitCurrency as float
declare @OpenPayablesInLocalCurrency as float
declare @OpenPayablesInProfitCurrency as float
declare @AccountedPayablesInLocalCurrency as float
declare @AccountedPayablesInProfitCurrency as float
declare @OpenReceivablesInLocalCurrency as float
declare @OpenReceivablesInProfitCurrency as float
declare @AccountedReceivablesInLocalCurrency as float
declare @AccountedReceivablesInProfitCurrency as float
declare @ShipmentPayableStatusCode AS varchar(4)
declare @ShipmentReceivableStatusCode AS varchar(4)
declare @ARInvoiceIssued as bit
declare @CreditNoteIssued as bit
declare @NotInvoicedReceivablesAmount as float
END
-- Reset Variables
BEGIN
set @ProfitInLocalCurrency = 0
set @ProfitInProfitCurrency = 0
set @AllPayablesInLocalCurrency = 0
set @AllPayablesInProfitCurrency = 0
set @AllReceivablesInLocalCurrency = 0
set @AllReceivablesInProfitCurrency = 0
set @OpenPayablesInLocalCurrency = 0
set @OpenPayablesInProfitCurrency = 0
set @AccountedPayablesInLocalCurrency = 0
set @AccountedPayablesInProfitCurrency = 0
set @OpenReceivablesInLocalCurrency = 0
set @OpenReceivablesInProfitCurrency = 0
set @AccountedReceivablesInLocalCurrency = 0
set @AccountedReceivablesInProfitCurrency = 0
set @ShipmentPayableStatusCode = ''NOPA''
set @ShipmentReceivableStatusCode = ''NORE''
set @ARInvoiceIssued = 0
set @CreditNoteIssued = 0
set @NotInvoicedReceivablesAmount = 0
END
-- Get Payables Data
BEGIN
if (@IsConsoleShipment = 1)
begin
if exists (select * from Shipments where Tenant = @Tenant AND ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId)
begin
select
@OpenPayablesInLocalCurrency = sum(isnull(OpenAmountInLocalCurrency,0)),
@OpenPayablesInProfitCurrency = sum(isnull(OpenAmountInProfitCurrency,0)),
@AccountedPayablesInLocalCurrency = sum(isnull(AccountedAmountInLocalCurrency,0)),
@AccountedPayablesInProfitCurrency = sum(isnull(AccountedAmountInProfitCurrency,0))
from ShipmentPayables
where
Tenant = @Tenant
AND (ShipmentId in (select Id from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId))
end
else
begin
select
@OpenPayablesInLocalCurrency = sum(isnull(OpenAmountInLocalCurrency,0)),
@OpenPayablesInProfitCurrency = sum(isnull(OpenAmountInProfitCurrency,0)),
@AccountedPayablesInLocalCurrency = sum(isnull(AccountedAmountInLocalCurrency,0)),
@AccountedPayablesInProfitCurrency = sum(isnull(AccountedAmountInProfitCurrency,0))
from ShipmentPayables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
end
end
else
begin
select
@OpenPayablesInLocalCurrency = sum(isnull(OpenAmountInLocalCurrency,0)),
@OpenPayablesInProfitCurrency = sum(isnull(OpenAmountInProfitCurrency,0)),
@AccountedPayablesInLocalCurrency = sum(isnull(AccountedAmountInLocalCurrency,0)),
@AccountedPayablesInProfitCurrency = sum(isnull(AccountedAmountInProfitCurrency,0))
from ShipmentPayables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
end
END
-- Get Receivables Data
BEGIN
if (@IsConsoleShipment = 1)
begin
if (@ProrateReceivables = 1)
BEGIN
if exists (select * from Shipments where Tenant = @Tenant AND ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId)
begin
select
@OpenReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@OpenReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND (ShipmentId in (select Id from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId))
AND (ShipmentReceivableLineStatusCode = ''OAMT'' OR ShipmentReceivableLineStatusCode = ''DRFT'')
select
@AccountedReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@AccountedReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND (ShipmentId in (select Id from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId))
AND ShipmentReceivableLineStatusCode = ''ACCT''
end
else
begin
select
@OpenReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@OpenReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
AND (ShipmentReceivableLineStatusCode = ''OAMT'' OR ShipmentReceivableLineStatusCode = ''DRFT'')
select
@AccountedReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@AccountedReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
AND ShipmentReceivableLineStatusCode = ''ACCT''
end
END
else
BEGIN
select
@OpenReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@OpenReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND (ShipmentId = @ShipmentId OR ShipmentId in (select Id from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId))
AND (ShipmentReceivableLineStatusCode = ''OAMT'' OR ShipmentReceivableLineStatusCode = ''DRFT'')
select
@AccountedReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@AccountedReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND (ShipmentId = @ShipmentId OR ShipmentId in (select Id from Shipments where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId))
AND ShipmentReceivableLineStatusCode = ''ACCT''
END
end
else
begin
select
@OpenReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@OpenReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
AND (ShipmentReceivableLineStatusCode = ''OAMT'' OR ShipmentReceivableLineStatusCode = ''DRFT'')
select
@AccountedReceivablesInLocalCurrency = sum(isnull(TotalAmountLocal,0)),
@AccountedReceivablesInProfitCurrency = sum(isnull(AmountInProfitCurrency,0))
from ShipmentReceivables
where
Tenant = @Tenant
AND ShipmentId = @ShipmentId
AND ShipmentReceivableLineStatusCode = ''ACCT''
end
END
-- FIX NULL Variables
BEGIN
set @OpenPayablesInLocalCurrency = isnull(@OpenPayablesInLocalCurrency,0)
set @OpenPayablesInProfitCurrency = isnull(@OpenPayablesInProfitCurrency,0)
set @AccountedPayablesInLocalCurrency = isnull(@AccountedPayablesInLocalCurrency,0)
set @AccountedPayablesInProfitCurrency = isnull(@AccountedPayablesInProfitCurrency,0)
set @OpenReceivablesInLocalCurrency = isnull(@OpenReceivablesInLocalCurrency,0)
set @OpenReceivablesInProfitCurrency = isnull(@OpenReceivablesInProfitCurrency,0)
set @AccountedReceivablesInLocalCurrency = isnull(@AccountedReceivablesInLocalCurrency,0)
set @AccountedReceivablesInProfitCurrency = isnull(@AccountedReceivablesInProfitCurrency,0)
END
-- Compute Profit Fields
BEGIN
set @AllPayablesInLocalCurrency = @OpenPayablesInLocalCurrency + @AccountedPayablesInLocalCurrency
set @AllPayablesInProfitCurrency = @OpenPayablesInProfitCurrency + @AccountedPayablesInProfitCurrency
set @AllReceivablesInLocalCurrency = @OpenReceivablesInLocalCurrency + @AccountedReceivablesInLocalCurrency
set @AllReceivablesInProfitCurrency = @OpenReceivablesInProfitCurrency + @AccountedReceivablesInProfitCurrency
-- New Design
set @ProfitInLocalCurrency = @AllReceivablesInLocalCurrency - @AllPayablesInLocalCurrency
set @ProfitInProfitCurrency = @AllReceivablesInProfitCurrency - @AllPayablesInProfitCurrency
-- Old Design
--if (@AllReceivablesInLocalCurrency <> 0)
--BEGIN
--	set @ProfitInLocalCurrency = @AllReceivablesInLocalCurrency - @AllPayablesInLocalCurrency
--	set @ProfitInProfitCurrency = @AllReceivablesInProfitCurrency - @AllPayablesInProfitCurrency
--END
END
-- Compute Payables Status
BEGIN
if (@OpenPayablesInLocalCurrency is null)
set @OpenPayablesInLocalCurrency = 0
if (@AccountedPayablesInLocalCurrency is null)
set @AccountedPayablesInLocalCurrency = 0
if (@OpenPayablesInLocalCurrency = 0 AND @AccountedPayablesInLocalCurrency = 0)
begin
set @ShipmentPayableStatusCode = ''NOPA''
end
else if (@OpenPayablesInLocalCurrency = 0 AND @AccountedPayablesInLocalCurrency <> 0)
begin
set @ShipmentPayableStatusCode = ''CLSD''
end
else
begin
set @ShipmentPayableStatusCode = ''OPEN''
end
END
-- Compute Receivables Status
BEGIN
if (@OpenReceivablesInLocalCurrency is null)
set @OpenReceivablesInLocalCurrency = 0
if (@AccountedReceivablesInLocalCurrency is null)
set @AccountedReceivablesInLocalCurrency = 0
if (@OpenReceivablesInLocalCurrency = 0 AND @AccountedReceivablesInLocalCurrency = 0)
begin
set @ShipmentReceivableStatusCode = ''NORE''
end
else if (@OpenReceivablesInLocalCurrency = 0 AND @AccountedReceivablesInLocalCurrency <> 0)
begin
set @ShipmentReceivableStatusCode = ''CLSD''
end
else
begin
set @ShipmentReceivableStatusCode = ''OPEN''
end
END
-- Compute Invoice Fields
BEGIN
if exists (select * from ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
where ARInvoiceEntities.EntityId = @ShipmentId AND ARInvoices.ARInvoiceTypeCode = ''CD'')
begin
set @CreditNoteIssued = 1
end
if exists (select * from ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
where ARInvoiceEntities.EntityId = @ShipmentId AND ARInvoices.ARInvoiceTypeCode != ''CD'')
begin
set @ARInvoiceIssued = 1
end
END
-- NotInvoicedReceivables
select @NotInvoicedReceivablesAmount = sum(isnull(TotalAmountLocal,0))
from ShipmentReceivables
where Tenant = @Tenant
AND ShipmentId = @ShipmentId
AND ShipmentReceivableLineStatusCode = ''OAMT''
-- Update Shipment
BEGIN
Update Shipments
set
OpenPayablesInLocalCurrency = round(@OpenPayablesInLocalCurrency,2),
OpenPayablesInProfitCurrency = round(@OpenPayablesInProfitCurrency,2),
AccountedPayablesInLocalCurrency = round(@AccountedPayablesInLocalCurrency,2),
AccountedPayablesInProfitCurrency = round(@AccountedPayablesInProfitCurrency,2),
OpenReceivablesInLocalCurrency = round(@OpenReceivablesInLocalCurrency,2),
OpenReceivablesInProfitCurrency = round(@OpenReceivablesInProfitCurrency,2),
AccountedReceivablesInLocalCurrency = round(@AccountedReceivablesInLocalCurrency,2),
AccountedReceivablesInProfitCurrency = round(@AccountedReceivablesInProfitCurrency,2),
ProfitInLocalCurrency = round(@ProfitInLocalCurrency,2),
ProfitInProfitCurrency = round(@ProfitInProfitCurrency,2),
ShipmentPayableStatusCode = @ShipmentPayableStatusCode,
ShipmentReceivableStatusCode = @ShipmentReceivableStatusCode,
ARInvoiceIssued = @ARInvoiceIssued,
CreditNoteIssued = @CreditNoteIssued,
NotInvoicedReceivablesAmount = @NotInvoicedReceivablesAmount
Where Id = @ShipmentId AND Tenant = @Tenant
END
END');


-- Procedure Script From UpdateShipmentProfitProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentProfit]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentProfit] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentProfit]
(
@ShipmentId varchar(15)
)
AS
declare @Tenant as int
declare @MasterId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
select
@Tenant = Tenant,
@MasterId = MasterShipmentDataId,
@ShipmentLevelCode = ShipmentLevelCode
from Shipments where Id = @ShipmentId
if (@ShipmentLevelCode = ''D'' OR (@ShipmentLevelCode = ''H'' AND @MasterId is null))
BEGIN
EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @ShipmentId, 0
END
else
BEGIN
EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @MasterId, 1
-- Loop Houses
declare @HouseId as varchar(15)
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @HouseId, 0
FETCH NEXT FROM HousesCursor INTO @HouseId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END');


-- Procedure Script From UpdateShipmentRegistryDateProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentRegistryDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate]
(
@ShipmentId varchar(15)
)
AS
if (@ShipmentId is not null)
BEGIN
declare @Tenant as int
declare @HouseId as varchar(15)
declare @MasterDataId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @RegistryDate_Master as DateTime
declare @ARInvoiceId as varchar(15)
declare @ApprovedDate as DateTime
declare @IsConstituent as bit
declare @StatusCode as varchar(2)
declare @ConsolidationInvoiceId as varchar(15)
declare @FirstApprovalDate as DateTime
select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentLevelCode = ShipmentLevelCode
from Shipments where Id = @ShipmentId
set @FirstApprovalDate = null
-- Get the First Approval Date
-- for the Shipment from its own invoices
BEGIN
DECLARE ARInvoiceEntitiesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.ApprovedDate, ARInvoices.IsConstituentInvoice, ARInvoices.StatusCode, ARInvoices.ConsolidationInvoiceId
FROM ARInvoiceEntities
JOIN ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
WHERE ARInvoiceEntities.EntityId = @ShipmentId
AND ARInvoiceEntities.Tenant = @Tenant
AND ARInvoices.Tenant = @Tenant
AND ARInvoices.IsAutoCredit = 0
AND ARInvoices.StatusCode != ''DR''
AND ARInvoices.StatusCode != ''LL''
AND ARInvoices.StatusCode != ''VD''
AND ARInvoices.StatusCode != ''NT''
AND ARInvoices.StatusCode != ''AC''
OPEN ARInvoiceEntitiesCursor FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
WHILE @@FETCH_STATUS = 0
BEGIN
if (@IsConstituent = 1 AND @StatusCode = ''CN'' AND @ConsolidationInvoiceId is not null)
BEGIN
set @ApprovedDate = (select ApprovedDate from ARInvoices
where IsConsolidationInvoice = 1
AND Id = @ConsolidationInvoiceId
AND IsAutoCredit = 0
AND StatusCode != ''DR''
AND StatusCode != ''LL''
AND StatusCode != ''VD''
AND StatusCode != ''AC''
)
END
if (@ApprovedDate is not null)
BEGIN
if (@FirstApprovalDate is null)
set @FirstApprovalDate = @ApprovedDate
else if (@FirstApprovalDate > @ApprovedDate)
set @FirstApprovalDate = @ApprovedDate
END
FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
END
CLOSE ARInvoiceEntitiesCursor
DEALLOCATE ARInvoiceEntitiesCursor
END
if (@ShipmentLevelCode = ''H'' AND @MasterDataId is not null)
BEGIN
if exists (select * from ShipmentMasterDatas where Id = @MasterDataId AND ProrateReceivables = 1)
begin
set @RegistryDate_Master = (select RegistryDate from Shipments where Id = @MasterDataId AND Tenant = @Tenant)
if (@RegistryDate_Master is not null)
begin
if (@FirstApprovalDate is null)
set @FirstApprovalDate = @RegistryDate_Master
else if (@FirstApprovalDate > @RegistryDate_Master)
set @FirstApprovalDate = @RegistryDate_Master
end
end
END
update Shipments set RegistryDate = @FirstApprovalDate where Id = @ShipmentId AND Tenant = @Tenant
if (@ShipmentLevelCode = ''C'')
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Shipments
WHERE ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @ShipmentId
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
EXECUTE [usp_UpdateShipmentRegistryDate] @HouseId
FETCH NEXT FROM ShipmentsCursor INTO @HouseId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END
END');


-- Trigger Script From AutomaticLastUpdateDateShipmentsTrigger.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipments] END');
EXEC('CREATE TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipments]
ON [dbo].[Shipments]
AFTER UPDATE
AS
BEGIN
UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
END;');


