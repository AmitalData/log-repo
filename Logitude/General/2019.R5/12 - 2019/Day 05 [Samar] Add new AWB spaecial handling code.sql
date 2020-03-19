

declare @Id as varchar(15)

if not exists (select * from AWBSpecialHandlingCodes where Code = 'ECP')
begin
                     
	EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBSpecialHandlingCode'
                     
	insert into AWBSpecialHandlingCodes (Code, Name, SearchFields, IsIATA, AirlineId, Id, InActive)
	values ('ECP'
			, 'A paper AWB needs to be printed to comply to any applicable legislation'
			, 'ECP,A paper AWB needs to be printed to comply to any applicable legislation'
			, 1
			, NULL
			, @Id
			, 0)

end 