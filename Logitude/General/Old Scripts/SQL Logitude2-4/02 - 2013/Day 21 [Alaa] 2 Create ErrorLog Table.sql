Use [Logitude2-4_SystemLogs] 
create table [dbo].[ErrorLogs] (
    [Id] [varchar](40) not null,
    [Tenant] [int] not null,
    [UserName] [varchar](100) not null,
    [LogDate] [datetime] not null,
    [ClientDate] [datetime] not null,
    [Tier] [varchar](40) not null,
    [Exception] [varchar](1000) not null,
    [StackTrace] [varchar](8000) null,
    [SearchFields] [nvarchar](1500) null,
    primary key ([Id])
);
