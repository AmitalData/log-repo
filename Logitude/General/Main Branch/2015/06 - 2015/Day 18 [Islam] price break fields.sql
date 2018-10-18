
update ObjectFields set DisplayInEntityVariables = 0 where (FieldName = 'SaleUnitPrice' or FieldName = 'CostUnitPrice') and ObjectTableId = (select  id from ObjectTables where name = 'QuotePriceSteps')

