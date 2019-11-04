INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('BluesnapPageAnalyzer'
           ,'BluesnapPageAnalyzerWorkerRole'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('BluesnapPageAnalyzer'
           ,0
           ,1)







		   