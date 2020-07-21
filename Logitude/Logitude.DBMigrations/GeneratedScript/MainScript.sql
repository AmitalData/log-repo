-- Create New Table With Name QuoteClosingReasons
CREATE TABLE [dbo].[QuoteClosingReasons](
[Code] VARCHAR(2) NOT NULL,
[Name] VARCHAR(60) NOT NULL,
[SearchFields] NVARCHAR(1000) NULL,
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[CreateDate] DATETIME NOT NULL,
[UpdateDate] DATETIME NOT NULL,
[CreatedByUserId] VARCHAR(15) NOT NULL,
[UpdatedByUserId] VARCHAR(15) NOT NULL,
[Inactive] BIT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_QuoteClosingReasons] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cc1d916f-ccab-4f64-9be8-60625ef367c9', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name QuoteClosingReasonsCREATE TABLE [dbo].[QuoteClosingReasons]([Code] VARCHAR(2) NOT NULL,[Name] VARCHAR(60) NOT NULL,[SearchFields] NVARCHAR(1000) NULL,[Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CreateDate] DATETIME NOT NULL,[UpdateDate] DATETIME NOT NULL,[CreatedByUserId] VARCHAR(15) NOT NULL,[UpdatedByUserId] VARCHAR(15) NOT NULL,[Inactive] BIT DEFAULT(0) NOT NULL,CONSTRAINT [PK_QuoteClosingReasons] PRIMARY KEY([Id]));');


-- Add New Column With Name InsidePackageId
ALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD [InsidePackageId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('479ea6a8-6c94-4703-80f4-07880076ee8f', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Add Column', GETDATE(), '-- Add New Column With Name InsidePackageIdALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD [InsidePackageId] VARCHAR(15) NULL;');


-- Add Default Value For Column IsEditedByUser
ALTER TABLE [dbo].[ShipmentPayables] ADD DEFAULT 0 FOR [IsEditedByUser];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('db56ab29-6123-4406-91c8-f03b3becace9', 'ShipmentPayable.dxml', 'ShipmentPayables', 'IsEditedByUser', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsEditedByUserALTER TABLE [dbo].[ShipmentPayables] ADD DEFAULT 0 FOR [IsEditedByUser];');

-- Add Default Value For Column IsFromQuote
ALTER TABLE [dbo].[ShipmentPayables] ADD DEFAULT 0 FOR [IsFromQuote];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('561c9acd-beab-4d92-a3f3-cadf4c869f6b', 'ShipmentPayable.dxml', 'ShipmentPayables', 'IsFromQuote', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsFromQuoteALTER TABLE [dbo].[ShipmentPayables] ADD DEFAULT 0 FOR [IsFromQuote];');

-- Drop Column IsAdhoc
EXEC SP_RENAME 'dbo.ShipmentPayables.IsAdhoc', 'Drop_IsAdhoc', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('65c68b23-3c60-4c72-b559-933f3a6b029b', 'ShipmentPayable.dxml', 'ShipmentPayables', 'IsAdhoc', 'Drop Column', GETDATE(), '-- Drop Column IsAdhocEXEC SP_RENAME ''dbo.ShipmentPayables.IsAdhoc'', ''Drop_IsAdhoc'', ''COLUMN'';');

-- Drop Column IATACodeCode
EXEC SP_RENAME 'dbo.ShipmentPayables.IATACodeCode', 'Drop_IATACodeCode', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e2874a12-0c28-48aa-b921-503e26240c86', 'ShipmentPayable.dxml', 'ShipmentPayables', 'IATACodeCode', 'Drop Column', GETDATE(), '-- Drop Column IATACodeCodeEXEC SP_RENAME ''dbo.ShipmentPayables.IATACodeCode'', ''Drop_IATACodeCode'', ''COLUMN'';');

-- Create Index On ShipmentPayables Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentPayables_AutomaticLastUpdateDate] ON [dbo].[ShipmentPayables]([AutomaticLastUpdateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d7ba1f5b-6e59-4efa-92c1-9a2fc4954da3', 'ShipmentPayable.dxml', 'ShipmentPayables', 'AutomaticLastUpdateDate', 'Create Index', GETDATE(), '-- Create Index On ShipmentPayables TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentPayables_AutomaticLastUpdateDate] ON [dbo].[ShipmentPayables]([AutomaticLastUpdateDate])'');');


-- Add New Column With Name AutomaticLastUpdateDate
ALTER TABLE [dbo].[ShipmentPayableStatus] ADD [AutomaticLastUpdateDate] DATETIME DEFAULT(GETDATE()) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c79d91a7-5936-44ce-844c-b5de45777676', 'ShipmentPayableStatus.dxml', 'ShipmentPayableStatus', 'AutomaticLastUpdateDate', 'Add Column', GETDATE(), '-- Add New Column With Name AutomaticLastUpdateDateALTER TABLE [dbo].[ShipmentPayableStatus] ADD [AutomaticLastUpdateDate] DATETIME DEFAULT(GETDATE()) NOT NULL;');

-- Create Index On ShipmentPayableStatus Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentPayableStatus_AutomaticLastUpdateDate] ON [dbo].[ShipmentPayableStatus]([AutomaticLastUpdateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('655ee131-f9f4-4bb2-8591-371ec1a9f5ff', 'ShipmentPayableStatus.dxml', 'ShipmentPayableStatus', 'AutomaticLastUpdateDate', 'Create Index', GETDATE(), '-- Create Index On ShipmentPayableStatus TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentPayableStatus_AutomaticLastUpdateDate] ON [dbo].[ShipmentPayableStatus]([AutomaticLastUpdateDate])'');');


-- Change Size From 20 To 25 For Column PickUpDeliveryNumber
ALTER TABLE [dbo].[ShipmentPickUpDeliveries] ALTER COLUMN [PickUpDeliveryNumber] VARCHAR(25) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('90c244a5-47c0-4e7d-976b-895fcffc0802', 'ShipmentPickUpDelivery.dxml', 'ShipmentPickUpDeliveries', 'PickUpDeliveryNumber', 'Alter Column Size', GETDATE(), '-- Change Size From 20 To 25 For Column PickUpDeliveryNumberALTER TABLE [dbo].[ShipmentPickUpDeliveries] ALTER COLUMN [PickUpDeliveryNumber] VARCHAR(25) NOT NULL;');

-- Add Default Value For Column FullResponsibility
ALTER TABLE [dbo].[ShipmentPickUpDeliveries] ADD DEFAULT 0 FOR [FullResponsibility];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ecd11149-7987-4ca5-964f-9e699c7d2c87', 'ShipmentPickUpDelivery.dxml', 'ShipmentPickUpDeliveries', 'FullResponsibility', 'Add Default Value', GETDATE(), '-- Add Default Value For Column FullResponsibilityALTER TABLE [dbo].[ShipmentPickUpDeliveries] ADD DEFAULT 0 FOR [FullResponsibility];');


-- Drop Column Seal
EXEC SP_RENAME 'dbo.ShipmentPickUpDeliveryPackages.Seal', 'Drop_Seal', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('15fdc80d-7dba-431e-90fb-d1043ae36892', 'ShipmentPickUpDeliveryPackage.dxml', 'ShipmentPickUpDeliveryPackages', 'Seal', 'Drop Column', GETDATE(), '-- Drop Column SealEXEC SP_RENAME ''dbo.ShipmentPickUpDeliveryPackages.Seal'', ''Drop_Seal'', ''COLUMN'';');


-- Change Precision And Scale From (18, 0) To (18, 2) For Column PayableLocal
ALTER TABLE [dbo].[ShipmentReceivables] ALTER COLUMN [PayableLocal] DECIMAL(18, 2) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('89272dde-5894-45e7-85eb-f6165fa9c4a1', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'PayableLocal', 'Alter Column Precision And Scale', GETDATE(), '-- Change Precision And Scale From (18, 0) To (18, 2) For Column PayableLocalALTER TABLE [dbo].[ShipmentReceivables] ALTER COLUMN [PayableLocal] DECIMAL(18, 2) NOT NULL;');

-- Add Default Value For Column IsFromQuote
ALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsFromQuote];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6716e742-6460-4f58-928f-c42de51b9ead', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'IsFromQuote', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsFromQuoteALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsFromQuote];');

-- Add Default Value For Column IsFixedPrice
ALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsFixedPrice];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9fadd407-20d0-43bf-a048-97827c4da191', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'IsFixedPrice', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsFixedPriceALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsFixedPrice];');

-- Add Default Value For Column IsExchangeRateFixed
ALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsExchangeRateFixed];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('77390b59-8b76-4987-bf73-d657203c0266', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'IsExchangeRateFixed', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsExchangeRateFixedALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [IsExchangeRateFixed];');

-- Add Default Value For Column AWBPrint
ALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [AWBPrint];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('de02d0f4-cef3-4d1c-bd1b-b29d91151ed6', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'AWBPrint', 'Add Default Value', GETDATE(), '-- Add Default Value For Column AWBPrintALTER TABLE [dbo].[ShipmentReceivables] ADD DEFAULT 0 FOR [AWBPrint];');

-- Drop Column IATACodeCode
EXEC SP_RENAME 'dbo.ShipmentReceivables.IATACodeCode', 'Drop_IATACodeCode', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('76a92da2-5dcb-4215-86c9-74a7ac5b522d', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'IATACodeCode', 'Drop Column', GETDATE(), '-- Drop Column IATACodeCodeEXEC SP_RENAME ''dbo.ShipmentReceivables.IATACodeCode'', ''Drop_IATACodeCode'', ''COLUMN'';');

-- Create Index On ShipmentReceivables Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentReceivables_AutomaticLastUpdateDate] ON [dbo].[ShipmentReceivables]([AutomaticLastUpdateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1cda875e-2311-4120-b99d-be9fee52cd04', 'ShipmentReceivable.dxml', 'ShipmentReceivables', 'AutomaticLastUpdateDate', 'Create Index', GETDATE(), '-- Create Index On ShipmentReceivables TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentReceivables_AutomaticLastUpdateDate] ON [dbo].[ShipmentReceivables]([AutomaticLastUpdateDate])'');');


-- Add New Column With Name AutomaticLastUpdateDate
ALTER TABLE [dbo].[ShipmentReceivableStatus] ADD [AutomaticLastUpdateDate] DATETIME DEFAULT(GETDATE()) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d79fe1a6-7179-4c10-b954-f5583ca70fd5', 'ShipmentReceivableStatus.dxml', 'ShipmentReceivableStatus', 'AutomaticLastUpdateDate', 'Add Column', GETDATE(), '-- Add New Column With Name AutomaticLastUpdateDateALTER TABLE [dbo].[ShipmentReceivableStatus] ADD [AutomaticLastUpdateDate] DATETIME DEFAULT(GETDATE()) NOT NULL;');

-- Create Index On ShipmentReceivableStatus Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentReceivableStatus_AutomaticLastUpdateDate] ON [dbo].[ShipmentReceivableStatus]([AutomaticLastUpdateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1f152a85-e134-4651-86a2-131560d23393', 'ShipmentReceivableStatus.dxml', 'ShipmentReceivableStatus', 'AutomaticLastUpdateDate', 'Create Index', GETDATE(), '-- Create Index On ShipmentReceivableStatus TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentReceivableStatus_AutomaticLastUpdateDate] ON [dbo].[ShipmentReceivableStatus]([AutomaticLastUpdateDate])'');');


-- Add Foreign Key Constraint For Column QuoteClosingReasonId In Table Quotes As Reference To Column Id In Table QuoteClosingReasons
EXEC('ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_QuoteClosingReasons_QuoteClosingReasonId] FOREIGN KEY([QuoteClosingReasonId]) REFERENCES [dbo].[QuoteClosingReasons]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c1bea17c-f1f8-4420-aa09-82bf724fc365', 'Quote.dxml', 'Quotes', 'QuoteClosingReasonId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column QuoteClosingReasonId In Table Quotes As Reference To Column Id In Table QuoteClosingReasonsEXEC(''ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_Quotes_QuoteClosingReasons_QuoteClosingReasonId] FOREIGN KEY([QuoteClosingReasonId]) REFERENCES [dbo].[QuoteClosingReasons]([Id])'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3fd460ff-9a1a-431b-8fc4-64ff18902a2a', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On QuoteClosingReasons Table
EXEC('CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d39b2ffe-6b6a-4946-b275-538f168e04cc', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On QuoteClosingReasons TableEXEC(''CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_CreatedByUserId] ON [dbo].[QuoteClosingReasons]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e36e85ae-3204-47e3-9eeb-27f186cfacd2', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table QuoteClosingReasons As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[QuoteClosingReasons] ADD CONSTRAINT [FK_QuoteClosingReasons_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On QuoteClosingReasons Table
EXEC('CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ba4f6870-9676-4129-bbec-4fa0cd81bdc9', 'QuoteClosingReason.dxml', 'QuoteClosingReasons', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On QuoteClosingReasons TableEXEC(''CREATE NONCLUSTERED INDEX [IX_QuoteClosingReasons_UpdatedByUserId] ON [dbo].[QuoteClosingReasons]([UpdatedByUserId])'');');


-- Add Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize As Reference To Column Id In Table InsideShipmentPackages
EXEC('ALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] FOREIGN KEY([InsidePackageId]) REFERENCES [dbo].[InsideShipmentPackages]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8c39a5ef-b93d-47e2-8a9e-ce7bb0c1c4a5', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column InsidePackageId In Table ShipmentPackageHarmonize As Reference To Column Id In Table InsideShipmentPackagesEXEC(''ALTER TABLE [dbo].[ShipmentPackageHarmonize] ADD CONSTRAINT [FK_ShipmentPackageHarmonize_InsideShipmentPackages_InsidePackageId] FOREIGN KEY([InsidePackageId]) REFERENCES [dbo].[InsideShipmentPackages]([Id])'');');

-- Create Index On ShipmentPackageHarmonize Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize]([InsidePackageId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dffdc82d-6396-41cb-ab9f-0755405f864c', 'ShipmentPackageHarmonize.dxml', 'ShipmentPackageHarmonize', 'InsidePackageId', 'Create Index', GETDATE(), '-- Create Index On ShipmentPackageHarmonize TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ShipmentPackageHarmonize_InsidePackageId] ON [dbo].[ShipmentPackageHarmonize]([InsidePackageId])'');');


-- DataView Script From CardGLAccountDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[CardGLAccountDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[CardGLAccountDataView] END');
EXEC('CREATE VIEW [dbo].[CardGLAccountDataView]
AS SELECT
dbo.Cards.EnglishName AS CardEnglishName, dbo.Cards.GLAccountId AS CardGLAccountId, dbo.GLAccounts.EnglishName AS GLAccountEnglishName,
dbo.Cards.SalesmanUserId, SalesMan.EnglishName AS SalesManEnglishName, dbo.Cards.CollectorId, Collectors.EnglishName AS CollectorEnglishName,
dbo.GLAccounts.Id, dbo.GLAccounts.Tenant, dbo.GLAccounts.InternalNumber, dbo.GLAccounts.AccountTypeCode, dbo.GLAccounts.DisplayNumber,
dbo.GLAccounts.LocalName AS GLAccountLocalName, dbo.GLAccounts.SearchFields, dbo.GLAccounts.IsMultiCurrency, dbo.GLAccounts.CurrencyId,
dbo.GLAccounts.RevenueExpenseType, dbo.GLAccounts.IsControlAccount, dbo.GLAccounts.ChartOfAccountsId, dbo.GLAccounts.Inactive,
dbo.GLAccounts.ChartOfAccountsTypeCode, dbo.GLAccounts.ReconcileMethodCode, dbo.GLAccounts.ControlAccountId, dbo.GLAccounts.AutomaticReconcileId,
dbo.GLAccounts.PreviousEnglishName, dbo.GLAccounts.PreviousEnglishNameChangeDate, dbo.GLAccounts.PreviousLocalName,
dbo.GLAccounts.PreviousLocalNameChangeDate, dbo.GLAccounts.PreviousNumber, dbo.GLAccounts.PreviousNumberChangeDate,
dbo.GLAccounts.PreviousChartOfAccountsId, dbo.GLAccounts.PreviousChartOfAccountsChangeDate, MOREDATA.BalanceInLocalCurrency,
dbo.GLAccounts.RevaluationEnabled, dbo.GLAccounts.ParentAccountId, dbo.GLAccounts.Category2Id, dbo.GLAccounts.Category3Id, dbo.GLAccounts.Category4Id,
dbo.GLAccounts.Category5Id, dbo.GLAccounts.Category1Id, dbo.GLAccounts.IsVATExempt, dbo.GLAccounts.CustomerGLAccountId, dbo.Cards.VatNumber,
MOREDATA.LocalBalanceInDue, MOREDATA.NextDueDate,
dbo.Cards.LocalName AS CardLocalName, SalesMan.LocalName AS SalesManLocalName, Collectors.LocalName AS CollectorLocalName, dbo.Cards.VatTypeId,
dbo.Cards.CountryId, dbo.Cards.CountryCode, dbo.Cards.CityName, dbo.Cards.CountryName, dbo.Cards.PaymentTermId,
dbo.ChartOfAccounts.LocalName AS ChartOfAccountsLocalName, dbo.ChartOfAccounts.EnglishName AS ChartOfAccountsEnglishName,
dbo.ChartOfAccounts.Code AS ChartOfAccountsCode
FROM dbo.Cards
LEFT OUTER JOIN
dbo.Contacts
AS Collectors
ON dbo.Cards.CollectorId = Collectors.Id
LEFT OUTER JOIN
dbo.Contacts
AS SalesMan
ON dbo.Cards.SalesmanUserId = SalesMan.Id
RIGHT OUTER JOIN
dbo.GLAccounts
ON dbo.Cards.GLAccountId = dbo.GLAccounts.Id
LEFT OUTER JOIN
dbo.ChartOfAccounts
ON dbo.GLAccounts.ChartOfAccountsId = dbo.ChartOfAccounts.Id
INNER JOIN dbo.GLAccountMoreDatas
AS MOREDATA
ON dbo.GLAccounts.Id = MOREDATA.AccountId');


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


-- DataView Script From ShipmentCountryDashboardView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentCountryDashboardView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentCountryDashboardView] END');
EXEC('CREATE VIEW [dbo].[ShipmentCountryDashboardView]
AS SELECT
dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber, dbo.Shipments.TransportModeId, dbo.Shipments.DirectionId,
dbo.Shipments.CountryForStatisticsId, dbo.Countries.Code AS CountryForStatisticsCode, dbo.Countries.EnglishName AS CountryForStatisticsName,
dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.CustomerId, dbo.Shipments.CreateDateTime, dbo.Shipments.OperationalDate,
dbo.Shipments.ShipmentLevelCode, dbo.Shipments.ProfitInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency,dbo.Shipments.OpenReceivablesInProfitCurrency,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.BranchId, dbo.Shipments.IsCancelled
FROM
dbo.Shipments
INNER JOIN
dbo.Countries
ON dbo.Shipments.CountryForStatisticsId = dbo.Countries.Id');


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


-- DataView Script From ShipmentDirectionTransmodeView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentDirectionTransmodeView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentDirectionTransmodeView] END');
EXEC('CREATE VIEW [dbo].[ShipmentDirectionTransmodeView]
AS SELECT
dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber, dbo.Shipments.CreateDateTime,dbo.Shipments.OperationalDate, dbo.Shipments.TransportModeId,
dbo.Shipments.DirectionId, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.OpenReceivablesInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.ProfitInLocalCurrency, dbo.Shipments.CustomerId, dbo.Shipments.ProfitInProfitCurrency,
dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.IsCancelled, dbo.Shipments.BranchId
FROM
dbo.Shipments
INNER JOIN
dbo.Directions
ON dbo.Shipments.DirectionId = dbo.Directions.Id
INNER JOIN
dbo.TransportModes
ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id');


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


-- DataView Script From ShipmentsCustomersDashboardView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentsCustomersDashboardView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentsCustomersDashboardView] END');
EXEC('CREATE VIEW [dbo].[ShipmentsCustomersDashboardView]
AS SELECT
dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber, dbo.Shipments.BranchId, dbo.Shipments.CreateDateTime, dbo.Shipments.OperationalDate,
dbo.Shipments.TransportModeId, dbo.Shipments.DirectionId, dbo.Shipments.IsCancelled, dbo.Shipments.OpenReceivablesInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.ProfitInLocalCurrency, dbo.Shipments.CustomerId, dbo.Shipments.ProfitInProfitCurrency,
dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.ShipmentLevelCode,
dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Cards.EnglishName AS CustomerName
FROM
dbo.Shipments
INNER JOIN
dbo.Cards
ON dbo.Shipments.CustomerId = dbo.Cards.Id');


-- Procedure Script From usp_DeleteCRMRecords.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteCRMRecords]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteCRMRecords] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteCRMRecords
(
@Tenant int
)
AS
BEGIN
delete from ActivityNotes where Tenant = @Tenant
delete from ActivityOwnerHistories where Tenant = @Tenant
delete from ActivityInvitees where Tenant = @Tenant
delete from ActivityEmailRecipients where Tenant = @Tenant
delete from Activities where Tenant = @Tenant
delete from OpportunityStages  where Tenant = @Tenant
delete from OpportunityProductLocations  where Tenant = @Tenant
delete from OpportunityProducts  where Tenant = @Tenant
delete from OpportunityCompetitors  where Tenant = @Tenant
delete from OpportunityAdditionalServices  where Tenant = @Tenant
delete from Opportunities  where Tenant = @Tenant
END');


-- Procedure Script From usp_DeleteTicketsRecords.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteTicketsRecords]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteTicketsRecords] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteTicketsRecords
(
@Tenant int
)
AS
BEGIN
delete from InboundEmailLines where Tenant = @Tenant
delete from InboundEmails where Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = ''Ticket'')
delete from Correspondences where Tenant =  @Tenant
delete from TicketEscalations where Tenant =  @Tenant
delete from Tickets where Tenant =  @Tenant
END');


-- Procedure Script From ChangeVendorToTrucker.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ChangeVendorToTrucker]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[ChangeVendorToTrucker] END');
EXEC('--CREATE  PROCEDURE [dbo].[ChangeVendorToTrucker]
--(
--@tenant int,@Code varchar(15)               --Input parameter ,  tenant to delete
--)
--AS
--declare @Id as varchar(15)
--set @Id = (select id from cards where tenant = @tenant and code = @Code)
--BEGIN
--insert into truckers values (@tenant,0,@Id)
--update cards set partnertypeid = ''TR'' where Id = @Id and Tenant = @tenant
--delete from vendors where id = @Id and tenant = @tenant
--END');


-- Procedure Script From DeleteOldAuthenticationTokens.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldAuthenticationTokens]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldAuthenticationTokens] END');
EXEC('Create procedure [dbo].[DeleteOldAuthenticationTokens]
as
begin
delete from [dbo].[AuthenticationTokens] where [ExpirationDate] < GETDATE() - 14 and ClientType = ''DocumentDownload''
end');


-- Procedure Script From DeleteOldCommunicationLogs.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldCommunicationLogs]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldCommunicationLogs] END');
EXEC('create procedure [dbo].[DeleteOldCommunicationLogs]
as
begin
IF OBJECT_ID(''dbo.TempDeletedCommunicationLogs'') IS NOT NULL
DROP TABLE TempDeletedCommunicationLogs
SELECT * INTO TempDeletedCommunicationLogs
FROM (SELECT top(1000) Id,DocumentId
FROM CommunicationLogs
WHERE CreateDate < GETDATE() - 120) AS t
DELETE FROM CommunicationLogs WHERE Id IN (SELECT Id FROM TempDeletedCommunicationLogs)
UPDATE Documents SET MarkForDelete = 1 WHERE Id IN (SELECT DocumentId FROM TempDeletedCommunicationLogs)
end');


-- Procedure Script From DeleteOldFilingInboxes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldFilingInboxes]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldFilingInboxes] END');
EXEC('create procedure [dbo].[DeleteOldFilingInboxes]
as
begin
delete from FilingInboxAttachmentLogs where FilingInboxAttachmentId in (select id from FilingInboxAttachments where FilingInboxId in (select id from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE())))
delete from FilingInboxAttachments where FilingInboxId in (select id from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE()))
delete from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE())
end');


