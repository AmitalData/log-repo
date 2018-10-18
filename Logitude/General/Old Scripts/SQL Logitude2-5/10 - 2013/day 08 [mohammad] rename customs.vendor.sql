BEGIN transaction
BEGIN

UPDATE dbo.ObjectTables SET Name='Customs.CustomsVendor' WHERE Name='Customs.Vendor'


EXEC sp_rename '[Customs].[Vendors]', 'CustomsVendors';

delete FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsVendor%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsVendor')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.Vendor%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsVendor')

UPDATE dbo.TextCodes SET Code = replace(Code, 'Vendor.', 'CustomsVendor.') WHERE Code LIKE 'Customs.Vendor.%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsVendor')


SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.Vendor%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsVendor')

UPDATE dbo.DBIdCounters SET TableName='Customs.CustomsVendor' WHERE TableName='Customs.Vendor'

END
COMMIT transaction