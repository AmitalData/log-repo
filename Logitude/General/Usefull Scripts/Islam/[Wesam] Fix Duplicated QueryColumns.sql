declare @Tenant as int
declare @UserId as varchar(15)
declare @ObjectFieldcode as varchar(200)
declare @Querycode as varchar(200)
declare @Id as varchar(15)
DECLARE QueryColumns CURSOR READ_ONLY
FOR
	select Tenant,UserId,ObjectFieldCode,QueryCode from QueryColumns 
	group by Tenant,userid,ObjectFieldcode,Querycode 
	having count(*) > 1
OPEN QueryColumns FETCH NEXT FROM QueryColumns INTO @Tenant,@UserId,@ObjectFieldCode,@QueryCode
WHILE @@FETCH_STATUS = 0
BEGIN
	
	set @Id = (select top(1)id from QueryColumns as QC1 where QC1.Tenant = @Tenant and QC1.UserId = @UserId and QC1.ObjectFieldCode = @ObjectFieldcode and QC1.QueryCode = @Querycode)
	delete from QueryColumns where Id <> @Id and Tenant = @Tenant and UserId = @UserId and ObjectFieldCode = @ObjectFieldCode and QueryCode = @QueryCode
FETCH NEXT FROM QueryColumns INTO @Tenant,@UserId,@ObjectFieldCode,@QueryCode
END
CLOSE QueryColumns
DEALLOCATE QueryColumns