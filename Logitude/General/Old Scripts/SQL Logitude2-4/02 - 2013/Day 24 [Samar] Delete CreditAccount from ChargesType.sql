
--excute on Logitude2-4_Main

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CreditAccount' and ObjectTableId = (select Id from ObjectTables where name = 'ChargesType') )
delete from ObjectFields where FieldName = 'CreditAccount' and ObjectTableId = (select Id from ObjectTables where name = 'ChargesType') 

delete from TextCodes where Code = 'ChargesType.F.CreditAccount'
delete from TextCodes where Code = 'ChargesType.CreditAccountHelpText'
delete from TextCodes where Code = 'ChargesType.CH.CreditAccountListLable'