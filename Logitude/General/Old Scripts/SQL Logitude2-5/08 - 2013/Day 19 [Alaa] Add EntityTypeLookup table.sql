create table [Customs].[EntityTypeLookups] (
    [Code] [varchar](9) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

alter table [Customs].[PaymentOrders] add   
    [CustomsEntityTypeCode] [varchar](9) null,
    [FirstEntityID] [varchar](35) null,
    [SecondEntityID] [varchar](35) null,
    [ThirdEntityID] [varchar](35) null;

alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_EntityTypeLookup] foreign key ([CustomsEntityTypeCode]) references [Customs].[EntityTypeLookups]([Code]);
go