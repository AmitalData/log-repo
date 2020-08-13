-- Create New Table With Name CardSearchs
CREATE TABLE [dbo].[CardSearchs](
[Id] VARCHAR(15) NOT NULL,
[Tenant] INT NOT NULL,
[RecordDate] DATETIME NOT NULL,
[Keyword] NVARCHAR(100) NULL,
[Weight] INT NOT NULL,
[CardId] VARCHAR(15) NULL,
CONSTRAINT [PK_CardSearchs] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('66b0d64f-766d-4454-bb61-5edd75efa6eb', 'CardSearch.dxml', 'CardSearchs', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CardSearchsCREATE TABLE [dbo].[CardSearchs]([Id] VARCHAR(15) NOT NULL,[Tenant] INT NOT NULL,[RecordDate] DATETIME NOT NULL,[Keyword] NVARCHAR(100) NULL,[Weight] INT NOT NULL,[CardId] VARCHAR(15) NULL,CONSTRAINT [PK_CardSearchs] PRIMARY KEY([Id]));');


-- Change Type From nvarchar To varchar For Column CompetitorFields
ALTER TABLE [dbo].[Customers] ALTER COLUMN [CompetitorFields] VARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('12c36928-0cce-4d3c-8a24-d0ccd4349ec3', 'Customer.dxml', 'Customers', 'CompetitorFields', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column CompetitorFieldsALTER TABLE [dbo].[Customers] ALTER COLUMN [CompetitorFields] VARCHAR(1000);');


-- Create Unique Constraint On Shipments Table
EXEC('ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [UQ_Shipments_Tenant_ComputedForwarderShipmentNumber] UNIQUE([Tenant],[ComputedForwarderShipmentNumber])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0a03a94d-edf2-400b-be76-abf3336c8775', 'Shipment.dxml', 'Shipments', 'Tenant,ComputedForwarderShipmentNumber', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On Shipments TableEXEC(''ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [UQ_Shipments_Tenant_ComputedForwarderShipmentNumber] UNIQUE([Tenant],[ComputedForwarderShipmentNumber])'');');


-- Add Foreign Key Constraint For Column CardId In Table CardSearchs As Reference To Column Id In Table Cards
EXEC('ALTER TABLE [dbo].[CardSearchs] ADD CONSTRAINT [FK_CardSearchs_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('48d64a6b-f914-48fb-a91c-6ef346fa29eb', 'CardSearch.dxml', 'CardSearchs', 'CardId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CardId In Table CardSearchs As Reference To Column Id In Table CardsEXEC(''ALTER TABLE [dbo].[CardSearchs] ADD CONSTRAINT [FK_CardSearchs_Cards_CardId] FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id])'');');

-- Create Index On CardSearchs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearchs_CardId] ON [dbo].[CardSearchs]([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ef9ebb82-5778-40ae-ba9c-881d6e0dc52e', 'CardSearch.dxml', 'CardSearchs', 'CardId', 'Create Index', GETDATE(), '-- Create Index On CardSearchs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearchs_CardId] ON [dbo].[CardSearchs]([CardId])'');');


