--Use [Logitude2-4_Global]

--create table [dbo].[ContactPasswords] (
--    [Email] [varchar](70) not null,
--    [Password]  [varchar](40) not null,
--    [MustChangePassword] [bit] not null,
--	[IsLocked] [bit] not null,
--    [NumberOfRetries] [int] not null,
--    primary key ([Email])
--);
--go
--**********************************************************************************************


declare @GlobalTenantId as int
declare @Email as varchar(70)
declare @Password as varchar(70)
BEGIN
	DECLARE ContactsCursor CURSOR READ_ONLY
	FOR
	SELECT GlobalTenantId,Email,[Password]
	FROM  GlobalContacts
	where InActive = 0 and [Password] IS NOT NULL and Email IS NOT NULL
	OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO  @GlobalTenantId,@Email,@Password
	WHILE @@FETCH_STATUS = 0

		BEGIN
			if(Not Exists (select * from ContactPasswords where Email = @Email))
			Begin
			print(@Email)
			INSERT INTO ContactPasswords(Email,[Password],MustChangePassword,IsLocked,NumberOfRetries)
	        VALUES (@Email,@Password,0,0,0)

			End
		


	FETCH NEXT FROM ContactsCursor INTO @GlobalTenantId,@Email,@Password
	END
	CLOSE ContactsCursor
	DEALLOCATE ContactsCursor	
END



-- Creating table 'PasswordResetRequests'
 
CREATE TABLE [dbo].[PasswordResetRequests] (
    [RequestNumber]varchar(40)   NOT NULL,
    [Email]varchar(70)   NULL,
    [IsDone]bit   NOT NULL,
	 primary key ([RequestNumber])
);
GO

-- Run this script after copy data to the new table

--alter table globalcontacts drop column [Password]

--******************************************************************************************************************

-- Run this script after copy data to the new table
alter table contacts drop column [Password], [IsLocked], [MustChangePassword], [NumberOfRetries]
drop table [dbo].[PasswordResetRequests]




--*******************************************************************************************************************


