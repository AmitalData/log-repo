create table [Customs].[DeficitConnectedFileParagraphTypes] (
    [DeficitId] [varchar](15) not null,
    [DeclarationId] [varchar](15) not null,
    [ParagraphTypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [Amount] [decimal](16, 2) null,
    primary key ([DeficitId], [DeclarationId], [ParagraphTypeCode])
);


create table [Customs].[TapagConnectionTables] (
    [TapagId] [varchar](15) not null,
    [DeclarationId] [varchar](15) not null,
    [Tenant] [int] not null,
    primary key ([TapagId], [DeclarationId])
);

alter table customs.Deficits add PaymentOrderId varchar(15) null
go
alter table customs.Deficits add PaymentOrderNumber varchar(9) null
go

alter table [Customs].[DeficitConnectedFileParagraphTypes] add constraint [DeficitConnectedFileParagraphType_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[DeficitConnectedFileParagraphTypes] add constraint [DeficitConnectedFileParagraphType_Deficit] foreign key ([DeficitId]) references [Customs].[Deficits]([Id]);
alter table [Customs].[DeficitConnectedFileParagraphTypes] add constraint [DeficitConnectedFileParagraphType_ParagraphType] foreign key ([ParagraphTypeCode]) references [Customs].[ParagraphTypes]([Code]);
alter table [Customs].[TapagConnectionTables] add constraint [TapagConnectionTable_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[TapagConnectionTables] add constraint [TapagConnectionTable_Tapag] foreign key ([TapagId]) references [Customs].[Tapags]([Id]);
alter table [Customs].[Deficits] add constraint [Deficit_PaymentOrder] foreign key ([PaymentOrderId]) references [Customs].[PaymentOrders]([Id]);

