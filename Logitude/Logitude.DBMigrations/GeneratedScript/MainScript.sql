-- Create New Table With Name DBMigrationsDataScriptCounters
CREATE TABLE [dbo].[DBMigrationsDataScriptCounters](
[TableName] VARCHAR(500) NOT NULL,
[LastCounter] INT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_DBMigrationsDataScriptCounters] PRIMARY KEY([TableName])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('956cfffd-c449-41fa-8f5f-5a02c852d218', 'DBMigrationsDataScriptCounters.dxml', 'DBMigrationsDataScriptCounters', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name DBMigrationsDataScriptCountersCREATE TABLE [dbo].[DBMigrationsDataScriptCounters]([TableName] VARCHAR(500) NOT NULL,[LastCounter] INT DEFAULT(0) NOT NULL,CONSTRAINT [PK_DBMigrationsDataScriptCounters] PRIMARY KEY([TableName]));');


-- Create New Table With Name DBMigrationsDataScripts
CREATE TABLE [dbo].[DBMigrationsDataScripts](
[Id] NVARCHAR(128) NOT NULL,
[SxmlFileName] VARCHAR(500) NOT NULL,
[DatabaseType] VARCHAR(15) NOT NULL,
[SxmlScript] NVARCHAR(MAX) NOT NULL,
[IsPreSxml] BIT DEFAULT(0) NOT NULL,
[Status] VARCHAR(25) NOT NULL,
[ScriptExecutionNumber] INT NOT NULL,
[StartDate] DATETIME NULL,
[EndDate] DATETIME NULL,
[LastBatchElapsedTime] INT NOT NULL,
[ScriptVersion] INT NOT NULL,
[ScriptHashValue] NVARCHAR(MAX) NOT NULL,
[ScriptHistoryAction] VARCHAR(10) NOT NULL,
[TargetTableName] VARCHAR(500) NOT NULL,
CONSTRAINT [PK_DBMigrationsDataScripts] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('74d9e195-4e29-433c-a3a6-b0d7b6fc4d6b', 'DBMigrationsDataScripts.dxml', 'DBMigrationsDataScripts', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name DBMigrationsDataScriptsCREATE TABLE [dbo].[DBMigrationsDataScripts]([Id] NVARCHAR(128) NOT NULL,[SxmlFileName] VARCHAR(500) NOT NULL,[DatabaseType] VARCHAR(15) NOT NULL,[SxmlScript] NVARCHAR(MAX) NOT NULL,[IsPreSxml] BIT DEFAULT(0) NOT NULL,[Status] VARCHAR(25) NOT NULL,[ScriptExecutionNumber] INT NOT NULL,[StartDate] DATETIME NULL,[EndDate] DATETIME NULL,[LastBatchElapsedTime] INT NOT NULL,[ScriptVersion] INT NOT NULL,[ScriptHashValue] NVARCHAR(MAX) NOT NULL,[ScriptHistoryAction] VARCHAR(10) NOT NULL,[TargetTableName] VARCHAR(500) NOT NULL,CONSTRAINT [PK_DBMigrationsDataScripts] PRIMARY KEY([Id]));');


-- Create New Table With Name DBMigrationsSetDefaultValues
CREATE TABLE [dbo].[DBMigrationsSetDefaultValues](
[Id] NVARCHAR(128) NOT NULL,
[DatabaseType] VARCHAR(15) NOT NULL,
[SchemaName] VARCHAR(15) NOT NULL,
[TableName] VARCHAR(500) NOT NULL,
[ColumnName] VARCHAR(500) NOT NULL,
[Status] VARCHAR(25) NOT NULL,
[DefaultValue] NVARCHAR(MAX) NOT NULL,
[UpdateNumber] INT NOT NULL,
[DoneRecordsCount] INT NOT NULL,
[StartDate] DATETIME NULL,
[EndDate] DATETIME NULL,
[LastBatchElapsedTime] INT NOT NULL,
CONSTRAINT [PK_DBMigrationsSetDefaultValues] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6a97314d-59b5-4018-b116-0e3ae25a1e23', 'DBMigrationsSetDefaultValues.dxml', 'DBMigrationsSetDefaultValues', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name DBMigrationsSetDefaultValuesCREATE TABLE [dbo].[DBMigrationsSetDefaultValues]([Id] NVARCHAR(128) NOT NULL,[DatabaseType] VARCHAR(15) NOT NULL,[SchemaName] VARCHAR(15) NOT NULL,[TableName] VARCHAR(500) NOT NULL,[ColumnName] VARCHAR(500) NOT NULL,[Status] VARCHAR(25) NOT NULL,[DefaultValue] NVARCHAR(MAX) NOT NULL,[UpdateNumber] INT NOT NULL,[DoneRecordsCount] INT NOT NULL,[StartDate] DATETIME NULL,[EndDate] DATETIME NULL,[LastBatchElapsedTime] INT NOT NULL,CONSTRAINT [PK_DBMigrationsSetDefaultValues] PRIMARY KEY([Id]));');


-- Create New Table With Name DBMigrationsSetValueCounters
CREATE TABLE [dbo].[DBMigrationsSetValueCounters](
[TableName] VARCHAR(500) NOT NULL,
[LastCounter] INT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_DBMigrationsSetValueCounters] PRIMARY KEY([TableName])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b86f471d-6f8f-4bf7-97b3-52562bb9a513', 'DBMigrationsSetValueCounters.dxml', 'DBMigrationsSetValueCounters', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name DBMigrationsSetValueCountersCREATE TABLE [dbo].[DBMigrationsSetValueCounters]([TableName] VARCHAR(500) NOT NULL,[LastCounter] INT DEFAULT(0) NOT NULL,CONSTRAINT [PK_DBMigrationsSetValueCounters] PRIMARY KEY([TableName]));');


-- Add New Column With Name ReportingAsAnotherDocument
ALTER TABLE [dbo].[GLAccounts] ADD [ReportingAsAnotherDocument] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('78205949-4842-4b96-a450-94b924de6ffa', 'GLAccount.dxml', 'GLAccounts', 'ReportingAsAnotherDocument', 'Add Column', GETDATE(), '-- Add New Column With Name ReportingAsAnotherDocumentALTER TABLE [dbo].[GLAccounts] ADD [ReportingAsAnotherDocument] BIT DEFAULT(0) NOT NULL;');


-- Change Size From 120 To 4000 For Column ProgressMessage
ALTER TABLE [dbo].[BatchTaskExecutions] ALTER COLUMN [ProgressMessage] NVARCHAR(4000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4eee64e1-98eb-4f5e-bef1-cef3989d5202', 'BatchTaskExecution.dxml', 'BatchTaskExecutions', 'ProgressMessage', 'Alter Column Size', GETDATE(), '-- Change Size From 120 To 4000 For Column ProgressMessageALTER TABLE [dbo].[BatchTaskExecutions] ALTER COLUMN [ProgressMessage] NVARCHAR(4000);');


-- Add New Column With Name PartnerObjectFieldCode
ALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('68c37806-4666-483d-8819-29e0ba1f7bf9', 'AutomationResultEmailRecipient.dxml', 'AutomationResultEmailRecipients', 'PartnerObjectFieldCode', 'Add Column', GETDATE(), '-- Add New Column With Name PartnerObjectFieldCodeALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;');


-- Add New Column With Name GLAccountDisplayNumber
ALTER TABLE [dbo].[Cards] ADD [GLAccountDisplayNumber] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7a5acf4d-8998-4bf3-a544-1b8df1ab35a5', 'Card.dxml', 'Cards', 'GLAccountDisplayNumber', 'Add Column', GETDATE(), '-- Add New Column With Name GLAccountDisplayNumberALTER TABLE [dbo].[Cards] ADD [GLAccountDisplayNumber] VARCHAR(15) NULL;');


-- Create New Table With Name CardSearches
CREATE TABLE [dbo].[CardSearches](
[Id] INT IDENTITY(1,1) NOT NULL,
[Tenant] INT NOT NULL,
[RecordDate] DATETIME NOT NULL,
[Keyword] NVARCHAR(100) NULL,
[Weight] INT NOT NULL,
[CardId] VARCHAR(15) NULL,
[PartnerTypeId] VARCHAR(2) NOT NULL,
[InActive] BIT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_CardSearches] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ea0c0d46-68e6-4a09-ab58-eec82851b0a6', 'CardSearch.dxml', 'CardSearches', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CardSearchesCREATE TABLE [dbo].[CardSearches]([Id] INT IDENTITY(1,1) NOT NULL,[Tenant] INT NOT NULL,[RecordDate] DATETIME NOT NULL,[Keyword] NVARCHAR(100) NULL,[Weight] INT NOT NULL,[CardId] VARCHAR(15) NULL,[PartnerTypeId] VARCHAR(2) NOT NULL,[InActive] BIT DEFAULT(0) NOT NULL,CONSTRAINT [PK_CardSearches] PRIMARY KEY([Id]));');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId]) INCLUDE([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97b1a6db-c131-46a5-9020-2d32c38dee07', 'CardSearch.dxml', 'CardSearches', 'Tenant,InActive,Keyword,PartnerTypeId', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId]) INCLUDE([CardId])'');');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_Weight] ON [dbo].[CardSearches]([Tenant],[Weight])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0a0f88e9-4759-43ff-9b11-ad8b9b4c93ff', 'CardSearch.dxml', 'CardSearches', 'Tenant,Weight', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_Weight] ON [dbo].[CardSearches]([Tenant],[Weight])'');');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_CardId] ON [dbo].[CardSearches]([Tenant],[CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d8dd0d23-6732-464d-938a-7e9dcee8ef07', 'CardSearch.dxml', 'CardSearches', 'Tenant,CardId', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_CardId] ON [dbo].[CardSearches]([Tenant],[CardId])'');');


-- Add New Column With Name OnSendPopulateDateFieldName
ALTER TABLE [dbo].[DocumentTypes] ADD [OnSendPopulateDateFieldName] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('207fdc87-3284-46bc-b01b-dcd31cd657cf', 'DocumentType.dxml', 'DocumentTypes', 'OnSendPopulateDateFieldName', 'Add Column', GETDATE(), '-- Add New Column With Name OnSendPopulateDateFieldNameALTER TABLE [dbo].[DocumentTypes] ADD [OnSendPopulateDateFieldName] VARCHAR(100) NULL;');

-- Add New Column With Name OnUploadPopulateDateFieldName
ALTER TABLE [dbo].[DocumentTypes] ADD [OnUploadPopulateDateFieldName] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f1717992-ab53-4cc6-b4db-4756df2cdd77', 'DocumentType.dxml', 'DocumentTypes', 'OnUploadPopulateDateFieldName', 'Add Column', GETDATE(), '-- Add New Column With Name OnUploadPopulateDateFieldNameALTER TABLE [dbo].[DocumentTypes] ADD [OnUploadPopulateDateFieldName] VARCHAR(100) NULL;');

-- Add New Column With Name OnPrintPopulateDateFieldName
ALTER TABLE [dbo].[DocumentTypes] ADD [OnPrintPopulateDateFieldName] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bd1ac7ef-47a4-4af9-bebd-d09d644d30cc', 'DocumentType.dxml', 'DocumentTypes', 'OnPrintPopulateDateFieldName', 'Add Column', GETDATE(), '-- Add New Column With Name OnPrintPopulateDateFieldNameALTER TABLE [dbo].[DocumentTypes] ADD [OnPrintPopulateDateFieldName] VARCHAR(100) NULL;');


-- Add New Column With Name PrivateUserName
ALTER TABLE [dbo].[DWHSettings] ADD [PrivateUserName] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5e26636b-0919-4304-a817-b8608167336d', 'DWHSetting.dxml', 'DWHSettings', 'PrivateUserName', 'Add Column', GETDATE(), '-- Add New Column With Name PrivateUserNameALTER TABLE [dbo].[DWHSettings] ADD [PrivateUserName] VARCHAR(200) NULL;');


-- Create New Table With Name Horses
CREATE TABLE [dbo].[Horses](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CreateDate] DATETIME NULL,
[CreatedByUserId] VARCHAR(15) NULL,
[UpdateDate] DATETIME NULL,
[UpdatedByUserId] VARCHAR(15) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Name] VARCHAR(200) NOT NULL,
[YearOfBirth] INT NULL,
[Color] VARCHAR(50) NULL,
[Gender] VARCHAR(50) NULL,
[Breed] VARCHAR(50) NULL,
[Discipline] VARCHAR(100) NULL,
[TravelBehavior] VARCHAR(100) NULL,
[MicochipNumber] VARCHAR(50) NOT NULL,
[PassportNumber] VARCHAR(50) NOT NULL,
[CountryOfBirthId] VARCHAR(15) NULL,
[CurrentStable] VARCHAR(100) NULL,
[Owner] VARCHAR(100) NULL,
[Remarks] NVARCHAR(500) NULL,
[Inactive] BIT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_Horses] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6b284174-5d97-4a58-8b9f-4fe7bb2e5d05', 'Horse.dxml', 'Horses', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name HorsesCREATE TABLE [dbo].[Horses]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CreateDate] DATETIME NULL,[CreatedByUserId] VARCHAR(15) NULL,[UpdateDate] DATETIME NULL,[UpdatedByUserId] VARCHAR(15) NULL,[SearchFields] NVARCHAR(MAX) NULL,[Name] VARCHAR(200) NOT NULL,[YearOfBirth] INT NULL,[Color] VARCHAR(50) NULL,[Gender] VARCHAR(50) NULL,[Breed] VARCHAR(50) NULL,[Discipline] VARCHAR(100) NULL,[TravelBehavior] VARCHAR(100) NULL,[MicochipNumber] VARCHAR(50) NOT NULL,[PassportNumber] VARCHAR(50) NOT NULL,[CountryOfBirthId] VARCHAR(15) NULL,[CurrentStable] VARCHAR(100) NULL,[Owner] VARCHAR(100) NULL,[Remarks] NVARCHAR(500) NULL,[Inactive] BIT DEFAULT(0) NOT NULL,CONSTRAINT [PK_Horses] PRIMARY KEY([Id]));');


