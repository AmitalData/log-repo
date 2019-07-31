CREATE NONCLUSTERED INDEX [IX_ObjectTableId_EntityId_Tenant] ON [dbo].[TraceEvents]
(
	[ObjectTableId] ASC,
	[Tenant] ASC,
	[EntityId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]