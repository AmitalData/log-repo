delete from QueryColumns where QueryId in (select id from Queries where Code ='All Inbound Emails' or Code = 'InboundEmails')
delete from Queries where  ObjectTableId = (select Id from ObjectTables where Name ='inboundemail') 
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name ='inboundemail') and FieldName = 'Id'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name ='inboundemail') and FieldName = 'Tenant'
delete from  TextCodes where ObjectTableId = (select Id from ObjectTables where Name ='inboundemail') and TextCodeTypeCode = 'F' and (DefaultText = 'Tenant' or DefaultText = 'ID' )
delete from Textcodes where code ='InboundEmail.TenantHelpText'  or code like '%InboundEmail.CH.TenantLabel%'
