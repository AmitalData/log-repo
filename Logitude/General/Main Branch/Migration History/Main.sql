--2/10/2016

ALTER TABLE [dbo].[Shipments] ALTER COLUMN [ExceptionDescription] [nvarchar](500) NULL
ALTER TABLE [dbo].[Shipments] ALTER COLUMN [ExceptionResolvedDescription] [nvarchar](500) NULL


-----------------------------------------------------------

--4/10/2016

ALTER TABLE [dbo].[Category1] ALTER COLUMN [LocalName] [nvarchar](max) NULL
ALTER TABLE [dbo].[Category2] ALTER COLUMN [LocalName] [nvarchar](max) NULL
ALTER TABLE [dbo].[Category3] ALTER COLUMN [LocalName] [nvarchar](max) NULL
ALTER TABLE [dbo].[Category4] ALTER COLUMN [LocalName] [nvarchar](max) NULL
ALTER TABLE [dbo].[Category5] ALTER COLUMN [LocalName] [nvarchar](max) NULL

----------------------------------------------------------

--9/10/2016
----------------------------------------------------------

--10/10/2016

-----------------------------------------------------------

--17/10/2016

ALTER TABLE [dbo].[Journals] ADD [IsVoided] [bit]
ALTER TABLE [dbo].[Journals] ADD [VoidedBy] [bit]

CREATE TABLE [dbo].[BankCodes] (
    [Id] [nvarchar](128) NOT NULL,
    [Code] [nvarchar](max),
    [EnglishName] [nvarchar](max),
    [SearchFields] [nvarchar](max),
    [Tenant] [int] NOT NULL,
    [LocalName] [nvarchar](max),
    [Inactive] [bit],
    CONSTRAINT [PK_dbo.BankCodes] PRIMARY KEY ([Id])
)

ALTER TABLE [dbo].[AutomaticReconcileMethods] ADD [Inactive] [bit]

CREATE TABLE [dbo].[BankAccounts] (
    [Id] [nvarchar](128) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15),
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15),
    [SearchFields] [nvarchar](max),
    [LocalName] [nvarchar](max),
    [EnglishName] [nvarchar](max),
    [BankId] [nvarchar](128),
    [BranchNumber] [nvarchar](max),
    [AccountNumber] [nvarchar](max),
    [GLAccountId] [varchar](15),
    [DeferredGLAccountId] [varchar](15),
    [IBAN] [nvarchar](max),
    [SwiftCode] [nvarchar](max),
    [BranchAddress] [nvarchar](max),
    [Inactive] [bit],
    CONSTRAINT [PK_dbo.BankAccounts] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[BankAccounts]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[BankAccounts]([UpdatedByUserId])
CREATE INDEX [IX_BankId] ON [dbo].[BankAccounts]([BankId])
CREATE INDEX [IX_GLAccountId] ON [dbo].[BankAccounts]([GLAccountId])
CREATE INDEX [IX_DeferredGLAccountId] ON [dbo].[BankAccounts]([DeferredGLAccountId])
ALTER TABLE [dbo].[BankAccounts] ADD CONSTRAINT [FK_dbo.BankAccounts_dbo.BankCodes_BankId] FOREIGN KEY ([BankId]) REFERENCES [dbo].[BankCodes] ([Id])
ALTER TABLE [dbo].[BankAccounts] ADD CONSTRAINT [FK_dbo.BankAccounts_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[BankAccounts] ADD CONSTRAINT [FK_dbo.BankAccounts_dbo.GLAccounts_DeferredGLAccountId] FOREIGN KEY ([DeferredGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[BankAccounts] ADD CONSTRAINT [FK_dbo.BankAccounts_dbo.GLAccounts_GLAccountId] FOREIGN KEY ([GLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[BankAccounts] ADD CONSTRAINT [FK_dbo.BankAccounts_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])

CREATE TABLE [dbo].[CashBookTypes] (
    [Code] [nvarchar](128) NOT NULL,
    [EnglishName] [nvarchar](max),
    [SearchFields] [nvarchar](max),
    [LocalName] [nvarchar](max),
    [Inactive] [bit],
    CONSTRAINT [PK_dbo.CashBookTypes] PRIMARY KEY ([Code])
)
ALTER TABLE [dbo].[CashBookTypes] DROP CONSTRAINT [PK_dbo.CashBookTypes]
ALTER TABLE [dbo].[CashBookTypes] ALTER COLUMN [Code] [varchar](3) NOT NULL
ALTER TABLE [dbo].[CashBookTypes] ALTER COLUMN [EnglishName] [varchar](45) NOT NULL
ALTER TABLE [dbo].[CashBookTypes] ALTER COLUMN [LocalName] [nvarchar](45) NOT NULL
ALTER TABLE [dbo].[CashBookTypes] ADD CONSTRAINT [PK_dbo.CashBookTypes] PRIMARY KEY ([Code])

ALTER TABLE [dbo].[CashBookTypes] ALTER COLUMN [SearchFields] [nvarchar](200) NULL

ALTER TABLE [dbo].[CashBookTypes] DROP CONSTRAINT [PK_dbo.CashBookTypes]
CREATE TABLE [dbo].[CashBooks] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](1000),
    [EnglishName] [varchar](60) NOT NULL,
    [LocalName] [nvarchar](60) NOT NULL,
    [Inactive] [bit],
    [CurrencyId] [varchar](15) NOT NULL,
    [CashBookTypeCode] [varchar](15) NOT NULL,
    [TotalAmount] [decimal](16, 2),
    [AccountId] [varchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.CashBooks] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[CashBooks]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[CashBooks]([UpdatedByUserId])
CREATE INDEX [IX_CurrencyId] ON [dbo].[CashBooks]([CurrencyId])
CREATE INDEX [IX_CashBookTypeCode] ON [dbo].[CashBooks]([CashBookTypeCode])
CREATE INDEX [IX_AccountId] ON [dbo].[CashBooks]([AccountId])
ALTER TABLE [dbo].[CashBookTypes] ALTER COLUMN [Code] [varchar](15) NOT NULL
ALTER TABLE [dbo].[CashBookTypes] ADD CONSTRAINT [PK_dbo.CashBookTypes] PRIMARY KEY ([Code])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.GLAccounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.CashBookTypes_CashBookTypeCode] FOREIGN KEY ([CashBookTypeCode]) REFERENCES [dbo].[CashBookTypes] ([Code])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currencies] ([Id])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])

CREATE TABLE [dbo].[ARPChequeLines] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [SearchFields] [nvarchar](4000),
    [PaymentId] [varchar](15) NOT NULL,
    [LineNumber] [int],
    [ChequeNumber] [varchar](15) NOT NULL,
    [ValueDate] [datetime] NOT NULL,
    [CurrencyId] [varchar](15) NOT NULL,
    [LocalAmount] [decimal](16, 2) NOT NULL,
    [ForeignAmount] [decimal](16, 2) NOT NULL,
    [BankId] [varchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.ARPChequeLines] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_PaymentId] ON [dbo].[ARPChequeLines]([PaymentId])
CREATE INDEX [IX_BankId] ON [dbo].[ARPChequeLines]([BankId])
CREATE TABLE [dbo].[CashBookLines] (
    [CashBookId] [varchar](15) NOT NULL,
    [ARPChequeId] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [IsDeposited] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.CashBookLines] PRIMARY KEY ([CashBookId], [ARPChequeId])
)
CREATE INDEX [IX_CashBookId] ON [dbo].[CashBookLines]([CashBookId])
CREATE INDEX [IX_ARPChequeId] ON [dbo].[CashBookLines]([ARPChequeId])
ALTER TABLE [dbo].[ARPChequeLines] ADD CONSTRAINT [FK_dbo.ARPChequeLines_dbo.ARPayments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[ARPayments] ([Id])
ALTER TABLE [dbo].[CashBookLines] ADD CONSTRAINT [FK_dbo.CashBookLines_dbo.CashBooks_CashBookId] FOREIGN KEY ([CashBookId]) REFERENCES [dbo].[CashBooks] ([Id])
ALTER TABLE [dbo].[CashBookLines] ADD CONSTRAINT [FK_dbo.CashBookLines_dbo.ARPChequeLines_ARPChequeId] FOREIGN KEY ([ARPChequeId]) REFERENCES [dbo].[ARPChequeLines] ([Id])

ALTER TABLE [dbo].[ARPChequeLines] ADD [BankBranch] [varchar](30) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[ARPChequeLines] ADD [BankAccount] [varchar](15) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[ARPChequeLines] ALTER COLUMN [LineNumber] [int] NOT NULL
CREATE INDEX [IX_CurrencyId] ON [dbo].[ARPChequeLines]([CurrencyId])
ALTER TABLE [dbo].[ARPChequeLines] ADD CONSTRAINT [FK_dbo.ARPChequeLines_dbo.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currencies] ([Id])

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CreatedByUserId' AND object_id = object_id(N'[dbo].[BankAccounts]', N'U'))
    DROP INDEX [IX_CreatedByUserId] ON [dbo].[BankAccounts]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_UpdatedByUserId' AND object_id = object_id(N'[dbo].[BankAccounts]', N'U'))
    DROP INDEX [IX_UpdatedByUserId] ON [dbo].[BankAccounts]
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [CreatedByUserId] [varchar](15) NULL
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [UpdatedByUserId] [varchar](15) NULL
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[BankAccounts]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[BankAccounts]([UpdatedByUserId])

CREATE TABLE [dbo].[ARPaymentCheques] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [SearchFields] [nvarchar](4000),
    [PaymentId] [varchar](15) NOT NULL,
    [LineNumber] [int] NOT NULL,
    [ChequeNumber] [varchar](15) NOT NULL,
    [ValueDate] [datetime] NOT NULL,
    [CurrencyId] [varchar](15) NOT NULL,
    [LocalAmount] [decimal](16, 2) NOT NULL,
    [ForeignAmount] [decimal](16, 2) NOT NULL,
    [BankId] [varchar](15) NOT NULL,
    [BankBranch] [varchar](30) NOT NULL,
    [BankAccount] [varchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.ARPaymentCheques] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_PaymentId] ON [dbo].[ARPaymentCheques]([PaymentId])
CREATE INDEX [IX_CurrencyId] ON [dbo].[ARPaymentCheques]([CurrencyId])
CREATE INDEX [IX_BankId] ON [dbo].[ARPaymentCheques]([BankId])
ALTER TABLE [dbo].[ARPaymentCheques] ADD CONSTRAINT [FK_dbo.ARPaymentCheques_dbo.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currencies] ([Id])
ALTER TABLE [dbo].[ARPaymentCheques] ADD CONSTRAINT [FK_dbo.ARPaymentCheques_dbo.ARPayments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[ARPayments] ([Id])

CREATE TABLE [dbo].[BankDeposits] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](max),
    [DepositNumber] [int] NOT NULL,
    [DepositDate] [datetime] NOT NULL,
    [DepositCurrencyId] [varchar](15) NOT NULL,
    [LocalDepositAmount] [decimal](16, 2) NOT NULL,
    [ForeignAmount] [decimal](16, 2) NOT NULL,
    [DepositBankAccountId] [varchar](15),
    [CashBookId] [varchar](15),
    CONSTRAINT [PK_dbo.BankDeposits] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[BankDeposits]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[BankDeposits]([UpdatedByUserId])
