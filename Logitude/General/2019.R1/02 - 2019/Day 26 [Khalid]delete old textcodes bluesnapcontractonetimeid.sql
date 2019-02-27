delete from ObjectFields where HelpTextCodeId in (Select Id  from TextCodes where Code like '%bluesnaponetimecontractid%')
delete  from TextCodes where Code like '%bluesnaponetimecontractid%'