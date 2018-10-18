

update TraceEvents set IsAddedManually=1 where EventTypeId in (select id from EventTypes where IsManualEntry=1)

