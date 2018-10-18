CREATE TABLE [Customs].[CargoIdentityQualifiers](
	[Code] [varchar](4) NOT NULL,
	[EnglishName] [varchar](100) NULL,
	[LocalName] [nvarchar](100) NULL,
	[SearchFields] [nvarchar](1000) NULL,
	[InActive] [bit] NOT NULL,
 CONSTRAINT [PK_Customs.CargoIdentityQualifiers] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
