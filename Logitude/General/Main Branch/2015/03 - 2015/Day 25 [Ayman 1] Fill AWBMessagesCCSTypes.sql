
-- Run On Global

if not exists (select * from AWBMessagesCCSTypes where Code = 'CHAMP')
begin
	insert into AWBMessagesCCSTypes(Code, Name, SearchFields)
	values ('CHAMP', 'Champ', 'CHAMP,Champ')
end

if not exists (select * from AWBMessagesCCSTypes where Code = 'GLSHK')
begin
	insert into AWBMessagesCCSTypes(Code, Name, SearchFields)
	values ('GLSHK', 'GLSHK', 'GLSHK,GLSHK')
end

update TenantManagements set AWBMessagesCCSTypeCode = 'CHAMP' where AWBMessagesCCSTypeCode is null
go