-- EXECUTE IN GLOBAL DATABASE ------------------------------------------

-- Creating table 'AnalyzeQueues'
CREATE TABLE [dbo].[AnalyzeQueues] (
    [Id] varchar(15)  NOT NULL,
    [MessageBody] varbinary(max)  NOT NULL,
    [From] varchar(40)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [FileSize] float  NOT NULL,
    [Status] varchar(4)  NOT NULL,
    [ErrorMessage] varchar(500)  NULL,
    [Tenant] int  NULL,
    [CommunicationLogId] varchar(15)  NULL,
    [Subject] varchar(40)  NULL,
    [Retries] int  NOT NULL,
    [ConnectedToTenant] bit  NOT NULL,
    [ConnectedToEntity] bit  NOT NULL
);
GO

-- Creating table 'AnalyzeQueueStatus'
CREATE TABLE [dbo].[AnalyzeQueueStatus] (
    [Code] varchar(4)  NOT NULL,
    [Name] varchar(40)  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'AnalyzeQueues'
ALTER TABLE [dbo].[AnalyzeQueues]
ADD CONSTRAINT [PK_AnalyzeQueues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'AnalyzeQueueStatus'
ALTER TABLE [dbo].[AnalyzeQueueStatus]
ADD CONSTRAINT [PK_AnalyzeQueueStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [Status] in table 'AnalyzeQueues'
ALTER TABLE [dbo].[AnalyzeQueues]
ADD CONSTRAINT [FK_AnalyzeQueueAnalyzeQueueStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[AnalyzeQueueStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AnalyzeQueueAnalyzeQueueStatus'
CREATE INDEX [IX_FK_AnalyzeQueueAnalyzeQueueStatus]
ON [dbo].[AnalyzeQueues]
    ([Status]);
GO

