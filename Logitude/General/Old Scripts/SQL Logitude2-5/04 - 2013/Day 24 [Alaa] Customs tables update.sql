--***************************
-- DO NOT RUN ONLINE ----

update objecttables set name='Customs.Declaration'
where name='Declaration'
go
update objecttables set DBTableName='Customs.Declarations'
where Name='Customs.Declaration'
go
UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'Declaration.','Customs.Declaration.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Declaration')
go

update TextCodes set code = 'Customs.Declaration' where code ='Declaration' and ObjectTableId = ( select id from objecttables where name='Customs.Declaration')
go

update objecttables set Name='Customs.PhysicalCheck' where Name='PhysicalCheck'
go
update objecttables set DBTableName='Customs.PhysicalChecks' where Name ='Customs.PhysicalCheck'
go

UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'PhysicalCheck.','Customs.PhysicalCheck.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.PhysicalCheck')
go

update TextCodes set code = 'Customs.PhysicalCheck' where code ='PhysicalCheck' and ObjectTableId = ( select id from objecttables where name='Customs.PhysicalCheck')
go


Update ObjectTables set Name= 'Customs.CheckEntityType' where Name='CheckEntityType'
go

update ObjectTables set DBTableName='Customs.CheckEntityTypes' where Name='Customs.CheckEntityType'
go

UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'CheckEntityType.','Customs.CheckEntityType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.CheckEntityType')
go

update TextCodes set code = 'Customs.CheckEntityType' where code ='CheckEntityType' and ObjectTableId = ( select id from objecttables where name='Customs.CheckEntityType')
go


update objecttables set Name='Customs.CheckRepresentativeType' where name='CheckRepresentativeType'
go

update ObjectTables set DBTableName='Customs.CheckRepresentativeTypes' where name='Customs.CheckRepresentativeType'
go


UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'CheckRepresentativeType.','Customs.CheckRepresentativeType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.CheckRepresentativeType')
go

update TextCodes set code = 'Customs.CheckRepresentativeType' where code ='CheckRepresentativeType' and ObjectTableId = ( select id from objecttables where name='Customs.CheckRepresentativeType')
go

update ObjectTables set name='Customs.SiteLookup' where Name='SiteLookup'
go

update ObjectTables set DBTableName='Customs.SiteLookups' where Name='Customs.SiteLookup'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'SiteLookup.','Customs.SiteLookup.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.SiteLookup')
go

update TextCodes set code = 'Customs.SiteLookup' where code ='SiteLookup' and ObjectTableId = ( select id from objecttables where name='Customs.SiteLookup')
go


update ObjectTables set Name='Customs.CheckQueueType' where Name='CheckQueueType'
go

update ObjectTables set DBTableName='Customs.CheckQueueTypes' where Name='Customs.CheckQueueType'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'CheckQueueType.','Customs.CheckQueueType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.CheckQueueType')
go

update TextCodes set code = 'Customs.CheckQueueType' where code ='CheckQueueType' and ObjectTableId = ( select id from objecttables where name='Customs.CheckQueueType')
go


update ObjectTables set Name='Customs.CargoIdentifireType' where Name ='CargoIdentifireType'
go

update ObjectTables set DBTableName='Customs.CargoIdentifireTypes' where name='Customs.CargoIdentifireType'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'CargoIdentifireType.','Customs.CargoIdentifireType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.CargoIdentifireType')
go

update TextCodes set code='Customs.CargoIdentifireType' where Code='CargoIdentifireType' and ObjectTableId = ( select id from objecttables where name='Customs.CargoIdentifireType')
go


update ObjectTables set name='Customs.PhysicalCheckOperation' where Name='PhysicalCheckOperation'
go

update ObjectTables set DBTableName='Customs.PhysicalCheckOperations' where Name='Customs.PhysicalCheckOperation'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'PhysicalCheckOperation.','Customs.PhysicalCheckOperation.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.PhysicalCheckOperation')
go

update TextCodes set code= 'Customs.PhysicalCheckOperation' where code='PhysicalCheckOperation'
go



update ObjectTables set Name='Customs.PhysicalCheckStatusMessage' where Name='PhysicalCheckStatusMessage'
go

update ObjectTables set DBTableName='Customs.PhysicalCheckStatusMessages' where name='Customs.PhysicalCheckStatusMessage'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'PhysicalCheckStatusMessage.','Customs.PhysicalCheckStatusMessage.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.PhysicalCheckStatusMessage')
go

update TextCodes set Code='Customs.PhysicalCheckStatusMessage' where code='PhysicalCheckStatusMessage' and ObjectTableId = ( select id from objecttables where name='Customs.PhysicalCheckStatusMessage')
go


update ObjectTables set Name='Customs.VendorCommunication' where name='VendorCommunication'
go

update ObjectTables set DBTableName='Customs.VendorCommunications' where Name='VendorCommunication'
go


update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'VendorCommunication.','Customs.VendorCommunication.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.VendorCommunication')
go

update TextCodes set Code= 'Customs.VendorCommunication' where Code='VendorCommunication' and ObjectTableId = ( select id from objecttables where name='Customs.VendorCommunication')
go



update ObjectTables set Name='Customs.SubCountry' where name='SubCountry'
go

