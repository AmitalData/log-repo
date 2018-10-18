
-- communicationLog searchFields
Update CommunicationLogs
set SearchFields = 
isnull(BCC,'') + ',' +
isnull(CC,'') + ',' +
isnull([From],'') + ',' +
isnull([To],'') + ',' +
isnull([EntityReference],'') + ',' +
isnull([Subject],'')

