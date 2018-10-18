delete from ObjectFields where FieldName = 'AirlineCode' and ObjectTableId = (select Id from ObjectTables where name = 'AirlineMessagingRule')
delete from TextCodes where Code = 'AirlineMessagingRule.F.AirlineCode'
delete from TextCodes where Code = 'AirlineMessagingRule.AirlineCodeHelpText'
delete from TextCodes where Code = 'AirlineMessagingRule.CH.AirlineCodeListLable'