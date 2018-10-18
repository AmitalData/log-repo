
delete from ScreenFields where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='DeclarationNo' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))
update ScreenFields set Row=0 where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='StorageSiteCode' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))
update ScreenFields set Row=1 where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='CheckSiteCode' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))
update ScreenFields set Row=2 where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='CheckId' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))
update ScreenFields set Row=3 where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='OperationCode' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))
update ScreenFields set Row=4 where ScreenId=(select id from Screens where code='Customs.PhysicalCheck.GeneralTabScreen') and ObjectFieldId=(select id from objectfields where FieldName='StatusMessageCode' and ObjectTableId=(select id from objecttables where name='Customs.PhysicalCheck'))


