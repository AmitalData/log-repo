



-- Run this Script then update Tenants to fix some default text lables

update TextCodes 
set IsSpellChecked = 0,
SpellCheckDate = NULL
where Code like '%SubTotalInInvoiceCurrency%' or Code like '%AmountInInvoiceCurrency%'

