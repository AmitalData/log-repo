


IF COLUMNPROPERTY( OBJECT_ID(N'Users'), 'BusinessUnitId', 'ColumnId') IS NULL
begin

	ALTER TABLE Users ADD BusinessUnitId varchar(50) not null CONSTRAINT DF_Users_Fixed_Name default '0';

	ALTER TABLE Users ADD CONSTRAINT FK_UserBusinessUnit
		FOREIGN KEY (BusinessUnitId)
		REFERENCES BusinessUnits(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_UserBusinessUnit] ON Users (BusinessUnitId);

	ALTER TABLE Users DROP DF_Users_Fixed_Name
end
GO

IF COLUMNPROPERTY( OBJECT_ID(N'Quotes'), 'BusinessUnitId', 'ColumnId') IS NULL
begin

	ALTER TABLE Quotes ADD BusinessUnitId varchar(50) not null CONSTRAINT DF_Quotes_Fixed_Name default '0';

	ALTER TABLE Quotes ADD CONSTRAINT FK_QuoteBusinessUnit
		FOREIGN KEY (BusinessUnitId)
		REFERENCES BusinessUnits(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_QuoteBusinessUnit] ON Quotes (BusinessUnitId);

	ALTER TABLE Quotes DROP DF_Quotes_Fixed_Name
end
GO

IF COLUMNPROPERTY( OBJECT_ID(N'Activities'), 'BusinessUnitId', 'ColumnId') IS NULL
begin

	ALTER TABLE Activities ADD BusinessUnitId varchar(50) not null CONSTRAINT DF_Activities_Fixed_Name default '0';

	ALTER TABLE Activities ADD CONSTRAINT FK_ActivityBusinessUnit
		FOREIGN KEY (BusinessUnitId)
		REFERENCES BusinessUnits(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_ActivityBusinessUnit] ON Activities (BusinessUnitId);

	ALTER TABLE Activities DROP DF_Activities_Fixed_Name
end
GO

IF COLUMNPROPERTY( OBJECT_ID(N'Opportunities'), 'BusinessUnitId', 'ColumnId') IS NULL
begin

	ALTER TABLE Opportunities ADD BusinessUnitId varchar(50) not null CONSTRAINT DF_Opportunities_Fixed_Name default '0';

	ALTER TABLE Opportunities ADD CONSTRAINT FK_OpportunityBusinessUnit
		FOREIGN KEY (BusinessUnitId)
		REFERENCES BusinessUnits(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_OpportunityBusinessUnit] ON Opportunities (BusinessUnitId);

	ALTER TABLE Opportunities DROP DF_Opportunities_Fixed_Name
end
GO

update Users set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Quotes set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Activities set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Opportunities set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go


