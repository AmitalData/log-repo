

update DWHBuildStatus set IsFullBuildDWRunning =0 , IsIncrementalDWRunning = 0
select * from ObjectFields where CopyToDW = 1



IF OBJECT_ID('[dbo].[usp_ReconnectObjectTableMetadata]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
GO
create PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
(
    @pTableName    varchar(50)
)
AS

 
 
 Declare @ObjectTableId As varchar(15)

 Begin
 
 SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
----MetaData All Scripts: Never Apply these scripts

 

--*--After Delete--*--
--ObjectFields
update querycolumns set ObjectFieldId = (select Id from objectfields where FieldCode=querycolumns.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update ScreenFields set ObjectFieldId = (select Id from objectfields where FieldCode=ScreenFields.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update AdvancedQueryFilters set ObjectFieldId = (select Id from objectfields where FieldCode=AdvancedQueryFilters.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update RuleConditionFields set ObjectFieldId = (select Id from objectfields where FieldCode=RuleConditionFields.ObjectFieldCode)
update ObjectTableRuleFields set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectTableRuleFields.ObjectFieldCode)
update ObjectTableRules set TriggerFieldId = (select Id from objectfields where FieldCode=ObjectTableRules.TriggerFieldCode)
update AirlineMessagingRules set RuleFieldId = (select Id from objectfields where FieldCode=AirlineMessagingRules.RuleFieldCode)
update restrictions  set ObjectFieldId = (select Id from objectfields where FieldCode=restrictions.ObjectFieldCode)
update CustomerFieldsUpdateSettings  set ObjectFieldId = (select Id from objectfields where FieldCode=CustomerFieldsUpdateSettings.ObjectFieldCode)
update ObjectFieldValidations  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldValidations.ObjectFieldCode)
update ObjectFieldModifications  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldModifications.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)

--Screens
update ScreenModifications set ScreenId = (select Id from Screens where code=ScreenModifications.ScreenCode) where screencode in (select code from screens)
update ScreenFields set ScreenId = (select Id from Screens where code=ScreenFields.ScreenCode) where screencode in (select code from screens)
update ObjectTables set HeaderScreenId = (select Id from Screens where code=ObjectTables.HeaderScreenCode)

--Queries
update Queries set OriginalQueryId =  (select q1.Id from Queries q1 where q1.UniqueCode = Queries.OriginalQueryCode)
update AdvancedQueryFilters set QueryId = (select Id from Queries where UniqueCode = AdvancedQueryFilters.QueryCode) where QueryCode in (select UniqueCode from Queries)
update QueryColumns set QueryId = (select Id from Queries where UniqueCode = QueryColumns.QueryCode) where QueryCode in (select UniqueCode from Queries)
update SharedUserQueries set QueryId = (select Id from Queries where UniqueCode = SharedUserQueries.QueryCode)

--TextCodes
update Queries set NameTextCodeId = (select Id from TextCodes where Code=Queries.NameTextCodeCode and tenant = Queries.Tenant)
update ObjectTables set DescriptionTextCodeId = (select Id from TextCodes where Code=ObjectTables.DescriptionTextCodeCode and tenant = ObjectTables.Tenant)
update ObjectTables set NewButtonTextCodeId = (select Id from TextCodes where Code=ObjectTables.NewButtonTextCodeCode)
update Features set NameTextCodeId = (select Id from TextCodes where Code=Features.NameTextCodeCode and tenant = Features.Tenant)
update Tips set ShortTextCode = (select Id from TextCodes where Code=Tips.ShortTextCodeCode)
update ObjectTableTabs set TabNameTextCodeId = (select Id from TextCodes where Code=ObjectTableTabs.TabNameTextCodeCode and tenant = ObjectTableTabs.Tenant)
update MenuButtons set LabelTextCodeId = (select Id from TextCodes where Code=MenuButtons.LabelTextCodeCode)
update ObjectFields set FullNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.FullNameTextCodeCode and tenant = ObjectFields.Tenant) 
update ObjectFields set HelpTextCodeId = (select Id from TextCodes where Code=ObjectFields.HelpTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ShortNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.ShortNameTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ListTextCodeId = (select Id from TextCodes where Code=ObjectFields.ListTextCodeCode and tenant = ObjectFields.Tenant)
update Translations set TextCodeId = (select Id from TextCodes where Code=Translations.TextCodeCode and (tenant =  Translations.Tenant or tenant = 0)) where TextCodeCode in (select code from textcodes)

--Features
update MenusTables set FeatureId = (select Id from features where FeatureUniqeCode=MenusTables.FeatureUniqeCode)
update Queries set FeatureId = (select Id from features where FeatureUniqeCode=Queries.FeatureUniqeCode)
update ObjectTableTabs set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableTabs.FeatureUniqeCode)
update ObjectTableHelperControls set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableHelperControls.FeatureUniqeCode)
update MenuButtons set FeatureId = (select Id from features where FeatureUniqeCode=MenuButtons.FeatureUniqeCode)
update Reports set FeatureId = (select Id from features where FeatureUniqeCode=Reports.FeatureUniqeCode)
update PackageFeatures set FeatureId = (select Id from features where FeatureUniqeCode=PackageFeatures.FeatureUniqeCode) where PackageFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)
update RoleFeatures set FeatureId = (select Id from features where FeatureUniqeCode=RoleFeatures.FeatureUniqeCode) where RoleFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)



