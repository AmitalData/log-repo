
delete from ObjectFields where HelpTextCodeId in (select id  from textcodes where code like '%IsChronologicalDatesHelpText%' )
delete  from textcodes where code like '%IsChronologicalDatesHelpText%' 
