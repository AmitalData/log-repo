
begin transaction
begin

alter table [Customs].[CustomsDocuments] alter column DocumentTypeCode varchar(7) not null

END
commit transaction