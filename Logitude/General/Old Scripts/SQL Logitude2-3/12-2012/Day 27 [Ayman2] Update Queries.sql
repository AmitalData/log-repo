
--delete from AdvancedQueryFilters
--delete from QueryColumns
--delete from Queries

delete from TextCodes where Code = 'Shipment.Q.OperationalOpenHousesDirects'
delete from TextCodes where Code = 'Shipment.Q.OperationalOpenMastersDirects'
delete from TextCodes where Code = 'Shipment.Q.AccountingOpenHousesDirects'
delete from TextCodes where Code = 'Shipment.Q.AccountingOpenMastersDirects'

update TextCodes set DefaultText = 'Routing' where DefaultText = 'Rout' AND Code like '%.CH.%'

update TextCodes set DefaultText = 'Open Payables (Local Currency)' where Code = 'Shipment.F.OpenPayablesInLocalCurrency'
update TextCodes set DefaultText = 'Open Payables (Profit Currency)' where Code = 'Shipment.F.OpenPayablesInProfitCurrency'
update TextCodes set DefaultText = 'Accounted Payables (Local Currency)' where Code = 'Shipment.F.AccountedPayablesInLocalCurrency'
update TextCodes set DefaultText = 'Accounted Payables (Profit Currency)' where Code = 'Shipment.F.AccountedPayablesInProfitCurrency'
update TextCodes set DefaultText = 'Open Receivables (Local Currency)' where Code = 'Shipment.F.OpenReceivablesInLocalCurrency'
update TextCodes set DefaultText = 'Open Receivables (Profit Currency)' where Code = 'Shipment.F.OpenReceivablesInProfitCurrency'
update TextCodes set DefaultText = 'Accounted Receivables (Local Currency)' where Code = 'Shipment.F.AccountedReceivablesInLocalCurrency'
update TextCodes set DefaultText = 'Accounted Receivables (Profit Currency)' where Code = 'Shipment.F.AccountedReceivablesInProfitCurrency'
update TextCodes set DefaultText = 'Profit (Local Currency)' where Code = 'Shipment.F.ProfitInLocalCurrency'
update TextCodes set DefaultText = 'Profit (Profit Currency)' where Code = 'Shipment.F.ProfitInProfitCurrency'

update TextCodes set DefaultText = 'Open Payables' where Code like 'Shipment.CH.OpenPayablesInLocalCurrency%'
update TextCodes set DefaultText = 'Open Payables' where Code like 'Shipment.CH.OpenPayablesInProfitCurrency%'
update TextCodes set DefaultText = 'Accounted Payables' where Code like 'Shipment.CH.AccountedPayablesInLocalCurrency%'
update TextCodes set DefaultText = 'Accounted Payables' where Code like 'Shipment.CH.AccountedPayablesInProfitCurrency%'
update TextCodes set DefaultText = 'Open Receivables' where Code like 'Shipment.CH.OpenReceivablesInLocalCurrency%'
update TextCodes set DefaultText = 'Open Receivables' where Code like 'Shipment.CH.OpenReceivablesInProfitCurrency%'
update TextCodes set DefaultText = 'Accounted Receivables' where Code like 'Shipment.CH.AccountedReceivablesInLocalCurrency%'
update TextCodes set DefaultText = 'Accounted Receivables' where Code like 'Shipment.CH.AccountedReceivablesInProfitCurrency%'
update TextCodes set DefaultText = 'Profit' where Code like 'Shipment.CH.Profit%'
