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

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7eafcebf-eac4-473f-8922-ba2eb173ad88', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CardCurrenciesAccountingsCREATE TABLE [dbo].[CardCurrenciesAccountings]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[CardId] VARCHAR(15) NULL,[CurrencyId] VARCHAR(15) NULL,[PayableDebitAccount] VARCHAR(15) NULL,[ReceivableCreditAccount] VARCHAR(15) NULL,CONSTRAINT [PK_CardCurrenciesAccountings] PRIMARY KEY([Id]));');


-- Add Foreign Key Constraint For Column CardId In Table CardCurrenciesAccountings As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c99af11a-6ae9-4470-9496-4750f802abd1', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CardId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CardId In Table CardCurrenciesAccountings As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On CardCurrenciesAccountings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CardId] ON [dbo].[CardCurrenciesAccountings]([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4854937a-4269-44b7-a18a-98ca9eb8749d', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CardId', 'Create Index', GETDATE(), '-- Create Index On CardCurrenciesAccountings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CardId] ON [dbo].[CardCurrenciesAccountings]([CardId])'');');

-- Add Foreign Key Constraint For Column CurrencyId In Table CardCurrenciesAccountings As Reference To Column Id In Table Currencies
EXEC('ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d4ab79e9-455a-4a54-b7a1-724172930192', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CurrencyId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrencyId In Table CardCurrenciesAccountings As Reference To Column Id In Table CurrenciesEXEC(''ALTER TABLE [dbo].[CardCurrenciesAccountings] ADD CONSTRAINT [FK_CardCurrenciesAccountings_Currencies_CurrencyId] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currencies]([Id])'');');

-- Create Index On CardCurrenciesAccountings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CurrencyId] ON [dbo].[CardCurrenciesAccountings]([CurrencyId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d35ffc53-d240-42b4-9634-8175ba76a18a', 'CardCurrenciesAccounting.dxml', 'CardCurrenciesAccountings', 'CurrencyId', 'Create Index', GETDATE(), '-- Create Index On CardCurrenciesAccountings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardCurrenciesAccountings_CurrencyId] ON [dbo].[CardCurrenciesAccountings]([CurrencyId])'');');


