
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
	where dw_Contacts.Id !='-1'
	OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @Id , @Name, @LocalName , @Email ,@Phone ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_ContactsTemp (Id,Name,[Local Name],Email, Phone ,[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@Name,@LocalName ,@Email ,@Phone , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM ContactsCursor  INTO @Id , @Name, @LocalName , @Email , @Phone, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE ContactsCursor
	DEALLOCATE ContactsCursor
