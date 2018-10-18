
sp_rename 'cards.[CalssifierId]', 'ClassifierId', 'COLUMN'

delete from ObjectFields where FieldName = 'CalssifierId'
delete from TextCodes where code = 'Card.F.CalssifierId'
delete from TextCodes where code = 'Card.CalssifierIdHelpText'
delete from TextCodes where code = 'Customer.F.CalssifierId'
delete from TextCodes where code = 'Customer.CalssifierIdHelpText'


alter table [Cards] drop constraint [Card_CalssifierUser]
go

alter table [Cards] add constraint [Card_ClassifierUser] foreign key ([ClassifierId]) references [Users]([Id]);
go