 
insert into FeaturePackageTypes(Code, Name, SearchFields) values('BS', 'Base', 'BS,Base');
insert into FeaturePackageTypes(Code, Name, SearchFields) values('AD', 'Add-on', 'AD,Add-on')
insert into FeaturePackageTypes(Code, Name, SearchFields) values('PK', 'Package', 'PK,Package')

select * from packages

update Packages set FeaturePackageTypeCode = 'BS'