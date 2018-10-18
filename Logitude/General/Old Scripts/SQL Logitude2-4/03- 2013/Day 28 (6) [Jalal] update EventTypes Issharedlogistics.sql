update EventTypes
set issharedlogisticsenabled = 1
where IsCustomerView =1 or IsAgentView = 1

select * from EventTypes
where IsCustomerView =1 or IsAgentView = 1