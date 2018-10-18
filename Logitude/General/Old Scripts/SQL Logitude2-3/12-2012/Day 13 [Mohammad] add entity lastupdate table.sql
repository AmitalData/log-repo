-- Creating table 'EntityLastUpdates'
CREATE TABLE [dbo].[EntityLastUpdates] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [EntityId]varchar(15)   NOT NULL,
    [ObjectTableId]varchar(15)   NOT NULL,
    [UpdatedByUserId]varchar(15)   NOT NULL,
    [UpdateDate]datetime   NOT NULL,
	[EntityGUID]varchar(40)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'EntityLastUpdates'
ALTER TABLE [dbo].[EntityLastUpdates]
ADD CONSTRAINT [PK_EntityLastUpdates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [ObjectTableId] in table 'EntityLastUpdates'
ALTER TABLE [dbo].[EntityLastUpdates]
ADD CONSTRAINT [FK_EntityLastUpdateObjectTable]
    FOREIGN KEY ([ObjectTableId])
    REFERENCES [dbo].[ObjectTables]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityLastUpdateObjectTable'
CREATE INDEX [IX_FK_EntityLastUpdateObjectTable]
ON [dbo].[EntityLastUpdates]
    ([ObjectTableId]);
GO

-- Creating foreign key on [UpdatedByUserId] in table 'EntityLastUpdates'
ALTER TABLE [dbo].[EntityLastUpdates]
ADD CONSTRAINT [FK_EntityLastUpdateUser]
    FOREIGN KEY ([UpdatedByUserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityLastUpdateUser'
CREATE INDEX [IX_FK_EntityLastUpdateUser]
ON [dbo].[EntityLastUpdates]
    ([UpdatedByUserId]);
GO
