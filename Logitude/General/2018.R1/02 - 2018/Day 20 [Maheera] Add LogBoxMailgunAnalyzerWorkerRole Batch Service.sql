--> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('LogBoxMailgunAnalyzerWorkerRole'
           ,'LogBoxMailgunAnalyzerWorkerRole'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('LogBoxMailgunAnalyzerWorkerRole'
           ,0
           ,1)



