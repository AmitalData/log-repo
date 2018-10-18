sp_RENAME '[dbo].[TranslationHeaders].Id' , 'Code', 'COLUMN'
sp_RENAME '[dbo].[Translations].TranslationHeaderId' , 'TranslationHeaderCode', 'COLUMN'


update translations set TranslationHeaderCode=(select code from translationheaders where tenant=0)

delete from TranslationHeaders where Tenant<>0

alter table translationheaders drop UQ_Tenant_Description

alter table translationheaders drop column tenant

ALTER TABLE [dbo].[Translations] DROP CONSTRAINT [FK_TranslationHeaderTranslation]; -- doesn't work online
drop index IX_FK_TranslationHeaderTranslation on Translations -- doesn't work online 

alter table translationheaders drop constraint PK_TranslationHeaders --doesn't work online 

update translationheaders set code='EN'

update translations set translationheadercode='EN'

-- Creating primary key on [Code] in table 'TranslationHeaders'
ALTER TABLE [dbo].[TranslationHeaders]
ADD CONSTRAINT [PK_TranslationHeaders]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [TranslationHeaderCode] in table 'Translations'
ALTER TABLE [dbo].[Translations]
ADD CONSTRAINT [FK_TranslationHeaderTranslation]
    FOREIGN KEY ([TranslationHeaderCode])
    REFERENCES [dbo].[TranslationHeaders]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TranslationHeaderTranslation'
CREATE INDEX [IX_FK_TranslationHeaderTranslation]
ON [dbo].[Translations]
    ([TranslationHeaderCode]);
GO


alter table translations
add TranslateDate datetime null

alter table translations
add TranslatedByUserId varchar(15) null

-- Creating foreign key on [TranslatedByUserId] in table 'Translations'
ALTER TABLE [dbo].[Translations]
ADD CONSTRAINT [FK_TranslatedByTranslationUser]
    FOREIGN KEY ([TranslatedByUserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TranslatedByTranslationUser'
CREATE INDEX [IX_FK_TranslatedByTranslationUser]
ON [dbo].[Translations]
    ([TranslatedByUserId]);
GO