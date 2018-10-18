
begin transaction
begin

alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_DocumentIn] foreign key ([DocumentInId]) references [dbo].[DocumentIns]([Id]);
END
commit transaction