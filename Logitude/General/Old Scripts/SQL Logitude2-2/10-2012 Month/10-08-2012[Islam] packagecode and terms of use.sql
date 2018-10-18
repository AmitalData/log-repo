--update tenantmanagements
--set packagecode =  'BUSN'

 

drop index [IX_FK_TenantPackage] on tenants

alter table tenants drop FK_TenantPackage

alter table tenants drop column packagecode



-- Creating table 'TermsofUseSignatures'
CREATE TABLE [dbo].[TermsofUseSignatures] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [SignedDatetime]datetime   NOT NULL,
    [ContactId]varchar(15)   NOT NULL,
    [TermsofUseVersion]int   NOT NULL
);
GO

-- Creating table 'TermsofUses'
CREATE TABLE [dbo].[TermsofUses] (
    [Version]int   NOT NULL,
    [Date]datetime   NOT NULL
);
GO





-- Creating primary key on [Id] in table 'TermsofUseSignatures'
ALTER TABLE [dbo].[TermsofUseSignatures]
ADD CONSTRAINT [PK_TermsofUseSignatures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Version] in table 'TermsofUses'
ALTER TABLE [dbo].[TermsofUses]
ADD CONSTRAINT [PK_TermsofUses]
    PRIMARY KEY CLUSTERED ([Version] ASC);
GO


-- Creating foreign key on [ContactId] in table 'TermsofUseSignatures'
ALTER TABLE [dbo].[TermsofUseSignatures]
ADD CONSTRAINT [FK_TermsofUseSignatureContact]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TermsofUseSignatureContact'
CREATE INDEX [IX_FK_TermsofUseSignatureContact]
ON [dbo].[TermsofUseSignatures]
    ([ContactId]);
GO

-- Creating foreign key on [TermsofUseVersion] in table 'TermsofUseSignatures'
ALTER TABLE [dbo].[TermsofUseSignatures]
ADD CONSTRAINT [FK_TermsofUseSignatureTermsofUse]
    FOREIGN KEY ([TermsofUseVersion])
    REFERENCES [dbo].[TermsofUses]
        ([Version])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TermsofUseSignatureTermsofUse'
CREATE INDEX [IX_FK_TermsofUseSignatureTermsofUse]
ON [dbo].[TermsofUseSignatures]
    ([TermsofUseVersion]);
GO