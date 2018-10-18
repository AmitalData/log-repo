--delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where  FieldName='AgentFileReferenceNumber') 
--GO

--delete from ObjectFields where FieldName='AgentFileReferenceNumber'
--go
--delete from TextCodes where Code='Customs.Declaration.F.AgentFileReferenceNumber'
--go
--delete from TextCodes where code='Customs.Declaration.AgentFileReferenceNumberHelpText'
--go
--delete from TextCodes where code='Customs.Declaration.CH.AgentFileReferenceNumberListLable'
--go


--alter table [Customs].[Declarations] drop column AgentFileReferenceNumber