
alter table communicationlogs alter column  [InOut]varchar(3)   NOT NULL

alter table UserLoginLogs add  [ComputerId]varchar(40)   NOT NULL default(' ')
 
-- Creating table 'UserLastLogins'
CREATE TABLE [dbo].[UserLastLogins] (
    [ComputerId]varchar(40)   NOT NULL,
    [LoginDateTime]datetime   NULL,
    [Tenant]int   NOT NULL,
    [Id]varchar(15)   NOT NULL
);
GO

-- Creating primary key on [Id] in table 'UserLastLogins'
ALTER TABLE [dbo].[UserLastLogins]
ADD CONSTRAINT [PK_UserLastLogins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [Id] in table 'UserLastLogins'
ALTER TABLE [dbo].[UserLastLogins]
ADD CONSTRAINT [FK_UserLastLoginUser]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO