

declare @Tenant as int
declare @ShipmentNumber as varchar(25)

set @Tenant = 211
set @ShipmentNumber = 'EX2718'

declare @ShipmentId as varchar(15)
declare @EntityId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @VendorId as varchar(15)
declare @AccountedAmount as float
declare @AccountedLocal as float
declare @AccountedProfit as float
declare @CurrencyId as varchar(15)
declare @LocalCurrencyCode as varchar(3)
declare @ProfitCurrencyCode as varchar(3)
declare @ParentPayableId as varchar(15)
declare @AmountType as varchar(5)

declare @MemoryTable table
(
  Id varchar(15),
  Charge varchar(100),
  Vendor varchar(100),
  Amount varchar(100) ,
  AmountLocal varchar(100) ,
  AmountProfit varchar(100),
  ParentPayableId varchar(15),
  AmountType varchar(5)
)

set @ShipmentId = (select Id from Shipments where Tenant = @Tenant and ShipmentNumber = @ShipmentNumber)

if (@ShipmentId is not null)
BEGIN
		set @LocalCurrencyCode = (select Code from Currencies where Tenant = @Tenant and Id = (Select CurrencyId from Tenants where Id = @Tenant))
		set @ProfitCurrencyCode = (select Code from Currencies where Tenant = @Tenant and Id = (select ProfitCurrencyId from Shipments where Tenant = @Tenant and Id = @ShipmentId))

		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, ChargesTypeId, VendorId, CurrencyId, AccountedAmount, AccountedAmountInLocalCurrency, AccountedAmountInProfitCurrency, ShipmentPayableAmountTypeCode, ShipmentPayableParentId
		FROM ShipmentPayables
		where Tenant = @Tenant and ShipmentId = @ShipmentId
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @ChargesTypeId, @VendorId,@CurrencyId, @AccountedAmount, @AccountedLocal, @AccountedProfit, @AmountType, @ParentPayableId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if (@AccountedAmount is null) set @AccountedAmount = 0
			if (@AccountedLocal is null) set @AccountedLocal = 0
			if (@AccountedProfit is null) set @AccountedProfit = 0

			insert into @MemoryTable
			values
			(
				@EntityId,
				(select EnglishName from ChargesTypes where Tenant = @Tenant and Id = @ChargesTypeId),
				(select EnglishName from Cards where Tenant = @Tenant and Id = @VendorId),
				convert(varchar,@AccountedAmount) + ' ' + (select Code from Currencies where Id = @CurrencyId and Tenant = @Tenant),
				convert(varchar,@AccountedLocal) + ' ' + @LocalCurrencyCode,
				convert(varchar,@AccountedProfit) + ' ' + @ProfitCurrencyCode,
				@ParentPayableId,
				@AmountType
			)

			FETCH NEXT FROM DataCursor INTO @EntityId, @ChargesTypeId,@VendorId,@CurrencyId, @AccountedAmount, @AccountedLocal, @AccountedProfit, @AmountType, @ParentPayableId
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor

		select * from @MemoryTable
END

select EntityPayableId, InvoiceCurrencyAmount, ForiegnCurrencyAmount, LocalCurrencyAmount, ProfitCurrencyAmount from APInvoiceLines where Tenant = @Tenant and EntityPayableId in (select Id from @MemoryTable)
go