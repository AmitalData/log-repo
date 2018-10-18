
 delete  from ScreenFields where
 objectfieldid=(select id from objectfields where fieldname='LogDateTime' and 
 objecttableid=(select id from objecttables where name='ErrorLog'))

 GO

delete from QueryColumns where
 ObjectFieldId in (select id from ObjectFields where fieldname='LogDateTime') and
 QueryId = (select id from Queries where objecttableid=( select id from objecttables where name='errorlog'))
 
 GO

  delete from AdvancedQueryFilters  where 
 ObjectFieldId in (select id from ObjectFields where fieldname='LogDateTime')

 GO

delete from ObjectFields where FieldName='LogDateTime' and
 ObjectTableId in ( select id from objecttables where name='ErrorLog')

GO


 delete  from ScreenFields where
 objectfieldid=(select id from objectfields where fieldname='UserId' and 
 objecttableid=(select id from objecttables where name='ErrorLog'))

 GO

delete from QueryColumns where
 ObjectFieldId in (select id from ObjectFields where fieldname='UserId') and
 QueryId in (select id from Queries where objecttableid=( select id from objecttables where name='errorlog'))

 GO

 delete from AdvancedQueryFilters  where 
 ObjectFieldId in (select id from ObjectFields where fieldname='UserId')

 GO

 delete from ObjectFields where FieldName='UserId' and
 ObjectTableId in ( select id from objecttables where name='ErrorLog')

 GO