
declare @AirlineId as varchar(15)

--1
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Air Greenland', 'Air Greenland', 'AL', 'GL', GETDATE(), 'Greenland', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '631' , 'GRL', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--2
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Globus', 'Globus', 'AL', 'GH', GETDATE(), 'Russia', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '674' , 'GLP', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--3
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Aklak Air', 'Aklak Air', 'AL', '6L', GETDATE(), 'Canada', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '709' , 'AKK', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--4
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Fars Air Qeshm', 'Fars Air Qeshm', 'AL', 'QE', GETDATE(), 'Iran', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '721' , 'QFZ', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--5
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'LASER Airlines', 'LASER Airlines', 'AL', 'QL', GETDATE(), 'Venezuela', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '722' , 'LER', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--6
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Air Rarotonga', 'Air Rarotonga', 'AL', 'GZ', GETDATE(), 'Cook Islands', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '755' , null, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--7
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Sky Work Airlines', 'Sky Work Airlines', 'AL', 'SX', GETDATE(), 'Switzerland', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '772' , 'SRK', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--8
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Israir', 'Israir', 'AL', '6H', GETDATE(), 'Israel', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '818' , 'ISR', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--9
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Khors Aircompany', 'Khors Aircompany', 'AL', 'KO', GETDATE(), 'Ukraine', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '852' , 'KHO', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--10
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Tranwest Air', 'Tranwest Air', 'AL', '9T', GETDATE(), 'Canada', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '909' , 'ABS', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--11
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Air Uganda', 'Air Uganda', 'AL', 'U7', GETDATE(), 'Uganda', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '926' , 'UGB', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--12
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Edelweiss Air', 'Edelweiss Air', 'AL', 'WK', GETDATE(), 'Switzerland', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '945' , 'EDW', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--13
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Vensecar International', 'Vensecar International', 'AL', 'K1', GETDATE(), 'Venezuela', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '946' , 'VEC', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--14
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'PAL Airlines', 'PAL Airlines', 'AL', 'PB', GETDATE(), 'Canada', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '967' , 'PVL', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)

--15
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Hawkair Aviation Services', 'Hawkair Aviation Services', 'AL', 'BH', GETDATE(), 'Canada', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '993' , 'BHA', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
