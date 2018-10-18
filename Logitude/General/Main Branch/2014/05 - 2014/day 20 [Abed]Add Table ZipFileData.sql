

begin transaction
begin

IF OBJECT_ID(N'[dbo].[XapFileDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XapFileDatas];


CREATE TABLE [dbo].[XapFileDatas] (
 [Id] varchar(40)  NOT NULL,
 [FileName] varchar(60)  NOT NULL,
 [FileData] varbinary(Max)  NOT NULL,
 [IsStaging] bit  NOT NULL
	
     primary key ([Id])
);


END
commit transaction