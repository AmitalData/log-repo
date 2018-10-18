
DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Customs.OrganizationUnitTypes DROP CONSTRAINT |ConstraintName| '

SET @SQL = REPLACE(@SQL, '|ConstraintName|', ( SELECT   name
                                               FROM     sysobjects
                                               WHERE    xtype = 'PK'
                                                        AND parent_obj = OBJECT_ID('Customs.OrganizationUnitTypes')
                                             ))

EXEC (@SQL)


alter table Customs.OrganizationUnitTypes alter column code varchar(3) not null
go


ALTER TABLE customs.organizationunittypes
ADD PRIMARY KEY (Code)
go







