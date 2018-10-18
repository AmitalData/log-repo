
---------------------add feature id to helper and tabs----------------------------------------------------
alter table ObjectTableHelperControls
add FeatureId varchar(15) null

alter table ObjectTabletabs
add FeatureId varchar(15) null

-- Creating foreign key on [FeatureId] in table 'ObjectTableHelperControls'
ALTER TABLE [dbo].[ObjectTableHelperControls]
ADD CONSTRAINT [FK_ObjectTableHelperControlFeature]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[Features]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjectTableHelperControlFeature'
CREATE INDEX [IX_FK_ObjectTableHelperControlFeature]
ON [dbo].[ObjectTableHelperControls]
    ([FeatureId]);
GO

-- Creating foreign key on [FeatureId] in table 'ObjectTableTabs'
ALTER TABLE [dbo].[ObjectTableTabs]
ADD CONSTRAINT [FK_ObjectTableTabFeature]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[Features]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjectTableTabFeature'
CREATE INDEX [IX_FK_ObjectTableTabFeature]
ON [dbo].[ObjectTableTabs]
    ([FeatureId]);
GO

---------------------------------------------------------------------------------------------------

-----------------------add objecttabletypes---------------------
-- Creating table 'ObjectTableTypes'
CREATE TABLE [dbo].[ObjectTableTypes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'ObjectTableTypes'
ALTER TABLE [dbo].[ObjectTableTypes]
ADD CONSTRAINT [PK_ObjectTableTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

insert into objecttabletypes(code,name)
values ('MD','Master Data')
insert into objecttabletypes(code,name)
values ('BR','Business Record')
----------------------------------------------------------------

-------------------------ObjectTable adjustments --------------------------------

alter table objecttables
add EnableSecurity bit not null default 0

alter table objecttables
add ObjectTableTypeCode varchar(4) null

update objecttables 
set ObjectTableTypeCode='MD'

ALTER TABLE objecttables
ALTER COLUMN ObjectTableTypeCode varchar(4) not null
 
 -- Creating foreign key on [ObjectTableTypeCode] in table 'ObjectTables'
ALTER TABLE [dbo].[ObjectTables]
ADD CONSTRAINT [FK_ObjectTableTypeObjectTable]
    FOREIGN KEY ([ObjectTableTypeCode])
    REFERENCES [dbo].[ObjectTableTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjectTableTypeObjectTable'
CREATE INDEX [IX_FK_ObjectTableTypeObjectTable]
ON [dbo].[ObjectTables]
    ([ObjectTableTypeCode]);
GO

----------------------------------------------------------------------------------


----------------------Create tables feature types and role types---------------------

-- Creating table 'FeatureTypes'
CREATE TABLE [dbo].[FeatureTypes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating table 'RoleTypes'
CREATE TABLE [dbo].[RoleTypes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'FeatureTypes'
ALTER TABLE [dbo].[FeatureTypes]
ADD CONSTRAINT [PK_FeatureTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'RoleTypes'
ALTER TABLE [dbo].[RoleTypes]
ADD CONSTRAINT [PK_RoleTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO


insert into [RoleTypes](code,name)
values('CU','Customer')
insert into [RoleTypes](code,name)
values('AG','Agent')
insert into [RoleTypes](code,name)
values('IN','Internal User')


insert into [FeatureTypes](code,name)
values('NEW','New')

insert into [FeatureTypes](code,name)
values('EDIT','Edit')

insert into [FeatureTypes](code,name)
values('READ','Read')

insert into [FeatureTypes](code,name)
values('ACT','Actions')

insert into [FeatureTypes](code,name)
values('SET','Settings')

insert into [FeatureTypes](code,name)
values('OTH','Others')



-------------------------------------------------------------------------------------


------------------------add role type and feature type for role and features------------------------

alter table features 
add FeatureTypeCode varchar(4) null

update features
set FeatureTypeCode='OTH'

alter table features 
alter column FeatureTypeCode varchar(4) not null

-- Creating foreign key on [FeatureTypeCode] in table 'Features'
ALTER TABLE [dbo].[Features]
ADD CONSTRAINT [FK_FeatureTypeFeature]
    FOREIGN KEY ([FeatureTypeCode])
    REFERENCES [dbo].[FeatureTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FeatureTypeFeature'
CREATE INDEX [IX_FK_FeatureTypeFeature]
ON [dbo].[Features]
    ([FeatureTypeCode]);
GO


alter table roles 
add RoleTypeCode varchar(4) null

update roles
set RoleTypeCode='IN'

alter table roles 
alter column RoleTypeCode varchar(4) not null

-- Creating foreign key on [RoleTypeCode] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_RoleTypeRole]
    FOREIGN KEY ([RoleTypeCode])
    REFERENCES [dbo].[RoleTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleTypeRole'
CREATE INDEX [IX_FK_RoleTypeRole]
ON [dbo].[Roles]
    ([RoleTypeCode]);
GO



-----------------------------------------------------------------------------------------------------

-----------------------------------------filling permission Type table-------------------------------

insert into PermissionTypes(code,name)
values('RDUP','Read/Update')

insert into PermissionTypes(code,name)
values('NOAC','No Access')

insert into PermissionTypes(code,name)
values('READ','Read')

------------------------------------------------------------------------------------------------------
-----------------------------------objectfield permission types---------------------------------------
alter table objectfields
add CustomerPermissionTypeCode varchar(15) null

alter table objectfields
add AgentPermissionTypeCode varchar(15) null


-- Creating foreign key on [AgentPermissionTypeCode] in table 'ObjectFields'
ALTER TABLE [dbo].[ObjectFields]
ADD CONSTRAINT [FK_AgentPermissionTypeObjectField]
    FOREIGN KEY ([AgentPermissionTypeCode])
    REFERENCES [dbo].[PermissionTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AgentPermissionTypeObjectField'
CREATE INDEX [IX_FK_AgentPermissionTypeObjectField]
ON [dbo].[ObjectFields]
    ([AgentPermissionTypeCode]);
GO

-- Creating foreign key on [CustomerPermissionTypeCode] in table 'ObjectFields'
ALTER TABLE [dbo].[ObjectFields]
ADD CONSTRAINT [FK_CustomerObjectFieldPermissionType]
    FOREIGN KEY ([CustomerPermissionTypeCode])
    REFERENCES [dbo].[PermissionTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerObjectFieldPermissionType'
CREATE INDEX [IX_FK_CustomerObjectFieldPermissionType]
ON [dbo].[ObjectFields]
    ([CustomerPermissionTypeCode]);
GO

------------------------------------------------------------------------------------------------------

-------------------------------------------Rename Edit To update--------------------------------------
update features
set code='UPDATE'
where code='EDIT'

------------------------------------------------------------------------------------------------------