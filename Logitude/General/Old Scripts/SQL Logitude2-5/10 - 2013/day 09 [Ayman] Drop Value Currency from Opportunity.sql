
-- Run this script



BEGIN transaction
BEGIN

delete from Screenfields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EstimatedRevenueAndCurrency')

delete from ObjectFields where FieldName = 'EstimatedRevenueAndCurrency'

delete from TextCodes where Code like '%EstimatedRevenueAndCurrency%'

END
COMMIT transaction