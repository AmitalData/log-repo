
USE [Logitude2-2_Main]
GO

Create PROCEDURE [dbo].[usp_FixAutoCreditShipments] 
AS

	-- Receivable Fields
	DECLARE @CurrentTenant AS INT
	DECLARE @CurrentReceivableId AS varchar(15)
	DECLARE @CurrentShipmentId AS varchar(15)
	declare @TotalAmount as float
	declare @TotalAmountLocal as float
	declare @AmountInProfitCurrency as float

	declare @LastModified timestamp

	DECLARE ReceivablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,ShipmentId,TotalAmount,TotalAmountLocal,AmountInProfitCurrency
	FROM ShipmentReceivables
	WHERE UnitPrice < 0
	OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @CurrentReceivableId,@CurrentTenant,@CurrentShipmentId,@TotalAmount,@TotalAmountLocal,@AmountInProfitCurrency
	WHILE @@FETCH_STATUS = 0 
	BEGIN

		if(@TotalAmount > 0) set @TotalAmount = @TotalAmount * -1
		if(@TotalAmountLocal > 0) set @TotalAmountLocal = @TotalAmountLocal * -1
		if(@AmountInProfitCurrency > 0) set @AmountInProfitCurrency = @AmountInProfitCurrency * -1

		Update ShipmentReceivables 
		set
		TotalAmount = @TotalAmount,
		TotalAmountLocal = @TotalAmountLocal,
		AmountInProfitCurrency = @AmountInProfitCurrency		
		where Id = @CurrentReceivableId AND Tenant = @CurrentTenant

		EXECUTE usp_UpdateShipmentProfit @CurrentShipmentId,@LastModified OUTPUT

	FETCH NEXT FROM ReceivablesCursor INTO @CurrentReceivableId,@CurrentTenant,@CurrentShipmentId,@TotalAmount,@TotalAmountLocal,@AmountInProfitCurrency
	END
	CLOSE ReceivablesCursor
	DEALLOCATE ReceivablesCursor