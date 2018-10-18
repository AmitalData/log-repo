/**
	This script update ticket's first resolve date of old Tickets
	It should be run one time only
**/

update Tickets 
set FirstResolveDate = FullResolvedTime
