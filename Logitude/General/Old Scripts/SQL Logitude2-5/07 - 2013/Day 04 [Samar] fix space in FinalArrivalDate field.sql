-- do not run online
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'FinalArrivalDate')
delete from ObjectFields where FieldName = 'FinalArrivalDate'
delete from TextCodes where code = 'shipment.f.FinalArrivalDate'
delete from TextCodes where code = 'Shipment.CH.FinalArrivalDateListLable'
delete from TextCodes where code = 'Shipment.FinalArrivalDateHelpText'