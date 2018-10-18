

delete from ObjectTableRuleFields where ObjectFieldId = (Select Id from ObjectFields where FieldName = 'AWBPrintRate' AND ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment'))
delete from ObjectTableRuleFields where ObjectFieldId = (Select Id from ObjectFields where FieldName = 'AWBPrintRate' AND ObjectTableId = (Select Id from ObjectTables where Name = 'Master'))

delete from ObjectFields where FieldName = 'AWBPrintRate' 

delete from TextCodes where Code like '%AWBPrintRate%'

