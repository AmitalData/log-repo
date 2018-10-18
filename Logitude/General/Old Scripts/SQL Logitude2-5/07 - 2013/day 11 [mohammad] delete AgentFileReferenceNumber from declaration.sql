alter table customs.declarations drop column AgentFileReferenceNumber

delete from ObjectFields where FieldName like '%AgentFileReferenceNumber%' and ObjectTableId =(select id from ObjectTables where Name='Customs.declaration')

delete from TextCodes where Code like '%AgentFileReferenceNumber%' and ObjectTableId =(select id from ObjectTables where Name='Customs.declaration')
