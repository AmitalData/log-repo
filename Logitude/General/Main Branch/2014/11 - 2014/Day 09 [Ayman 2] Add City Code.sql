

IF COLUMNPROPERTY( OBJECT_ID(N'CountryCities'), 'Code', 'ColumnId') IS NULL
begin

	ALTER TABLE CountryCities ADD Code nvarchar(15) not null CONSTRAINT DF_CountryCities_Fixed_Code default (0);

	ALTER TABLE CountryCities DROP DF_CountryCities_Fixed_Code	
end
GO

IF COLUMNPROPERTY( OBJECT_ID(N'CountryCities'), 'Code', 'ColumnId') IS NOT NULL
begin

	update CountryCities set Code = Id

	ALTER TABLE CountryCities ADD CONSTRAINT uc_CountryCityCode UNIQUE (CountryId,Code,Tenant)
end
	
select * from CountryCities