-- Procedure Script From DeleteTenantFromMainDB.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteTenantFromMainDB]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteTenantFromMainDB] END');
EXEC('CREATE  PROCEDURE dbo.DeleteTenantFromMainDB
(
@tenant int                     --Input parameter ,  tenant to delete
)
AS
BEGIN
----delete tenants stuff
delete from accountingsettings
where id=@tenant
delete from contacttenantroleset
where tenant=@tenant
delete from contacttenants
where tenantid=@tenant
delete from tenants
where id=@tenant
-------------------------------------
delete from followups
where tenant=@tenant
--delete shipments for tenant
delete from insideshipmentpackages
where tenant=@tenant
delete from shipmentpackages
where tenant=@tenant
delete from ShipmentPickUpDeliveryPackages
where tenant=@tenant
delete from shipmentpickupdeliveries
where tenant=@tenant
delete from shipmentorderpackages
where tenant=@tenant
delete from shipmentpayables
where tenant=@tenant
delete from shipmentreceivables
where tenant=@tenant
delete from shipmentawbprintonlies
where tenant=@tenant
delete from shipmentmasterdatas
where tenant=@tenant
delete from shipments
where tenant=@tenant
---end of shipment deletions
--delete apinvoices stuff-------------
delete from apinvoiceentities
where tenant=@tenant
delete from apinvoicelines
where tenant=@tenant
delete from apinvoicetotalvats
where tenant=@tenant
delete from apinvoicepayments
where tenant=@tenant
delete from appayments
where tenant=@tenant
delete from apinvoices
where tenant=@tenant
--delete arinvoices stuff-------------
delete from arinvoiceentities
where tenant=@tenant
delete from arinvoicelines
where tenant=@tenant
delete from arinvoicetotalvats
where tenant=@tenant
delete from arinvoicepayments
where tenant=@tenant
delete from arpayments
where tenant=@tenant
delete from arinvoices
where tenant=@tenant
-----------------------------------------------------------------
--delete quotes
delete from quotepricesteps
where tenant=@tenant
delete from quotecharges
where tenant=@tenant
delete from quotes
where tenant=@tenant
--------------------------------------------
delete from communicationattachments
where tenant=@tenant
delete from communicationlogs
where tenant=@tenant
delete from documentoutcopies
where tenant=@tenant
delete from documentouts
where tenant=@tenant
delete from documentins
where tenant=@tenant
delete from documenttypecopies
where tenant=@tenant
delete from documenttypecustomfields1
where tenant=@tenant
delete from formcustomfields1
where tenant=@tenant
delete from documenttypetemplates
where tenant=@tenant
delete from documenttypes
where tenant=@tenant
----------------------------------------------------------------
-------------------delete cards but card and addresses is for last
delete from agents
where tenant=@tenant
delete from customers
where tenant=@tenant
delete from airlines
where tenant=@tenant
delete from truckers
where tenant=@tenant
delete from shippinglines
where tenant=@tenant
delete from vendors
where tenant=@tenant
delete from warehouses
where tenant=@tenant
delete from Customagents
where tenant=@tenant
delete from shippingagents
where tenant=@tenant
delete from potentialcustomers
where tenant=@tenant
delete from addresses
where tenant=@tenant
delete from cardcontacts
where tenant=@tenant
delete from cards
where tenant=@tenant
delete from mawbstacks
where tenant=@tenant
----------------------------------------
-- billing stuff
delete from incoterms
where tenant=@tenant
delete from ratestables
where tenant=@tenant
delete from currencies
where tenant=@tenant
delete from paymentterms
where tenant=@tenant
delete from chargestypes
where tenant=@tenant
delete from VatTypePercentages
where tenant=@tenant
delete from vattypes
where tenant=@tenant
delete from Accounts1
where tenant=@tenant
-----------------------------
-----------delete location stuff
delete from ports
where tenant=@tenant
delete from states
where tenant=@tenant
delete from countries
where tenant=@tenant
delete from globalzones
where tenant=@tenant
delete from tarrifsteps
where tenant=@tenant
delete from tarriffromtoes
where tenant=@tenant
delete from tarrifcharges
where tenant=@tenant
delete from tarrifheaders
where tenant=@tenant
----------------------------
---------contact users branch departments
delete from userloginlogs
where tenant=@tenant
delete from traceevents
where tenant=@tenant
delete from users
where tenant=@tenant
delete from contacts
where tenant=@tenant
delete from branches
where tenant=@tenant
delete from departments
where tenant=@tenant
---------------------------------------
delete from eventtypes
where tenant=@tenant
delete from entitystatus
where tenant=@tenant
delete from querycolumns
where tenant=@tenant
delete from advancedqueryfilters
where tenant=@tenant
delete from queries
where tenant=@tenant
delete from blobs
where tenant=@tenant
delete from counterdefinitions
where tenant=@tenant
delete from counterlastnumbers
where tenant=@tenant
delete from counterstats
where tenant=@tenant
delete from counters
where tenant=@tenant
delete from customtables
where tenant=@tenant
delete from descriptionofgoods
where tenant=@tenant
delete from entitylastaccesses
where tenant=@tenant
delete from packagetypes
where tenant=@tenant
delete from measurements
where tenant=@tenant
delete from ranks
where tenant=@tenant
delete from vessels
where tenant=@tenant
----------------------------------
-------------------metadata stuff
delete from objecttablerulefields
where tenant=@tenant
delete from objecttablerules
where tenant=@tenant
delete from objectfieldvalidations
where tenant=@tenant
delete from screenfields
where tenant=@tenant
delete from objectfields
where tenant=@tenant
delete from objecttabletabs
where tenant=@tenant
delete from objecttablehelpercontrols
where tenant=@tenant
delete from menuButtons
where tenant=@tenant
delete from menubuttonGroups
where tenant=@tenant
delete from menustables
where tenant=@tenant
update objecttables
set DescriptionTextCodeId=null
where tenant=@tenant
delete from textcodes
where tenant=@tenant
update objecttables
set HeaderScreenId=null
where tenant=@tenant
delete from screenfields
where tenant=@tenant
delete from screens
where tenant=@tenant
delete from objecttables
where tenant =@tenant
delete from tips
where tenant=@tenant
delete from rolefeatures
where tenant=@tenant
delete from features
where tenant=@tenant
delete from roles
where tenant=@tenant
END');


-- Procedure Script From DeleteTenantFromMainDB_Jalal.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteTenantFromMainDB_Jalal]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteTenantFromMainDB_Jalal] END');
EXEC('Create  PROCEDURE DeleteTenantFromMainDB_Jalal
(
@tenant int                     --Input parameter ,  tenant to delete
)
AS
Begin
----delete tenants stuff
delete from accountingsettings
where id=@tenant
delete from Restrictions
where Tenant=@tenant
delete from contacttenantroleset
where tenant=@tenant
delete from contacttenants
where tenantid=@tenant
-------------------------------------
delete from followups
where tenant=@tenant
--delete shipments for tenant
delete from ShipmentPackageItems
where tenant=@tenant
delete from insideshipmentpackages
where tenant=@tenant
delete from shipmentpackages
where tenant=@tenant
delete from ShipmentPickUpDeliveryPackages
where tenant=@tenant
delete from shipmentpickupdeliveries
where tenant=@tenant
delete from shipmentorderpackages
where tenant=@tenant
delete from shipmentpayables
where tenant=@tenant
delete from shipmentreceivables
where tenant=@tenant
delete from shipmentawbprintonlies
where tenant=@tenant
--update ShipmentMasterDatas
--set Id = null
--where tenant=@tenant
delete from ShipmentComputedFields
where tenant=@tenant
delete from ShipmentCommodities
where tenant=@tenant
delete from AWBMessagingStocks where TenantNumber=@tenant
delete from AWBStockUsageHistories
where tenant=@tenant
--IF EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = ''FK_ShipmentShipmentMasterData''  and parent_object_id = OBJECT_ID(N''dbo.ShipmentMasterDatas''))
--BEGIN
--ALTER TABLE ShipmentMasterDatas DROP CONSTRAINT FK_ShipmentShipmentMasterData
--END
--update Shipments
--set Id = CONCAT(Id,''anyid'')
--where tenant=@tenant
delete from AWBOCIs
where tenant=@tenant
---end of shipment deletions
--delete apinvoices stuff-------------
delete from apinvoiceentities
where tenant=@tenant
delete from apinvoicelines
where tenant=@tenant
delete from apinvoicetotalvats
where tenant=@tenant
delete from apinvoicepayments
where tenant=@tenant
delete from appayments
where tenant=@tenant
delete from apinvoices
where tenant=@tenant
--delete arinvoices stuff-------------
delete from arinvoiceentities
where tenant=@tenant
delete from arinvoicelines
where tenant=@tenant
delete from arinvoicetotalvats
where tenant=@tenant
delete from arinvoicepayments
where tenant=@tenant
delete from arpayments
where tenant=@tenant
delete from arinvoices
where tenant=@tenant
-----------------------------------------------------------------
delete from ActivityNotes
where tenant=@tenant
delete from ActivityEmailRecipients
where tenant=@tenant
delete from ActivityInvitees
where tenant=@tenant
delete from ActivityOwnerHistories
where tenant=@tenant
delete from Activities
where tenant=@tenant
--delete quotes
delete from QuotePackages
where tenant=@tenant
delete from quotepricesteps
where tenant=@tenant
delete from quotecharges
where tenant=@tenant
delete from QuoteDocumentVersions
where tenant=@tenant
--------------------------------------------
delete from communicationattachments
where tenant=@tenant
delete from communicationlogs
where tenant=@tenant
delete from documentoutcopies
where tenant=@tenant
delete from documentouts
where tenant=@tenant
--delete from documentins
--where tenant=@tenant
delete from documenttypecopies
where tenant=@tenant
delete from documenttypecustomfields1
where tenant=@tenant
delete from formcustomfields1
where tenant=@tenant
delete from documenttypetemplates
where tenant=@tenant
delete from DocumentsFilings
where tenant=@tenant
delete from documenttypes
where tenant=@tenant
----------------------------------------------------------------
-------------------delete cards but card and addresses is for last
delete from customerProducts
where tenant=@tenant
delete from agents
where tenant=@tenant
delete from CustomerProductActualDatas
where tenant=@tenant
delete from CustomerProductLocations
where tenant=@tenant
delete from CustomerProductLocationActualDatas
where tenant=@tenant
delete from CustomerCompetitors
where tenant=@tenant
delete from CustomerAdditionalServices
where tenant=@tenant
delete from customers
where tenant=@tenant
delete from MAWBStacks
where tenant=@tenant
delete from airlines
where tenant=@tenant
delete from truckers
where tenant=@tenant
delete from shippinglines
where tenant=@tenant
delete from vendors
where tenant=@tenant
delete from warehouses
where tenant=@tenant
delete from Customagents
where tenant=@tenant
delete from shippingagents
where tenant=@tenant
--delete from potentialcustomers
--where tenant=@tenant
delete from OpportunityCompetitors
where tenant=@tenant
delete from Competitors
where tenant=@tenant
delete from Customs.ConsignmentInternalTransitions
where tenant=@tenant
delete from Customs.Consignments
where tenant=@tenant
--delete from Customs.SupplierInvioceItemCertificats
--where tenant=@tenant
--delete from Customs.SupplierInvoiceItemsConDeclars
--where tenant=@tenant
--delete from Customs.SupplierInvoiceItemsProdIdents
--where tenant=@tenant
--delete from Customs.SupplierInvoiceItemsSerialNums
--where tenant=@tenant
--delete from Customs.SupplierInvoiceItemsDescripts
--where tenant=@tenant
delete from Customs.SupplierInvoiceItemsTaxes
where tenant=@tenant
delete from Customs.SupplierInvoiceItems
where tenant=@tenant
delete from Customs.SupplierInvoiceFreightAmounts
where tenant=@tenant
delete from Customs.SupplierInvoices
where tenant=@tenant
delete from Customs.DeclarationPayments
where tenant=@tenant
delete from Customs.DeclarationPaymentProtests
where tenant=@tenant
delete from Customs.DeclarationPaymentMethods
where tenant=@tenant
delete from Customs.DeclarationTaxes
where tenant=@tenant
delete from Customs.Declarations
where tenant=@tenant
delete from cardcontacts
where tenant=@tenant
delete from OpportunityProducts
where tenant=@tenant
delete from OpportunityStages
where tenant=@tenant
delete from OpportunityProductLocations
where tenant=@tenant
delete from OpportunityAdditionalServices
where tenant=@tenant
delete from Opportunities
where tenant=@tenant
delete from CustomerMediatorByProducts
where tenant=@tenant
delete from CustomerForwarderByProducts
where tenant=@tenant
delete from CustomerSalesNote
where tenant=@tenant
delete from CustomerCustomsAgentByProducts
where tenant=@tenant
----------------------------------
delete from shipmentmasterdatas where Tenant = 0
delete from shipments where Tenant = 0
delete from quotes where Tenant = 0
delete from shipmentmasterdatas
where tenant=@tenant
update Shipments set MasterShipmentDataId = null
where tenant=@tenant
delete from shipments
where tenant=@tenant
delete from quotes
where tenant=@tenant
update Tenants set AddressId = null where id = @tenant
update Tenants set agentid = null where Id = @tenant
update Addresses set CardId= null where Tenant = @tenant
delete from addresses
where tenant=@tenant
delete from cards
where tenant=@tenant
----------------------------------
delete from mawbstacks
where tenant=@tenant
----------------------------------------
-- billing stuff
delete from incoterms
where tenant=@tenant
delete from ratestables
where tenant=@tenant
update tenants set OtherChargesCurrencyId = null
where id=@tenant
update tenants set ProfitCurrencyId = null
where id=@tenant
update tenants set QuoteSaleCurrencyId = null
where id=@tenant
update tenants set CurrencyId = null
where id=@tenant
update tenants set FreightCurrencyId = null
where id=@tenant
update tenants set PaymentTermId = null
where id=@tenant
delete from currencies
where tenant=@tenant
delete from ChargeTypeAccountings
where tenant=@tenant
delete from paymentterms
where tenant=@tenant
delete from chargestypes
where tenant=@tenant
delete from VatTypePercentages
where tenant=@tenant
delete from vattypes
where tenant=@tenant
delete from Accounts1
where tenant=@tenant
-----------------------------
-----------delete location stuff
update Shipments set FromPortId = null
delete from ShipmentCarrierStatuses
where tenant=@tenant
delete from ports
where tenant=@tenant
delete from states
where tenant=@tenant
delete from CountryCities
where tenant=@tenant
update Tenants set VatMandatoryCountryId = null where Id=@tenant
update Cards set CountryId = null where Id in (select id from cards where tenant=@tenant)
delete from countries
where tenant=@tenant
delete from globalzones
where tenant=@tenant
delete from tarrifsteps
where tenant=@tenant
delete from tarriffromtoes
where tenant=@tenant
delete from tarrifcharges
where tenant=@tenant
delete from tarrifheaders
where tenant=@tenant
----------------------------
---------contact users branch departments
delete from userloginlogs
where tenant=@tenant
delete from traceevents
where tenant=@tenant
delete from CustomerAccountManagerByProducts
where tenant=@tenant
delete from CustomerSalesmanByProducts
where tenant=@tenant
delete from UserLastLogins
where tenant=@tenant
delete from ConversationHeaderParticipants
where tenant=@tenant
delete from ConversationHeaderMessages
where tenant=@tenant
delete from ConversationHeaders
where tenant=@tenant
delete from EntityLastActivities
where tenant=@tenant
delete from Feeds
where tenant=@tenant
delete from PostLikes
where tenant=@tenant
delete from Followers
where tenant=@tenant
delete from FollowEntities
where tenant=@tenant
delete from Ports
where tenant=@tenant
delete from CRMFilterSettings
where tenant=@tenant
delete From QuestionnaireQuestions
where tenant=@tenant
delete from QuestionnaireAnswerLines
where tenant=@tenant
delete from QuestionnaireAnswers
where tenant=@tenant
delete from Questionnaires
where tenant=@tenant
delete from TipsVisibilities
where tenant=@tenant
delete from QuoteTemplateDetailsFields
where tenant=@tenant
delete from QuoteTemplateHeaderFields
where tenant=@tenant
delete from QuoteTemplateTextCodes
where tenant=@tenant
delete from QuoteTemplateSections
where tenant=@tenant
delete from QuoteTemplates
where tenant=@tenant
delete from AccountingTransferLines
where tenant=@tenant
delete from AccountingTransferHeaders
where tenant=@tenant
delete from Translations
where tenant=@tenant
delete from ObjectTableLastUpdates
where tenant=@tenant
delete from TipsVisibilities
delete from SharedLogisticsUpdates
where tenant=@tenant
delete from EntityLastUpdates
where tenant=@tenant
delete from TicketClassifications
where tenant=@tenant
delete from EmployeeGroups
where tenant=@tenant
delete from Posts
where tenant=@tenant
delete from TraceEvents where UserId in (select Id from Users
where tenant=@tenant)
delete from BusinessHours
where tenant=@tenant
delete from SLAHeaders
where tenant=@tenant
delete from Users
where tenant=@tenant
delete from TermsofUseSignatures
where tenant=@tenant
delete from ContactsUnseenEntities
where tenant=@tenant
delete from ContactLoginLogs
where tenant=@tenant
delete from ContactLastLogins
where tenant=@tenant
delete from contacts
where tenant=@tenant
delete from branches
where tenant=@tenant
delete from departments
where tenant=@tenant
---------------------------------------
delete from eventtypes
where tenant=@tenant
delete from entitystatus
where tenant=@tenant
delete from querycolumns
where tenant=@tenant
delete from advancedqueryfilters
where tenant=@tenant
delete from queries
where tenant=@tenant
delete from blobs
where tenant=@tenant
delete from counterdefinitions
where tenant=@tenant
delete from counterlastnumbers
where tenant=@tenant
delete from counterstats
where tenant=@tenant
delete from counters
where tenant=@tenant
delete from customtables
where tenant=@tenant
delete from descriptionofgoods
where tenant=@tenant
delete from packagetypes
where tenant=@tenant
delete from measurements
where tenant=@tenant
delete from ranks
where tenant=@tenant
delete from vessels
where tenant=@tenant
-------------------metadata stuff
delete from objecttablerulefields
where tenant=@tenant
delete from objecttablerules
where tenant=@tenant
delete from objectfieldvalidations
where tenant=@tenant
delete from screenfields
where tenant=@tenant
delete from objectfields
where tenant=@tenant
delete from objecttabletabs
where tenant=@tenant
delete from objecttablehelpercontrols
where tenant=@tenant
delete from menuButtons
where tenant=@tenant
delete from menubuttonGroups
where tenant=@tenant
delete from menustables
where tenant=@tenant
update objecttables
set DescriptionTextCodeId=null
where tenant=@tenant
delete from Queries
where tenant=@tenant
delete from AdvancedQueryFilters
where QueryId in (
select Id from Queries
where NameTextCodeId in (select Id from textcodes
where tenant=@tenant))
delete from QueryColumns
where QueryId in (
select Id from Queries
where NameTextCodeId in (select Id from textcodes
where tenant=@tenant))
delete from Queries
where NameTextCodeId in (select Id from textcodes
where tenant=@tenant)
delete from textcodes
where tenant=@tenant
update objecttables
set HeaderScreenId=null
where tenant=@tenant
delete from screenfields
where tenant=@tenant
delete from screens
where tenant=@tenant
delete from objecttables
where tenant =@tenant
delete from tips
where tenant=@tenant
delete from rolefeatures
where tenant=@tenant
delete from features
where tenant=@tenant
delete from roles
where tenant=@tenant
--delete from tenants
--where id=@tenant
delete from Industries
where tenant=@tenant
delete from dbo.RoleFeatures where tenant=@tenant
delete from dbo.ObjectFields where tenant=@tenant
delete from dbo.Documents where tenant=@tenant
delete from dbo.PackageFeatures where tenant=@tenant
delete from dbo.EventTypes where tenant=@tenant
delete from dbo.QueryColumns where tenant=@tenant
delete from dbo.DocumentTypeCopies where tenant=@tenant
delete from dbo.Industries where tenant=@tenant
delete from dbo.Features where tenant=@tenant
delete from dbo.UserLoginLogs where tenant=@tenant
delete from dbo.DocumentTypes where tenant=@tenant
delete from dbo.ScreenFields where tenant=@tenant
delete from dbo.Cards where tenant=@tenant
delete from dbo.Airlines where tenant=@tenant
delete from dbo.EmailAlertSettings where tenant=@tenant
delete from dbo.ProductTypeModifications where tenant=@tenant
delete from dbo.LeadSources where tenant=@tenant
delete from dbo.MoveTypes where tenant=@tenant
delete from dbo.EntityLastActivities where tenant=@tenant
delete from dbo.QuoteStages where tenant=@tenant
delete from dbo.Measurements where tenant=@tenant
delete from dbo.ObjectTables where tenant=@tenant
delete from dbo.ObjectTableTabs where tenant=@tenant
delete from QuoteTemplateTableDesigns where tenant=@tenant
delete from dbo.QuoteTemplateSettings where tenant=@tenant
delete from dbo.QuoteTemplateTextDesigns where tenant=@tenant
delete from dbo.Countries where tenant=@tenant
delete from dbo.OpportunityClosingReasons where tenant=@tenant
delete from dbo.OpportunityTypes where tenant=@tenant
delete from dbo.CounterDefinitions where tenant=@tenant
delete from SLALines where tenant=@tenant
delete from dbo.TicketSeverities where tenant=@tenant
delete from dbo.Screens where tenant=@tenant
delete from dbo.Stages where tenant=@tenant
delete from dbo.TenantSettings where tenant=@tenant
--delete from dbo.DBIdCounters where tenant=@tenant
delete from dbo.Queries where tenant=@tenant
delete from dbo.TicketStages where tenant=@tenant
delete from dbo.DocumentTypeCustomFields1 where tenant=@tenant
delete from dbo.CustomerSizes where tenant=@tenant
delete from dbo.ExternalSystemsMissingTranslations where tenant=@tenant
delete from dbo.EntityStatus where tenant=@tenant
delete from dbo.CreditCardTypes where tenant=@tenant
delete from dbo.TraceEvents where tenant=@tenant
delete from dbo.AdditionalServices where tenant=@tenant
delete from dbo.ObjectTableRuleFields where tenant=@tenant
delete from dbo.ShippingLines where tenant=@tenant
delete from dbo.Addresses where tenant=@tenant
delete from dbo.TicketTypes where tenant=@tenant
delete from dbo.MenuButtons where tenant=@tenant
delete from dbo.AdvancedQueryFilters where tenant=@tenant
delete from dbo.MenusTables where tenant=@tenant
delete from dbo.States where tenant=@tenant
delete from dbo.BusinessUnits where tenant=@tenant
delete from dbo.ChargesTypes where tenant=@tenant
delete from dbo.DocumentTypeTemplates where tenant=@tenant
delete from dbo.ObjectTableRules where tenant=@tenant
delete from dbo.ExternalSystemsTablesCodes where tenant=@tenant
delete from dbo.Ranks where tenant=@tenant
delete from dbo.PackageTypes where tenant=@tenant
delete from Customs.VendorCommunications where tenant=@tenant
delete from Customs.ConsignmentPackages where tenant=@tenant
delete from dbo.RuleConditionFields where tenant=@tenant
delete from dbo.MenuButtonGroups where tenant=@tenant
delete from dbo.Counters where tenant=@tenant
delete from dbo.ImageDetails where tenant=@tenant
delete from dbo.AuthenticationTokens where tenant=@tenant
delete from dbo.Accounts1 where tenant=@tenant
delete from dbo.Currencies where tenant=@tenant
delete from dbo.CustomPickLists where tenant=@tenant
delete from dbo.Incoterms where tenant=@tenant
delete from dbo.ScreenModifications where tenant=@tenant
delete from dbo.Roles where tenant=@tenant
delete from dbo.ContactTenantRoleSet where tenant=@tenant
delete from dbo.ObjectTableHelperControls where tenant=@tenant
delete from Customs.ClientAddresses where tenant=@tenant
delete from Customs.Clients where tenant=@tenant
delete from dbo.GlobalZones where tenant=@tenant
delete from dbo.CounterLastNumbers where tenant=@tenant
delete from dbo.AccountingSystemsSettings where tenant=@tenant
delete from dbo.Contacts where tenant=@tenant
delete from Customs.CustomsVendors where tenant=@tenant
delete from dbo.ObjectFieldValidations where tenant=@tenant
delete from dbo.VatTypes where tenant=@tenant
delete from dbo.PaymentTerms where tenant=@tenant
delete from dbo.QuickbooksSyncRequestTickets where tenant=@tenant
delete from dbo.ReportGroups where tenant=@tenant
delete from dbo.Users where tenant=@tenant
delete from Customs.PaymentOrderMethods where tenant=@tenant
delete from dbo.Reports where tenant=@tenant
delete from dbo.ReportModifications where tenant=@tenant
delete from dbo.Tips where tenant=@tenant
delete from dbo.AccountingSystemsSyncStatuses where tenant=@tenant
delete from dbo.Branches where tenant=@tenant
delete from dbo.CRMFilterSettings where tenant=@tenant
delete from Customs.CustomBanks where tenant=@tenant
delete from dbo.Departments where tenant=@tenant
delete from Customs.PaymentOrderProtestReasons where tenant=@tenant
delete from Customs.PaymentOrders where tenant=@tenant
delete from dbo.SpecialServicesTypes where tenant=@tenant
delete from dbo.TermsofUseSignatures where tenant=@tenant
delete from dbo.Customers where tenant=@tenant
delete from dbo.Regions where tenant=@tenant
delete from dbo.Agents where tenant=@tenant
delete from dbo.Commodities where tenant=@tenant
delete from dbo.EmployeeGroups where tenant=@tenant
delete from dbo.ObjectFieldModifications where tenant=@tenant
delete from dbo.TicketClassifications where tenant=@tenant
delete from dbo.UserLastLogins where tenant=@tenant
delete from dbo.VatTypePercentages where tenant=@tenant
--LeadSources
--MoveTypes
--QuoteStages
--EntityLastActivities
--Measurements
--QuoteTemplateTextDesigns
--OpportunityClosingReasons
--OpportunityTypes
--CounterDefinitions
--TicketSeverities
--Stages
--TenantSettings
--TicketStages
--DocumentTypeCustomFields1
--CustomerSizes
--ExternalSystemsMissingTranslations
--CreditCardTypes
--TicketTypes
--BusinessUnits
--ChargesTypes
--DocumentTypeTemplates
--QuoteTemplateTableDesigns
--ExternalSystemsTablesCodes
--Ranks
--customs.VendorCommunications
--customs.ConsignmentPackages
--Reports
--Counters
--ImageDetails
End');


