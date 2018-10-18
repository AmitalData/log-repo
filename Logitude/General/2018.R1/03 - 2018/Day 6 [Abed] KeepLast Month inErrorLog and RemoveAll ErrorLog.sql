

select *
 into #ErrorLogstemp
from ErrorLogs
where LogDate >=DATEADD(Month, -1, getdate()) order by LogDate

truncate table ErrorLogs

INSERT INTO ErrorLogs
    SELECT *  
    FROM #ErrorLogstemp

 drop table #ErrorLogstemp


