update FFRStatus
set Name = 'Cancellation Resuest Rejected', SearchFields = 'CRR,Cancellation Resuest Rejected'
where Code = 'CRR'

update FFRStatus
set Name = 'Cancellation Resuest Sent', SearchFields = 'CRS,Cancellation Resuest Sent'
where Code = 'CRS'