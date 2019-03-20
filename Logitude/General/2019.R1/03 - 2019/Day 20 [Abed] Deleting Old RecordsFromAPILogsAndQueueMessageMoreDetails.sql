
 --APILog

--If(OBJECT_ID('tempdb..#APILogstemp') Is  Null) Begin 
select *
 into #APILogstemp
from APILogs
where CreateDate > GETDATE() - 90 
--END

--If(OBJECT_ID('tempdb..#APILogsDatatemp') Is  Null) Begin 
 select *
 into #APILogsDatatemp
from APILogsData
where id in (select id from #APILogstemp ) 
--END



truncate table APILogsData
 ALTER TABLE  [dbo].[APILogsData] DROP CONSTRAINT [FK_dbo.APILogsData_dbo.APILogs_Id]
truncate table APILogs

INSERT INTO APILogs
    SELECT *  
    FROM #APILogstemp
 drop table #APILogstemp

INSERT INTO APILogsData
    SELECT *  
    FROM #APILogsDatatemp
    drop table #APILogsDatatemp

 



 ALTER TABLE [dbo].[APILogsData]   ADD  CONSTRAINT [FK_dbo.APILogsData_dbo.APILogs_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[APILogs] ([Id])

 --QueueMessageMoreDetails
 --If(OBJECT_ID('tempdb..#QueueMessageMoreDetailstemp') Is  Null) Begin 
 select *
 into #QueueMessageMoreDetailstemp
from QueueMessageMoreDetails
where CreateDateTime > GETDATE() - 90 
--end


truncate table QueueMessageMoreDetails
INSERT INTO QueueMessageMoreDetails
    SELECT *  
    FROM #QueueMessageMoreDetailstemp
 drop table #QueueMessageMoreDetailstemp

