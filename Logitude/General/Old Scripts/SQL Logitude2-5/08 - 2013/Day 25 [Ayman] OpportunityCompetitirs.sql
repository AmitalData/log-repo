
IF OBJECT_ID(N'[dbo].[OpportunityProductCompetitors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OpportunityProductCompetitors];
GO

-- Creating table 'OpportunityProductCompetitors'
CREATE TABLE [dbo].[OpportunityProductCompetitors] (
    [OpportunityId]varchar(15)   NOT NULL,
	[OpportunityProductTypeCode] [varchar](2) not null,
	[CompetitorId]varchar(15)   NOT NULL,
	[Tenant] [int] not null,
	[Notes] nvarchar(255) null,
	[ProductPeriodCode] varchar(2) null,
	primary key ([OpportunityId], [OpportunityProductTypeCode], [CompetitorId])
);
GO

-- Creating foreign key on [OpportunityId]
ALTER TABLE [dbo].[OpportunityProductCompetitors]
ADD CONSTRAINT [FK_OpportunityProductCompetitor_Opportunity]
    FOREIGN KEY ([OpportunityId])
    REFERENCES [dbo].[Opportunities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CompetitorId]
ALTER TABLE [dbo].[OpportunityProductCompetitors]
ADD CONSTRAINT [FK_OpportunityProductCompetitor_Competitor]
    FOREIGN KEY ([CompetitorId])
    REFERENCES [dbo].[Competitors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OpportunityProductTypeCode]
ALTER TABLE [dbo].[OpportunityProductCompetitors]
ADD CONSTRAINT [FK_OpportunityProductCompetitor_OpportunityProductType]
    FOREIGN KEY ([OpportunityProductTypeCode])
    REFERENCES [dbo].[OpportunityProductTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ProductPeriodCode]
ALTER TABLE [dbo].[OpportunityProductCompetitors]
ADD CONSTRAINT [FK_OpportunityProductCompetitor_OpportunityProductPeriod]
    FOREIGN KEY ([ProductPeriodCode])
    REFERENCES [dbo].[OpportunityProductPeriods]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


