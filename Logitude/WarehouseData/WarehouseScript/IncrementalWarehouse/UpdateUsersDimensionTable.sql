
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'User' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Users )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

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
   declare @AutomaticLastUpdateDate as datetime

	DECLARE UsersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Users.Id, dw_Contacts.EnglishName,dw_Contacts.LocalName , dw_Contacts.Email, dw_Departments.EnglishName , dw_Branches.EnglishName ,  dw_Users.Tenant, dw_DWHSettings.ParentTenant, dw_Users.AutomaticLastUpdateDate
	From dw_Users
	INNER JOIN dw_Branches ON dw_Users.BranchId = dw_Branches.Id
	INNER JOIN dw_Departments ON dw_Users.DepartmentId = dw_Departments.Id
	INNER JOIN dw_Contacts ON dw_Users.Id = dw_Contacts.Id
	INNER JOIN dw_DWHSettings ON dw_Users.Tenant = dw_DWHSettings.Tenant
	where dw_Users.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from Dim_Users where Id = @Id)
	if(@Key is  null) begin  insert into Dim_Users (Id,Name,[Local Name],Email, Department ,Branch,  [Source Tenant],[Parent Tenant],[Automatic Last Update Date]) values(@Id,@Name,@LocalName ,@Email,@Department, @Branch, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate); end
	else begin update   Dim_Users set Name =@Name,  [Local Name] =@LocalName ,  Email = @Email , Department = @Department,  Branch = @Branch,  [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate  Where Id = @Id; end



	FETCH NEXT FROM UsersCursor  INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate 
		End
	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	
	update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'User'
End