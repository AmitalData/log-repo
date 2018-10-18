
create table [Industries] (
    [Id] [varchar](15) not null,
	[Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [Tenant] [int] not null,
	[SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
GO

alter table [Customers] add [IndustryId] varchar(15) null
go

alter table [Customers] add [LeadSourceId] varchar(15) null
go

alter table [Customers] add constraint [Customer_Industry] foreign key ([IndustryId]) references [Industries]([Id]);
go

alter table [Customers] add constraint [Customer_LeadSource] foreign key ([LeadSourceId]) references [LeadSources]([Id]);
go

alter table [Cards] add [CollectorId] varchar(15) null
go

alter table [Cards] add [CalssifierId] varchar(15) null
go

alter table [Cards] add constraint [Card_CollectorUser] foreign key ([CollectorId]) references [Users]([Id]);
go

alter table [Cards] add constraint [Card_CalssifierUser] foreign key ([CalssifierId]) references [Users]([Id]);
go