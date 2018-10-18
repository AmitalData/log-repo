ALTER TABLE [Customs].[CourierMasters] ADD  CONSTRAINT [UQ_Airline_MAWB_HAWB_Tenant] UNIQUE NONCLUSTERED
(
	[AirlineId] ASC,
	[MAWB] ASC,
	[HAWB] ASC,
	[Tenant] ASC
)