update ObjectTables set DBTableName='Customs.SubCountries' where name='Customs.SubCountry'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'SubCountry.','Customs.SubCountry.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.SubCountry')
go

update TextCodes set Code='Customs.SubCountry' where Code ='SubCountry' and  ObjectTableId = ( select id from objecttables where name='Customs.SubCountry')
go


update objecttables set name='Customs.CommunicationType' where name='CommunicationType'
go

update objecttables set DBTableName='Customs.CommunicationTypes' where Name='Customs.CommunicationType'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'CommunicationType.','Customs.CommunicationType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.CommunicationType')
go

update TextCodes set Code='Customs.CommunicationType' where Code='CommunicationType'
go


update ObjectTables set name='Customs.VendorType' where name='VendorType'
go

update objecttables set DBTableName='Customs.VendorTypes' where Name='Customs.VendorType'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'VendorType.','Customs.VendorType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.VendorType')
go

update TextCodes set code='Customs.VendorType' where code='VendorType'
go


update ObjectTables set Name='Customs.AutonomyType' where name='AutonomyType'
go

update ObjectTables set DBTableName='Customs.AutonomyTypes' where name='Customs.AutonomyType'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'AutonomyType.','Customs.AutonomyType.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.AutonomyType')
go

update TextCodes set Code='Customs.AutonomyType' where code='AutonomyType' 
go


update ObjectTables set name='Customs.Client' where name='Client'
go

update objecttables set DBTableName='Customs.Client' where Name='Customs.Client'
go


update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'Client.','Customs.Client.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.Client')
go

update TextCodes set code='Customs.Client' where code='Client'
go


update ObjectTables set name='Customs.Consignment' where name='Consignment'
go

update ObjectTables set DBTableName='Customs.Consignments'  where name='Customs.Consignment'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'Consignment.','Customs.Consignment.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.Consignment')
go

update TextCodes set code='Customs.Consignment' where code='Consignment'
go

update objecttables set name='Customs.ConsignmentPackage' where name='ConsignmentPackage'
go

update ObjectTables set DBTableName='Customs.ConsignmentPackages' where name='Customs.ConsignmentPackage'
go

update TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'ConsignmentPackage.','Customs.ConsignmentPackage.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.ConsignmentPackage')
go

update TextCodes set code='Customs.ConsignmentPackage' where Code='ConsignmentPackage'
go


update ObjectTables set name='Cutoms.CountryGroup' where name='CountryGroup'
go

update ObjectTables set DBTableName='Cutoms.CountryGroups' where name='Cutoms.CountryGroup'
go

update TextCodes set code='Customs.CountryGroup' where code='CountryGroup'
go


update ObjectTables set name='Customs.EntitlementType' where Name='EntitlementType'
go

update ObjectTables set DBTableName='Customs.EntitlementTypes' where name='Customs.EntitlementType'
go

update TextCodes set code='Customs.EntitlementType'  where  code='EntitlementType'
go

update objecttables set name='Customs.GovernmentProcedureType' where name='GovernmentProcedureType'
go

update objecttables set DBTableName='Customs.GovernmentProcedureTypes' where name='Customs.GovernmentProcedureType'
go

update TextCodes set code='Customs.GovernmentProcedureType' where code='GovernmentProcedureType'
go


update ObjectTables set name='Customs.LeadDocumentType' where name='LeadDocumentType'
go

update ObjectTables set DBTableName= 'Customs.LeadDocumentTypes' where name='Customs.LeadDocumentType'
go

update TextCodes set code='Customs.LeadDocumentType' where code='LeadDocumentType'
go

update objecttables set name='Customs.PackageMeasureQualifier' where name='PackageMeasureQualifier'
go

update ObjectTables set DBTableName='Customs.PackageMeasureQualifiers' where name='Customs.PackageMeasureQualifier' 
go

update TextCodes set code='Customs.PackageMeasureQualifier' where code='PackageMeasureQualifier'
go


update ObjectTables set name='Customs.PackingType' where name='PackingType'
go

update objecttables set DBTableName='Customs.PackingType' where name='Customs.PackingType'
go

update TextCodes set code='Customs.PackingType' where code='PackingType'
go


update ObjectTables set name='Customs.SiteType' where  name='SiteType'
go

update ObjectTables set DBTableName='Customs.SiteType' where name='Customs.SiteType'
go

update TextCodes set Code='Customs.SiteType' where code='SiteType'
go

update screens set code='Customs.PhysicalCheck.HeaderScreen' where code='PhysicalCheck.HeaderScreen'
update screens set code='Customs.PhysicalCheck.GeneralTabScreen' where code='PhysicalCheck.GeneralTabScreen'


update screens set code='Customs.Declaration.HeaderScreen' where code='Declaration.HeaderScreen'
update screens set code='Customs.Declaration.GeneralTabScreen' where code='Declaration.GeneralTabScreen'


update screens set code='Customs.Vendor.HeaderScreen' where code='Vendor.HeaderScreen'
update screens set code='Customs.Vendor.GeneralTabScreen' where code='Vendor.GeneralTabScreen'

delete from ObjectTableTabs where code='DETS' and objecttableid =(select id from objecttables where name='Customs.Declaration')
delete from textcodes where code ='Customs.Declaration.TH.Test'

