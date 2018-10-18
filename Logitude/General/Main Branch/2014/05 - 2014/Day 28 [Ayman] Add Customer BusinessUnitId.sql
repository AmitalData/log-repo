

IF COLUMNPROPERTY( OBJECT_ID(N'Customers'), 'BusinessUnitId', 'ColumnId') IS NULL
begin

	ALTER TABLE Customers ADD BusinessUnitId varchar(50) not null CONSTRAINT DF_Customers_Fixed_Name default '0';

	ALTER TABLE Customers ADD CONSTRAINT FK_CustomerBusinessUnit
		FOREIGN KEY (BusinessUnitId)
		REFERENCES BusinessUnits(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_CustomerBusinessUnit] ON Customers (BusinessUnitId);

	ALTER TABLE Customers DROP DF_Customers_Fixed_Name
end
GO

update Customers set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Customers set BusinessUnitId = (select BusinessUnitId from Users where Id = Customers.SalesmanUserId and Tenant = Customers.Tenant)
where SalesmanUserId is not null
go

