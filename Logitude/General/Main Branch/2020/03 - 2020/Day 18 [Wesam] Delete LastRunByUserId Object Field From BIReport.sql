

delete from objectfields where objecttableid = (select id from objecttables where name='bireport') and FieldName='LastRunByUserId'