-- errors 

alter table Customs.ConsignmentPackages drop ConsignmentPackage_PackingType
go

alter table Customs.PackingTypes drop PK__PackingT__A25C5AA64CE05A84
go

alter table Customs.PackingTypes alter column Code varchar(2) NOT NULL
go

alter table Customs.ConsignmentPackages alter column PackageTypeCode varchar(2) null
go

-- Creating primary key on [Code] in table 'PackingTypes'
ALTER TABLE [Customs].[PackingTypes]
ADD CONSTRAINT [PK_PackingTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [PackageTypeCode] in table 'Customs.ConsignmentPackages'
ALTER TABLE [Customs].[ConsignmentPackages]
ADD CONSTRAINT [FK_ConsignmentPackagePackingType]
    FOREIGN KEY ([PackageTypeCode])
    REFERENCES [Customs].[PackingTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
go