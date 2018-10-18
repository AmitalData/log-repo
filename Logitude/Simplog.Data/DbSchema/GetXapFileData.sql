

create PROCEDURE [dbo].[usp_GetXapFileData]
(
    @pFileName   varchar(60) ,
    @pFileData    varbinary(MAX) OUTPUT,
    @pIsStaging    bit  
)

  AS

 BEGIN;

  Set @pFileData	= (SELECT  FileData
          FROM XapFileDatas with (UPDLOCK) WHERE  [FileName] = @pFileName and  IsStaging = @pIsStaging )

  --Set @pIsStaging	= (SELECT  IsStaging
  --        FROM XapFileDatas with (UPDLOCK) WHERE  [FileName] = @pFileName and IsStaging = @pIsStaging )

  End;