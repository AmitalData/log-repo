

-- Run on Main db

delete from ObjectFields where ObjectTableId = (Select Id from ObjectTables where Name = 'AWBOCI')
go

delete from ObjectFields where ObjectTableId = (Select Id from ObjectTables where Name = 'AWBCustomsInfo')
go

delete from ObjectFields where ObjectTableId = (Select Id from ObjectTables where Name = 'AWBCustomsInformation')
go

select * from features 
where NameTextCodeId in (select id from TextCodes where code like 'AWBOCI%')

select * from RoleFeatures 
where FeatureId in (select id from features 
where NameTextCodeId in (select id from TextCodes where code like 'AWBOCI%'))

delete from RoleFeatures 
where FeatureId in (select id from features 
where NameTextCodeId in (select id from TextCodes where code like 'AWBOCI%'))

delete from Features
where NameTextCodeId in (select id from TextCodes where code like 'AWBOCI%')

delete from TextCodes where code like 'AWBOCI%'
go


delete from TextCodes where code like 'AWBCustomsInfo%'
go

delete from ObjectTables where Name = 'AWBCustomsInfo'
go

delete from ObjectTables where Name = 'AWBCustomsInformation'
go

delete from ObjectTables where Name = 'AWBOCI'
go

IF OBJECT_ID(N'[dbo].[AWBOCIs]', 'U') IS NOT NULL
    DROP TABLE [dbo].AWBOCIs;
GO

IF OBJECT_ID(N'[dbo].[AWBCustomsInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].AWBCustomsInfos;
GO

IF OBJECT_ID(N'[dbo].[AWBCustomsInformations]', 'U') IS NOT NULL
    DROP TABLE [dbo].AWBCustomsInformations;
GO

IF OBJECT_ID(N'[dbo].[AWBInformations]', 'U') IS NOT NULL
    DROP TABLE [dbo].AWBInformations;
GO


-- Creating table 'AWBOCIs'
CREATE TABLE [dbo].AWBOCIs (
    [Id]varchar(15) NOT NULL,
	[Tenant]int NOT NULL,
	[ShipmentId]varchar(15) NOT NULL,
    [CountryId]varchar(15) NULL,
    [AWBCustomsInformationCode]varchar(2) NULL,
    [AWBInformationCode]varchar(3) NULL,
    [SupplementaryCustomsInfo]varchar(35) NOT NULL,
);
GO

-- Creating table 'AWBCustomsInformations'
CREATE TABLE [dbo].AWBCustomsInformations (
    [Code]varchar(2) NOT NULL,
    [Name]varchar(200) NOT NULL,
    [SearchFields]nvarchar(1000) NULL,
);
GO

-- Creating table 'AWBInformations'
CREATE TABLE [dbo].AWBInformations (
    [Code]varchar(3) NOT NULL,
    [Name]varchar(200) NOT NULL,
    [SearchFields]nvarchar(1000) NULL,
);
GO

-- Creating primary key on [Id] in table 'AWBOCIs'
ALTER TABLE [dbo].[AWBOCIs]
ADD CONSTRAINT [PK_AWBOCIs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'AWBCustomsInformations'
ALTER TABLE [dbo].[AWBCustomsInformations]
ADD CONSTRAINT [PK_AWBCustomsInformations]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'AWBInformations'
ALTER TABLE [dbo].[AWBInformations]
ADD CONSTRAINT [PK_AWBInformations]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [ShipmentId] in table 'AWBOCIs'
ALTER TABLE [dbo].[AWBOCIs]
ADD CONSTRAINT [FK_AWBOCIShipment]
    FOREIGN KEY ([ShipmentId])
    REFERENCES [dbo].[Shipments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
go

-- Creating foreign key on [CountryId] in table 'AWBOCIs'
ALTER TABLE [dbo].[AWBOCIs]
ADD CONSTRAINT [FK_AWBOCICountry]
    FOREIGN KEY ([CountryId])
    REFERENCES [dbo].[Countries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
go

-- Creating foreign key on [AWBCustomsInformationCode] in table 'AWBOCIs'
ALTER TABLE [dbo].[AWBOCIs]
ADD CONSTRAINT [FK_AWBOCIAWBCustomsInformation]
    FOREIGN KEY ([AWBCustomsInformationCode])
    REFERENCES [dbo].[AWBCustomsInformations]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AWBInformationCode] in table 'AWBOCIs'
ALTER TABLE [dbo].[AWBOCIs]
ADD CONSTRAINT [FK_AWBOCIAWBInformation]
    FOREIGN KEY ([AWBInformationCode])
    REFERENCES [dbo].[AWBInformations]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO