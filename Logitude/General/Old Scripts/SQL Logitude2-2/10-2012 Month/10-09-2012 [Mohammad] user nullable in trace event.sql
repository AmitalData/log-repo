alter table traceevents
drop [FK_TraceEventObjectTable]

drop index [IX_FK_TraceEventObjectTable] on traceevents

alter table traceevents
alter column UserId varchar(15) null
-- Creating foreign key on [ObjectTableId] in table 'TraceEvents'
ALTER TABLE [dbo].[TraceEvents]
ADD CONSTRAINT [FK_TraceEventObjectTable]
    FOREIGN KEY ([ObjectTableId])
    REFERENCES [dbo].[ObjectTables]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TraceEventObjectTable'
CREATE INDEX [IX_FK_TraceEventObjectTable]
ON [dbo].[TraceEvents]
    ([ObjectTableId]);
GO