create table [dbo].[Reports] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [Name] [varchar](40) null,   
    [Description] [nvarchar](250) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
