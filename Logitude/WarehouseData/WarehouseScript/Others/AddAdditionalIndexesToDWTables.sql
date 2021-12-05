
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
end




