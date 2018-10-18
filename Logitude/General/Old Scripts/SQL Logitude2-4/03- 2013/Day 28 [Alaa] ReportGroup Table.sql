--Excute in Main DB--

create table [dbo].[ReportGroups] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [nvarchar](40) null,
    primary key ([Id])
);

alter table reports add ReportGroupId varchar(15)  null