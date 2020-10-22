
--FactCharges
CREATE NONCLUSTERED INDEX [IX_FactCharges_SourceTenant_ShipmentCreateDate]
ON [dbo].[Fact_Charges] ([Source Tenant],[Shipment Create Date])

CREATE NONCLUSTERED INDEX [IX_FactCharges_SourceTenant_ShipmentCreateDateTime]
ON [dbo].[Fact_Charges] ([Source Tenant],[Shipment Create Date Time])


--FactShipments
CREATE NONCLUSTERED INDEX [IX_FactShipments_SourceTenant_CreateDate]
ON [dbo].[Fact_Shipments] ([Source Tenant],[Create Date])


CREATE NONCLUSTERED INDEX [IX_FactShipments_SourceTenant_CreateDateTime]
ON [dbo].[Fact_Shipments] ([Source Tenant],[Create Date Time])