CREATE INDEX [IX_DepositCurrencyId] ON [dbo].[BankDeposits]([DepositCurrencyId])
CREATE INDEX [IX_DepositBankAccountId] ON [dbo].[BankDeposits]([DepositBankAccountId])
CREATE INDEX [IX_CashBookId] ON [dbo].[BankDeposits]([CashBookId])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.BankAccounts_DepositBankAccountId] FOREIGN KEY ([DepositBankAccountId]) REFERENCES [dbo].[BankAccounts] ([Id])

CREATE TABLE [dbo].[BankDeposits] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](max),
    [DepositNumber] [int] NOT NULL,
    [DepositDate] [datetime] NOT NULL,
    [DepositCurrencyId] [varchar](15) NOT NULL,
    [LocalDepositAmount] [decimal](16, 2) NOT NULL,
    [ForeignAmount] [decimal](16, 2) NOT NULL,
    [DepositBankAccountId] [varchar](15),
    [CashBookId] [varchar](15),
    CONSTRAINT [PK_dbo.BankDeposits] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[BankDeposits]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[BankDeposits]([UpdatedByUserId])
CREATE INDEX [IX_DepositCurrencyId] ON [dbo].[BankDeposits]([DepositCurrencyId])
CREATE INDEX [IX_DepositBankAccountId] ON [dbo].[BankDeposits]([DepositBankAccountId])
CREATE INDEX [IX_CashBookId] ON [dbo].[BankDeposits]([CashBookId])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.BankAccounts_DepositBankAccountId] FOREIGN KEY ([DepositBankAccountId]) REFERENCES [dbo].[BankAccounts] ([Id])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.CashBooks_CashBookId] FOREIGN KEY ([CashBookId]) REFERENCES [dbo].[CashBooks] ([Id])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.Currencies_DepositCurrencyId] FOREIGN KEY ([DepositCurrencyId]) REFERENCES [dbo].[Currencies] ([Id])
ALTER TABLE [dbo].[BankDeposits] ADD CONSTRAINT [FK_dbo.BankDeposits_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])

CREATE TABLE [dbo].[BankDepositLines] (
    [DepositId] [varchar](15) NOT NULL,
    [Line] [int] NOT NULL,
    [Tenant] [int] NOT NULL,
    [ARPaymentChequeId] [varchar](15) NOT NULL,
    [IsOutOfDeposit] [bit],
    [OutOfDepositeDate] [datetime] NOT NULL,
    [Notes] [nvarchar](100),
    CONSTRAINT [PK_dbo.BankDepositLines] PRIMARY KEY ([DepositId], [Line])
)
CREATE INDEX [IX_DepositId] ON [dbo].[BankDepositLines]([DepositId])
CREATE INDEX [IX_ARPaymentChequeId] ON [dbo].[BankDepositLines]([ARPaymentChequeId])
ALTER TABLE [dbo].[BankDepositLines] ADD CONSTRAINT [FK_dbo.BankDepositLines_dbo.ARPaymentCheques_ARPaymentChequeId] FOREIGN KEY ([ARPaymentChequeId]) REFERENCES [dbo].[ARPaymentCheques] ([Id])
ALTER TABLE [dbo].[BankDepositLines] ADD CONSTRAINT [FK_dbo.BankDepositLines_dbo.BankDeposits_DepositId] FOREIGN KEY ([DepositId]) REFERENCES [dbo].[BankDeposits] ([Id])

