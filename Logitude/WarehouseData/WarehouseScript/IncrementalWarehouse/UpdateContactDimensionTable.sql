
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Contact' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Contacts )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(60)
   declare @LocalName as nvarchar(100)
   declare @Email as varchar(70)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit
   declare @Phone as varchar(25)

	DECLARE ContactsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Contacts.Id, dw_Contacts.EnglishName,dw_Contacts.LocalName , dw_Contacts.Email, dw_Contacts.BusinessPhone, dw_Contacts.Tenant, dw_DWHSettings.ParentTenant, dw_Contacts.AutomaticLastUpdateDate, dw_Contacts.InActive
	From dw_Contacts
	INNER JOIN dw_DWHSettings ON dw_Contacts.Tenant = dw_DWHSettings.Tenant
	where dw_Contacts.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @Id , @Name, @LocalName , @Email ,@Phone ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from Dim_Contacts where Id = @Id)
	if(@Key is  null) begin  insert into Dim_Contacts (Id,Name,[Local Name],Email, Phone ,[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@Name,@LocalName ,@Email ,@Phone , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive); end
	else begin update   Dim_Contacts set Name =@Name,  [Local Name] =@LocalName ,  Email = @Email , Phone = @Phone,  [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate ,[InActive] = @InActive Where Id = @Id; end



	FETCH NEXT FROM ContactsCursor  INTO @Id , @Name, @LocalName , @Email , @Phone , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE ContactsCursor
	DEALLOCATE ContactsCursor
	
	update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Contact'
End