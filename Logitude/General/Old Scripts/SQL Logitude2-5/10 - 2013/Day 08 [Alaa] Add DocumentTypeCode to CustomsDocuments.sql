begin transaction
begin

alter table [customs].[CustomsDocuments] add DocumentTypeCode varchar(7)

alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_CustomDocumentType] foreign key ([DocumentTypeCode]) references [Customs].[CustomDocumentTypes]([Code]);

end
commit transaction