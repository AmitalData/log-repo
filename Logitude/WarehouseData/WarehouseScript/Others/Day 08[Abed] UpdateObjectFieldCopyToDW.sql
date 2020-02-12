





update  ObjectFields set CopyToDW = 0 
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'DWHSetting') and (FieldName = 'Tenant' or FieldName = 'ParentTenant')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Direction') and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'TransportMode')and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentLevel')and (FieldName = 'Code' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentType')and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'Branch') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Code' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Card') and (FieldName = 'EnglishName' or FieldName = 'Code' or FieldName = 'LocalName'  or FieldName = 'VatNumber' or FieldName = 'CityName' or FieldName = 'ZipCode'  or FieldName='SalesmanUserId' or FieldName='PrimaryContactId' or FieldName='PartnerTypeId'  or  FieldName='CountryName' or FieldName = 'CountryId' or FieldName = 'ReceivablesAccountingCard'  or FieldName = 'Address1' or  FieldName = 'Address2'  or  FieldName = 'Phone')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'User') and (FieldName = 'BranchId' or FieldName = 'DepartmentId'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Contact') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Email' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Currency') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Code' or FieldName = 'Sign')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Department') and (FieldName = 'EnglishName' or FieldName = 'LocalName' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Port') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName='Code'  or FieldName='CombinedCode' or  FieldName='CountryId' or FieldName='StateId')


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Address') and ( FieldName = 'StateId' or FieldName = 'CountryId'  or FieldName = 'AddressTypeId' or FieldName='CardId')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Country') and (FieldName = 'EnglishName' or FieldName = 'Code')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'State') and (FieldName = 'EnglishName' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'PartnerType') and (FieldName = 'Name' )





update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Customer') and (FieldName = 'CreditLimitAmount' or FieldName = 'CreditLimitOpenBalance' or FieldName = 'AccountManagerUserId' or FieldName = 'RankId'  or FieldName = 'RegionId' or FieldName = 'CustomerSizeId'  or FieldName = 'IndustryId' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Tenant') and (FieldName = 'AddressId' or FieldName = 'CountryId' or FieldName = 'Company' or  FieldName = 'CurrencyId' )


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Incoterm') and (FieldName = 'Name' or FieldName = 'LocalName' or FieldName='Code' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'EntityStatus') and (FieldName = 'Name' or FieldName = 'Code' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Rank') and (FieldName = 'Name'  )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Region') and (FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'CustomerSize') and (FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Industry') and (FieldName = 'Name'  )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'CustomPickList') and (FieldName = 'Value' or FieldName = 'Code' or FieldName = 'IsMultipleChoice' )


 


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Shipment') and (FieldName = 'DirectionId' or FieldName = 'TransportModeId' or FieldName = 'ShipmentLevelCode' or FieldName = 'ShipmentTypeId' or FieldName = 'DepartmentId' or FieldName = 'BranchId'or FieldName = 'MasterShipmentDataId' or FieldName = 'ShipperId'or FieldName = 'ConsigneeId' or FieldName = 'AgentId'or FieldName = 'CustomerId' or FieldName = 'IncotermId'or FieldName = 'SalesmanUserId' or FieldName = 'AccountManagerUserId'or FieldName = 'ProfitCurrencyId' or FieldName = 'StatusId'or FieldName = 'FromPortId' or FieldName = 'ToPortId'  
or FieldName = 'ShipmentNumber' or FieldName = 'House' or FieldName = 'GrossWeightInKG' or FieldName = 'ChargeableWeightInKG' or FieldName = 'VolumeInCBM' or FieldName = 'NumberOfPackages'or FieldName = 'NumberOfContainers'  or FieldName = 'ProfitInLocalCurrency'or   FieldName = 'ProfitInProfitCurrency' or FieldName = 'IsOperationalClosed'or FieldName = 'IsAccountingClosed' or FieldName = 'StatusLocation'or FieldName = 'FinalArrivalDate' or FieldName = 'CustomsClearanceDate' or FieldName = 'CreateDateTime' or  FieldName = 'OperationalDate' 
or  FieldName = 'OperationalCloseDate'or  FieldName = 'AccountingCloseDate'or  FieldName = 'LastUpdateDate' or  FieldName = 'OpenReceivablesInLocalCurrency'or  FieldName = 'OpenReceivablesInProfitCurrency'or  FieldName = 'AccountedReceivablesInLocalCurrency'
or  FieldName = 'AccountedReceivablesInProfitCurrency'or  FieldName = 'OpenPayablesInLocalCurrency'or  FieldName = 'OpenPayablesInProfitCurrency' or  FieldName = 'AccountedPayablesInLocalCurrency'or  FieldName = 'AccountedPayablesInProfitCurrency' or    FieldName='TEU'or FieldName='ValueOfGoods'or FieldName='ValueOfGoodsCurrencyId' or FieldName='FirstPickupETA'or FieldName='FirstPickupETD'  
or FieldName = 'ProjectNumber' or FieldName = 'CustomerReference1' or FieldName = 'CustomerReference2'  or FieldName = 'ShipperReference1' or FieldName = 'ShipperReference2'  or FieldName = 'ConsigneeReference1' or FieldName = 'ConsigneeReference2'  or FieldName = 'AgentReference1' or FieldName = 'AgentReference2'   or FieldName = 'AMSBL' or FieldName = 'FreightPrepaidCollectId'  or FieldName = 'OtherPrepaidCollectId'   or FieldName = 'MainHarmonize'
or FieldName = 'ForwarderPartnerId' or FieldName = 'CreatedByUserId' or FieldName = 'CustomAgentExportId'  or FieldName = 'CustomAgentImportId'  or FieldName = 'CustomAgentExportId' or FieldName = 'WarehouseLegWarehouseId' or FieldName = 'IsCancelled' 
or FieldName = 'StatusDate' or FieldName = 'CustomsDeclarationNumber' or  FieldName ='FirstOperationalCloseDate'   or FieldName ='EstimatedFinalArrivalDate' or FieldName = 'ActualFinalArrivalDate'  or FieldName = 'Routing' or FieldName = 'DescriptionOfGoods' or FieldName= 'PreCarriageETD' or FieldName='MoveTypeId' or FieldName='SpecialServicesTypeId'  or FieldName= 'AgentComputed' or FieldName= 'ARInvoices' or    FieldName= 'ConsolidatorId' or FieldName = 'ConsolidatorReference' or FieldName = 'Notes'or FieldName = 'Notify1Id' or FieldName = 'Notify1Reference'or FieldName = 'Notify2Id' or FieldName = 'Notify2Reference'
or FieldName = 'ColoaderId' or FieldName = 'ColoaderReference1'or FieldName = 'ShipperNotExporterId' or FieldName = 'ShipperNotExporterReference' or FieldName = 'ReleasingAgentId' or FieldName = 'ReleasingAgentReference1' or FieldName ='Ratio' or FieldName ='VolumetricWeight' or FieldName ='WarehouseLegActualEntryDate'  or FieldName ='WarehouseLegExpectedEntryDate'  or FieldName ='WarehouseLegActualReleaseDate'  or FieldName ='WarehouseLegExpectedReleaseDate' or FieldName ='ChargeableWeightUnitCode'
or FieldName = 'IncludesCustoms' or FieldName = 'DeclarationNumber' or FieldName = 'DeclarationDate' or FieldName = 'TerminalAvailable' or FieldName = 'WarehouseLegLastFreeDate'
or FieldName = 'OrderGrossWeight' or FieldName = 'OrderChargeableWeight' or FieldName = 'BookingVolume' or FieldName = 'BookingNumberOfPackages' or FieldName = 'EstimateProfitInProfitCurrency'or FieldName = 'EstimateProfitInLocalCurrency'
or FieldName = 'GrossWeightUnitCode'or FieldName = 'VolumeUnitCode' or FieldName = 'ConsigneeNotImporterId'or FieldName = 'IssuingCarrierAgentId' or FieldName = 'OnCarriageTransportModeId'or FieldName = 'FirstARInvoiceApprovalDate' or FieldName = 'FreightForwarderId' or FieldName = 'FreightRelease'

)



