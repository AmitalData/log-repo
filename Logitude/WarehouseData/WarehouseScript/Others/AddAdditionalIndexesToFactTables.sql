

IF (OBJECT_ID ('Fact_Charges', 'U')  IS NOT NULL)
BEGIN
CREATE NONCLUSTERED INDEX [Fact_Charges_ProfitIndex]
ON [dbo].[Fact_Charges] ([Source Tenant],[Shipment Accounting Closed],[DirectHouse])
INCLUDE ([Shipment Id],[Shipment Number],[Customer],[Account Manager],[Status],[Shipment Create Date],[Charges Type],[Accounted Receivables in Profit],[Open Payables in Profit],[Main Carriage ATD],[Main Carriage ATA])
END





