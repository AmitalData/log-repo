delete from ObjectFields where HelpTextCodeId in (select Id from TextCodes where Code = 'TasksScheduler.LastRunTimeHelpText')
delete from TextCodes where Code = 'TasksScheduler.LastRunTimeHelpText'

delete from ObjectFields where HelpTextCodeId in (select Id from TextCodes where Code = 'TasksScheduler.ServiceClassNameHelpText')
delete from TextCodes where Code = 'TasksScheduler.ServiceClassNameHelpText'