
-- Creating table 'UserPermittedBranches'
CREATE TABLE [dbo].[UserPermittedBranches] (
    [Id]varchar(15)   NOT NULL,
    [UserId]varchar(15)   NOT NULL,
    [BranchId]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL
);
GO

 
ALTER TABLE [dbo].[UserPermittedBranches]
ADD CONSTRAINT [PK_UserPermittedBranches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

 
ALTER TABLE [dbo].[UserPermittedBranches]
ADD CONSTRAINT [FK_UserPermittedBranchUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


CREATE INDEX [IX_FK_UserPermittedBranchUser]
ON [dbo].[UserPermittedBranches]
    ([UserId]);
GO

ALTER TABLE [dbo].[UserPermittedBranches]
ADD CONSTRAINT [FK_BranchUserPermittedBranch]
    FOREIGN KEY ([BranchId])
    REFERENCES [dbo].[Branches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


CREATE INDEX [IX_FK_BranchUserPermittedBranch]
ON [dbo].[UserPermittedBranches]
    ([BranchId]);
GO

alter table [dbo].[Users] add [IsBranchRestricted]bit   NOT NULL default(0)