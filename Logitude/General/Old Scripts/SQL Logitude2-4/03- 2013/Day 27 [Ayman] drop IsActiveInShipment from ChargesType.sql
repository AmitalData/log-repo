

alter table ChargesTypes drop column IsActiveInShipment
go

alter table ChargesTypes drop column IsActiveInConsolidation
go

delete from ObjectFields where FieldName like 'IsActiveIn%'
go

delete from TextCodes where Code like '%IsActiveIn%'
go