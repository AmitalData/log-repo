-- General Script From 202006151350_DeleteOldPaymentMethodsMetadata.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod'))
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod'))
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = 'APPaymentMethod')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = 'ARPaymentMethod')
delete from ObjectTables where Name in ('APPaymentMethod', 'ARPaymentMethod')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151350_DeleteOldPaymentMethodsMetadata.sxml', GETDATE(), 'delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Queries where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from EntityLastActivities where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Screens where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from Features where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectTableTabs where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from MenusTables where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod''))
delete from TraceEvents where EventTypeId in (select Id from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod''))
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from EventTypes where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = ''APPaymentMethod'')
delete from ObjectTableLastUpdates where ObjectTableId in (select Id from ObjectTables where Name = ''ARPaymentMethod'')
delete from ObjectTables where Name in (''APPaymentMethod'', ''ARPaymentMethod'')', DATEDIFF(MS,@StartTime,@EndTime), '1692d6d174ab08bd4e6effcbbd1ccc7d', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Add New Column With Name AccountingVATSplit
ALTER TABLE [dbo].[Cards] ADD [AccountingVATSplit] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3bb6166c-dad5-495a-a979-d9a26922fd4f', 'Card.dxml', 'Cards', 'AccountingVATSplit', 'Add Column', GETDATE(), '-- Add New Column With Name AccountingVATSplitALTER TABLE [dbo].[Cards] ADD [AccountingVATSplit] BIT DEFAULT(0) NOT NULL;');


-- Create New Table With Name CardCurrenciesAccountings
CREATE TABLE [dbo].[CardCurrenciesAccountings](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CardId] VARCHAR(15) NULL,
[CurrencyId] VARCHAR(15) NULL,
[PayableDebitAccount] VARCHAR(15) NULL,
[ReceivableCreditAccount] VARCHAR(15) NULL,
CONSTRAINT [PK_CardCurrenciesAccountings] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('76da7d85-1a1b-4dc8-b2e0-b866620ec7d9', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CardCurrenciesAccountingsCREATE TABLE [dbo].[CardCurrenciesAccountings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CardId] VARCHAR(15) NULL,[CurrencyId] VARCHAR(15) NULL,[PayableDebitAccount] VARCHAR(15) NULL,[ReceivableCreditAccount] VARCHAR(15) NULL,CONSTRAINT [PK_CardCurrenciesAccountings] PRIMARY KEY([Id]));');


-- Add New Column With Name ShipmentSubTypeId
ALTER TABLE [dbo].[Quotes] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a3fdb9fe-41c3-4f49-8412-83bd6249de82', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentSubTypeIdALTER TABLE [dbo].[Quotes] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;');


-- Add New Column With Name ShipmentSubTypeId
ALTER TABLE [dbo].[Shipments] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0d724155-b9eb-4516-abea-f29a067498c7', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentSubTypeIdALTER TABLE [dbo].[Shipments] ADD [ShipmentSubTypeId] VARCHAR(15) NULL;');


-- Add New Column With Name DeliveryTruckerId
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTruckerId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a997de8c-5797-4aca-98c3-2c0e3f3a7139', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryTruckerId', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryTruckerIdALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTruckerId] VARCHAR(15) NULL;');

-- Add New Column With Name DeliveryTruckerNumber
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTruckerNumber] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a1ed136e-6e77-4687-b343-a666b8f0bc55', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryTruckerNumber', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryTruckerNumberALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTruckerNumber] VARCHAR(15) NULL;');

-- Add New Column With Name DeliveryDriver
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryDriver] VARCHAR(40) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aaa22d0b-1ea3-4812-9c93-94bc1bcadec4', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryDriver', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryDriverALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryDriver] VARCHAR(40) NULL;');

-- Add New Column With Name DeliveryTrailerNumber
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTrailerNumber] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c19e6590-5a79-4dd4-ac31-18760422e3ae', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryTrailerNumber', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryTrailerNumberALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryTrailerNumber] VARCHAR(15) NULL;');

-- Add New Column With Name DeliveryNotes
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNotes] NVARCHAR(2000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e9f3ae75-62ec-40c4-9ce1-6b7dc52a080b', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryNotes', 'Add Column', GETDATE(), '-- Add New Column With Name DeliveryNotesALTER TABLE [dbo].[ShipmentComputedFields] ADD [DeliveryNotes] NVARCHAR(2000) NULL;');

