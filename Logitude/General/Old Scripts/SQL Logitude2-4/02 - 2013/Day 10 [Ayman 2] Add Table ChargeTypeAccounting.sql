


-- Creating table 'ChargeTypeAccountings'
CREATE TABLE [dbo].ChargeTypeAccountings (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [VatTypeId]varchar(15) NOT NULL,
	[ChargeTypeId]varchar(15) NOT NULL,
	[PayableDebitAccount]varchar(15)  NULL,
	[ReceivableCreditAccount]varchar(15)  NULL
);
GO

-- Creating primary key on [Id] in table 'ChargeTypeAccountings'
ALTER TABLE [dbo].[ChargeTypeAccountings]
ADD CONSTRAINT [PK_ChargeTypeAccountings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [VatTypeId] in table 'ChargeTypeAccountings'
ALTER TABLE [dbo].[ChargeTypeAccountings]
ADD CONSTRAINT [FK_VatTypeChargeTypeAccounting]
    FOREIGN KEY ([VatTypeId])
    REFERENCES [dbo].[VatTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VatTypeChargeTypeAccounting'
CREATE INDEX [IX_FK_VatTypeChargeTypeAccounting]
ON [dbo].[ChargeTypeAccountings]
    ([VatTypeId]);
GO


-- Creating foreign key on [ChargeTypeId] in table 'ChargeTypeAccountings'
ALTER TABLE [dbo].[ChargeTypeAccountings]
ADD CONSTRAINT [FK_ChargeTypeChargeTypeAccounting]
    FOREIGN KEY ([ChargeTypeId])
    REFERENCES [dbo].[ChargesTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChargeTypeChargeTypeAccounting'
CREATE INDEX [IX_FK_ChargeTypeChargeTypeAccounting]
ON [dbo].[ChargeTypeAccountings]
    ([ChargeTypeId]);
GO