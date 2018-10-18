

UPDATE dbo.ObjectTables SET Name='Customs.CustomsAddressType' WHERE Name='Customs.AddressType'

EXEC sp_rename '[Customs].[AddressTypes]', 'CustomsAddressTypes';



