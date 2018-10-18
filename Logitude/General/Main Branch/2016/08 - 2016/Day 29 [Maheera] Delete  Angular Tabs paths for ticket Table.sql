update ObjectTableTabs 
set HtmlComponentName = null , HtmlComponentUrl = null 
where ObjectTableId = (select id from objecttables where name ='ticket') and (code ='TIDI' or code = 'TIEV')
