
alter table Customs.ConsignmentPackages drop FK_ConsignmentPackagePackingType
go

DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Customs.PackingTypes DROP CONSTRAINT |ConstraintName| '

SET @SQL = REPLACE(@SQL, '|ConstraintName|', ( SELECT   name
                                               FROM     sysobjects
                                               WHERE    xtype = 'PK'
                                                        AND parent_obj = OBJECT_ID('Customs.PackingTypes')
                                             ))

EXEC (@SQL)




alter table Customs.PackingTypes alter column code varchar(4) not null
go

alter table Customs.ConsignmentPackages alter column PackageTypeCode varchar(4) null
go


ALTER TABLE Customs.PackingTypes
ADD PRIMARY KEY (Code)
go

alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_PackingType] foreign key ([PackageTypeCode]) references [Customs].[PackingTypes]([Code]);
go