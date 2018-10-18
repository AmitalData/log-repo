
--declare @Tenant as int
--declare @TenantName as varchar(100)
--declare @LocalCurrencyId as varchar(15)
--declare @ProfitCurrencyId as varchar(15) 
--declare @LocalCurrencyCode as varchar(5)
--declare @ProfitCurrencyCode as varchar(5)
--declare @ShipmentId as varchar(15)
--declare @InvoiceId as varchar(15)
--declare @InvoiceDate as date
--set @Tenant = 1326

--select
--@TenantName = Company,
--@LocalCurrencyId = CurrencyId,
--@ProfitCurrencyId = ProfitCurrencyId
--from Tenants where Id = @Tenant

--select @LocalCurrencyCode = Code from Currencies where Id = @LocalCurrencyId 
--select @ProfitCurrencyCode = Code from Currencies where Id = @ProfitCurrencyId 

--print 'Tenant: ' + @TenantName
--print 'Local Currency: ' + @LocalCurrencyCode
--print 'Profit Currency: ' + @ProfitCurrencyCode



--DECLARE ShipmentsCursor CURSOR READ_ONLY
--FOR
--SELECT Id
--FROM Shipments
--WHERE Tenant = @Tenant
--AND ShipmentNumber in ('IA1065', 'IA1067')
--OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId
--WHILE @@FETCH_STATUS = 0
--	BEGIN

--		-- ARInvoices Cursor
--		DECLARE ARInvoicesCursor CURSOR READ_ONLY
--		FOR
--		SELECT ARInvoiceId
--		FROM ARInvoiceEntities
--		WHERE Tenant = @Tenant
--		AND EntityId = @ShipmentId
--		OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @InvoiceId
--		WHILE @@FETCH_STATUS = 0
--			BEGIN

			
--			FETCH NEXT FROM ARInvoicesCursor INTO @InvoiceId
--			END
--		CLOSE ARInvoicesCursor
--		DEALLOCATE ARInvoicesCursor



--	FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId
--	END
--CLOSE ShipmentsCursor
--DEALLOCATE ShipmentsCursor



--select * from Shipments where ShipmentNumber = 'IA1065' and Tenant = 1326
--select * from ARInvoiceEntities where EntityId = '1-918447'
--select * from APInvoiceEntities where EntityId = '1-918447'
--select * from Currencies where Tenant = 1326

--select Id, Tenant, InvoiceNumber, InvoiceDate,
--InvoiceCurrencyId, ProfitCurrencyId,
--InvoiceCurrencyExchangeRate, ProfitCurrencyExchangeRate,
--AmountInInvoiceCurrency, AmountInProfitCurrency,
--AmountDue, AmountDueInProfitCurrency
--from ARInvoices where Id  = '1-292963'

--select * from Currencies where Tenant = 1326
--select Id, Tenant, InvoiceNumber, InvoiceDate,
--InvoiceCurrencyId, ProfitCurrencyId,
--InvoiceCurrencyExchangeRate, ProfitCurrencyExchangeRate,
--AmountInInvoiceCurrency, AmountInProfitCurrency,
--AmountDue, AmountDueInProfitCurrency
--from APInvoices where Id  = '1-270018'

--declare @Rate as float
--set @Rate = dbo.GetLastRate(1326,'2017-12-01 00:00:00.000','1-6175','1-6178')
--print convert(varchar,@Rate)

--declare @Rate as float
--set @Rate = dbo.GetLastRate(1326,'2017-11-30 00:00:00.000','1-6175','1-6178')
--print convert(varchar,@Rate)



---


-- 1-4756     EUR
-- 1-4758     USD
-- 1-4753     HRK
-- 7.40819

--update APInvoices set ProfitCurrencyExchangeRate = 7.40819 where Tenant = 1050 and id in ('1-218042','1-218043')
--update APInvoiceLines set ProfitCurrencyAmount = round(LocalCurrencyAmount / 7.40819,2) where Tenant = 1050 and APInvoiceId in ('1-218042','1-218043')
--update APInvoiceTotalVATs set ProfitVatableAmount = 30.01, ProfitCurrencyVATAmount = 7.5 where id = '1-272347'
--update APInvoiceTotalVATs set ProfitVatableAmount = 1163.56, ProfitCurrencyVATAmount = 0 where id = '1-272348'

--update APInvoices
--set
--AmountInProfitCurrency = round(AmountInLocalCurrency / 7.40819,2),
--AmountDueInProfitCurrency = round(AmountDueInLocalCurrency  / 7.40819,2)
--where Id in ('1-218042','1-218043')


--select * from APInvoiceTotalVATs where APInvoiceId in ('1-218042','1-218043')

--select Id, Tenant, InvoiceNumber, InvoiceCurrencyId, ProfitCurrencyId, ProfitCurrencyExchangeRate, InvoiceCurrencyExchangeRate,
--AmountInInvoiceCurrency, AmountInLocalCurrency, AmountInProfitCurrency
--from APInvoices where Tenant = 1050 and Id in ('1-218042','1-218043')


--update ShipmentPayables set AccountedAmountInProfitCurrency = 30.01 where Id = '1-1152178'
--update ShipmentPayables set AccountedAmountInProfitCurrency = 1009.22 where Id = '1-1152180'
--update ShipmentPayables set AccountedAmountInProfitCurrency = 14.94 where Id = '1-1152181'
--update ShipmentPayables set AccountedAmountInProfitCurrency = 139.41 where Id = '1-1152182'

--select APInvoiceId, LineNumber, LocalCurrencyAmount, Description, EntityPayableId, ForiegnExchangeRate, ForiegnCurrencyAmount, LocalCurrencyAmount, InvoiceCurrencyAmount, ProfitCurrencyAmount
--from APInvoiceLines 
--where APInvoiceId in ('1-218042','1-218043')


--select Id, Tenant, Rate, ProfitCurrencyExchangeRate, ExpectedAmount, AccountedAmount, AccountedAmountInLocalCurrency, AccountedAmountInProfitCurrency
--from ShipmentPayables where Id in (select EntityPayableId from APInvoiceLines where APInvoiceId in ('1-218042','1-218043'))


--select APInvoiceId, sum(LocalCurrencyAmount) as local, sum(InvoiceCurrencyAmount) as invoice, sum(ProfitCurrencyAmount) as profit
--from APInvoiceLines 
--where APInvoiceId in ('1-218042','1-218043')
--group by APInvoiceId



--select Id, InvoiceNumber, SubTotalInLocalCurrency, SubTotalInInvoiceCurrency
--AmountInInvoiceCurrency, AmountInLocalCurrency, AmountInProfitCurrency
--from APInvoices where Tenant = 1050 and Id in ('1-218042','1-218043')