-- Procedure Script From usp_CopyDocTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_CopyDocTypes]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_CopyDocTypes] END');
EXEC('-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 13/01/2016
-- Description:	Copy Package Type From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyDocTypes]
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @DocTypeId as varchar(15)
declare @DocumentTypeDefaultHTMLTemplateId as varchar(15)
declare @DocumentTypeDefaultReportTemplateId as varchar(15)
declare @DocTypeCode as varchar(15)
declare @DocumentTypeTemplatesHtmlId as varchar(15)
declare @DocumentTypeTemplatesReportId as varchar(15)
declare @NewDocTypeId as varchar(15)
declare @DocTypeOTId as varchar(15)
declare @DocTypeCopyId as varchar(15)
declare @NewDocTypeCopyId as varchar(15)
declare @DocTypeCustomFieldId as varchar(15)
declare @NewDocTypeCustomFieldId as varchar(15)
BEGIN
print ''1111''
DECLARE eventsCursor CURSOR READ_ONLY
FOR
SELECT Id,Code,ObjectTableId,DocumentTypeDefaultHTMLTemplateId,DocumentTypeDefaultReportTemplateId
FROM [dbo].[DocumentTypes] where Tenant = @SourceTenant
OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @DocTypeId,@DocTypeCode,@DocTypeOTId,@DocumentTypeDefaultHTMLTemplateId,@DocumentTypeDefaultReportTemplateId
WHILE @@FETCH_STATUS = 0
BEGIN
IF NOT EXISTS ( SELECT [Code] FROM [dbo].[DocumentTypes] where [Code] = @DocTypeCode and [Tenant] = @DestanationTenant)
begin
print ''1111''
set @NewDocTypeId = null
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @NewDocTypeId OUTPUT,
@pTableName = N''DocumentType''
print ''@NewDocTypeId: '' + @NewDocTypeId
declare @ObjectTableName as varchar(100)
declare @ObjectTableId as varchar(15)
SELECT @ObjectTableName = [Name]
FROM [dbo].[ObjectTables] where Id = @DocTypeOTId and Tenant = @SourceTenant
SELECT @ObjectTableId = [Id]
FROM [dbo].[ObjectTables] where Name = @ObjectTableName and Tenant = @DestanationTenant
INSERT INTO [dbo].[DocumentTypes]
([Id],[Tenant],[Name],[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],[ObjectTableId],[Subject],[DocumentTypeDefaultReportTemplateId]
,[DocumentTypeDefaultHTMLTemplateId],[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode])
( select @NewDocTypeId,@DestanationTenant,[Name],[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],@ObjectTableId,[Subject],null
,null,[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode]
from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
----------------------------------------------------------------------------
if @DocumentTypeDefaultHTMLTemplateId is not null
begin
print ''2''
set @DocumentTypeTemplatesHtmlId = null
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @DocumentTypeTemplatesHtmlId OUTPUT,
@pTableName = N''DocumentTypeTemplate''
INSERT INTO [dbo].[DocumentTypeTemplates]
([Id],[Tenant],[DocumentTypeId],[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId] ,[InActive]
,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
,[Language],[OriginalTemplateId])
( select @DocumentTypeTemplatesHtmlId,@DestanationTenant,@NewDocTypeId,[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId],[InActive]
,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
,[Language],[OriginalTemplateId]
from [dbo].[DocumentTypeTemplates] where Id = @DocumentTypeDefaultHTMLTemplateId and Tenant = @SourceTenant)
end
------------------------------------------------------------------------------
------------------------------------------------------------------------------
if @DocumentTypeDefaultReportTemplateId is not null
begin
set @DocumentTypeTemplatesReportId = null
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @DocumentTypeTemplatesReportId OUTPUT,
@pTableName = N''DocumentTypeTemplate''
print ''@DocumentTypeTemplatesReportId: '' + @DocumentTypeTemplatesReportId
INSERT INTO [dbo].[DocumentTypeTemplates]
([Id],[Tenant],[DocumentTypeId],[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId] ,[InActive]
,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
,[Language],[OriginalTemplateId])
( select @DocumentTypeTemplatesReportId,@DestanationTenant,@NewDocTypeId,[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId],[InActive]
,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
,[Language],[OriginalTemplateId]
from [dbo].[DocumentTypeTemplates] where Id = @DocumentTypeDefaultReportTemplateId and Tenant = @SourceTenant)
end
------------------------------------------------------------------------------
update [dbo].[DocumentTypes] set [DocumentTypeDefaultHTMLTemplateId] = @DocumentTypeTemplatesHtmlId,[DocumentTypeDefaultReportTemplateId] = @DocumentTypeTemplatesReportId
------------------------------------------------------------------------------
DECLARE eventsCursor1 CURSOR READ_ONLY LOCAL
FOR
SELECT Id
FROM [dbo].[DocumentTypeCopies] where DocumentTypeId = @DocTypeId and Tenant = @SourceTenant
OPEN eventsCursor1 FETCH NEXT FROM eventsCursor1 INTO @DocTypeCopyId
WHILE @@FETCH_STATUS = 0
BEGIN
set @NewDocTypeCopyId = null
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @NewDocTypeCopyId OUTPUT,
@pTableName = N''DocumentTypeCopy''
print ''@NewDocTypeCopyId : '' + @NewDocTypeCopyId
INSERT INTO [dbo].[DocumentTypeCopies]
([Id],[Tenant],[Code],[Name],[DocumentTypeId],[IndexOrder],[IsSelectedByDefault],[InActive])
(select @NewDocTypeCopyId,@DestanationTenant,[Code],[Name],[DocumentTypeId],[IndexOrder],[IsSelectedByDefault],[InActive]
from [dbo].[DocumentTypeCopies] where Id = @DocTypeCopyId and Tenant = @SourceTenant)
FETCH NEXT FROM eventsCursor1 INTO @DocTypeCopyId
END
CLOSE eventsCursor1;
DEALLOCATE eventsCursor1;
-----------------------------------------------------------------------------
DECLARE eventsCursor2 CURSOR READ_ONLY LOCAL
FOR
SELECT Id
FROM [dbo].[DocumentTypeCustomFields1] where DocumentTypeId = @DocTypeId and Tenant = @SourceTenant
OPEN eventsCursor2 FETCH NEXT FROM eventsCursor2 INTO @DocTypeCustomFieldId
WHILE @@FETCH_STATUS = 0
BEGIN
print ''6''
set @NewDocTypeCustomFieldId = null
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @NewDocTypeCustomFieldId OUTPUT,
@pTableName = N''DocumentTypeCustomField''
INSERT INTO [dbo].[DocumentTypeCustomFields1]
([Id],[Tenant],[DocumentTypeId],[FieldCode],[FieldDataTypeCode],[InActive],[IsRequired],[DefaultValue],[MultiLine],[Name],[IndexOrder])
(select @NewDocTypeCustomFieldId,@DestanationTenant,[DocumentTypeId],[FieldCode],[FieldDataTypeCode],[InActive],[IsRequired],[DefaultValue],[MultiLine],[Name],[IndexOrder]
from [dbo].[DocumentTypeCustomFields1] where Id = @DocTypeCustomFieldId and Tenant = @SourceTenant)
FETCH NEXT FROM eventsCursor2 INTO @DocTypeCustomFieldId
END
CLOSE eventsCursor2;
DEALLOCATE eventsCursor2;
end -- First If
else
begin
print ''Doc ID'' + @DocTypeId
declare @xx as varchar(200)
set @xx = (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
print @xx
--print (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
UPDATE [dbo].[DocumentTypes] SET
[Name] = (select [Name] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[Code] = (select [Code] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[Notes]= (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsAir] = (select [IsAir] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsOcean] = (select [IsOcean] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsInland] = (select [IsInland] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsDocIn] = (select [IsDocIn] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsDocOut] = (select [IsDocOut] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[ObjectTableId] = (select [ObjectTableId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[Subject] = (select [Subject] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
--,[DocumentTypeDefaultReportTemplateId] = (select [DocumentTypeDefaultReportTemplateId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
--,[DocumentTypeDefaultHTMLTemplateId] = (select [DocumentTypeDefaultHTMLTemplateId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[TemplateFormatCode] = (select [TemplateFormatCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[DocumentTypeDefaultEditorTool] = (select [DocumentTypeDefaultEditorTool] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[InActive] = (select [InActive] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsMaster] = (select [IsMaster] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsHouse] = (select [IsHouse] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsDirect] = (select [IsDirect] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[SearchFields] = (select [SearchFields] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[CustomControl] = (select [CustomControl] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
--,[CustomerRoleId] = (select [CustomerRoleId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
--,[AgentRoleId] = (select [AgentRoleId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsCustomerView] = (select [IsCustomerView] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsAgentView] = (select [IsAgentView] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsReadOnly] = (select [IsReadOnly] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsDocumentOneTimePrintLimited] = (select [IsDocumentOneTimePrintLimited] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[LimitedPrintCopyId] = (select [LimitedPrintCopyId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsEnabledForCustomers] = (select [IsEnabledForCustomers] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[IsCopiedAtSignup] = (select [IsCopiedAtSignup] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[CountryCode] = (select [CountryCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[DocumentsDataProviderCode] = (select [DocumentsDataProviderCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
,[DocumentTypeCategoryCode] = (select [DocumentTypeCategoryCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
where Code = @DocTypeCode and Tenant = @DestanationTenant
--(select [Name] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant),[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],@ObjectTableId,[Subject],null
--,null,[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
--,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
-- ,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode]
--  from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant
end -- else
FETCH NEXT FROM eventsCursor INTO @DocTypeId,@DocTypeCode,@DocTypeOTId,@DocumentTypeDefaultHTMLTemplateId,@DocumentTypeDefaultReportTemplateId
END
CLOSE eventsCursor
DEALLOCATE eventsCursor
END
END
-- ======================================================================================
SET ANSI_NULLS ON');


-- Procedure Script From usp_CopyDocumentsMetaDataTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_CopyDocumentsMetaDataTypes]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_CopyDocumentsMetaDataTypes] END');
EXEC('-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 14/02/2016
-- Description:	Copy DocMetaDataTypes From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyDocumentsMetaDataTypes]
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @Id as varchar(15)
declare @Code as varchar(15)
declare @MeasurementId as varchar(15)
declare @SourceCode as varchar(15)
declare @DestId as varchar(15)
BEGIN
DECLARE eventsCursor CURSOR READ_ONLY
FOR
SELECT Id,Code
FROM dbo.DocumentsMetaDataTypes where Tenant = @SourceTenant
OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Code
WHILE @@FETCH_STATUS = 0
BEGIN
set @Code = null
SELECT @Code = Code
FROM [dbo].[DocumentsMetaDataTypes] where [Code] = @Code and Tenant = @DestanationTenant
print @Code
IF @Code = '''' or @Code is null
begin
print ''innnnnn''
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @DestId OUTPUT,
@pTableName = N''DocumentsMetaDataType''
print ''@DestId : '' + @DestId
INSERT INTO [dbo].[DocumentsMetaDataTypes]([Id],[Tenant],[Code],[EnglishName],[LocalName],[InActive],[CustomsMetaDataCode],[Format])
( select @DestId,@DestanationTenant,[Code],[EnglishName],[LocalName],[InActive],[CustomsMetaDataCode],[Format]
from [dbo].[DocumentsMetaDataTypes] where Id = @Id and Tenant = @SourceTenant)
end
FETCH NEXT FROM eventsCursor INTO @Id,@Code
END
CLOSE eventsCursor
DEALLOCATE eventsCursor
END
END
-- =================================================================================
SET ANSI_NULLS ON');


-- Procedure Script From usp_CopyDocumentTypeMetaDatas.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_CopyDocumentTypeMetaDatas]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_CopyDocumentTypeMetaDatas] END');
EXEC('Create PROCEDURE dbo.usp_CopyDocumentTypeMetaDatas
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @Id as varchar(15)
declare @Code as varchar(15)
declare @DocTypeCode as varchar(15)
declare @DocumentsMetaDataTypeId as varchar(15)
declare @DocumentTypeId as varchar(15)
declare @DestDocumentTypeId as varchar(15)
declare @SourceCode as varchar(15)
declare @DestDocumentsMetaDataTypeId as varchar(15)
declare @DestId as varchar(15)
BEGIN
DECLARE eventsCursor CURSOR READ_ONLY
FOR
SELECT Id,DocumentsMetaDataTypeId,DocumentTypeId
FROM dbo.DocumentTypeMetaDatas where Tenant = @SourceTenant
OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@DocumentsMetaDataTypeId,@DocumentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @Code = null
SELECT @Code = Code
FROM [dbo].[DocumentsMetaDataTypes] where [Id] = @DocumentsMetaDataTypeId and Tenant = @SourceTenant
print ''@DocumentsMetaDataTypeId: '' + @DocumentsMetaDataTypeId
print ''@Code: '' + @Code
set @DestDocumentsMetaDataTypeId = null
SELECT @DestDocumentsMetaDataTypeId = Id
FROM [dbo].[DocumentsMetaDataTypes] where [Code] = @Code and Tenant = @DestanationTenant
print ''@@DestDocumentsMetaDataTypeId: '' + @DestDocumentsMetaDataTypeId
set @DocTypeCode = null
SELECT @DocTypeCode = Code
FROM [dbo].DocumentTypes where [Id] = @DocumentTypeId and Tenant = @SourceTenant
print ''@DocTypeCode: '' + @DocTypeCode
set @DestDocumentTypeId = null
SELECT @DestDocumentTypeId = Id
FROM [dbo].DocumentTypes where [Code] = @DocTypeCode and Tenant = @DestanationTenant
print ''@@DestDocumentTypeId: '' + @DestDocumentTypeId
print ''innnnnn''
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @DestId OUTPUT,
@pTableName = N''DocumentTypeMetaData''
print ''@DestId : '' + @DestId
INSERT INTO [dbo].DocumentTypeMetaDatas([Id],DocumentTypeId,DocumentsMetaDataTypeId,Tenant,Mandatory)
( select @DestId,@DestDocumentTypeId,@DestDocumentsMetaDataTypeId,@DestanationTenant,Mandatory
from [dbo].DocumentTypeMetaDatas where Id = @Id and Tenant = @SourceTenant)
FETCH NEXT FROM eventsCursor INTO @Id,@DocumentsMetaDataTypeId,@DocumentTypeId
END
CLOSE eventsCursor
DEALLOCATE eventsCursor
END
END');


-- Procedure Script From usp_CopyPackageTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_CopyPackageTypes]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_CopyPackageTypes] END');
EXEC('-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 27/12/2015
-- Description:	Copy Package Type From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyPackageTypes]
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @Id as varchar(15)
declare @MeasurementId as varchar(15)
declare @SourceCode as varchar(15)
declare @DestId as varchar(15)
BEGIN
DECLARE eventsCursor CURSOR READ_ONLY
FOR
SELECT Id,MeasurementId
FROM [dbo].[PackageTypes] where Tenant = @SourceTenant
OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@MeasurementId
WHILE @@FETCH_STATUS = 0
BEGIN
set @SourceCode = null
SELECT @SourceCode = [Code]
FROM [dbo].[Measurements] where Id = @MeasurementId
set @DestId = null
SELECT @DestId = Id
FROM [dbo].[Measurements] where [Code] = @SourceCode and Tenant = @DestanationTenant
IF @DestId = '''' or @DestId is null and @SourceCode is not null
begin
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @DestId OUTPUT,
@pTableName = N''Measurement''
INSERT INTO [dbo].[Measurements]([Code] ,[Name],[ShortName],[Id],[Tenant],[IsContainerMeasurement],[IsContainer],[InActive],[SearchFields],[LocalName])
( select [Code],[Name],[ShortName],@DestId,@DestanationTenant,[IsContainerMeasurement],[IsContainer],[InActive],[SearchFields],[LocalName]
from [dbo].[Measurements] where Id = @MeasurementId and Tenant = @SourceTenant)
end
IF @SourceCode is null
begin
set @DestId = null
end
declare @PackageTypeId as varchar(15)
declare @DestTypeCode as varchar(100)
declare @DestTypeId as varchar(15)
SELECT @DestTypeCode = [Code]
FROM [dbo].[PackageTypes] where Id = @Id
SELECT @DestTypeId = Id
FROM [dbo].[PackageTypes] where [Code] = @DestTypeCode and Tenant = @DestanationTenant
IF @DestTypeId = '''' or @DestTypeId is null
begin
EXEC   [dbo].[usp_GetNextTableIdValue]
@pLastNumber = @PackageTypeId OUTPUT,
@pTableName = N''PackageType''
INSERT INTO [dbo].[PackageTypes]
([Id],[Tenant],[Code],[EnglishName],[LocalName],[IsAir],[IsOcean],[IsInland],[AddedManually],[Notes],[IsContainer],[TEU],[ContainerSize],[Volume],[InActive]
,[MeasurementId],[SearchFields],[PrintAs])
(SELECT @PackageTypeId,@DestanationTenant,Code,EnglishName,LocalName,IsAir,IsOcean,IsInland,AddedManually,Notes,IsContainer,TEU,ContainerSize,Volume,InActive
,@DestId,SearchFields,PrintAs
from [PackageTypes] where Id = @Id and Tenant = @SourceTenant)
end
FETCH NEXT FROM eventsCursor INTO @Id,@MeasurementId
END
CLOSE eventsCursor
DEALLOCATE eventsCursor
END
END');


-- Procedure Script From usp_DeleteBusinessRecords.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteBusinessRecords]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteBusinessRecords] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteBusinessRecords
(
@Tenant int
)
AS
BEGIN
update Activities set QuoteId = NULL where Tenant = @Tenant
update Shipments set MasterShipmentDataId = NULL where Tenant = @Tenant
delete from FollowUps where Tenant = @Tenant
delete from QuotePriceSteps where Tenant = @Tenant
delete from QuoteCharges where Tenant = @Tenant
delete from QuoteDocumentVersions where Tenant = @Tenant
delete from QuotePackages where Tenant = @Tenant
delete from QuoteTotalVATs where Tenant = @Tenant
delete from Quotes where Tenant = @Tenant
delete from ShipmentCustomsTransmissions where Tenant = @Tenant
delete from ShipmentReceivables where Tenant= @Tenant
delete from ShipmentPayables where Tenant= @Tenant
delete from InsideShipmentPackages where Tenant = @Tenant
delete from ShipmentPackageHarmonize where Tenant = @Tenant
delete from ShipmentPackageItems where Tenant = @Tenant
delete from ShipmentPackages where Tenant= @Tenant
delete from ShipmentOrderPackages where Tenant= @Tenant
delete from PickUpDeliveryPackageHarmonizes where Tenant = @Tenant
delete from ShipmentPickUpDeliveryPackages where Tenant= @Tenant
delete from ShipmentPickUpDeliveries where Tenant= @Tenant
delete from ShipmentAWBPrintOnlies where Tenant= @Tenant
delete from ShipmentCarrierStatuses where Tenant= @Tenant
delete from AWBOCIs where Tenant= @Tenant
delete from ShipmentCommodities where Tenant= @Tenant
delete from ShipmentAssemblies where Tenant= @Tenant
delete from ShipmentReceivables where Tenant= @Tenant
delete from MessagingStockUsageHistories where Tenant = @Tenant
delete from ShipmentMasterDatas where Tenant = @Tenant
delete from ShipmentComputedFields where Tenant = @Tenant
delete from ShipmentAdditionalCloudDatas where Tenant = @Tenant
delete from WarehouseEntryPackagesReleases where Tenant = @Tenant
delete from WarehouseEntryPackages where Tenant = @Tenant
delete from WarehouseReleasePackages where Tenant = @Tenant
delete from WarehouseEntries where Tenant = @Tenant
delete from WarehouseReleases where Tenant = @Tenant
delete from Shipments where Tenant = @Tenant
delete from ARInvoiceLines where Tenant = @Tenant
delete from ARInvoiceEntities where Tenant = @Tenant
delete from ARInvoicePayments where Tenant = @Tenant
delete from ARInvoiceTotalVATs where Tenant = @Tenant
delete from ARInvoices where Tenant = @Tenant
delete from ARPayments where Tenant = @Tenant
delete from APInvoiceLines where Tenant = @Tenant
delete from APInvoiceEntities where Tenant = @Tenant
delete from APInvoicePayments where Tenant = @Tenant
delete from APInvoiceTotalVATs where Tenant = @Tenant
delete from APInvoices where Tenant = @Tenant
delete from APPayments where Tenant = @Tenant
END');


-- Procedure Script From usp_DeleteContactsUnseenEntitiesByContactId_Main.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteContactsUnseenEntitiesByContactId]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteContactsUnseenEntitiesByContactId] END');
EXEC('Create PROCEDURE [dbo].[usp_DeleteContactsUnseenEntitiesByContactId]
(
@pContactId   varchar(15),
@pObjectTableId   varchar(15),
@pTenant    int
)
as
BEGIN;
Delete  From ContactsUnseenEntities where [ContactId] =  @pContactId  and [Tenant] =  @pTenant and  [ObjectTableId] =  @pObjectTableId;
End;');


-- Procedure Script From usp_DeleteCustomerRecords.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteCustomerRecords]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteCustomerRecords] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteCustomerRecords
(
@Tenant int
)
AS
BEGIN
declare @DeletedContactIdTable table
(
Id varchar(15) not null
)
update Tickets set CompanyId = NULL where Tenant = @Tenant
update MAWBStacks set AssignedToId = null where Tenant = @Tenant
update Cards set PrimaryContactId = NULL where Tenant = @Tenant and (PartnerTypeId = ''CS'' or PartnerTypeId = ''PO'')
insert into @DeletedContactIdTable SELECT ContactId From CardContacts
Where Tenant = @Tenant  and CardId in (select Id from Cards where Tenant = @Tenant and (PartnerTypeId = ''CS'' or PartnerTypeId = ''PO''))
delete from CardContacts where Tenant = @Tenant and CardId in (select Id from Cards where Tenant = @Tenant and (PartnerTypeId = ''CS'' or PartnerTypeId = ''PO''))
declare @ContactId as varchar(15)
BEGIN
DECLARE DeleteContactCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM @DeletedContactIdTable
OPEN DeleteContactCursor FETCH NEXT FROM DeleteContactCursor INTO @ContactId
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select * from CardContacts where ContactId = @ContactId)
BEGIN
delete from ContactTenants where ContactId = @ContactId
delete from Contacts where Id = @ContactId
END
FETCH NEXT FROM DeleteContactCursor INTO @ContactId
END
CLOSE DeleteContactCursor
DEALLOCATE DeleteContactCursor
END
delete from Addresses where Tenant = @Tenant and CardId in (select Id from Cards where Tenant = @Tenant and (PartnerTypeId = ''CS'' or PartnerTypeId = ''PO''))
delete from CustomerProductLocations where Tenant = @Tenant
delete from CustomerProducts where Tenant = @Tenant
delete from CustomerProductLocationActualDatas where Tenant = @Tenant
delete from CustomerProductActualDatas where Tenant = @Tenant
delete from CustomerCompetitorProducts where Tenant = @Tenant
delete from CustomerCompetitors where Tenant = @Tenant
delete from CustomerAdditionalServices where Tenant = @Tenant
delete from CustomerSalesNote where Tenant = @Tenant
delete from CustomerSalesmanByProducts where Tenant = @Tenant
delete from CustomerAccountManagerByProducts where Tenant = @Tenant
delete from CustomerCustomsAgentByProducts where Tenant = @Tenant
delete from CustomerForwarderByProducts where Tenant = @Tenant
delete from CustomerMediatorByProducts where Tenant = @Tenant
delete from CardExternalCodeByCurrencies where Tenant = @Tenant
delete from Customers where Tenant = @Tenant
delete from Cards where Tenant = @Tenant and (PartnerTypeId = ''CS'' or PartnerTypeId = ''PO'')
END');


-- Procedure Script From usp_DeleteQBOTranslations.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteQBOTranslations]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteQBOTranslations] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteQBOTranslations
(
@Tenant int
)
AS
BEGIN
update ChargesTypes set ReceivablesChargesTypeExternalCode = NULL,PayablesChargesTypeExternalCode = NULL where Tenant = @Tenant
update vattypes set ExternalVATCard = NULL where Tenant = @Tenant
update Currencies set AccountingExternalCode = NULL where Tenant = @Tenant
update PaymentTerms set ExternalId = NULL where Tenant = @Tenant
update AccountingPaymentMethods set ARExternalId = NULL,APExternalId=NULL where Tenant = @Tenant
update CardExternalCodeByCurrencies set ExternalPayableTableId = NULL,ExternalRecievableTableId=NULL where Tenant = @Tenant
END');


-- Procedure Script From usp_DocumentOutsConversion.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DocumentOutsConversion]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DocumentOutsConversion] END');
EXEC('--Create PROCEDURE dbo.usp_DocumentOutsConversion
--(
--    @keyName as varchar(500) output
--)
--AS
--BEGIN;
--declare @Id as varchar(15)
--declare @Tenant as int
--declare @ChildEntityId as varchar(15)
--declare @ChildEntityReference as varchar(40)
--declare @EntityId as varchar(15)
--declare @IssuedByUserId as varchar(15)
--declare @IssuedDate as datetime
--declare @DocumentTypeId as varchar(15)
--declare @Notes as varchar(250)
--declare @ObjectTableId as varchar(15)
--declare @Issued as bit
--	DECLARE documentCursor CURSOR READ_ONLY
--	FOR
--	SELECT Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,DocumentTypeId,Note,ObjectTableId
--	From DocumentOuts
--	OPEN documentCursor FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentTypeId,@Notes,@ObjectTableId
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--	declare @codeCounter varchar(15)
--	EXECUTE usp_GetNextTableCodeValue @codeCounter OUTPUT,''DocumentsFiling'',@Tenant
--   declare @email as varchar(60)
--   declare @tenantString as varchar(50)
--   set @tenantString=CONVERT(varchar(50), @Tenant)
--   set @email=''system@tenant''+@tenantString+''.com''
--  declare @createdDate as datetime
--  set @createdDate =GETDATE()
--  declare @createdBy as varchar(15)
--  set @createdBy=(select id from Contacts where Email=@email and Tenant=@Tenant)
--   declare @existedId as varchar(15)
--   set @existedId=NULL
--   set @existedId =(select id from DocumentsFilings where Id=@Id)
--   if(@existedId is null)
--   begin
--   insert into DocumentsFilings (Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,UpdatedByUserId,UpdateDate,DocumentTypeId,Notes,ObjectTableId,CreateDate,CreatedByUserId,DirectionCode,OwnerId,HasCopies,Code)
--   values (@Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@createdBy,@createdDate,@DocumentTypeId,@Notes,@ObjectTableId,@createdDate,@createdBy,''O'',@createdBy,0,@codeCounter)
--   print @email
--   end
--	FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentTypeId,@Notes,@ObjectTableId
--	END
--	CLOSE documentCursor
--	DEALLOCATE documentCursor
--END
--set @keyName=''finished''');


-- Procedure Script From usp_DocumentsConversion.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DocumentsConversion]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DocumentsConversion] END');
EXEC('Create PROCEDURE dbo.usp_DocumentsConversion
(
@keyName as varchar(500) output
)
AS
BEGIN;
declare @Id as varchar(15)
declare @Tenant as int
declare @ChildEntityId as varchar(15)
declare @ChildEntityReference as varchar(40)
declare @EntityId as varchar(15)
declare @DocumentId as varchar(15)
declare @Received as bit
declare @ReceivedByUserId as varchar(15)
declare @ReceivedDate as datetime
declare @DocumentTypeId as varchar(15)
declare @Notes as varchar(250)
declare @ObjectTableId as varchar(15)
declare @ExternalId as varchar(15)
DECLARE documentCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,DocumentId,Received,ReceivedByUserId,ReceivedDate,DocumentTypeId,Notes,ObjectTableId,ExternalId
From DocumentIns
OPEN documentCursor FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentId,@Received,@ReceivedByUserId,@ReceivedDate,@DocumentTypeId,@Notes,@ObjectTableId,@ExternalId
WHILE @@FETCH_STATUS = 0
BEGIN
declare @codeCounter varchar(15)
set @codeCounter=@ExternalId
if(@ExternalId is null)
begin
EXECUTE usp_GetNextTableCodeValue @codeCounter OUTPUT,''DocumentsFiling'',@Tenant
end
declare @email as varchar(60)
declare @tenantString as varchar(50)
set @tenantString=CONVERT(varchar(50), @Tenant)
set @email=''system@tenant''+@tenantString+''.com''
declare @createdDate as datetime
set @createdDate =GETDATE()
declare @createdBy as varchar(15)
set @createdBy=(select id from Contacts where Email=@email and Tenant=@Tenant)
declare @existedId as varchar(15)
set @existedId=NULL
set @existedId =(select id from DocumentsFilings where Id=@Id)
if(@existedId is null)
begin
insert into DocumentsFilings (Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,DocumentId,Received,UpdatedByUserId,UpdateDate,DocumentTypeId,Notes,ObjectTableId,Code,CreateDate,CreatedByUserId,ReceivedByUserId,ReceivedDate,DirectionCode,OwnerId,HasCopies)
values (@Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentId,@Received,@createdBy,@createdDate,@DocumentTypeId,@Notes,@ObjectTableId,@codeCounter,@createdDate,@createdBy,@ReceivedByUserId,@ReceivedDate,''I'',@createdBy,0)
end
FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentId,@Received,@ReceivedByUserId,@ReceivedDate,@DocumentTypeId,@Notes,@ObjectTableId,@ExternalId
END
CLOSE documentCursor
DEALLOCATE documentCursor
END
set @keyName=''finished''');


-- Procedure Script From usp_UpdateAllTenantsCustomersActualData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateAllTenantsCustomersActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData]
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


-- Procedure Script From usp_UpdateCustomerActualData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCustomerActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCustomerActualData] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCustomerActualData]
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


-- Procedure Script From usp_UpdateCustomers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCustomers]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCustomers] END');
EXEC('Create PROCEDURE dbo.usp_UpdateCustomers
AS
DECLARE @CurrentCustomerId AS varchar(15)
DECLARE @CurrentCustomerTenant AS INT
DECLARE CustomersCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant
FROM Customers
OPEN CustomersCursor FETCH NEXT FROM CustomersCursor INTO @CurrentCustomerId,@CurrentCustomerTenant
WHILE @@FETCH_STATUS = 0
BEGIN
DECLARE @MaxDate as datetime
DECLARE @MinDate as datetime
set @MaxDate = (SELECT MAX(CreateDateTime) FROM Shipments WHERE CustomerId = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant)
set @MinDate = (SELECT MIN(CreateDateTime) FROM Shipments WHERE CustomerId = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant)
--print ''[Min Date:'' + Convert(varchar,ISNULL(@MinDate,0)) + '']''
--print ''[Max Date:'' + Convert(varchar,ISNULL(@MaxDate,0)) + '']''
Update Customers
set StartWorkingDate = @MinDate, LastShipmentDate = @MaxDate
Where Id = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant
FETCH NEXT FROM CustomersCursor INTO @CurrentCustomerId,@CurrentCustomerTenant
END
CLOSE CustomersCursor
DEALLOCATE CustomersCursor');


-- Procedure Script From usp_UpdateQueueCommunicationLog.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateQueueCommunicationLog]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateQueueCommunicationLog] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateQueueCommunicationLog]
(
@pCommunicationLogId  as  varchar(40),
@pTenant   as int,
@pCommunicationStatusTypeCode as varchar(4),
@pLog as  nvarchar(MAX),
@pExceptionMessage as  nvarchar(MAX),
@pMessageLockId  as  varchar(40)
)
AS
declare @LogString  as nvarchar(max);
declare @OldLogString  as nvarchar(max);
DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
IF  EXISTS (SELECT Id
FROM CommunicationLogs with (UPDLOCK) WHERE Id =@pCommunicationLogId and Tenant = @pTenant)
BEGIN;
set @OldLogString = (select Logs
FROM CommunicationLogs with (UPDLOCK) WHERE Id =@pCommunicationLogId and Tenant = @pTenant);
if(@OldLogString is not null)
begin
set @pLog = @OldLogString +@NewLineChar + @pLog
end
update CommunicationLogs
set MessageLockId = @pMessageLockId, CommunicationStatusTypeCode = @pCommunicationStatusTypeCode,Logs = @pLog,ExceptionMessage = @pExceptionMessage
WHERE Id = @pCommunicationLogId and Tenant = @pTenant
End;');


-- Procedure Script From usp_UpdateTenantCustomersActualData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateTenantCustomersActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData]
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


-- Procedure Script From DeleteDuplicatedQueueMessagesProcedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[RemoveDuplicatedQueueMessages]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[RemoveDuplicatedQueueMessages] END');
EXEC('CREATE procedure [dbo].[RemoveDuplicatedQueueMessages]
as begin
declare @HashCode as varchar(1000)
declare duplicatedQueuesCursor Cursor read_only
for
select hashcode
from QueueMessages WITH (UPDLOCK, READPAST,ROWLOCK)  inner join QueueDefinitions on QueueMessages.QueueDefinitionCode = QueueDefinitions.Code
WHERE Status = 0 and QueueDefinitions.DuplicateMessagesAutoRemove = 1 and HashCode is not null
group by HashCode
having count(*) > 1
open duplicatedQueuesCursor
fetch next from duplicatedQueuesCursor into @HashCode
while @@FETCH_STATUS = 0
begin
--print @HashCode
declare @TopQueueMessageId as varchar(1000)
set @TopQueueMessageId = (select top 1 id from QueueMessages where hashcode = @HashCode order by NextRunDateTime ASC)
--print ''top queue message id:'' + @TopQueueMessageId
update QueueMessages set Status = 2 where hashcode = @HashCode and Id <> @TopQueueMessageId
fetch next from duplicatedQueuesCursor into @HashCode
end
close duplicatedQueuesCursor
deallocate duplicatedQueuesCursor
end;');


-- Procedure Script From DeleteOldAPILogs.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldAPILogs]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldAPILogs] END');
EXEC('create procedure [dbo].[DeleteOldAPILogs]
as
begin
IF OBJECT_ID(''dbo.TempDeletedAPILogs'') IS NOT NULL
DROP TABLE TempDeletedAPILogs
SELECT * INTO TempDeletedAPILogs
FROM (SELECT top(1000) Id
FROM APILogs
WHERE CreateDate < GETDATE() - 90) AS t
DELETE FROM APILogsData WHERE Id IN (SELECT Id FROM TempDeletedAPILogs)
DELETE FROM APILogs WHERE Id IN (SELECT Id FROM TempDeletedAPILogs)
end');


-- Procedure Script From DeleteOldQueueMessageMoreDetailsTask.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldQueueMessageMoreDetailsTask]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldQueueMessageMoreDetailsTask] END');
EXEC('Create procedure [dbo].[DeleteOldQueueMessageMoreDetailsTask]
as
begin
delete top(1000) from [dbo].[QueueMessageMoreDetails] where [CreateDateTime] < GETDATE() - 90
end');


-- Procedure Script From DeleteTaskSchedulerHistories.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteTaskSchedulerHistories]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteTaskSchedulerHistories] END');
EXEC('Create procedure [dbo].[DeleteTaskSchedulerHistories]
(
@TaskId  as  varchar(15)
)
as
begin
delete from [dbo].[TaskSchedulerHistory] where [TaskId] = @TaskId and [StartDateTime] < DATEADD(month,-3,GETDATE())
end');


-- Procedure Script From GetIndexFragmentation.dxml
EXEC('IF (OBJECT_ID(''[dbo].[GetIndexFragmentation]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[GetIndexFragmentation] END');
EXEC('CREATE PROCEDURE [dbo].[GetIndexFragmentation]
@tableName nvarchar(max)
AS
SELECT name, avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats (
DB_ID(N''SimplogMain'')
, OBJECT_ID(@tableName)
, NULL
, NULL
, NULL) AS a
JOIN sys.indexes AS b
ON a.object_id = b.object_id AND a.index_id = b.index_id;');


-- Procedure Script From IdCounter GetIdRange Procedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetNextTableIdsRange]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetNextTableIdsRange] END');
EXEC('Create PROCEDURE [dbo].[usp_GetNextTableIdsRange]
(
@pEndNumber   int OUTPUT,
@pStartNumber int OutPUT,
@DBStringNumber varchar(50) OutPUT,
@pTableName    varchar(40),
@pNumberOfIds int
)
AS
DECLARE @COUNTERINIT AS INT
SET @COUNTERINIT = @pNumberOfIds
SET @pStartNumber = 1
Declare @Current As Int
Declare @DataBaseNumber As Int
--Declare @DBStringNumber As varchar(50)
Declare @StartStrNumber As varchar(50)
Declare @EndStrNumber As varchar(50)
SELECT TOP 1 @DataBaseNumber = DataBaseNumber FROM DataBaseProperties
Set @DBStringNumber = CONVERT(varchar(50) , @DataBaseNumber)
IF NOT EXISTS (SELECT TableName
FROM DBIdCounters (UPDLOCK) WHERE TableName =@pTableName)
BEGIN;
INSERT INTO DBIdCounters
(
TableName,
LastIdNumber
)
VALUES
(
@pTableName,
@COUNTERINIT
)
Set @Current = 1
End
Else
BEGIN;
Set @Current	= (SELECT  LastIdNumber
FROM DBIdCounters with (UPDLOCK) WHERE  TableName =@pTableName)
set @pEndNumber = @Current + @pNumberOfIds
set @pStartNumber =  @Current + 1
Update DBIdCounters
set LastIdNumber = LastIdNumber + @pNumberOfIds
Where TableName = @pTableName
End;');


-- Procedure Script From Queue_DelayMessage.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_DelayMessage]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_DelayMessage] END');
EXEC('CREATE procedure [dbo].[Queue_DelayMessage]
(
@MessageId bigint,
@DelaySeconds int
)
as begin
IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)
BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate())
WHERE [Id] = @MessageId
UPDATE [dbo].[QueueMessageMoreDetails]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate())
WHERE [Id] = @MessageId
END
end');


-- Procedure Script From Queue_DelayMessageandChangeStatusTozero.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_DelayMessageandChangeStatusTozero]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_DelayMessageandChangeStatusTozero] END');
EXEC('Create procedure [dbo].[Queue_DelayMessageandChangeStatusTozero]
(
@MessageId bigint,
@DelaySeconds int
)
as begin
IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)
BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate()),[Status] = 0
WHERE [Id] = @MessageId and [Status] <> 0
UPDATE [dbo].[QueueMessageMoreDetails]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate()),[Status] = 0
WHERE [Id] = @MessageId and [Status] <> 0
END
end');


-- Procedure Script From Queue_Enqueue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_Enqueue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_Enqueue] END');
EXEC('CREATE procedure [dbo].[Queue_Enqueue]
(
@QueueDefinitionCode varchar(255),
@MessageBody varchar(1000),
@Tenant int,
@DelaySeconds int,
@CustomerId varchar(15),
@BatchNumber varchar(15),
@NextRunDTime datetime = null,
@HashCode nvarchar(1000),
@WatingStatus int,
@MessageId bigint out
)
as begin
declare @currentdate  datetime
declare @nextRunDateTime  datetime
set @currentdate =getdate()
if @NextRunDTime is null
set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())
else
set @nextRunDateTime = @NextRunDTime
--set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())
insert into [dbo].[QueueMessages] ([CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[Tenant],[NextRunDateTime],RetryNumber,HashCode)
values(@currentdate,@QueueDefinitionCode,@WatingStatus,@MessageBody,@Tenant,@nextRunDateTime,0,@HashCode)
declare @CId as varchar(15);
declare @BNo as varchar(15);
if @CustomerId = ''''
set @CId = NULL
else
set @CId = @CustomerId
if @BatchNumber = ''''
set @BNo = NULL
else
set @BNo = @BatchNumber
Set @MessageId = (SELECT SCOPE_IDENTITY());
insert into [dbo].[QueueMessageMoreDetails] ([Id],[CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[Tenant],[NextRunDateTime],RetryNumber,Field1,Field2)
values((SELECT SCOPE_IDENTITY()),@currentdate,@QueueDefinitionCode,@WatingStatus,@MessageBody,@Tenant,@nextRunDateTime,0,@CId,@BNo)
end;');


-- Procedure Script From Queue_Peek.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_Peek]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_Peek] END');
EXEC('CREATE procedure [dbo].[Queue_Peek]
(@MessageId  bigint output,
@MessageBody  varchar(1000) output,
@RetryNumber int output,
@QueueDefinitionCode varchar(255),
@WatingStatus int)
as
begin
--DECLARE @NextId INTEGER
Declare @frequency INTEGER
set @frequency = 60
-- Find next available item available where the status is enabled
SET @MessageId = (SELECT TOP 1 [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK, READPAST,ROWLOCK) WHERE [NextRunDateTime] <= getdate() and [Status] = @WatingStatus and QueueDefinitionCode = @QueueDefinitionCode ORDER BY [NextRunDateTime] ASC)
--If found, flag it to prevent being picked up again
IF (@MessageId IS NOT NULL)
BEGIN
Set @MessageBody = (Select MessageBody from [dbo].[QueueMessages] where [Id] = @MessageId)
Set @RetryNumber = (Select RetryNumber from [dbo].[QueueMessages] where [Id] = @MessageId)
Set @MessageBody = (Select MessageBody from [dbo].[QueueMessages] where [Id] = @MessageId)
UPDATE [dbo].[QueueMessages]
SET [ProcessingDateTime] = getdate(),[NextRunDateTime] = dateadd(second,@frequency,getdate()),RetryNumber = (RetryNumber+1)
WHERE [Id] = @MessageId
UPDATE [dbo].[QueueMessageMoreDetails]
SET [ProcessingDateTime] = getdate(),[NextRunDateTime] = dateadd(second,@frequency,getdate()),RetryNumber = (RetryNumber+1)
WHERE [Id] = @MessageId
END
-- return queue data
--IF (@NextId IS NOT NULL)
--select [QueueID],[QueueDateTime],[Title],[Status],[TextData],[NextRunTime],[ProcessingTime]
--from [dbo].[TasksQueue]
--where [QueueID] = @NextId
end');


-- Procedure Script From Queue_ReturnMessage.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_ReturnMessage]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_ReturnMessage] END');
EXEC('CREATE procedure [dbo].[Queue_ReturnMessage]
(
@MessageId bigint
)
as begin
IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)
BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = getdate()
WHERE [Id] = @MessageId
UPDATE [dbo].[QueueMessageMoreDetails]
SET  [NextRunDateTime] = getdate()
WHERE [Id] = @MessageId
END
end');


-- Procedure Script From Queue_SetStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Queue_SetStatus]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[Queue_SetStatus] END');
EXEC('CREATE procedure [dbo].[Queue_SetStatus]
(
@MessageId bigint,
@Statud int
)
as begin
IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [Id] = @MessageId)
BEGIN
UPDATE [dbo].[QueueMessages]
SET  [Status] = @Statud,[CompleteDateTime] = getdate()
WHERE [Id] = @MessageId
UPDATE [dbo].[QueueMessageMoreDetails]
SET  [Status] = @Statud,[CompleteDateTime] = getdate()
WHERE [Id] = @MessageId
END
end');


-- Procedure Script From usp_DeleteObjectTableMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteObjectTableMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteObjectTableMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_DeleteObjectTableMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
--*--Delete--*--
--ObjectFields
delete from objectfields where tenant = 0 and ObjectTableId = @ObjectTableId
delete from querycolumns where tenant = 0 and  userid is null  and QueryCode in (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)
delete from AdvancedQueryFilters where tenant = 0  and userid is null and QueryCode in  (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)
delete from RuleConditionFields where tenant = 0   and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 )  and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRuleFields where tenant = 0  and systemlevel = 1 and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRules where tenant = 0  and systemlevel = 1 and Id in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
--Screens
delete from ScreenFields where tenant = 0  and ScreenCode in (select code from Screens where ObjectTableId = @ObjectTableId)
delete from screens where tenant = 0  and ObjectTableId = @ObjectTableId
--Queries
delete from Queries where tenant = 0  and userid is null and systemlevel = 1 and ObjectTableId = @ObjectTableId
--TextCodes
delete from MenuButtons where tenant = 0 and  MenuButtonGroupId in (select id from MenuButtonGroups where ObjectTableId = @ObjectTableId)
delete from ObjectTableTabs where tenant = 0  and ObjectTableId = @ObjectTableId
delete from textcodes where tenant = 0 and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null) and code not in (select ShortTextCodeCode from tips)  and ObjectTableId = @ObjectTableId
--Features
delete from Features where tenant = 0  and ObjectTableId = @ObjectTableId
-----------
End');


-- Procedure Script From usp_GetForeignKeyName_Main.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetForeignKeyName]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetForeignKeyName] END');
EXEC('Create PROCEDURE dbo.usp_GetForeignKeyName
(
@keyName as varchar(500) output,
@baseTableName as varchar(500) ,
@foreignTableName as varchar(500) ,
@foreignColumnName as varchar(500)
)
AS
set @keyName = (select g.ForeignKey from
(
SELECT
f.name AS ForeignKey,
OBJECT_NAME(f.parent_object_id) AS TableName,
COL_NAME(fc.parent_object_id,
fc.parent_column_id) AS ColumnName,
OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
COL_NAME(fc.referenced_object_id,
fc.referenced_column_id) AS ReferenceColumnName
FROM
sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
) as g
where g.TableName = @baseTableName
and g.ReferenceTableName = @foreignTableName
and g.ColumnName = @foreignColumnName
)');


-- Procedure Script From usp_GetNextTableCodeValue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetNextTableCodeValue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetNextTableCodeValue] END');
EXEC('--drop procedure [dbo].[usp_GetNextTableCodeValue]
create PROCEDURE [dbo].[usp_GetNextTableCodeValue]
(
@pLastNumber INT OUTPUT,
@pTableName    nvarchar(40),
@pTenant    INT
)
AS
DECLARE @COUNTER AS INT
SET @COUNTER = 1000
Declare @Current As Int
IF NOT EXISTS (SELECT TableName
FROM CounterLastNumbers with (UPDLOCK) WHERE TableName =@pTableName AND Tenant = @pTenant)
BEGIN;
IF(@pTableName = ''Tenant'')
Begin;
SET @COUNTER = 0
INSERT INTO CounterLastNumbers
(
TableName,
Tenant,
LastNumber
)
VALUES
(
@pTableName,
@pTenant,
@COUNTER
)
Set @Current = 0
End
Else
Begin
SET @COUNTER = 1000
IF(@pTableName = ''Agent'')
begin
SET @COUNTER = 10000
end
IF(@pTableName = ''CustomAgent'')
begin
SET @COUNTER = 20000
end
IF(@pTableName = ''ShippingAgent'')
begin
SET @COUNTER = 30000
end
IF(@pTableName = ''Customer'')
begin
SET @COUNTER = 70000
end
INSERT INTO CounterLastNumbers
(
TableName,
Tenant,
LastNumber
)
VALUES
(
@pTableName,
@pTenant,
@COUNTER
)
Set @Current = @COUNTER
End
End;
Else
BEGIN;
Set @Current	= (SELECT  LastNumber
FROM CounterLastNumbers with (UPDLOCK) WHERE TableName =@pTableName AND Tenant = @pTenant)
Set @Current = @Current + 1
Update CounterLastNumbers
set LastNumber = @Current
Where TableName = @pTableName and Tenant = @pTenant
End;
Set @pLastNumber = @Current');


-- Procedure Script From usp_GetNextTableIdValue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetNextTableIdValue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetNextTableIdValue] END');
EXEC('CREATE PROCEDURE [dbo].[usp_GetNextTableIdValue]
(
@pLastNumber varchar(100) OUTPUT,
@pTableName    varchar(40)
)
AS
DECLARE @COUNTER AS INT
SET @COUNTER = 1
Declare @Current As Int
Declare @DataBaseNumber As Int
Declare @DBStringNumber As varchar(50)
Declare @CurrentStrNumber As varchar(50)
SELECT TOP 1 @DataBaseNumber = DataBaseNumber FROM DataBaseProperties
Set @DBStringNumber = CONVERT(varchar(50) , @DataBaseNumber)
IF NOT EXISTS (SELECT TableName
FROM DBIdCounters with (UPDLOCK) WHERE TableName =@pTableName)
BEGIN;
SET @COUNTER = 1
INSERT INTO DBIdCounters
(
TableName,
LastIdNumber
)
VALUES
(
@pTableName,
@COUNTER
)
Set @Current = 1
End
Else
BEGIN;
Set @Current	= (SELECT  LastIdNumber
FROM DBIdCounters with (UPDLOCK) WHERE  TableName =@pTableName)
Set @Current = @Current + 1
Update DBIdCounters
set LastIdNumber = @Current
Where TableName = @pTableName
End;
Set  @CurrentStrNumber = CONVERT(varchar(50) ,@Current)
Set @pLastNumber = @DBStringNumber + ''-''+ @CurrentStrNumber');


-- Procedure Script From usp_GetNextTableNumberValue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetNextTableNumberValue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetNextTableNumberValue] END');
EXEC('CREATE PROCEDURE [dbo].[usp_GetNextTableNumberValue]
(
@pLastValue INT OUTPUT,
@pTenant    INT,
@pCounterId    nvarchar(40),
@pPrefix    nvarchar(15),
@pStartNumber    INT
)
AS
DECLARE @COUNTER AS INT
SET @COUNTER = @pStartNumber
Declare @Current As Int
IF @pPrefix IS NOT NULL
BEGIN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK) WHERE CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK) WHERE  CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant
End;
END
ELSE
BEGIN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK) WHERE CounterId = @pCounterId and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK) WHERE  CounterId = @pCounterId  and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId  and Tenant =  @pTenant
End;
END
Set @pLastValue = @Current');


-- Procedure Script From usp_GetPrimaryKeyName.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetPrimaryKeyName]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetPrimaryKeyName] END');
EXEC('Create PROCEDURE dbo.usp_GetPrimaryKeyName
(
@keyName as varchar(500) output,
@tableName as varchar(500) ,
@columnName as varchar(500)
)
AS
set @keyName = (select i.name
from sys.index_columns ic
join sys.indexes i on ic.index_id=i.index_id
join sys.columns c on c.column_id=ic.column_id
where
i.[object_id] = object_id(@tableName) and
ic.[object_id] = object_id(@tableName) and
c.[object_id] = object_id(@tableName) and
is_primary_key = 1 and c.name=@columnName)');


-- Procedure Script From usp_PreDeleteMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_PreDeleteMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_PreDeleteMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_PreDeleteMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
----*--Before Delete--*--
if object_id(''dbo.TempOldFeatures'') is not null
drop table TempOldFeatures
select * into TempOldFeatures
from (select Code,FeatureUniqeCode,IsOld,Tenant
from features
where IsOld = 1) as t
--select * from TempOldFeatures
if object_id(''dbo.TempIsSpellCheckedTextCodes'') is not null
drop table TempIsSpellCheckedTextCodes
select * into TempIsSpellCheckedTextCodes
from (select *
from TextCodes
where IsSpellChecked = 1) as t
--select * from TempIsSpellCheckedTextCodes
End');


-- Procedure Script From usp_ReconnectObjectTableMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ReconnectObjectTableMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
----MetaData All Scripts: Never Apply these scripts
--*--After Delete--*--
--ObjectFields
update querycolumns set ObjectFieldId = (select Id from objectfields where FieldCode=querycolumns.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update ScreenFields set ObjectFieldId = (select Id from objectfields where FieldCode=ScreenFields.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update AdvancedQueryFilters set ObjectFieldId = (select Id from objectfields where FieldCode=AdvancedQueryFilters.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update RuleConditionFields set ObjectFieldId = (select Id from objectfields where FieldCode=RuleConditionFields.ObjectFieldCode)
update ObjectTableRuleFields set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectTableRuleFields.ObjectFieldCode)
update ObjectTableRules set TriggerFieldId = (select Id from objectfields where FieldCode=ObjectTableRules.TriggerFieldCode)
update AirlineMessagingRules set RuleFieldId = (select Id from objectfields where FieldCode=AirlineMessagingRules.RuleFieldCode)
update restrictions  set ObjectFieldId = (select Id from objectfields where FieldCode=restrictions.ObjectFieldCode)
update CustomerFieldsUpdateSettings  set ObjectFieldId = (select Id from objectfields where FieldCode=CustomerFieldsUpdateSettings.ObjectFieldCode)
update ObjectFieldValidations  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldValidations.ObjectFieldCode)
update ObjectFieldModifications  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldModifications.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
--Screens
update ScreenModifications set ScreenId = (select Id from Screens where code=ScreenModifications.ScreenCode) where screencode in (select code from screens)
update ScreenFields set ScreenId = (select Id from Screens where code=ScreenFields.ScreenCode) where screencode in (select code from screens)
update ObjectTables set HeaderScreenId = (select Id from Screens where code=ObjectTables.HeaderScreenCode)
--Queries
update Queries set OriginalQueryId =  (select q1.Id from Queries q1 where q1.UniqueCode = Queries.OriginalQueryCode)
update AdvancedQueryFilters set QueryId = (select Id from Queries where UniqueCode = AdvancedQueryFilters.QueryCode) where QueryCode in (select UniqueCode from Queries)
update QueryColumns set QueryId = (select Id from Queries where UniqueCode = QueryColumns.QueryCode) where QueryCode in (select UniqueCode from Queries)
update SharedUserQueries set QueryId = (select Id from Queries where UniqueCode = SharedUserQueries.QueryCode)
--TextCodes
update Queries set NameTextCodeId = (select Id from TextCodes where Code=Queries.NameTextCodeCode and tenant = Queries.Tenant)
update ObjectTables set DescriptionTextCodeId = (select Id from TextCodes where Code=ObjectTables.DescriptionTextCodeCode and tenant = ObjectTables.Tenant)
update ObjectTables set NewButtonTextCodeId = (select Id from TextCodes where Code=ObjectTables.NewButtonTextCodeCode)
update Features set NameTextCodeId = (select Id from TextCodes where Code=Features.NameTextCodeCode and tenant = Features.Tenant)
update Tips set ShortTextCode = (select Id from TextCodes where Code=Tips.ShortTextCodeCode)
update ObjectTableTabs set TabNameTextCodeId = (select Id from TextCodes where Code=ObjectTableTabs.TabNameTextCodeCode and tenant = ObjectTableTabs.Tenant)
update MenuButtons set LabelTextCodeId = (select Id from TextCodes where Code=MenuButtons.LabelTextCodeCode)
update ObjectFields set FullNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.FullNameTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set HelpTextCodeId = (select Id from TextCodes where Code=ObjectFields.HelpTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ShortNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.ShortNameTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ListTextCodeId = (select Id from TextCodes where Code=ObjectFields.ListTextCodeCode and tenant = ObjectFields.Tenant)
update Translations set TextCodeId = (select Id from TextCodes where Code=Translations.TextCodeCode and (tenant =  Translations.Tenant or tenant = 0)) where TextCodeCode in (select code from textcodes)
--Features
update MenusTables set FeatureId = (select Id from features where FeatureUniqeCode=MenusTables.FeatureUniqeCode)
update Queries set FeatureId = (select Id from features where FeatureUniqeCode=Queries.FeatureUniqeCode)
update ObjectTableTabs set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableTabs.FeatureUniqeCode)
update ObjectTableHelperControls set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableHelperControls.FeatureUniqeCode)
update MenuButtons set FeatureId = (select Id from features where FeatureUniqeCode=MenuButtons.FeatureUniqeCode)
update Reports set FeatureId = (select Id from features where FeatureUniqeCode=Reports.FeatureUniqeCode)
update PackageFeatures set FeatureId = (select Id from features where FeatureUniqeCode=PackageFeatures.FeatureUniqeCode) where PackageFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)
update RoleFeatures set FeatureId = (select Id from features where FeatureUniqeCode=RoleFeatures.FeatureUniqeCode) where RoleFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)
-------------
UPDATE f
SET f.IsOld = temp.IsOld
FROM Features f
JOIN TempOldFeatures temp
ON f.FeatureUniqeCode = temp.FeatureUniqeCode
update tc
set tc.DefaultText = temp.DefaultText, tc.DefaultTextPlural = temp.DefaultTextPlural, tc.SpellCheckDate = temp.SpellCheckDate,
tc.SpellCheckedByUserId = temp.SpellCheckedByUserId,tc.LocalDefaultText = temp.LocalDefaultText,IsSpellChecked = temp.IsSpellChecked
from TextCodes tc
Join TempIsSpellCheckedTextCodes temp
on tc.Code = temp.Code
where tc.ObjectTableId = temp.ObjectTableId
End');


-- Procedure Script From usp_TestGetNextTableNumberValue.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_TestGetNextTableNumberValue]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_TestGetNextTableNumberValue] END');
EXEC('CREATE   PROCEDURE [dbo].[usp_TestGetNextTableNumberValue]
(
@pLastValue INT OUTPUT,
@pTenant    INT,
@pCounterId    nvarchar(40),
@pPrefix    nvarchar(15),
@pStartNumber    INT
)
AS
DECLARE @COUNTER AS INT
SET @COUNTER = @pStartNumber
Declare @Current As Int
IF @pPrefix IS NOT NULL
BEGIN
SET TRANSACTION ISOLATION LEVEL SNAPSHOT
-- BEGIN TRAN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK) WHERE CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK) WHERE  CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant
End;
END
ELSE
BEGIN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK) WHERE CounterId = @pCounterId and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK) WHERE  CounterId = @pCounterId  and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId  and Tenant =  @pTenant
End;
-- COMMIT TRAN
END
Set @pLastValue = @Current');


-- Procedure Script From usp_UpdateAPILog.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateAPILog]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateAPILog] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateAPILog]
(
@pLogId  as  varchar(15),
@pTenant   as int,
@pStatus as varchar(1),
@pRetriesNo as int,
@pUpdateDate as DateTime,
@pUpdateDateUTC as DateTime,
@pLog as  nvarchar(MAX),
@pRequestData as  nvarchar(MAX),
@pResponseData as  nvarchar(MAX),
@pExceptionMessage as  nvarchar(MAX),
@pLastExceptionMessage as  nvarchar(MAX)
)
AS
declare @LogString  as nvarchar(max);
declare @OldLogString  as nvarchar(max);
declare @RequestDataString  as nvarchar(max);
declare @OldRequestDataString  as nvarchar(max);
declare @ResponseDataString  as nvarchar(max);
declare @OldResponseDataString  as nvarchar(max);
declare @ExceptionDataString  as nvarchar(max);
declare @OldExceptionDataString  as nvarchar(max);
declare @RetriesNo  as int;
declare @DiagnosticLogLength  as int;
DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
--IF  EXISTS (SELECT Id
--    FROM APILogs with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant)
--    and EXISTS(SELECT Id
--    FROM APILogsData with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant)
--BEGIN
select
@OldLogString = DiagnosticLog,
@OldRequestDataString= RequestData,
@OldResponseDataString = ResponseData,
@OldExceptionDataString = ExceptionsMessage,
@DiagnosticLogLength = LEN(DiagnosticLog)
from APILogsData
where Id =@pLogId and Tenant = @pTenant
--set @OldLogString = (select DiagnosticLog
--FROM APILogsData with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant);
if(@OldLogString is not null)
begin
if(@pLog is not null)
begin
set @LogString = @OldLogString +@NewLineChar + convert(varchar(20),@pUpdateDate,103) + @NewLineChar +    isnull(@pLog,'''')
end
ELSE
begin
set @LogString = @OldLogString
end
end
else
begin
if(@pLog is not null)
begin
set @LogString = @pLog
end
if(LEN(@LogString) > 1000)
begin
set @LogString = right(@LogString,1000)
end
end
---------------------------------------------------------------
--set @OldRequestDataString = (select RequestData
--FROM APILogsData with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant);
if(@OldRequestDataString is not null)
begin
if(@pRequestData is not null)
begin
set @RequestDataString = @pRequestData
end
ELSE
begin
set @RequestDataString = @OldRequestDataString
end
end
else
begin
if(@pRequestData is not null)
begin
set @RequestDataString = @pRequestData
end
end
------------------------------------------------------------------
--set @OldResponseDataString = (select ResponseData
--FROM APILogsData with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant);
if(@OldResponseDataString is not null)
begin
if(@pResponseData is not null)
begin
set @ResponseDataString = @pResponseData
end
ELSE
begin
set @ResponseDataString = @OldResponseDataString
end
end
else
begin
if(@pResponseData is not null)
begin
set @ResponseDataString = @pResponseData
end
end
-----------------------------------------------------------------
--set @OldExceptionDataString = (select ExceptionsMessage
--FROM APILogsData with (UPDLOCK) WHERE Id =@pLogId and Tenant = @pTenant);
if(@OldExceptionDataString is not null)
begin
if(@pExceptionMessage is not null)
begin
set @ExceptionDataString = @pExceptionMessage
end
ELSE
begin
set @ExceptionDataString = @OldExceptionDataString
end
end
else
begin
if(@pExceptionMessage is not null)
begin
set @ExceptionDataString = @pExceptionMessage
end
end
-------------------------------------------------------------------
--set @RetriesNo = (select NumberOfRetries from APILogs WHERE Id = @pLogId and Tenant = @pTenant);
--if(@pStatus = ''F'')
-- begin
--set @RetriesNo = @RetriesNo + 1;
--end
update APILogs
set Status = @pStatus,
NumberOfRetries = @pRetriesNo,
LastUpdateDate = @pUpdateDate,
LastUpdateDateUTC = @pUpdateDateUTC,
LastExceptionMessage = @pLastExceptionMessage
WHERE Id = @pLogId and Tenant = @pTenant
update APILogsData
set DiagnosticLog = @LogString,
RequestData = @RequestDataString,
ResponseData = @ResponseDataString,
ExceptionsMessage = @ExceptionDataString
WHERE Id = @pLogId and Tenant = @pTenant
--End');


-- Procedure Script From usp_DeleteAccountingDataByTenant_Jan2018.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteAccountingDataByTenant_Jan2018]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteAccountingDataByTenant_Jan2018] END');
EXEC('create PROCEDURE [dbo].[usp_DeleteAccountingDataByTenant_Jan2018]
(
@tenant int
)
AS
BEGIN TRANSACTION
delete from arinvoiceentities
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoiceentities'', 16, 1)
RETURN
END
delete from arinvoicelines
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicelines'', 16, 1)
RETURN
END
delete from arinvoicetotalvats
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicetotalvats'', 16, 1)
RETURN
END
delete from arinvoicepayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicepayments'', 16, 1)
RETURN
END
delete from arpayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arpayments'', 16, 1)
RETURN
END
delete from arinvoices
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoices'', 16, 1)
RETURN
END
COMMIT
--exec usp_DeleteShipmentsDataByTenant 1');


-- Procedure Script From usp_DeleteAPinvoicesByTenant_Jan2018.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteAPinvoicesByTenant_Jan2018]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteAPinvoicesByTenant_Jan2018] END');
EXEC('create PROCEDURE [dbo].[usp_DeleteAPinvoicesByTenant_Jan2018]
(
@tenant int
)
AS
BEGIN TRANSACTION
delete from apinvoiceentities
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting apinvoiceentities'', 16, 1)
RETURN
END
delete from apinvoicelines
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting apinvoicelines'', 16, 1)
RETURN
END
delete from apinvoicetotalvats
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting apinvoicetotalvats'', 16, 1)
RETURN
END
delete from apinvoicepayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting apinvoicepayments'', 16, 1)
RETURN
END
delete from appayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting appayments'', 16, 1)
RETURN
END
delete from apinvoices
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting apinvoices'', 16, 1)
RETURN
END
COMMIT
--exec usp_DeleteShipmentsDataByTenant 1');


-- Procedure Script From usp_DeleteARinvoicesByTenant_Jan2018.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteARinvoicesByTenant_Jan2018]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteARinvoicesByTenant_Jan2018] END');
EXEC('create PROCEDURE [dbo].[usp_DeleteARinvoicesByTenant_Jan2018]
(
@tenant int
)
AS
BEGIN TRANSACTION
delete from arinvoiceentities
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoiceentities'', 16, 1)
RETURN
END
delete from arinvoicelines
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicelines'', 16, 1)
RETURN
END
delete from arinvoicetotalvats
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicetotalvats'', 16, 1)
RETURN
END
delete from arinvoicepayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoicepayments'', 16, 1)
RETURN
END
delete from arpayments
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arpayments'', 16, 1)
RETURN
END
delete from arinvoices
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting arinvoices'', 16, 1)
RETURN
END
COMMIT
--exec usp_DeleteShipmentsDataByTenant 1');


-- Procedure Script From usp_QuotesAutomaticallyClosingDailyJob.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_QuotesAutomaticallyClosingDailyJob]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_QuotesAutomaticallyClosingDailyJob] END');
EXEC('Create PROCEDURE [dbo].[usp_QuotesAutomaticallyClosingDailyJob]
AS
declare @Tenant integer
declare @QuoteId varchar(15)
declare @CloseDate DateTime
declare @StageMaxDays integer
declare @StageDueDate DateTime
declare @TodayDate DateTime
declare @NewGuidId varchar(40)
declare @ObjectTableId varchar(15)
declare @EventTypeId varchar(15)
declare @TraceEventNote nvarchar(4000)
declare @SystemUserId varchar(15)
set @TodayDate = CONVERT (DateTime, GETDATE())
set @ObjectTableId = (select Id from ObjectTables where Name = ''Quote'')
BEGIN
DECLARE QuotesCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, AutomaticallyCloseDate, StageDueDate
From Quotes
where IsAutomaticallyClosed = 1 AND AutomaticallyCloseDate is not null and IsClosed = 0
OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @CloseDate, @StageDueDate
WHILE @@FETCH_STATUS = 0
BEGIN
if (Year(@CloseDate) = Year(@TodayDate) AND Month(@CloseDate) = Month(@TodayDate) AND Day(@CloseDate) = Day(@TodayDate))
begin
set @StageMaxDays = (select MaxDays from QuoteStages where Tenant = @Tenant and Code = ''QTDC'')
if (@StageMaxDays is not null)
begin
set @StageDueDate = DATEADD(day,@StageMaxDays,@TodayDate)
end
update Quotes
set
IsClosed = 1,
QuoteClosingReasonCode = ''XQ'',
StageId = (select Id from QuoteStages where Tenant = @Tenant and Code = ''QTDC''),
StageDueDate = @StageDueDate
where Id = @QuoteId and Tenant = @Tenant
set @NewGuidId = CONVERT (uniqueidentifier, NEWID())
set @EventTypeId = (select Id from EventTypes where Code = ''QTDL'' and Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = ''Quote''))
set @TraceEventNote = (select EnglishName from EventTypes where Code = ''QTDL'' and Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = ''Quote''))
set @SystemUserId = (select Max(Id) from Contacts where Tenant = @Tenant AND UserType = ''S'')
insert into TraceEvents(Id, Tenant, EntityId, ObjectTableId, EventTypeId, EventDateTime, LogDateTime, UserId, Notes, Deleted, ExternalId, IsAddedManually)
values(@NewGuidId, @Tenant, @QuoteId, @ObjectTableId, @EventTypeId, @TodayDate, @TodayDate, @SystemUserId, @TraceEventNote, 0, null, 0)
end
FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @CloseDate, @StageDueDate
END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor
END');


-- Procedure Script From usp_ComputeForeignPartnerCountryCode.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeForeignPartnerCountryCode]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeForeignPartnerCountryCode] END');
EXEC('Create PROCEDURE [dbo].[usp_ComputeForeignPartnerCountryCode]
(
@ShipmentId varchar(15)
)
AS
declare @MasterShipmentDataId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @DirectionId as varchar(15)
declare @ShipperId as varchar(15)
declare @ConsigneeId as varchar(15)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @MasterFromPortId as varchar(15)
declare @MasterToPortId as varchar(15)
declare @CountryId as varchar(15)
declare @ForeignPartnerCountryCode as varchar(2)
declare @ShipperName as varchar(70)
declare @ConsigneeName as varchar(70)
select
@MasterShipmentDataId = MasterShipmentDataId,
@ShipmentLevelCode= ShipmentLevelCode,
@DirectionId = DirectionId,
@ConsigneeId = ConsigneeId,
@ShipperId = ShipperId,
@FromPortId =FromPortId,
@ToPortId = ToPortId,
@ConsigneeName = ConsigneeName,
@ShipperName =ShipperName
from Shipments
where Id = @ShipmentId
if(@DirectionId =''I'' or @DirectionId =''C'')
begin
select @ForeignPartnerCountryCode = CountryCode,@ShipperName= EnglishName from Cards where Id = @ShipperId
if(@ForeignPartnerCountryCode is null)
begin
if(@ShipmentLevelCode = ''H'' and @MasterShipmentDataId is null)
begin
set @CountryId = (select CountryId from Ports where Id = @FromPortId )
set @ForeignPartnerCountryCode = (select Code from Countries where Id = @CountryId )
end
else
begin
set @MasterFromPortId = (select MainCarriageFromPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId )
set @CountryId = (select CountryId from Ports where Id = @MasterFromPortId )
set @ForeignPartnerCountryCode = (select Code from Countries where Id = @CountryId )
end
End
end
else
Begin
select @ForeignPartnerCountryCode = CountryCode,@ConsigneeName= EnglishName from Cards where Id = @ConsigneeId
if(@ForeignPartnerCountryCode is null)
begin
if(@MasterShipmentDataId is not null)
begin
--    set @MasterFromPortId = (select FromPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId )
set @MasterToPortId = (select MainCarriageFinalDestinationPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId )
set @CountryId = (select CountryId from Ports where Id = @MasterToPortId )
set @ForeignPartnerCountryCode = (select Code from Countries where Id = @CountryId )
end
if(@ForeignPartnerCountryCode is null)
begin
set @CountryId = (select CountryId from Ports where Id = @ToPortId )
set @ForeignPartnerCountryCode = (select Code from Countries where Id = @CountryId )
end
end
End
update Shipments set ForeignPartnerCountryCode = @ForeignPartnerCountryCode ,ConsigneeName = @ConsigneeName  , ShipperName =@ShipperName where Id = @ShipmentId');


-- Procedure Script From usp_ComputeHouseShipmentStatusFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeHouseShipmentStatusFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeHouseShipmentStatusFunction] END');
EXEC('CREATE PROCEDURE [dbo].[usp_ComputeHouseShipmentStatusFunction]
(
@ShipmentId varchar(15)
)
AS
declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ComputedStatusId as varchar(15)
declare @ShipmentStatusId as varchar(35)
declare @ShipmentDeclarationNumber as varchar(35)
declare @ShipmentStatusWeight as int
declare @CustomsDeclarationNumber as varchar(35)
declare @ComputedStatusDate as datetime
declare @MasterStatusWeight as int
declare @MasterStatusId as varchar(15)
declare @Tenant as int
declare @CustomStatusWeight as int
declare @CustomStatusId as varchar(15)
declare @SearchFields as varchar(1000)
if (@ShipmentId is not null)
begin
select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentStatusId = StatusId,
@CustomFileId = CustomFileId,
@ComputedStatusDate = StatusDate,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = StatusId,
@SearchFields = SearchFields
from Shipments
where Id = @ShipmentId
set @ShipmentStatusWeight = (select StatusWeight from EntityStatus where Id = @ShipmentStatusId AND Tenant = @Tenant)
if(@MasterDataId is not null)
Begin
select @MasterStatusId = StatusId  from Shipments  where  Id = @MasterDataId AND Tenant = @Tenant
set @MasterStatusWeight = (select StatusWeight from EntityStatus where Id = @MasterStatusId AND Tenant = @Tenant)
if(@MasterStatusWeight > @ShipmentStatusWeight)
begin
set @ShipmentStatusWeight = @MasterStatusWeight
set @ComputedStatusId = @MasterStatusId
set @ComputedStatusDate = ( select StatusDate from Shipments where Id = @MasterStatusId AND Tenant = @Tenant)
end
End
if (@CustomFileId is not null)
Begin
select @CustomsDeclarationNumber = CustomsDeclarationNumber ,@CustomStatusId = StatusId from Shipments where Id = @CustomFileId AND Tenant = @Tenant
set @CustomStatusWeight = (select StatusWeight from EntityStatus where Id = @CustomStatusId AND Tenant = @Tenant)
if(@CustomStatusWeight > @ShipmentStatusWeight)
begin
set @ShipmentStatusWeight = @CustomStatusWeight
set @ComputedStatusId = @CustomStatusId
set @ComputedStatusDate = ( select StatusDate from Shipments where Id = @CustomFileId AND Tenant = @Tenant)
end
END
if(@CustomsDeclarationNumber is not null and (@ShipmentDeclarationNumber is null or   @ShipmentDeclarationNumber !=@CustomsDeclarationNumber   ))
begin
set @SearchFields = left((@SearchFields + '','' + @CustomsDeclarationNumber ),1000);
update Shipments set
ComputedStatusDate =@ComputedStatusDate ,
ComputedStatusId=  @ComputedStatusId ,
CustomsDeclarationNumber =@CustomsDeclarationNumber ,
SearchFields = @SearchFields
where Id = @ShipmentId and Tenant = @Tenant
end
else
begin
update Shipments set ComputedStatusDate =@ComputedStatusDate ,
ComputedStatusId=  @ComputedStatusId
where Id = @ShipmentId and Tenant = @Tenant
end
end');


-- Procedure Script From usp_ComputeShipmentFirstApprovalDate.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeShipmentFirstApprovalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeShipmentFirstApprovalDate] END');
EXEC('Create PROCEDURE [dbo].[usp_ComputeShipmentFirstApprovalDate]
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


-- Procedure Script From usp_ComputeShipmentStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeShipmentStatus]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeShipmentStatus] END');
EXEC('Create PROCEDURE [dbo].[usp_ComputeShipmentStatus]
(
@ShipmentId varchar(15)
)
AS
declare @Tenant as int
declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @ShipmentStatusId as varchar(15)
declare @ShipmentStatusWeight as int
declare @ShipmentDeclarationNumber as varchar(35)
declare @CustomDeclarationNumber as varchar(35)
declare @HouseId   as varchar(15)
declare @CustomId as varchar(15)
declare @IsConnect as bit
declare @ComputedStatusDate as datetime
declare @ComputedStatusId as varchar(15)
select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentLevelCode= ShipmentLevelCode,
@ShipmentStatusId = StatusId,
@CustomFileId = CustomFileId,
@ComputedStatusDate = StatusDate,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = StatusId
from Shipments
where Id = @ShipmentId
set @ShipmentStatusWeight = (select StatusWeight from EntityStatus where Id = @ShipmentStatusId AND Tenant = @Tenant)
set  @IsConnect = 0
if(@ShipmentLevelCode = ''C'')
BEGIN
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
From Shipments
where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterDataId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
begin
EXECUTE usp_ComputeHouseShipmentStatusFunction  @HouseId
end
FETCH NEXT FROM HousesCursor INTO  @HouseId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
if(@ShipmentLevelCode = ''A'')
BEGIN
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
From Shipments
where CustomFileId = @ShipmentId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @CustomId
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @IsConnect = 1
EXECUTE usp_ComputeHouseShipmentStatusFunction  @CustomId
end
FETCH NEXT FROM HousesCursor INTO @CustomId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
if(@ShipmentLevelCode =''A'' or @ShipmentLevelCode =''C'')
begin
update Shipments set CustomConnectToShipment = @IsConnect , ComputedStatusDate =@ComputedStatusDate , ComputedStatusId=  @ComputedStatusId where Id = @ShipmentId and Tenant = @Tenant
set @IsConnect = 0
end
if(@ShipmentLevelCode = ''H''  OR @ShipmentLevelCode = ''D'')
begin
EXECUTE usp_ComputeHouseShipmentStatusFunction  @ShipmentId
end');


-- Procedure Script From usp_DeleteShipmentsDataByTenant.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteShipmentsDataByTenant]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteShipmentsDataByTenant] END');
EXEC('Create PROCEDURE dbo.usp_DeleteShipmentsDataByTenant
(
@tenant int
)
AS
BEGIN TRANSACTION
delete from followups
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting followups'', 16, 1)
RETURN
END
delete from insideshipmentpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting insideshipmentpackages'', 16, 1)
RETURN
END
delete from ShipmentPackageItems
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentPackageItems'', 16, 1)
RETURN
END
delete from shipmentpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpackages'', 16, 1)
RETURN
END
delete from ShipmentPickUpDeliveryPackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentPickUpDeliveryPackages'', 16, 1)
RETURN
END
delete from shipmentpickupdeliveries
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpickupdeliveries'', 16, 1)
RETURN
END
delete from shipmentorderpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentorderpackages'', 16, 1)
RETURN
END
delete from shipmentpayables
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpayables'', 16, 1)
RETURN
END
delete from shipmentreceivables
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentreceivables'', 16, 1)
RETURN
END
delete from shipmentawbprintonlies
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentawbprintonlies'', 16, 1)
RETURN
END
delete from ShipmentCommodities
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentCommodities'', 16, 1)
RETURN
END
delete from AWBStockUsageHistories
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting AWBStockUsageHistories'', 16, 1)
RETURN
END
delete from AWBOCIs
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting AWBOCIs'', 16, 1)
RETURN
END
delete from shipmentmasterdatas
where tenant=@tenant;
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentmasterdatas'', 16, 1)
RETURN
END
if exists (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME =''FK_ShipmentShipmentMasterData'')
begin
alter table shipmentmasterdatas drop constraint FK_ShipmentShipmentMasterData
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in droping constraint FK_ShipmentShipmentMasterData'', 16, 1)
RETURN
END
end
delete from shipments
where tenant = @tenant;
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipments'', 16, 1)
RETURN
END
COMMIT
--exec usp_DeleteShipmentsDataByTenant 1');