-------------

 
UPDATE f
SET f.IsOld = temp.IsOld
FROM Features f
JOIN TempOldFeatures temp
    ON f.FeatureUniqeCode = temp.FeatureUniqeCode

update tc
set tc.DefaultText = temp.DefaultText, tc.DefaultTextPlural = temp.DefaultTextPlural, tc.SpellCheckDate = temp.SpellCheckDate,
tc.SpellCheckedByUserId = temp.SpellCheckedByUserId,tc.LocalDefaultText = temp.LocalDefaultText,IsSpellChecked = temp.IsSpellChecked
from TextCodes tc
Join TempIsSpellCheckedTextCodes temp
on tc.Code = temp.Code
where tc.ObjectTableId = temp.ObjectTableId






--Abed DataWarehouse Fields
update  ObjectFields set CopyToDW = 0 
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'DWHSetting') and ( FieldName = 'ParentTenant')
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





update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Customer') and (FieldName = 'LeadSourceId' or FieldName = 'CreditLimitAmount' or FieldName = 'CreditLimitOpenBalance' or FieldName = 'AccountManagerUserId' or FieldName = 'RankId'  or FieldName = 'RegionId' or FieldName = 'CustomerSizeId'  or FieldName = 'IndustryId' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Tenant') and (FieldName = 'AddressId' or FieldName = 'CountryId' or FieldName = 'Company' or  FieldName = 'CurrencyId' )


update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Incoterm') and (FieldName = 'Name' or FieldName = 'LocalName' or FieldName='Code' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'EntityStatus') and (FieldName = 'Name' or FieldName = 'Code' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'Rank') and (FieldName = 'Name'  )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'LeadSource') and (FieldName = 'Name'  )

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


update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentPayable') and (FieldName = 'VendorId' or FieldName = 'ChargesTypeId' or FieldName = 'ShipmentId'  or FieldName = 'OpenAmount' or FieldName = 'OpenAmountInLocalCurrency' or FieldName = 'OpenAmountInProfitCurrency' or FieldName = 'AccountedAmount' or FieldName = 'AccountedAmountInLocalCurrency' or FieldName = 'AccountedAmountInProfitCurrency' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'APInvoiceLine') and (FieldName = 'EntityPayableId' or FieldName = 'LineNumber' or FieldName ='ProfitCurrencyAmount' or FieldName ='LocalCurrencyAmount')
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'APInvoice') and (FieldName = 'InvoiceNumber' or FieldName = 'AmountInInvoiceCurrency'  or FieldName = 'InvoiceCurrencyId' or FieldName = 'InvoiceCurrencyExchangeRate' )

update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ShipmentReceivable') and (FieldName = 'AmountInProfitCurrency'   or FieldName = 'ChargesTypeId' or FieldName = 'ShipmentId' or FieldName = 'TotalAmount' or FieldName = 'TotalAmountLocal' or FieldName =  'ARInvoiceLineId' )
update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ARInvoiceLineId') and (FieldName = 'ReceivableId' or FieldName = 'ARInvoiceId')
update  ObjectFields set CopyToDW = 1  where ObjectTableId = (select id from ObjectTables where Name = 'ARInvoice') and (FieldName = 'BillToId'  or FieldName = 'InvoiceNumber' or FieldName = 'AmountInInvoiceCurrency'  or FieldName = 'InvoiceCurrencyId' or FieldName = 'InvoiceCurrencyExchangeRate' )



update  ObjectFields set CopyToDW = 1 where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType') and (FieldName = 'Code' or FieldName = 'EnglishName' or FieldName = 'LocalName'  or FieldName = 'ChargesGroupCode' or FieldName='ChargesGroupId')


------------------------------------


 End