-- update IsMultiCurrency.GLAccount set false to single currency account

update GLAccounts set IsMultiCurrency = 0 where IsMultiCurrency is null

