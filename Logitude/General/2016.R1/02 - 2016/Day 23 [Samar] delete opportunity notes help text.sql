
update ObjectFields
set HelpTextCodeId = NULL where FieldName = 'Notes' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

delete from TextCodes where Code = 'Opportunity.NotesHelpText'
