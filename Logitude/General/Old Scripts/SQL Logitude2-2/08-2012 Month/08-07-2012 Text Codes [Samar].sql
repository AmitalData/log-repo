
--Shipment Profit
update TextCodes set Code = 'Shipment.S.Profit.ProfitDetails', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.ProfitDetails'
update TextCodes set Code = 'Shipment.S.Profit.ChargeType', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.ChargeType'
update TextCodes set Code = 'Shipment.S.Profit.Receivables', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.Receivables'
update TextCodes set Code = 'Shipment.S.Profit.Payables', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.Payables'
update TextCodes set Code = 'Shipment.S.Profit.Profit', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.Profit'
update TextCodes set Code = 'Shipment.S.Profit.EstimateProfit', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.EstimateProfit'
update TextCodes set Code = 'Shipment.S.Profit.Difference', TextCodeTypeCode = 'S' where Code = 'Shipment.O.Profit.Difference'

delete from TextCodes where Code = 'Shipment.O.Profit.ProfitLocal'


--Shipment Airway Bill

update TextCodes set Code = 'Shipment.S.AWB.EditShipmentAWB', TextCodeTypeCode = 'S' where Code = 'Shipment.O.AWB.EditShipmentAWB'
update TextCodes set Code = 'Shipment.S.AWB.AddShipmentAWB', TextCodeTypeCode = 'S' where Code = 'Shipment.O.AWB.AddShipmentAWB'
update TextCodes set Code = 'Shipment.S.AWB.Name', TextCodeTypeCode = 'S' where Code = 'Shipment.O.AWB.Name'
update TextCodes set Code = 'Shipment.S.AWB.Type', TextCodeTypeCode = 'S' where Code = 'Shipment.O.AWB.Type'

delete from TextCodes where Code = 'Shipment.O.AWB.ExRate'


