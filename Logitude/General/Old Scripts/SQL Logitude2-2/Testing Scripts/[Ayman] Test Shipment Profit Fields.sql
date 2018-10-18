
-- select Tenant,ShipmentNumber from Shipments where ProfitCurrencyId is null
-- Update Shipments set ProfitCurrencyId = (select ProfitCurrencyId from Tenants where Id = Tenant) where ProfitCurrencyId is null

--select SUM(OpenAmountInProfitCurrency),SUM(OpenAmountInLocalCurrency) from ShipmentPayables where ShipmentId = '1-648'



-- Test Local Currency Fields
select
Id,Tenant,ShipmentNumber,
'' as ______,

OpenPayablesInLocalCurrency as Open_P_Local,
AccountedPayablesInLocalCurrency as ACCT_P_Local,
(OpenPayablesInLocalCurrency + AccountedPayablesInLocalCurrency) as Total_P_Local,
'' as ______,

OpenReceivablesInLocalCurrency as Open_R_Local,
AccountedReceivablesInLocalCurrency as ACCT_R_Local,
(OpenReceivablesInLocalCurrency + AccountedReceivablesInLocalCurrency) as Total_R_Local,
'' as ______,

(OpenReceivablesInLocalCurrency + AccountedReceivablesInLocalCurrency) - (OpenPayablesInLocalCurrency + AccountedPayablesInLocalCurrency) as ComputedProfit_Local,
ProfitInLocalCurrency as ProfitInLocal

from Shipments 
where
 (OpenReceivablesInLocalCurrency + AccountedReceivablesInLocalCurrency) != 0
and ROUND(OpenReceivablesInLocalCurrency + AccountedReceivablesInLocalCurrency - OpenPayablesInLocalCurrency - AccountedPayablesInLocalCurrency,2) != ROUND(ProfitInLocalCurrency,2)



-- Test Profit Currency Fields
select
Id,Tenant,ShipmentNumber,
'' as ______,

OpenPayablesInProfitCurrency as Open_P,
AccountedPayablesInProfitCurrency as ACCT_P,
(OpenPayablesInProfitCurrency + AccountedPayablesInProfitCurrency) as Total_P,
'' as ______,

OpenReceivablesInProfitCurrency as Open_R,
AccountedReceivablesInProfitCurrency as ACCT_R,
(OpenReceivablesInProfitCurrency + AccountedReceivablesInProfitCurrency) as Total_R,
'' as ______,

(OpenReceivablesInProfitCurrency + AccountedReceivablesInProfitCurrency) - (OpenPayablesInProfitCurrency + AccountedPayablesInProfitCurrency) as ComputedProfit,
ProfitInProfitCurrency as ProfitInProfit

from Shipments 
where (OpenReceivablesInProfitCurrency + AccountedReceivablesInProfitCurrency) <> 0
and ROUND(OpenReceivablesInProfitCurrency + AccountedReceivablesInProfitCurrency - OpenPayablesInProfitCurrency - AccountedPayablesInProfitCurrency,2) <> ROUND(ProfitInProfitCurrency,2)
