-- Add New Column With Name ChargeStorage
ALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4e7e3d0f-a615-48d4-b5e1-7f0d688020f1', 'Warehouse.dxml', 'Warehouses', 'ChargeStorage', 'Add Column', GETDATE(), '-- Add New Column With Name ChargeStorageALTER TABLE [dbo].[Warehouses] ADD [ChargeStorage] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name CurrencyId
ALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('24d21153-a080-4daa-8639-29f84570d85b', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Add Column', GETDATE(), '-- Add New Column With Name CurrencyIdALTER TABLE [dbo].[Warehouses] ADD [CurrencyId] VARCHAR(15) NULL;');

-- Add New Column With Name AirWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4cb8ac03-e006-473f-9380-9429ec468734', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9403c3de-45a4-423a-aa28-95182d2025b9', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightMeasurementCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dfe6bf5c-778a-44fb-9072-20865b1d6a69', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightMeasurementCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightMeasurementCode] VARCHAR(4) NULL;');

-- Add New Column With Name AirWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3f61d1c8-8c10-4780-96f1-dd16e1c9b1dc', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name AirWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [AirWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name OceanWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b107520e-2b97-4877-8ba4-7a9c40784543', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name OceanWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [OceanWeightRoundingCode] VARCHAR(4) NULL;');

-- Add New Column With Name InlandWeightRoundingCode
ALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1e8c9bfe-2f76-4c04-915c-1c97c53df738', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Add Column', GETDATE(), '-- Add New Column With Name InlandWeightRoundingCodeALTER TABLE [dbo].[Warehouses] ADD [InlandWeightRoundingCode] VARCHAR(4) NULL;');


-- Create New Table With Name WarehouseStoragePricings
CREATE TABLE [dbo].[WarehouseStoragePricings](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[WarehouseId] VARCHAR(15) NULL,
[StepFrom] INT NULL,
[StepTo] INT NULL,
[Days] INT NULL,
[SalePrice] DECIMAL(18, 3) NULL,
CONSTRAINT [PK_WarehouseStoragePricings] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5fcc0cf9-3b0c-4216-8312-d621b58e8b6d', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseStoragePricingsCREATE TABLE [dbo].[WarehouseStoragePricings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[WarehouseId] VARCHAR(15) NULL,[StepFrom] INT NULL,[StepTo] INT NULL,[Days] INT NULL,[SalePrice] DECIMAL(18, 3) NULL,CONSTRAINT [PK_WarehouseStoragePricings] PRIMARY KEY([Id]));');


