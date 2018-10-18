select top 10 * from AnalyzeQueues
--EXECUTE on GLOBAL database

ALTER TABLE dbo.AnalyzeQueues ADD StackTrace VARCHAR(8000) NULL