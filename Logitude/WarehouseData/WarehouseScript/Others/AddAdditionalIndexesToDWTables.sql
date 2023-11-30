

IF (OBJECT_ID ('dw_ARInvoices', 'U')  IS NOT NULL)
BEGIN


IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoice_ConsolidationInvoiceId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoices]'))
  begin CREATE NONCLUSTERED INDEX [IX_ARInvoice_ConsolidationInvoiceId] ON [dbo].[dw_ARInvoices] ([ConsolidationInvoiceId]) end


  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoice_IsConsolidationInvoice_Id' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoices]'))
  begin CREATE NONCLUSTERED INDEX [IX_ARInvoice_IsConsolidationInvoice_Id] ON [dbo].[dw_ARInvoices] ([IsConsolidationInvoice],[Id]) end

 IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoice_AutomaticLastUpdateDate_IsConsolidationInvoice_Id' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoices]'))
    begin CREATE NONCLUSTERED INDEX [IX_ARInvoice_AutomaticLastUpdateDate_IsConsolidationInvoice_Id] ON [dbo].[dw_ARInvoices] ([AutomaticLastUpdateDate] ASC,[IsConsolidationInvoice] ASC,[Id] ASC) end




end



IF (OBJECT_ID ('dw_Addresses', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_dw_Addresses_AddressTypeId_CardId' 
    AND object_id = OBJECT_ID('[dbo].[dw_Addresses]'))
  begin CREATE NONCLUSTERED INDEX [IX_dw_Addresses_AddressTypeId_CardId] ON[dbo].[dw_Addresses]([AddressTypeId],[CardId]) end

end


IF (OBJECT_ID ('dw_APInvoices', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_APInvoice_Tenant_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[dw_APInvoices]'))
  begin CREATE NONCLUSTERED INDEX [IX_APInvoice_Tenant_AutomaticLastUpdateDate] ON [dbo].[dw_APInvoices] ([Tenant],[AutomaticLastUpdateDate]) end





end


IF (OBJECT_ID ('dw_Partners', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_dw_Partners_SalesmanUserId_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[dw_Partners]'))
  begin CREATE NONCLUSTERED INDEX [IX_dw_Partners_SalesmanUserId_AutomaticLastUpdateDate] ON[dbo].[dw_Partners]([SalesmanUserId],[AutomaticLastUpdateDate]) end



end



IF (OBJECT_ID ('dw_Shipments', 'U')  IS NOT NULL)
BEGIN


  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_IsStandalonePickupDelivery_ShipmentLevelCode_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dw_Shipments]'))
  begin
  
  CREATE NONCLUSTERED INDEX [IX_Shipment_IsStandalonePickupDelivery_ShipmentLevelCode_AutomaticLastUpdateDate] ON [dbo].[dw_Shipments] ([IsStandalonePickupDelivery] , [ShipmentLevelCode],[AutomaticLastUpdateDate])
  INCLUDE([ComputedStatusId],[FromPortId],[ToPortId],[IsOperationalClosed],[IsAccountingClosed],[ShipmentNumber],[House],[BranchId],[IncotermId],[SalesmanUserId],[CreatedByUserId],[CreateDateTime],[DepartmentId],[ShipmentTypeId],[CustomerId],[ShipperId],[ConsigneeId],[AgentComputed],[Routing],[DirectionId],[TransportModeId],[VolumeUnitCode],[AgentReference2],[AgentReference1],[GrossWeightUnitCode],[ChargeableWeightUnitCode],[VolumetricWeight],[GrossWeightInKG],[NumberOfPackages],[VolumeInCBM],[BookingNumberOfPackages],[OrderGrossWeight],[BookingVolume],[SpecialServicesTypeId],[CustomerReference1],[CustomerReference2],[AccountManagerUserId],[OperationalCloseDate],[AccountingCloseDate],[OperationalDate],[MasterShipmentDataId],[RegistryDate],[GrossWeightPerTon],[FirstOperationalCloseDate],[ProjectNumber],[HousesOpenPayablesInLocal],[HousesOpenPayablesInProfit],[HousesACCTPayablesInLocal],[HousesACCTPayablesInProfit],[HousesOpenReceivablesInLocal],[HousesOpenReceivablesInProfit],[HousesACCTReceivablesInLocal],[HousesACCTReceivablesInProfit],[PlannedCargoReadyDate],[ApprovedCargoReadyDate],[HandlerUserId],[Notify1Reference2],[Id],[Tenant]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

  end


    IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_IsCancelled_IsStandalonePickupDelivery_ShipmentLevelCode' 
    AND object_id = OBJECT_ID('[dw_Shipments]'))
  begin
  
  CREATE NONCLUSTERED INDEX [IX_Shipment_IsCancelled_IsStandalonePickupDelivery_ShipmentLevelCode] ON [dbo].[dw_Shipments] ([IsCancelled] , [IsStandalonePickupDelivery] , [ShipmentLevelCode])
INCLUDE ([ComputedStatusId],[FromPortId],[ToPortId],[IsOperationalClosed],[IsAccountingClosed],[ShipmentNumber],[House],[BranchId],[IncotermId],[SalesmanUserId],[CreatedByUserId],[CreateDateTime],[DepartmentId],[ShipmentTypeId],[CustomerId],[ShipperId],[ConsigneeId],[AgentComputed],[Routing],[DirectionId],[TransportModeId],[VolumeUnitCode],[AgentReference2],[AgentReference1],[GrossWeightUnitCode],[ChargeableWeightUnitCode],[VolumetricWeight],[GrossWeightInKG],[NumberOfPackages],[VolumeInCBM],[BookingNumberOfPackages],[OrderGrossWeight],[BookingVolume],[SpecialServicesTypeId],[CustomerReference1],[CustomerReference2],[AccountManagerUserId],[OperationalCloseDate],[AccountingCloseDate],[OperationalDate],[MasterShipmentDataId],[RegistryDate],[GrossWeightPerTon],[FirstOperationalCloseDate],[ProjectNumber],[HousesOpenPayablesInLocal],[HousesOpenPayablesInProfit],[HousesACCTPayablesInLocal],[HousesACCTPayablesInProfit],[HousesOpenReceivablesInLocal],[HousesOpenReceivablesInProfit],[HousesACCTReceivablesInLocal],[HousesACCTReceivablesInProfit],[PlannedCargoReadyDate],[ApprovedCargoReadyDate],[HandlerUserId],[Notify1Reference2],[Id],[Tenant])

  end



  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_IsCancelled_ShipmentLevelCode_RowNumber_Fact_Shipments' 
    AND object_id = OBJECT_ID('[dw_Shipments]'))
  begin CREATE NONCLUSTERED INDEX [IX_Shipment_IsCancelled_ShipmentLevelCode_RowNumber_Fact_Shipments] ON [dbo].[dw_Shipments] ([IsCancelled] , [ShipmentLevelCode],[RowNumber_Fact_Shipments]) end



   IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipmentLevelCode_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dw_Shipments]'))
  begin CREATE NONCLUSTERED INDEX [IX_Shipment_ShipmentLevelCode_AutomaticLastUpdateDate] ON [dbo].[dw_Shipments] ([ShipmentLevelCode],[AutomaticLastUpdateDate]) end


     IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='dw_Shipments_ShipmentLevelCode_AutomaticLastUpdateDate_InCludeSomeFields' 
    AND object_id = OBJECT_ID('[dw_Shipments]'))
  begin 
  CREATE NONCLUSTERED INDEX [dw_Shipments_ShipmentLevelCode_AutomaticLastUpdateDate_InCludeSomeFields]
ON [dbo].[dw_Shipments] ([ShipmentLevelCode],[AutomaticLastUpdateDate])
INCLUDE ([ProfitCurrencyId],[ComputedStatusId],[FreelancerAddressId],[FreelancerContactId],[ReleasingAgentId],[ReleasingAgentAddressId],[ReleasingAgentContactId],[FromPortId],[ToPortId],[ShipmentPayableStatusCode],[ShipmentReceivableStatusCode],[BranchId],[IncotermId],[SalesmanUserId],[CreatedByUserId],[DepartmentId],[ShipmentTypeId],[CustomerId],[ShipperId],[ConsigneeId],[AgentComputed],[CustomAgentExportId],[CustomAgentImportId],[Notify1Id],[Notify2Id],[FreightForwarderId],[ShipperAddressId],[CustomerAddressId],[ConsigneeAddressId],[AgentAddressId],[CustomAgentExportAddressId],[CustomAgentImportAddressId],[Notify1AddressId],[Notify2AddressId],[FreightForwarderAddressId],[CustomerContactId],[ShipperContactId],[ConsigneeContactId],[AgentContactId],[CustomAgentExportContactId],[CustomAgentImportContactId],[Notify1ContactId],[Notify2ContactId],[FreightForwarderContactId],[DirectionId],[TransportModeId],[ShipperNotExporterContactId],[ConsigneeNotImporterContactId],[ConsigneeNotImporterAddressId],[ShipperNotExporterAddressId],[ConsigneeNotImporterId],[ShipperNotExporterId],[IssuingCarrierAgentId],[CustomClearancePointAddressId],[CustomClearancePointContactId],[ColoaderId],[ColoaderAddressId],[ColoaderContactId],[MoveTypeId],[SpecialServicesTypeId],[AccountManagerUserId],[ConsolidatorId],[ConsolidatorAddressId],[ConsolidatorContactId],[ValueOfGoodsCurrencyId],[MasterShipmentDataId],[WarehouseLegWarehouseId],[ShipmentSubTypeId],[PreForwardingTransportModeId],[PreForwardingFromPortId],[PreForwardingToPortId],[PreForwardingCarrierId],[OnForwardingTransportModeId],[OnForwardingCarrierId],[HandlerUserId],[Id],[Tenant])

  end


end





IF (OBJECT_ID ('dw_ShipmentReceivables', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentReceivables_ShipmentId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ShipmentReceivables]'))
  begin 
  
  CREATE NONCLUSTERED INDEX [IX_ShipmentReceivables_ShipmentId] ON[dbo].[dw_ShipmentReceivables]([ShipmentId])
  INCLUDE ([ARInvoiceLineId],[AmountInProfitCurrency],[Notes],[TotalAmount],[TotalAmountLocal],[ChargesTypeId],[ShipmentReceivableParentId],[Id])

  end

end


IF (OBJECT_ID ('dw_PayableProratedAmounts', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PayableProratedAmounts_ShipmentId' 
    AND object_id = OBJECT_ID('[dbo].[dw_PayableProratedAmounts]'))
  begin 
  
  CREATE NONCLUSTERED INDEX [IX_PayableProratedAmounts_ShipmentId] ON[dbo].[dw_PayableProratedAmounts]([ShipmentId])
  INCLUDE ([ProratedAmountInLocalCurrency],[ProratedAmountInProfitCurrency])

  end

end






IF (OBJECT_ID ('dw_ShipmentPayables', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentPayables_ShipmentId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ShipmentPayables]'))
  begin 
  
  CREATE NONCLUSTERED INDEX [IX_ShipmentPayables_ShipmentId] ON[dbo].[dw_ShipmentPayables]([ShipmentId])
INCLUDE([ShipmentPayableParentId],[ChargesTypeId],[VendorId],[Notes],[ExpectedAmount],[ExpectedAmountLocal],[ExpectedAmountInProfitCurrency],[AccountedAmountInLocalCurrency],[AccountedAmountInProfitCurrency],[OpenAmountInProfitCurrency],[OpenAmountInLocalCurrency],[Id]) WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

  end

end

IF (OBJECT_ID ('dw_APInvoiceLines', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_APInvoiceLines_EntityPayableId' 
    AND object_id = OBJECT_ID('[dbo].[dw_APInvoiceLines]'))
  begin 
  
  CREATE NONCLUSTERED INDEX [IX_APInvoiceLines_EntityPayableId] ON[dbo].[dw_APInvoiceLines]([EntityPayableId])
INCLUDE([APInvoiceId],[LocalCurrencyAmount],[ProfitCurrencyAmount],[ForiegnCurrencyAmount],[ForiegnCurrencyId],[Notes])

  end

end



IF (OBJECT_ID ('dw_ARInvoiceLines', 'U')  IS NOT NULL)
BEGIN

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoiceLines_ReceivableId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoiceLines]'))
  begin 
  
  CREATE NONCLUSTERED INDEX [IX_ARInvoiceLines_ReceivableId] ON[dbo].[dw_ARInvoiceLines]([ReceivableId])
    INCLUDE ([ARInvoiceId],[LocalCurrencyAmount],[ProfitCurrencyAmount],[ForiegnCurrencyAmount],[ForiegnCurrencyId],[Notes],[Description] , [LocalDescription])					  -- ,

  end




  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoiceLines_ARInvoiceId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoiceLines]'))
  begin 

CREATE NONCLUSTERED INDEX [IX_ARInvoiceLines_ARInvoiceId] ON [dbo].[dw_ARInvoiceLines]([ARInvoiceId] ASC )
INCLUDE([ForiegnCurrencyId],[VatTypeId],[ChargesTypeId],[ForiegnCurrencyAmount],[Description],[LocalDescription],[UnitPrice],[Quantity] , [VatPercentage],[LocalCurrencyAmount],[InvoiceCurrencyAmount],[ProfitCurrencyAmount],[Notes],[ForiegnExchangeRate],[IsExpense],[IsRegionalTax],[EntityId]) 


end





end

