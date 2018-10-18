
create table [OpportunityTypes] (
    [Code] [varchar](1) not null,
    [Name] [nvarchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

insert into [OpportunityTypes] (Code, Name, SearchFields) values ('N','New Business', 'N,New Business' )
go

alter table Opportunities add [ChargeableWeight] decimal  null
go

alter table Opportunities add [TEU] decimal null
go

alter table Opportunities add [NumberOfShipments] int null
go

alter table Opportunities add [OpportunityTypeCode] varchar(1) not null default 'N'
go

alter table [Opportunities] add constraint [Opportunity_OpportunityType] foreign key ([OpportunityTypeCode]) references [OpportunityTypes]([Code]);
go

alter table [Activities] add [OutlookId] varchar(40) null
go

alter table [Activities] add [NeedSynchronization] bit not null default 0
go


