alter table [Customs].[Declarations] add ReferentUserId varchar(15) null

alter table [Customs].[Declarations] add constraint [Declaration_User] foreign key ([ReferentUserId]) references [dbo].[Users]([Id]);