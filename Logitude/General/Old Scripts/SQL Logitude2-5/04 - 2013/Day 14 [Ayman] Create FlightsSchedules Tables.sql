
-- Run on SystemLogs db

IF OBJECT_ID(N'[dbo].[FlightsSchedulesRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlightsSchedulesRequests];
GO

IF OBJECT_ID(N'[dbo].[FlightsSchedulesAnswers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlightsSchedulesAnswers];
GO

-- Creating table 'FlightsSchedulesRequests'
CREATE TABLE [dbo].[FlightsSchedulesRequests] (
    [Id]varchar(15)   NOT NULL,
    [FromPortCode]varchar(3) NOT NULL,
    [ToPortCode]varchar(3) NOT NULL,
    [AirlineCode]varchar(2) NOT NULL,
    [FlightNumber]varchar(15) NULL,
    [ETD]datetime NOT NULL,
    [ETA]datetime NULL,
    [HasResponse]bit NOT NULL,
);
GO

-- Creating table 'FlightsSchedulesAnswers'
CREATE TABLE [dbo].[FlightsSchedulesAnswers] (
    [Id]varchar(15)   NOT NULL,
    [RequestId]varchar(15) NOT NULL,
    [FlightNumber]varchar(15) NOT NULL,
    [AircraftTypeCode]varchar(5) NOT NULL,
	[NumberOfStops]int NOT NULL,	
    [ETD]datetime NOT NULL,
    [ETA]datetime NOT NULL
);
GO

-- Creating primary key on [Id] in table 'FlightsSchedulesRequests'
ALTER TABLE [dbo].[FlightsSchedulesRequests]
ADD CONSTRAINT [PK_FlightsSchedulesRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FlightsSchedulesAnswers'
ALTER TABLE [dbo].[FlightsSchedulesAnswers]
ADD CONSTRAINT [PK_FlightsSchedulesAnswers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [RequestId] in table 'FlightsSchedulesAnswers'
ALTER TABLE [dbo].[FlightsSchedulesAnswers]
ADD CONSTRAINT [FK_FlightsSchedulesAnswerFlightsSchedulesRequest]
    FOREIGN KEY ([RequestId])
    REFERENCES [dbo].[FlightsSchedulesRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FlightsSchedulesAnswerFlightsSchedulesRequest'
CREATE INDEX [IX_FK_FlightsSchedulesAnswerFlightsSchedulesRequest]
ON [dbo].[FlightsSchedulesAnswers]
    ([RequestId]);
GO