-- Procedure Script From usp_DeleteShipmentsDataByTenant_Jan2018.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteShipmentsDataByTenant_Jan2018]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteShipmentsDataByTenant_Jan2018] END');
EXEC('CREATE PROCEDURE [dbo].[usp_DeleteShipmentsDataByTenant_Jan2018]
(
@tenant int
)
AS
BEGIN TRANSACTION
delete from followups
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting followups'', 16, 1)
RETURN
END
delete from insideshipmentpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting insideshipmentpackages'', 16, 1)
RETURN
END
delete from ShipmentPackageItems
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentPackageItems'', 16, 1)
RETURN
END
delete from shipmentpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpackages'', 16, 1)
RETURN
END
delete from ShipmentPickUpDeliveryPackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentPickUpDeliveryPackages'', 16, 1)
RETURN
END
delete from shipmentpickupdeliveries
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpickupdeliveries'', 16, 1)
RETURN
END
delete from shipmentorderpackages
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentorderpackages'', 16, 1)
RETURN
END
delete from shipmentpayables
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentpayables'', 16, 1)
RETURN
END
delete from shipmentreceivables
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentreceivables'', 16, 1)
RETURN
END
delete from shipmentawbprintonlies
where tenant=@tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentawbprintonlies'', 16, 1)
RETURN
END
delete from ShipmentCommodities
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentCommodities'', 16, 1)
RETURN
END
delete from AWBStockUsageHistories
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting AWBStockUsageHistories'', 16, 1)
RETURN
END
delete from AWBOCIs
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting AWBOCIs'', 16, 1)
RETURN
END
delete from ShipmentAdditionalCloudDatas
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentAdditionalCloudDatas'', 16, 1)
RETURN
END
delete from ShipmentComputedFields
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentComputedFields'', 16, 1)
RETURN
END
delete from ShipmentAssemblies
where Tenant = @tenant
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting ShipmentAssemblies'', 16, 1)
RETURN
END
delete from shipmentmasterdatas
where tenant=@tenant;
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipmentmasterdatas'', 16, 1)
RETURN
END
if exists (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME =''FK_ShipmentShipmentMasterData'')
begin
alter table shipmentmasterdatas drop constraint FK_ShipmentShipmentMasterData
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in droping constraint FK_ShipmentShipmentMasterData'', 16, 1)
RETURN
END
end
delete from shipments
where tenant = @tenant;
IF @@ERROR <> 0
BEGIN
ROLLBACK
RAISERROR (''Error in deleting shipments'', 16, 1)
RETURN
END
COMMIT
--exec usp_DeleteShipmentsDataByTenant 1');


