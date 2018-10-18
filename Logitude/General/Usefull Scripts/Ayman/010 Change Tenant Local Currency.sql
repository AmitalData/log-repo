
declare @Tenant as int
declare @NewLocalCurrencyCode as varchar(30)

-- Insert your variables (NIS:USD:EUR)
set @Tenant = 99999
set @NewLocalCurrencyCode = '99999'

declare @LocalCurrencyId as varchar(15)
set @LocalCurrencyId = (select Id from Currencies where Tenant = @Tenant and Code = @NewLocalCurrencyCode)

if not exists (select * from Tenants where Id = @Tenant)
begin
       print 'Error 01: This Tenant is not exists!'
end

else if not exists (select * from Currencies where Tenant = @Tenant and Id = @LocalCurrencyId)
begin
       print 'Error 02: This Currency is not exists!'
end

else
begin
	update Tenants set CurrencyId = @LocalCurrencyId where Id = @Tenant and CurrencyId <> @LocalCurrencyId
end