
-- Run this script after updating database migration
-- Field Name "Rank" was added

update QuoteStages set Rank = 1 where Code = 'QTDR'
update QuoteStages set Rank = 2 where Code = 'QTST'
update QuoteStages set Rank = 3 where Code = 'QTVW'
update QuoteStages set Rank = 4 where Code = 'QTID'


