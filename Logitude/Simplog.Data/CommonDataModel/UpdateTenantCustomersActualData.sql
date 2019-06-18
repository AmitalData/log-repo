
IF OBJECT_ID('[dbo].[usp_UpdateTenantCustomersActualData]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData]
GO

Create PROCEDURE [dbo].[usp_UpdateTenantCustomersActualData]
(
	@Tenant int
)
AS

declare @StartDateTime as datetime
declare @EndDateTime as datetime

if @Tenant is not null
BEGIN 

declare @CustomerId varchar(15)
set @CustomerId = null

	if not exists (select * from CustomerActualDataHistory where Tenant = @Tenant and CONVERT(date,StartDateTime) = CONVERT(date,getdate()))
	begin
		set @StartDateTime = getdate()
		EXECUTE usp_UpdateCustomerActualData @CustomerId, @Tenant
		set @EndDateTime = getdate()

		insert into CustomerActualDataHistory(Tenant, StartDateTime, EndDateTime)
		values(@Tenant,	@StartDateTime,	@EndDateTime)
	end
END
