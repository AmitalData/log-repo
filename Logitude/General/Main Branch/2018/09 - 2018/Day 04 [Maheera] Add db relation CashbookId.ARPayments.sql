-- Creating foreign key on [CashbookId] in table 'ARPayments'
ALTER TABLE [dbo].[ARPayments]
ADD CONSTRAINT [FK_CashbookIdARPaymentCashBook]
    FOREIGN KEY ([CashbookId])
    REFERENCES [dbo].[CashBooks]([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CashbookIdARPaymentCashBook'
CREATE INDEX [IX_FK_CashbookIdARPaymentCashBook]
ON [dbo].[ARPayments]
    ([CashbookId]);
GO