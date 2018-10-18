delete from AdvancedQueryFilters where ObjectFieldId in (
	   select id  from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'ticket') 
		and 
		(
		FieldName = 'ClassificationId' or FieldName = 'ClassificationName'
		or FieldName = 'CreatedByUserId'
		or FieldName = 'ResolveTime'
		or FieldName = 'MyAllTickets'
		or FieldName = 'FirstResponseColor'
		or FieldName = 'ResolveWithinColor'
		or FieldName = 'FirstResponseExclamationmark'
		or FieldName = 'ResolveExclamationmark'
		or FieldName = 'FirstResponseDueColor'
		or FieldName = 'ResolveDueColor'
		)
)

delete from QueryColumns where ObjectFieldId in (
	   select id  from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'ticket') 
		and 
		(
		FieldName = 'ClassificationId' or FieldName = 'ClassificationName'
		or FieldName = 'CreatedByUserId'
		or FieldName = 'ResolveTime'
		or FieldName = 'MyAllTickets'
		or FieldName = 'FirstResponseColor'
		or FieldName = 'ResolveWithinColor'
		or FieldName = 'FirstResponseExclamationmark'
		or FieldName = 'ResolveExclamationmark'
		or FieldName = 'FirstResponseDueColor'
		or FieldName = 'ResolveDueColor'
		)
)

delete  from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'ticket') 
		and 
		(
		FieldName = 'ClassificationId' or FieldName = 'ClassificationName'
		or FieldName = 'CreatedByUserId'
		or FieldName = 'ResolveTime'
		or FieldName = 'MyAllTickets'
		or FieldName = 'FirstResponseColor'
		or FieldName = 'ResolveWithinColor'
		or FieldName = 'FirstResponseExclamationmark'
		or FieldName = 'ResolveExclamationmark'
		or FieldName = 'FirstResponseDueColor'
		or FieldName = 'ResolveDueColor'
		)