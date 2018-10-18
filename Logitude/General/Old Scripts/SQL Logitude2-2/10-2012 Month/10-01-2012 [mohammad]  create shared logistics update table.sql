-- Creating table 'SharedLogisticsUpdates'
CREATE TABLE [dbo].[SharedLogisticsUpdates] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [ObjectTableId]varchar(15)   NOT NULL,
    [EntityId]varchar(15)   NOT NULL,
    [ReceivedDate]datetime   NOT NULL,
    [ReceivedFrom]varchar(40)   NOT NULL,
    [DocumentId]varchar(15)   NULL,
    [Subject]varchar(50)   NULL,
    [Read]bit   NOT NULL,
    [Status]varchar(4)   NOT NULL,
    [HandledByUserId]varchar(15)   NULL,
    [HandledDate]datetime   NULL
);
GO

-- Creating table 'SharedLogisticsUpdateStatus'
CREATE TABLE [dbo].[SharedLogisticsUpdateStatus] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Id] in table 'SharedLogisticsUpdates'
ALTER TABLE [dbo].[SharedLogisticsUpdates]
ADD CONSTRAINT [PK_SharedLogisticsUpdates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'SharedLogisticsUpdateStatus'
ALTER TABLE [dbo].[SharedLogisticsUpdateStatus]
ADD CONSTRAINT [PK_SharedLogisticsUpdateStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [ObjectTableId] in table 'SharedLogisticsUpdates'
ALTER TABLE [dbo].[SharedLogisticsUpdates]
ADD CONSTRAINT [FK_SharedLogisticsUpdateObjectTable]
    FOREIGN KEY ([ObjectTableId])
    REFERENCES [dbo].[ObjectTables]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SharedLogisticsUpdateObjectTable'
CREATE INDEX [IX_FK_SharedLogisticsUpdateObjectTable]
ON [dbo].[SharedLogisticsUpdates]
    ([ObjectTableId]);
GO

-- Creating foreign key on [DocumentId] in table 'SharedLogisticsUpdates'
ALTER TABLE [dbo].[SharedLogisticsUpdates]
ADD CONSTRAINT [FK_SharedLogisticsUpdateDocument]
    FOREIGN KEY ([DocumentId])
    REFERENCES [dbo].[Documents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SharedLogisticsUpdateDocument'
CREATE INDEX [IX_FK_SharedLogisticsUpdateDocument]
ON [dbo].[SharedLogisticsUpdates]
    ([DocumentId]);
GO

-- Creating foreign key on [Status] in table 'SharedLogisticsUpdates'
ALTER TABLE [dbo].[SharedLogisticsUpdates]
ADD CONSTRAINT [FK_SharedLogisticsUpdateSharedLogisticsUpdateStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[SharedLogisticsUpdateStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SharedLogisticsUpdateSharedLogisticsUpdateStatus'
CREATE INDEX [IX_FK_SharedLogisticsUpdateSharedLogisticsUpdateStatus]
ON [dbo].[SharedLogisticsUpdates]
    ([Status]);
GO

-- Creating foreign key on [HandledByUserId] in table 'SharedLogisticsUpdates'
ALTER TABLE [dbo].[SharedLogisticsUpdates]
ADD CONSTRAINT [FK_SharedLogisticsUpdateUser]
    FOREIGN KEY ([HandledByUserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SharedLogisticsUpdateUser'
CREATE INDEX [IX_FK_SharedLogisticsUpdateUser]
ON [dbo].[SharedLogisticsUpdates]
    ([HandledByUserId]);
GO
