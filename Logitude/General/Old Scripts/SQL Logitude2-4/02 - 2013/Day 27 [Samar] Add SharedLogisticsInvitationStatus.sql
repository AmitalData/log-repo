--excute on Logitude2-4_Main

-- Creating table 'SharedLogisticsInvitationStatus'
CREATE TABLE [dbo].[SharedLogisticsInvitationStatus] (
    [Code] int  NOT NULL,
    [Name] varchar (40) NOT NULL,
    [SearchFields] nvarchar(1000) NULL
);
GO

-- Creating primary key on [Code] in table 'SharedLogisticsInvitationStatus'
ALTER TABLE [dbo].[SharedLogisticsInvitationStatus]
ADD CONSTRAINT [PK_SharedLogisticsInvitationStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Add foreign key [SharedLogisticsInvitationStatusCode] to 'Cards'
alter table Cards add [SharedLogisticsInvitationStatusCode] int NULL
go
alter table Cards add [InvitationDate] DateTime NULL
go

-- Creating foreign key on [SharedLogisticsInvitationStatusCode] in table 'Cards'
ALTER TABLE [dbo].[Cards]
ADD CONSTRAINT [FK_SharedLogisticsInvitationStatusCard]
    FOREIGN KEY ([SharedLogisticsInvitationStatusCode])
    REFERENCES [dbo].[SharedLogisticsInvitationStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SharedLogisticsInvitationStatusCard'
CREATE INDEX [IX_FK_SharedLogisticsInvitationStatusCard]
ON [dbo].[Cards]
    ([SharedLogisticsInvitationStatusCode]);
GO





