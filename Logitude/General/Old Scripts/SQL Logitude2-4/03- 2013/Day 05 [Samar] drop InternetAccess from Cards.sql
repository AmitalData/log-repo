-- excute on Logitude2-4_Main

alter table cards drop constraint DF__Cards__InternetA__0DEF03D2
go

alter table Cards drop column InternetAccess
go

delete from ObjectFields where FieldName = 'InternetAccess' and ObjectTableId = (select Id from ObjectTables where Name = 'Card')

delete from TextCodes where code = 'Card.F.InternetAccess'
delete from TextCodes where code = 'Card.InternetAccessHelpText'
delete from TextCodes where code = 'Card.CH.InternetAccess'