-- Add New Column With Name IsIncrementalBuildRunning
ALTER TABLE [dbo].[Tenants] ADD [IsIncrementalBuildRunning] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('848c269a-ac07-43b2-9ac7-c49f9979c2be', 'Tenant.dxml', 'Tenants', 'IsIncrementalBuildRunning', 'Add Column', GETDATE(), '-- Add New Column With Name IsIncrementalBuildRunningALTER TABLE [dbo].[Tenants] ADD [IsIncrementalBuildRunning] BIT DEFAULT(0) NOT NULL;');


-- Add New Column With Name ChargeStorage
ALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('af83831f-5955-4cf9-90f0-734f2d467e2d', 'Warehouse.dxml', 'Warehouses', 'ChargeStorage', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name CurrencyId
ALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('846a7088-2400-4a08-bc57-2400a1ccbfa2', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Add Column', GETDATE(), '-- Add New Column With Name CurrencyIdALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;');

-- Add New Column With Name AirWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('69553853-4eb7-45d8-96df-b54b46436c3a', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f0f13dd9-685a-475e-8289-9a58e51cf035', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b9ae267b-1221-495e-b511-2342dddddfaf', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name AirWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e4c72286-ee73-4427-8dbc-bda927593356', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('81b334ea-1b71-461d-8f0d-51f5da57ff75', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8cb58f25-cc20-4c28-b0b4-a8795b6e9955', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;');


-- Create New Table With Name WarehouseStoragePricings
CREATE TABLE [dbo].[WarehouseStoragePricings](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[WarehouseId] VARCHAR(15) NULL,
[StepFrom] INT NOT NULL,
[StepTo] INT NULL,
[Days] INT NULL,
[SalePrice] DECIMAL(18, 3) NULL,
[LineNumber] INT NOT NULL,
CONSTRAINT [PK_WarehouseStoragePricings] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('05b1352a-705c-459c-bfe3-89631751c21d', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseStoragePricingsCREATE TABLE [dbo].[WarehouseStoragePricings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[WarehouseId] VARCHAR(15) NULL,[StepFrom] INT NOT NULL,[StepTo] INT NULL,[Days] INT NULL,[SalePrice] DECIMAL(18, 3) NULL,[LineNumber] INT NOT NULL,CONSTRAINT [PK_WarehouseStoragePricings] PRIMARY KEY([Id]));');


-- Create New Table With Name WarehouseWeightMeasurements
CREATE TABLE [dbo].[WarehouseWeightMeasurements](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1cf4049d-df62-4c7c-a01b-6960c3f17651', 'WarehouseWeightMeasurement.dxml', 'WarehouseWeightMeasurements', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightMeasurementsCREATE TABLE [dbo].[WarehouseWeightMeasurements]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code]));');


-- Create New Table With Name WarehouseWeightRoundings
CREATE TABLE [dbo].[WarehouseWeightRoundings](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Display] VARCHAR(20) NULL,
CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a4d9fc34-6c3d-48e4-ac8c-db39b9f37bba', 'WarehouseWeightRounding.dxml', 'WarehouseWeightRoundings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightRoundingsCREATE TABLE [dbo].[WarehouseWeightRoundings]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,[Display] VARCHAR(20) NULL,CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code]));');


-- Add New Column With Name ViewFieldDisplayName
ALTER TABLE [dbo].[DWObjectFields] ADD [ViewFieldDisplayName] NVARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1a61d797-9a62-4f68-af18-55e2eadfd364', 'DWObjectField.dxml', 'DWObjectFields', 'ViewFieldDisplayName', 'Add Column', GETDATE(), '-- Add New Column With Name ViewFieldDisplayNameALTER TABLE [dbo].[DWObjectFields] ADD [ViewFieldDisplayName] NVARCHAR(200) NULL;');

-- Add New Column With Name DontDisplayInView
ALTER TABLE [dbo].[DWObjectFields] ADD [DontDisplayInView] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4ac74270-47e2-4226-aff4-a42469d1b4b7', 'DWObjectField.dxml', 'DWObjectFields', 'DontDisplayInView', 'Add Column', GETDATE(), '-- Add New Column With Name DontDisplayInViewALTER TABLE [dbo].[DWObjectFields] ADD [DontDisplayInView] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name DimensionDataViewName
ALTER TABLE [dbo].[DWObjectFields] ADD [DimensionDataViewName] NVARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4ba3383d-c750-4fbb-b4d9-dde935163e90', 'DWObjectField.dxml', 'DWObjectFields', 'DimensionDataViewName', 'Add Column', GETDATE(), '-- Add New Column With Name DimensionDataViewNameALTER TABLE [dbo].[DWObjectFields] ADD [DimensionDataViewName] NVARCHAR(200) NULL;');

-- Add New Column With Name IsMultipleSelection
ALTER TABLE [dbo].[DWObjectFields] ADD [IsMultipleSelection] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('858f5d36-88ce-47f0-bc30-b33c86bcc45b', 'DWObjectField.dxml', 'DWObjectFields', 'IsMultipleSelection', 'Add Column', GETDATE(), '-- Add New Column With Name IsMultipleSelectionALTER TABLE [dbo].[DWObjectFields] ADD [IsMultipleSelection] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name RecordType
ALTER TABLE [dbo].[DWObjectFields] ADD [RecordType] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3144ecd8-25ae-4582-91cd-eaa6504e50aa', 'DWObjectField.dxml', 'DWObjectFields', 'RecordType', 'Add Column', GETDATE(), '-- Add New Column With Name RecordTypeALTER TABLE [dbo].[DWObjectFields] ADD [RecordType] VARCHAR(100) NULL;');


-- Add New Column With Name DataViewName
ALTER TABLE [dbo].[DWObjectTables] ADD [DataViewName] NVARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b5f38752-f0f5-453f-bedd-cb0454960c5c', 'DWObjectTable.dxml', 'DWObjectTables', 'DataViewName', 'Add Column', GETDATE(), '-- Add New Column With Name DataViewNameALTER TABLE [dbo].[DWObjectTables] ADD [DataViewName] NVARCHAR(200) NULL;');

-- Add New Column With Name HasPivotColumn
ALTER TABLE [dbo].[DWObjectTables] ADD [HasPivotColumn] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e6f9705a-527c-476f-b413-7f62dd7da79a', 'DWObjectTable.dxml', 'DWObjectTables', 'HasPivotColumn', 'Add Column', GETDATE(), '-- Add New Column With Name HasPivotColumnALTER TABLE [dbo].[DWObjectTables] ADD [HasPivotColumn] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name PivotFieldCode
ALTER TABLE [dbo].[DWObjectTables] ADD [PivotFieldCode] VARCHAR(50) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2e5c5e63-8aa2-49e4-8d3c-668b4d2f768b', 'DWObjectTable.dxml', 'DWObjectTables', 'PivotFieldCode', 'Add Column', GETDATE(), '-- Add New Column With Name PivotFieldCodeALTER TABLE [dbo].[DWObjectTables] ADD [PivotFieldCode] VARCHAR(50) NULL;');

-- Add New Column With Name AdditionalFactCode
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('45404dee-7f4d-41d6-b731-172860ca26fc', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactCode', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactCodeALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;');

-- Add New Column With Name AdditionalFactForeignKey
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d9dbeb08-78bd-4971-b57d-15bf4b8e5c3b', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactForeignKey', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactForeignKeyALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;');


-- Change Size From 15 To 30 For Column AccountNumber
ALTER TABLE [dbo].[BankAccountLites] ALTER COLUMN [AccountNumber] VARCHAR(30) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('572a7546-bcca-4a80-83c8-88a67d99d4fb', 'BankAccountLite.dxml', 'BankAccountLites', 'AccountNumber', 'Alter Column Size', GETDATE(), '-- Change Size From 15 To 30 For Column AccountNumberALTER TABLE [dbo].[BankAccountLites] ALTER COLUMN [AccountNumber] VARCHAR(30) NOT NULL;');


-- Add New Column With Name PickupDeliveryRatio
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryRatio] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('52292737-2924-451e-8260-70d0a65d56cc', 'Quote.dxml', 'Quotes', 'PickupDeliveryRatio', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryRatioALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryRatio] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryChargeableWeight
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryChargeableWeight] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('439ee798-d42e-4431-8182-77ae1c03cd1c', 'Quote.dxml', 'Quotes', 'PickupDeliveryChargeableWeight', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryChargeableWeightALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryChargeableWeight] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryVolumetricWeight
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('594c4fbd-5eb8-4414-b4b3-449609f71e9b', 'Quote.dxml', 'Quotes', 'PickupDeliveryVolumetricWeight', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryVolumetricWeightALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryCWeightUnitCode
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryCWeightUnitCode] VARCHAR(3) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cd0367b2-8115-4a61-b06b-b7282cca51b1', 'Quote.dxml', 'Quotes', 'PickupDeliveryCWeightUnitCode', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryCWeightUnitCodeALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryCWeightUnitCode] VARCHAR(3) NULL;');

