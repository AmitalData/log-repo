
alter table customers add BillToId varchar(15) null
go
update customers set BillToId=BillTold
go
alter table customers drop Column BillTold
go
-- Creating foreign key on [BillToId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_BillToCustomerCard]
    FOREIGN KEY ([BillToId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BillToCustomerCard'
CREATE INDEX [IX_FK_BillToCustomerCard]
ON [dbo].[Customers]
    ([BillToId]);
GO