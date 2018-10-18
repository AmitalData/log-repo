 Update AnalyzeQueues
set SearchFields = 
isnull([Subject],'') + ',' + 
isnull([From],'') + ',' + 
isnull([Status],'') + ',' + 
isnull(EntityReference,'') + ',' + 
isnull(ObjectTableName,'') 


select id,searchfields from AnalyzeQueues 
	

