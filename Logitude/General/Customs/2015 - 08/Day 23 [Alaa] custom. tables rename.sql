
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsDescription')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsDescription')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsDescription')
delete from objecttables where name ='customs.SupplierInvoiceItemsDescription'


delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.DeficitConnectedFileParagraphType')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.DeficitConnectedFileParagraphType')
delete from objecttables where name ='customs.DeficitConnectedFileParagraphType'


delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProductIdentification')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProductIdentification')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProductIdentification')
delete from objecttables where name ='customs.SupplierInvoiceItemsProductIdentification'


delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsSerialNumber')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsSerialNumber')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsSerialNumber')
delete from objecttables where name ='customs.SupplierInvoiceItemsSerialNumber'

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.ClientsAddressCommunicationType')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.ClientsAddressCommunicationType')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.ClientsAddressCommunicationType')
delete from objecttables where name ='customs.ClientsAddressCommunicationType'

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsConnectedDeclaration')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsConnectedDeclaration')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsConnectedDeclaration')
delete from objecttables where name ='customs.SupplierInvoiceItemsConnectedDeclaration'

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from QueryColumns where QueryId = (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus'))

delete from Queries  where ObjectTableId =  (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from Features where ObjectTableId = (select id from ObjectTables where  name ='customs.ImporterPeriodicDeclarationStatus')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from ObjectFields where LookUpTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from ObjectFields where LookUpTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.ImporterPeriodicDeclarationStatus')

delete from objecttables where name ='customs.ImporterPeriodicDeclarationStatus'


delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProcessType')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProcessType')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsProcessType')
delete from objecttables where name ='customs.SupplierInvoiceItemsProcessType'

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.CollateralsRequestFileCondition')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.CollateralsRequestFileCondition')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.CollateralsRequestFileCondition')
delete from objecttables where name ='customs.CollateralsRequestFileCondition'

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsTaxesModification')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsTaxesModification')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsTaxesModification')
delete from objecttables where name ='customs.SupplierInvoiceItemsTaxesModification'


delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsModification')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsModification')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvoiceItemsModification')
delete from objecttables where name ='customs.SupplierInvoiceItemsModification'


delete from QueryColumns where QueryId = (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInProcessType'))
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')

delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')
delete from textcodes where objecttableid =(select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInProcessType'))
delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInProcessType'))
delete from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInProcessType')
delete from ObjectTables where name ='customs.ProceduralFaultInProcessType'


delete from QueryColumns where QueryId in (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInSourceType'))
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')

delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInSourceType'))

delete from textcodes where objecttableid =(select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')
delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInSourceType'))
delete from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultInSourceType')
delete from ObjectTables where name ='customs.ProceduralFaultInSourceType'


delete from QueryColumns where QueryId in (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity'))
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')

delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity'))

delete from textcodes where objecttableid =(select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity'))
delete from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.ProceduralFaultsConnEntity')
delete from ObjectTables where name ='customs.ProceduralFaultsConnEntity'


delete from QueryColumns where QueryId in (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat'))
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')

delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat'))

delete from textcodes where objecttableid =(select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat'))
delete from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.SupplierInvioceItemCertificat')
delete from ObjectTables where name ='customs.SupplierInvioceItemCertificat'


delete from QueryColumns where QueryId in (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType'))
delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')

delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType'))

delete from textcodes where objecttableid =(select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType'))
delete from ObjectFields where LookUpTableId =(select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from customs.CustomsClosedTables where ObjectTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from ObjectFields where MultiTableId = (select id from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType')
delete from ObjectTables where name ='customs.VehicleSafeAccessoryInstlType'






--update ObjectFields set FullNameTextCodeId = NULL where FieldName = 'ProceduralFaultInputProcessCode'
--update ObjectFields set FullNameTextCodeId = NULL where FieldName = 'IsCustomerProceduralFaultCountable'
--update ObjectFields set FullNameTextCodeId = NULL where FieldName = 'ProceduralFaultsConnectedEntities'
--update ObjectFields set FullNameTextCodeId = NULL where FieldName = 'IsAgentProceduralFaultCountable'

--update ObjectFields set ListTextCodeId = NULL where FieldName = 'ProceduralFaultsConnectedEntities'
--update ObjectFields set ListTextCodeId = NULL where FieldName = 'IsCustomerProceduralFaultCountable'
--update ObjectFields set ListTextCodeId = NULL where FieldName = 'IsAgentProceduralFaultCountable'


--update ObjectFields set HelpTextCodeId = NULL where FieldName = 'IsCustomerProceduralFaultCountable'
--update ObjectFields set HelpTextCodeId = NULL where FieldName = 'ProceduralFaultsConnectedEntities'
--update ObjectFields set HelpTextCodeId = NULL where FieldName = 'IsAgentProceduralFaultCountable'



--delete from TextCodes where code like '%IsAgentProceduralFaultCountable%'

--delete from TextCodes where code like '%IsCustomerProceduralFaultCountable%'

--delete from TextCodes where code like '%ProceduralFaultsConnectedEntities%'
