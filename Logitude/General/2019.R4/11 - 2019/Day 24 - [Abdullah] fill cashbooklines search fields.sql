

-- Task 59601: slowness in opening the cheque cashbook screen - Cloud R4

update cbl 
set		cbl.searchfields = arpch.SearchFields + ',' + arpch.BankAccount
from CashBookLines cbl
	join CashBooks cb on cbl.CashBookId = cb.Id
	join ARPaymentCheques arpch on cbl.ARPChequeId = arpch.Id