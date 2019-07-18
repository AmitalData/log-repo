--Task 43690: Banks: Add new field CurrencyId.BankAccounts (DB + Screens)
UPDATE b
SET b.CurrencyId = g.CurrencyId
FROM BankAccounts b
JOIN GLAccounts g
    ON b.GLAccountId = g.Id 
WHERE b.CurrencyId is null