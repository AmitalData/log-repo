begin transaction
begin

create table [dbo].[OpportunityCompetitors] (
    [OpportunityId] [varchar](15) not null,
    [CompetitorId] [varchar](15) not null,
    [Tenant] [int] not null,
    primary key ([OpportunityId], [CompetitorId])
);
create table [dbo].[OpportunityCompetitorProducts] (
    [OpportunityId] [varchar](15) not null,
    [CompetitorId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
    [Tenant] [int] not null,
    primary key ([OpportunityId], [CompetitorId], [ProductTypeCode])
);

alter table [dbo].[OpportunityCompetitors] add constraint [OpportunityCompetitor_Competitor] foreign key ([CompetitorId]) references [dbo].[Competitors]([Id]);
alter table [dbo].[OpportunityCompetitors] add constraint [OpportunityCompetitor_Opportunity] foreign key ([OpportunityId]) references [dbo].[Opportunities]([Id]);
alter table [dbo].[OpportunityCompetitorProducts] add constraint [OpportunityCompetitorProduct_Competitor] foreign key ([CompetitorId]) references [dbo].[Competitors]([Id]);
alter table [dbo].[OpportunityCompetitorProducts] add constraint [OpportunityCompetitorProduct_Opportunity] foreign key ([OpportunityId]) references [dbo].[Opportunities]([Id]);
alter table [dbo].[OpportunityCompetitorProducts] add constraint [OpportunityCompetitorProduct_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);

END
commit transaction