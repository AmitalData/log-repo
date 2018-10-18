create table [Customs].[DeclarationPayments] (
    [DeclarationId] [varchar](15) not null,
    [PaymentDate] [datetime] null,
    [CreatedByUserId] [varchar](15) null,
    [SignatoryIdentification] [varchar](17) null,
    [IsProcessA] [bit] not null,
    [ProcessADescription] [nvarchar](512) null,
    primary key ([DeclarationId])
);
create table [Customs].[DeclarationPaymentMethods] (
    [DeclarationId] [varchar](15) not null,
    [Line] [int] not null,
    [SequenceNumeric] [int] not null,
    [PayerActivityTypeCode] [varchar](4) null,
    [MethodTypeCode] [varchar](3) null,
    [Amount] [decimal](16, 2) null,
    [BankCode] [varchar](2) null,
    [BranchCode] [varchar](3) null,
    [AccountNumber] [varchar](11) null,
    primary key ([DeclarationId], [Line])
);
create table [Customs].[DeclarationPaymentProtests] (
    [DeclarationId] [varchar](15) not null,
    [Line] [int] not null,
    [ProtestTypeCode] [varchar](4) null,
    [CustomsAgentExplanation] [nvarchar](255) null,
    [InvoiceNumber] [varchar](35) null,
    [GoodsItemLineNumber] [decimal](16, 5) null,
    [GoodsItemClassification] [varchar](13) null,
    [AmountInDispute] [decimal](16, 2) null,
    primary key ([DeclarationId], [Line])
);

create table [Customs].[PayerActivityTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[DeclarationPayments] add constraint [DeclarationPayment_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_Bank] foreign key ([BankCode]) references [Customs].[Banks]([Code]);
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_Branch] foreign key ([BranchCode]) references [Customs].[Branches]([Code]);
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_PayerActivityType] foreign key ([PayerActivityTypeCode]) references [Customs].[PayerActivityTypes]([Code]);
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_PaymentMethodType] foreign key ([MethodTypeCode]) references [Customs].[PaymentMethodTypes]([Code]);
alter table [Customs].[DeclarationPaymentProtests] add constraint [DeclarationPaymentProtest_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[DeclarationPaymentProtests] add constraint [DeclarationPaymentProtest_PaymentProtestType] foreign key ([ProtestTypeCode]) references [Customs].[PaymentProtestTypes]([Code]);