ALTER TABLE [dbo].[BankDeposits] ADD [AccountingDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'

ALTER TABLE [dbo].[BankDepositLines] ALTER COLUMN [OutOfDepositeDate] [datetime] NULL

ALTER TABLE [dbo].[GLAccounts] ADD [ParentAccountId] [varchar](15)
CREATE INDEX [IX_ParentAccountId] ON [dbo].[GLAccounts]([ParentAccountId])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.GLAccounts_ParentAccountId] FOREIGN KEY ([ParentAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CreatedByUserId' AND object_id = object_id(N'[dbo].[BankAccounts]', N'U'))
    DROP INDEX [IX_CreatedByUserId] ON [dbo].[BankAccounts]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_UpdatedByUserId' AND object_id = object_id(N'[dbo].[BankAccounts]', N'U'))
    DROP INDEX [IX_UpdatedByUserId] ON [dbo].[BankAccounts]
CREATE TABLE [dbo].[GLAccountCurrencies] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [GLAccountId] [varchar](15) NOT NULL,
    [CurrencyId] [varchar](15) NOT NULL,
    [CustomerGLAccountId] [varchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.GLAccountCurrencies] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CurrencyId] ON [dbo].[GLAccountCurrencies]([CurrencyId])
CREATE INDEX [IX_CustomerGLAccountId] ON [dbo].[GLAccountCurrencies]([CustomerGLAccountId])
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [CreatedByUserId] [varchar](15) NULL
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [UpdatedByUserId] [varchar](15) NULL
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[BankAccounts]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[BankAccounts]([UpdatedByUserId])
ALTER TABLE [dbo].[GLAccountCurrencies] ADD CONSTRAINT [FK_dbo.GLAccountCurrencies_dbo.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currencies] ([Id])
ALTER TABLE [dbo].[GLAccountCurrencies] ADD CONSTRAINT [FK_dbo.GLAccountCurrencies_dbo.GLAccounts_CustomerGLAccountId] FOREIGN KEY ([CustomerGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

CREATE INDEX [IX_GLAccountId] ON [dbo].[GLAccountCurrencies]([GLAccountId])
ALTER TABLE [dbo].[GLAccountCurrencies] ADD CONSTRAINT [FK_dbo.GLAccountCurrencies_dbo.GLAccounts_GLAccountId] FOREIGN KEY ([GLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[GLAccounts] ADD [CustomerGLAccountId] [varchar](15)
ALTER TABLE [dbo].[GLAccounts] ADD [BalanceInLocalCurrency] [decimal](16, 2)
CREATE INDEX [IX_CustomerGLAccountId] ON [dbo].[GLAccounts]([CustomerGLAccountId])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.GLAccounts_CustomerGLAccountId] FOREIGN KEY ([CustomerGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[BankAccounts] ADD [ChequeCounter] [int]

ALTER TABLE [dbo].[AutomaticReconcileMethods] ADD [SearchFields] [nvarchar](1000)

CREATE TABLE [dbo].[FullAccountingSettings] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](max),
    [DeductionFileNumber] [varchar](30),
    [ConsolidationVAT] [varchar](30),
    [DefaultVATTypeId] [varchar](15) NOT NULL,
    [VATInputsGLAccountId] [varchar](15) NOT NULL,
    [AutomaticReconcileMethodId] [varchar](15) NOT NULL,
    [ExchangeRateDiffGLAccountId] [varchar](15) NOT NULL,
    [RevenueExpenseGLAccountId] [varchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.FullAccountingSettings] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[FullAccountingSettings]([UpdatedByUserId])
CREATE INDEX [IX_DefaultVATTypeId] ON [dbo].[FullAccountingSettings]([DefaultVATTypeId])
CREATE INDEX [IX_VATInputsGLAccountId] ON [dbo].[FullAccountingSettings]([VATInputsGLAccountId])
CREATE INDEX [IX_AutomaticReconcileMethodId] ON [dbo].[FullAccountingSettings]([AutomaticReconcileMethodId])
CREATE INDEX [IX_ExchangeRateDiffGLAccountId] ON [dbo].[FullAccountingSettings]([ExchangeRateDiffGLAccountId])
CREATE INDEX [IX_RevenueExpenseGLAccountId] ON [dbo].[FullAccountingSettings]([RevenueExpenseGLAccountId])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.AutomaticReconcileMethods_AutomaticReconcileMethodId] FOREIGN KEY ([AutomaticReconcileMethodId]) REFERENCES [dbo].[AutomaticReconcileMethods] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.VatTypes_DefaultVATTypeId] FOREIGN KEY ([DefaultVATTypeId]) REFERENCES [dbo].[VatTypes] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_ExchangeRateDiffGLAccountId] FOREIGN KEY ([ExchangeRateDiffGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_RevenueExpenseGLAccountId] FOREIGN KEY ([RevenueExpenseGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_VATInputsGLAccountId] FOREIGN KEY ([VATInputsGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[FullAccountingSettings] ADD [VATOutputGLAccountId] [varchar](15) NOT NULL DEFAULT ''
CREATE INDEX [IX_VATOutputGLAccountId] ON [dbo].[FullAccountingSettings]([VATOutputGLAccountId])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_VATOutputGLAccountId] FOREIGN KEY ([VATOutputGLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

IF object_id(N'[dbo].[FK_dbo.FullAccountingSettings_dbo.Users_UpdatedByUserId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[FullAccountingSettings] DROP CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.Users_UpdatedByUserId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_UpdatedByUserId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_UpdatedByUserId] ON [dbo].[FullAccountingSettings]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FullAccountingSettings')
AND col_name(parent_object_id, parent_column_id) = 'UpdateDate';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FullAccountingSettings] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[FullAccountingSettings] DROP COLUMN [UpdateDate]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FullAccountingSettings')
AND col_name(parent_object_id, parent_column_id) = 'UpdatedByUserId';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FullAccountingSettings] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[FullAccountingSettings] DROP COLUMN [UpdatedByUserId]
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FullAccountingSettings')
AND col_name(parent_object_id, parent_column_id) = 'SearchFields';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FullAccountingSettings] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[FullAccountingSettings] DROP COLUMN [SearchFields]

IF object_id(N'[dbo].[FK_dbo.GLAccounts_dbo.GLAccounts_CustomerGLAccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [FK_dbo.GLAccounts_dbo.GLAccounts_CustomerGLAccountId]
IF object_id(N'[dbo].[FK_dbo.GLAccounts_dbo.GLAccounts_ParentAccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [FK_dbo.GLAccounts_dbo.GLAccounts_ParentAccountId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ParentAccountId' AND object_id = object_id(N'[dbo].[GLAccounts]', N'U'))
    DROP INDEX [IX_ParentAccountId] ON [dbo].[GLAccounts]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CustomerGLAccountId' AND object_id = object_id(N'[dbo].[GLAccounts]', N'U'))
    DROP INDEX [IX_CustomerGLAccountId] ON [dbo].[GLAccounts]
ALTER TABLE [dbo].[GLAccounts] ADD [RevaluationEnabled] [bit]

DECLARE @var3 nvarchar(128)
SELECT @var3 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.GLAccounts')
AND col_name(parent_object_id, parent_column_id) = 'ParentAccountId';
IF @var3 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [' + @var3 + ']')
ALTER TABLE [dbo].[GLAccounts] DROP COLUMN [ParentAccountId]

CREATE TABLE [dbo].[Revaluations] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](max),
    [RevaluationNumber] [int] NOT NULL,
    [RevaluationDate] [datetime] NOT NULL,
    [ChartOfAccountsId] [varchar](15),
    [GLAccountId] [varchar](15),
    [RevaluationEnabled] [bit],
    CONSTRAINT [PK_dbo.Revaluations] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[Revaluations]([CreatedByUserId])
CREATE INDEX [IX_ChartOfAccountsId] ON [dbo].[Revaluations]([ChartOfAccountsId])
CREATE INDEX [IX_GLAccountId] ON [dbo].[Revaluations]([GLAccountId])
ALTER TABLE [dbo].[Revaluations] ADD CONSTRAINT [FK_dbo.Revaluations_dbo.ChartOfAccounts_ChartOfAccountsId] FOREIGN KEY ([ChartOfAccountsId]) REFERENCES [dbo].[ChartOfAccounts] ([Id])
ALTER TABLE [dbo].[Revaluations] ADD CONSTRAINT [FK_dbo.Revaluations_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[Revaluations] ADD CONSTRAINT [FK_dbo.Revaluations_dbo.GLAccounts_GLAccountId] FOREIGN KEY ([GLAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[GLAccounts] ADD [ParentAccountId] [varchar](15)


ALTER TABLE [dbo].[Journals] ADD [ExternalSystem] [nvarchar](60)

ALTER TABLE [dbo].[LedgerTransactions] ADD [IsReconciled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Reconciliations] ADD [IsCancelled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[FullAccountingSettings] ADD [CustomerControlAccountId] [varchar](15)
ALTER TABLE [dbo].[FullAccountingSettings] ADD [VendorControlAccountId] [varchar](15)
ALTER TABLE [dbo].[FullAccountingSettings] ADD [FileControlAccountId] [varchar](15)
CREATE INDEX [IX_CustomerControlAccountId] ON [dbo].[FullAccountingSettings]([CustomerControlAccountId])
CREATE INDEX [IX_VendorControlAccountId] ON [dbo].[FullAccountingSettings]([VendorControlAccountId])
CREATE INDEX [IX_FileControlAccountId] ON [dbo].[FullAccountingSettings]([FileControlAccountId])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_CustomerControlAccountId] FOREIGN KEY ([CustomerControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_FileControlAccountId] FOREIGN KEY ([FileControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_VendorControlAccountId] FOREIGN KEY ([VendorControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[FullAccountingSettings] ADD [JobControlAccountId] [varchar](15)
CREATE INDEX [IX_JobControlAccountId] ON [dbo].[FullAccountingSettings]([JobControlAccountId])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_JobControlAccountId] FOREIGN KEY ([JobControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

ALTER TABLE [dbo].[JournalLines] ADD [ExternalOpenAmount] [decimal](16, 2)

IF object_id(N'[dbo].[FK_dbo.GLAccountBalancesByYear_dbo.Currencies_CurrencyId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountBalancesByYear] DROP CONSTRAINT [FK_dbo.GLAccountBalancesByYear_dbo.Currencies_CurrencyId]
IF object_id(N'[dbo].[FK_dbo.GLAccountBalancesByYear_dbo.GLAccounts_AccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountBalancesByYear] DROP CONSTRAINT [FK_dbo.GLAccountBalancesByYear_dbo.GLAccounts_AccountId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AccountId' AND object_id = object_id(N'[dbo].[GLAccountBalancesByYear]', N'U'))
    DROP INDEX [IX_AccountId] ON [dbo].[GLAccountBalancesByYear]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CurrencyId' AND object_id = object_id(N'[dbo].[GLAccountBalancesByYear]', N'U'))
    DROP INDEX [IX_CurrencyId] ON [dbo].[GLAccountBalancesByYear]
DROP TABLE [dbo].[GLAccountBalancesByYear]

----------
--18/10/2016


ALTER TABLE [dbo].[Users] ADD [SetAngularAsDefault] [bit] NOT NULL DEFAULT 0

--------------------- 6/12/2016----------------------------------
-------------------------2016R5----------------------------------


ALTER TABLE [dbo].[OceanInsightsRequests] ALTER COLUMN [BLNumber] [varchar](18) NULL



IF object_id(N'[dbo].[FK_dbo.GLAccountBalancesByYear_dbo.Currencies_CurrencyId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountBalancesByYear] DROP CONSTRAINT [FK_dbo.GLAccountBalancesByYear_dbo.Currencies_CurrencyId]
IF object_id(N'[dbo].[FK_dbo.GLAccountBalancesByYear_dbo.GLAccounts_AccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountBalancesByYear] DROP CONSTRAINT [FK_dbo.GLAccountBalancesByYear_dbo.GLAccounts_AccountId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AccountId' AND object_id = object_id(N'[dbo].[GLAccountBalancesByYear]', N'U'))
    DROP INDEX [IX_AccountId] ON [dbo].[GLAccountBalancesByYear]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CurrencyId' AND object_id = object_id(N'[dbo].[GLAccountBalancesByYear]', N'U'))
    DROP INDEX [IX_CurrencyId] ON [dbo].[GLAccountBalancesByYear]
ALTER TABLE [dbo].[GLAccounts] ADD [Category2Id] [varchar](15)
ALTER TABLE [dbo].[GLAccounts] ADD [Category3Id] [varchar](15)
ALTER TABLE [dbo].[GLAccounts] ADD [Category4Id] [varchar](15)
ALTER TABLE [dbo].[GLAccounts] ADD [Category5Id] [varchar](15)
ALTER TABLE [dbo].[GLAccounts] ADD [Category1Id] [varchar](15)
CREATE INDEX [IX_Category2Id] ON [dbo].[GLAccounts]([Category2Id])
CREATE INDEX [IX_Category3Id] ON [dbo].[GLAccounts]([Category3Id])
CREATE INDEX [IX_Category4Id] ON [dbo].[GLAccounts]([Category4Id])
CREATE INDEX [IX_Category5Id] ON [dbo].[GLAccounts]([Category5Id])
CREATE INDEX [IX_Category1Id] ON [dbo].[GLAccounts]([Category1Id])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.Category1_Category1Id] FOREIGN KEY ([Category1Id]) REFERENCES [dbo].[Category1] ([Id])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.Category2_Category2Id] FOREIGN KEY ([Category2Id]) REFERENCES [dbo].[Category2] ([Id])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.Category3_Category3Id] FOREIGN KEY ([Category3Id]) REFERENCES [dbo].[Category3] ([Id])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.Category4_Category4Id] FOREIGN KEY ([Category4Id]) REFERENCES [dbo].[Category4] ([Id])
ALTER TABLE [dbo].[GLAccounts] ADD CONSTRAINT [FK_dbo.GLAccounts_dbo.Category5_Category5Id] FOREIGN KEY ([Category5Id]) REFERENCES [dbo].[Category5] ([Id])

EXECUTE sp_rename @objname = N'dbo.FullAccountingSettings.JobControlAccountId', @newname = N'AirExportJobControlAccountId', @objtype = N'COLUMN'
EXECUTE sp_rename @objname = N'dbo.FullAccountingSettings.IX_JobControlAccountId', @newname = N'IX_AirExportJobControlAccountId', @objtype = N'INDEX'
ALTER TABLE [dbo].[FullAccountingSettings] ADD [OceanExportJobControlAccountId] [varchar](15)
ALTER TABLE [dbo].[FullAccountingSettings] ADD [OceanImportJobControlAccountId] [varchar](15)
ALTER TABLE [dbo].[FullAccountingSettings] ADD [AirImportJobControlAccountId] [varchar](15)
CREATE INDEX [IX_OceanExportJobControlAccountId] ON [dbo].[FullAccountingSettings]([OceanExportJobControlAccountId])
CREATE INDEX [IX_OceanImportJobControlAccountId] ON [dbo].[FullAccountingSettings]([OceanImportJobControlAccountId])
CREATE INDEX [IX_AirImportJobControlAccountId] ON [dbo].[FullAccountingSettings]([AirImportJobControlAccountId])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_AirImportJobControlAccountId] FOREIGN KEY ([AirImportJobControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_OceanExportJobControlAccountId] FOREIGN KEY ([OceanExportJobControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])
ALTER TABLE [dbo].[FullAccountingSettings] ADD CONSTRAINT [FK_dbo.FullAccountingSettings_dbo.GLAccounts_OceanImportJobControlAccountId] FOREIGN KEY ([OceanImportJobControlAccountId]) REFERENCES [dbo].[GLAccounts] ([Id])

IF object_id(N'[dbo].[FK_dbo.GLAccounts_dbo.Customers_ClientId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [FK_dbo.GLAccounts_dbo.Customers_ClientId]
IF object_id(N'[dbo].[FK_dbo.GLAccounts_dbo.Vendors_VendorId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [FK_dbo.GLAccounts_dbo.Vendors_VendorId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ClientId' AND object_id = object_id(N'[dbo].[GLAccounts]', N'U'))
    DROP INDEX [IX_ClientId] ON [dbo].[GLAccounts]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_VendorId' AND object_id = object_id(N'[dbo].[GLAccounts]', N'U'))
    DROP INDEX [IX_VendorId] ON [dbo].[GLAccounts]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.GLAccounts')
AND col_name(parent_object_id, parent_column_id) = 'ClientId';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[GLAccounts] DROP COLUMN [ClientId]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.GLAccounts')
AND col_name(parent_object_id, parent_column_id) = 'VendorId';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[GLAccounts] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[GLAccounts] DROP COLUMN [VendorId]

ALTER TABLE [dbo].[BlobFiles] DROP CONSTRAINT [PK_dbo.BlobFiles]
ALTER TABLE [dbo].[BlobFiles] ALTER COLUMN [Id] [varchar](200) NOT NULL
ALTER TABLE [dbo].[BlobFiles] ADD CONSTRAINT [PK_dbo.BlobFiles] PRIMARY KEY ([Id])

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_DefaultVATTypeId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_DefaultVATTypeId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_VATInputsGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_VATInputsGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AutomaticReconcileMethodId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_AutomaticReconcileMethodId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ExchangeRateDiffGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_ExchangeRateDiffGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_RevenueExpenseGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_RevenueExpenseGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_VATOutputGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_VATOutputGLAccountId] ON [dbo].[FullAccountingSettings]
ALTER TABLE [dbo].[Category1] ADD [SearchFields] [nvarchar](1000)
ALTER TABLE [dbo].[Category2] ADD [SearchFields] [nvarchar](1000)
ALTER TABLE [dbo].[Category3] ADD [SearchFields] [nvarchar](1000)
ALTER TABLE [dbo].[Category4] ADD [SearchFields] [nvarchar](1000)
ALTER TABLE [dbo].[Category5] ADD [SearchFields] [nvarchar](1000)


IF object_id(N'[dbo].[FK_dbo.GLAccountTotalsByMonth_dbo.Currencies_CurrencyId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountTotalsByMonth] DROP CONSTRAINT [FK_dbo.GLAccountTotalsByMonth_dbo.Currencies_CurrencyId]
IF object_id(N'[dbo].[FK_dbo.GLAccountTotalsByMonth_dbo.GLAccounts_AccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountTotalsByMonth] DROP CONSTRAINT [FK_dbo.GLAccountTotalsByMonth_dbo.GLAccounts_AccountId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AccountId' AND object_id = object_id(N'[dbo].[GLAccountTotalsByMonth]', N'U'))
    DROP INDEX [IX_AccountId] ON [dbo].[GLAccountTotalsByMonth]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CurrencyId' AND object_id = object_id(N'[dbo].[GLAccountTotalsByMonth]', N'U'))
    DROP INDEX [IX_CurrencyId] ON [dbo].[GLAccountTotalsByMonth]
DROP TABLE [dbo].[GLAccountTotalsByMonth]


IF object_id(N'[dbo].[FK_dbo.GLAccountTotalsByMonth_dbo.Currencies_CurrencyId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountTotalsByMonth] DROP CONSTRAINT [FK_dbo.GLAccountTotalsByMonth_dbo.Currencies_CurrencyId]
IF object_id(N'[dbo].[FK_dbo.GLAccountTotalsByMonth_dbo.GLAccounts_AccountId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[GLAccountTotalsByMonth] DROP CONSTRAINT [FK_dbo.GLAccountTotalsByMonth_dbo.GLAccounts_AccountId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AccountId' AND object_id = object_id(N'[dbo].[GLAccountTotalsByMonth]', N'U'))
    DROP INDEX [IX_AccountId] ON [dbo].[GLAccountTotalsByMonth]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CurrencyId' AND object_id = object_id(N'[dbo].[GLAccountTotalsByMonth]', N'U'))
    DROP INDEX [IX_CurrencyId] ON [dbo].[GLAccountTotalsByMonth]
ALTER TABLE [dbo].[BlobFiles] DROP CONSTRAINT [PK_dbo.BlobFiles]
ALTER TABLE [dbo].[QuoteTemplateSettings] ADD [ShowChargeDescriptionPackages] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[QuoteTemplateSettings] ADD [ShowChargeDescriptionContainers] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[BlobFiles] ALTER COLUMN [Id] [varchar](200) NOT NULL
ALTER TABLE [dbo].[OceanInsightsRequests] ALTER COLUMN [BLNumber] [varchar](18) NULL
ALTER TABLE [dbo].[BlobFiles] ADD CONSTRAINT [PK_dbo.BlobFiles] PRIMARY KEY ([Id])

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_DefaultVATTypeId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_DefaultVATTypeId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_VATInputsGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_VATInputsGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_AutomaticReconcileMethodId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_AutomaticReconcileMethodId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ExchangeRateDiffGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_ExchangeRateDiffGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_RevenueExpenseGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_RevenueExpenseGLAccountId] ON [dbo].[FullAccountingSettings]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_VATOutputGLAccountId' AND object_id = object_id(N'[dbo].[FullAccountingSettings]', N'U'))
    DROP INDEX [IX_VATOutputGLAccountId] ON [dbo].[FullAccountingSettings]
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [DefaultVATTypeId] [varchar](15) NULL
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [VATInputsGLAccountId] [varchar](15) NULL
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [AutomaticReconcileMethodId] [varchar](15) NULL
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [ExchangeRateDiffGLAccountId] [varchar](15) NULL
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [RevenueExpenseGLAccountId] [varchar](15) NULL
ALTER TABLE [dbo].[FullAccountingSettings] ALTER COLUMN [VATOutputGLAccountId] [varchar](15) NULL
CREATE INDEX [IX_DefaultVATTypeId] ON [dbo].[FullAccountingSettings]([DefaultVATTypeId])
CREATE INDEX [IX_VATInputsGLAccountId] ON [dbo].[FullAccountingSettings]([VATInputsGLAccountId])
CREATE INDEX [IX_AutomaticReconcileMethodId] ON [dbo].[FullAccountingSettings]([AutomaticReconcileMethodId])
CREATE INDEX [IX_ExchangeRateDiffGLAccountId] ON [dbo].[FullAccountingSettings]([ExchangeRateDiffGLAccountId])
CREATE INDEX [IX_RevenueExpenseGLAccountId] ON [dbo].[FullAccountingSettings]([RevenueExpenseGLAccountId])
CREATE INDEX [IX_VATOutputGLAccountId] ON [dbo].[FullAccountingSettings]([VATOutputGLAccountId])

CREATE TABLE [dbo].[ARInvoiceLineActions] (
    [Code] [varchar](1) NOT NULL,
    [Name] [varchar](20) NOT NULL,
    [LocalName] [nvarchar](50) NOT NULL,
    [Inactvie] [bit] NOT NULL,
    [SearchFields] [nvarchar](1000),
    CONSTRAINT [PK_dbo.ARInvoiceLineActions] PRIMARY KEY ([Code])
)
ALTER TABLE [dbo].[Cards] ADD [GLAccountId] [varchar](15)
ALTER TABLE [dbo].[ChargesTypes] ADD [PayableDebitGLAcountId] [varchar](15)
ALTER TABLE [dbo].[ChargesTypes] ADD [ReceivableCreditGLAccountId] [varchar](15)
ALTER TABLE [dbo].[ARInvoices] ADD [DateForVATInterest] [datetime]
ALTER TABLE [dbo].[ARInvoices] ADD [SplitJournalByCurrency] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ARInvoices] ADD [IsExternalEntity] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ARInvoiceLines] ADD [DateForInterest] [datetime]
ALTER TABLE [dbo].[ARInvoiceLines] ADD [ValueDate] [datetime]
ALTER TABLE [dbo].[ARInvoiceLines] ADD [GLAccountId] [varchar](15)
ALTER TABLE [dbo].[ARInvoiceLines] ADD [LineActionCode] [varchar](1)
ALTER TABLE [dbo].[ChargeTypeAccountings] ADD [PayableDebitGLAcountId] [nvarchar](max)
ALTER TABLE [dbo].[ChargeTypeAccountings] ADD [ReceivableCreditGLAccountId] [nvarchar](max)
CREATE INDEX [IX_LineActionCode] ON [dbo].[ARInvoiceLines]([LineActionCode])
ALTER TABLE [dbo].[ARInvoiceLines] ADD CONSTRAINT [FK_dbo.ARInvoiceLines_dbo.ARInvoiceLineActions_LineActionCode] FOREIGN KEY ([LineActionCode]) REFERENCES [dbo].[ARInvoiceLineActions] ([Code])

EXECUTE sp_rename @objname = N'dbo.ARInvoiceLineActions.Inactvie', @newname = N'Inactive', @objtype = N'COLUMN'

ALTER TABLE [dbo].[APInvoices] ADD [VendoeGLAccountId] [varchar](15)
ALTER TABLE [dbo].[APInvoices] ADD [AccountingDate] [datetime]
ALTER TABLE [dbo].[APInvoices] ADD [IsExternalEntity] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[APInvoiceLines] ADD [ChargeTypeGLAccountId] [varchar](15)
ALTER TABLE [dbo].[APInvoiceLines] ADD [AuthorizedSignatory] [bit] NOT NULL DEFAULT 0

CREATE TABLE [dbo].[GLAccountTotalByMonths] (
    [AccountId] [varchar](15) NOT NULL,
    [Year] [int] NOT NULL,
    [Month] [int] NOT NULL,
    [CurrencyId] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [LocalAmountDebit] [decimal](18, 2) NOT NULL,
    [LocalAmountCredit] [decimal](18, 2) NOT NULL,
    [ForeignAmountDebit] [decimal](18, 2),
    [ForeignAmountCredit] [decimal](18, 2),
    CONSTRAINT [PK_dbo.GLAccountTotalByMonths] PRIMARY KEY ([AccountId], [Year], [Month], [CurrencyId])
)
CREATE INDEX [IX_AccountId] ON [dbo].[GLAccountTotalByMonths]([AccountId])
CREATE INDEX [IX_CurrencyId] ON [dbo].[GLAccountTotalByMonths]([CurrencyId])
ALTER TABLE [dbo].[GLAccountTotalByMonths] ADD CONSTRAINT [FK_dbo.GLAccountTotalByMonths_dbo.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currencies] ([Id])
ALTER TABLE [dbo].[GLAccountTotalByMonths] ADD CONSTRAINT [FK_dbo.GLAccountTotalByMonths_dbo.GLAccounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[GLAccounts] ([Id])



CREATE TABLE [dbo].[PeriodTypes](
      [Code] [varchar](1) NOT NULL,
      [EnglishName] [varchar](30) NOT NULL,
      [LocalName] [nvarchar](50) NULL,
      [SearchFields] [nvarchar](200) NULL,
      [Inactive] [bit] NOT NULL,
CONSTRAINT [PK_dbo.PeriodTypes] PRIMARY KEY CLUSTERED 
(
      [Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--------------------

ALTER TABLE [dbo].[PeriodTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[AutomaticReconciles] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[ChartOfAccountsTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[GLAccountTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[ReconcileMethods] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[RevenueExpenseTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[JournalStatusTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[JournalTypes] ALTER COLUMN [Inactive] [bit] NOT NULL
ALTER TABLE [dbo].[JournalActionTypes] ALTER COLUMN [Inactive] [bit] NOT NULL

ALTER TABLE [dbo].[Journals] ADD [QueueId] [varchar](15)

ALTER TABLE [dbo].[Shipments] ALTER COLUMN [MainHarmonize] [varchar](18) NULL

ALTER TABLE [dbo].[LedgerTransactions] ALTER COLUMN [LocalAmountDebit] [decimal](16, 2) NULL
ALTER TABLE [dbo].[LedgerTransactions] ALTER COLUMN [LocalAmountCredit] [decimal](16, 2) NULL
ALTER TABLE [dbo].[LedgerTransactions] ALTER COLUMN [ForeignAmountDebit] [decimal](16, 2) NULL
ALTER TABLE [dbo].[LedgerTransactions] ALTER COLUMN [ForeignAmountCredit] [decimal](16, 2) NULL

ALTER TABLE [dbo].[GLAccounts] ADD [IsVATExempt] [bit]

ALTER TABLE [dbo].[GLAccounts] ALTER COLUMN [LocalName] [nvarchar](200) NOT NULL

ALTER TABLE [dbo].[CashBooks] ADD [BranchId] [varchar](15)
CREATE INDEX [IX_BranchId] ON [dbo].[CashBooks]([BranchId])
ALTER TABLE [dbo].[CashBooks] ADD CONSTRAINT [FK_dbo.CashBooks_dbo.Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branches] ([Id])

IF object_id(N'[dbo].[FK_dbo.CashBookLines_dbo.ARPChequeLines_ARPChequeId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[CashBookLines] DROP CONSTRAINT [FK_dbo.CashBookLines_dbo.ARPChequeLines_ARPChequeId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ARPChequeId' AND object_id = object_id(N'[dbo].[CashBookLines]', N'U'))
    DROP INDEX [IX_ARPChequeId] ON [dbo].[CashBookLines]
DROP TABLE [dbo].[ARPChequeLines]
ALTER TABLE [dbo].[CashBookLines] ADD CONSTRAINT [FK_dbo.CashBookLines_dbo.ARPaymentCheques_ARPChequeId] FOREIGN KEY ([ARPChequeId]) REFERENCES [dbo].[ARPaymentCheques] ([Id])
CREATE INDEX [IX_ARPChequeId] ON [dbo].[CashBookLines]([ARPChequeId])

ALTER TABLE [dbo].[AccountingSystems] ADD [IsExternalCodesFromAPI] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Journals] ADD [VoidedByJournalId] [varchar](15)
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Journals')
AND col_name(parent_object_id, parent_column_id) = 'VoidedBy';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Journals] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Journals] DROP COLUMN [VoidedBy]

ALTER TABLE [dbo].[BankCodes] ALTER COLUMN [Code] [varchar](15) NOT NULL

CREATE TABLE [dbo].[ReconciliationLines](
      [ReconciliationId] [varchar](15) NOT NULL,
      [Line] [int] NOT NULL,
      [Tenant] [int] NOT NULL,
      [CurrencyId] [varchar](15) NOT NULL,
      [TransactionId] [varchar](15) NOT NULL,
      [ReconciliationAmount] [decimal](16, 2) NOT NULL,
      [IsPartial] [bit] NOT NULL,
      [GroupNumber] [int] NOT NULL,
      [IsAdjustTransaction] [bit] NOT NULL,
CONSTRAINT [PK_dbo.ReconciliationLines] PRIMARY KEY CLUSTERED 
(
      [ReconciliationId] ASC,
      [Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

----------------------

ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [EnglishName] [varchar](30) NULL
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [BranchAddress] [nvarchar](60) NULL

CREATE TABLE [dbo].[ARPaymentChequeStatuses] (
    [Code] [varchar](15) NOT NULL,
    [SearchFields] [nvarchar](max),
    [LocalName] [nvarchar](60),
    [EnglishName] [nvarchar](60),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.ARPaymentChequeStatuses] PRIMARY KEY ([Code])
)

ALTER TABLE [dbo].[ARPaymentCheques] ADD [StatusCode] [varchar](15)
CREATE INDEX [IX_StatusCode] ON [dbo].[ARPaymentCheques]([StatusCode])
ALTER TABLE [dbo].[ARPaymentCheques] ADD CONSTRAINT [FK_dbo.ARPaymentCheques_dbo.ARPaymentChequeStatuses_StatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[ARPaymentChequeStatuses] ([Code])


IF object_id(N'[dbo].[FK_dbo.ARPaymentCheques_dbo.BankCodes_BankId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[ARPaymentCheques] DROP CONSTRAINT [FK_dbo.ARPaymentCheques_dbo.BankCodes_BankId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_BankId' AND object_id = object_id(N'[dbo].[ARPaymentCheques]', N'U'))
    DROP INDEX [IX_BankId] ON [dbo].[ARPaymentCheques]

ALTER TABLE [dbo].[ARPaymentCheques] ALTER COLUMN [BankId] [varchar](15) NULL

ALTER TABLE [dbo].[ARPaymentChequeStatuses] ALTER COLUMN [EnglishName] [varchar](60) NULL

ALTER TABLE [dbo].[Shipments] ADD [LastExceptionDescription] [nvarchar](2000)

ALTER TABLE [dbo].[ARPayments] ADD [BankAccountId] [nvarchar](15)


-----14/12/2016

IF object_id(N'[dbo].[FK_dbo.QueueMessageMoreDetails_dbo.QueueMessages_Id]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[QueueMessageMoreDetails] DROP CONSTRAINT [FK_dbo.QueueMessageMoreDetails_dbo.QueueMessages_Id]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Id' AND object_id = object_id(N'[dbo].[QueueMessageMoreDetails]', N'U'))
    DROP INDEX [IX_Id] ON [dbo].[QueueMessageMoreDetails]
ALTER TABLE [dbo].[QueueMessageMoreDetails] DROP CONSTRAINT [PK_dbo.QueueMessageMoreDetails]
ALTER TABLE [dbo].[QueueMessageMoreDetails] ALTER COLUMN [Id] [bigint] NOT NULL
ALTER TABLE [dbo].[QueueMessageMoreDetails] ADD CONSTRAINT [PK_dbo.QueueMessageMoreDetails] PRIMARY KEY ([Id])


ALTER TABLE [dbo].[ObjectTables] ADD [UpdateKey] [varchar](100)


CREATE TABLE [dbo].[CustomerFieldsUpdateSettings] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [ObjectFieldId] [varchar](15) NOT NULL,
    [UpdateDirection] [varchar](20),
    CONSTRAINT [PK_dbo.CustomerFieldsUpdateSettings] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ObjectFieldId] ON [dbo].[CustomerFieldsUpdateSettings]([ObjectFieldId])
ALTER TABLE [dbo].[CustomerFieldsUpdateSettings] ADD CONSTRAINT [FK_dbo.CustomerFieldsUpdateSettings_dbo.ObjectFields_ObjectFieldId] FOREIGN KEY ([ObjectFieldId]) REFERENCES [dbo].[ObjectFields] ([Id])

ALTER TABLE [dbo].[BankDeposits] ADD [IsCanceled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Features] ALTER COLUMN [Code] [varchar](50) NOT NULL

ALTER TABLE [dbo].[ObjectFields] ADD [AllowedInCustomerFieldsSettings] [bit] NOT NULL DEFAULT 0



----18/12/2016
EXECUTE sp_rename @objname = N'dbo.Tenants.IsRightToLeftEnabled', @newname = N'IsCorrespondenceRightToLeftEnabled', @objtype = N'COLUMN'

ALTER TABLE [dbo].[Tenants] ADD [IsNotesRightToLeftEnabled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Activities] ADD [DescriptionRightToLeft] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Activities] ADD [MeetingSummaryRightToLeft] [bit] NOT NULL DEFAULT 0

---

ALTER TABLE [dbo].[GLAccounts] ALTER COLUMN [EnglishName] [varchar](60) NULL

------------------------------------------------------------
--2016.R5
------------------------------------------------------------
--20/12/2016

EXECUTE sp_rename @objname = N'dbo.GLAccountCurrencies.CustomerGLAccountId', @newname = N'MainGLAccountId', @objtype = N'COLUMN'
EXECUTE sp_rename @objname = N'dbo.GLAccountCurrencies.IX_CustomerGLAccountId', @newname = N'IX_MainGLAccountId', @objtype = N'INDEX'

ALTER TABLE [dbo].[OpportunityProducts] ADD [NotesRightToLeft] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[OpportunityAdditionalServices] ADD [NotesRightToLeft] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[CustomerAdditionalServices] ADD [NotesRightToLeft] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[CustomerProducts] ADD [NotesRightToLeft] [bit] NOT NULL DEFAULT 0


---------------------------------------------------------------------------------

--29/12/2016

ALTER TABLE [dbo].[JournalStatusTypes] ALTER COLUMN [LocalName] [nvarchar](50) NULL


-------------------------------------------------------------------------------

--2/1/2016

ALTER TABLE [dbo].[DocumentTypes] ADD [OrderBy] [int] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[CashBookLines] ADD [SearchFields] [nvarchar](4000)


------------------------------------------------------------------------------

--3/1/2016

ALTER TABLE [dbo].[DocumentTypeTemplates] ADD [TemplateFooterHtml] [varbinary](max)
ALTER TABLE [dbo].[DocumentTypeTemplates] ADD [TemplateHeaderHtml] [varbinary](max)


------------------------------------------------------------------------------
--15/1/2017

ALTER TABLE [dbo].[ChartOfAccounts] ALTER COLUMN [LocalName] [nvarchar](30) NOT NULL
ALTER TABLE [dbo].[ChartOfAccounts] ALTER COLUMN [EnglishName] [varchar](60) NULL

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_BranchId' AND object_id = object_id(N'[dbo].[CashBooks]', N'U'))
    DROP INDEX [IX_BranchId] ON [dbo].[CashBooks]
ALTER TABLE [dbo].[CashBooks] ALTER COLUMN [BranchId] [varchar](15) NOT NULL
CREATE INDEX [IX_BranchId] ON [dbo].[CashBooks]([BranchId])

ALTER TABLE [dbo].[ARPayments] ADD [CashbookId] [nvarchar](15)
ALTER TABLE [dbo].[ARInvoices] ADD [IsGeneralInvoice] [bit] NOT NULL DEFAULT 0


------------------------------------------------------------------------------
--24/1/2017


ALTER TABLE [dbo].[GLAccounts] ADD [LocalBalanceInDue] [decimal](16, 2)
ALTER TABLE [dbo].[GLAccounts] ADD [NextDueDate] [datetime]

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_GLAccountId' AND object_id = object_id(N'[dbo].[BankAccounts]', N'U'))
    DROP INDEX [IX_GLAccountId] ON [dbo].[BankAccounts]
ALTER TABLE [dbo].[BankAccounts] ALTER COLUMN [GLAccountId] [varchar](15) NOT NULL
CREATE INDEX [IX_GLAccountId] ON [dbo].[BankAccounts]([GLAccountId])

ALTER TABLE [dbo].[CreditCardTypes] ADD [BankAccountId] [varchar](15)

ALTER TABLE [dbo].[AccountingSettings] ADD [QBOrealMeID] [nvarchar](max)
ALTER TABLE [dbo].[AccountingSettings] ADD [QBOAccessToken] [nvarchar](max)
ALTER TABLE [dbo].[AccountingSettings] ADD [QBOAccessTokenSecret] [nvarchar](max)

ALTER TABLE [dbo].[AccountingSettings] ALTER COLUMN [QBOrealMeID] [nvarchar](200) NULL
ALTER TABLE [dbo].[AccountingSettings] ALTER COLUMN [QBOAccessToken] [nvarchar](200) NULL
ALTER TABLE [dbo].[AccountingSettings] ALTER COLUMN [QBOAccessTokenSecret] [nvarchar](200) NULL


-----------------------------------------------------------------------------------

--30/1/2017

ALTER TABLE [dbo].[Quotes] ADD [ValueOfGoods] [float]
ALTER TABLE [dbo].[Quotes] ADD [ValueOfGoodsCurrencyId] [varchar](15)
CREATE INDEX [IX_ValueOfGoodsCurrencyId] ON [dbo].[Quotes]([ValueOfGoodsCurrencyId])
ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_dbo.Quotes_dbo.Currencies_ValueOfGoodsCurrencyId] FOREIGN KEY ([ValueOfGoodsCurrencyId]) REFERENCES [dbo].[Currencies] ([Id])

ALTER TABLE [dbo].[Shipments] ADD [ValueOfGoods] [float]
ALTER TABLE [dbo].[Shipments] ADD [ValueOfGoodsCurrencyId] [varchar](15)
CREATE INDEX [IX_ValueOfGoodsCurrencyId] ON [dbo].[Shipments]([ValueOfGoodsCurrencyId])
ALTER TABLE [dbo].[Shipments] ADD CONSTRAINT [FK_dbo.Shipments_dbo.Currencies_ValueOfGoodsCurrencyId] FOREIGN KEY ([ValueOfGoodsCurrencyId]) REFERENCES [dbo].[Currencies] ([Id])


-----------------------------------------------------------------------------------

--31/1/2017

CREATE TABLE [dbo].[WarehouseEntries] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [EntryNumber] [varchar](15) NOT NULL,
    [CustomerId] [varchar](15),
    [ShipmentId] [varchar](15),
    [ShipmentNumber] [varchar](15),
    [WarehouseId] [varchar](15) NOT NULL,
    [ExpectedEntryDate] [datetime] NOT NULL,
    [ActualEntryDate] [datetime] NOT NULL,
    [ReceivedBy] [varchar](15),
    [SpecialInstruction] [varchar](250),
    [StatusCode] [varchar](4),
    [TotalPieces] [int] NOT NULL,
    [TotalGrossWeight] [decimal](18, 2) NOT NULL,
    [GrossWeightUnitCode] [varchar](3),
    [TotalVolume] [decimal](18, 2) NOT NULL,
    [Notes] [nvarchar](1000),
    [CustomerRef1] [varchar](15),
    [CustomerRef2] [varchar](15),
    [HouseNumber] [varchar](15),
    [MasterNumber] [varchar](15),
    CONSTRAINT [PK_dbo.WarehouseEntries] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[WarehouseEntries]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[WarehouseEntries]([UpdatedByUserId])
CREATE INDEX [IX_CustomerId] ON [dbo].[WarehouseEntries]([CustomerId])
CREATE INDEX [IX_ShipmentId] ON [dbo].[WarehouseEntries]([ShipmentId])
CREATE INDEX [IX_WarehouseId] ON [dbo].[WarehouseEntries]([WarehouseId])
CREATE INDEX [IX_StatusCode] ON [dbo].[WarehouseEntries]([StatusCode])
CREATE TABLE [dbo].[WarehouseEntryStatuses] (
    [Code] [varchar](4) NOT NULL,
    [Name] [varchar](100),
    [SearchFields] [nvarchar](max),
    CONSTRAINT [PK_dbo.WarehouseEntryStatuses] PRIMARY KEY ([Code])
)
CREATE TABLE [dbo].[WarehouseEntryPackages] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [WarehouseEntryId] [varchar](15) NOT NULL,
    [ContainerNumber] [varchar](20),
    [Quantity] [int] NOT NULL,
    [Weight] [decimal](16, 2) NOT NULL,
    [Volume] [float] NOT NULL,
    [Description] [varchar](250),
    [PackageTypeId] [varchar](15),
    [Seal] [varchar](15),
    [Harmonize] [varchar](60),
    [Width] [decimal](16, 2) NOT NULL,
    [Length] [float] NOT NULL,
    [Height] [float] NOT NULL,
    CONSTRAINT [PK_dbo.WarehouseEntryPackages] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[WarehouseEntryPackages]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[WarehouseEntryPackages]([UpdatedByUserId])
CREATE INDEX [IX_WarehouseEntryId] ON [dbo].[WarehouseEntryPackages]([WarehouseEntryId])
CREATE INDEX [IX_PackageTypeId] ON [dbo].[WarehouseEntryPackages]([PackageTypeId])
CREATE TABLE [dbo].[WarehouseEntryPackagesReleases] (
    [EntryPackageId] [varchar](15) NOT NULL,
    [ReleasePackageId] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [Quantity] [int] NOT NULL,
    [IsCanceled] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.WarehouseEntryPackagesReleases] PRIMARY KEY ([EntryPackageId], [ReleasePackageId])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[WarehouseEntryPackagesReleases]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[WarehouseEntryPackagesReleases]([UpdatedByUserId])
CREATE TABLE [dbo].[WarehouseReleasePackages] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [WarehouseReleaseId] [varchar](15) NOT NULL,
    [ContainerNumber] [varchar](20),
    [Quantity] [int] NOT NULL,
    [Weight] [decimal](16, 2) NOT NULL,
    [Volume] [float] NOT NULL,
    [Description] [varchar](250),
    [PackageTypeId] [varchar](15),
    [Seal] [varchar](15),
    [Harmonize] [varchar](60),
    [Width] [decimal](16, 2) NOT NULL,
    [Length] [float] NOT NULL,
    [Height] [float] NOT NULL,
    [Instock] [int] NOT NULL,
    CONSTRAINT [PK_dbo.WarehouseReleasePackages] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[WarehouseReleasePackages]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[WarehouseReleasePackages]([UpdatedByUserId])
CREATE INDEX [IX_WarehouseReleaseId] ON [dbo].[WarehouseReleasePackages]([WarehouseReleaseId])
CREATE INDEX [IX_PackageTypeId] ON [dbo].[WarehouseReleasePackages]([PackageTypeId])
CREATE TABLE [dbo].[WarehouseReleases] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [CreatedByUserId] [varchar](15) NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [UpdatedByUserId] [varchar](15) NOT NULL,
    [EntryNumber] [varchar](15) NOT NULL,
    [CustomerId] [varchar](15),
    [ShipmentId] [varchar](15),
    [ShipmentNumber] [varchar](15),
    [WarehouseId] [varchar](15) NOT NULL,
    [ExpectedReleaseDate] [datetime] NOT NULL,
    [ActualReleaseDate] [datetime] NOT NULL,
    [ReceivedBy] [varchar](15),
    [SpecialInstruction] [varchar](250),
    [StatusCode] [varchar](4),
    [TotalPieces] [int] NOT NULL,
    [TotalGrossWeight] [decimal](18, 2) NOT NULL,
    [GrossWeightUnitCode] [varchar](3),
    [TotalVolume] [decimal](18, 2) NOT NULL,
    [Notes] [nvarchar](1000),
    [CustomerRef1] [varchar](15),
    [CustomerRef2] [varchar](15),
    [HouseNumber] [varchar](15),
    [MasterNumber] [varchar](15),
    CONSTRAINT [PK_dbo.WarehouseReleases] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[WarehouseReleases]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[WarehouseReleases]([UpdatedByUserId])
CREATE INDEX [IX_CustomerId] ON [dbo].[WarehouseReleases]([CustomerId])
CREATE INDEX [IX_ShipmentId] ON [dbo].[WarehouseReleases]([ShipmentId])
CREATE INDEX [IX_WarehouseId] ON [dbo].[WarehouseReleases]([WarehouseId])
CREATE INDEX [IX_StatusCode] ON [dbo].[WarehouseReleases]([StatusCode])
CREATE TABLE [dbo].[WarehouseReleaseStatuses] (
    [Code] [varchar](4) NOT NULL,
    [Name] [varchar](100),
    [SearchFields] [nvarchar](max),
    CONSTRAINT [PK_dbo.WarehouseReleaseStatuses] PRIMARY KEY ([Code])
)
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.Cards_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cards] ([Id])
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.Shipments_ShipmentId] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipments] ([Id])
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
ALTER TABLE [dbo].[WarehouseEntries] ADD CONSTRAINT [FK_dbo.WarehouseEntries_dbo.WarehouseEntryStatuses_StatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[WarehouseEntryStatuses] ([Code])
ALTER TABLE [dbo].[WarehouseEntryPackages] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackages_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseEntryPackages] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackages_dbo.PackageTypes_PackageTypeId] FOREIGN KEY ([PackageTypeId]) REFERENCES [dbo].[PackageTypes] ([Id])
ALTER TABLE [dbo].[WarehouseEntryPackages] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackages_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseEntryPackages] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackages_dbo.WarehouseEntries_WarehouseEntryId] FOREIGN KEY ([WarehouseEntryId]) REFERENCES [dbo].[WarehouseEntries] ([Id])
ALTER TABLE [dbo].[WarehouseEntryPackagesReleases] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackagesReleases_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseEntryPackagesReleases] ADD CONSTRAINT [FK_dbo.WarehouseEntryPackagesReleases_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseReleasePackages] ADD CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseReleasePackages] ADD CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.PackageTypes_PackageTypeId] FOREIGN KEY ([PackageTypeId]) REFERENCES [dbo].[PackageTypes] ([Id])
ALTER TABLE [dbo].[WarehouseReleasePackages] ADD CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseReleasePackages] ADD CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.WarehouseEntries_WarehouseReleaseId] FOREIGN KEY ([WarehouseReleaseId]) REFERENCES [dbo].[WarehouseEntries] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.Cards_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cards] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.Shipments_ShipmentId] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipments] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.Users_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ADD CONSTRAINT [FK_dbo.WarehouseReleases_dbo.WarehouseReleaseStatuses_StatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[WarehouseReleaseStatuses] ([Code])

ALTER TABLE [dbo].[WarehouseReleases] ADD [ReleaseNumber] [varchar](15) NOT NULL DEFAULT ''
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.WarehouseReleases')
AND col_name(parent_object_id, parent_column_id) = 'EntryNumber';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[WarehouseReleases] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[WarehouseReleases] DROP COLUMN [EntryNumber]


----------------------------------------------------------------------------------

--1/2/2017


IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CustomerId' AND object_id = object_id(N'[dbo].[WarehouseEntries]', N'U'))
    DROP INDEX [IX_CustomerId] ON [dbo].[WarehouseEntries]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_StatusCode' AND object_id = object_id(N'[dbo].[WarehouseEntries]', N'U'))
    DROP INDEX [IX_StatusCode] ON [dbo].[WarehouseEntries]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CustomerId' AND object_id = object_id(N'[dbo].[WarehouseReleases]', N'U'))
    DROP INDEX [IX_CustomerId] ON [dbo].[WarehouseReleases]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_StatusCode' AND object_id = object_id(N'[dbo].[WarehouseReleases]', N'U'))
    DROP INDEX [IX_StatusCode] ON [dbo].[WarehouseReleases]
ALTER TABLE [dbo].[WarehouseEntries] ADD [VolumeUnitCode] [varchar](3) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[WarehouseReleases] ADD [VolumeUnitCode] [varchar](3) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [CustomerId] [varchar](15) NOT NULL
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [ExpectedEntryDate] [datetime] NULL
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [ActualEntryDate] [datetime] NULL
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [StatusCode] [varchar](4) NOT NULL
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [GrossWeightUnitCode] [varchar](3) NOT NULL
ALTER TABLE [dbo].[WarehouseEntryStatuses] ALTER COLUMN [Name] [varchar](100) NOT NULL
ALTER TABLE [dbo].[WarehouseEntryStatuses] ALTER COLUMN [SearchFields] [nvarchar](max) NOT NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [CustomerId] [varchar](15) NOT NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [StatusCode] [varchar](4) NOT NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [GrossWeightUnitCode] [varchar](3) NOT NULL
ALTER TABLE [dbo].[WarehouseReleaseStatuses] ALTER COLUMN [Name] [varchar](100) NOT NULL
ALTER TABLE [dbo].[WarehouseReleaseStatuses] ALTER COLUMN [SearchFields] [nvarchar](max) NOT NULL
CREATE INDEX [IX_CustomerId] ON [dbo].[WarehouseEntries]([CustomerId])
CREATE INDEX [IX_StatusCode] ON [dbo].[WarehouseEntries]([StatusCode])
CREATE INDEX [IX_CustomerId] ON [dbo].[WarehouseReleases]([CustomerId])
CREATE INDEX [IX_StatusCode] ON [dbo].[WarehouseReleases]([StatusCode])


----------------------------------------------------------------------------------

--12/2/2017


update textcodes set localdefaulttext= 'סטאטוס הוראה' where code = 'Customs.PaymentOrder.F.PaymentStatusCode'

EXECUTE sp_rename @objname = N'Customs.SupplierInvoiceItemVehiclesAddtionals', @newname = N'SupplierInvoiceItemVehicleAdds', @objtype = N'OBJECT'
IF object_id('[PK_Customs.SupplierInvoiceItemVehiclesAddtionals]') IS NOT NULL BEGIN
    EXECUTE sp_rename @objname = N'[PK_Customs.SupplierInvoiceItemVehiclesAddtionals]', @newname = N'PK_Customs.SupplierInvoiceItemVehicleAdds', @objtype = N'OBJECT'
END
Caution: Changing any part of an object name could break scripts and stored procedures.
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemVehiclesAddtional' )
delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemVehiclesAddtional' )
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemVehiclesAddtional' )
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemVehiclesAddtional' )
delete from ObjectTables where name ='customs.SupplierInvoiceItemVehiclesAddtional' 

ALTER TABLE [Customs].[SupplierInvoiceItemVehicleAdds] ADD [WindowNumber] [varchar](13)

ALTER TABLE [Customs].[CustomsCollateralsAnswers] ADD [IsClosed] [bit] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[CustomsCollateralsAnswers] ADD [RequestedTapagNumeral] [decimal](16, 2)

ALTER TABLE [Customs].[CustomsCollateralsAnswers] ADD [RequestedTapagFile] [varchar](25)
ALTER TABLE [Customs].[CustomsCollateralsAnswers] ALTER COLUMN [RequestedTapagNumeral] [varchar](3) NULL

ALTER TABLE [Customs].[CustomDocumentTypeMetaData] ADD [Inactive] [bit] NOT NULL DEFAULT 0

update customs.Consignments set SequenceNumeric = 1 where SequenceNumeric is null

ALTER TABLE [Customs].[Notifications] ADD [CustomerId] [varchar](15)
CREATE INDEX [IX_CustomerId] ON [Customs].[Notifications]([CustomerId])
ALTER TABLE [Customs].[Notifications] ADD CONSTRAINT [FK_Customs.Notifications_dbo.Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])

ALTER TABLE [Customs].[MeasurmentUnits] ALTER COLUMN [MalamId] [int] NULL

CREATE TABLE [dbo].[GeneralLocks] (
    [GeneralKey] [nvarchar](128) NOT NULL,
    [Tenant] [int] NOT NULL,
    [CreatedAt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.GeneralLocks] PRIMARY KEY ([GeneralKey], [Tenant])
)
ALTER TABLE [Customs].[Claims] ADD [AccountBranchCode] [varchar](6)
ALTER TABLE [Customs].[Claims] ADD [CustomsFiles] [varchar](30)
ALTER TABLE [Customs].[ClaimsRelatedEntities] ADD [CourtCode] [varchar](2)
ALTER TABLE [Customs].[CustomsRequestsSheets] ADD [AnalyzeDcaAggregateKey] [varchar](128)
ALTER TABLE [Customs].[ClaimsRelatedEntities] ALTER COLUMN [CustomsExceptions] [nvarchar](1000) NULL
ALTER TABLE [Customs].[MeasurmentUnits] ALTER COLUMN [MalamId] [int] NULL
CREATE INDEX [IX_AccountBranchCode] ON [Customs].[Claims]([AccountBranchCode])
CREATE INDEX [IX_CourtCode] ON [Customs].[ClaimsRelatedEntities]([CourtCode])
ALTER TABLE [Customs].[Claims] ADD CONSTRAINT [FK_Customs.Claims_Customs.CustomsBranches_AccountBranchCode] FOREIGN KEY ([AccountBranchCode]) REFERENCES [Customs].[CustomsBranches] ([Id])
ALTER TABLE [Customs].[ClaimsRelatedEntities] ADD CONSTRAINT [FK_Customs.ClaimsRelatedEntities_Customs.CourtInstances_CourtCode] FOREIGN KEY ([CourtCode]) REFERENCES [Customs].[CourtInstances] ([Code])

ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] DROP CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3Bs]
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] ADD [Tenant] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] ADD [SaleAmountClaim] [decimal](16, 2)
ALTER TABLE [Customs].[ClaimsRelatedEntities] ADD [AbandonmentDestructionReferenc] [varchar](22)
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] ALTER COLUMN [LineNo] [int] NOT NULL
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] ADD CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3Bs] PRIMARY KEY ([ClaimId], [LineNo])
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'Customs.ClaimImporterDeclarsPage3Bs')
AND col_name(parent_object_id, parent_column_id) = 'SaleAmountATClaimTime';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3Bs] DROP COLUMN [SaleAmountATClaimTime]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'Customs.ClaimsRelatedEntities')
AND col_name(parent_object_id, parent_column_id) = 'AbandonmentDestructionReference';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [Customs].[ClaimsRelatedEntities] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [Customs].[ClaimsRelatedEntities] DROP COLUMN [AbandonmentDestructionReference]

