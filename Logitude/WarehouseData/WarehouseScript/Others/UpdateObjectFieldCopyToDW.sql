

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'DWHSetting') and (FieldName = 'Tenant' or FieldName = 'ParentTenant')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Direction') and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'TransportMode')and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentLevel')and (FieldName = 'Code' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentType')and (FieldName = 'Id' or FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'Branch') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Code' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Card') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'CityName' or FieldName = 'ZipCode'  or FieldName='SalesmanUserId' or FieldName='PrimaryContactId' or FieldName='PartnerTypeId'  or  FieldName='CountryName')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'User') and (FieldName = 'BranchId' or FieldName = 'DepartmentId'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Contact') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Email' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Currency') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName = 'Code' or FieldName = 'Sign')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Department') and (FieldName = 'EnglishName' or FieldName = 'LocalName' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Port') and (FieldName = 'EnglishName' or FieldName = 'LocalName' or FieldName='Code'  or FieldName='CombinedCode' or  FieldName='CountryId' or FieldName='StateId')


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Address') and ( FieldName = 'StateId' or FieldName = 'CountryId' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Country') and (FieldName = 'EnglishName')
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'State') and (FieldName = 'EnglishName' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'PartnerType') and (FieldName = 'Name' )


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Customer') and (FieldName = 'AccountManagerUserId' or FieldName = 'RankId'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Tenant') and (FieldName = 'AddressId' or FieldName = 'CountryId' or FieldName = 'Company' or  FieldName = 'CurrencyId' )


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Incoterm') and (FieldName = 'Name' or FieldName = 'LocalName' or FieldName='Code' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'EntityStatus') and (FieldName = 'Name' or FieldName = 'Code' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Rank') and (FieldName = 'Name'  )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Shipment') and (FieldName = 'DirectionId' or FieldName = 'TransportModeId' or FieldName = 'ShipmentLevelCode' or FieldName = 'ShipmentTypeId' or FieldName = 'DepartmentId' or FieldName = 'BranchId'or FieldName = 'MasterShipmentDataId' or FieldName = 'ShipperId'or FieldName = 'ConsigneeId' or FieldName = 'AgentId'or FieldName = 'CustomerId' or FieldName = 'IncotermId'or FieldName = 'SalesmanUserId' or FieldName = 'AccountManagerUserId'or FieldName = 'ProfitCurrencyId' or FieldName = 'StatusId'or FieldName = 'FromPortId' or FieldName = 'ToPortId'  
or FieldName = 'ShipmentNumber' or FieldName = 'House' or FieldName = 'GrossWeightInKG' or FieldName = 'ChargeableWeightInKG' or FieldName = 'VolumeInCBM' or FieldName = 'NumberOfPackages'or FieldName = 'NumberOfContainers' or FieldName = 'AccountedReceivablesInLocalCurrency'or FieldName = 'AccountedPayablesInLocalCurrency' or FieldName = 'ProfitInLocalCurrency'or FieldName = 'AccountedReceivablesInProfitCurrency' or FieldName = 'AccountedPayablesInProfitCurrency'or FieldName = 'ProfitInProfitCurrency' or FieldName = 'IsOperationalClosed'or FieldName = 'IsAccountingClosed' or FieldName = 'StatusLocation'or FieldName = 'FinalArrivalDate' or FieldName = 'CustomsClearanceDate' or FieldName = 'CreateDateTime' or  FieldName = 'OperationalDate'or  FieldName = 'OperationalCloseDate'or  FieldName = 'AccountingCloseDate'or  FieldName = 'LastUpdateDate' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Master') and (FieldName = 'MainCarriageATD' or FieldName = 'Master' or FieldName = 'MainCarriageToPortId' or FieldName = 'Transshipment1ToPortId' or FieldName = 'Transshipment2ToPortId' or FieldName = 'Transshipment3ToPortId' )







