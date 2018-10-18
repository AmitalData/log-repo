
begin transaction
begin

IF OBJECT_ID(N'[dbo].[Commodities]', 'U') IS NOT NULL
    DROP TABLE [dbo].Commodities;

-- Creating table 'Commodities'
CREATE TABLE [dbo].Commodities (
    [Id]varchar(15) NOT NULL,
	[Tenant]int NOT NULL,
	[Code]varchar(7) NOT NULL,
    [Name]nvarchar(250) NOT NULL,	    		
	[SearchFields]nvarchar(1000) NULL,	
);

-- Creating primary
ALTER TABLE [dbo].[Commodities]
ADD CONSTRAINT [PK_Commodities]
    PRIMARY KEY CLUSTERED ([Id] ASC);

END
commit transaction