ALTER TABLE [dbo].[DocumentsFilings] ADD [LastVersion] [int] NOT NULL DEFAULT 0

ALTER TABLE [Customs].[PaymentOrders] ADD [PaymentOrderLeftAmount] [decimal](16, 2)

IF object_id(N'[Customs].[FK_Customs.ClaimImporterDeclarsP3Lois_Customs.ClaimImporterDeclarsPage3s_ClaimId_ImporterLoiDeclarationTypeCode]', N'F') IS NOT NULL
    ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] DROP CONSTRAINT [FK_Customs.ClaimImporterDeclarsP3Lois_Customs.ClaimImporterDeclarsPage3s_ClaimId_ImporterLoiDeclarationTypeCode]
IF object_id(N'[Customs].[FK_Customs.ClaimsRelatedEntsReasonsExps_Customs.ClaimsRelatedEntitiesReasons_ClaimId_CounterKey_ReasonListTypeCode]', N'F') IS NOT NULL
    ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] DROP CONSTRAINT [FK_Customs.ClaimsRelatedEntsReasonsExps_Customs.ClaimsRelatedEntitiesReasons_ClaimId_CounterKey_ReasonListTypeCode]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ClaimId_ImporterLoiDeclarationTypeCode' AND object_id = object_id(N'[Customs].[ClaimImporterDeclarsP3Lois]', N'U'))
    DROP INDEX [IX_ClaimId_ImporterLoiDeclarationTypeCode] ON [Customs].[ClaimImporterDeclarsP3Lois]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_CommercialSaleTypeCode' AND object_id = object_id(N'[Customs].[ClaimImporterDeclarsPage3As]', N'U'))
    DROP INDEX [IX_CommercialSaleTypeCode] ON [Customs].[ClaimImporterDeclarsPage3As]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ReasonListTypeCode' AND object_id = object_id(N'[Customs].[ClaimsRelatedEntitiesReasons]', N'U'))
    DROP INDEX [IX_ReasonListTypeCode] ON [Customs].[ClaimsRelatedEntitiesReasons]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ClaimId_CounterKey_ReasonListTypeCode' AND object_id = object_id(N'[Customs].[ClaimsRelatedEntsReasonsExps]', N'U'))
    DROP INDEX [IX_ClaimId_CounterKey_ReasonListTypeCode] ON [Customs].[ClaimsRelatedEntsReasonsExps]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ClaimExplanationTypeCode' AND object_id = object_id(N'[Customs].[ClaimsRelatedEntsReasonsExps]', N'U'))
    DROP INDEX [IX_ClaimExplanationTypeCode] ON [Customs].[ClaimsRelatedEntsReasonsExps]
