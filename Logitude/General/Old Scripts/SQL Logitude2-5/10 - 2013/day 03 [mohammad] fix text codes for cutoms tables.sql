-- execute it only one time.


begin transaction
BEGIN

delete FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsAddressType%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsAddressType')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.AddressType%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsAddressType')

UPDATE dbo.TextCodes SET Code = replace(Code, 'AddressType', 'CustomsAddressType') WHERE Code LIKE 'Customs.AddressType%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsAddressType')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.AddressType%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsAddressType')


delete FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsBranch%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsBranch')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.Branch%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsBranch')

UPDATE dbo.TextCodes SET Code = replace(Code, 'Branch', 'CustomsBranch') WHERE Code LIKE 'Customs.Branch%'AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsBranch')

SELECT * FROM dbo.TextCodes  WHERE Code LIKE 'Customs.CustomsBranch%' AND ObjectTableId=(SELECT id FROM dbo.ObjectTables WHERE name='Customs.CustomsBranch')

END
commit transaction