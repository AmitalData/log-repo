create table [OpportunityProductLocations] (
    [OpportunityId] [varchar](15) not null,
    [OpportunityProductTypeCode] [varchar](2) not null,
	[LineNumber] [int] not null,
    [Tenant] [int] not null,
	[CountryId] [varchar](15) null,
	[GlobalZoneId] [varchar](15) null,
    primary key ([OpportunityId], [OpportunityProductTypeCode], [LineNumber])
);
GO

alter table [OpportunityProductLocations] add constraint [OpportunityProductLocation_Opportunity] foreign key ([OpportunityId]) references [Opportunities]([Id]);
go

alter table [OpportunityProductLocations] add constraint [OpportunityProductLocation_OpportunityProductType] foreign key ([OpportunityProductTypeCode]) references [OpportunityProductTypes]([Code]);
go

alter table [OpportunityProductLocations] add constraint [OpportunityProductLocation_Country] foreign key ([CountryId]) references [Countries]([Id]);
go

alter table [OpportunityProductLocations] add constraint [OpportunityProductLocation_GlobalZone] foreign key ([GlobalZoneId]) references [GlobalZones]([Id]);
go