-- Add New Column With Name RegionalTaxId
ALTER TABLE [dbo].[Quotes] ADD [RegionalTaxId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ed9e275d-678c-4ccf-9e3a-40be451dbd9b', 'Quote.dxml', 'Quotes', 'RegionalTaxId', 'Add Column', GETDATE(), '-- Add New Column With Name RegionalTaxIdALTER TABLE [dbo].[Quotes] ADD [RegionalTaxId] VARCHAR(15) NULL;');

-- Add New Column With Name RegionalTaxPercentage
ALTER TABLE [dbo].[Quotes] ADD [RegionalTaxPercentage] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7bf04372-eb09-40f0-af06-ce5ab14fa04c', 'Quote.dxml', 'Quotes', 'RegionalTaxPercentage', 'Add Column', GETDATE(), '-- Add New Column With Name RegionalTaxPercentageALTER TABLE [dbo].[Quotes] ADD [RegionalTaxPercentage] FLOAT NULL;');


-- Add New Column With Name IsRegionalTax
ALTER TABLE [dbo].[QuoteCharges] ADD [IsRegionalTax] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4f90bc20-63d5-4717-96d8-6e59bdb9b419', 'QuoteCharge.dxml', 'QuoteCharges', 'IsRegionalTax', 'Add Column', GETDATE(), '-- Add New Column With Name IsRegionalTaxALTER TABLE [dbo].[QuoteCharges] ADD [IsRegionalTax] BIT DEFAULT(0) NOT NULL;');


-- Create New Table With Name NONE
CREATE TABLE [dbo].[NONE](
[IsRegionalTax] BIT DEFAULT(0) NOT NULL,
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cbc7f294-cb69-4d48-9c38-b52694bf4232', 'QuoteCostCharge.dxml', 'NONE', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name NONECREATE TABLE [dbo].[NONE]([IsRegionalTax] BIT DEFAULT(0) NOT NULL,);');


-- Create New Table With Name NONE
CREATE TABLE [dbo].[NONE](
[IsRegionalTax] BIT DEFAULT(0) NOT NULL,
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d0f31e58-49dc-47d0-a2d9-08ad6b3ff60e', 'QuoteSaleCharge.dxml', 'NONE', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name NONECREATE TABLE [dbo].[NONE]([IsRegionalTax] BIT DEFAULT(0) NOT NULL,);');


-- Add New Column With Name AutomaticLastUpdateDate
ALTER TABLE [dbo].[OBLTypes] ADD [AutomaticLastUpdateDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('73ed768f-1c65-4f58-b3a5-cdac53825362', 'OBLType.dxml', 'OBLTypes', 'AutomaticLastUpdateDate', 'Add Column', GETDATE(), '-- Add New Column With Name AutomaticLastUpdateDateALTER TABLE [dbo].[OBLTypes] ADD [AutomaticLastUpdateDate] DATETIME NULL;');


-- Add New Column With Name ChargeStorage
ALTER TABLE [dbo].[Shipments] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('315d5049-6cf0-4f0e-a9df-2499372a9b8e', 'Shipment.dxml', 'Shipments', 'ChargeStorage', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageALTER TABLE [dbo].[Shipments] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name ChargeStorageCurrencyId
ALTER TABLE [dbo].[Shipments] ADD [ChargeStorageCurrencyId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1b01d2e4-6c3a-4df5-99af-a079087a14f7', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageCurrencyIdALTER TABLE [dbo].[Shipments] ADD [ChargeStorageCurrencyId] VARCHAR(15) NULL;');

-- Add New Column With Name WeightMeasurementCode
ALTER TABLE [dbo].[Shipments] ADD [WeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8dd84b51-c9da-40eb-aa2f-a4cad76851de', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name WeightMeasurementCodeALTER TABLE [dbo].[Shipments] ADD [WeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name WeightRoundingCode
ALTER TABLE [dbo].[Shipments] ADD [WeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3e751e5f-f33b-48e9-a6fb-86bf47564d39', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name WeightRoundingCodeALTER TABLE [dbo].[Shipments] ADD [WeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name IsBondedWarehouse
ALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouse] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6d405244-2552-4bcf-b96e-de1fc1353e08', 'Shipment.dxml', 'Shipments', 'IsBondedWarehouse', 'Add Column', GETDATE(), '-- Add New Column With Name IsBondedWarehouseALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouse] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name IsBondedWarehouseChanged
ALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouseChanged] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ac50eaf9-d3c9-4b6d-8744-3a2f3f302d04', 'Shipment.dxml', 'Shipments', 'IsBondedWarehouseChanged', 'Add Column', GETDATE(), '-- Add New Column With Name IsBondedWarehouseChangedALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouseChanged] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name PreAlertSentDate
ALTER TABLE [dbo].[Shipments] ADD [PreAlertSentDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('54010fe8-f75d-4e95-adfb-83496b07d907', 'Shipment.dxml', 'Shipments', 'PreAlertSentDate', 'Add Column', GETDATE(), '-- Add New Column With Name PreAlertSentDateALTER TABLE [dbo].[Shipments] ADD [PreAlertSentDate] DATETIME NULL;');

-- Add New Column With Name DeliveryNoticeSentDate
ALTER TABLE [dbo].[Shipments] ADD [DeliveryNoticeSentDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9ee9e063-f3b8-4734-9c63-a16b17bdea22', 'Shipment.dxml', 'Shipments', 'DeliveryNoticeSentDate', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryNoticeSentDateALTER TABLE [dbo].[Shipments] ADD [DeliveryNoticeSentDate] DATETIME NULL;');

-- Add New Column With Name ExpectedArrivalNoticeSentDate
ALTER TABLE [dbo].[Shipments] ADD [ExpectedArrivalNoticeSentDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6c6f2abc-0b9b-4969-bce5-6bfa9f6d490c', 'Shipment.dxml', 'Shipments', 'ExpectedArrivalNoticeSentDate', 'Add Column', GETDATE(), '-- Add New Column With Name ExpectedArrivalNoticeSentDateALTER TABLE [dbo].[Shipments] ADD [ExpectedArrivalNoticeSentDate] DATETIME NULL;');

-- Add New Column With Name T1ReceivedDate
ALTER TABLE [dbo].[Shipments] ADD [T1ReceivedDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d1f95383-efdd-4c08-81c9-27ef63c42011', 'Shipment.dxml', 'Shipments', 'T1ReceivedDate', 'Add Column', GETDATE(), '-- Add New Column With Name T1ReceivedDateALTER TABLE [dbo].[Shipments] ADD [T1ReceivedDate] DATETIME NULL;');

-- Add New Column With Name ArrivalNoticeSentDate
ALTER TABLE [dbo].[Shipments] ADD [ArrivalNoticeSentDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('669ce84f-d34b-4ee8-bc1e-09fb33fee928', 'Shipment.dxml', 'Shipments', 'ArrivalNoticeSentDate', 'Add Column', GETDATE(), '-- Add New Column With Name ArrivalNoticeSentDateALTER TABLE [dbo].[Shipments] ADD [ArrivalNoticeSentDate] DATETIME NULL;');


-- Add New Column With Name BookingConfirmationSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [BookingConfirmationSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fe3bffd9-5728-43eb-9dc8-4d07a09e82f0', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'BookingConfirmationSent', 'Add Column', GETDATE(), '-- Add New Column With Name BookingConfirmationSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [BookingConfirmationSent] DATETIME NULL;');

-- Add New Column With Name PreAlertSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PreAlertSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('215e99b4-9455-4a9d-9ea4-8b95844848d1', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PreAlertSent', 'Add Column', GETDATE(), '-- Add New Column With Name PreAlertSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [PreAlertSent] DATETIME NULL;');

-- Add New Column With Name DeliveryNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('40596fa6-22fd-4795-ba6a-436d6d33c893', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNoticeSent] DATETIME NULL;');

-- Add New Column With Name ExpectedArrivalNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [ExpectedArrivalNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('49dee619-c589-4741-848d-374652bd87a8', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ExpectedArrivalNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name ExpectedArrivalNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [ExpectedArrivalNoticeSent] DATETIME NULL;');

-- Add New Column With Name T1Received
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [T1Received] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5aab4097-98c5-4ea3-986b-859b094336b1', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'T1Received', 'Add Column', GETDATE(), '-- Add New Column With Name T1ReceivedALTER TABLE [dbo].[ShipmentComputedFields] ADD [T1Received] DATETIME NULL;');

-- Add New Column With Name ArrivalNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [ArrivalNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c91cbf59-5918-4bfe-97c0-eff8c0d3fa17', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ArrivalNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name ArrivalNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [ArrivalNoticeSent] DATETIME NULL;');

-- Add New Column With Name ContainersNumbersandTypesArray
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [ContainersNumbersandTypesArray] NVARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d80cf124-b26b-442d-a6f6-579887395590', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ContainersNumbersandTypesArray', 'Add Column', GETDATE(), '-- Add New Column With Name ContainersNumbersandTypesArrayALTER TABLE [dbo].[ShipmentComputedFields] ADD [ContainersNumbersandTypesArray] NVARCHAR(1000) NULL;');


-- Add New Column With Name HorseId
ALTER TABLE [dbo].[ShipmentPackages] ADD [HorseId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5c32560e-34cf-4934-a76b-b2e202231685', 'ShipmentPackage.dxml', 'ShipmentPackages', 'HorseId', 'Add Column', GETDATE(), '-- Add New Column With Name HorseIdALTER TABLE [dbo].[ShipmentPackages] ADD [HorseId] VARCHAR(15) NULL;');


-- Create New Table With Name ShipmentStoragePricings
CREATE TABLE [dbo].[ShipmentStoragePricings](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[ShipmentId] VARCHAR(15) NOT NULL,
[WarehouseId] VARCHAR(15) NOT NULL,
[StepFrom] INT NOT NULL,
[StepTo] INT NULL,
[Days] INT NULL,
[SalePrice] DECIMAL(18, 3) NULL,
[Amount] DECIMAL(18, 3) NULL,
[LineNumber] INT NOT NULL,
CONSTRAINT [PK_ShipmentStoragePricings] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7f579bc7-adb7-412a-813e-b8e9866f3a3e', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name ShipmentStoragePricingsCREATE TABLE [dbo].[ShipmentStoragePricings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[ShipmentId] VARCHAR(15) NOT NULL,[WarehouseId] VARCHAR(15) NOT NULL,[StepFrom] INT NOT NULL,[StepTo] INT NULL,[Days] INT NULL,[SalePrice] DECIMAL(18, 3) NULL,[Amount] DECIMAL(18, 3) NULL,[LineNumber] INT NOT NULL,CONSTRAINT [PK_ShipmentStoragePricings] PRIMARY KEY([Id]));');


-- Add New Column With Name OverManifest
ALTER TABLE [dbo].[WarehouseEntryPackages] ADD [OverManifest] INT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4110a61c-68f1-4082-ad9b-160ee17706ee', 'WarehouseEntryPackage.dxml', 'WarehouseEntryPackages', 'OverManifest', 'Add Column', GETDATE(), '-- Add New Column With Name OverManifestALTER TABLE [dbo].[WarehouseEntryPackages] ADD [OverManifest] INT DEFAULT(0) NOT NULL;');


-- Add Foreign Key Constraint For Column CardId In Table CardSearches As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[CardSearches] ADD CONSTRAINT [FK_CardSearches_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97295592-eaf6-4cf3-8f74-92396cc2e631', 'CardSearch.dxml', 'CardSearches', 'CardId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CardId In Table CardSearches As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[CardSearches] ADD CONSTRAINT [FK_CardSearches_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_CardId] ON [dbo].[CardSearches]([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0166638c-922a-4af6-ac36-0bdd97cf2caa', 'CardSearch.dxml', 'CardSearches', 'CardId', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_CardId] ON [dbo].[CardSearches]([CardId])'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table Horses As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a8fd728e-e839-4caa-a754-0b509513d9a8', 'Horse.dxml', 'Horses', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table Horses As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On Horses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Horses_CreatedByUserId] ON [dbo].[Horses]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3fe74caf-fe22-4cac-81cf-a9c84c61e36a', 'Horse.dxml', 'Horses', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On Horses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Horses_CreatedByUserId] ON [dbo].[Horses]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table Horses As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0bfbf302-138c-45e8-9a49-0b3654f3f673', 'Horse.dxml', 'Horses', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table Horses As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On Horses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Horses_UpdatedByUserId] ON [dbo].[Horses]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e5bee2c0-4252-4efa-ac84-c41b5527318b', 'Horse.dxml', 'Horses', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On Horses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Horses_UpdatedByUserId] ON [dbo].[Horses]([UpdatedByUserId])'');');

-- Add Foreign Key Constraint For Column CountryOfBirthId In Table Horses As Reference To Column Id In Table Countries
EXEC('ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Countries_CountryOfBirthId] FOREIGN KEY([CountryOfBirthId]) REFERENCES [dbo].[Countries]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('30554ddf-1073-4fc8-8bf4-333f3c4f5a3e', 'Horse.dxml', 'Horses', 'CountryOfBirthId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CountryOfBirthId In Table Horses As Reference To Column Id In Table CountriesEXEC(''ALTER TABLE [dbo].[Horses] ADD CONSTRAINT [FK_Horses_Countries_CountryOfBirthId] FOREIGN KEY([CountryOfBirthId]) REFERENCES [dbo].[Countries]([Id])'');');

-- Create Index On Horses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Horses_CountryOfBirthId] ON [dbo].[Horses]([CountryOfBirthId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d8a07af3-0fd9-474d-adc7-7be2968e5434', 'Horse.dxml', 'Horses', 'CountryOfBirthId', 'Create Index', GETDATE(), '-- Create Index On Horses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Horses_CountryOfBirthId] ON [dbo].[Horses]([CountryOfBirthId])'');');


-- Add Foreign Key Constraint For Column VatUniquePartnerTypeCode In Table Tenants As Reference To Column Code In Table VatUniquePartnerTypes
EXEC('ALTER TABLE [dbo].[Tenants] ADD CONSTRAINT [FK_Tenants_VatUniquePartnerTypes_VatUniquePartnerTypeCode] FOREIGN KEY([VatUniquePartnerTypeCode]) REFERENCES [dbo].[VatUniquePartnerTypes]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3d85ae2e-4b1c-4191-a568-6089445723c7', 'Tenant.dxml', 'Tenants', 'VatUniquePartnerTypeCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column VatUniquePartnerTypeCode In Table Tenants As Reference To Column Code In Table VatUniquePartnerTypesEXEC(''ALTER TABLE [dbo].[Tenants] ADD CONSTRAINT [FK_Tenants_VatUniquePartnerTypes_VatUniquePartnerTypeCode] FOREIGN KEY([VatUniquePartnerTypeCode]) REFERENCES [dbo].[VatUniquePartnerTypes]([Code])'');');

-- Create Index On Tenants Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Tenants_VatUniquePartnerTypeCode] ON [dbo].[Tenants]([VatUniquePartnerTypeCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('88f07be0-8988-451d-9241-ec194e047e4f', 'Tenant.dxml', 'Tenants', 'VatUniquePartnerTypeCode', 'Create Index', GETDATE(), '-- Create Index On Tenants TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Tenants_VatUniquePartnerTypeCode] ON [dbo].[Tenants]([VatUniquePartnerTypeCode])'');');


-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8a39a809-f987-4305-b59c-7baa923484de', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1288322c-d793-4ead-bf95-3650ed21b3f3', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])'');');

-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c3511a94-f553-4879-96a2-afd3c0eb60f2', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dc3ca949-f172-438c-bb29-6178bbf2642a', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97c3ed38-f08b-4acf-a351-946308a72d91', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8f1330c4-0258-45af-addb-d65e6777860d', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2b6737c6-671c-415d-a89f-1f92ef00b28d', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('86b38dd7-d164-41c5-860d-a07da6c6ab23', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fbeeb6d9-7bcd-49ba-9b7c-b9b0291d266f', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5afb0c5b-ce06-4062-b5fb-aac7eaf707d1', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e57aab5e-af11-402b-b8aa-609626c81bf7', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e71be2b6-4e69-4bda-847f-89c6f4de2383', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9796ebbc-bb22-4d94-b46a-497445af6533', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('00fded9d-69ce-4e93-98d7-164557d423e9', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])'');');


-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f36c4635-9e4c-44fa-a0d0-59992c2a806c', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On WarehouseStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d46acbba-0f13-470e-9b1d-2ad6582fb85a', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Index', GETDATE(), '-- Create Index On WarehouseStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])'');');


-- Add Foreign Key Constraint For Column RegionalTaxId In Table Quotes As Reference To Column Id In Table VatTypes
EXEC('ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_VatTypes_RegionalTaxId] FOREIGN KEY([RegionalTaxId]) REFERENCES [dbo].[VatTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('af9f196d-4b49-4c14-aeb9-e7afe293e734', 'Quote.dxml', 'Quotes', 'RegionalTaxId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column RegionalTaxId In Table Quotes As Reference To Column Id In Table VatTypesEXEC(''ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_VatTypes_RegionalTaxId] FOREIGN KEY([RegionalTaxId]) REFERENCES [dbo].[VatTypes]([Id])'');');

-- Create Index On Quotes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Quotes_RegionalTaxId] ON [dbo].[Quotes]([RegionalTaxId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('33b15943-223e-479d-b14c-2827c1c7da0f', 'Quote.dxml', 'Quotes', 'RegionalTaxId', 'Create Index', GETDATE(), '-- Create Index On Quotes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Quotes_RegionalTaxId] ON [dbo].[Quotes]([RegionalTaxId])'');');


-- Add Foreign Key Constraint For Column ChargeStorageCurrencyId In Table Shipments As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_Currencies_ChargeStorageCurrencyId] FOREIGN KEY([ChargeStorageCurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('88f04918-b822-49e4-b240-657965409449', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ChargeStorageCurrencyId In Table Shipments As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_Currencies_ChargeStorageCurrencyId] FOREIGN KEY([ChargeStorageCurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_ChargeStorageCurrencyId] ON [dbo].[Shipments]([ChargeStorageCurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f7fcaa23-cc50-4f3f-9359-9b3d394c491c', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_ChargeStorageCurrencyId] ON [dbo].[Shipments]([ChargeStorageCurrencyId])'');');

-- Add Foreign Key Constraint For Column WeightMeasurementCode In Table Shipments As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightMeasurements_WeightMeasurementCode] FOREIGN KEY([WeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8c0e61f2-7c1b-4e00-8cff-10d0488b4f2f', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WeightMeasurementCode In Table Shipments As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightMeasurements_WeightMeasurementCode] FOREIGN KEY([WeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_WeightMeasurementCode] ON [dbo].[Shipments]([WeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e3db2ab4-308a-4a10-8cb1-341ead55b204', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_WeightMeasurementCode] ON [dbo].[Shipments]([WeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column WeightRoundingCode In Table Shipments As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightRoundings_WeightRoundingCode] FOREIGN KEY([WeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c0a524db-4a4b-4ea6-bc05-2641b9c09c35', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WeightRoundingCode In Table Shipments As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightRoundings_WeightRoundingCode] FOREIGN KEY([WeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_WeightRoundingCode] ON [dbo].[Shipments]([WeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8f497e22-64f6-4293-a9ee-43e5f8aa0f4d', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_WeightRoundingCode] ON [dbo].[Shipments]([WeightRoundingCode])'');');


-- Add Foreign Key Constraint For Column HorseId In Table ShipmentPackages As Reference To Column Id In Table Horses
EXEC('ALTER TABLE [dbo].[ShipmentPackages] ADD CONSTRAINT [FK_ShipmentPackages_Horses_HorseId] FOREIGN KEY([HorseId]) REFERENCES [dbo].[Horses]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fdcb48e0-746e-4712-9889-da1ad973b754', 'ShipmentPackage.dxml', 'ShipmentPackages', 'HorseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column HorseId In Table ShipmentPackages As Reference To Column Id In Table HorsesEXEC(''ALTER TABLE [dbo].[ShipmentPackages] ADD CONSTRAINT [FK_ShipmentPackages_Horses_HorseId] FOREIGN KEY([HorseId]) REFERENCES [dbo].[Horses]([Id])'');');

-- Create Index On ShipmentPackages Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentPackages_HorseId] ON [dbo].[ShipmentPackages]([HorseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2f7e68e8-1229-4bb4-9f6e-aad0008be43d', 'ShipmentPackage.dxml', 'ShipmentPackages', 'HorseId', 'Create Index', GETDATE(), '-- Create Index On ShipmentPackages TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentPackages_HorseId] ON [dbo].[ShipmentPackages]([HorseId])'');');


-- Add Foreign Key Constraint For Column ShipmentId In Table ShipmentStoragePricings As Reference To Column Id In Table Shipments
EXEC('ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Shipments_ShipmentId] FOREIGN KEY([ShipmentId]) REFERENCES [dbo].[Shipments]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('94427d99-7a1a-4905-b6b4-5f601dc2ad07', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'ShipmentId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentId In Table ShipmentStoragePricings As Reference To Column Id In Table ShipmentsEXEC(''ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Shipments_ShipmentId] FOREIGN KEY([ShipmentId]) REFERENCES [dbo].[Shipments]([Id])'');');

-- Create Index On ShipmentStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_ShipmentId] ON [dbo].[ShipmentStoragePricings]([ShipmentId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('993e858e-39be-4556-b2c5-5a185f40d734', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'ShipmentId', 'Create Index', GETDATE(), '-- Create Index On ShipmentStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_ShipmentId] ON [dbo].[ShipmentStoragePricings]([ShipmentId])'');');

-- Add Foreign Key Constraint For Column WarehouseId In Table ShipmentStoragePricings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1240f10f-0fe9-47d6-8bd0-6d6129ac5931', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'WarehouseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WarehouseId In Table ShipmentStoragePricings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On ShipmentStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_WarehouseId] ON [dbo].[ShipmentStoragePricings]([WarehouseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('753c1741-f13c-41a8-8c8a-61efbad26be7', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'WarehouseId', 'Create Index', GETDATE(), '-- Create Index On ShipmentStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_WarehouseId] ON [dbo].[ShipmentStoragePricings]([WarehouseId])'');');


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
ActivationRequestedByUserContacts.EnglishName as ActivationRequestedByUserName, dbo.Customers.ActivationDate, dbo.Customers.InactiveDate, dbo.Customers.ActivationRequestDate,dbo.cards.CreatedByPartner, dbo.cards.StateName, dbo.cards.GLAccountId , dbo.cards.GLAccountDisplayNumber
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
dbo.ShipmentMasterDatas.ImportManifest,
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
dbo.Shipments.ShipmentSubTypeId,
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
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,dbo.Shipments.WarehouseStorageFreeDays,
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
dbo.ShipmentSubTypes.Name AS ShipmentSubTypeName,
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
ShipmentComputedFields.CreatedFromDigital as CreatedFromDigital,
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
dbo.Shipments.ComputedStatusDate AS StatusDate,
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
dbo.ShipmentSubTypes ON dbo.Shipments.ShipmentSubTypeId = dbo.ShipmentSubTypes.Id LEFT OUTER JOIN
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


-- Procedure Script From usp_UpdateCardSearchFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCardSearchFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCardSearchFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCardSearchFunction]
(
@CardId varchar(15)
)
AS
declare  @Tenant int
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
if (@CardId is not null)
begin
delete CardSearches where CardId = @CardId
select
@Tenant = Tenant,
@Code = Code,
@EnglishName = EnglishName,
@LocalName = LocalName,
@VatNumber = VatNumber,
@CityName = CityName,
@CountryName =CountryName,
@ReceivablesAccountingCard = ReceivablesAccountingCard,
@PayablesAccountingCard = PayablesAccountingCard,
@CreateDate = CreateDate,
@UpdateDate = UpdateDate,
@PartnerTypeId = PartnerTypeId,
@InActive = InActive
from Cards
where Id = @CardId
set @Weight = 0
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
if (@Code is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !='' '' end
if (@EnglishName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !='' '' end
if (@LocalName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !='' '' end
if (@VatNumber is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !='' '' end
if (@CountryName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !='' '' end
if (@CityName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !='' '' end
if (@ReceivablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
if (@PayablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
end');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipments] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipments ON Shipments AFTER UPDATE  AS  BEGIN SET NOCOUNT ON; UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- General Script From 202007201431_FillWarehouseWeightClosedTables.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from WarehouseWeightMeasurements where Code = 'GRWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('GRWT', 'Gross Weight', 'GRWT,Gross Weight')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = 'CHWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('CHWT', 'Chargeable Weight', 'CHWT,Chargeable Weight')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'NON')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('NON', 'None', 'NON,None', 'None')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'HAF')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('HAF', 'Half', 'HAF,Half', '0.5')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'ONE')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('ONE', 'One', 'ONE,One', '1')
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201431_FillWarehouseWeightClosedTables.sxml', GETDATE(), 'if not exists (select Code from WarehouseWeightMeasurements where Code = ''GRWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''GRWT'', ''Gross Weight'', ''GRWT,Gross Weight'')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = ''CHWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''CHWT'', ''Chargeable Weight'', ''CHWT,Chargeable Weight'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''NON'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''NON'', ''None'', ''NON,None'', ''None'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''HAF'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''HAF'', ''Half'', ''HAF,Half'', ''0.5'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''ONE'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''ONE'', ''One'', ''ONE,One'', ''1'')
end', DATEDIFF(MS,@StartTime,@EndTime), '5a31949cce6467cce78c87ba8d9dff83', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007201436_FillWarehouseFieldsDefaultValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Warehouses set AirWeightMeasurementCode = 'GRWT'
update Warehouses set OceanWeightMeasurementCode = 'GRWT'
update Warehouses set InlandWeightMeasurementCode = 'GRWT'
update Warehouses set AirWeightRoundingCode = 'NON'
update Warehouses set OceanWeightRoundingCode = 'NON'
update Warehouses set InlandWeightRoundingCode = 'NON'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201436_FillWarehouseFieldsDefaultValues.sxml', GETDATE(), 'update Warehouses set AirWeightMeasurementCode = ''GRWT''
update Warehouses set OceanWeightMeasurementCode = ''GRWT''
update Warehouses set InlandWeightMeasurementCode = ''GRWT''
update Warehouses set AirWeightRoundingCode = ''NON''
update Warehouses set OceanWeightRoundingCode = ''NON''
update Warehouses set InlandWeightRoundingCode = ''NON''', DATEDIFF(MS,@StartTime,@EndTime), 'b17d9a558fa92bab0d7aece163282d61', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008051018_AddNewMeasurementAndChargesType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = 'STFE')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('STFE', 'Storage Fee', 'Storage Fee', @MeasurementId, @Tenant, 0, 0, 0, 'STFE,Storage Fee,Storage Fee', 'Storage Fee')
end
--ChargesType
set @ChargesGroupCode = 'HNDCH'
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = 'HNDCH')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = 'STD')
set @IATACodeId = (select Id from IATACodes where Code = 'SO')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = 'ISTOR')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,'ChargesType'
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values('ISTOR', 'Import Storage', 'Import Storage', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, 'AG',
0, @MeasurementId, NULL, 70,'ISTOR,Import Storage,Import Storage', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'Measurement')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008051018_AddNewMeasurementAndChargesType.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = ''STFE'')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''STFE'', ''Storage Fee'', ''Storage Fee'', @MeasurementId, @Tenant, 0, 0, 0, ''STFE,Storage Fee,Storage Fee'', ''Storage Fee'')
end
--ChargesType
set @ChargesGroupCode = ''HNDCH''
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = ''HNDCH'')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = ''STD'')
set @IATACodeId = (select Id from IATACodes where Code = ''SO'')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = ''ISTOR'')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,''ChargesType''
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values(''ISTOR'', ''Import Storage'', ''Import Storage'', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, ''AG'',
0, @MeasurementId, NULL, 70,''ISTOR,Import Storage,Import Storage'', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''Measurement'')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), 'b252e3857be84e566299715d80b06883', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008110941_AddNewMeasurement.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'PDCW')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('PDCW', 'Pickup/Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight', @MeasurementId, @Tenant, 0, 0, 0, 'PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008110941_AddNewMeasurement.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = ''PDCW'')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''PDCW'', ''Pickup/Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'', @MeasurementId, @Tenant, 0, 0, 0, ''PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '34b9d9902a50650ec4ac4fa4aa87bfe1', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 20200812_SetIATACodeToSRForImportStorageCharge.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = 'SR')
where Code = 'ISTOR'
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('20200812_SetIATACodeToSRForImportStorageCharge.sxml', GETDATE(), 'update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = ''SR'')
where Code = ''ISTOR''
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), '60d427a5e1ea664ef4e7d5766423a171', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008261425_SetToggleCodeForHorsesFeature.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from Toggles where Code = 'HRS')
begin
insert into Toggles (Code, Name, SearchFields)
values ('HRS', 'Horse', 'HRS,Horse')
end
update Features
set ToggleCode = 'HRS'
where Code = 'Horse.M.Horses'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008261425_SetToggleCodeForHorsesFeature.sxml', GETDATE(), 'if not exists (select Code from Toggles where Code = ''HRS'')
begin
insert into Toggles (Code, Name, SearchFields)
values (''HRS'', ''Horse'', ''HRS,Horse'')
end
update Features
set ToggleCode = ''HRS''
where Code = ''Horse.M.Horses''', DATEDIFF(MS,@StartTime,@EndTime), '3e01d0de48c070160eed409222a814c8', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008300915_AddHorsesEventTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = 'QuoteClosingReason')
set @Code = 'HRIN'
set @Name = 'Set as Inactive'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = 'HRRC'
set @Name = 'Reactivated'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008300915_AddHorsesEventTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = ''QuoteClosingReason'')
set @Code = ''HRIN''
set @Name = ''Set as Inactive''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = ''HRRC''
set @Name = ''Reactivated''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '5f18abe5124698d5536097a0706ca9fb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009011450_ImportToUSADropMaman.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete from CustomsInterfaces where code = 'CMN'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009011450_ImportToUSADropMaman.sxml', GETDATE(), 'delete from CustomsInterfaces where code = ''CMN''', DATEDIFF(MS,@StartTime,@EndTime), '830638ae226d14b30e478d0b45d1f45b', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009031302_FillDataFromShipmentPackagesToShipmentComputedFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#temp') Is Not Null)
Begin
Drop Table #temp
End
If(OBJECT_ID('tempdb..#tempGrouped') Is Not Null)
Begin
Drop Table #tempGrouped
End
If(OBJECT_ID('tempdb..#temp_ShipmentComputedFields') Is Not Null)
Begin
Drop Table #temp_ShipmentComputedFields
End
CREATE TABLE #temp_ShipmentComputedFields
(
Id varchar(15) not null,
ContainersNumbersandTypesArray varchar(1000) null
)
select
Shipments.Id, ShipmentPackages.ContainerNumber as ContainerNumber, PackageTypes.Code as PackageTypeCode
into #temp
from Shipments
left outer join ShipmentPackages on Shipments.Id = ShipmentPackages.ShipmentId
left outer join PackageTypes on ShipmentPackages.PackageTypeId = PackageTypes.Id
SELECT DISTINCT temp3.Id,
SUBSTRING(
(
SELECT ','+temp1.ContainerNumber+'['+temp1.PackageTypeCode+']'
FROM #temp temp1
WHERE temp1.Id = temp3.Id
ORDER BY temp1.Id
FOR XML PATH ('')
), 2, 1000) ContainersNumbersandTypesArray
into #tempGrouped
FROM #temp temp3
declare @Id as varchar(15)
declare @ContainersNumbersandTypesArray as varchar(1000)
declare @Count as int
set @Count = 0;
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, ContainersNumbersandTypesArray
FROM #tempGrouped
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_ShipmentComputedFields(Id, ContainersNumbersandTypesArray) values(@Id, @ContainersNumbersandTypesArray)
set @Count = @Count + 1;
if(@Count = 1000)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
truncate table #temp_ShipmentComputedFields
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
END
CLOSE DataCursor
DEALLOCATE DataCursor
if (@Count > 0)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
end
drop table #temp
drop table #tempGrouped
drop table #temp_ShipmentComputedFields
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009031302_FillDataFromShipmentPackagesToShipmentComputedFields.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#temp'') Is Not Null)
Begin
Drop Table #temp
End
If(OBJECT_ID(''tempdb..#tempGrouped'') Is Not Null)
Begin
Drop Table #tempGrouped
End
If(OBJECT_ID(''tempdb..#temp_ShipmentComputedFields'') Is Not Null)
Begin
Drop Table #temp_ShipmentComputedFields
End
CREATE TABLE #temp_ShipmentComputedFields
(
Id varchar(15) not null,
ContainersNumbersandTypesArray varchar(1000) null
)
select
Shipments.Id, ShipmentPackages.ContainerNumber as ContainerNumber, PackageTypes.Code as PackageTypeCode
into #temp
from Shipments
left outer join ShipmentPackages on Shipments.Id = ShipmentPackages.ShipmentId
left outer join PackageTypes on ShipmentPackages.PackageTypeId = PackageTypes.Id
SELECT DISTINCT temp3.Id,
SUBSTRING(
(
SELECT '',''+temp1.ContainerNumber+''[''+temp1.PackageTypeCode+'']''
FROM #temp temp1
WHERE temp1.Id = temp3.Id
ORDER BY temp1.Id
FOR XML PATH ('''')
), 2, 1000) ContainersNumbersandTypesArray
into #tempGrouped
FROM #temp temp3
declare @Id as varchar(15)
declare @ContainersNumbersandTypesArray as varchar(1000)
declare @Count as int
set @Count = 0;
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, ContainersNumbersandTypesArray
FROM #tempGrouped
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_ShipmentComputedFields(Id, ContainersNumbersandTypesArray) values(@Id, @ContainersNumbersandTypesArray)
set @Count = @Count + 1;
if(@Count = 1000)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
truncate table #temp_ShipmentComputedFields
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
END
CLOSE DataCursor
DEALLOCATE DataCursor
if (@Count > 0)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
end
drop table #temp
drop table #tempGrouped
drop table #temp_ShipmentComputedFields', DATEDIFF(MS,@StartTime,@EndTime), '1a2368963f979ff3a8286b129ace299f', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

