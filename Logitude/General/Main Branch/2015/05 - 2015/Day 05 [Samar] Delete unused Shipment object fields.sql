delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Master'))

delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Master'))

delete from ObjectFields where FullNameTextCodeId = (select Id from TextCodes where Code = 'Shipment.F.PayablesInLocalCurrency')
delete from ObjectFields where FullNameTextCodeId = (select Id from TextCodes where Code = 'Master.F.PayablesInLocalCurrency')
delete from ObjectFields where FullNameTextCodeId = (select Id from TextCodes where Code = 'Master.CH.PayablesInLocalCurrencyListLable')

delete from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')
delete from ObjectFields where FieldName = 'PayablesInLocalCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'Master')

delete from TextCodes where Code = 'Shipment.F.PayablesInLocalCurrency'
delete from TextCodes where Code = 'Master.F.PayablesInLocalCurrency'
delete from TextCodes where Code = 'Master.CH.PayablesInLocalCurrencyListLable'