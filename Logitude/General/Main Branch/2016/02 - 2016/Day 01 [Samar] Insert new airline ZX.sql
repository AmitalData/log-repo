
declare @AirlineId as varchar(15)

--1
EXECUTE usp_GetNextTableIdValue @AirlineId OUTPUT,'Card'
insert into Cards(Id, Tenant, EnglishName, LocalName, PartnerTypeId, Code, CreateDate, CountryName, IsCustomer, InActive, EnableConsolidationInvoices, IsActiveForMobile)
values(@AirlineId, 0, 'Central Mountain Air', 'Central Mountain Air', 'AL', 'ZX', GETDATE(), 'Canada', 0, 0, 0, 0)

insert into Airlines(Id, Tenant, Prefix, ICAO, LimitedLength, AddedManually, ChampFSU, ChampFSRFSA, ChampFWB, ChampFHL, ChampFVRFVA, ChampNeedsRegistration, IsChampRegistered, GLSHKNeedsRegistration, IsGLSHKRegistered, ChampFFRFFA, GLSHKFWB, GLSHKFHL, GLSHKFSU, GLSHKFSRFSA, GLSHKFVRFVA, GLSHKFFRFFA, IsAllowedInAirlinesRestriction, ChampRegistrationRequested, GLSHKRegistrationRequested, HasAdaptations, CheckDigit)
values(@AirlineId, 0, '634' , 'GLR', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)