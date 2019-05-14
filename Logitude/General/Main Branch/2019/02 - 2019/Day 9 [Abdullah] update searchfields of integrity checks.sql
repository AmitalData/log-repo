
-- USAGE: fill searchfields of accounting integrity checks by: [status name, status code]
update a set SearchFields = s.Name + ',' + s.Code
from AccountingIntegrityChecks a
join IntegrityCheckStatuses s on s.Code = a.StatusCode
