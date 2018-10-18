
begin transaction
begin

alter table [CustomerProducts] add [Revenue] decimal  null

alter table [CustomerProductLocations] add [Revenue] decimal  null

alter table [CustomerProductActualDatas] add [Revenue] decimal  null

alter table [CustomerProductLocationActualDatas] add [Revenue] decimal  null

alter table [OpportunityProducts] add [Revenue] decimal  null

END
commit transaction

ALTER TABLE [dbo].[QuoteTemplateSettings] drop DF__QuoteTemp__PageF__59DB2E46