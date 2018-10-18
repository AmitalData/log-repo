-- do not run online
update DocumentTypes set IsDocIn=0,IsCustomerView=0,IsAgentView=0 where code='paln'
update DocumentTypes set IsDocIn=0 where code='pali'
update DocumentTypes set IsDocIn=0,IsCustomerView=0 where code='740pp'
update DocumentTypes set IsDocIn=0,IsCustomerView=0,IsDirect=1 where code='714pp'
