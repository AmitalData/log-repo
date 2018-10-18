
delete from ObjectFields where FieldName = 'PaymentBlockDate'
delete from TextCodes where code like '%PaymentBlockDate%'

--EXECUTE migration on Simplog.Global.Data