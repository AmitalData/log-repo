 --EXCUTE ON GLOBAL DB
 
 alter table TenantManagements add [PaymentMethodCode] varchar(2)  NULL
 go
 alter table TenantManagements add[PaymentChannelCode] varchar(2)  NULL
 go
 alter table TenantManagements add [LicensePrice] float  NULL
 go
sp_RENAME 'TenantManagements.[ProductionStartDate]' , 'FirstPaymentDate', 'COLUMN'
go

	-- Creating table 'PaymentMethods'
CREATE TABLE [dbo].[PaymentMethods] (
	[Code] varchar(2)  NOT NULL,
	[Name] nvarchar(40)  NOT NULL,
	[SearchFields] nvarchar(1000)  NULL
);
GO

-- Creating table 'PaymentChannels'
CREATE TABLE [dbo].[PaymentChannels] (
	[Code] varchar(2)  NOT NULL,
	[Name] nvarchar(40)  NOT NULL,
	[SearchFields] nvarchar(1000)  NULL
);
GO

-- Creating primary key on [Code] in table 'PaymentMethods'
ALTER TABLE [dbo].[PaymentMethods]
ADD CONSTRAINT [PK_PaymentMethods]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'PaymentChannels'
ALTER TABLE [dbo].[PaymentChannels]
ADD CONSTRAINT [PK_PaymentChannels]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [PaymentMethodCode] in table 'TenantManagements'
ALTER TABLE [dbo].[TenantManagements]
ADD CONSTRAINT [FK_PaymentMethodTenantManagement]
	FOREIGN KEY ([PaymentMethodCode])
	REFERENCES [dbo].[PaymentMethods]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentMethodTenantManagement'
CREATE INDEX [IX_FK_PaymentMethodTenantManagement]
ON [dbo].[TenantManagements]
	([PaymentMethodCode]);
GO

-- Creating foreign key on [PaymentChannelCode] in table 'TenantManagements'
ALTER TABLE [dbo].[TenantManagements]
ADD CONSTRAINT [FK_PaymentChannelTenantManagement]
	FOREIGN KEY ([PaymentChannelCode])
	REFERENCES [dbo].[PaymentChannels]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentChannelTenantManagement'
CREATE INDEX [IX_FK_PaymentChannelTenantManagement]
ON [dbo].[TenantManagements]
	([PaymentChannelCode]);
GO
