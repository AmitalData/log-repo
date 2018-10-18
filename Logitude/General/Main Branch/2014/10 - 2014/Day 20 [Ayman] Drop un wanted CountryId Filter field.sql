

delete from AdvancedQueryFilters
where ObjectFieldId in
(
select Id from ObjectFields
where
 FieldName = 'CountryId'
 and CanFilter = 1
 and ObjectTableId in (select Id from ObjectTables where Name = 'Agent' Or Name = 'Airline' Or Name = 'CustomAgent' Or Name = 'ShippingAgent' Or Name = 'ShippingLine' Or Name = 'Trucker' Or Name = 'Vendor' Or Name = 'Warehouse') 
)
GO

delete from QueryColumns
where ObjectFieldId in
(
select Id from ObjectFields
where
 FieldName = 'CountryId'
 and CanFilter = 1
 and ObjectTableId in (select Id from ObjectTables where Name = 'Agent' Or Name = 'Airline' Or Name = 'CustomAgent' Or Name = 'ShippingAgent' Or Name = 'ShippingLine' Or Name = 'Trucker' Or Name = 'Vendor' Or Name = 'Warehouse') 
)
GO

delete from ObjectFields
where
 FieldName = 'CountryId'
 and CanFilter = 1
 and ObjectTableId in (select Id from ObjectTables where Name = 'Agent' Or Name = 'Airline' Or Name = 'CustomAgent' Or Name = 'ShippingAgent' Or Name = 'ShippingLine' Or Name = 'Trucker' Or Name = 'Vendor' Or Name = 'Warehouse')
 GO

delete from TextCodes where Code like 'Agent%CountryId%'
delete from TextCodes where Code like 'Airline%CountryId%'
delete from TextCodes where Code like 'CustomAgent%CountryId%'
delete from TextCodes where Code like 'ShippingAgent%CountryId%'
delete from TextCodes where Code like 'ShippingLine%CountryId%'
delete from TextCodes where Code like 'Trucker%CountryId%'
delete from TextCodes where Code like 'Vendor%CountryId%'
delete from TextCodes where Code like 'Warehouse%CountryId%'