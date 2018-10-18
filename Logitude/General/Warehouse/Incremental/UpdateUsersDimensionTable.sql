
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'User' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Users )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(60)
   declare @LocalName as nvarchar(100)
   declare @Email as varchar(70)
   declare @Department varchar(40) 
   declare @Branch varchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE UsersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Users.Id, dw_Contacts.EnglishName,dw_Contacts.LocalName , dw_Contacts.Email, dw_Departments.EnglishName , dw_Branches.EnglishName ,  dw_Users.Tenant, dw_Users.Tenant
	From dw_Users
	INNER JOIN dw_Branches ON dw_Users.BranchId = dw_Branches.Id
	INNER JOIN dw_Departments ON dw_Users.DepartmentId = dw_Departments.Id
	INNER JOIN dw_Contacts ON dw_Users.Id = dw_Contacts.Id
	where dw_Contacts.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from Dim_Users where Id = @Id)
	if(@Key is  null) begin  insert into Dim_Users values(@Id,@Name,@LocalName ,@Email,@Department, @Branch, @SourceTenant , @ParentTenant ); end
	else begin update   Dim_Users set Name =@Name,  LocalName =@LocalName ,  Email = @Email , Department = @Department,  Branch = @Branch,  SourceTenant = @SourceTenant , ParentTenant = @ParentTenant  Where Id = @Id; end



	FETCH NEXT FROM UsersCursor  INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant  
		End
	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	
	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'User'
End