alter table [Customs].[Declarations] Add TransportModeId char(1) null

alter table [Customs].[Declarations] add constraint [Declaration_TransportMode] foreign key ([TransportModeId]) references [dbo].[TransportModes]([Id]);