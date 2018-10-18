sp_RENAME 'EntityLastAccesses.AccessDate' , 'ActivityDate', 'COLUMN'
go

sp_RENAME 'EntityLastAccesses' , 'EntityLastActivities'
go

alter table EntityLastActivities add ActivityTypeCode varchar(4) not null
go

-- Creating table 'EntityLastActivityTypes'
CREATE TABLE [dbo].[EntityLastActivityTypes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL
);
GO

-- Creating primary key on [Code] in table 'EntityLastActivityTypes'
ALTER TABLE [dbo].[EntityLastActivityTypes]
ADD CONSTRAINT [PK_EntityLastActivityTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [ActivityTypeCode] in table 'EntityLastActivities'
ALTER TABLE [dbo].[EntityLastActivities]
ADD CONSTRAINT [FK_EntityLastActivityEntityLastActivityType]
    FOREIGN KEY ([ActivityTypeCode])
    REFERENCES [dbo].[EntityLastActivityTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityLastActivityEntityLastActivityType'
CREATE INDEX [IX_FK_EntityLastActivityEntityLastActivityType]
ON [dbo].[EntityLastActivities]
    ([ActivityTypeCode]);
GO
