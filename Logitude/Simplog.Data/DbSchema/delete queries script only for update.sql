update queries set originalqueryid=NULL
delete from querycolumns where queryid in (select id from queries where tenant=0)
delete from Advancedqueryfilters where queryid in (select id from queries where tenant=0)
delete from queries where tenant=0