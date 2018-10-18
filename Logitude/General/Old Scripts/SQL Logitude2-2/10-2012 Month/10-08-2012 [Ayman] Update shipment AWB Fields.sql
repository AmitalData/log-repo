
select AWBDeclaredValueForCarriage from Shipments where (AWBDeclaredValueForCarriage is null Or AWBDeclaredValueForCarriage = 'N V D')
select AWBDeclaredValueForCustoms from Shipments where (AWBDeclaredValueForCustoms is null Or AWBDeclaredValueForCustoms = 'N C V')
select AWBInsurrenceValue from Shipments where (AWBInsurrenceValue is null Or AWBInsurrenceValue = 'X X X')
select RateClassCode from Shipments where RateClassCode is null
select AWBPrintSpecificationCode from Shipments where AWBPrintSpecificationCode is null
select AWBChargeRate,ChargeableWeight,AWBChargeAmount from Shipments where AWBChargeAmount is null

Update Shipments set AWBDeclaredValueForCarriage = 'NVD' where (AWBDeclaredValueForCarriage is null Or AWBDeclaredValueForCarriage = 'N V D')
Update Shipments set AWBDeclaredValueForCustoms = 'NCV' where (AWBDeclaredValueForCustoms is null Or AWBDeclaredValueForCustoms = 'N C V')
Update Shipments set AWBInsurrenceValue = 'XXX' where (AWBInsurrenceValue is null Or AWBInsurrenceValue = 'X X X')
Update Shipments set RateClassCode = 'Q' where RateClassCode is null
Update Shipments set AWBPrintSpecificationCode = 'RAT' where AWBPrintSpecificationCode is null


Update Shipments set AWBChargeAmount = ROUND(AWBChargeRate,2) where (AWBChargeAmount is null AND RateClassCode = 'M')
Update Shipments set AWBChargeAmount = ROUND(ChargeableWeight * AWBChargeRate,2) where (AWBChargeAmount is null AND RateClassCode <> 'M')