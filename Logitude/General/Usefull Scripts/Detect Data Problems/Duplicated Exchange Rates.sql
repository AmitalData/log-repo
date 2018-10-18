
select Tenant, BaseCurrencyId, ForeignCurrencyId, ValueDate, LogDateTime, count(*)
from RatesTables
where Tenant = 973
group by Tenant, BaseCurrencyId, ForeignCurrencyId, ValueDate, LogDateTime
having count(*) > 1
