update shipments set LastStatusLogDate = StatusDate
where LastStatusLogDate is null
 