
USE [Logitude2-3_Main]
GO

Create PROCEDURE [dbo].[usp_UpdateCustomers] 
AS

	DECLARE @CurrentCustomerId AS varchar(15)
	DECLARE @CurrentCustomerTenant AS INT

	DECLARE CustomersCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant
	FROM Customers 
	OPEN CustomersCursor FETCH NEXT FROM CustomersCursor INTO @CurrentCustomerId,@CurrentCustomerTenant
	WHILE @@FETCH_STATUS = 0 
	BEGIN

			DECLARE @MaxDate as datetime
			DECLARE @MinDate as datetime
			set @MaxDate = (SELECT MAX(CreateDateTime) FROM Shipments WHERE CustomerId = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant)
			set @MinDate = (SELECT MIN(CreateDateTime) FROM Shipments WHERE CustomerId = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant)

			--print '[Min Date:' + Convert(varchar,ISNULL(@MinDate,0)) + ']'
			--print '[Max Date:' + Convert(varchar,ISNULL(@MaxDate,0)) + ']' 
		 
			 Update Customers
			 set StartWorkingDate = Convert(date,ISNULL(@MinDate,null)), LastShipmentDate = Convert(date,ISNULL(@MaxDate,null))
			 Where Id = @CurrentCustomerId AND Tenant = @CurrentCustomerTenant

			 update customers 
			 set RankId = (select id from Ranks where tenant = @CurrentCustomerTenant and Code ='1')
		 	 where id=@CurrentCustomerId and Tenant = @CurrentCustomerTenant and RankId is null

	FETCH NEXT FROM CustomersCursor INTO @CurrentCustomerId,@CurrentCustomerTenant
	END
	CLOSE CustomersCursor
	DEALLOCATE CustomersCursor
