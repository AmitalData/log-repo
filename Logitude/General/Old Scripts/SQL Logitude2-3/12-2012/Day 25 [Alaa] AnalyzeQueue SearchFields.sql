-- EXCUTE IN GLOBAL DATABASE

Update [Logitude2-3_Global].dbo.AnalyzeQueues
set SearchFields = 
 isnull(CAST(Tenant as varchar(15))  ,'') + ',' +
  isnull(CAST(CreateDate as varchar(40)) ,'') + ','+(select entityreference from [Logitude2-3_Main].dbo.communicationlogs where [Logitude2-3_Main].dbo.communicationlogs.id=communicationlogid)
  +','+(select name from [Logitude2-3_Main].dbo.ObjectTables where [Logitude2-3_Main].dbo.ObjectTables.Id=(select objecttableid from [Logitude2-3_Main].dbo.communicationlogs where [Logitude2-3_Main].dbo.communicationlogs.id=communicationlogid))
  + ',' + ISNULL (Status , '' ) + ','+
  isnull([Subject] ,'')+ ','+ 
  isnull([from] , '' ) +',' 


