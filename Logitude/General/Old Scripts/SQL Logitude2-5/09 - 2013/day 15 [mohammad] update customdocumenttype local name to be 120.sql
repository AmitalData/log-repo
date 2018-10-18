
begin transaction
begin

alter table Customs.CustomDocumentTypes alter column LocalName nvarchar(120) null

END
commit transaction
