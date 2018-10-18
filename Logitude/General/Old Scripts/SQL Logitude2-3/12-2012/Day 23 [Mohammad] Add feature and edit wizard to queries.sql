alter table queries add FeatureId varchar(15) null
go
-- Creating foreign key on [FeatureId] in table 'Queries'
ALTER TABLE [dbo].[Queries]
ADD CONSTRAINT [FK_QueryFeature]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[Features]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QueryFeature'
CREATE INDEX [IX_FK_QueryFeature]
ON [dbo].[Queries]
    ([FeatureId]);
GO

alter table queries add EditWizardName varchar(100) null
go
