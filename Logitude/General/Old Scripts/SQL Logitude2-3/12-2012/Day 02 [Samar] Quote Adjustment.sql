 
 alter table Quotes add [FromPartnerId] varchar(15)   NULL
 alter table Quotes add [ToPartnerId] varchar(15)   NULL
 alter table Quotes add [FromPartnerAddressId] varchar(15)   NULL
 alter table Quotes add [ToPartnerAddressId] varchar(15)   NULL

 -- Creating foreign key on [FromPartnerId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_FromPartnerCardQuote]
    FOREIGN KEY ([FromPartnerId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromPartnerCardQuote'
CREATE INDEX [IX_FK_FromPartnerCardQuote]
ON [dbo].[Quotes]
    ([FromPartnerId]);
GO

-- Creating foreign key on [ToPartnerId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ToPartnerCardQuote]
    FOREIGN KEY ([ToPartnerId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToPartnerCardQuote'
CREATE INDEX [IX_FK_ToPartnerCardQuote]
ON [dbo].[Quotes]
    ([ToPartnerId]);
GO

-- Creating foreign key on [FromPartnerAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_FromPartnerAddressQuote]
    FOREIGN KEY ([FromPartnerAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromPartnerAddressQuote'
CREATE INDEX [IX_FK_FromPartnerAddressQuote]
ON [dbo].[Quotes]
    ([FromPartnerAddressId]);
GO

-- Creating foreign key on [ToPartnerAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ToPartnerAddressQuote]
    FOREIGN KEY ([ToPartnerAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToPartnerAddressQuote'
CREATE INDEX [IX_FK_ToPartnerAddressQuote]
ON [dbo].[Quotes]
    ([ToPartnerAddressId]);
GO