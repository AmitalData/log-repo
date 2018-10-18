update TextCodes set code = REPLACE (Code, 'IsCustomerProceduralFaultCountable', 'IsCustProceduralFaultCountabl') where code like '%IsCustomerProceduralFaultCountable%'

update TextCodes set code = REPLACE (Code, 'IsAgentProceduralFaultCountable', 'IsAgentProceduralFaultCountabl') where code like '%IsAgentProceduralFaultCountable%'


update TextCodes set code = REPLACE (Code, 'NonCustomsItemPriceCurrencyCode', 'NonCustomsItemPriceCurCode') where code like '%NonCustomsItemPriceCurrencyCode%'



update TextCodes set code = REPLACE (Code, 'AlternateDefinedPerUnitQuantity', 'AlternateDefinedPerUnitQuant') where code like '%AlternateDefinedPerUnitQuantity%'

update TextCodes set code = REPLACE (Code, 'ProceduralFaultInputProcessCode', 'ProceduralFaultInputProcesCode') where code like '%ProceduralFaultInputProcessCode%'

update TextCodes set code = REPLACE (Code, 'VehicleSafetyAccessoryInstallationTypeCode', 'VehicleSafAccessoryInstTypeCod') where code like '%VehicleSafetyAccessoryInstallationTypeCode%'



update TextCodes set code = REPLACE (Code, '.ProceduralFaultInputProcessType.', '.ProceduralFaultInProcessType.') where code like '%ProceduralFaultInputProcessType%'

update TextCodes set code = REPLACE (Code, '.ProceduralFaultInputSourceType.', '.ProceduralFaultInSourceType.') where code like '%ProceduralFaultInputSourceType%'

update TextCodes set code = REPLACE (Code, '.ProceduralFaultsConnectedEntity.', '.ProceduralFaultsConnEntity.') where code like '%ProceduralFaultsConnectedEntity%'


update TextCodes set code = REPLACE (Code, '.SupplierInvioceItemsCertificate.', '.SupplierInvioceItemCertificat.') where code like '%SupplierInvioceItemsCertificate%'

update TextCodes set code = REPLACE (Code, '.VehicleSafetyAccessoryInstallationType.', '.VehicleSafeAccessoryInstlType.') where code like '%VehicleSafetyAccessoryInstallationType%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsDescription.', '.SupplierInvoiceItemsDescript.') where code like '%SupplierInvoiceItemsDescription%'

update TextCodes set code = REPLACE (Code, '.DeficitConnectedFileParagraphType.', '.DeficitConnFileParagraphType.') where code like '%DeficitConnectedFileParagraphType%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsProductIdentification.', '.SupplierInvoiceItemsProdIdent.') where code like '%SupplierInvoiceItemsProductIdentification%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsSerialNumber.', '.SupplierInvoiceItemsSerialNum.') where code like '%SupplierInvoiceItemsSerialNumber%'

update TextCodes set code = REPLACE (Code, '.ClientsAddressCommunicationType.', '.ClientsAddressCommType.') where code like '%ClientsAddressCommunicationType%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsConnectedDeclaration.', '.SupplierInvoiceItemsConDeclar.') where code like '%SupplierInvoiceItemsConnectedDeclaration%'

update TextCodes set code = REPLACE (Code, '.ImporterPeriodicDeclarationStatus.', '.ImporterPeriodicDeclarStatus.') where code like '%ImporterPeriodicDeclarationStatus%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsProcessType.', '.SupplierInvoiceItemProcesType.') where code like '%SupplierInvoiceItemsProcessType%'

update TextCodes set code = REPLACE (Code, '.CollateralsRequestFileCondition.', '.CollateralsRequestFileCond.') where code like '%CollateralsRequestFileCondition%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsTaxesModification.', '.SupplierInvoiceItemsTaxesMod.') where code like '%SupplierInvoiceItemsTaxesModification%'

update TextCodes set code = REPLACE (Code, '.SupplierInvoiceItemsModification.', '.SupplierInvoiceItemsMod.') where code like '%SupplierInvoiceItemsModification%'


update ObjectFields set FieldName = 'ProceduralFaultInputProcesName' where FieldName = 'ProceduralFaultInputProcessName'
update TextCodes set code = REPLACE (Code, 'ProceduralFaultInputProcessName', 'ProceduralFaultInputProcesName') where code like '%ProceduralFaultInputProcessName%'

update ObjectFields set FieldName = 'VehicleSafAccessoryInstlTypName' where FieldName = 'VehicleSafetyAccessoryInstallationTypeName'

update TextCodes set code = REPLACE (Code, 'VehicleSafetyAccessoryInstallationTypeName', 'VehicleSafAccessoryInstlTypName') where code like '%VehicleSafetyAccessoryInstallationTypeName%'



select * from TextCodes where code like '%DeficitConnectedFileParagraphType%'

--update TextCodes set code ='Customs.VehicleSafetyAccessory.CH.VehicleSafetyAccessoryInstallationTypeNameListLable' where Code='Customs.VehicleSafetyAccessory.CH.VehicleSafeAccessoryInstlTypeNameListLable'
--update TextCodes set code ='Customs.VehicleSafetyAccessory.VehicleSafetyAccessoryInstallationTypeNameHelpText' where Code='Customs.VehicleSafetyAccessory.VehicleSafeAccessoryInstlTypeNameHelpText'
--update TextCodes set code ='Customs.VehicleSafetyAccessory.F.VehicleSafetyAccessoryInstallationTypeName' where Code='Customs.VehicleSafetyAccessory.F.VehicleSafeAccessoryInstlTypeName'