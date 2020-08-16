-- Set Nullable For Column ReconcileExternalPageLineId
ALTER TABLE [dbo].[JournalExternalReconciles] ALTER COLUMN [ReconcileExternalPageLineId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e034fae2-5c23-49f5-93bd-35c022bd82b4', 'JournalExternalReconcile.dxml', 'JournalExternalReconciles', 'ReconcileExternalPageLineId', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ReconcileExternalPageLineIdALTER TABLE [dbo].[JournalExternalReconciles] ALTER COLUMN [ReconcileExternalPageLineId] VARCHAR(15) NULL;');


-- Change Size From 120 To 4000 For Column ProgressMessage
ALTER TABLE [dbo].[BatchTaskExecutions] ALTER COLUMN [ProgressMessage] NVARCHAR(4000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e044e67f-c798-4b77-835b-27e44c442e56', 'BatchTaskExecution.dxml', 'BatchTaskExecutions', 'ProgressMessage', 'Alter Column Size', GETDATE(), '-- Change Size From 120 To 4000 For Column ProgressMessageALTER TABLE [dbo].[BatchTaskExecutions] ALTER COLUMN [ProgressMessage] NVARCHAR(4000);');


-- Add New Column With Name PartnerObjectFieldCode
ALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('747a49d3-67f4-4828-9375-ebde274a3553', 'AutomationResultEmailRecipient.dxml', 'AutomationResultEmailRecipients', 'PartnerObjectFieldCode', 'Add Column', GETDATE(), '-- Add New Column With Name PartnerObjectFieldCodeALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;');


-- Change Type From nvarchar To varchar For Column CompetitorFields
ALTER TABLE [dbo].[Customers] ALTER COLUMN [CompetitorFields] VARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9610f573-5288-421d-9280-e07ee96bb871', 'Customer.dxml', 'Customers', 'CompetitorFields', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column CompetitorFieldsALTER TABLE [dbo].[Customers] ALTER COLUMN [CompetitorFields] VARCHAR(1000);');


-- Add New Column With Name ChargeStorage
ALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4819922f-b057-4b48-98e2-244b7ac12bf5', 'Warehouse.dxml', 'Warehouses', 'ChargeStorage', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name CurrencyId
ALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7c1538ed-3e9f-4549-85e4-3fe84d5104bd', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Add Column', GETDATE(), '-- Add New Column With Name CurrencyIdALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;');

-- Add New Column With Name AirWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ea22424e-24b2-45e2-b42b-999314da73e0', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2c9b9ef3-641c-4fb0-8117-17b306a2fb10', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('178df7eb-31a1-4b95-9a89-cc9006cf27eb', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name AirWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f64705eb-bb79-4157-b61a-5726e82e1f5a', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a299c5e5-c496-470e-b2ee-d5f9c8d3866b', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b28c6a17-2b3b-4da4-8858-0d6a8f8b8782', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;');


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

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3ded23d6-08ba-41d2-8895-b7c31db45132', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseStoragePricingsCREATE TABLE [dbo].[WarehouseStoragePricings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[WarehouseId] VARCHAR(15) NULL,[StepFrom] INT NOT NULL,[StepTo] INT NULL,[Days] INT NULL,[SalePrice] DECIMAL(18, 3) NULL,[LineNumber] INT NOT NULL,CONSTRAINT [PK_WarehouseStoragePricings] PRIMARY KEY([Id]));');


-- Create New Table With Name WarehouseWeightMeasurements
CREATE TABLE [dbo].[WarehouseWeightMeasurements](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4cdd508e-f84b-49da-b20d-f9155fe74217', 'WarehouseWeightMeasurement.dxml', 'WarehouseWeightMeasurements', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightMeasurementsCREATE TABLE [dbo].[WarehouseWeightMeasurements]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code]));');


-- Create New Table With Name WarehouseWeightRoundings
CREATE TABLE [dbo].[WarehouseWeightRoundings](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Display] VARCHAR(20) NULL,
CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f606d91a-81e4-41b6-94e6-dd4125d6249e', 'WarehouseWeightRounding.dxml', 'WarehouseWeightRoundings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightRoundingsCREATE TABLE [dbo].[WarehouseWeightRoundings]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,[Display] VARCHAR(20) NULL,CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code]));');


-- Add New Column With Name RecordType
ALTER TABLE [dbo].[DWObjectFields] ADD [RecordType] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('864bea97-855f-4131-8e43-99e20ea4dd29', 'DWObjectField.dxml', 'DWObjectFields', 'RecordType', 'Add Column', GETDATE(), '-- Add New Column With Name RecordTypeALTER TABLE [dbo].[DWObjectFields] ADD [RecordType] VARCHAR(100) NULL;');


