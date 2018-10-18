Create PROCEDURE GetNextGlobalTenantId
(
  @pLastNumber INT OUTPUT
  
)
AS
BEGIN



 Declare @Current As Int   



 
 Set @Current	= (SELECT  LastNumber
         FROM GlobalTenantCounters  WHERE Id=1)

		 Set @Current = @Current + 1

		 Update GlobalTenantCounters
		 set LastNumber = LastNumber + 1
		 Where Id=1
	
 End;

 Set @pLastNumber = @Current
   