EXECUTE sp_rename @objname = N'Customs.ClaimImporterDeclarsP3Lois.ImporterLoiDeclarationTypeCode', @newname = N'CounterKey', @objtype = N'COLUMN'
EXECUTE sp_rename @objname = N'Customs.ClaimsRelatedEntsReasonsExps.ReasonListTypeCode', @newname = N'ReasonLineNo', @objtype = N'COLUMN'
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] DROP CONSTRAINT [PK_Customs.ClaimImporterDeclarsP3Lois]
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3s] DROP CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3s]
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3As] DROP CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3As]
ALTER TABLE [Customs].[ClaimsRelatedEntitiesReasons] DROP CONSTRAINT [PK_Customs.ClaimsRelatedEntitiesReasons]
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] DROP CONSTRAINT [PK_Customs.ClaimsRelatedEntsReasonsExps]
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] ADD [LineNo] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3s] ADD [LineNo] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3As] ADD [LineNo] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimsRelatedEntitiesReasons] ADD [LineNo] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] ADD [LineNo] [int] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] ALTER COLUMN [CounterKey] [int] NOT NULL
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] ALTER COLUMN [DeclarationNumber] [varchar](35) NULL
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3s] ALTER COLUMN [ImporterLoiDeclarationTypeCode] [varchar](2) NULL
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3As] ALTER COLUMN [CommercialSaleTypeCode] [varchar](2) NULL
ALTER TABLE [Customs].[ClaimsRelatedEntitiesReasons] ALTER COLUMN [ReasonListTypeCode] [varchar](3) NULL
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] ALTER COLUMN [ReasonLineNo] [int] NOT NULL
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] ALTER COLUMN [ClaimExplanationTypeCode] [varchar](2) NULL
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] ADD CONSTRAINT [PK_Customs.ClaimImporterDeclarsP3Lois] PRIMARY KEY ([ClaimId], [CounterKey], [LineNo])
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3s] ADD CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3s] PRIMARY KEY ([ClaimId], [LineNo])
ALTER TABLE [Customs].[ClaimImporterDeclarsPage3As] ADD CONSTRAINT [PK_Customs.ClaimImporterDeclarsPage3As] PRIMARY KEY ([ClaimId], [LineNo])
ALTER TABLE [Customs].[ClaimsRelatedEntitiesReasons] ADD CONSTRAINT [PK_Customs.ClaimsRelatedEntitiesReasons] PRIMARY KEY ([ClaimId], [CounterKey], [LineNo])
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] ADD CONSTRAINT [PK_Customs.ClaimsRelatedEntsReasonsExps] PRIMARY KEY ([ClaimId], [CounterKey], [ReasonLineNo], [LineNo])
CREATE INDEX [IX_ClaimId_CounterKey] ON [Customs].[ClaimImporterDeclarsP3Lois]([ClaimId], [CounterKey])
CREATE INDEX [IX_CommercialSaleTypeCode] ON [Customs].[ClaimImporterDeclarsPage3As]([CommercialSaleTypeCode])
CREATE INDEX [IX_ReasonListTypeCode] ON [Customs].[ClaimsRelatedEntitiesReasons]([ReasonListTypeCode])
CREATE INDEX [IX_ClaimId_CounterKey_ReasonLineNo] ON [Customs].[ClaimsRelatedEntsReasonsExps]([ClaimId], [CounterKey], [ReasonLineNo])
CREATE INDEX [IX_ClaimExplanationTypeCode] ON [Customs].[ClaimsRelatedEntsReasonsExps]([ClaimExplanationTypeCode])
ALTER TABLE [Customs].[ClaimImporterDeclarsP3Lois] ADD CONSTRAINT [FK_Customs.ClaimImporterDeclarsP3Lois_Customs.ClaimImporterDeclarsPage3s_ClaimId_CounterKey] FOREIGN KEY ([ClaimId], [CounterKey]) REFERENCES [Customs].[ClaimImporterDeclarsPage3s] ([ClaimId], [LineNo])
ALTER TABLE [Customs].[ClaimsRelatedEntsReasonsExps] ADD CONSTRAINT [FK_Customs.ClaimsRelatedEntsReasonsExps_Customs.ClaimsRelatedEntitiesReasons_ClaimId_CounterKey_ReasonLineNo] FOREIGN KEY ([ClaimId], [CounterKey], [ReasonLineNo]) REFERENCES [Customs].[ClaimsRelatedEntitiesReasons] ([ClaimId], [CounterKey], [LineNo])

