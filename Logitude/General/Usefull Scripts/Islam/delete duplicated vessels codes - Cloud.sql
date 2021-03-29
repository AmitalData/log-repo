BEGIN;
declare @Tenant as int
declare @Id as varchar(15)
declare @Code as varchar(5)

	DECLARE VesselsCursor CURSOR READ_ONLY
	FOR
	SELECT Code,Tenant
	From Vessels
	group by Code,Tenant having count(*) > 1
    OPEN VesselsCursor FETCH NEXT FROM VesselsCursor INTO @Code,@Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
		begin
			set @Id = (select top(1) Id from Vessels  where Code = @Code and Tenant = @Tenant order by Id)
			delete from Vessels where Code = @Code and Tenant = @Tenant and Id <> @Id
		end
	FETCH NEXT FROM VesselsCursor INTO @Code,@Tenant
	END
	CLOSE VesselsCursor
	DEALLOCATE VesselsCursor
END