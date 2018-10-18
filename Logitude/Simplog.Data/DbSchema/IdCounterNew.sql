Alter PROCEDURE [dbo].[usp_GetNextTableIdsRange]
(
  @pEndNumber   int OUTPUT,
  @pStartNumber int OutPUT,
  @DBStringNumber varchar(50) OutPUT,
  @pTableName    varchar(40),
  @pNumberOfIds int
  
)
AS

DECLARE @COUNTERINIT AS INT
   SET @COUNTERINIT = @pNumberOfIds
   SET @pStartNumber = 1
 
 
 Declare @Current As Int 
 Declare @DataBaseNumber As Int 
 --Declare @DBStringNumber As varchar(50)
 Declare @StartStrNumber As varchar(50)
 Declare @EndStrNumber As varchar(50)
 
 SELECT TOP 1 @DataBaseNumber = DataBaseNumber FROM DataBaseProperties 

Set @DBStringNumber = CONVERT(varchar(50) , @DataBaseNumber) 

    IF NOT EXISTS (SELECT TableName 
         FROM DBIdCounters (UPDLOCK) WHERE TableName =@pTableName)
		 
BEGIN; 
 

  INSERT INTO DBIdCounters
   (
       TableName,
	   
	   LastIdNumber
   )
   VALUES
   (
      @pTableName,
       
      @COUNTERINIT 
     )
     
     Set @Current = 1

	 End

    

   Else   
   
 BEGIN;

 
 Set @Current	= (SELECT  LastIdNumber
         FROM DBIdCounters with (UPDLOCK,ROWLOCK) WHERE  TableName =@pTableName)
		 
		 set @pEndNumber = @Current + @pNumberOfIds
		 --Set @Current = @Current + 1
		 set @pStartNumber =  @Current + 1
		
		
		 Update DBIdCounters
		 set LastIdNumber = LastIdNumber + @pNumberOfIds
		 Where TableName = @pTableName
	
 End;

 --Set  @CurrentStrNumber = CONVERT(varchar(50) ,@Current)

 
 --Set  @StartStrNumber = CONVERT(varchar(50) ,@pStartNumber)
-- Set  @EndStrNumber = CONVERT(varchar(50) ,@pEndNumber)

  --Set @pStartNumber = @DBStringNumber --+ '-'+ @StartStrNumber
  --Set @pEndNumber = @DBStringNumber-- + '-'+ @EndStrNumber


-- Set @pLastNumber = @DBStringNumber + '-'+ @CurrentStrNumber
   







