

update shipments set ShipmentPayableStatusCode = 'NOPA'
where (isnull(OpenPayablesInLocalCurrency,0) = 0) AND (isnull(AccountedPayablesInLocalCurrency,0) = 0)
go

update shipments set ShipmentPayableStatusCode = 'CLSD'
where (isnull(OpenPayablesInLocalCurrency,0) = 0) AND (isnull(AccountedPayablesInLocalCurrency,0) != 0)
go

update shipments set ShipmentPayableStatusCode = 'OPEN'
where (isnull(OpenPayablesInLocalCurrency,0) != 0)
go

---
update shipments set ShipmentReceivableStatusCode = 'NORE'
where (isnull(OpenReceivablesInLocalCurrency,0) = 0) AND (isnull(AccountedReceivablesInLocalCurrency,0) = 0)
go

update shipments set ShipmentReceivableStatusCode = 'CLSD'
where (isnull(OpenReceivablesInLocalCurrency,0) = 0) AND (isnull(AccountedReceivablesInLocalCurrency,0) != 0)
go

update shipments set ShipmentReceivableStatusCode = 'OPEN'
where (isnull(OpenReceivablesInLocalCurrency,0) != 0)
go