ALTER TABLE [Customs].[Tapags] ADD [ReferantId] [varchar](15)

CREATE INDEX [IX_ReferantId] ON [Customs].[Tapags]([ReferantId])
ALTER TABLE [Customs].[Tapags] ADD CONSTRAINT [FK_Customs.Tapags_dbo.Users_ReferantId] FOREIGN KEY ([ReferantId]) REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [Customs].[Declarations] ADD [IsConnectedToUnifreight] [bit] NOT NULL DEFAULT 0

ALTER TABLE [Customs].[SupplierInvoiceItems] ADD [DeferredCustomsTax] [decimal](5, 2)
ALTER TABLE [Customs].[SupplierInvoiceItems] ADD [DeferredPurchaseTax] [decimal](5, 2)


ALTER TABLE [Customs].[Declarations] ADD [ImporterName] [nvarchar](100)
ALTER TABLE [Customs].[Declarations] ADD [EntitleImporterName] [nvarchar](55)
ALTER TABLE [Customs].[Declarations] ADD [TransferImporterName] [nvarchar](100)
ALTER TABLE [Customs].[Declarations] ADD [MainImporterEntitlemntTypeCode] [varchar](3)
ALTER TABLE [Customs].[Declarations] ADD [TransImporterEntitleTypeCode] [varchar](3)
ALTER TABLE [Customs].[Declarations] ADD [ImporterAddress] [varchar](236)
ALTER TABLE [Customs].[Declarations] ADD [TransferImporterAddress] [varchar](236)
ALTER TABLE [Customs].[Declarations] ADD [EntitleImporterAddress] [varchar](236)
ALTER TABLE [Customs].[Declarations] ADD [ImporterPassportNumber] [varchar](15)
ALTER TABLE [Customs].[Declarations] ADD [TransferPassportNumber] [varchar](15)
CREATE INDEX [IX_MainImporterEntitlemntTypeCode] ON [Customs].[Declarations]([MainImporterEntitlemntTypeCode])
ALTER TABLE [Customs].[Declarations] ADD CONSTRAINT [FK_Customs.Declarations_Customs.EntitlementTypes_MainImporterEntitlemntTypeCode] FOREIGN KEY ([MainImporterEntitlemntTypeCode]) REFERENCES [Customs].[EntitlementTypes] ([Code])

