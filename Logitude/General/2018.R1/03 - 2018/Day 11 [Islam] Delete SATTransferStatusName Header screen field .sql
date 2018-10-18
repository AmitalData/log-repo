select * from ScreenFields where ObjectFieldId in (select Id from ObjectFields where FieldName = 'SATTransferStatusName' and ObjectTableId in (select Id from ObjectTables where Name = 'arpayment' or Name = 'arinvoice'))
 delete from ScreenFields where ObjectFieldId in (select Id from ObjectFields where FieldName = 'SATTransferStatusName' and ObjectTableId in (select Id from ObjectTables where Name = 'arpayment' or Name = 'arinvoice'))

 update screens set NumberOfColumns = 5 where code = 'ARInvoice.HeaderScreen'
 update screens set NumberOfColumns = 4 where code = 'ARPayment.HeaderScreen'