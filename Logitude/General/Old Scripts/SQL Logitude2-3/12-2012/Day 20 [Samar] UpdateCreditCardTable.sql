IF OBJECT_ID(N'[dbo].[FK_CreditCardTypeAPPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[APPayments] DROP CONSTRAINT [FK_CreditCardTypeAPPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditCardTypeARPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ARPayments] DROP CONSTRAINT [FK_CreditCardTypeARPayment];
GO

drop index [IX_FK_CreditCardTypeARPayment] on ARPayments
go

drop index [IX_FK_CreditCardTypeAPPayment] on APPayments
go

IF OBJECT_ID(N'[dbo].[CreditCardTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardTypes];
GO

-- Creating table 'CreditCardTypes'
CREATE TABLE [dbo].[CreditCardTypes] (
    [Code]varchar(2)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL,
    [Tenant]int   NOT NULL,
    [Id]varchar(15)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'CreditCardTypes'
ALTER TABLE [dbo].[CreditCardTypes]
ADD CONSTRAINT [PK_CreditCardTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

--Add foregin keys
alter table ARPayments add [CreditCardTypeId] varchar(15)   NULL
go

alter table APPayments add [CreditCardTypeId] varchar(15)   NULL
go

-- Creating foreign key on [CreditCardTypeId] in table 'ARPayments'
ALTER TABLE [dbo].[ARPayments]
ADD CONSTRAINT [FK_CreditCardTypeARPayment]
    FOREIGN KEY ([CreditCardTypeId])
    REFERENCES [dbo].[CreditCardTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditCardTypeARPayment'
CREATE INDEX [IX_FK_CreditCardTypeARPayment]
ON [dbo].[ARPayments]
    ([CreditCardTypeId]);
GO

-- Creating foreign key on [CreditCardTypeId] in table 'APPayments'
ALTER TABLE [dbo].[APPayments]
ADD CONSTRAINT [FK_CreditCardTypeAPPayment]
    FOREIGN KEY ([CreditCardTypeId])
    REFERENCES [dbo].[CreditCardTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditCardTypeAPPayment'
CREATE INDEX [IX_FK_CreditCardTypeAPPayment]
ON [dbo].[APPayments]
    ([CreditCardTypeId]);
GO

-- objectFields and textCodes
delete from ObjectFields where FieldName = 'creditCardTypeCode'

delete from TextCodes where Code = 'ARPayment.F.creditCardTypeCode'
delete from TextCodes where Code = 'APPayment.F.creditCardTypeCode'





