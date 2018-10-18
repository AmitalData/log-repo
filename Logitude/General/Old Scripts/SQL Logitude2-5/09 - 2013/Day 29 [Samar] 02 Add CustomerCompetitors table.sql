begin transaction
begin

create table [dbo].[CustomerCompetitors] (
    [CompetitorId] [varchar](15) not null,
    [CustomerId] [varchar](15) not null,
    [Tenant] [int] not null,
    primary key ([CompetitorId],[CustomerId])
);

create table [dbo].[CustomerCompetitorProducts] (
    [ProductTypeCode] [varchar](2) not null,
    [CustomerId] [varchar](15) not null,
    [CompetitorId] [varchar](15) not null,
    [Tenant] [int] not null,
    primary key ([ProductTypeCode],[CustomerId],[CompetitorId])
);

alter table [dbo].[CustomerCompetitors] add constraint [CustomerCompetitor_Competitor] foreign key ([CompetitorId]) references [dbo].[Competitors]([Id]);
alter table [dbo].[CustomerCompetitors] add constraint [CustomerCompetitor_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerCompetitorProducts] add constraint [CustomerCompetitorProduct_Competitor] foreign key ([CompetitorId]) references [dbo].[Competitors]([Id]);
alter table [dbo].[CustomerCompetitorProducts] add constraint [CustomerCompetitorProduct_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerCompetitorProducts] add constraint [CustomerCompetitorProduct_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);


END
commit transaction