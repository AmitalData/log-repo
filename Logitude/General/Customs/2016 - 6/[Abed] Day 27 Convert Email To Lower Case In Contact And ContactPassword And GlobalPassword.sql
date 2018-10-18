

 --Global DB
    declare @GlobalEmail as varchar(70)
    declare @Id as varchar(15)
	DECLARE GlobalContactsCursor CURSOR READ_ONLY
	FOR
	SELECT Email,Id
	From GlobalContacts
	OPEN GlobalContactsCursor FETCH NEXT FROM GlobalContactsCursor INTO @GlobalEmail , @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	update GlobalContacts set Email = LOWER(@GlobalEmail) where Id = @Id
 
	FETCH NEXT FROM GlobalContactsCursor INTO  @GlobalEmail, @Id
		End
	CLOSE GlobalContactsCursor
	DEALLOCATE GlobalContactsCursor



	--Global DB
    declare @ContactPasswordsEmail as varchar(70)
	DECLARE ContactPasswordsCursor CURSOR READ_ONLY
	FOR
	SELECT Email
	From ContactPasswords
	OPEN ContactPasswordsCursor FETCH NEXT FROM ContactPasswordsCursor INTO @ContactPasswordsEmail
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    update ContactPasswords set Email = LOWER(@ContactPasswordsEmail) where Email = @ContactPasswordsEmail

	FETCH NEXT FROM ContactPasswordsCursor INTO  @ContactPasswordsEmail
    End
	CLOSE ContactPasswordsCursor
	DEALLOCATE ContactPasswordsCursor




   --Main DB
	declare @ContactsEmail as varchar(70)
    declare @Id as varchar(15)
	DECLARE ContactsCursor CURSOR READ_ONLY
	FOR
	SELECT Email,Id
	From Contacts
	OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @ContactsEmail , @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN

	update Contacts set Email =LOWER(@ContactsEmail)  where Id = @Id

	FETCH NEXT FROM ContactsCursor INTO  @ContactsEmail, @Id
		End
	CLOSE ContactsCursor
	DEALLOCATE ContactsCursor



