

update TraceEvents set notes = null where EventTypeId in (select id from EventTypes where EnglishName like '%Update%')


--select *,(select name from objecttables where id=TraceEvents.ObjectTableId),(select EnglishName from EventTypes where id=TraceEvents.EventTypeId)
--,(select Code from EventTypes where id=TraceEvents.EventTypeId) from TraceEvents  where EventTypeId in (select id from EventTypes where EnglishName like '%Update%') and Notes <> null

--select * from EventTypes where EnglishName like '%Update%'