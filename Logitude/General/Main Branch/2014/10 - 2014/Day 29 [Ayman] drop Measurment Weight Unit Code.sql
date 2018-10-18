

if exists (select * from sys.objects o where o.object_id = object_id(N'FK_MeasurementWeightUnit') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
alter table Measurements drop FK_MeasurementWeightUnit
go

if exists (select * from sys.indexes where name = 'IX_FK_MeasurementWeightUnit')
drop index IX_FK_MeasurementWeightUnit on Measurements
go

if COLUMNPROPERTY(OBJECT_ID(N'Measurements'), 'WeightUnitCode', 'ColumnId') is not null
alter table Measurements drop column WeightUnitCode
go


delete from AdvancedQueryFilters
where
ObjectFieldId = (select Id from ObjectFields where FieldName = 'WeightUnitCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Measurement'))
and QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Measurement'))
go

delete from QueryColumns
where
ObjectFieldId = (select Id from ObjectFields where FieldName = 'WeightUnitCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Measurement'))
and QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Measurement'))
go

delete from ObjectFields where FieldName = 'WeightUnitCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Measurement')
go

delete from TextCodes where code like '%Measurement%WeightUnitCode%'
go