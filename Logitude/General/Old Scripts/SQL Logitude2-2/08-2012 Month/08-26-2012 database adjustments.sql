alter table shipments
add AWBPlace varchar(165) null

alter table shipments
add AWBSignature varchar(40) null

alter table shipments 
add IssuingCarrierIATACode nvarchar(15) null

-- Creating table 'AWBSpecialHandlingCodes'
CREATE TABLE [dbo].[AWBSpecialHandlingCodes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(200)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'AWBSpecialHandlingCodes'
ALTER TABLE [dbo].[AWBSpecialHandlingCodes]
ADD CONSTRAINT [PK_AWBSpecialHandlingCodes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO


alter table shipments
add AWBSpecialHandlingCodeCode1 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode2 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode3 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode4 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode5 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode6 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode7 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode8 varchar(4) null
alter table shipments
add AWBSpecialHandlingCodeCode9 varchar(4) null


-- Creating foreign key on [AWBSpecialHandlingCodeCode1] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment1]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode1])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment1'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment1]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode1]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode2] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment2]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode2])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment2'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment2]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode2]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode3] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment3]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode3])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment3'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment3]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode3]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode4] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment4]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode4])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment4'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment4]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode4]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode5] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment5]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode5])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment5'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment5]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode5]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode6] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment6]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode6])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment6'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment6]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode6]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode7] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment7]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode7])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment7'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment7]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode7]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode8] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment8]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode8])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment8'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment8]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode8]);
GO

-- Creating foreign key on [AWBSpecialHandlingCodeCode9] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBHandlingCodeShipment9]
    FOREIGN KEY ([AWBSpecialHandlingCodeCode9])
    REFERENCES [dbo].[AWBSpecialHandlingCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBHandlingCodeShipment9'
CREATE INDEX [IX_FK_AWBHandlingCodeShipment9]
ON [dbo].[Shipments]
    ([AWBSpecialHandlingCodeCode9]);
GO


--alter table AWBChargesCodes alter column name varchar(100) not null 