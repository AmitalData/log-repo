
-- RUN THIS SCRIPT ON GLOBAL DATABASE


-- Creating table 'TenantManagements'
CREATE TABLE [dbo].[TenantManagements] (
    [Id] int  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [PackageCode] varchar(4)  NULL,
    [TrialStartDate] datetime  NULL,
    [TrialEndDate] datetime  NULL,
    [ProductionStartDate] datetime  NULL,
    [ProductionEndDate] datetime  NULL,
    [PaidUntilDate] datetime  NULL,
    [IsTrial] bit  NOT NULL,
    [NumberOfUsers] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'TenantManagements'
ALTER TABLE [dbo].[TenantManagements]
ADD CONSTRAINT [PK_TenantManagements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

DECLARE @Id int
DECLARE @CompanyName varchar(100)


DECLARE c1 CURSOR READ_ONLY
FOR
SELECT Id,companyname
FROM GlobalTenants

OPEN c1

	FETCH NEXT FROM c1
	INTO @Id,@CompanyName


WHILE @@FETCH_STATUS = 0


BEGIN


insert into tenantmanagements ([Id],[Name],[PackageCode],[TrialStartDate],[TrialEndDate],[ProductionStartDate],[ProductionEndDate],[PaidUntilDate],[IsTrial],[NumberOfUsers])
values(@Id,@CompanyName,null,null,null,null,null,null,0,1)


	FETCH NEXT FROM c1
	INTO @Id,@CompanyName
END

CLOSE c1
DEALLOCATE c1




-- Creating foreign key on [Id] in table 'TenantManagements'
ALTER TABLE [dbo].[TenantManagements]
ADD CONSTRAINT [FK_GlobalTenantTenantManagement]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GlobalTenants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO