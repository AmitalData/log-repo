alter table documenttypes
add CustomerRoleId varchar(15) null
alter table documenttypes
add AgentRoleId varchar(15) null

alter table EventTypes
add CustomerRoleId varchar(15) null
alter table EventTypes
add AgentRoleId varchar(15) null

alter table Objectfields
add CustomerRoleId varchar(15) null
alter table Objectfields
add AgentRoleId varchar(15) null

-- Creating foreign key on [CustomerRoleId] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [FK_CustomerDocumentTypeRole]
    FOREIGN KEY ([CustomerRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerDocumentTypeRole'
CREATE INDEX [IX_FK_CustomerDocumentTypeRole]
ON [dbo].[DocumentTypes]
    ([CustomerRoleId]);
GO

-- Creating foreign key on [AgentRoleId] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [FK_AgentDocumentTypeRole]
    FOREIGN KEY ([AgentRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AgentDocumentTypeRole'
CREATE INDEX [IX_FK_AgentDocumentTypeRole]
ON [dbo].[DocumentTypes]
    ([AgentRoleId]);
GO

-- Creating foreign key on [CustomerRoleId] in table 'ObjectFields'
ALTER TABLE [dbo].[ObjectFields]
ADD CONSTRAINT [FK_CustomerObjectFieldRole]
    FOREIGN KEY ([CustomerRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerObjectFieldRole'
CREATE INDEX [IX_FK_CustomerObjectFieldRole]
ON [dbo].[ObjectFields]
    ([CustomerRoleId]);
GO

-- Creating foreign key on [AgentRoleId] in table 'ObjectFields'
ALTER TABLE [dbo].[ObjectFields]
ADD CONSTRAINT [FK_AgentObjectFieldRole]
    FOREIGN KEY ([AgentRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AgentObjectFieldRole'
CREATE INDEX [IX_FK_AgentObjectFieldRole]
ON [dbo].[ObjectFields]
    ([AgentRoleId]);
GO

-- Creating foreign key on [CustomerRoleId] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [FK_CustomerEventTypeRole]
    FOREIGN KEY ([CustomerRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerEventTypeRole'
CREATE INDEX [IX_FK_CustomerEventTypeRole]
ON [dbo].[EventTypes]
    ([CustomerRoleId]);
GO

-- Creating foreign key on [AgentRoleId] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [FK_AgentEventTypeRole]
    FOREIGN KEY ([AgentRoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AgentEventTypeRole'
CREATE INDEX [IX_FK_AgentEventTypeRole]
ON [dbo].[EventTypes]
    ([AgentRoleId]);
GO

alter table queries
add Internal bit not null default 0
alter table queries
add Customer bit not null default 0
alter table queries
add Agent bit not null default 0


-- Creating table 'PermissionTypes'
CREATE TABLE [dbo].[PermissionTypes] (
    [Code]varchar(15)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
	[SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'PermissionTypes'
ALTER TABLE [dbo].[PermissionTypes]
ADD CONSTRAINT [PK_PermissionTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

