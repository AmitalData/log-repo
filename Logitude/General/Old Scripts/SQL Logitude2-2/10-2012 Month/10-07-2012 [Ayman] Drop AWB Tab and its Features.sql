
-- Delete AWB Tab From Shipment + Master
select * from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
select * from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Master')
select * from RoleFeatures where FeatureId = (select Id from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment'))
select * from RoleFeatures where FeatureId = (select Id from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Master'))
select * from ObjectTableTabs where Code = 'SHAB'
select * from ObjectTableTabs where Code = 'JHAB'
select * from TextCodes where Code = 'Shipment.TH.AirwayBill'
select * from TextCodes where Code = 'Master.TH.AirwayBill'
select * from TextCodes where Code = 'Shipment.Features.Airwaybill'
select * from TextCodes where Code = 'Master.Features.Airwaybill'



delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Master'))
delete from ObjectTableTabs where Code = 'SHAB'
delete from ObjectTableTabs where Code = 'JHAB'
delete from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
delete from Features where Code = 'AIRWAYBILL' and ObjectTableId = (Select Id from ObjectTables where Name = 'Master')
delete from TextCodes where Code = 'Shipment.TH.AirwayBill'
delete from TextCodes where Code = 'Master.TH.AirwayBill'
delete from TextCodes where Code = 'Shipment.Features.Airwaybill'
delete from TextCodes where Code = 'Master.Features.Airwaybill'