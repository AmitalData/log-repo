update ObjectFields 
set IsCustomFilter = '0'
where ObjectTableId = (select id from objecttables where name = 'ratestable') and FieldName = 'ForeignCurrencyId'

select * from ObjectFields  where ObjectTableId = (select id from objecttables where name = 'ratestable') and FieldName = 'ForeignCurrencyId'