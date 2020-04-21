if OBJECT_ID('[dbo].[QuoteAutomaticallyClosingDataHistory]', 'U') IS NULL
	CREATE TABLE QuoteAutomaticallyClosingDataHistory
	 (
	 Id int not null identity,
	 Tenant int not null,
	 StartDateTime datetime,
	 EndDateTime datetime, 
	 HasException bit default '0', 
	 ExceptionMessage NVARCHAR(max),
	 PRIMARY KEY (Id)
	 )
GO