-- Add New Column With Name PickupTruckerId
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTruckerId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fd5979f1-cebd-4bd3-9e7b-7eb189bdb0d0', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupTruckerId', 'Add Column', GETDATE(), '-- Add New Column With Name PickupTruckerIdALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTruckerId] VARCHAR(15) NULL;');

-- Add New Column With Name PickupTruckerNumber
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTruckerNumber] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('db29f95b-0974-4d22-8e2e-3ada498bdcab', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupTruckerNumber', 'Add Column', GETDATE(), '-- Add New Column With Name PickupTruckerNumberALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTruckerNumber] VARCHAR(15) NULL;');

-- Add New Column With Name PickupDriver
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupDriver] VARCHAR(40) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('892b71b5-5e08-40bb-a5e8-92ef1581b070', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupDriver', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDriverALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupDriver] VARCHAR(40) NULL;');

-- Add New Column With Name PickupTrailerNumber
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTrailerNumber] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f007bcdb-8ca5-4a8f-a717-9bf3415e9c04', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupTrailerNumber', 'Add Column', GETDATE(), '-- Add New Column With Name PickupTrailerNumberALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupTrailerNumber] VARCHAR(15) NULL;');

-- Add New Column With Name PickupNotes
ALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupNotes] NVARCHAR(2000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('56e655bf-66ff-4b59-8746-55404b5b5370', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupNotes', 'Add Column', GETDATE(), '-- Add New Column With Name PickupNotesALTER TABLE [dbo].[ShipmentComputedFields] ADD [PickupNotes] NVARCHAR(2000) NULL;');


-- Create New Table With Name ShipmentSubTypes
CREATE TABLE [dbo].[ShipmentSubTypes](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CreateDate] DATETIME NOT NULL,
[CreatedByUserId] VARCHAR(15) NOT NULL,
[UpdateDate] DATETIME NOT NULL,
[UpdatedByUserId] VARCHAR(15) NOT NULL,
[SearchFields] NVARCHAR(MAX) NULL,
[Code] VARCHAR(5) NOT NULL,
[Name] VARCHAR(60) NOT NULL,
[Inactive] BIT DEFAULT(0) NOT NULL,
[ShipmentTypeCode] VARCHAR(4) NOT NULL,
CONSTRAINT [PK_ShipmentSubTypes] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('666ebe55-e1cb-4280-bd76-cbf0d9d82f24', 'ShipmentSubType.dxml', 'ShipmentSubTypes', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name ShipmentSubTypesCREATE TABLE [dbo].[ShipmentSubTypes]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CreateDate] DATETIME NOT NULL,[CreatedByUserId] VARCHAR(15) NOT NULL,[UpdateDate] DATETIME NOT NULL,[UpdatedByUserId] VARCHAR(15) NOT NULL,[SearchFields] NVARCHAR(MAX) NULL,[Code] VARCHAR(5) NOT NULL,[Name] VARCHAR(60) NOT NULL,[Inactive] BIT DEFAULT(0) NOT NULL,[ShipmentTypeCode] VARCHAR(4) NOT NULL,CONSTRAINT [PK_ShipmentSubTypes] PRIMARY KEY([Id]));');


-- Add Foreign Key Constraint For Column CardId In Table CardCurrenciesAccountings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7e41c74c-b4cb-4341-b6ce-ffcf26cc2a3f', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CardId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CardId In Table CardCurrenciesAccountings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On CardCurrenciesAccountings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CardId] ON [dbo].[CardCurrenciesAccountings]([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aa49181c-b033-4d6f-9fd2-7e849afec695', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CardId', 'Create Index', GETDATE(), '-- Create Index On CardCurrenciesAccountings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CardId] ON [dbo].[CardCurrenciesAccountings]([CardId])'');');

-- Add Foreign Key Constraint For Column CurrencyId In Table CardCurrenciesAccountings As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0943e90b-df0d-418b-bdb7-acf64dbfe65a', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table CardCurrenciesAccountings As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On CardCurrenciesAccountings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CurrencyId] ON [dbo].[CardCurrenciesAccountings]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2da124d2-797f-43f9-b388-056773f4a414', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On CardCurrenciesAccountings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CurrencyId] ON [dbo].[CardCurrenciesAccountings]([CurrencyId])'');');


