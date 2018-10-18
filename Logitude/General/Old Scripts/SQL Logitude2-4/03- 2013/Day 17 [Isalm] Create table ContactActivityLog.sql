
create table [dbo].[ContactActivityLogs] (
    [Id] [varchar](40) not null,
    [Tenant] [int] not null,
    [ContactId] [varchar](15) not null,
    [LogDateTime] [datetime] not null,
    [GMTLogDateTime]  [datetime] not null,
    [Module] [varchar](100) not null,
	[Activity] [varchar](150) not null,
	[IsSharedLogisticsContact] bit not null
    
    primary key ([Id])
);


 