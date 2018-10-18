
-- Creating table 'ContactLastLogins'
CREATE TABLE [dbo].[ContactLastLogins] (
    [ComputerId]varchar(40)   NOT NULL,
    [LoginDateTime]datetime   NULL,
    [Tenant]int   NOT NULL,
    [Id]varchar(15)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'ContactLastLogins'
ALTER TABLE [dbo].[ContactLastLogins]
ADD CONSTRAINT [PK_ContactLastLogins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating table 'ContactLoginLogs'
CREATE TABLE [dbo].[ContactLoginLogs] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [IP]varchar(15)   NULL,
    [Browser]varchar(40)   NULL,
    [ContactId]varchar(15)   NOT NULL,
    [LocalDateTime]datetime   NULL,
    [GMTDateTime]datetime   NULL,
    [ContactAgent]nvarchar(400)   NULL,
    [ComputerId]varchar(40)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'ContactLoginLogs'
ALTER TABLE [dbo].[ContactLoginLogs]
ADD CONSTRAINT [PK_ContactLoginLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


--ContactLastLogins Contact
ALTER TABLE [dbo].[ContactLastLogins]
ADD CONSTRAINT [FK_ContactLastLoginContact]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- ContactLoginLogs Contacts
ALTER TABLE [dbo].[ContactLoginLogs]
ADD CONSTRAINT [FK_ContactLoginLogContact]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContactLoginLogContact'
CREATE INDEX [IX_FK_ContactLoginLogContact]
ON [dbo].[ContactLoginLogs]
    ([ContactId]);
GO
