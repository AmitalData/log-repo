-- excute on main DB

update DocumentTypes 
set IsCustomerView = 1
where code = 'VU' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = 'NNA' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = 'NAO' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = 'LCLL' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = 'FU' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = 'COO' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1, IsAgentView = 1
where code = 'CMR' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = 'BDC' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = 'BCO' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1, IsAgentView = 1
where code = 'ARP' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = 'ARNT' and tenant = 0

update DocumentTypes 
set IsAgentView = 1
where code = 'APP' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1, IsAgentView = 1
where code = '999S' and tenant = 0

update DocumentTypes 
set IsAgentView = 1
where code = '999M' and tenant = 0

update DocumentTypes 
set IsAgentView = 1
where code = '785O' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '785A' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '740L' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '740HL' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '740' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '721' and tenant = 0

update DocumentTypes 
set IsAgentView = 1, IsCustomerView = 1
where code = '716SD' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = '716' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = '714' and tenant = 0

update DocumentTypes 
set IsCustomerView = 1
where code = '380' and tenant = 0