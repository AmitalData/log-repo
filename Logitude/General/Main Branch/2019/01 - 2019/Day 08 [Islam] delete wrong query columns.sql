    
delete from querycolumns where queryid in (select id from queries where code = 'customers' )
 and objectfieldid in (select id from objectfields where fieldname = 'billtoname' and objecttableid in (select id from objecttables where name = 'customer')) and tenant = 0
 
 
delete from querycolumns where queryid in (select id from queries where code = 'Participants' )
 and objectfieldid in (select id from objectfields where fieldname = 'AirlineTenant' and objecttableid in (select id from objecttables where name = 'Participant')) and tenant = 0
 