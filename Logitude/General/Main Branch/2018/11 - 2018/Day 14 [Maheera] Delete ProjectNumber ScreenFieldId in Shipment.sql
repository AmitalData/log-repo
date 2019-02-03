select *  from ScreenFields where ScreenId in (select id from Screens where (code = 'Shipment.GeneralTabScreen' or code ='Master.GeneralTabScreen') 
and ObjectTableId = (select id from objecttables where name = 'shipment')
and ObjectFieldId = (select id from ObjectFields where FieldName = 'projectnumber' and ObjectTableId = (select id from objecttables where name = 'shipment') )
)