-- Add New Column With Name AdditionalFactCode
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('47f40d1c-9524-43a2-8971-0ec631f3894f', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactCode', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactCodeALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;');

-- Add New Column With Name AdditionalFactForeignKey
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bbbc70cb-a2c7-4f54-98a2-292b12febaf5', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactForeignKey', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactForeignKeyALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;');


-- Change Size From 15 To 20 For Column ShipmentNumber
ALTER TABLE [dbo].[ARPayments] ALTER COLUMN [ShipmentNumber] VARCHAR(20);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1a88576e-cb17-4a01-ae51-0e9249af7d19', 'ARPayment.dxml', 'ARPayments', 'ShipmentNumber', 'Alter Column Size', GETDATE(), '-- Change Size From 15 To 20 For Column ShipmentNumberALTER TABLE [dbo].[ARPayments] ALTER COLUMN [ShipmentNumber] VARCHAR(20);');


-- Create New Table With Name ARPaymentChequeReplicas
CREATE TABLE [dbo].[ARPaymentChequeReplicas](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[SearchFields] NVARCHAR(4000) NULL,
[PaymentId] VARCHAR(15) NOT NULL,
[LineNumber] INT NOT NULL,
[ChequeNumber] VARCHAR(15) NOT NULL,
[ValueDate] DATE NOT NULL,
[CurrencyId] VARCHAR(15) NOT NULL,
[LocalAmount] DECIMAL(16, 2) NOT NULL,
[ForeignAmount] DECIMAL(16, 2) NOT NULL,
[BankId] VARCHAR(15) NULL,
[BankBranch] VARCHAR(30) NOT NULL,
[BankAccount] VARCHAR(15) NOT NULL,
[StatusCode] VARCHAR(15) NULL,
[ExchangeRate] DECIMAL(5, 3) NULL,
CONSTRAINT [PK_ARPaymentChequeReplicas] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7d4d380d-1496-4055-8349-cfafac6ef132', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name ARPaymentChequeReplicasCREATE TABLE [dbo].[ARPaymentChequeReplicas]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[SearchFields] NVARCHAR(4000) NULL,[PaymentId] VARCHAR(15) NOT NULL,[LineNumber] INT NOT NULL,[ChequeNumber] VARCHAR(15) NOT NULL,[ValueDate] DATE NOT NULL,[CurrencyId] VARCHAR(15) NOT NULL,[LocalAmount] DECIMAL(16, 2) NOT NULL,[ForeignAmount] DECIMAL(16, 2) NOT NULL,[BankId] VARCHAR(15) NULL,[BankBranch] VARCHAR(30) NOT NULL,[BankAccount] VARCHAR(15) NOT NULL,[StatusCode] VARCHAR(15) NULL,[ExchangeRate] DECIMAL(5, 3) NULL,CONSTRAINT [PK_ARPaymentChequeReplicas] PRIMARY KEY([Id]));');


-- Add New Column With Name PickupDeliveryRatio
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryRatio] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bd945f7e-7ced-43be-9056-02e76e05fcff', 'Quote.dxml', 'Quotes', 'PickupDeliveryRatio', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryRatioALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryRatio] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryChargeableWeight
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryChargeableWeight] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1e9bee2c-9ffa-4958-af88-4ddf2d1adeee', 'Quote.dxml', 'Quotes', 'PickupDeliveryChargeableWeight', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryChargeableWeightALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryChargeableWeight] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryVolumetricWeight
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f4daae8d-eb47-4034-8c7b-e0c0ade02fa7', 'Quote.dxml', 'Quotes', 'PickupDeliveryVolumetricWeight', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryVolumetricWeightALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;');


-- Add New Column With Name PickupDeliveryVolumetricWeight
ALTER TABLE [dbo].[QuotePackages] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('262eab12-4340-4ee8-9e97-f6afeaa8b9bb', 'QuotePackage.dxml', 'QuotePackages', 'PickupDeliveryVolumetricWeight', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryVolumetricWeightALTER TABLE [dbo].[QuotePackages] ADD [PickupDeliveryVolumetricWeight] FLOAT NULL;');

-- Add New Column With Name PickupDeliveryVolume
ALTER TABLE [dbo].[QuotePackages] ADD [PickupDeliveryVolume] FLOAT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aa00c11b-a154-4c06-92fd-e732d391ba51', 'QuotePackage.dxml', 'QuotePackages', 'PickupDeliveryVolume', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryVolumeALTER TABLE [dbo].[QuotePackages] ADD [PickupDeliveryVolume] FLOAT NULL;');


