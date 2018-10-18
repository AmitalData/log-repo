-- Creating table 'GlobalTenantCounters'
CREATE TABLE [dbo].[GlobalTenantCounters] (
    [Id] int  NOT NULL,
    [LastNumber] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'GlobalTenantCounters'
ALTER TABLE [dbo].[GlobalTenantCounters]
ADD CONSTRAINT [PK_GlobalTenantCounters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

insert into GlobalTenantCounters (Id,LastNumber)
values(1,16)