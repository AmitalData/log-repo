CREATE OR REPLACE PROCEDURE usp_ReconnectObjectTableMetada(
    v_pTableName IN VARCHAR2 )
AS
  v_ObjectTableId VARCHAR2(15);
BEGIN
  BEGIN
    SELECT Id
    INTO v_ObjectTableId
    FROM ObjectTables
    WHERE NAME  = v_pTableName
    AND ROWNUM <= 1;
    ----MetaData All Scripts: Never Apply these scripts
    --*--After Delete--*--
    --ObjectFields
    UPDATE querycolumns
    SET ObjectFieldId =
      ( SELECT Id FROM objectfields WHERE FieldCode = querycolumns.ObjectFieldCode
      )
    WHERE objectfieldcode IN
      ( SELECT fieldcode FROM ObjectFields
      ) ;
    UPDATE ScreenFields
    SET ObjectFieldId =
      ( SELECT Id FROM objectfields WHERE FieldCode = ScreenFields.ObjectFieldCode
      )
    WHERE objectfieldcode IN
      ( SELECT fieldcode FROM ObjectFields
      ) ;
    UPDATE AdvancedQueryFilters
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = AdvancedQueryFilters.ObjectFieldCode
      )
    WHERE objectfieldcode IN
      ( SELECT fieldcode FROM ObjectFields
      ) ;
    UPDATE RuleConditionFields
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = RuleConditionFields.ObjectFieldCode
      );
    UPDATE ObjectTableRuleFields
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = ObjectTableRuleFields.ObjectFieldCode
      );
    UPDATE ObjectTableRules
    SET TriggerFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = ObjectTableRules.TriggerFieldCode
      );
    UPDATE AirlineMessagingRules
    SET RuleFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = AirlineMessagingRules.RuleFieldCode
      );
    UPDATE restrictions
    SET ObjectFieldId =
      ( SELECT Id FROM objectfields WHERE FieldCode = restrictions.ObjectFieldCode
      );
    UPDATE CustomerFieldsUpdateSettings
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = CustomerFieldsUpdateSettings.ObjectFieldCode
      );
    UPDATE ObjectFieldValidations
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = ObjectFieldValidations.ObjectFieldCode
      );
    UPDATE ObjectFieldModifications
    SET ObjectFieldId =
      (SELECT Id
      FROM objectfields
      WHERE FieldCode = ObjectFieldModifications.ObjectFieldCode
      )
    WHERE objectfieldcode IN
      ( SELECT fieldcode FROM ObjectFields
      ) ;
    --Screens
    UPDATE ScreenModifications
    SET ScreenId =
      ( SELECT Id FROM Screens WHERE code = ScreenModifications.ScreenCode
      )
    WHERE screencode IN
      ( SELECT code FROM screens
      ) ;
    UPDATE ScreenFields
    SET ScreenId =
      ( SELECT Id FROM Screens WHERE code = ScreenFields.ScreenCode
      )
    WHERE screencode IN
      ( SELECT code FROM screens
      ) ;
    UPDATE ObjectTables
    SET HeaderScreenId =
      ( SELECT Id FROM Screens WHERE code = ObjectTables.HeaderScreenCode
      );
    --Queries
    UPDATE Queries
    SET OriginalQueryId =
      (SELECT q1.Id
      FROM Queries q1
      WHERE q1.UniqueCode = Queries.OriginalQueryCode
      );
    UPDATE AdvancedQueryFilters
    SET QueryId =
      ( SELECT Id FROM Queries WHERE UniqueCode = AdvancedQueryFilters.QueryCode
      )
    WHERE QueryCode IN
      ( SELECT UniqueCode FROM Queries
      ) ;
    UPDATE QueryColumns
    SET QueryId =
      ( SELECT Id FROM Queries WHERE UniqueCode = QueryColumns.QueryCode
      )
    WHERE QueryCode IN
      ( SELECT UniqueCode FROM Queries
      ) ;
    UPDATE SharedUserQueries
    SET QueryId =
      ( SELECT Id FROM Queries WHERE UniqueCode = SharedUserQueries.QueryCode
      );
    --TextCodes
    UPDATE Queries
    SET NameTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = Queries.NameTextCodeCode
      AND tenant = Queries.Tenant
      );
    UPDATE ObjectTables
    SET DescriptionTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectTables.DescriptionTextCodeCode
      AND tenant = ObjectTables.Tenant
      );
    UPDATE ObjectTables
    SET NewButtonTextCodeId =
      ( SELECT Id FROM TextCodes WHERE Code = ObjectTables.NewButtonTextCodeCode
      );
    UPDATE Features
    SET NameTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = Features.NameTextCodeCode
      AND tenant = Features.Tenant
      );
    UPDATE Tips
    SET ShortTextCode =
      ( SELECT Id FROM TextCodes WHERE Code = Tips.ShortTextCodeCode
      );
    UPDATE ObjectTableTabs
    SET TabNameTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectTableTabs.TabNameTextCodeCode
      AND tenant = ObjectTableTabs.Tenant
      );
    UPDATE MenuButtons
    SET LabelTextCodeId =
      ( SELECT Id FROM TextCodes WHERE Code = MenuButtons.LabelTextCodeCode
      );
    UPDATE ObjectFields
    SET FullNameTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectFields.FullNameTextCodeCode
      AND tenant = ObjectFields.Tenant
      );
    UPDATE ObjectFields
    SET HelpTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectFields.HelpTextCodeCode
      AND tenant = ObjectFields.Tenant
      );
    UPDATE ObjectFields
    SET ShortNameTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectFields.ShortNameTextCodeCode
      AND tenant = ObjectFields.Tenant
      );
    UPDATE ObjectFields
    SET ListTextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code = ObjectFields.ListTextCodeCode
      AND tenant = ObjectFields.Tenant
      );
    UPDATE Translations
    SET TextCodeId =
      (SELECT Id
      FROM TextCodes
      WHERE Code   = Translations.TextCodeCode
      AND ( tenant = Translations.Tenant
      OR tenant    = 0 )
      )
    WHERE TextCodeCode IN
      ( SELECT code FROM textcodes
      ) ;
    --Features
    UPDATE MenusTables
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = MenusTables.FeatureUniqeCode
      );
    UPDATE Queries
    SET FeatureId =
      ( SELECT Id FROM features WHERE FeatureUniqeCode = Queries.FeatureUniqeCode
      );
    UPDATE ObjectTableTabs
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = ObjectTableTabs.FeatureUniqeCode
      );
    UPDATE ObjectTableHelperControls
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = ObjectTableHelperControls.FeatureUniqeCode
      );
    UPDATE MenuButtons
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = MenuButtons.FeatureUniqeCode
      );
    UPDATE Reports
    SET FeatureId =
      ( SELECT Id FROM features WHERE FeatureUniqeCode = Reports.FeatureUniqeCode
      );
    UPDATE PackageFeatures
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = PackageFeatures.FeatureUniqeCode
      )
    WHERE PackageFeatures.FeatureUniqeCode IN
      ( SELECT FeatureUniqeCode FROM Features
      ) ;
    UPDATE RoleFeatures
    SET FeatureId =
      (SELECT Id
      FROM features
      WHERE FeatureUniqeCode = RoleFeatures.FeatureUniqeCode
      )
    WHERE RoleFeatures.FeatureUniqeCode IN
      ( SELECT FeatureUniqeCode FROM Features
      ) ;
    -------------
    MERGE INTO Features f USING
    (SELECT f.ROWID row_id,
      temp.IsOld
    FROM Features ,
      Features f
    JOIN TempOldFeatures temp
    ON f.FeatureUniqeCode = temp.FeatureUniqeCode
    ) src ON ( f.ROWID    = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET f.IsOld = src.IsOld;
    MERGE INTO TextCodes tc USING
    (SELECT tc.ROWID row_id,
      temp.DefaultText,
      temp.DefaultTextPlural,
      temp.SpellCheckDate,
      temp.SpellCheckedByUserId,
      temp.LocalDefaultText,
      temp.IsSpellChecked
    FROM TextCodes ,
      TextCodes tc
    JOIN TempIsSpellCheckedTextCodes temp
    ON tc.Code             = temp.Code
    WHERE tc.ObjectTableId = temp.ObjectTableId
    ) src ON ( tc.ROWID    = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET tc.DefaultText        = src.DefaultText,
      tc.DefaultTextPlural    = src.DefaultTextPlural,
      tc.SpellCheckDate       = src.SpellCheckDate,
      tc.SpellCheckedByUserId = src.SpellCheckedByUserId,
      tc.LocalDefaultText     = src.LocalDefaultText,
      IsSpellChecked          = src.IsSpellChecked;
    --Abed DataWarehouse Fields
    UPDATE ObjectFields SET CopyToDW = 0;
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'DWHSetting'
      )
    AND ( FieldName = 'ParentTenant' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Direction'
      )
    AND ( FieldName = 'Id'
    OR FieldName    = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'TransportMode'
      )
    AND ( FieldName = 'Id'
    OR FieldName    = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ShipmentLevel'
      )
    AND ( FieldName = 'Code'
    OR FieldName    = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ShipmentType'
      )
    AND ( FieldName = 'Id'
    OR FieldName    = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Branch'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Code' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Card'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'Code'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'VatNumber'
    OR FieldName    = 'CityName'
    OR FieldName    = 'ZipCode'
    OR FieldName    = 'SalesmanUserId'
    OR FieldName    = 'PrimaryContactId'
    OR FieldName    = 'PartnerTypeId'
    OR FieldName    = 'CountryName'
    OR FieldName    = 'CountryId'
    OR FieldName    = 'ReceivablesAccountingCard'
    OR FieldName    = 'Address1'
    OR FieldName    = 'Address2'
    OR FieldName    = 'Phone' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'User'
      )
    AND ( FieldName = 'BranchId'
    OR FieldName    = 'DepartmentId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Contact'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Email' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Currency'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Code'
    OR FieldName    = 'Sign' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Department'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'LocalName' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Port'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Code'
    OR FieldName    = 'CombinedCode'
    OR FieldName    = 'CountryId'
    OR FieldName    = 'StateId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Address'
      )
    AND ( FieldName = 'StateId'
    OR FieldName    = 'CountryId'
    OR FieldName    = 'AddressTypeId'
    OR FieldName    = 'CardId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Country'
      )
    AND ( FieldName = 'EnglishName'
    OR FieldName    = 'Code' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'State'
      )
    AND ( FieldName = 'EnglishName' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'PartnerType'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Customer'
      )
    AND ( FieldName = 'LeadSourceId'
    OR FieldName    = 'CreditLimitAmount'
    OR FieldName    = 'CreditLimitOpenBalance'
    OR FieldName    = 'AccountManagerUserId'
    OR FieldName    = 'RankId'
    OR FieldName    = 'RegionId'
    OR FieldName    = 'CustomerSizeId'
    OR FieldName    = 'IndustryId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Tenant'
      )
    AND ( FieldName = 'AddressId'
    OR FieldName    = 'CountryId'
    OR FieldName    = 'Company'
    OR FieldName    = 'CurrencyId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Incoterm'
      )
    AND ( FieldName = 'Name'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Code' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'EntityStatus'
      )
    AND ( FieldName = 'Name'
    OR FieldName    = 'Code' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Rank'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'LeadSource'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Region'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'CustomerSize'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Industry'
      )
    AND ( FieldName = 'Name' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'CustomPickList'
      )
    AND ( FieldName = 'Value'
    OR FieldName    = 'Code'
    OR FieldName    = 'IsMultipleChoice' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Shipment'
      )
    AND ( FieldName = 'DirectionId'
    OR FieldName    = 'TransportModeId'
    OR FieldName    = 'ShipmentLevelCode'
    OR FieldName    = 'ShipmentTypeId'
    OR FieldName    = 'DepartmentId'
    OR FieldName    = 'BranchId'
    OR FieldName    = 'MasterShipmentDataId'
    OR FieldName    = 'ShipperId'
    OR FieldName    = 'ConsigneeId'
    OR FieldName    = 'AgentId'
    OR FieldName    = 'CustomerId'
    OR FieldName    = 'IncotermId'
    OR FieldName    = 'SalesmanUserId'
    OR FieldName    = 'AccountManagerUserId'
    OR FieldName    = 'ProfitCurrencyId'
    OR FieldName    = 'StatusId'
    OR FieldName    = 'FromPortId'
    OR FieldName    = 'ToPortId'
    OR FieldName    = 'ShipmentNumber'
    OR FieldName    = 'House'
    OR FieldName    = 'GrossWeightInKG'
    OR FieldName    = 'ChargeableWeightInKG'
    OR FieldName    = 'VolumeInCBM'
    OR FieldName    = 'NumberOfPackages'
    OR FieldName    = 'NumberOfContainers'
    OR FieldName    = 'ProfitInLocalCurrency'
    OR FieldName    = 'ProfitInProfitCurrency'
    OR FieldName    = 'IsOperationalClosed'
    OR FieldName    = 'IsAccountingClosed'
    OR FieldName    = 'StatusLocation'
    OR FieldName    = 'FinalArrivalDate'
    OR FieldName    = 'CustomsClearanceDate'
    OR FieldName    = 'CreateDateTime'
    OR FieldName    = 'OperationalDate'
    OR FieldName    = 'OperationalCloseDate'
    OR FieldName    = 'AccountingCloseDate'
    OR FieldName    = 'LastUpdateDate'
    OR FieldName    = 'OpenReceivablesInLocalCurrency'
    OR FieldName    = 'OpenReceivablesInProfitCurrency'
    OR FieldName    = 'AccountedReceivablesInLocalCurrency'
    OR FieldName    = 'AccountedReceivablesInProfitCurrency'
    OR FieldName    = 'OpenPayablesInLocalCurrency'
    OR FieldName    = 'OpenPayablesInProfitCurrency'
    OR FieldName    = 'AccountedPayablesInLocalCurrency'
    OR FieldName    = 'AccountedPayablesInProfitCurrency'
    OR FieldName    = 'TEU'
    OR FieldName    = 'ValueOfGoods'
    OR FieldName    = 'ValueOfGoodsCurrencyId'
    OR FieldName    = 'FirstPickupETA'
    OR FieldName    = 'FirstPickupETD'
    OR FieldName    = 'ProjectNumber'
    OR FieldName    = 'CustomerReference1'
    OR FieldName    = 'CustomerReference2'
    OR FieldName    = 'ShipperReference1'
    OR FieldName    = 'ShipperReference2'
    OR FieldName    = 'ConsigneeReference1'
    OR FieldName    = 'ConsigneeReference2'
    OR FieldName    = 'AgentReference1'
    OR FieldName    = 'AgentReference2'
    OR FieldName    = 'AMSBL'
    OR FieldName    = 'FreightPrepaidCollectId'
    OR FieldName    = 'OtherPrepaidCollectId'
    OR FieldName    = 'MainHarmonize'
    OR FieldName    = 'ForwarderPartnerId'
    OR FieldName    = 'CreatedByUserId'
    OR FieldName    = 'CustomAgentExportId'
    OR FieldName    = 'CustomAgentImportId'
    OR FieldName    = 'CustomAgentExportId'
    OR FieldName    = 'WarehouseLegWarehouseId'
    OR FieldName    = 'IsCancelled'
    OR FieldName    = 'StatusDate'
    OR FieldName    = 'CustomsDeclarationNumber'
    OR FieldName    = 'FirstOperationalCloseDate'
    OR FieldName    = 'EstimatedFinalArrivalDate'
    OR FieldName    = 'ActualFinalArrivalDate'
    OR FieldName    = 'Routing'
    OR FieldName    = 'DescriptionOfGoods'
    OR FieldName    = 'PreCarriageETD'
    OR FieldName    = 'MoveTypeId'
    OR FieldName    = 'SpecialServicesTypeId'
    OR FieldName    = 'AgentComputed'
    OR FieldName    = 'ARInvoices'
    OR FieldName    = 'ConsolidatorId'
    OR FieldName    = 'ConsolidatorReference'
    OR FieldName    = 'Notes'
    OR FieldName    = 'Notify1Id'
    OR FieldName    = 'Notify1Reference'
    OR FieldName    = 'Notify2Id'
    OR FieldName    = 'Notify2Reference'
    OR FieldName    = 'ColoaderId'
    OR FieldName    = 'ColoaderReference1'
    OR FieldName    = 'ShipperNotExporterId'
    OR FieldName    = 'ShipperNotExporterReference'
    OR FieldName    = 'ReleasingAgentId'
    OR FieldName    = 'ReleasingAgentReference1'
    OR FieldName    = 'Ratio'
    OR FieldName    = 'VolumetricWeight'
    OR FieldName    = 'WarehouseLegActualEntryDate'
    OR FieldName    = 'WarehouseLegExpectedEntryDate'
    OR FieldName    = 'WarehouseLegActualReleaseDate'
    OR FieldName    = 'WarehouseLegExpectedReleaseDate'
    OR FieldName    = 'ChargeableWeightUnitCode'
    OR FieldName    = 'IncludesCustoms'
    OR FieldName    = 'DeclarationNumber'
    OR FieldName    = 'DeclarationDate'
    OR FieldName    = 'TerminalAvailable'
    OR FieldName    = 'WarehouseLegLastFreeDate'
    OR FieldName    = 'OrderGrossWeight'
    OR FieldName    = 'OrderChargeableWeight'
    OR FieldName    = 'BookingVolume'
    OR FieldName    = 'BookingNumberOfPackages'
    OR FieldName    = 'EstimateProfitInProfitCurrency'
    OR FieldName    = 'EstimateProfitInLocalCurrency'
    OR FieldName    = 'GrossWeightUnitCode'
    OR FieldName    = 'VolumeUnitCode'
    OR FieldName    = 'ConsigneeNotImporterId'
    OR FieldName    = 'IssuingCarrierAgentId'
    OR FieldName    = 'OnCarriageTransportModeId'
    OR FieldName    = 'FirstARInvoiceApprovalDate'
    OR FieldName    = 'FreightForwarderId'
    OR FieldName    = 'FreightRelease' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Master'
      )
    AND ( FieldName = 'MainCarriageATD'
    OR FieldName    = 'Master'
    OR FieldName    = 'MainCarriageToPortId'
    OR FieldName    = 'Transshipment1ToPortId'
    OR FieldName    = 'Transshipment2ToPortId'
    OR FieldName    = 'Transshipment3ToPortId'
    OR FieldName    = 'MainCarriageETD'
    OR FieldName    = 'MainCarriageFinalDestinationATA'
    OR FieldName    = 'MainCarriageFinalDestinationETA'
    OR FieldName    = 'MainCarriageCarrierNumber'
    OR FieldName    = 'MainCarriageCarrierId'
    OR FieldName    = 'AirlinePrefix'
    OR FieldName    = 'BookingConfirmationNumber'
    OR FieldName    = 'MainCarriageATA'
    OR FieldName    = 'MAWBOBLDate'
    OR FieldName    = 'MasterShipmentNumber'
    OR FieldName    = 'MainCarriageETA'
    OR FieldName    = 'MainCarriageVesselId'
    OR FieldName    = 'Transshipment1ETA'
    OR FieldName    = 'Transshipment1ETD'
    OR FieldName    = 'Transshipment1ATA'
    OR FieldName    = 'Transshipment1ATD'
    OR FieldName    = 'Transshipment1AdditionalMAWBOBLBL'
    OR FieldName    = 'CutoffDate'
    OR FieldName    = 'Transshipment1VesselId'
    OR FieldName    = 'Transshipment1CarrierId'
    OR FieldName    = 'BookingConfirmationNotes'
    OR FieldName    = 'BookingConfirmedBy' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'MoveType'
      )
    AND ( FieldName = 'Code'
    OR FieldName    = 'MoveTypeEnglishName'
    OR FieldName    = 'MoveTypeLocalName'
    OR FieldName    = 'TransportModeId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'Vessel'
      )
    AND ( FieldName = 'Code'
    OR FieldName    = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'Notes'
    OR FieldName    = 'IMOCode'
    OR FieldName    = 'EnglishName' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'SpecialServicesType'
      )
    AND ( FieldName = 'Code'
    OR FieldName    = 'EnglishName'
    OR FieldName    = 'LocalName' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ShipmentComputedFields'
      )
    AND ( FieldName = 'FirstPickupATD'
    OR FieldName    = 'FirstPickupATA'
    OR FieldName    = 'FinalDeliveryETD'
    OR FieldName    = 'FinalDeliveryETA'
    OR FieldName    = 'FinalDeliveryATD'
    OR FieldName    = 'FinalDeliveryATA'
    OR FieldName    = 'ContainersNumbers'
    OR FieldName    = 'FirstPickupLocation'
    OR FieldName    = 'NumberOfDeliveries'
    OR FieldName    = 'OperationallyClosedByUserId'
    OR FieldName    = 'LastPickupATA'
    OR FieldName    = 'LastPickupATD'
    OR FieldName    = 'LastPickupETA'
    OR FieldName    = 'LastPickupETD'
    OR FieldName    = 'DeliveryToPortId'
    OR FieldName    = 'DeliveryFrom'
    OR FieldName    = 'DeliveryTo'
    OR FieldName    = 'PickupFrom'
    OR FieldName    = 'PickupTo' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ShipmentPayable'
      )
    AND ( FieldName = 'VendorId'
    OR FieldName    = 'ChargesTypeId'
    OR FieldName    = 'ShipmentId'
    OR FieldName    = 'OpenAmount'
    OR FieldName    = 'OpenAmountInLocalCurrency'
    OR FieldName    = 'OpenAmountInProfitCurrency'
    OR FieldName    = 'AccountedAmount'
    OR FieldName    = 'AccountedAmountInLocalCurrency'
    OR FieldName    = 'AccountedAmountInProfitCurrency' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'APInvoiceLine'
      )
    AND ( FieldName = 'EntityPayableId'
    OR FieldName    = 'LineNumber'
    OR FieldName    = 'ProfitCurrencyAmount'
    OR FieldName    = 'LocalCurrencyAmount' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'APInvoice'
      )
    AND ( FieldName = 'InvoiceNumber'
    OR FieldName    = 'AmountInInvoiceCurrency'
    OR FieldName    = 'InvoiceCurrencyId'
    OR FieldName    = 'InvoiceCurrencyExchangeRate' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ShipmentReceivable'
      )
    AND ( FieldName = 'AmountInProfitCurrency'
    OR FieldName    = 'ChargesTypeId'
    OR FieldName    = 'ShipmentId'
    OR FieldName    = 'TotalAmount'
    OR FieldName    = 'TotalAmountLocal'
    OR FieldName    = 'ARInvoiceLineId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ARInvoiceLineId'
      )
    AND ( FieldName = 'ReceivableId'
    OR FieldName    = 'ARInvoiceId' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ARInvoice'
      )
    AND ( FieldName = 'BillToId'
    OR FieldName    = 'InvoiceNumber'
    OR FieldName    = 'AmountInInvoiceCurrency'
    OR FieldName    = 'InvoiceCurrencyId'
    OR FieldName    = 'InvoiceCurrencyExchangeRate' );
    UPDATE ObjectFields
    SET CopyToDW        = 1
    WHERE ObjectTableId =
      ( SELECT id FROM ObjectTables WHERE NAME = 'ChargesType'
      )
    AND ( FieldName = 'Code'
    OR FieldName    = 'EnglishName'
    OR FieldName    = 'LocalName'
    OR FieldName    = 'ChargesGroupCode'
    OR FieldName    = 'ChargesGroupId' );------------------------------------
  END;
END;