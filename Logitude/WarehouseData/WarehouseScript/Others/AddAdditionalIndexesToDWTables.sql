

IF (OBJECT_ID ('dw_ARInvoices', 'U')  IS NOT NULL)
BEGIN


IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoice_ConsolidationInvoiceId' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoices]'))
  begin CREATE NONCLUSTERED INDEX [IX_ARInvoice_ConsolidationInvoiceId] ON [dbo].[dw_ARInvoices] ([ConsolidationInvoiceId]) end


  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ARInvoice_IsConsolidationInvoice_Id' 
    AND object_id = OBJECT_ID('[dbo].[dw_ARInvoices]'))
  begin CREATE NONCLUSTERED INDEX [IX_ARInvoice_IsConsolidationInvoice_Id] ON [dbo].[dw_ARInvoices] ([IsConsolidationInvoice],[Id]) end

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


