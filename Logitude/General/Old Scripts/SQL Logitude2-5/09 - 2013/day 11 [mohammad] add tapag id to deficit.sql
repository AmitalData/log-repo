

begin transaction
begin


alter table [Customs].Tapags drop constraint [Deficit_Tapag]

alter table [Customs].[Deficits] add TapagId varchar(15) null

alter table [Customs].[Deficits] add constraint [Deficit_Tapag] foreign key ([TapagId]) references [Customs].[Tapags]([Id]);

END
commit transaction
