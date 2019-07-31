


update ARInvoices
set IsPrinted = 1
where
IsPrinted = 0
AND PrintDate is not null
AND PrintByUserId is not null
AND
(
StatusCode = 'AD' OR StatusCode = 'VD' OR StatusCode = 'PD' OR StatusCode = 'PP' OR StatusCode = 'AR'
)