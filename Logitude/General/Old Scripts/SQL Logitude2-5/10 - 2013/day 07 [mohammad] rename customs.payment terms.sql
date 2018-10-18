BEGIN transaction
BEGIN

UPDATE dbo.ObjectTables SET Name='Customs.CustomsPaymentTerm' WHERE Name='Customs.PaymentTerm'


EXEC sp_rename '[Customs].[PaymentTerms]', 'CustomsPaymentTerms';

delete FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsPaymentTerm%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsPaymentTerm')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.PaymentTerm%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsPaymentTerm')

UPDATE dbo.TextCodes SET Code = replace(Code, 'PaymentTerm', 'CustomsPaymentTerm') WHERE Code LIKE 'Customs.PaymentTerm%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsPaymentTerm')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.PaymentTerm%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsPaymentTerm')


END
COMMIT transaction