alter table communicationlogs alter column createdbyuserid varchar(15) null

alter table communicationlogs drop constraint [FK_CommunicationLogUser]

drop index [IX_FK_CommunicationLogUser] on communicationlogs

-- Creating foreign key on [CreatedByUserId] in table 'CommunicationLogs'
ALTER TABLE [dbo].[CommunicationLogs]
ADD CONSTRAINT [FK_CommunicationLogUser]
    FOREIGN KEY ([CreatedByUserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CommunicationLogUser'
CREATE INDEX [IX_FK_CommunicationLogUser]
ON [dbo].[CommunicationLogs]
    ([CreatedByUserId]);
GO
