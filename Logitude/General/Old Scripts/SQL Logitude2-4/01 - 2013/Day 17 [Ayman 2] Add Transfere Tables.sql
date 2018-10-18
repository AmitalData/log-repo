

-- Creating table 'AccountingTransferHeaders'
CREATE TABLE [dbo].[AccountingTransferHeaders] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [TransferNumber]varchar(20)   NOT NULL,
    [TransferDate]datetime   NULL,
    [FileName]varchar(40)   NOT NULL,
    [AccountingTransferTypeCode]varchar(4)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL,
    [UserId]varchar(15)   NOT NULL
);
GO

-- Creating table 'AccountingTransferLines'
CREATE TABLE [dbo].[AccountingTransferLines] (
    [Id]varchar(15)   NOT NULL,
    [EntityId]varchar(15)   NOT NULL,
    [AccountingTransferHeaderId]varchar(15)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating table 'AccountingTransferTypes'
CREATE TABLE [dbo].[AccountingTransferTypes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO


-- Creating primary key on [Id] in table 'AccountingTransferHeaders'
ALTER TABLE [dbo].[AccountingTransferHeaders]
ADD CONSTRAINT [PK_AccountingTransferHeaders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountingTransferLines'
ALTER TABLE [dbo].[AccountingTransferLines]
ADD CONSTRAINT [PK_AccountingTransferLines]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'AccountingTransferTypes'
ALTER TABLE [dbo].[AccountingTransferTypes]
ADD CONSTRAINT [PK_AccountingTransferTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [AccountingTransferTypeCode] in table 'AccountingTransferHeaders'
ALTER TABLE [dbo].[AccountingTransferHeaders]
ADD CONSTRAINT [FK_AccountingTransferHeaderTransferType]
    FOREIGN KEY ([AccountingTransferTypeCode])
    REFERENCES [dbo].[AccountingTransferTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountingTransferHeaderTransferType'
CREATE INDEX [IX_FK_AccountingTransferHeaderTransferType]
ON [dbo].[AccountingTransferHeaders]
    ([AccountingTransferTypeCode]);
GO

-- Creating foreign key on [AccountingTransferHeaderId] in table 'AccountingTransferLines'
ALTER TABLE [dbo].[AccountingTransferLines]
ADD CONSTRAINT [FK_AccountingTransferHeaderAccountingTransferLine]
    FOREIGN KEY ([AccountingTransferHeaderId])
    REFERENCES [dbo].[AccountingTransferHeaders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountingTransferHeaderAccountingTransferLine'
CREATE INDEX [IX_FK_AccountingTransferHeaderAccountingTransferLine]
ON [dbo].[AccountingTransferLines]
    ([AccountingTransferHeaderId]);
GO

-- Creating foreign key on [UserId] in table 'AccountingTransferHeaders'
ALTER TABLE [dbo].[AccountingTransferHeaders]
ADD CONSTRAINT [FK_UserAccountingTransferHeader]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountingTransferHeader'
CREATE INDEX [IX_FK_UserAccountingTransferHeader]
ON [dbo].[AccountingTransferHeaders]
    ([UserId]);
GO
