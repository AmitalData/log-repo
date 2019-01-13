    
delete from querycolumns where queryid in (select id from queries where code = 'customers' )
 and objectfieldid in (select id from objectfields where fieldname = 'billtoname' and objecttableid in (select id from objecttables where name = 'customer')) and tenant = 0
 
 
delete from querycolumns where queryid in (select id from queries where code = 'Participants' )
 and objectfieldid in (select id from objectfields where fieldname = 'AirlineTenant' and objecttableid in (select id from objecttables where name = 'Participant')) and tenant = 0

 update TextCodes set Code = 'ApiCredintials.F.maskedSeconderyAccessKey' where Code = 'ApiCredintials.F.masked Secondery Access Key' and ObjectTableId in (select Id from ObjectTables where Name = 'ApiCredintials')
 

delete from ObjectFields where FieldName = 'ComputingPartnerId' and ObjectTableId in (select Id from ObjectTables where Name = 'ApiCredintials')
delete from TextCodes where Code like '%.Computing%' and ObjectTableId in (select Id from ObjectTables where Name = 'ApiCredintials')