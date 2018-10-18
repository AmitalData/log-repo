
BEGIN transaction
BEGIN

UPDATE dbo.ObjectTables SET Name='Customs.CustomsCountry' WHERE Name='Customs.Country'


EXEC sp_rename '[Customs].[Countries]', 'CustomsCountries';

delete FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsCountry%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsCountry')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.Country%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsCountry')

UPDATE dbo.TextCodes SET Code = replace(Code, 'Country', 'CustomsCountry') WHERE Code LIKE 'Customs.Country%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsCountry')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.Country%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsCountry')


END
COMMIT transaction