-- Add New Column With Name ChargeStorage
ALTER TABLE [dbo].[Shipments] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e4280956-08b5-42bf-a54d-47ba1f7505fc', 'Shipment.dxml', 'Shipments', 'ChargeStorage', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageALTER TABLE [dbo].[Shipments] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name ChargeStorageCurrencyId
ALTER TABLE [dbo].[Shipments] ADD [ChargeStorageCurrencyId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8047a051-e0d2-45d6-b664-04aaf2a1863a', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageCurrencyIdALTER TABLE [dbo].[Shipments] ADD [ChargeStorageCurrencyId] VARCHAR(15) NULL;');

-- Add New Column With Name WeightMeasurementCode
ALTER TABLE [dbo].[Shipments] ADD [WeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('34c31e2b-e9c0-4e1c-b473-81d564e1a817', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name WeightMeasurementCodeALTER TABLE [dbo].[Shipments] ADD [WeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name WeightRoundingCode
ALTER TABLE [dbo].[Shipments] ADD [WeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b51f4b61-2c0d-43bf-ac53-e9968d10a30b', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name WeightRoundingCodeALTER TABLE [dbo].[Shipments] ADD [WeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name IsBondedWarehouse
ALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouse] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bfb590b0-d127-48b3-87b8-7f0f629608b5', 'Shipment.dxml', 'Shipments', 'IsBondedWarehouse', 'Add Column', GETDATE(), '-- Add New Column With Name IsBondedWarehouseALTER TABLE [dbo].[Shipments] ADD [IsBondedWarehouse] BIT DEFAULT(0) NOT NULL;');

-- Create Unique Constraint On Shipments Table
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [UQ_Shipments_Tenant_ComputedForwarderShipmentNumber] UNIQUE([Tenant],[ComputedForwarderShipmentNumber])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('422615b9-16c2-4afc-be5e-f7125c6eac34', 'Shipment.dxml', 'Shipments', 'Tenant,ComputedForwarderShipmentNumber', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On Shipments TableEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [UQ_Shipments_Tenant_ComputedForwarderShipmentNumber] UNIQUE([Tenant],[ComputedForwarderShipmentNumber])'');');


-- Add New Column With Name BookingConfirmationSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [BookingConfirmationSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('09b32d9b-54b7-4530-9a34-c36c0ad4df8a', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'BookingConfirmationSent', 'Add Column', GETDATE(), '-- Add New Column With Name BookingConfirmationSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [BookingConfirmationSent] DATETIME NULL;');

-- Add New Column With Name PreAlertSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PreAlertSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('63bf9aa0-f869-479f-bde1-c5d44199b306', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PreAlertSent', 'Add Column', GETDATE(), '-- Add New Column With Name PreAlertSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [PreAlertSent] DATETIME NULL;');

-- Add New Column With Name DeliveryNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9a317221-5d8b-499d-8a00-5a9ca108857c', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNoticeSent] DATETIME NULL;');

-- Add New Column With Name ExpectedArrivalNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [ExpectedArrivalNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b6bbc13d-0faa-49f0-91be-b297aba5472e', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ExpectedArrivalNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name ExpectedArrivalNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [ExpectedArrivalNoticeSent] DATETIME NULL;');

-- Add New Column With Name T1Received
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [T1Received] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('143c0797-b4c1-4235-a2e4-22c96e07581e', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'T1Received', 'Add Column', GETDATE(), '-- Add New Column With Name T1ReceivedALTER TABLE [dbo].[ShipmentComputedFields] ADD [T1Received] DATETIME NULL;');

-- Add New Column With Name ArrivalNoticeSent
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [ArrivalNoticeSent] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('67a9db80-4478-46f1-8299-288113729f1b', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ArrivalNoticeSent', 'Add Column', GETDATE(), '-- Add New Column With Name ArrivalNoticeSentALTER TABLE [dbo].[ShipmentComputedFields] ADD [ArrivalNoticeSent] DATETIME NULL;');


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

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4782c6c6-4dfe-4fa2-9176-51a6e15f88da', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name ShipmentStoragePricingsCREATE TABLE [dbo].[ShipmentStoragePricings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[ShipmentId] VARCHAR(15) NOT NULL,[WarehouseId] VARCHAR(15) NOT NULL,[StepFrom] INT NOT NULL,[StepTo] INT NULL,[Days] INT NULL,[SalePrice] DECIMAL(18, 3) NULL,[Amount] DECIMAL(18, 3) NULL,[LineNumber] INT NOT NULL,CONSTRAINT [PK_ShipmentStoragePricings] PRIMARY KEY([Id]));');


-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('084782b5-db38-4fa4-96a0-256906320d2d', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f4ef270c-0008-4723-86f5-7c5a9474a521', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])'');');

-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('82952470-9885-4ae0-bb10-0bd4a3421f30', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('30274bb6-3b42-4d1e-9f7c-d53be6f84b80', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ec39ac73-a77a-40f1-8c93-20c769687ae0', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('db113767-8c22-4f32-93c8-5eb6c83d2d0c', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('713be075-7b61-4f4f-82c0-9fab6a3df994', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fff7eecc-c552-415b-9ec3-49ab77f002c3', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1e8ca9c2-48cc-4b9a-b1e1-9aafd2f3e65b', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d09ba8ae-4517-465a-abee-2569338c474c', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a95e2769-2b2f-48f9-893f-acc6815115bf', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('209f35cf-ea23-40b3-8e71-227af567b581', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c75cd79c-f9cb-4a17-b520-b95a19b7b627', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c1872602-a105-492e-b820-054479ed250a', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])'');');


-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('54e13ecf-73be-4516-bfad-7ce54df158ac', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On WarehouseStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3977d26c-616d-40ac-98c7-ee4bb25663f3', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Index', GETDATE(), '-- Create Index On WarehouseStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])'');');


-- Add Foreign Key Constraint For Column PaymentId In Table ARPaymentChequeReplicas As Reference To Column Id In Table ARPayments
EXEC('ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_ARPayments_PaymentId] FOREIGN KEY([PaymentId]) REFERENCES [dbo].[ARPayments]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0308edeb-ac64-4c18-ad99-31da3fe32091', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'PaymentId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column PaymentId In Table ARPaymentChequeReplicas As Reference To Column Id In Table ARPaymentsEXEC(''ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_ARPayments_PaymentId] FOREIGN KEY([PaymentId]) REFERENCES [dbo].[ARPayments]([Id])'');');

-- Create Index On ARPaymentChequeReplicas Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_PaymentId] ON [dbo].[ARPaymentChequeReplicas]([PaymentId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c3524b9d-edcb-4a34-aeda-cebaeb38f79f', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'PaymentId', 'Create Index', GETDATE(), '-- Create Index On ARPaymentChequeReplicas TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_PaymentId] ON [dbo].[ARPaymentChequeReplicas]([PaymentId])'');');

-- Add Foreign Key Constraint For Column CurrencyId In Table ARPaymentChequeReplicas As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('48c88b03-1403-4b48-8605-4a1ef3757a5a', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table ARPaymentChequeReplicas As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On ARPaymentChequeReplicas Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_CurrencyId] ON [dbo].[ARPaymentChequeReplicas]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aec629f0-c66b-40d4-b2fb-a96a86d91d8f', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On ARPaymentChequeReplicas TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_CurrencyId] ON [dbo].[ARPaymentChequeReplicas]([CurrencyId])'');');

-- Add Foreign Key Constraint For Column StatusCode In Table ARPaymentChequeReplicas As Reference To Column Code In Table ARPaymentChequeStatuses
EXEC('ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_ARPaymentChequeStatuses_StatusCode] FOREIGN KEY([StatusCode]) REFERENCES [dbo].[ARPaymentChequeStatuses]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('36994103-fba6-4639-b1b2-206f93a666cd', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'StatusCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column StatusCode In Table ARPaymentChequeReplicas As Reference To Column Code In Table ARPaymentChequeStatusesEXEC(''ALTER TABLE [dbo].[ARPaymentChequeReplicas] ADD CONSTRAINT [FK_ARPaymentChequeReplicas_ARPaymentChequeStatuses_StatusCode] FOREIGN KEY([StatusCode]) REFERENCES [dbo].[ARPaymentChequeStatuses]([Code])'');');

-- Create Index On ARPaymentChequeReplicas Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_StatusCode] ON [dbo].[ARPaymentChequeReplicas]([StatusCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f9354347-c2b0-4173-a7d4-6e4e008e54b6', 'ARPaymentChequeReplica.dxml', 'ARPaymentChequeReplicas', 'StatusCode', 'Create Index', GETDATE(), '-- Create Index On ARPaymentChequeReplicas TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ARPaymentChequeReplicas_StatusCode] ON [dbo].[ARPaymentChequeReplicas]([StatusCode])'');');


-- Add Foreign Key Constraint For Column ChargeStorageCurrencyId In Table Shipments As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_Currencies_ChargeStorageCurrencyId] FOREIGN KEY([ChargeStorageCurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3a3654c6-5e69-4f62-961d-04cd776e26e1', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ChargeStorageCurrencyId In Table Shipments As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_Currencies_ChargeStorageCurrencyId] FOREIGN KEY([ChargeStorageCurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_ChargeStorageCurrencyId] ON [dbo].[Shipments]([ChargeStorageCurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e0801421-aea3-49cb-bf4f-8549b32dfaf0', 'Shipment.dxml', 'Shipments', 'ChargeStorageCurrencyId', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_ChargeStorageCurrencyId] ON [dbo].[Shipments]([ChargeStorageCurrencyId])'');');

-- Add Foreign Key Constraint For Column WeightMeasurementCode In Table Shipments As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightMeasurements_WeightMeasurementCode] FOREIGN KEY([WeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('25e60fe9-1bea-4ba4-a56e-a09a2839bce2', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WeightMeasurementCode In Table Shipments As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightMeasurements_WeightMeasurementCode] FOREIGN KEY([WeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_WeightMeasurementCode] ON [dbo].[Shipments]([WeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('92d8af85-11b1-4166-a6d1-21f42ae0ce3e', 'Shipment.dxml', 'Shipments', 'WeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_WeightMeasurementCode] ON [dbo].[Shipments]([WeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column WeightRoundingCode In Table Shipments As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightRoundings_WeightRoundingCode] FOREIGN KEY([WeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8bd8d132-d576-4886-a177-882350c598a8', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WeightRoundingCode In Table Shipments As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_WarehouseWeightRoundings_WeightRoundingCode] FOREIGN KEY([WeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_WeightRoundingCode] ON [dbo].[Shipments]([WeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2e0026f8-3b0f-42b2-b2ff-c2b5796f3368', 'Shipment.dxml', 'Shipments', 'WeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_WeightRoundingCode] ON [dbo].[Shipments]([WeightRoundingCode])'');');


-- Add Foreign Key Constraint For Column ShipmentId In Table ShipmentStoragePricings As Reference To Column Id In Table Shipments
EXEC('ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Shipments_ShipmentId] FOREIGN KEY([ShipmentId]) REFERENCES [dbo].[Shipments]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5a0bd81b-94ce-4859-adbb-93ce10192da7', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'ShipmentId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentId In Table ShipmentStoragePricings As Reference To Column Id In Table ShipmentsEXEC(''ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Shipments_ShipmentId] FOREIGN KEY([ShipmentId]) REFERENCES [dbo].[Shipments]([Id])'');');

-- Create Index On ShipmentStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_ShipmentId] ON [dbo].[ShipmentStoragePricings]([ShipmentId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e433bab1-a101-4adc-8069-b0a625d8d445', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'ShipmentId', 'Create Index', GETDATE(), '-- Create Index On ShipmentStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_ShipmentId] ON [dbo].[ShipmentStoragePricings]([ShipmentId])'');');

-- Add Foreign Key Constraint For Column WarehouseId In Table ShipmentStoragePricings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b5a1b30b-3067-4ac3-bcc9-490bf4cf743e', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'WarehouseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WarehouseId In Table ShipmentStoragePricings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[ShipmentStoragePricings] ADD CONSTRAINT [FK_ShipmentStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On ShipmentStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_WarehouseId] ON [dbo].[ShipmentStoragePricings]([WarehouseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a465fec9-40ae-4d86-829a-0d9dfffde20c', 'ShipmentStoragePricing.dxml', 'ShipmentStoragePricings', 'WarehouseId', 'Create Index', GETDATE(), '-- Create Index On ShipmentStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentStoragePricings_WarehouseId] ON [dbo].[ShipmentStoragePricings]([WarehouseId])'');');


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

-- General Script From 202008110940_SetIsBondedWarehouseField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
UPDATE Shipments SET
IsBondedWarehouse = 1
WHERE WarehouseLegWarehouseId IN (SELECT Id FROM Warehouses WHERE TypeCode='BO')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008110940_SetIsBondedWarehouseField.sxml', GETDATE(), 'UPDATE Shipments SET
IsBondedWarehouse = 1
WHERE WarehouseLegWarehouseId IN (SELECT Id FROM Warehouses WHERE TypeCode=''BO'')', DATEDIFF(MS,@StartTime,@EndTime), '16bad89d4b3a5dad0652ed8625f3b117', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

