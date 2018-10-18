--alter table Shipments add [FinalArrivalDate] datetime   NULL
--go

alter table Cards alter column [IBANNumber]nvarchar(30)   NULL
go

alter table EventTypes add [EventTypeCategoryCode] nvarchar(3)   NULL
go

-- Creating table 'EventTypeCategories'
CREATE TABLE [dbo].[EventTypeCategories] (
    [Code]nvarchar(3)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'EventTypeCategories'
ALTER TABLE [dbo].[EventTypeCategories]
ADD CONSTRAINT [PK_EventTypeCategories]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [EventTypeCategoryCode] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [FK_EventTypeCategoryEventType]
    FOREIGN KEY ([EventTypeCategoryCode])
    REFERENCES [dbo].[EventTypeCategories]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EventTypeCategoryEventType'
CREATE INDEX [IX_FK_EventTypeCategoryEventType]
ON [dbo].[EventTypes]
    ([EventTypeCategoryCode]);
GO