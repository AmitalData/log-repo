delete from objectfields where ObjectTableId=(select id from objecttables where name='Customs.DeclarationOffice')
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId=(select id from objecttables where name='Customs.DeclarationOffice'))
delete from Features where ObjectTableId=(select id from objecttables where name='Customs.DeclarationOffice')
delete from TextCodes where ObjectTableId=(select id from objecttables where name='Customs.DeclarationOffice')
delete from MenusTables where ObjectTableId=(select id from objecttables where name='Customs.DeclarationOffice')
delete from ObjectTables where name='Customs.DeclarationOffice'


delete from objectfields where ObjectTableId=(select id from objecttables where name='Customs.PreferenceDocumentType')
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId=(select id from objecttables where name='Customs.PreferenceDocumentType'))
delete from Features where ObjectTableId=(select id from objecttables where name='Customs.PreferenceDocumentType')
delete from TextCodes where ObjectTableId=(select id from objecttables where name='Customs.PreferenceDocumentType')
delete from MenusTables where ObjectTableId=(select id from objecttables where name='Customs.PreferenceDocumentType')
delete from ObjectTables where name='Customs.PreferenceDocumentType'


--file from amital