
update ObjectFields set IsRequiered='false' 
where FieldName = 'AirlineTenant'
and ObjectTableId = (select Id from ObjectTables where Name ='Participant')




delete from QueryColumns where ObjectFieldId in (select id  from ObjectFields  where FieldName = 'AirlineTenant'
and ObjectTableId = (select Id from ObjectTables where Name ='Participant'))



delete from QueryColumns
where
ObjectFieldId in (select Id from ObjectFields where FieldName = 'Percentage' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))
and
QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))


delete from ObjectFields where FieldName = 'Percentage' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType')
delete from TextCodes where Code in ('VatType.F.Percentage', 'VatType.CH.PercentageListLable', 'VatType.PercentageHelpText')


update textcodes set code = 'CountryCity.F.StateId' where code = 'CountryCity.F.State' and objecttableid in (select id from objecttables where name = 'CountryCity')
update textcodes set code = 'Participant.F.EnglishName' where code = 'Participant.F.English Name' and objecttableid in (select id from objecttables where name = 'Participant')
update textcodes set code = 'VatType.F.EnglishName' where code = 'VatType.F.Name' and objecttableid in (select id from objecttables where name = 'VatType')