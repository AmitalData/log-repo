

IF (OBJECT_ID ('dw_ARInvoices', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [IX_ARInvoice_ConsolidationInvoiceId] ON [dbo].[dw_ARInvoices] ([ConsolidationInvoiceId])

CREATE NONCLUSTERED INDEX [IX_ARInvoice_IsConsolidationInvoice_Id] ON [dbo].[dw_ARInvoices] ([IsConsolidationInvoice],[Id])
end



IF (OBJECT_ID ('dw_Addresses', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [IX_dw_Addresses_AddressTypeId_CardId] ON[dbo].[dw_Addresses]([AddressTypeId],[CardId])
end


IF (OBJECT_ID ('dw_APInvoices', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [IX_APInvoice_Tenant_AutomaticLastUpdateDate] ON [dbo].[dw_APInvoices] ([Tenant],[AutomaticLastUpdateDate])
end


IF (OBJECT_ID ('dw_Partners', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [IX_dw_Partners_SalesmanUserId_AutomaticLastUpdateDate] ON[dbo].[dw_Partners]([SalesmanUserId],[AutomaticLastUpdateDate])
end



IF (OBJECT_ID ('dw_Shipments', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [IX_Shipment_IsCancelled_ShipmentLevelCode_RowNumber_Fact_Shipments] ON [dbo].[dw_Shipments] ([IsCancelled] , [ShipmentLevelCode],[RowNumber_Fact_Shipments])
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipmentLevelCode_AutomaticLastUpdateDate] ON [dbo].[dw_Shipments] ([ShipmentLevelCode],[AutomaticLastUpdateDate])

CREATE NONCLUSTERED INDEX [dw_Shipments_ShipmentLevelCode_AutomaticLastUpdateDate_InCludeSomeFields]
ON [dbo].[dw_Shipments] ([ShipmentLevelCode],[AutomaticLastUpdateDate])
INCLUDE ([ProfitCurrencyId],[ComputedStatusId],[FreelancerAddressId],[FreelancerContactId],[ReleasingAgentId],[ReleasingAgentAddressId],[ReleasingAgentContactId],[FromPortId],[ToPortId],[ShipmentPayableStatusCode],[ShipmentReceivableStatusCode],[BranchId],[IncotermId],[SalesmanUserId],[CreatedByUserId],[DepartmentId],[ShipmentTypeId],[CustomerId],[ShipperId],[ConsigneeId],[AgentComputed],[CustomAgentExportId],[CustomAgentImportId],[Notify1Id],[Notify2Id],[FreightForwarderId],[ShipperAddressId],[CustomerAddressId],[ConsigneeAddressId],[AgentAddressId],[CustomAgentExportAddressId],[CustomAgentImportAddressId],[Notify1AddressId],[Notify2AddressId],[FreightForwarderAddressId],[CustomerContactId],[ShipperContactId],[ConsigneeContactId],[AgentContactId],[CustomAgentExportContactId],[CustomAgentImportContactId],[Notify1ContactId],[Notify2ContactId],[FreightForwarderContactId],[DirectionId],[TransportModeId],[ShipperNotExporterContactId],[ConsigneeNotImporterContactId],[ConsigneeNotImporterAddressId],[ShipperNotExporterAddressId],[ConsigneeNotImporterId],[ShipperNotExporterId],[IssuingCarrierAgentId],[CustomClearancePointAddressId],[CustomClearancePointContactId],[ColoaderId],[ColoaderAddressId],[ColoaderContactId],[MoveTypeId],[SpecialServicesTypeId],[AccountManagerUserId],[ConsolidatorId],[ConsolidatorAddressId],[ConsolidatorContactId],[ValueOfGoodsCurrencyId],[MasterShipmentDataId],[WarehouseLegWarehouseId],[ShipmentSubTypeId],[PreForwardingTransportModeId],[PreForwardingFromPortId],[PreForwardingToPortId],[PreForwardingCarrierId],[OnForwardingTransportModeId],[OnForwardingCarrierId],[HandlerUserId],[Id],[Tenant])


end


