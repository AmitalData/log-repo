delete from ObjectFields where ObjectTableId = (Select id from ObjectTables where name = 'TMProject') and FieldName = 'ProjectId'
delete from TextCodes where Code = 'TMProject.F.ProjectId' 
delete from TextCodes where Code = 'TMProject.ProjectIdHelpText' 

delete from ObjectFields where ObjectTableId = (Select id from ObjectTables where name = 'tmemployeetime') and FieldName = 'Projectid'
delete from TextCodes where Code = 'TMEmployeeTime.F.Projectid' 
delete from TextCodes where Code = 'TMEmployeeTime.ProjectidHelpText' 