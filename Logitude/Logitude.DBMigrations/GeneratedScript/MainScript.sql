-- Create New Table With Name SuppInvoiceItemsAbachStatement
CREATE TABLE [Customs].[SuppInvoiceItemsAbachStatement](
[DeclarationId] VARCHAR(15) NOT NULL,
[InvoiceCounterKey] INT NOT NULL,
[InvoiceItemLineNumber] INT NOT NULL,
[SequenceNumeric] INT NOT NULL,
[Tenant] INT NOT NULL,
[StatementTypeCode] VARCHAR(4) NULL,
[IsStatementInd] BIT DEFAULT(0) NOT NULL,
CONSTRAINT [PK_SuppInvoiceItemsAbachStatement] PRIMARY KEY([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber],[SequenceNumeric])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('41263dc0-3b92-43a7-bfde-fd09642ebbbc', 'SuppInvoiceItemsAbachStatement.dxml', 'SuppInvoiceItemsAbachStatement', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name SuppInvoiceItemsAbachStatementCREATE TABLE [Customs].[SuppInvoiceItemsAbachStatement]([DeclarationId] VARCHAR(15) NOT NULL,[InvoiceCounterKey] INT NOT NULL,[InvoiceItemLineNumber] INT NOT NULL,[SequenceNumeric] INT NOT NULL,[Tenant] INT NOT NULL,[StatementTypeCode] VARCHAR(4) NULL,[IsStatementInd] BIT DEFAULT(0) NOT NULL,CONSTRAINT [PK_SuppInvoiceItemsAbachStatement] PRIMARY KEY([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber],[SequenceNumeric]));');


-- Add Foreign Key Constraint For Column DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber In Table SuppInvoiceItemsAbachStatement As Reference To Column DeclarationId,CounterKey,LineNumber In Table SupplierInvoiceItems
EXEC('ALTER TABLE [Customs].[SuppInvoiceItemsAbachStatement] ADD CONSTRAINT [FK_SuppInvoiceItemsAbachStatement_SupplierInvoiceItems_DeclarationId_InvoiceCounterKey_InvoiceItemLineNumber] FOREIGN KEY([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber]) REFERENCES [Customs].[SupplierInvoiceItems]([DeclarationId],[CounterKey],[LineNumber])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('26c9c605-5958-49b0-a309-e0e177c6075e', 'SuppInvoiceItemsAbachStatement.dxml', 'SuppInvoiceItemsAbachStatement', 'DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber In Table SuppInvoiceItemsAbachStatement As Reference To Column DeclarationId,CounterKey,LineNumber In Table SupplierInvoiceItemsEXEC(''ALTER TABLE [Customs].[SuppInvoiceItemsAbachStatement] ADD CONSTRAINT [FK_SuppInvoiceItemsAbachStatement_SupplierInvoiceItems_DeclarationId_InvoiceCounterKey_InvoiceItemLineNumber] FOREIGN KEY([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber]) REFERENCES [Customs].[SupplierInvoiceItems]([DeclarationId],[CounterKey],[LineNumber])'');');

-- Create Index On SuppInvoiceItemsAbachStatement Table
EXEC('CREATE NONCLUSTERED INDEX [IX_SuppInvoiceItemsAbachStatement_DeclarationId_InvoiceCounterKey_InvoiceItemLineNumber] ON [Customs].[SuppInvoiceItemsAbachStatement]([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('388b9632-ae67-4853-bc27-9a50754530c7', 'SuppInvoiceItemsAbachStatement.dxml', 'SuppInvoiceItemsAbachStatement', 'DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber', 'Create Index', GETDATE(), '-- Create Index On SuppInvoiceItemsAbachStatement TableEXEC(''CREATE NONCLUSTERED INDEX [IX_SuppInvoiceItemsAbachStatement_DeclarationId_InvoiceCounterKey_InvoiceItemLineNumber] ON [Customs].[SuppInvoiceItemsAbachStatement]([DeclarationId],[InvoiceCounterKey],[InvoiceItemLineNumber])'');');

-- Add Foreign Key Constraint For Column StatementTypeCode In Table SuppInvoiceItemsAbachStatement As Reference To Column Code In Table NbcDeclarationTypes
EXEC('ALTER TABLE [Customs].[SuppInvoiceItemsAbachStatement] ADD CONSTRAINT [FK_SuppInvoiceItemsAbachStatement_NbcDeclarationTypes_StatementTypeCode] FOREIGN KEY([StatementTypeCode]) REFERENCES [Customs].[NbcDeclarationTypes]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b3e0b65d-0ed5-448a-b5b6-32cd59131216', 'SuppInvoiceItemsAbachStatement.dxml', 'SuppInvoiceItemsAbachStatement', 'StatementTypeCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column StatementTypeCode In Table SuppInvoiceItemsAbachStatement As Reference To Column Code In Table NbcDeclarationTypesEXEC(''ALTER TABLE [Customs].[SuppInvoiceItemsAbachStatement] ADD CONSTRAINT [FK_SuppInvoiceItemsAbachStatement_NbcDeclarationTypes_StatementTypeCode] FOREIGN KEY([StatementTypeCode]) REFERENCES [Customs].[NbcDeclarationTypes]([Code])'');');

-- Create Index On SuppInvoiceItemsAbachStatement Table
EXEC('CREATE NONCLUSTERED INDEX [IX_SuppInvoiceItemsAbachStatement_StatementTypeCode] ON [Customs].[SuppInvoiceItemsAbachStatement]([StatementTypeCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('38c73321-e54d-4c3e-b25c-736bee20d6fa', 'SuppInvoiceItemsAbachStatement.dxml', 'SuppInvoiceItemsAbachStatement', 'StatementTypeCode', 'Create Index', GETDATE(), '-- Create Index On SuppInvoiceItemsAbachStatement TableEXEC(''CREATE NONCLUSTERED INDEX [IX_SuppInvoiceItemsAbachStatement_StatementTypeCode] ON [Customs].[SuppInvoiceItemsAbachStatement]([StatementTypeCode])'');');