-- Procedure Script From usp_UpdateConstituentShipment.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateConstituentShipment]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateConstituentShipment] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateConstituentShipment]
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


-- Procedure Script From usp_UpdateCustomConnectToShipment.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCustomConnectToShipment]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCustomConnectToShipment] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCustomConnectToShipment]
(
@ShipmentId varchar(15)
)
AS
declare @CountShipmentConnectedInCustomShipment as int
set @CountShipmentConnectedInCustomShipment = (select count(*) from Shipments where CustomFileId = @ShipmentId)
if(@CountShipmentConnectedInCustomShipment>0)
begin
update shipments set CustomConnectToShipment = 1 WHERE Id = @ShipmentId
end
else
begin
update shipments set CustomConnectToShipment = 0  WHERE Id = @ShipmentId
end');


-- Procedure Script From usp_UpdateInsidePackagesVolumetric.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateInsidePackagesVolumetric]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateInsidePackagesVolumetric] END');
EXEC('Create PROCEDURE dbo.usp_UpdateInsidePackagesVolumetric
AS
-- Inside Package Fields
DECLARE @CurrentTenant AS INT
DECLARE @CurrentPackageId AS varchar(15)
DECLARE @CurrentInsidePackageId AS varchar(15)
DECLARE @CurrentShipmentId AS varchar(15)
--Shipment Fields
DECLARE @Ratio as float
DECLARE @VolumeUnitCode as varchar(3)
DECLARE @GrossWeigthUnitCode as varchar(3)
DECLARE @ChargeableWeightUnitCode as varchar(3)
DECLARE @TransportModeId as varchar(1)
DECLARE @ShipmentTypeId as varchar(4)
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant,ShipmentPackageId
FROM InsideShipmentPackages
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @CurrentInsidePackageId,@CurrentTenant,@CurrentPackageId
WHILE @@FETCH_STATUS = 0
BEGIN
set @CurrentShipmentId = (select ShipmentId from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
set @Ratio = (select Ratio from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @VolumeUnitCode = (select VolumeUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @GrossWeigthUnitCode = (select GrossWeightUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @ChargeableWeightUnitCode = (select ChargeableWeightUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@Ratio is NULL)
begin
set @TransportModeId = (select TransportModeId from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@TransportModeId = ''A'') set @Ratio = 6
else if (@TransportModeId = ''O'') set @Ratio = 1
else if (@TransportModeId = ''I'')
begin
set @ShipmentTypeId = (select ShipmentTypeId from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@ShipmentTypeId = ''LTL'') set @Ratio = 3.3
else set @Ratio = 1
end
Update Shipments set Ratio = @Ratio where Id = @CurrentShipmentId AND Tenant = @CurrentTenant
end
DECLARE @Width as FLoat
DECLARE @Height as FLoat
DECLARE @Length as FLoat
DECLARE @Volume as FLoat
DECLARE @Weight as FLoat
DECLARE @VolumeCBM as FLoat
DECLARE @VolumeKG as FLoat
DECLARE @Volumetric as FLoat
set @Width = (SELECT Width from InsideShipmentPackages where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant)
set @Height = (SELECT Height from InsideShipmentPackages where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant)
set @Length = (SELECT Length from InsideShipmentPackages where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant)
set @Volume = (SELECT Volume from InsideShipmentPackages where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant)
set @Weight = (SELECT Weight from InsideShipmentPackages where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant)
-- Compute VolumeKG
if (@Width is NULL OR @Height is NULL OR @Length is NULL)
begin
if (@Volume is NULL)
begin
if (@GrossWeigthUnitCode = ''KG'')
begin
set @VolumeKG = Round(@Weight,3)
end
else if (@GrossWeigthUnitCode = ''LB'')
begin
set @VolumeKG = Round(@Weight / 2.20458,3)
end
else if (@GrossWeigthUnitCode = ''MT'')
begin
set @VolumeKG = Round(@Weight * 1000,3)
end
end
else
begin
if (@VolumeUnitCode = ''CBM'')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = ''CBF'')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = ''CBI'')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @Ratio,3)
end
end
else
begin
if (@VolumeUnitCode = ''CBM'')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = ''CBF'')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = ''CBI'')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @Ratio,3)
end
-- Compute Volumetric
if (@ChargeableWeightUnitCode = ''KG'')
begin
set @Volumetric = Round(@VolumeKG,3)
end
else if (@ChargeableWeightUnitCode = ''LB'')
begin
set @Volumetric = Round(@VolumeKG * 2.20458,3)
end
else if (@ChargeableWeightUnitCode = ''MT'')
begin
set @Volumetric = Round(@VolumeKG / 1000,3)
end
--print ''[PackageId:'' + @CurrentPackageId + ''] [Volume Unit:'' + @VolumeUnitCode + ''] [Gross Unit:'' + @GrossWeigthUnitCode + ''] [Chargeable Unit:'' + @ChargeableWeightUnitCode + ''] [Ratio:'' + Convert(varchar,ISNULL(@Ratio,-1)) + ''] [Volume:'' + Convert(varchar,ISNULL(@Volume,-1)) + ''] [VolumeKG:'' + Convert(varchar,ISNULL(@VolumeKG,-1)) + ''] [Volumetric:'' + Convert(varchar,ISNULL(@Volumetric,-1))
Update InsideShipmentPackages set VolumetricWeight = @Volumetric where Id = @CurrentInsidePackageId AND Tenant = @CurrentTenant
FETCH NEXT FROM PackagesCursor INTO @CurrentInsidePackageId,@CurrentTenant,@CurrentPackageId
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor');


-- Procedure Script From usp_UpdatePackagesVolumetric.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdatePackagesVolumetric]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdatePackagesVolumetric] END');
EXEC('Create PROCEDURE dbo.usp_UpdatePackagesVolumetric
AS
-- Package Fields
DECLARE @CurrentTenant AS INT
DECLARE @CurrentPackageId AS varchar(15)
DECLARE @CurrentShipmentId AS varchar(15)
--Shipment Fields
DECLARE @Ratio as float
DECLARE @VolumeUnitCode as varchar(3)
DECLARE @GrossWeigthUnitCode as varchar(3)
DECLARE @ChargeableWeightUnitCode as varchar(3)
DECLARE @TransportModeId as varchar(1)
DECLARE @ShipmentTypeId as varchar(4)
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant,ShipmentId
FROM ShipmentPackages
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @CurrentPackageId,@CurrentTenant,@CurrentShipmentId
WHILE @@FETCH_STATUS = 0
BEGIN
set @Ratio = (select Ratio from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @VolumeUnitCode = (select VolumeUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @GrossWeigthUnitCode = (select GrossWeightUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
set @ChargeableWeightUnitCode = (select ChargeableWeightUnitCode from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@Ratio is NULL)
begin
set @TransportModeId = (select TransportModeId from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@TransportModeId = ''A'') set @Ratio = 6
else if (@TransportModeId = ''O'') set @Ratio = 1
else if (@TransportModeId = ''I'')
begin
set @ShipmentTypeId = (select ShipmentTypeId from Shipments where Tenant = @CurrentTenant AND Id = @CurrentShipmentId)
if (@ShipmentTypeId = ''LTL'') set @Ratio = 3.3
else set @Ratio = 1
end
Update Shipments set Ratio = @Ratio where Id = @CurrentShipmentId AND Tenant = @CurrentTenant
end
DECLARE @Width as FLoat
DECLARE @Height as FLoat
DECLARE @Length as FLoat
DECLARE @Volume as FLoat
DECLARE @Weight as FLoat
DECLARE @VolumeCBM as FLoat
DECLARE @VolumeKG as FLoat
DECLARE @Volumetric as FLoat
set @Width = (SELECT Width from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
set @Height = (SELECT Height from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
set @Length = (SELECT Length from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
set @Volume = (SELECT Volume from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
set @Weight = (SELECT Weight from ShipmentPackages where Id = @CurrentPackageId AND Tenant = @CurrentTenant)
-- Compute VolumeKG
if (@Width is NULL OR @Height is NULL OR @Length is NULL)
begin
if (@Volume is NULL)
begin
if (@GrossWeigthUnitCode = ''KG'')
begin
set @VolumeKG = Round(@Weight,3)
end
else if (@GrossWeigthUnitCode = ''LB'')
begin
set @VolumeKG = Round(@Weight / 2.20458,3)
end
else if (@GrossWeigthUnitCode = ''MT'')
begin
set @VolumeKG = Round(@Weight * 1000,3)
end
end
else
begin
if (@VolumeUnitCode = ''CBM'')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = ''CBF'')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = ''CBI'')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @Ratio,3)
end
end
else
begin
if (@VolumeUnitCode = ''CBM'')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = ''CBF'')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = ''CBI'')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @Ratio,3)
end
-- Compute Volumetric
if (@ChargeableWeightUnitCode = ''KG'')
begin
set @Volumetric = Round(@VolumeKG,3)
end
else if (@ChargeableWeightUnitCode = ''LB'')
begin
set @Volumetric = Round(@VolumeKG * 2.20458,3)
end
else if (@ChargeableWeightUnitCode = ''MT'')
begin
set @Volumetric = Round(@VolumeKG / 1000,3)
end
--print ''[PackageId:'' + @CurrentPackageId + ''] [Volume Unit:'' + @VolumeUnitCode + ''] [Gross Unit:'' + @GrossWeigthUnitCode + ''] [Chargeable Unit:'' + @ChargeableWeightUnitCode + ''] [Ratio:'' + Convert(varchar,ISNULL(@Ratio,-1)) + ''] [Volume:'' + Convert(varchar,ISNULL(@Volume,-1)) + ''] [VolumeKG:'' + Convert(varchar,ISNULL(@VolumeKG,-1)) + ''] [Volumetric:'' + Convert(varchar,ISNULL(@Volumetric,-1))
Update ShipmentPackages set VolumetricWeight = @Volumetric where Id = @CurrentPackageId AND Tenant = @CurrentTenant
FETCH NEXT FROM PackagesCursor INTO @CurrentPackageId,@CurrentTenant,@CurrentShipmentId
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor');


-- Procedure Script From usp_UpdatePayablesData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdatePayablesData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdatePayablesData] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdatePayablesData]
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


-- Procedure Script From usp_UpdateReceivablesData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateReceivablesData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateReceivablesData] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateReceivablesData]
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


