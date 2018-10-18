
-- Creating table 'CustomPickLists'
CREATE TABLE [dbo].[CustomPickLists] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [Code]varchar(60)   NOT NULL,
    [Value]varchar(60)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'CustomPickLists'
ALTER TABLE [dbo].[CustomPickLists]
ADD CONSTRAINT [PK_CustomPickLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


alter table ObjectFields add [CustomPickListCode]varchar(60)   NULL