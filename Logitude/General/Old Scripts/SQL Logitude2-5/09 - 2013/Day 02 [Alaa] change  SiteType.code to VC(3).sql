alter table [Customs].[SiteTypes] alter column code varchar(3) not null

alter table [Customs].[SiteLookups] drop [SiteLookup_SiteType]

DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Customs.SiteTypes DROP CONSTRAINT |ConstraintName| '

SET @SQL = REPLACE(@SQL, '|ConstraintName|', ( SELECT   name
                                               FROM     sysobjects
                                               WHERE    xtype = 'PK'
                                                        AND parent_obj = OBJECT_ID('Customs.SiteTypes')
                                             ))

EXEC (@SQL)

ALTER TABLE  [Customs].[SiteTypes] ADD CONSTRAINT PK_SiteType_Code PRIMARY KEY (Code)

alter table [Customs].[SiteLookups] alter column siteTypeCode varchar(3)

alter table [Customs].[SiteLookups] add constraint [SiteLookup_SiteType] foreign key ([SiteTypeCode]) references [Customs].[SiteTypes]([Code]);