-- Procedure Script From usp_UpdateShipmentARInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentARInvoices]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentARInvoices] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentARInvoices]
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


-- Procedure Script From usp_UpdateShipmentFinalArrivalDate.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentFinalArrivalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
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


-- Procedure Script From usp_UpdateShipmentOperationalDate.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentOperationalDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentOperationalDate] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentOperationalDate]
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


-- Procedure Script From usp_UpdateShipmentProfit.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentProfit]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentProfit] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentProfit]
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


-- Procedure Script From usp_UpdateShipmentProfitFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentProfitFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentProfitFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentProfitFunction]
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


-- Procedure Script From usp_UpdateShipmentRegistryDate.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentRegistryDate]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate]
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


-- Procedure Script From usp_UpdateShipmentsSearchFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateShipmentsSearchFields]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateShipmentsSearchFields] END');
EXEC('CREATE PROCEDURE [dbo].[usp_UpdateShipmentsSearchFields]
AS
BEGIN
declare @SearchFields as nvarchar(1000)
declare @ShipmentId as varchar(15)
BEGIN
DECLARE eventsCursor CURSOR READ_ONLY
FOR
SELECT Id,SearchFields
FROM [dbo].[shipments] where SearchFields is not null and SearchFields <> '''' and SearchFields not like ''%*%'' --and (ShipmentNumber = ''1000'' and Tenant = 1)
OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @ShipmentId,@SearchFields
WHILE @@FETCH_STATUS = 0
BEGIN
declare @NewSearchFieldValue nvarchar(max) = (select SearchFields from [dbo].[shipments] where Id = @ShipmentId)
set @NewSearchFieldValue = @NewSearchFieldValue + ''*'' + (SELECT [dbo].[ReformatString](@SearchFields))
print len(@NewSearchFieldValue)
update [dbo].[shipments] set SearchFields = @NewSearchFieldValue where Id = @ShipmentId
FETCH NEXT FROM eventsCursor INTO @ShipmentId,@SearchFields
END
CLOSE eventsCursor
DEALLOCATE eventsCursor
END
END');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateAddresses.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateAddresses]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateAddresses] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAddresses ON Addresses AFTER Insert  AS  BEGIN UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateBranches.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateBranches]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateBranches] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateBranches ON Branches AFTER Insert  AS  BEGIN UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCards.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCards]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCards] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCards ON Cards AFTER Insert  AS  BEGIN UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateChargesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateChargesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateChargesTypes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateChargesTypes ON ChargesTypes AFTER Insert  AS  BEGIN UPDATE ChargesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateContacts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateContacts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateContacts] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateContacts ON Contacts AFTER Insert  AS  BEGIN UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCountries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCountries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCountries] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCountries ON Countries AFTER Insert  AS  BEGIN UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCurrencies.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCurrencies]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCurrencies] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCurrencies ON Currencies AFTER Insert  AS  BEGIN UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCustomers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomers] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomers ON Customers AFTER Insert  AS  BEGIN UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes ON CustomerSizes AFTER Insert  AS  BEGIN UPDATE CustomerSizes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateDepartments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateDepartments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateDepartments] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDepartments ON Departments AFTER Insert  AS  BEGIN UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateDWHSettings.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateDWHSettings]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateDWHSettings] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER Insert  AS  BEGIN UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Tenant IN (SELECT DISTINCT Tenant FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateIncoterms.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateIncoterms]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateIncoterms] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER Insert  AS  BEGIN UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateIndustries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateIndustries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateIndustries] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateIndustries ON Industries AFTER Insert  AS  BEGIN UPDATE Industries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateLeadSources.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateLeadSources]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateLeadSources] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateLeadSources ON LeadSources AFTER Insert  AS  BEGIN UPDATE LeadSources SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER Insert  AS  BEGIN UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDatePorts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDatePorts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDatePorts] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDatePorts ON Ports AFTER Insert  AS  BEGIN UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateRanks.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateRanks]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateRanks] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateRanks ON Ranks AFTER Insert  AS  BEGIN UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateRegions.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateRegions]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateRegions] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateRegions ON Regions AFTER Insert  AS  BEGIN UPDATE Regions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateStates.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateStates]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateStates] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateStates ON States AFTER Insert  AS  BEGIN UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateTenants.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateTenants]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateTenants] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateTenants ON Tenants AFTER Insert  AS  BEGIN UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateUsers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateUsers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateUsers] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateUsers ON Users AFTER Insert  AS  BEGIN UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateVessels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateVessels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateVessels] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateVessels ON Vessels AFTER Insert  AS  BEGIN UPDATE Vessels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateLeadSources.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateLeadSources]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateLeadSources] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateLeadSources ON LeadSources AFTER Insert  AS  BEGIN UPDATE LeadSources SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateAddresses.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAddresses]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAddresses] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAddresses ON Addresses AFTER UPDATE  AS  BEGIN UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateBranches.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateBranches]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateBranches] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateBranches ON Branches AFTER UPDATE  AS  BEGIN UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCards.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCards]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCards] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCards ON Cards AFTER UPDATE  AS  BEGIN UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateChargesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateChargesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateChargesTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateChargesTypes ON ChargesTypes AFTER UPDATE  AS  BEGIN UPDATE ChargesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateContacts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateContacts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateContacts] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateContacts ON Contacts AFTER UPDATE  AS  BEGIN UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCountries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCountries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCountries] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCountries ON Countries AFTER UPDATE  AS  BEGIN UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCurrencies.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCurrencies]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCurrencies] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCurrencies ON Currencies AFTER UPDATE  AS  BEGIN UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomers] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomers ON Customers AFTER UPDATE  AS  BEGIN UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomerSizes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomerSizes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomerSizes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomerSizes ON CustomerSizes AFTER UPDATE  AS  BEGIN UPDATE CustomerSizes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDepartments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDepartments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDepartments] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDepartments ON Departments AFTER UPDATE  AS  BEGIN UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDWHSettings.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDWHSettings]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDWHSettings] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER UPDATE  AS  BEGIN UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Tenant IN (SELECT DISTINCT Tenant FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateIncoterms.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateIncoterms]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateIncoterms] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER UPDATE  AS  BEGIN UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateIndustries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateIndustries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateIndustries] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateIndustries ON Industries AFTER UPDATE  AS  BEGIN UPDATE Industries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateLoadSoucess.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateLoadSoucess]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateLoadSoucess] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateLoadSoucess ON LeadSources AFTER UPDATE  AS  BEGIN UPDATE LeadSources SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDatePartnerTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDatePartnerTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDatePartnerTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER UPDATE  AS  BEGIN UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDatePorts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDatePorts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDatePorts] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDatePorts ON Ports AFTER UPDATE  AS  BEGIN UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateRanks.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateRanks]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateRanks] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateRanks ON Ranks AFTER UPDATE  AS  BEGIN UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateRegions.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateRegions]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateRegions] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateRegions ON Regions AFTER UPDATE  AS  BEGIN UPDATE Regions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateStates.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateStates]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateStates] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateStates ON States AFTER UPDATE  AS  BEGIN UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateTenants.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateTenants]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateTenants] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateTenants ON Tenants AFTER UPDATE  AS  BEGIN UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateUsers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateUsers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateUsers] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateUsers ON Users AFTER UPDATE  AS  BEGIN UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateVessels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateVessels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateVessels] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateVessels ON Vessels AFTER UPDATE  AS  BEGIN UPDATE Vessels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists ON CustomPickLists AFTER Insert  AS  BEGIN UPDATE CustomPickLists SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateDirections.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateDirections]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateDirections] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDirections ON Directions AFTER Insert  AS  BEGIN UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateEntityStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateEntityStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateEntityStatus] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER Insert  AS  BEGIN UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateMoveTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateMoveTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateMoveTypes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateMoveTypes ON MoveTypes AFTER Insert  AS  BEGIN UPDATE MoveTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateObjectFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateObjectFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateObjectFields] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateObjectFields ON ObjectFields AFTER Insert  AS  BEGIN UPDATE ObjectFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateTransportModes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateTransportModes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateTransportModes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER Insert  AS  BEGIN UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomPickLists.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomPickLists]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomPickLists] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomPickLists ON CustomPickLists AFTER UPDATE  AS  BEGIN UPDATE CustomPickLists SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDirections.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDirections]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDirections] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDirections ON Directions AFTER UPDATE  AS  BEGIN UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateEntityStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateEntityStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateEntityStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER UPDATE  AS  BEGIN UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateMoveTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateMoveTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateMoveTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateMoveTypes ON MoveTypes AFTER UPDATE  AS  BEGIN UPDATE MoveTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateObjectFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateObjectFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateObjectFields] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateObjectFields ON ObjectFields AFTER UPDATE  AS  BEGIN UPDATE ObjectFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateTransportModes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateTransportModes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateTransportModes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER UPDATE  AS  BEGIN UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines ON APInvoiceLines AFTER Insert  AS  BEGIN UPDATE APInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE APInvoiceId IN (SELECT DISTINCT APInvoiceId FROM Inserted) and LineNumber IN (SELECT DISTINCT LineNumber FROM Inserted)  END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateAPInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateAPInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateAPInvoices] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAPInvoices ON APInvoices AFTER Insert  AS  BEGIN UPDATE APInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines ON ARInvoiceLines AFTER Insert  AS  BEGIN UPDATE ARInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateARInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateARInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateARInvoices] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateARInvoices ON ARInvoices AFTER Insert  AS  BEGIN UPDATE ARInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateAPInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAPInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAPInvoiceLines] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAPInvoiceLines ON APInvoiceLines AFTER UPDATE  AS  BEGIN UPDATE APInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE APInvoiceId IN (SELECT DISTINCT APInvoiceId FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateAPInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAPInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAPInvoices] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAPInvoices ON APInvoices AFTER UPDATE  AS  BEGIN UPDATE APInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateARInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateARInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateARInvoiceLines] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateARInvoiceLines ON ARInvoiceLines AFTER UPDATE  AS  BEGIN UPDATE ARInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateARInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateARInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateARInvoices] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateARInvoices ON ARInvoices AFTER UPDATE  AS  BEGIN UPDATE ARInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields ON ShipmentComputedFields AFTER Insert  AS  BEGIN UPDATE ShipmentComputedFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER Insert  AS  BEGIN UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterData]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterData] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterData ON ShipmentMasterDatas AFTER Insert  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER Insert  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables ON ShipmentPayables AFTER Insert  AS  BEGIN UPDATE ShipmentPayables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables ON ShipmentReceivables AFTER Insert  AS  BEGIN UPDATE ShipmentReceivables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipments] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipments ON Shipments AFTER Insert  AS  BEGIN UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER Insert  AS  BEGIN UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes] END');
EXEC('CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes ON SpecialServicesTypes AFTER Insert  AS  BEGIN UPDATE SpecialServicesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentComputedFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentComputedFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentComputedFields] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentComputedFields ON ShipmentComputedFields AFTER UPDATE  AS  BEGIN UPDATE ShipmentComputedFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentLevels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentLevels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentLevels] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER UPDATE  AS  BEGIN UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentMasterDatas.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentMasterDatas]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentMasterDatas] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER UPDATE  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentPayables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentPayables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentPayables] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentPayables ON ShipmentPayables AFTER UPDATE  AS  BEGIN UPDATE ShipmentPayables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentPayableStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentPayableStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentPayableStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentPayableStatus ON ShipmentPayableStatus AFTER UPDATE  AS  BEGIN UPDATE ShipmentPayableStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentReceivables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivables] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentReceivables ON ShipmentReceivables AFTER UPDATE  AS  BEGIN UPDATE ShipmentReceivables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentReceivableStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivableStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivableStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentReceivableStatus ON ShipmentReceivableStatus AFTER UPDATE  AS  BEGIN UPDATE ShipmentReceivableStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipments] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipments ON Shipments AFTER UPDATE  AS  BEGIN UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER UPDATE  AS  BEGIN UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateSpecialServicesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateSpecialServicesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateSpecialServicesTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateSpecialServicesTypes ON SpecialServicesTypes AFTER UPDATE  AS  BEGIN UPDATE SpecialServicesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;');


-- General Script From FixFeatureBusinessUnitProperty.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Features set IsBusinessUnitEnabled = 0
declare @CustomerTableId as varchar(15)
declare @QuoteTableId as varchar(15)
declare @ActivityTableId as varchar(15)
declare @OpportunityTableId as varchar(15)
set @CustomerTableId = (select Id from ObjectTables where Name = 'Customer')
set @QuoteTableId = (select Id from ObjectTables where Name = 'Quote')
set @ActivityTableId = (select Id from ObjectTables where Name = 'Activity')
set @OpportunityTableId = (select Id from ObjectTables where Name = 'Opportunity')
update Features
set IsBusinessUnitEnabled = 1
where
ObjectTableId in (@CustomerTableId, @QuoteTableId, @ActivityTableId, @OpportunityTableId)
and Code in ('NEW', 'READ', 'UPDATE')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FixFeatureBusinessUnitProperty.sxml', GETDATE(), 'update Features set IsBusinessUnitEnabled = 0
declare @CustomerTableId as varchar(15)
declare @QuoteTableId as varchar(15)
declare @ActivityTableId as varchar(15)
declare @OpportunityTableId as varchar(15)
set @CustomerTableId = (select Id from ObjectTables where Name = ''Customer'')
set @QuoteTableId = (select Id from ObjectTables where Name = ''Quote'')
set @ActivityTableId = (select Id from ObjectTables where Name = ''Activity'')
set @OpportunityTableId = (select Id from ObjectTables where Name = ''Opportunity'')
update Features
set IsBusinessUnitEnabled = 1
where
ObjectTableId in (@CustomerTableId, @QuoteTableId, @ActivityTableId, @OpportunityTableId)
and Code in (''NEW'', ''READ'', ''UPDATE'')', DATEDIFF(MS,@StartTime,@EndTime), '11313c219477bd0296f4c33838080c22', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006092059_SetPermissionToAllBIFolders.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update BIReportFolders set PermissionForAll = 1
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006092059_SetPermissionToAllBIFolders.sxml', GETDATE(), 'update BIReportFolders set PermissionForAll = 1', DATEDIFF(MS,@StartTime,@EndTime), '1f42afaf1e581cb5298189859895c618', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From FillShipmentSearchFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
SearchFields nvarchar(4000)  null,
)
select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
into #tempTable
FROM Shipments
SET NOCOUNT ON
declare @MySearchFields as nvarchar(4000)
declare @PortsTable table
(
Id varchar(15) not null
)
declare @PartnersTable table
(
Id varchar(15) not null
)
declare @ReferencesTable table
(
Reference varchar(50) not null
)
-- Shipment fields
BEGIN
declare @Id as varchar(15)
declare @Tenant as int
declare @ShipmentNumber as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @TransportModeId as varchar(1)
declare @DirectionId as varchar(1)
declare @StatusId as varchar(15)
declare @StatusCode as varchar(4)
declare @StatusName as varchar(40)
declare @QuoteId as varchar(15)
declare @QuoteNumber as varchar(15)
declare @SalesmanUserId as varchar(15)
declare @SalesmanUserName as varchar(60)
declare @MasterShipmentDataId as varchar(15)
declare @House as varchar(20)
declare @CustomFileNumber as varchar(15)
declare @AWBCarrierTarrifReference as varchar(25)
declare @CustomsDeclarationNumber as varchar(35)
declare @ForwarderShipmentNumber as varchar(15)
declare @TransportDocumentNumber as varchar(50)
declare @ImportManifest as varchar(50)
declare @BookingConfirmationNumber as varchar(25)
declare @CarrierTransportDocumentNumber as varchar(50)
declare @ProjectNumber as varchar(100)
declare @AMSBL as nvarchar(17)
declare @WarehouseLegReference as nvarchar(50)
END
-- Ports Firlds
BEGIN
declare @PortId as varchar(15)
declare @PortCode as varchar(3)
declare @PortName as varchar(40)
declare @PortCountryCode as varchar(2)
declare @PortCountryName as varchar(120)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @PreCarriageFromPortId as varchar(15)
declare @PreCarriageToPortId as varchar(15)
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @MainCarriageFinalDestinationPortId as varchar(15)
END
-- Partners Fields
BEGIN
declare @PartnerId as varchar(15)
declare @PartnerName as varchar(60)
declare @CityName as varchar(120)
declare @PartnerReference1 as varchar(50)
declare @PartnerReference2 as varchar(50)
declare @AgentId as varchar(15)
declare @AgentReference1 as varchar(50)
declare @AgentReference2 as varchar(50)
declare @ShipperId as varchar(15)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)
declare @ConsigneeId as varchar(15)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)
declare @CustomerId as varchar(15)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)
declare @Notify1Id as varchar(15)
declare @Notify2Id as varchar(15)
declare @IssuingCarrierAgentId as varchar(15)
declare @CustomAgentImportId as varchar(15)
declare @CustomAgentImportReference as varchar(50)
declare @CustomAgentExportId as varchar(15)
declare @CustomAgentExportReference as varchar(50)
declare @ShipperNotExporterId as varchar(15)
declare @ConsigneeNotImporterId as varchar(15)
declare @FreightForwarderId as varchar(15)
declare @FreightForwarderReference as varchar(50)
declare @ConsolidatorId as varchar(15)
declare @ConsolidatorReference as varchar(50)
declare @ReleasingAgentId as varchar(15)
declare @ReleasingAgentReference1 as varchar(50)
declare @ReleasingAgentReference2 as varchar(50)
declare @MainCarriageFromAddressId as varchar(50)
declare @MainCarriageToAddressId as varchar(50)
END
-- MasterData Fields
BEGIN
declare @Master as varchar(20)
declare @LongMaster as varchar(30)
declare @MasterShipmentNumber as varchar(15)
declare @MainCarriageVesselId as varchar(15)
declare @MainCarriageVesselCode as varchar(5)
declare @MainCarriageVesselName as varchar(40)
declare @MainCarriageCarrierId as varchar(15)
declare @MainCarriageCarrierCode as varchar(15)
declare @MainCarriageCarrierName as varchar(60)
declare @MainCarriageCarrierPrefix as varchar(3)
declare @MainCarriageCarrierNumber as varchar(15)
declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
END
-- Custom Fields
BEGIN
declare @Field nvarchar(250)
declare @FieldName varchar(10)
declare @FieldDataTypeCode as varchar(10)
declare @Field1 nvarchar(250)
declare @Field2 nvarchar(250)
declare @Field3 nvarchar(250)
declare @Field4 nvarchar(250)
declare @Field5 nvarchar(250)
declare @Field6 nvarchar(250)
declare @Field7 nvarchar(250)
declare @Field8 nvarchar(250)
declare @Field9 nvarchar(250)
declare @Field10 nvarchar(250)
END
declare @ARInvoiceId as varchar(20)
declare @ARInvoiceNumber as varchar(20)
declare @ARInvoiceDraftNumber as varchar(20)
declare @ContainerNumber as varchar(20)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
WHILE @@FETCH_STATUS = 0
BEGIN
set @MySearchFields = ''
delete from @PortsTable
delete from @PartnersTable
delete from @ReferencesTable
-- Master Data
BEGIN
if (@MasterShipmentDataId is not null)
BEGIN
select
@Master = Master,
@MasterShipmentNumber = MasterShipmentNumber,
@MainCarriageVesselId = MainCarriageVesselId,
@MainCarriageCarrierId  = MainCarriageCarrierId,
@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
@MainCarriageFromPortId = MainCarriageFromPortId,
@MainCarriageToPortId = MainCarriageToPortId,
@Transshipment1FromPortId = Transshipment1FromPortId,
@Transshipment2FromPortId = Transshipment2FromPortId,
@Transshipment3FromPortId = Transshipment3FromPortId,
@Transshipment1ToPortId = Transshipment1ToPortId,
@Transshipment2ToPortId = Transshipment2ToPortId,
@Transshipment3ToPortId = Transshipment3ToPortId,
@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
@ImportManifest = ImportManifest,
@BookingConfirmationNumber = BookingConfirmationNumber,
@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
@MainCarriageFromAddressId = MainCarriageFromAddressId,
@MainCarriageToAddressId = MainCarriageToAddressId
from ShipmentMasterDatas
where Id = @MasterShipmentDataId AND Tenant = @Tenant
END
END
-- Fields
BEGIN
if (@ProjectNumber is not null AND @ProjectNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ProjectNumber
else set @MySearchFields = @MySearchFields + ',' + @ProjectNumber
end
if (@House is not null AND @House <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @House
else set @MySearchFields = @MySearchFields + ',' + @House
end
if (@Master is not null AND @Master <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Master
else set @MySearchFields = @MySearchFields + ',' + @Master
end
if (@ShipmentNumber is not null AND @ShipmentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ShipmentNumber
else set @MySearchFields = @MySearchFields + ',' + @ShipmentNumber
end
if (@CustomFileNumber is not null AND @CustomFileNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CustomFileNumber
else set @MySearchFields = @MySearchFields + ',' + @CustomFileNumber
end
if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @AWBCarrierTarrifReference
else set @MySearchFields = @MySearchFields + ',' + @AWBCarrierTarrifReference
end
if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CustomsDeclarationNumber
else set @MySearchFields = @MySearchFields + ',' + @CustomsDeclarationNumber
end
if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ForwarderShipmentNumber
else set @MySearchFields = @MySearchFields + ',' + @ForwarderShipmentNumber
end
if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @TransportDocumentNumber
else set @MySearchFields = @MySearchFields + ',' + @TransportDocumentNumber
end
if (@ImportManifest is not null AND @ImportManifest <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ImportManifest
else set @MySearchFields = @MySearchFields + ',' + @ImportManifest
end
if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @BookingConfirmationNumber
else set @MySearchFields = @MySearchFields + ',' + @BookingConfirmationNumber
end
if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CarrierTransportDocumentNumber
else set @MySearchFields = @MySearchFields + ',' + @CarrierTransportDocumentNumber
end
if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment1AdditionalMAWBOBLBL
end
if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment2AdditionalMAWBOBLBL
end
if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment3AdditionalMAWBOBLBL
end
if (@QuoteId is not null)
begin
set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
if (@QuoteNumber is not null AND @QuoteNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @QuoteNumber
else set @MySearchFields = @MySearchFields + ',' + @QuoteNumber
end
end
if (@StatusId is not null)
begin
select
@StatusCode = Code,
@StatusName = Name
from EntityStatus
where Id = @StatusId AND Tenant = @Tenant
if (@StatusCode is not null AND @StatusCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @StatusCode
else set @MySearchFields = @MySearchFields + ',' + @StatusCode
end
if (@StatusName is not null AND @StatusName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @StatusName
else set @MySearchFields = @MySearchFields + ',' + @StatusName
end
end
if (@MainCarriageVesselId is not null)
begin
select
@MainCarriageVesselCode = Code,
@MainCarriageVesselName = EnglishName
from Vessels
where Id = @MainCarriageVesselId AND Tenant = @Tenant
if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselCode
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselCode
end
if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselName
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselName
end
end
if (@MainCarriageCarrierId is not null)
begin
select
@MainCarriageCarrierCode = Cards.Code,
@MainCarriageCarrierName = Cards.EnglishName,
@MainCarriageCarrierPrefix = Airlines.Prefix
from Airlines join Cards on Airlines.Id = Cards.Id
where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
if (@TransportModeId = 'A' AND @Master is not null AND @Master <> '')
begin
set @LongMaster = @MainCarriageCarrierPrefix + '-' + @Master
if (@LongMaster is not null AND @LongMaster <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @LongMaster
else set @MySearchFields = @MySearchFields + ',' + @LongMaster
end
end
if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierCode
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierCode
end
if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierName
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierName
end
if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierNumber
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierNumber
end
end
if (@SalesmanUserId is not null)
begin
set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
if (@SalesmanUserName is not null AND @SalesmanUserName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @SalesmanUserName
else set @MySearchFields = @MySearchFields + ',' + @SalesmanUserName
end
end
if (@AMSBL is not null AND @AMSBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @AMSBL
else set @MySearchFields = @MySearchFields + ',' + @AMSBL
end
if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @WarehouseLegReference
else set @MySearchFields = @MySearchFields + ',' + @WarehouseLegReference
end
END
-- Ports
BEGIN
set @PortId = @FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @PreCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @PreCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @OnCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @OnCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment1FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment1ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment2FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment2ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment3FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment3ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageFinalDestinationPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
END
-- Partners
BEGIN
set @PartnerId = @AgentId
set @PartnerReference1 = @AgentReference1
set @PartnerReference2 = @AgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ShipperId
set @PartnerReference1 = @ShipperReference1
set @PartnerReference2 = @ShipperReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeId
set @PartnerReference1 = @ConsigneeReference1
set @PartnerReference2 = @ConsigneeReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomerId
set @PartnerReference1 = @CustomerReference1
set @PartnerReference2 = @CustomerReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @Notify1Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @Notify2Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @IssuingCarrierAgentId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentImportId
set @PartnerReference1 = @CustomAgentImportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentExportId
set @PartnerReference1 = @CustomAgentExportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ShipperNotExporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeNotImporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @FreightForwarderId
set @PartnerReference1 = @FreightForwarderReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsolidatorId
set @PartnerReference1 = @ConsolidatorReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ReleasingAgentId
set @PartnerReference1 = @ReleasingAgentReference1
set @PartnerReference2 = @ReleasingAgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
if(@TransportModeId = 'I' and @DirectionId ='D' and @MasterShipmentDataId is not null)
begin
set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '')
begin
if (@MySearchFields = '') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + ',' + @CityName
end
set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '')
begin
if (@MySearchFields = '') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + ',' + @CityName
end
end
END
-- Invoices
if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
BEGIN
DECLARE ARInvoicesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
begin
if (@MySearchFields = '') set @MySearchFields = @ARInvoiceNumber
else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceNumber
end
else if (@ARInvoiceDraftNumber is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @ARInvoiceDraftNumber
else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceDraftNumber
end
FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
END
CLOSE ARInvoicesCursor
DEALLOCATE ARInvoicesCursor
END
-- Packages Containers
if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
BEGIN
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT ContainerNumber
FROM ShipmentPackages
Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''
group by ContainerNumber
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ContainerNumber is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @ContainerNumber
else set @MySearchFields = @MySearchFields + ',' + @ContainerNumber
end
FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor
END
-- Custom Fields
BEGIN
set @Field = @Field1
set @FieldName = 'Field1'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field2
set @FieldName = 'Field2'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field3
set @FieldName = 'Field3'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field4
set @FieldName = 'Field4'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field5
set @FieldName = 'Field5'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field6
set @FieldName = 'Field6'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field7
set @FieldName = 'Field7'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field8
set @FieldName = 'Field8'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field9
set @FieldName = 'Field9'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field10
set @FieldName = 'Field10'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
END
insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
end
SET NOCOUNT OFF
drop table #tempTable
drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FillShipmentSearchFields.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
SearchFields nvarchar(4000)  null,
)
select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
into #tempTable
FROM Shipments
SET NOCOUNT ON
declare @MySearchFields as nvarchar(4000)
declare @PortsTable table
(
Id varchar(15) not null
)
declare @PartnersTable table
(
Id varchar(15) not null
)
declare @ReferencesTable table
(
Reference varchar(50) not null
)
-- Shipment fields
BEGIN
declare @Id as varchar(15)
declare @Tenant as int
declare @ShipmentNumber as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @TransportModeId as varchar(1)
declare @DirectionId as varchar(1)
declare @StatusId as varchar(15)
declare @StatusCode as varchar(4)
declare @StatusName as varchar(40)
declare @QuoteId as varchar(15)
declare @QuoteNumber as varchar(15)
declare @SalesmanUserId as varchar(15)
declare @SalesmanUserName as varchar(60)
declare @MasterShipmentDataId as varchar(15)
declare @House as varchar(20)
declare @CustomFileNumber as varchar(15)
declare @AWBCarrierTarrifReference as varchar(25)
declare @CustomsDeclarationNumber as varchar(35)
declare @ForwarderShipmentNumber as varchar(15)
declare @TransportDocumentNumber as varchar(50)
declare @ImportManifest as varchar(50)
declare @BookingConfirmationNumber as varchar(25)
declare @CarrierTransportDocumentNumber as varchar(50)
declare @ProjectNumber as varchar(100)
declare @AMSBL as nvarchar(17)
declare @WarehouseLegReference as nvarchar(50)
END
-- Ports Firlds
BEGIN
declare @PortId as varchar(15)
declare @PortCode as varchar(3)
declare @PortName as varchar(40)
declare @PortCountryCode as varchar(2)
declare @PortCountryName as varchar(120)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @PreCarriageFromPortId as varchar(15)
declare @PreCarriageToPortId as varchar(15)
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @MainCarriageFinalDestinationPortId as varchar(15)
END
-- Partners Fields
BEGIN
declare @PartnerId as varchar(15)
declare @PartnerName as varchar(60)
declare @CityName as varchar(120)
declare @PartnerReference1 as varchar(50)
declare @PartnerReference2 as varchar(50)
declare @AgentId as varchar(15)
declare @AgentReference1 as varchar(50)
declare @AgentReference2 as varchar(50)
declare @ShipperId as varchar(15)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)
declare @ConsigneeId as varchar(15)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)
declare @CustomerId as varchar(15)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)
declare @Notify1Id as varchar(15)
declare @Notify2Id as varchar(15)
declare @IssuingCarrierAgentId as varchar(15)
declare @CustomAgentImportId as varchar(15)
declare @CustomAgentImportReference as varchar(50)
declare @CustomAgentExportId as varchar(15)
declare @CustomAgentExportReference as varchar(50)
declare @ShipperNotExporterId as varchar(15)
declare @ConsigneeNotImporterId as varchar(15)
declare @FreightForwarderId as varchar(15)
declare @FreightForwarderReference as varchar(50)
declare @ConsolidatorId as varchar(15)
declare @ConsolidatorReference as varchar(50)
declare @ReleasingAgentId as varchar(15)
declare @ReleasingAgentReference1 as varchar(50)
declare @ReleasingAgentReference2 as varchar(50)
declare @MainCarriageFromAddressId as varchar(50)
declare @MainCarriageToAddressId as varchar(50)
END
-- MasterData Fields
BEGIN
declare @Master as varchar(20)
declare @LongMaster as varchar(30)
declare @MasterShipmentNumber as varchar(15)
declare @MainCarriageVesselId as varchar(15)
declare @MainCarriageVesselCode as varchar(5)
declare @MainCarriageVesselName as varchar(40)
declare @MainCarriageCarrierId as varchar(15)
declare @MainCarriageCarrierCode as varchar(15)
declare @MainCarriageCarrierName as varchar(60)
declare @MainCarriageCarrierPrefix as varchar(3)
declare @MainCarriageCarrierNumber as varchar(15)
declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
END
-- Custom Fields
BEGIN
declare @Field nvarchar(250)
declare @FieldName varchar(10)
declare @FieldDataTypeCode as varchar(10)
declare @Field1 nvarchar(250)
declare @Field2 nvarchar(250)
declare @Field3 nvarchar(250)
declare @Field4 nvarchar(250)
declare @Field5 nvarchar(250)
declare @Field6 nvarchar(250)
declare @Field7 nvarchar(250)
declare @Field8 nvarchar(250)
declare @Field9 nvarchar(250)
declare @Field10 nvarchar(250)
END
declare @ARInvoiceId as varchar(20)
declare @ARInvoiceNumber as varchar(20)
declare @ARInvoiceDraftNumber as varchar(20)
declare @ContainerNumber as varchar(20)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
WHILE @@FETCH_STATUS = 0
BEGIN
set @MySearchFields = ''''
delete from @PortsTable
delete from @PartnersTable
delete from @ReferencesTable
-- Master Data
BEGIN
if (@MasterShipmentDataId is not null)
BEGIN
select
@Master = Master,
@MasterShipmentNumber = MasterShipmentNumber,
@MainCarriageVesselId = MainCarriageVesselId,
@MainCarriageCarrierId  = MainCarriageCarrierId,
@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
@MainCarriageFromPortId = MainCarriageFromPortId,
@MainCarriageToPortId = MainCarriageToPortId,
@Transshipment1FromPortId = Transshipment1FromPortId,
@Transshipment2FromPortId = Transshipment2FromPortId,
@Transshipment3FromPortId = Transshipment3FromPortId,
@Transshipment1ToPortId = Transshipment1ToPortId,
@Transshipment2ToPortId = Transshipment2ToPortId,
@Transshipment3ToPortId = Transshipment3ToPortId,
@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
@ImportManifest = ImportManifest,
@BookingConfirmationNumber = BookingConfirmationNumber,
@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
@MainCarriageFromAddressId = MainCarriageFromAddressId,
@MainCarriageToAddressId = MainCarriageToAddressId
from ShipmentMasterDatas
where Id = @MasterShipmentDataId AND Tenant = @Tenant
END
END
-- Fields
BEGIN
if (@ProjectNumber is not null AND @ProjectNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ProjectNumber
else set @MySearchFields = @MySearchFields + '','' + @ProjectNumber
end
if (@House is not null AND @House <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @House
else set @MySearchFields = @MySearchFields + '','' + @House
end
if (@Master is not null AND @Master <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Master
else set @MySearchFields = @MySearchFields + '','' + @Master
end
if (@ShipmentNumber is not null AND @ShipmentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ShipmentNumber
else set @MySearchFields = @MySearchFields + '','' + @ShipmentNumber
end
if (@CustomFileNumber is not null AND @CustomFileNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CustomFileNumber
else set @MySearchFields = @MySearchFields + '','' + @CustomFileNumber
end
if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @AWBCarrierTarrifReference
else set @MySearchFields = @MySearchFields + '','' + @AWBCarrierTarrifReference
end
if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CustomsDeclarationNumber
else set @MySearchFields = @MySearchFields + '','' + @CustomsDeclarationNumber
end
if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ForwarderShipmentNumber
else set @MySearchFields = @MySearchFields + '','' + @ForwarderShipmentNumber
end
if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @TransportDocumentNumber
else set @MySearchFields = @MySearchFields + '','' + @TransportDocumentNumber
end
if (@ImportManifest is not null AND @ImportManifest <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ImportManifest
else set @MySearchFields = @MySearchFields + '','' + @ImportManifest
end
if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @BookingConfirmationNumber
else set @MySearchFields = @MySearchFields + '','' + @BookingConfirmationNumber
end
if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CarrierTransportDocumentNumber
else set @MySearchFields = @MySearchFields + '','' + @CarrierTransportDocumentNumber
end
if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment1AdditionalMAWBOBLBL
end
if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment2AdditionalMAWBOBLBL
end
if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment3AdditionalMAWBOBLBL
end
if (@QuoteId is not null)
begin
set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
if (@QuoteNumber is not null AND @QuoteNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @QuoteNumber
else set @MySearchFields = @MySearchFields + '','' + @QuoteNumber
end
end
if (@StatusId is not null)
begin
select
@StatusCode = Code,
@StatusName = Name
from EntityStatus
where Id = @StatusId AND Tenant = @Tenant
if (@StatusCode is not null AND @StatusCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @StatusCode
else set @MySearchFields = @MySearchFields + '','' + @StatusCode
end
if (@StatusName is not null AND @StatusName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @StatusName
else set @MySearchFields = @MySearchFields + '','' + @StatusName
end
end
if (@MainCarriageVesselId is not null)
begin
select
@MainCarriageVesselCode = Code,
@MainCarriageVesselName = EnglishName
from Vessels
where Id = @MainCarriageVesselId AND Tenant = @Tenant
if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselCode
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselCode
end
if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselName
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselName
end
end
if (@MainCarriageCarrierId is not null)
begin
select
@MainCarriageCarrierCode = Cards.Code,
@MainCarriageCarrierName = Cards.EnglishName,
@MainCarriageCarrierPrefix = Airlines.Prefix
from Airlines join Cards on Airlines.Id = Cards.Id
where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
if (@TransportModeId = ''A'' AND @Master is not null AND @Master <> '''')
begin
set @LongMaster = @MainCarriageCarrierPrefix + ''-'' + @Master
if (@LongMaster is not null AND @LongMaster <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @LongMaster
else set @MySearchFields = @MySearchFields + '','' + @LongMaster
end
end
if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierCode
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierCode
end
if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierName
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierName
end
if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierNumber
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierNumber
end
end
if (@SalesmanUserId is not null)
begin
set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
if (@SalesmanUserName is not null AND @SalesmanUserName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @SalesmanUserName
else set @MySearchFields = @MySearchFields + '','' + @SalesmanUserName
end
end
if (@AMSBL is not null AND @AMSBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @AMSBL
else set @MySearchFields = @MySearchFields + '','' + @AMSBL
end
if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @WarehouseLegReference
else set @MySearchFields = @MySearchFields + '','' + @WarehouseLegReference
end
END
-- Ports
BEGIN
set @PortId = @FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @PreCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @PreCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @OnCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @OnCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment1FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment1ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment2FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment2ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment3FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment3ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageFinalDestinationPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
END
-- Partners
BEGIN
set @PartnerId = @AgentId
set @PartnerReference1 = @AgentReference1
set @PartnerReference2 = @AgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ShipperId
set @PartnerReference1 = @ShipperReference1
set @PartnerReference2 = @ShipperReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeId
set @PartnerReference1 = @ConsigneeReference1
set @PartnerReference2 = @ConsigneeReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomerId
set @PartnerReference1 = @CustomerReference1
set @PartnerReference2 = @CustomerReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @Notify1Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @Notify2Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @IssuingCarrierAgentId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentImportId
set @PartnerReference1 = @CustomAgentImportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentExportId
set @PartnerReference1 = @CustomAgentExportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ShipperNotExporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeNotImporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @FreightForwarderId
set @PartnerReference1 = @FreightForwarderReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsolidatorId
set @PartnerReference1 = @ConsolidatorReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ReleasingAgentId
set @PartnerReference1 = @ReleasingAgentReference1
set @PartnerReference2 = @ReleasingAgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
if(@TransportModeId = ''I'' and @DirectionId =''D'' and @MasterShipmentDataId is not null)
begin
set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + '','' + @CityName
end
set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + '','' + @CityName
end
end
END
-- Invoices
if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
BEGIN
DECLARE ARInvoicesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
begin
if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceNumber
else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceNumber
end
else if (@ARInvoiceDraftNumber is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceDraftNumber
else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceDraftNumber
end
FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
END
CLOSE ARInvoicesCursor
DEALLOCATE ARInvoicesCursor
END
-- Packages Containers
if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
BEGIN
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT ContainerNumber
FROM ShipmentPackages
Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''''
group by ContainerNumber
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ContainerNumber is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @ContainerNumber
else set @MySearchFields = @MySearchFields + '','' + @ContainerNumber
end
FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor
END
-- Custom Fields
BEGIN
set @Field = @Field1
set @FieldName = ''Field1''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field2
set @FieldName = ''Field2''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field3
set @FieldName = ''Field3''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field4
set @FieldName = ''Field4''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field5
set @FieldName = ''Field5''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field6
set @FieldName = ''Field6''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field7
set @FieldName = ''Field7''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field8
set @FieldName = ''Field8''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field9
set @FieldName = ''Field9''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field10
set @FieldName = ''Field10''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
END
insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
end
SET NOCOUNT OFF
drop table #tempTable
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), '6db51d0586951d13a1df2d84cf5de152', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006181218_DropCardCurrenciesAccountingsTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if exists (SELECT * FROM sys.tables WHERE name='CardCurrenciesAccountings')
begin
drop table CardCurrenciesAccountings;
end
delete from Screens where ObjectTableId = (select id from ObjectTables where name = 'CardCurrenciesAccounting')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'CardCurrenciesAccounting')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name = 'CardCurrenciesAccounting')
delete from EventTypes where ObjectTableId = (select id from ObjectTables where name = 'CardCurrenciesAccounting')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name = 'CardCurrenciesAccounting')
delete from ObjectTables where name = 'CardCurrenciesAccounting'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006181218_DropCardCurrenciesAccountingsTable.sxml', GETDATE(), 'if exists (SELECT * FROM sys.tables WHERE name=''CardCurrenciesAccountings'')
begin
drop table CardCurrenciesAccountings;
end
delete from Screens where ObjectTableId = (select id from ObjectTables where name = ''CardCurrenciesAccounting'')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name = ''CardCurrenciesAccounting'')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name = ''CardCurrenciesAccounting'')
delete from EventTypes where ObjectTableId = (select id from ObjectTables where name = ''CardCurrenciesAccounting'')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name = ''CardCurrenciesAccounting'')
delete from ObjectTables where name = ''CardCurrenciesAccounting''', DATEDIFF(MS,@StartTime,@EndTime), '61861fee332ab2044675aae449118f10', 3);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051545_SetVATUniquePartnerDefaultValue.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Tenants set VatUniquePartnerTypeCode = 'ALL'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051545_SetVATUniquePartnerDefaultValue.sxml', GETDATE(), 'update Tenants set VatUniquePartnerTypeCode = ''ALL''', DATEDIFF(MS,@StartTime,@EndTime), 'c385eb7d7d93a571f33003705585983b', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007061105_FillMissingPortCountryCode.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
UPDATE Ports SET CountryCode = (SELECT Code FROM Countries WHERE Id = CountryId) WHERE CountryCode IS NULL
UPDATE Ports SET CountryName = (SELECT EnglishName FROM Countries WHERE Id = CountryId) WHERE CountryName IS NULL
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007061105_FillMissingPortCountryCode.sxml', GETDATE(), 'UPDATE Ports SET CountryCode = (SELECT Code FROM Countries WHERE Id = CountryId) WHERE CountryCode IS NULL
UPDATE Ports SET CountryName = (SELECT EnglishName FROM Countries WHERE Id = CountryId) WHERE CountryName IS NULL', DATEDIFF(MS,@StartTime,@EndTime), 'a261682157a8891372c24bf22c0e218a', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From CargoTrackingHeaderEntityTypeFillValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values ('O','','O,Order','Order',0)
Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values ('F','','F,Forwarding Shipment','Forwarding Shipment',0)
Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values ('C','','C,Customs Shipment','Customs Shipment',0)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('CargoTrackingHeaderEntityTypeFillValues.sxml', GETDATE(), 'Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values (''O'','''',''O,Order'',''Order'',0)
Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values (''F'','''',''F,Forwarding Shipment'',''Forwarding Shipment'',0)
Insert Into CargoTrackingHeaderEntityTypes (Code,LocalName,SearchFields,EnglishName,Inactive) values (''C'','''',''C,Customs Shipment'',''Customs Shipment'',0)', DATEDIFF(MS,@StartTime,@EndTime), '8140427f48a69dbd283d52c121c8a41a', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From CargoTrackingMilestoneFillValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('1','','1,Booking','Booking',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('2','','2,Pickup','Pickup',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('3','','3,From Warehouse','From Warehouse',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('4','','4,Departure','Departure',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('5','','5,Arrival','Arrival',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('6','','6,To Warehouse','To Warehouse',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('7','','7,Customs Process','Customs Process',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('8','','8,Customs Payment','Customs Payment',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('9','','9,Clearance','Clearance',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('10','','10,Assigned to Trucker','Assigned to Trucker',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('11','','11,Delivered','Delivered',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values ('12','','12,Invoiced','Invoiced',0)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('CargoTrackingMilestoneFillValues.sxml', GETDATE(), 'Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''1'','''',''1,Booking'',''Booking'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''2'','''',''2,Pickup'',''Pickup'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''3'','''',''3,From Warehouse'',''From Warehouse'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''4'','''',''4,Departure'',''Departure'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''5'','''',''5,Arrival'',''Arrival'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''6'','''',''6,To Warehouse'',''To Warehouse'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''7'','''',''7,Customs Process'',''Customs Process'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''8'','''',''8,Customs Payment'',''Customs Payment'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''9'','''',''9,Clearance'',''Clearance'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''10'','''',''10,Assigned to Trucker'',''Assigned to Trucker'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''11'','''',''11,Delivered'',''Delivered'',0)
Insert Into CargoTrackingMilestones (Code,LocalName,SearchFields,EnglishName,Inactive) values (''12'','''',''12,Invoiced'',''Invoiced'',0)', DATEDIFF(MS,@StartTime,@EndTime), '3145361782ef47357708ab03e7605d14', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011050_UpdateAllowedInTicketFieldValueOfObjectTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update ObjectTables set AllowedInTicket=1 where Name in ('shipment','Quote')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011050_UpdateAllowedInTicketFieldValueOfObjectTable.sxml', GETDATE(), 'update ObjectTables set AllowedInTicket=1 where Name in (''shipment'',''Quote'')', DATEDIFF(MS,@StartTime,@EndTime), '23be70443f012ab36745546aaef594f9', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006291330_FillPaidDateOfInvoices.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
-- not used, run manually
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006291330_FillPaidDateOfInvoices.sxml', GETDATE(), '-- not used, run manually', DATEDIFF(MS,@StartTime,@EndTime), '9c7d4b76a39e0183af6d3eaef3c13a20', 4);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011347_FillQuoteClosingReasonTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if exists (SELECT * FROM sys.tables WHERE name='QuoteClosingReasons')
begin
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
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
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'EQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('EQ', 'Expensive Quote', 'EQ,Expensive Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'GS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('GS', 'Given directly to the Shipping Line', 'GS,Given directly to the Shipping Line', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LC')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LC', 'Lost to Competitor', 'LC,Lost to Competitor', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LS', 'Lack of Service in the Last Shipment', 'LS,Lack of Service in the Last Shipment', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'XQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('XQ', 'Expired Quote', 'XQ,Expired Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'BM')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('BM', 'Benchmarking', 'BM,Benchmarking', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LT')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LT', 'Long Term Project', 'LT,Long Term Project', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
End
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011347_FillQuoteClosingReasonTable.sxml', GETDATE(), 'if exists (SELECT * FROM sys.tables WHERE name=''QuoteClosingReasons'')
begin
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
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
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''EQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''EQ'', ''Expensive Quote'', ''EQ,Expensive Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''GS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''GS'', ''Given directly to the Shipping Line'', ''GS,Given directly to the Shipping Line'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LC'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LC'', ''Lost to Competitor'', ''LC,Lost to Competitor'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LS'', ''Lack of Service in the Last Shipment'', ''LS,Lack of Service in the Last Shipment'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''XQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''XQ'', ''Expired Quote'', ''XQ,Expired Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''BM'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''BM'', ''Benchmarking'', ''BM,Benchmarking'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LT'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LT'', ''Long Term Project'', ''LT,Long Term Project'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
End', DATEDIFF(MS,@StartTime,@EndTime), '9a21026461ef1ddc105994a664531592', 3);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011458_FillQuoteClosingReasonId.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if exists (SELECT * FROM sys.tables WHERE name='QuoteClosingReasons')
begin
update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )
End
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011458_FillQuoteClosingReasonId.sxml', GETDATE(), 'if exists (SELECT * FROM sys.tables WHERE name=''QuoteClosingReasons'')
begin
update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )
End', DATEDIFF(MS,@StartTime,@EndTime), '3e7a89b90aa642f4b6f0d8f49384920e', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011500_AddQuoteClosingReasonEventTypesToTenants.sxml File
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
set @Code = 'MAIN'
set @Name = 'Marked as Inactive'
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
set @Code = 'REAC'
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
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011500_AddQuoteClosingReasonEventTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
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
set @Code = ''MAIN''
set @Name = ''Marked as Inactive''
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
set @Code = ''REAC''
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
END', DATEDIFF(MS,@StartTime,@EndTime), '378c9c4533e5cc2475120d6951d86fa2', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

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
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151500_FillAirQuoteShipmentTypeField.sxml', GETDATE(), 'NULL', DATEDIFF(MS,@StartTime,@EndTime), '0f315678c7cb8ac070f70fd277f9a5c2', 2);
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
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006151503_FillQuoteShipmentSubTypeBackward.sxml', GETDATE(), 'NULL', DATEDIFF(MS,@StartTime,@EndTime), 'f51b19219b64a436bc97a4c53f011604', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007091327_UpdateAirQuotesSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Quotes
set
ShipmentTypeId = 'Air',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = 'Air' and Tenant = Quotes.Tenant)
where TransportModeId = 'A'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007091327_UpdateAirQuotesSubType.sxml', GETDATE(), 'update Quotes
set
ShipmentTypeId = ''Air'',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = ''Air'' and Tenant = Quotes.Tenant)
where TransportModeId = ''A''', DATEDIFF(MS,@StartTime,@EndTime), '78425fec5fbe9daa77a54e2713ece129', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007091330_UpdateNotAirQuotesSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Quotes') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Quotes.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Quotes where TransportModeId <> 'A'
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Quotes(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007091330_UpdateNotAirQuotesSubType.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Quotes'') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Quotes.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Quotes where TransportModeId <> ''A''
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Quotes(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes', DATEDIFF(MS,@StartTime,@EndTime), '9b0f17994947605aebfe0de43df814c6', 2);
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
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'MyGI')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'MyGI')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'MyGI', 'My Groupage Inland', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'MyGI,My Groupage Inland')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'MyGO')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'MyGO')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'MyGO', 'My Groupage Ocean', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'MyGO,My Groupage Ocean')
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
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''MyGI'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''MyGI'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''MyGI'', ''My Groupage Inland'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''MyGI,My Groupage Inland'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''MyGO'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''MyGO'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''MyGO'', ''My Groupage Ocean'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''MyGO,My Groupage Ocean'')
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), 'edf4b5245328396929db6714829ebe59', 2);
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
print 'Not needed anymore'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006140958_FillAirShipmentShipmentTypeField.sxml', GETDATE(), 'print ''Not needed anymore''', DATEDIFF(MS,@StartTime,@EndTime), 'deabbd1a1c3f5b113b40db7a99f29d27', 4);
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
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006141227_FillShipmentSubTypeBackward.sxml', GETDATE(), 'NULL', DATEDIFF(MS,@StartTime,@EndTime), '18f11896d7c6c546efed183e4048901a', 4);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051320_UpdateAirShipmentSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Shipments
set
ShipmentTypeId = 'Air',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = 'Air' and Tenant = Shipments.Tenant)
where TransportModeId = 'A'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051320_UpdateAirShipmentSubType.sxml', GETDATE(), 'update Shipments
set
ShipmentTypeId = ''Air'',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = ''Air'' and Tenant = Shipments.Tenant)
where TransportModeId = ''A''', DATEDIFF(MS,@StartTime,@EndTime), 'dfc7b62614e58b4866e397ef403827f6', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051322_UpdateNotAirShipmentsSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'MyGO' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGO' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'MyGI' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGI' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> 'A'
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
end
drop table #tempTable
drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051322_UpdateNotAirShipmentsSubType.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''MyGO'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGO'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''MyGI'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGI'' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> ''A''
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id = #temp_Shipments.Id
end
drop table #tempTable
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), '3b008b6c77f3787e0fdf830e3a9fb222', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202005262050_FillFreightChargeTypeIdOfTariffTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Id as varchar(15)
declare @TypeCode as varchar(15)
declare @FreightChargeTypeId as varchar(15)
BEGIN
DECLARE TariffCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TypeCode
FROM Tariffs
OPEN TariffCursor FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
WHILE @@FETCH_STATUS = 0
BEGIN
if (@TypeCode = 'AFC' or @TypeCode = 'OLC' or @TypeCode = 'OFC')
begin
if (@TypeCode = 'AFC')
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = 'AFT')
end
else
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = 'OFT')
end
update Tariffs set FreightChargeId = @FreightChargeTypeId  where Id = @Id and Tenant = @Tenant
end
FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
END
CLOSE TariffCursor
DEALLOCATE TariffCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202005262050_FillFreightChargeTypeIdOfTariffTable.sxml', GETDATE(), 'declare @Tenant as int
declare @Id as varchar(15)
declare @TypeCode as varchar(15)
declare @FreightChargeTypeId as varchar(15)
BEGIN
DECLARE TariffCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TypeCode
FROM Tariffs
OPEN TariffCursor FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
WHILE @@FETCH_STATUS = 0
BEGIN
if (@TypeCode = ''AFC'' or @TypeCode = ''OLC'' or @TypeCode = ''OFC'')
begin
if (@TypeCode = ''AFC'')
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = ''AFT'')
end
else
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = ''OFT'')
end
update Tariffs set FreightChargeId = @FreightChargeTypeId  where Id = @Id and Tenant = @Tenant
end
FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
END
CLOSE TariffCursor
DEALLOCATE TariffCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '9f999a06fe8ba464ed210f3f9d3d7217', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006211023_SetTariffSettingDefaultCurrency.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update TariffSettings set DefaultCurrencyId = (select ProfitCurrencyId from Tenants where Id = TariffSettings.Tenant)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006211023_SetTariffSettingDefaultCurrency.sxml', GETDATE(), 'update TariffSettings set DefaultCurrencyId = (select ProfitCurrencyId from Tenants where Id = TariffSettings.Tenant)', DATEDIFF(MS,@StartTime,@EndTime), '43a37f27e3f9624fb0b112c28a0dd074', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007151230_FixNULLChargeableWeightForWarehouseEntries.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT ChargeableWeightUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND WarehouseEntries.TransportModeId = 'A'
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT WeightMeasurementUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND (WarehouseEntries.TransportModeId = 'O' OR WarehouseEntries.TransportModeId = 'I')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007151230_FixNULLChargeableWeightForWarehouseEntries.sxml', GETDATE(), 'Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT ChargeableWeightUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND WarehouseEntries.TransportModeId = ''A''
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT WeightMeasurementUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND (WarehouseEntries.TransportModeId = ''O'' OR WarehouseEntries.TransportModeId = ''I'')', DATEDIFF(MS,@StartTime,@EndTime), 'a6ef11f2fadee0049efaeb0d7cb2e1fb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