-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes As Reference To Column Id In Table ShipmentSubTypes
EXEC('ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('80bd28e6-fc62-48ef-b293-681baacc2997', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Quotes As Reference To Column Id In Table ShipmentSubTypesEXEC(''ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])'');');

-- Create Index On Quotes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes]([ShipmentSubTypeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('46694df0-79b9-4b56-89e0-41afb8d3d1de', 'Quote.dxml', 'Quotes', 'ShipmentSubTypeId', 'Create Index', GETDATE(), '-- Create Index On Quotes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Quotes_ShipmentSubTypeId] ON [dbo].[Quotes]([ShipmentSubTypeId])'');');


-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments As Reference To Column Id In Table ShipmentSubTypes
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ab5b01c5-e963-45a8-a41c-df910c9135af', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentSubTypeId In Table Shipments As Reference To Column Id In Table ShipmentSubTypesEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_Shipments_ShipmentSubTypes_ShipmentSubTypeId] FOREIGN KEY([ShipmentSubTypeId]) REFERENCES [dbo].[ShipmentSubTypes]([Id])'');');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments]([ShipmentSubTypeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b006c4ed-3388-41bf-9eb3-081cc88ab693', 'Shipment.dxml', 'Shipments', 'ShipmentSubTypeId', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_ShipmentSubTypeId] ON [dbo].[Shipments]([ShipmentSubTypeId])'');');


-- Add Foreign Key Constraint For Column DeliveryTruckerId In Table ShipmentComputedFields As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[ShipmentComputedFields] ADD CONSTRAINT [FK_ShipmentComputedFields_Cards_DeliveryTruckerId] FOREIGN KEY([DeliveryTruckerId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('221ac304-2e2b-461a-85ca-9721d75139aa', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryTruckerId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column DeliveryTruckerId In Table ShipmentComputedFields As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[ShipmentComputedFields] ADD CONSTRAINT [FK_ShipmentComputedFields_Cards_DeliveryTruckerId] FOREIGN KEY([DeliveryTruckerId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On ShipmentComputedFields Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_DeliveryTruckerId] ON [dbo].[ShipmentComputedFields]([DeliveryTruckerId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c96c362d-0bc2-475b-986b-c5a49585b128', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryTruckerId', 'Create Index', GETDATE(), '-- Create Index On ShipmentComputedFields TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_DeliveryTruckerId] ON [dbo].[ShipmentComputedFields]([DeliveryTruckerId])'');');

-- Add Foreign Key Constraint For Column PickupTruckerId In Table ShipmentComputedFields As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[ShipmentComputedFields] ADD CONSTRAINT [FK_ShipmentComputedFields_Cards_PickupTruckerId] FOREIGN KEY([PickupTruckerId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b38cac8f-07b1-4217-a61d-ebbf21a1d0cf', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupTruckerId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column PickupTruckerId In Table ShipmentComputedFields As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[ShipmentComputedFields] ADD CONSTRAINT [FK_ShipmentComputedFields_Cards_PickupTruckerId] FOREIGN KEY([PickupTruckerId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On ShipmentComputedFields Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_PickupTruckerId] ON [dbo].[ShipmentComputedFields]([PickupTruckerId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bd55536d-40f4-4826-b7b3-398b61c7f301', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PickupTruckerId', 'Create Index', GETDATE(), '-- Create Index On ShipmentComputedFields TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_PickupTruckerId] ON [dbo].[ShipmentComputedFields]([PickupTruckerId])'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('861a5b3e-5617-444f-b55f-28fa910a8379', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_CreatedByUserId] ON [dbo].[ShipmentSubTypes]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0ed99deb-7aea-4667-a3cf-55af07668c1d', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_CreatedByUserId] ON [dbo].[ShipmentSubTypes]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0120d467-3b35-4b4c-8cb0-79d5dad98437', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table ShipmentSubTypes As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_UpdatedByUserId] ON [dbo].[ShipmentSubTypes]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8ef42322-7346-4d00-9c16-20c9efcdac29', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_UpdatedByUserId] ON [dbo].[ShipmentSubTypes]([UpdatedByUserId])'');');

-- Add Foreign Key Constraint For Column ShipmentTypeCode In Table ShipmentSubTypes As Reference To Column Id In Table ShipmentTypes
EXEC('ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_ShipmentTypes_ShipmentTypeCode] FOREIGN KEY([ShipmentTypeCode]) REFERENCES [dbo].[ShipmentTypes]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a34c26a3-4015-41d8-98e5-c205e305e2ba', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'ShipmentTypeCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ShipmentTypeCode In Table ShipmentSubTypes As Reference To Column Id In Table ShipmentTypesEXEC(''ALTER TABLE [dbo].[ShipmentSubTypes] ADD CONSTRAINT [FK_ShipmentSubTypes_ShipmentTypes_ShipmentTypeCode] FOREIGN KEY([ShipmentTypeCode]) REFERENCES [dbo].[ShipmentTypes]([Id])'');');

-- Create Index On ShipmentSubTypes Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_ShipmentTypeCode] ON [dbo].[ShipmentSubTypes]([ShipmentTypeCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f9b10b99-0698-42c8-a741-e43160552e9e', 'ShipmentSubType.dxml', 'ShipmentSubTypes', 'ShipmentTypeCode', 'Create Index', GETDATE(), '-- Create Index On ShipmentSubTypes TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentSubTypes_ShipmentTypeCode] ON [dbo].[ShipmentSubTypes]([ShipmentTypeCode])'');');


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


-- General Script From 202006151400_AddAirShipmentType_V2.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Id from ShipmentTypes where Id = 'Air')
begin
insert into ShipmentTypes (Id, Name, SearchFields, TransportModeId, AutomaticLastUpdateDate)
values ('Air', 'Air', 'Air,Air', 'A', GETDATE())
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151400_AddAirShipmentType_V2.sxml', GETDATE(), 'if not exists (select Id from ShipmentTypes where Id = ''Air'')
begin
insert into ShipmentTypes (Id, Name, SearchFields, TransportModeId, AutomaticLastUpdateDate)
values (''Air'', ''Air'', ''Air,Air'', ''A'', GETDATE())
end', DATEDIFF(MS,@StartTime,@EndTime), 'b359fbfc8a145c6c7d90def0a951767e', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006151500_FillAirQuoteShipmentTypeField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE QuotesCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Quotes
Where ShipmentTypeId is null
OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if(@TransportModeId = 'A')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'Air' and Tenant = @Tenant)
end
update Quotes set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
update Quotes set ShipmentTypeId = 'Air' where Id = @EntityId and Tenant = @Tenant and TransportModeId = 'A'
FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151500_FillAirQuoteShipmentTypeField.sxml', GETDATE(), 'declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE QuotesCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Quotes
Where ShipmentTypeId is null
OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if(@TransportModeId = ''A'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''Air'' and Tenant = @Tenant)
end
update Quotes set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
update Quotes set ShipmentTypeId = ''Air'' where Id = @EntityId and Tenant = @Tenant and TransportModeId = ''A''
FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '2891ede05e6d408dcd8a293fea76b007', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006151503_FillQuoteShipmentSubTypeBackward.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE QuotesCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Quotes
OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @ShipmentSubTypeId = null
if(@TransportModeId = 'A')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'Air' and Tenant = @Tenant)
end
else if(@TransportModeId = 'O')
begin
if(@ShipmentTypeId = 'FCLD')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'FCL' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = 'LCLD')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LCL' and Tenant = @Tenant)
end
end
else if(@TransportModeId = 'I')
begin
if(@ShipmentTypeId = 'FTL')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'FTL' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = 'LTL')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LTL' and Tenant = @Tenant)
end
end
update Quotes set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151503_FillQuoteShipmentSubTypeBackward.sxml', GETDATE(), 'declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE QuotesCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Quotes
OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @ShipmentSubTypeId = null
if(@TransportModeId = ''A'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''Air'' and Tenant = @Tenant)
end
else if(@TransportModeId = ''O'')
begin
if(@ShipmentTypeId = ''FCLD'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = ''LCLD'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = @Tenant)
end
end
else if(@TransportModeId = ''I'')
begin
if(@ShipmentTypeId = ''FTL'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = ''LTL'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = @Tenant)
end
end
update Quotes set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor
END', DATEDIFF(MS,@StartTime,@EndTime), 'a70d9d0c39d6fe3b640d648754f71a2a', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006111511_AddAirShipmentType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Id from ShipmentTypes where Id = 'Air')
begin
insert into ShipmentTypes (Id, Name, SearchFields, TransportModeId, AutomaticLastUpdateDate)
values ('Air', 'Air', 'Air,Air', 'A', GETDATE())
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006111511_AddAirShipmentType.sxml', GETDATE(), 'if not exists (select Id from ShipmentTypes where Id = ''Air'')
begin
insert into ShipmentTypes (Id, Name, SearchFields, TransportModeId, AutomaticLastUpdateDate)
values (''Air'', ''Air'', ''Air,Air'', ''A'', GETDATE())
end', DATEDIFF(MS,@StartTime,@EndTime), 'b359fbfc8a145c6c7d90def0a951767e', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006111515_AddShipmentSubTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
declare @ShipmentTypeCode as varchar(4)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = 'system@tenant'+ @TenantString + '.com'
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'Air')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'Air')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'Air', 'Air', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'Air,Air')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'FCL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'FCLD')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'FCL', 'FCL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'FCL,FCL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'LCL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'LCLD')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'LCL', 'LCL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'LCL,LCL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'FTL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'FTL')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'FTL', 'FTL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'FTL,FTL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'LTL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'LTL')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'LTL', 'LTL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'LTL,LTL')
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006111515_AddShipmentSubTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
declare @ShipmentTypeCode as varchar(4)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = ''system@tenant''+ @TenantString + ''.com''
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''Air'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''Air'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''Air'', ''Air'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''Air,Air'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''FCL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''FCLD'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''FCL'', ''FCL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''FCL,FCL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''LCL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''LCLD'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''LCL'', ''LCL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''LCL,LCL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''FTL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''FTL'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''FTL'', ''FTL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''FTL,FTL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''LTL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''LTL'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''LTL'', ''LTL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''LTL,LTL'')
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '0f56c05ce3cc89966a1d2fd6a5bcc0a0', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006140958_FillAirShipmentShipmentTypeField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Shipments
Where DirectionId != 'C' and ShipmentTypeId is null
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if(@TransportModeId = 'A')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'Air' and Tenant = @Tenant)
end
else if(@TransportModeId = 'O')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LCL' and Tenant = @Tenant)
end
if(@TransportModeId = 'I')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LTL' and Tenant = @Tenant)
end
update Shipments set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
update Shipments set ShipmentTypeId = 'Air' where Id = @EntityId and Tenant = @Tenant and TransportModeId = 'A'
FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006140958_FillAirShipmentShipmentTypeField.sxml', GETDATE(), 'declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Shipments
Where DirectionId != ''C'' and ShipmentTypeId is null
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if(@TransportModeId = ''A'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''Air'' and Tenant = @Tenant)
end
else if(@TransportModeId = ''O'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = @Tenant)
end
if(@TransportModeId = ''I'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = @Tenant)
end
update Shipments set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
update Shipments set ShipmentTypeId = ''Air'' where Id = @EntityId and Tenant = @Tenant and TransportModeId = ''A''
FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '0c0bd2941f5a12f814933c9cdce3331d', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006141227_FillShipmentSubTypeBackward.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Shipments
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @ShipmentSubTypeId = null
if(@TransportModeId = 'A')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'Air' and Tenant = @Tenant)
end
else if(@TransportModeId = 'O')
begin
if(@ShipmentTypeId = 'FCLD')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'FCL' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = 'LCLD')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LCL' and Tenant = @Tenant)
end
end
else if(@TransportModeId = 'I')
begin
if(@ShipmentTypeId = 'FTL')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'FTL' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = 'LTL')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = 'LTL' and Tenant = @Tenant)
end
end
update Shipments set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006141227_FillShipmentSubTypeBackward.sxml', GETDATE(), 'declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
BEGIN
DECLARE ShipmentsCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId
FROM Shipments
OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @ShipmentSubTypeId = null
if(@TransportModeId = ''A'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''Air'' and Tenant = @Tenant)
end
else if(@TransportModeId = ''O'')
begin
if(@ShipmentTypeId = ''FCLD'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = ''LCLD'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = @Tenant)
end
end
else if(@TransportModeId = ''I'')
begin
if(@ShipmentTypeId = ''FTL'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = @Tenant)
end
else if(@ShipmentTypeId = ''LTL'')
begin
set @ShipmentSubTypeId = (select Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = @Tenant)
end
end
update Shipments set ShipmentSubTypeId = @ShipmentSubTypeId where Id = @EntityId and Tenant = @Tenant
FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId
END
CLOSE ShipmentsCursor
DEALLOCATE ShipmentsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '908df2fd92b651891a15157002cee3b8', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

