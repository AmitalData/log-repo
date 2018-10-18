
--
-- Delete Object Field
--
delete from ObjectFields 
where FieldName = 'CurrencyId' 
and ObjectTableId = (select Id from ObjectTables where Name = 'ReconcileExternalPageLine')
 

 --
 -- Delete related TextCodes
 --
delete from TextCodes 
where  ObjectTableId = (select Id from ObjectTables where Name = 'ReconcileExternalPageLine')
and Code like '%currency%'




---------------------------------


--
-- Delete Object Field
--
delete from ObjectFields 
where FieldName = 'IsApproved' 
and ObjectTableId = (select Id from ObjectTables where Name = 'ReconcileExternalPage')
 

 --
 -- Delete related TextCodes
 --
delete from TextCodes 
where  ObjectTableId = (select Id from ObjectTables where Name = 'ReconcileExternalPage')
and Code like '%IsApproved%'