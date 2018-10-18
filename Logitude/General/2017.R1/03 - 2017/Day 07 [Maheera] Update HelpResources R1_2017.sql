--> Please run this script at Global db 

update HelpResources set IsNew = 0
go

update HelpResources 
set IsNew = 1, UpdateDate = GETDATE()
where Code = '7' or Code = '19' or Code = '22' or Code = '23' or Code = '17' or Code = '18' or code = '46' or  code ='REL007'