ALTER TABLE [Customs].[Declarations] ADD [EntitlePassportNumber] [varchar](15)

IF schema_id('Customs') IS NULL
    EXECUTE('CREATE SCHEMA [Customs]')
CREATE TABLE [Customs].[FacilitationType] (
    [Code] [varchar](15) NOT NULL,
    [LocalName] [nvarchar](100),
    [SearchFields] [nvarchar](max),
    [EnglishName] [varchar](100),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_Customs.FacilitationType] PRIMARY KEY ([Code])
)
ALTER TABLE [Customs].[Clients] ADD [FacilitationTypeCode] [varchar](15)

CREATE INDEX [IX_FacilitationTypeCode] ON [Customs].[Clients]([FacilitationTypeCode])
ALTER TABLE [Customs].[Clients] ADD CONSTRAINT [FK_Customs.Clients_Customs.FacilitationType_FacilitationTypeCode] FOREIGN KEY ([FacilitationTypeCode]) REFERENCES [Customs].[FacilitationType] ([Code])

ALTER TABLE [Customs].[CustomsCollateralsAnswers] ADD [PaymentOrderId] [varchar](15)
CREATE INDEX [IX_PaymentOrderId] ON [Customs].[CustomsCollateralsAnswers]([PaymentOrderId])
ALTER TABLE [Customs].[CustomsCollateralsAnswers] ADD CONSTRAINT [FK_Customs.CustomsCollateralsAnswers_Customs.PaymentOrders_PaymentOrderId] FOREIGN KEY ([PaymentOrderId]) REFERENCES [Customs].[PaymentOrders] ([Id])