-- Create New Table With Name WarehouseWeightMeasurements
CREATE TABLE [dbo].[WarehouseWeightMeasurements](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('88415528-804d-4f62-833a-e7e7ed24f7fb', 'WarehouseWeightMeasurement.dxml', 'WarehouseWeightMeasurements', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightMeasurementsCREATE TABLE [dbo].[WarehouseWeightMeasurements]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,CONSTRAINT [PK_WarehouseWeightMeasurements] PRIMARY KEY([Code]));');


-- Create New Table With Name WarehouseWeightRoundings
CREATE TABLE [dbo].[WarehouseWeightRoundings](
[Code] VARCHAR(4) NOT NULL,
[Name] VARCHAR(60) NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Display] VARCHAR(20) NULL,
CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('22b88200-181c-43a2-ac69-cf2e5c97f641', 'WarehouseWeightRounding.dxml', 'WarehouseWeightRoundings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name WarehouseWeightRoundingsCREATE TABLE [dbo].[WarehouseWeightRoundings]([Code] VARCHAR(4) NOT NULL,[Name] VARCHAR(60) NULL,[SearchFields] NVARCHAR(MAX) NULL,[Display] VARCHAR(20) NULL,CONSTRAINT [PK_WarehouseWeightRoundings] PRIMARY KEY([Code]));');


-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('474bd1c6-55aa-436b-a78b-199ca4a8e42d', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table Warehouses As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dc729015-1998-4f6a-9c04-97e55bdd3ded', 'Warehouse.dxml', 'Warehouses', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_CurrencyId] ON [dbo].[Warehouses]([CurrencyId])'');');

-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97ffd45d-68ea-419c-bae3-22897e2361b4', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_AirWeightMeasurementCode] FOREIGN KEY([AirWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c7d4481c-54ef-4ecd-8033-e78758158652', 'Warehouse.dxml', 'Warehouses', 'AirWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightMeasurementCode] ON [dbo].[Warehouses]([AirWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8a74c56e-01f1-424a-9688-f41a17fc9c88', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_OceanWeightMeasurementCode] FOREIGN KEY([OceanWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f9b8a500-aa3a-44bd-9a82-3add35e6dbba', 'Warehouse.dxml', 'Warehouses', 'OceanWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightMeasurementCode] ON [dbo].[Warehouses]([OceanWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurements
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0a27eb9a-d063-4f6f-8763-70444dc6293b', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightMeasurementCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightMeasurementsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightMeasurements_InlandWeightMeasurementCode] FOREIGN KEY([InlandWeightMeasurementCode]) REFERENCES [dbo].[WarehouseWeightMeasurements]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e0b3dc48-7ac1-4eaa-a67c-1b5b4d2024ba', 'Warehouse.dxml', 'Warehouses', 'InlandWeightMeasurementCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightMeasurementCode] ON [dbo].[Warehouses]([InlandWeightMeasurementCode])'');');

-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('47470253-6d70-44bc-aa36-4fa4304e57b2', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column AirWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_AirWeightRoundingCode] FOREIGN KEY([AirWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('86310cf7-8690-476c-8edd-507adff6dc8a', 'Warehouse.dxml', 'Warehouses', 'AirWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_AirWeightRoundingCode] ON [dbo].[Warehouses]([AirWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f8c585a8-c1e8-44c6-8027-1b37427a34c6', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column OceanWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_OceanWeightRoundingCode] FOREIGN KEY([OceanWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('618296ee-a997-42ac-b2ee-5270ef15aaf4', 'Warehouse.dxml', 'Warehouses', 'OceanWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_OceanWeightRoundingCode] ON [dbo].[Warehouses]([OceanWeightRoundingCode])'');');

-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundings
EXEC('ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d1891cc5-0972-42b1-829f-db28cffaed9b', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InlandWeightRoundingCode In Table Warehouses As Reference To Column Code In Table WarehouseWeightRoundingsEXEC(''ALTER TABLE [dbo].[Warehouses] ADD CONSTRAINT [FK_Warehouses_WarehouseWeightRoundings_InlandWeightRoundingCode] FOREIGN KEY([InlandWeightRoundingCode]) REFERENCES [dbo].[WarehouseWeightRoundings]([Code])'');');

-- Create Index On Warehouses Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8bf18dc3-e216-499d-a99b-b617249b8983', 'Warehouse.dxml', 'Warehouses', 'InlandWeightRoundingCode', 'Create Index', GETDATE(), '-- Create Index On Warehouses TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Warehouses_InlandWeightRoundingCode] ON [dbo].[Warehouses]([InlandWeightRoundingCode])'');');


-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1eee4650-5dbf-4bc3-8258-513fe77523fb', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column WarehouseId In Table WarehouseStoragePricings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[WarehouseStoragePricings] ADD CONSTRAINT [FK_WarehouseStoragePricings_Cards_WarehouseId] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On WarehouseStoragePricings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f7b3748d-533f-4504-be26-2dde8a5c29f1', 'WarehouseStoragePricing.dxml', 'WarehouseStoragePricings', 'WarehouseId', 'Create Index', GETDATE(), '-- Create Index On WarehouseStoragePricings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_WarehouseStoragePricings_WarehouseId] ON [dbo].[WarehouseStoragePricings]([WarehouseId])'');');


