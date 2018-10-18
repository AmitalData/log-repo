Create PROCEDURE [dbo].[usp_UpdateXapFileData] 
(
  @pFileName   varchar(60),
  @pFileData    varbinary(MAX),
  @pIsStaging    bit
)
as
    IF NOT EXISTS (SELECT [FileName] 
         FROM XapFileDatas with (UPDLOCK) WHERE [FileName] =  @pFileName  and IsStaging = @pIsStaging )
		 
BEGIN;  


	 
	INSERT INTO XapFileDatas
   (
       Id,
	  [FileName],
	   FileData,
	   IsStaging
	  
   )

   VALUES
   (

      NewID(),
      @pFileName ,
      @pFileData ,
	  @pIsStaging 
	   
   )

  End;

  else

  BEGIN;  


 
		 Update XapFileDatas
		
		 set FileData = @pFileData,
		     IsStaging = @pIsStaging
	
		 Where [FileName] = @pFileName
	

	
 End;