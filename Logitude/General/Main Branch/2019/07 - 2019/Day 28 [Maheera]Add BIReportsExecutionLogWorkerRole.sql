INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('BIReportsExecutionLog'
           ,'BIReportsExecutionLogWorkerRole'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('BIReportsExecutionLog'
           ,0
           ,1)