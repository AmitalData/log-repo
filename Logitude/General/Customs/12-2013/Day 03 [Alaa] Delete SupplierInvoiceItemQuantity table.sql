delete from objectfields where ObjectTableId=(select id from objecttables where name='Customs.SupplierInvoiceItemsQuantity')
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId=(select id from objecttables where name='Customs.SupplierInvoiceItemsQuantity'))
delete from Features where ObjectTableId=(select id from objecttables where name='Customs.SupplierInvoiceItemsQuantity')
delete from TextCodes where ObjectTableId=(select id from objecttables where name='Customs.SupplierInvoiceItemsQuantity')
delete from MenusTables where ObjectTableId=(select id from objecttables where name='Customs.SupplierInvoiceItemsQuantity')
delete from ObjectFields where FieldName = 'SupplierInvoiceItemsQuantities'
delete from ObjectTables where name='Customs.SupplierInvoiceItemsQuantity'

