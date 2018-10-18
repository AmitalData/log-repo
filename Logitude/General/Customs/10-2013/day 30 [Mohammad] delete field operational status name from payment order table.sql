
delete from ObjectFields where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and FieldName='OperationalStatusCode'
delete from TextCodes where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and code like '%.OperationalStatusCode%'
select * from ObjectFields where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and FieldName='OperationalStatusCode'
select * from TextCodes where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and code like '%.OperationalStatusCode%'


delete from QueryColumns where ObjectFieldId=(select id from ObjectFields where FieldName='OperationalStatusName' and ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') )
delete from ScreenFields where ObjectFieldId=(select id from ObjectFields where FieldName='OperationalStatusName' and ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') )

delete from ObjectFields where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and FieldName='OperationalStatusName'
delete from TextCodes where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and code like '%.OperationalStatusName%'
select * from ObjectFields where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and FieldName='OperationalStatusName'
select * from TextCodes where ObjectTableId=(select id from ObjectTables where name='customs.paymentorder') and code like '%.OperationalStatusName%'