update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Master') and (FieldName = 'MainCarriageATD' or FieldName = 'Master' or FieldName = 'MainCarriageToPortId' or FieldName = 'Transshipment1ToPortId' or FieldName = 'Transshipment2ToPortId' or FieldName = 'Transshipment3ToPortId' 
or FieldName = 'MainCarriageETD' or FieldName = 'MainCarriageFinalDestinationATA' or FieldName = 'MainCarriageFinalDestinationETA' or FieldName = 'MainCarriageCarrierNumber'  or FieldName='MainCarriageCarrierId'  or FieldName = 'AirlinePrefix'
or FieldName='BookingConfirmationNumber' or FieldName='MainCarriageATA' or FieldName='MAWBOBLDate'  or FieldName='MasterShipmentNumber' or FieldName='MainCarriageETA' or FieldName='MainCarriageVesselId' or FieldName = 'Transshipment1ETA' or FieldName = 'Transshipment1ETD' or FieldName = 'Transshipment1ATA' or FieldName = 'Transshipment1ATD'  or FieldName ='Transshipment1AdditionalMAWBOBLBL' or FieldName= 'CutoffDate' or FieldName ='Transshipment1VesselId'  or FieldName ='Transshipment1CarrierId' or FieldName = 'BookingConfirmationNotes' or FieldName = 'BookingConfirmedBy'

)


update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'MoveType') and (FieldName = 'Code' or FieldName = 'MoveTypeEnglishName' or FieldName = 'MoveTypeLocalName' or FieldName = 'TransportModeId' )
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'Vessel') and (FieldName = 'Code' or FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Notes' or FieldName = 'IMOCode' or FieldName='EnglishName')
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'SpecialServicesType') and (FieldName = 'Code' or FieldName = 'EnglishName' or FieldName = 'LocalName' )
update  ObjectFields  set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentComputedFields') and (FieldName = 'FirstPickupATD' or FieldName = 'FirstPickupATA' or FieldName = 'FinalDeliveryETD' or FieldName = 'FinalDeliveryETA' or FieldName = 'FinalDeliveryATD' or FieldName = 'FinalDeliveryATA' or FieldName='ContainersNumbers' or FieldName ='FirstPickupLocation' or FieldName ='NumberOfDeliveries' or FieldName ='OperationallyClosedByUserId' or FieldName ='LastPickupATA' or FieldName ='LastPickupATD' or FieldName ='LastPickupETA' or FieldName ='LastPickupETD' or  FieldName ='DeliveryToPortId' or FieldName ='DeliveryFrom' or FieldName ='DeliveryTo' or FieldName ='PickupFrom' or FieldName ='PickupTo')


