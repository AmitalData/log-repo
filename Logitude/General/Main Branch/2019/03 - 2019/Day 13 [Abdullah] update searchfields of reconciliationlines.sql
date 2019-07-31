
-- USAGE: fill searchfields of reconciliation lines by: [ledgertransaction.searchfields]
update r set SearchFields = l.SearchFields
--select r.SearchFields,l.SearchFields ,*
from ReconciliationLines r
join ledgertransactions l on r.TransactionId = l.Id
