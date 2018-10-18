create table [Customs].[Banks] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[Branches] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[PaymentMethodStatus] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PaymentMethodTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[PaymentOrderConnectionTables] (
    [PaymentOrderId] [varchar](15) not null,
    [ConnectedEntityId] [varchar](15) not null,
    [ConnectedEntityCode] [varchar](1) null,
    [Tenant] [int] not null,
    primary key ([PaymentOrderId], [ConnectedEntityId])
);

create table [Customs].[PaymentOrderMethods] (
    [PaymentOrderId] [varchar](15) not null,
    [Line] [int] not null,
    [Tenant] [int] not null,
    [TypeCode] [varchar](2) null,
    [Amount] [decimal](18, 2) not null,
    [BankCode] [varchar](2) null,
    [BranchCode] [varchar](3) null,
    [AccountNumber] [varchar](11) null,
    [PaymentMethodStatusCode] [varchar](2) null,
    primary key ([PaymentOrderId], [Line])
);

create table [Customs].[PaymentOrderProtestReasons] (
    [PaymentOrderId] [varchar](15) not null,
    [Line] [int] not null,
    [ProtestTypeCode] [varchar](4) null,
    [CustomsAgentExplanation] [nvarchar](255) null,
    [InvoiceNumber] [varchar](35) null,
    [GoodsItemLineNumber] [decimal](18, 2) null,
    [GoodsItemClassification] [varchar](13) null,
    [AmountInDispute] [decimal](18, 2) null,
    primary key ([PaymentOrderId], [Line])
);

create table [Customs].[PaymentProtestTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[PaymentOrderConnectionTables] add constraint [PaymentOrderConnectionTable_PaymentOrder] foreign key ([PaymentOrderId]) references [Customs].[PaymentOrders]([Id]);
alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_Bank] foreign key ([BankCode]) references [Customs].[Banks]([Code]);
alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_Branch] foreign key ([BranchCode]) references [Customs].[Branches]([Code]);
alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_PaymentMethodStatus] foreign key ([PaymentMethodStatusCode]) references [Customs].[PaymentMethodStatus]([Code]);
alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_PaymentMethodType] foreign key ([TypeCode]) references [Customs].[PaymentMethodTypes]([Code]);
alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_PaymentOrder] foreign key ([PaymentOrderId]) references [Customs].[PaymentOrders]([Id]);
alter table [Customs].[PaymentOrderProtestReasons] add constraint [PaymentOrderProtestReason_PaymentOrder] foreign key ([PaymentOrderId]) references [Customs].[PaymentOrders]([Id]);
alter table [Customs].[PaymentOrderProtestReasons] add constraint [PaymentOrderProtestReason_ProtestType] foreign key ([ProtestTypeCode]) references [Customs].[PaymentProtestTypes]([Code]);
