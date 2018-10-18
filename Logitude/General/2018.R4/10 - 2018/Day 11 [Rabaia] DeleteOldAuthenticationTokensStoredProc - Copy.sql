
 

GO
/****** Object:  StoredProcedure [dbo].[DeleteOldAuthenticationTokens]    Script Date: 2018-10-11 9:34:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[DeleteOldAuthenticationTokens]
as 

begin

delete from [dbo].[AuthenticationTokens] where [CreateDate] < GETDATE() - 60
 
end