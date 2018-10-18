
--	company									IATA				CASSCode	NEW IATA	NEW CASS	Tenant
--	Acumen Air Cargo Ltd.					91-475090000		NULL		9147509		0000		7
--	Globe Air Cargo							999-6938/0000		NULL		9996938		0000		31
--	Test Company							123212321			NULL		NULL					61
--	InterGlobal Forwarders INC				99-9-9231/0014		NULL		9999231		0014		130
--	Nordic Shipping Canada Limited			60192380010			NULL		6019238		0010		181
--	MGLine Trade & Logistic Solutions		37-4-7009			NULL		2747009					199
--	T S T (Transit Services Transport)		5447589/0011		NULL		5447589		0011		235
--	Noratra									5447750\0010		NULL		5447750		0010		248
--	peschaud - Deleted						544 7473			NULL		5447473					249
--	TTAM									54 47443			NULL		5447443					251
--	STATEMA									54 4 7006 \0013		NULL		5447006		0013		252

update Tenants set IATA = '9147509', CASSCode = '0000'		where Id = 7	AND IATA = '91-475090000'
update Tenants set IATA = '9996938', CASSCode = '0000'		where Id = 31	AND IATA = '999-6938/0000'
update Tenants set IATA = NULL,		 CASSCode = NULL		where Id = 61	AND IATA = '123212321'
update Tenants set IATA = '9999231', CASSCode = '0014'		where Id = 130	AND IATA = '99-9-9231/0014'
update Tenants set IATA = '6019238', CASSCode = '0010'		where Id = 181	AND IATA = '60192380010'
update Tenants set IATA = '3747009', CASSCode = NULL		where Id = 199	AND IATA = '37-4-7009'
update Tenants set IATA = '5447589', CASSCode = '0011'		where Id = 235	AND IATA = '5447589/0011'
update Tenants set IATA = '5447750', CASSCode = '0010'		where Id = 248	AND IATA = '5447750\0010'
update Tenants set IATA = '5447473', CASSCode = NULL		where Id = 249	AND IATA = '544 7473'
update Tenants set IATA = '5447443', CASSCode = NULL		where Id = 251	AND IATA = '54 47443'
update Tenants set IATA = '5447006', CASSCode = '0013'		where Id = 252	AND IATA = '54 4 7006 \0013'

alter table Tenants alter column IATA varchar(7) null
