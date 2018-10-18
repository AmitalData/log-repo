update ARInvoices set SATTransferStatusCode = 'TD' where SATXML is not null
update ARInvoices set SATTransferStatusCode = 'TE' where TransmissionError is not null and SATXML is null

update ARPayments set SATTransferStatusCode = 'TD' where SATXML is not null
update ARPayments set SATTransferStatusCode = 'TE' where TransmissionError is not null and SATXML is null