--execute on global DB AFTER UPDATE

update HelpResources 
set CreateDate = '2015-12-13', UpdateDate = '2015-12-13'
where Code = '36'

update HelpResources 
set CreateDate = '2016-02-28', UpdateDate = '2016-02-28'
where Code = '37'
