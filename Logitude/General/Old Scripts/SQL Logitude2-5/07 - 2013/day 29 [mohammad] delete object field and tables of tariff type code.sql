
delete from objectfields where ObjectTableId=(select id from objecttables where name = 'Customs.TariffCodeType')
delete from TextCodes where code like '%TariffCodeType%'
delete from MenusTables where ObjectTableId = (select id from ObjectTables where name='Customs.TariffCodeType')
delete from ObjectTables where name='Customs.TariffCodeType'