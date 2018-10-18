delete from objectfields where ObjectTableId=(select id from objecttables where name='Customs.PaymentOrderOperationalStatus')
delete from TextCodes where ObjectTableId=(select id from objecttables where name='Customs.PaymentOrderOperationalStatus')
delete from MenusTables where ObjectTableId=(select id from objecttables where name='Customs.PaymentOrderOperationalStatus')
delete from ObjectTables where name='Customs.PaymentOrderOperationalStatus'