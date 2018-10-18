--EXCUTE ON GLOBAL DB

-- Creating table 'RecurringPeriods'
CREATE TABLE [dbo].[RecurringPeriods] (
	[Code] nvarchar(2)  NOT NULL,
	[Name] nvarchar(40)  NOT NULL,
	[SearchFields] nvarchar(1000)  NULL
);
GO

-- Creating primary key on [Code] in table 'RecurringPeriods'
ALTER TABLE [dbo].[RecurringPeriods]
ADD CONSTRAINT [PK_RecurringPeriods]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO

--Adding new properties to TenantManagement
alter table TenantManagements add [FreeUsers] int  NULL
go

alter table TenantManagements add [IsRecurring] bit  NULL 
go

alter table TenantManagements add [CreateDate] datetime NOT NULL default '2012-12-23 12:00:00'
go

alter table TenantManagements add [UpdateDate] datetime  NULL
go

alter table TenantManagements add [IsBlocked] bit  NULL 
go

alter table TenantManagements add [BlockDate] datetime  NULL
go

alter table TenantManagements add [BlockNote] nvarchar(250)  NULL
go

alter table TenantManagements add [PlimusAccount] nvarchar(25)  NULL
go

alter table TenantManagements add [MainContract] nvarchar(25)  NULL
go

alter table TenantManagements add [TemporalStartDate] datetime  NULL
go

alter table TenantManagements add [TemporalEndDate] datetime  NULL
go

alter table TenantManagements add [RecurringPeriodCode] nvarchar(2)  NULL
go

alter table TenantManagements add [TemporalPackageCode] nvarchar(4)  NULL
go

	-- Creating foreign key on [RecurringPeriodCode] in table 'TenantManagements'
ALTER TABLE [dbo].[TenantManagements]
ADD CONSTRAINT [FK_RecurringPeriodTenantManagement]
	FOREIGN KEY ([RecurringPeriodCode])
	REFERENCES [dbo].[RecurringPeriods]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RecurringPeriodTenantManagement'
CREATE INDEX [IX_FK_RecurringPeriodTenantManagement]
ON [dbo].[TenantManagements]
	([RecurringPeriodCode]);
GO
