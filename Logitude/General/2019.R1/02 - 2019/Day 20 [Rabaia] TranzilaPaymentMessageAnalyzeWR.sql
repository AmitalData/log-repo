--> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('TranzilaPaymentMessageAnalyze'
           ,'TranzilaPaymentMessageAnalyzeWR'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('TranzilaPaymentMessageAnalyze'
           ,0
           ,1)
