

IF (OBJECT_ID ('Fact_Charges', 'U')  IS NOT NULL)
BEGIN

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'Fact_Charges_ProfitIndex' AND object_id = OBJECT_ID('Fact_Charges')) 

begin 
       CREATE NONCLUSTERED INDEX [Fact_Charges_ProfitIndex]
        ON [dbo].[Fact_Charges] ([Source Tenant],[Shipment Accounting Closed],[DirectHouse])
        INCLUDE ([Shipment Id],[Shipment Number],[Customer],[Account Manager],[Status],[Shipment Create Date],[Charges Type],[Accounted Receivables in Profit],[Open Payables in Profit],[Main Carriage ATD],[Main Carriage ATA])
end

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Fact_Charges_Volumetric_Weight_UnitSelection' AND object_id = OBJECT_ID('Fact_Charges')) 

begin 

CREATE NONCLUSTERED INDEX [IX_Fact_Charges_Volumetric_Weight_UnitSelection] 
ON [dbo].[Fact_Charges] ([Volumetric Weight])
END

END



IF (OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL)
BEGIN
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Fact_Shipments_Volumetric_Weight_UnitSelection' AND object_id = OBJECT_ID('Fact_Shipments')) 
BEGIN
CREATE NONCLUSTERED INDEX [IX_Fact_Shipments_Volumetric_Weight_UnitSelection] 
ON [dbo].[Fact_Shipments] ([Volumetric Weight])
END
END