ALTER TABLE [Customs].[CustomsCollaterals] ADD [CustomerId] [varchar](15)
CREATE INDEX [IX_CustomerId] ON [Customs].[CustomsCollaterals]([CustomerId])
ALTER TABLE [Customs].[CustomsCollaterals] ADD CONSTRAINT [FK_Customs.CustomsCollaterals_dbo.Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])

ALTER TABLE [Customs].[GovernmentProcedureTypes] ADD [Order] [int]

ALTER TABLE [Customs].[CustomsSettings] ADD [UnifreightCertificateActivated] [bit] NOT NULL DEFAULT 0

ALTER TABLE [Customs].[SupplierInvioceItemCertificats] ADD [ExternalCertificatCode] [varchar](8)

ALTER TABLE [Customs].[Notifications] ALTER COLUMN [Description] [nvarchar](max) NULL

ALTER TABLE [Customs].[NotificationReplies] ALTER COLUMN [ResponseToCustoms] [nvarchar](max) NULL

ALTER TABLE [Customs].[SupplierInvoiceItems] ADD [VehicleStatus] [bit] NOT NULL DEFAULT 0
ALTER TABLE [Customs].[SupplierInvoiceItems] ADD [ItemAdditionalStatus] [bit] NOT NULL DEFAULT 0

ALTER TABLE [Customs].[CustomsSettings] ADD [AutoFillPaymentScreen] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[APInvoices] ADD [IsGeneralInvoice] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[WarehouseEntryPackages] ADD [Instock] [int] NOT NULL DEFAULT 0
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.WarehouseReleasePackages')
AND col_name(parent_object_id, parent_column_id) = 'Instock';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[WarehouseReleasePackages] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[WarehouseReleasePackages] DROP COLUMN [Instock]

IF schema_id('Customs') IS NULL
    EXECUTE('CREATE SCHEMA [Customs]')
CREATE TABLE [Customs].[ImporterDeclarationTypes] (
    [Code] [nvarchar](128) NOT NULL,
    [EnglishName] [nvarchar](max),
    [LocalName] [nvarchar](max),
    [SearchFields] [nvarchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_Customs.ImporterDeclarationTypes] PRIMARY KEY ([Code])
)

IF object_id(N'[dbo].[FK_dbo.WarehouseReleasePackages_dbo.WarehouseEntries_WarehouseReleaseId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[WarehouseReleasePackages] DROP CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.WarehouseEntries_WarehouseReleaseId]
ALTER TABLE [dbo].[WarehouseReleasePackages] ADD CONSTRAINT [FK_dbo.WarehouseReleasePackages_dbo.WarehouseReleases_WarehouseReleaseId] FOREIGN KEY ([WarehouseReleaseId]) REFERENCES [dbo].[WarehouseReleases] ([Id])
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ExpectedReleaseDate] [datetime] NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ActualReleaseDate] [datetime] NULL

ALTER TABLE [dbo].[Features] ALTER COLUMN [Code] [varchar](120) NOT NULL

ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [ShipmentNumber] [varchar](100) NULL
ALTER TABLE [dbo].[WarehouseEntries] ALTER COLUMN [ReceivedBy] [varchar](200) NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ShipmentNumber] [varchar](100) NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ExpectedReleaseDate] [datetime] NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ActualReleaseDate] [datetime] NULL
ALTER TABLE [dbo].[WarehouseReleases] ALTER COLUMN [ReceivedBy] [varchar](200) NULL


----------------------------------------------------------------

--13/2/2017


IF object_id(N'[dbo].[FK_ChargesTypeChargesGroup]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[ChargesTypes] DROP CONSTRAINT [FK_ChargesTypeChargesGroup]
ALTER TABLE [dbo].[ChargesGroups] DROP CONSTRAINT [PK_ChargesGroups]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ChargesGroupCode' AND object_id = object_id(N'[dbo].[ChargesTypes]', N'U'))
    DROP INDEX [IX_ChargesGroupCode] ON [dbo].[ChargesTypes]
ALTER TABLE [dbo].[ChargesGroups] ADD [Id] [varchar](15) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[ChargesGroups] ADD [Tenant] [int] NOT NULL DEFAULT 0
BEGIN; declare @Tenant as int declare @Id as varchar(15)declare @Code as varchar(5) DECLARE ChargesGroupsCursor CURSOR READ_ONLY   FOR SELECT Code  FROM ChargesGroups    OPEN ChargesGroupsCursor FETCH NEXT FROM ChargesGroupsCursor INTO @Code WHILE @@FETCH_STATUS = 0   BEGIN  EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'ChargesGroup' update ChargesGroups set Id = @Id , Tenant = 0 where Code = @Code FETCH NEXT FROM ChargesGroupsCursor INTO  @Code END CLOSE ChargesGroupsCursor     DEALLOCATE ChargesGroupsCursor END
ALTER TABLE [dbo].[ChargesGroups] ADD CONSTRAINT [PK_dbo.ChargesGroups] PRIMARY KEY ([Id])
CREATE UNIQUE INDEX [IX_Code] ON [dbo].[ChargesGroups]([Code])
CREATE INDEX [IX_ChargesGroupCode] ON [dbo].[ChargesTypes]([ChargesGroupCode])
ALTER TABLE [dbo].[ChargesTypes] ADD CONSTRAINT [FK_dbo.ChargesTypes_dbo.ChargesGroups_ChargesGroupCode] FOREIGN KEY ([ChargesGroupCode]) REFERENCES [dbo].[ChargesGroups] ([Code])
ALTER TABLE [dbo].[ChargesGroups] ADD [LocalName] [varchar](40)

IF object_id(N'[dbo].[FK_dbo.ChargesTypes_dbo.ChargesGroups_ChargesGroupCode]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[ChargesTypes] DROP CONSTRAINT [FK_dbo.ChargesTypes_dbo.ChargesGroups_ChargesGroupCode]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ChargesGroupCode' AND object_id = object_id(N'[dbo].[ChargesTypes]', N'U'))
    DROP INDEX [IX_ChargesGroupCode] ON [dbo].[ChargesTypes]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Code' AND object_id = object_id(N'[dbo].[ChargesGroups]', N'U'))
    DROP INDEX [IX_Code] ON [dbo].[ChargesGroups]
ALTER TABLE [dbo].[ChargesTypes] ADD [ChargesGroupId] [varchar](15)
CREATE INDEX [IX_ChargesGroupId] ON [dbo].[ChargesTypes]([ChargesGroupId])
ALTER TABLE [dbo].[ChargesTypes] ADD CONSTRAINT [FK_dbo.ChargesTypes_dbo.ChargesGroups_ChargesGroupId] FOREIGN KEY ([ChargesGroupId]) REFERENCES [dbo].[ChargesGroups] ([Id])
