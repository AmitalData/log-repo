update communicationlogs 
set entityreference=(select shipmentnumber from shipments where id=communicationlogs.entityid)