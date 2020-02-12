
-- USAGE: move Id from ActionCode to ActionId
update	JournalLines 
set		ActionId = ActionCode
where	ActionCode like '%-%'

-- fill ActionCode from ActionId
update	jl
set		jl.ActionCode = t.Code
from	JournalLines jl join JournalActionTypes t on jl.ActionId = t.Id and jl.Tenant = t.Tenant
where	ActionCode like '%-%'


-- fill ActionId from ActionCode
update	jl
set		jl.ActionId = t.Id
from	JournalLines jl join JournalActionTypes t on jl.ActionCode = t.Code and jl.Tenant = t.Tenant
where	ActionCode not like '%-%'

-- fill ActionCode from ActionId
update	jl
set		jl.ActionCode = t.Code
from	JournalLines jl join JournalActionTypes t on jl.ActionId = t.Id and jl.Tenant = t.Tenant
where	ActionCode is null and actionid is not null
