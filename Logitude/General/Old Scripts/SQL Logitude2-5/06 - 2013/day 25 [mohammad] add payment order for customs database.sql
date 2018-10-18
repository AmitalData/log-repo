create table [Customs].[CustomerActivityTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[OrganizationUnitTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[PaymentOrders] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [PaymentNumber] [varchar](9) null,
    [TotalSumToPay] [decimal](18, 2) null,
    [LastPayDate] [datetime] null,
    [Reason] [varchar](512) null,
    [CustomerId] [varchar](15) null,
    [CustomerActivityTypeCode] [varchar](2) null,
    [PaymentOrderTypeCode] [varchar](2) null,
    [PaymentProcessCode] [varchar](2) null,
    [PaymentStatusCode] [varchar](2) null,
    [CustomsHouseCode] [varchar](2) null,
    [ActualPayDate] [datetime] null,
    [OperationalStatusCode] [varchar](2) null,
    [InternalNotes] [varchar](512) null,
    [CreateDate] [datetime] null,
    [UpdateDate] [datetime] null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
create table [Customs].[PaymentOrderLines] (
    [PaymentOrderId] [varchar](15) not null,
    [ParagraphTypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [Amount] [decimal](18, 2) null,
    primary key ([PaymentOrderId], [ParagraphTypeCode])
);
create table [Customs].[PaymentOrderOperationalStatus] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PaymentOrderStatus] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PaymentOrderTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PaymentProcesses] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_CustomerCard] foreign key ([CustomerId]) references [dbo].[Cards]([Id]);
alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_OperationalStatus] foreign key ([OperationalStatusCode]) references [Customs].[PaymentOrderOperationalStatus]([Code]);
alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_PaymentOrderType] foreign key ([PaymentOrderTypeCode]) references [Customs].[PaymentOrderTypes]([Code]);
alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_PaymentProcess] foreign key ([PaymentProcessCode]) references [Customs].[PaymentProcesses]([Code]);
alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_PaymentStatus] foreign key ([PaymentStatusCode]) references [Customs].[PaymentOrderStatus]([Code]);
alter table [Customs].[PaymentOrderLines] add constraint [PaymentOrderLine_ParagraphType] foreign key ([ParagraphTypeCode]) references [Customs].[ParagraphTypes]([Code]);
alter table [Customs].[PaymentOrderLines] add constraint [PaymentOrderLine_PaymentOrder] foreign key ([PaymentOrderId]) references [Customs].[PaymentOrders]([Id]);